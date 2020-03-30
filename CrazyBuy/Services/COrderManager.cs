using CrazyBuy.Common;
using CrazyBuy.DAO;
using CrazyBuy.Models;
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
            data.detail = DataManager.orderDao.getDetailLists(orderId);
            data.contactList = DataManager.orderContactItemDAO.getListByOrderId(orderId);
            return data;
        }

        public static bool isProductEnough(int userId)
        {
            List<ShopCartPrd> shopCartPrds = DataManager.shopCartDao.getItemsByMember(userId);
            foreach (ShopCartPrd item in shopCartPrds)
            {
                TenantPrd prdItem = DataManager.tenantPrdDao.getTenandPrd(item.productId);
                prdItem.stockNum = prdItem.stockNum - item.count;
                if (prdItem.stockNum < 0)
                {
                    return false;
                }
            }
            return true;
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

                //結算購物車價錢
                foreach (ShopCartPrd item in shopCartPrds)
                {
                    OrderDetail detail = new OrderDetail();
                    detail.prdId = item.productId;
                    detail.qty = item.count;
                    detail.unitPrice = item.amount / item.count;
                    detail.amount = item.amount;
                    detail.status = "正常";
                    detail.createTime = now;
                    detail.updateTime = now;
                    total += item.amount;
                    detailList.Add(detail);
                }

                //檢查是否數量足夠
                foreach (OrderDetail item in detailList)
                {
                    TenantPrd prdItem = DataManager.tenantPrdDao.getTenandPrd(item.prdId);
                    prdItem.stockNum = prdItem.stockNum - item.qty;
                    if (prdItem.stockNum < 0)
                    {
                        rm.code = MessageCode.PRD_NOT_ENOUGHT;
                        return rm;
                    }

                    if (!prdMap.ContainsKey(prdItem.id))
                    {
                        prdMap.Add(prdItem.id, prdItem);
                    }
                }

                //開始寫入價格
                if (total != 0)
                {
                    orderMaster.orderAmount = total;
                    orderMaster.totalAmount = total;
                    orderMaster.shippingAmount = total;
                    orderMaster.payStatus = "等待貨款";
                    orderMaster.shippingStatus = "未出貨";
                    orderMaster.status = "新訂單";

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
    }
}
