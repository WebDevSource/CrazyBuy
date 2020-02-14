using CrazyBuy.Common;
using CrazyBuy.DAO;
using CrazyBuy.Models;
using System;
using System.Collections.Generic;

namespace CrazyBuy.Services
{
    public class COrderManager
    {
        public static OrderData getOrderData(int orderId)
        {
            OrderData data = new OrderData();
            data.master = DataManager.orderDao.getOrder(orderId);
            data.detail = DataManager.orderDao.getDetailLists(orderId);
            return data;
        }

        public static int addOrder(OrderMaster orderMaster, UserInfo userInfo)
        {
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
                foreach (ShopCartPrd item in shopCartPrds)
                {
                    OrderDetail detail = new OrderDetail();
                    detail.prdId = item.productId;
                    detail.qty = item.count;
                    detail.unitPrice = item.amount / item.count;
                    detail.amount = item.amount;
                    detail.status = "0";
                    detail.createTime = now;
                    detail.updateTime = now;
                    total += item.amount;
                    detailList.Add(detail);
                }

                if (total != 0)
                {
                    orderMaster.orderAmount = total;
                    orderResult = DataManager.orderDao.addOrderMaster(orderMaster);
                    foreach (OrderDetail item in detailList)
                    {
                        item.orderId = orderResult;
                        DataManager.orderDao.addOrderDetail(item);
                    }
                    DataManager.shopCartDao.removeItemsByMember(userInfo.memberId);
                }
            }
            catch (Exception e)
            {
                MDebugLog.error("[COrderManager-addOrder] error:" + e.Message);
            }
            return orderResult;
        }
    }
}
