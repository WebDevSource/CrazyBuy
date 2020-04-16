using CrazyBuy.Common;
using CrazyBuy.DAO;
using CrazyBuy.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CrazyBuy.Services
{
    public class COrderManager
    {
        public static OrderData getOrderData(int orderId)
        {
            OrderData data = new OrderData();
            data.master = DataManager.orderDao.getOrder(orderId);
            int recipientCityId = data.master.recipientCityId.GetValueOrDefault(0);
            int invoiceCityId =  data.master.invoiceCityId.GetValueOrDefault(0);
            data.master.recipientCityName = DataManager.cityDao.getCity(recipientCityId) == null ? "" : DataManager.cityDao.getCity(recipientCityId).cityName;
            data.master.recipientTownName = DataManager.cityDao.getTown(data.master.recipientTownId) == null? "" : DataManager.cityDao.getTown(data.master.recipientTownId).townName;
            data.master.invoiceCityName = DataManager.cityDao.getCity(invoiceCityId) == null ? "" : DataManager.cityDao.getCity(invoiceCityId).cityName;
            data.master.invoiceTownName = DataManager.cityDao.getTown(data.master.invoiceTownId) == null ? "" : DataManager.cityDao.getTown(data.master.invoiceTownId).townName;
            data.detail = DataManager.orderDao.getDetailLists(orderId);
            data.contactList = DataManager.orderContactItemDAO.getListByOrderId(orderId);
            return data;
        }

        public static ReturnMessage isProductEnough(int userId)
        {
            ReturnMessage rm = new ReturnMessage();
            List<ShopCartPrd> shopCartPrds = DataManager.shopCartDao.getItemsByMember(userId);
            bool isCheck = true;
            List<int> data = new List<int>();
            foreach (ShopCartPrd item in shopCartPrds)
            {
                TenantPrd prdItem = DataManager.tenantPrdDao.getTenandPrd(item.productId);
                if (prdItem.zeroStockMessage == null || prdItem.zeroStockMessage == "")
                {
                    continue;
                }

                TenantPrd tmpPrdItem = isPrdSPecEnought(prdItem, item.prdSepc, item.count);
                prdItem.stockNum = prdItem.stockNum - item.count;
                if (prdItem.stockNum < 0 || tmpPrdItem == null)
                {
                    data.Add(prdItem.id);
                    isCheck = false;
                }

            }
            rm.code = isCheck ? MessageCode.SUCCESS : MessageCode.ERROR;
            rm.data = data;
            return rm;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static ReturnMessage addOrder(OrderMaster orderMaster, UserInfo userInfo)
        {
            ReturnMessage rm = new ReturnMessage();
            int orderResult = MessageCode.ERROR;
            try
            {
                DateTime now = DateTime.Now;
                orderMaster.tenantId = userInfo.tnenatId;
                orderMaster.memberId = userInfo.memberId;
                orderMaster.dtOrder = now;
                orderMaster.dtInStock = now;
                orderMaster.createTime = now;
                orderMaster.updateTime = now;
                int total = 0;
                List<ShopCartPrd> shopCartPrds = DataManager.shopCartDao.getItemsByMember(userInfo.memberId);
                List<OrderDetail> detailList = new List<OrderDetail>();
                Dictionary<int, TenantPrd> prdMap = new Dictionary<int, TenantPrd>();
                TenantMemLevel tenantMemLevel = DataManager.tenantMemberDao.getMemberLevel(userInfo.memberId);

                //是否有高級會員折扣
                double dis = 1;
                int memberLevelId = MessageCode.ERROR;
                if (tenantMemLevel != null)
                {
                    memberLevelId = tenantMemLevel.id;
                    dis = tenantMemLevel.discount * 0.01;
                }

                //結算購物車價錢及規格數量
                foreach (ShopCartPrd item in shopCartPrds)
                {
                    TenantPrd prdItem = DataManager.tenantPrdDao.getTenandPrd(item.productId);
                    prdItem = isPrdSPecEnought(prdItem, item.prdSepc, item.count);
                    if (prdItem != null && prdItem.status == "上架")
                    {
                        OrderDetail detail = new OrderDetail();
                        detail.prdId = item.productId;
                        detail.qty = item.count;

                        detail.amount = item.amount;
                        if (item.SpecialRule == null || !item.SpecialRule.Contains(UserDisType.NO_DIS))
                        {
                            detail.amount = (int)(detail.amount * dis);
                        }

                        detail.unitPrice = detail.amount / detail.qty;
                        detail.status = "正常";
                        detail.createTime = now;
                        detail.updateTime = now;
                        detail.prdSpec = item.prdSepc;
                        total += detail.amount;
                        detailList.Add(detail);
                        if (!prdMap.ContainsKey(prdItem.id))
                        {
                            prdMap.Add(prdItem.id, prdItem);
                        }
                    }
                    else
                    {
                        rm.code = MessageCode.PRD_NOT_ENOUGHT;
                        return rm;
                    }
                }

                //檢查是否數量足夠
                foreach (OrderDetail item in detailList)
                {
                    TenantPrd prdItem = prdMap.GetValueOrDefault(item.prdId);
                    prdItem.stockNum = prdItem.stockNum - item.qty;
                    if (!string.IsNullOrEmpty(prdItem.zeroStockMessage))
                    {
                        if (prdItem.stockNum < 0)
                        {
                            rm.code = MessageCode.PRD_NOT_ENOUGHT;
                            return rm;
                        }
                        else if (prdItem.stockNum == 0)
                        {
                            prdItem.status = prdItem.zeroStockMessage;
                        }
                    }
                }

                //開始寫入價格
                if (total != 0)
                {
                    orderMaster.orderAmount = total;
                    if (orderMaster.isNeedInvoice)
                    {
                        total += Convert.ToInt32(total * 0.05);
                    }
                    total += orderMaster.shippingAmount;
                    orderMaster.totalAmount = total;
                    orderMaster.payStatus = "等待貨款";
                    orderMaster.shippingStatus = "未出貨";
                    orderMaster.status = "新訂單";
                    if (MessageCode.ERROR != memberLevelId)
                    {
                        orderMaster.levelId = memberLevelId;
                    }

                    OrderMaster master = DataManager.orderDao.addOrderMaster(orderMaster);

                    orderResult = master.id;
                    rm.data = master;
                    foreach (OrderDetail item in detailList)
                    {
                        TenantPrd prdItem = prdMap.GetValueOrDefault(item.prdId);
                        DataManager.tenantPrdDao.updateTenandPrd(prdItem);

                        item.orderId = master.id;
                        DataManager.orderDao.addOrderDetail(item);
                    }
                    DataManager.shopCartDao.removeItemsByMember(userInfo.memberId);

                    OrderAmountHistory histroy = new OrderAmountHistory();
                    histroy.orderId = master.id;
                    histroy.changeDesc = "購物完成";
                    histroy.changeAmount = total;
                    histroy.cumulativeAmount = 0;
                    histroy.status = "正常";
                    histroy.createTime = DateTime.Now;
                    histroy.creator = userInfo.memberId;
                    DataManager.orderAmountHistoryDao.add(histroy);
                }
            }
            catch (Exception e)
            {
                MDebugLog.error("[COrderManager-addOrder] error: " + e);
            }
            rm.code = orderResult;
            return rm;
        }

        public static TenantPrd isPrdSPecEnought(TenantPrd prd, string item, int buyCount)
        {
            if (string.IsNullOrEmpty(item) || string.IsNullOrEmpty(prd.prdSepc))
            {
                // 沒有商品規格不判斷
                return prd;
            }
            else if (prd.status != "上架")
            {
                return null;
            }

            dynamic itemSpec = JValue.Parse(item);
            string code = itemSpec.code;

            dynamic prdSpecs = JArray.Parse(prd.prdSepc);
            foreach (var prdSpec in prdSpecs)
            {
                string prdCode = prdSpec.code;
                if (prdCode.Equals(code))
                {
                    int num = int.Parse(Convert.ToString(prdSpec.num));
                    if (string.IsNullOrEmpty(prd.zeroStockMessage) || num - buyCount > -1)
                    {
                        prdSpec.num = Convert.ToString(num - buyCount);
                        prd.prdSepc = JsonConvert.SerializeObject(prdSpecs);
                        return prd;
                    }
                }
            }
            return null;
        }
    }
}
