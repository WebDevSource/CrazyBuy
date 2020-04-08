﻿using System;
using System.Collections.Generic;
using CrazyBuy.DAO;
using CrazyBuy.Models;

namespace CrazyBuy.Services
{
    public class CTenantPrdManager
    {
        public static IEnumerable<Dictionary<string, object>> getPrdSearchListByCat(PrdSearchQuery query, int userId)
        {
            List<TenantPrd> data = DataManager.tenantPrdDao.getSearchTenandPrdByCatId(query);
            IEnumerable<Dictionary<string, object>> result = getPrdList(data, query.userType, userId);
            return result;
        }

        public static IEnumerable<Dictionary<string, object>> getPrdListByCat(PrdPageQuery query, int userId)
        {
            List<TenantPrd> data = DataManager.tenantPrdDao.getTenandPrdByCatId(query);
            IEnumerable<Dictionary<string, object>> result = getPrdList(data, query.userType, userId);
            return result;
        }

        public static IEnumerable<Dictionary<string, object>> getPrdList(List<TenantPrd> prds, string userType, int userId)
        {
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            foreach (TenantPrd prd in prds)
            {
                result.Add(getPrdItem(prd, userType, userId));
            }
            return result;
        }

        //顯示獨立價格
        public static PrdPrice getPrdPrice(TenantPrd prd, string userType, int userId)
        {
            PrdPrice prdPrice = new PrdPrice();
            string type = UserType.GUEST;
            int price = int.MaxValue;

            List<PrdPrice> prices = getPrdPrices(prd, userType, userId);
            foreach (PrdPrice itemPrice in prices)
            {
                if (itemPrice.price < price)
                {
                    price = itemPrice.price;
                    type = itemPrice.type;
                }
            }
            prdPrice.price = price;
            prdPrice.type = type;
            return prdPrice;
        }

        //畫面呈現價格
        public static List<PrdPrice> getPrdPrices(TenantPrd prd, string userType, int userId)
        {
            List<PrdPrice> prices = new List<PrdPrice>();
            switch (userType)
            {
                case LoginType.LOGIN_USER:
                    TenantMember tenantMember = DataManager.tenantMemberDao.getTenantMemberByMemberId(userId);
                    PrdPrice prdPriceUser = new PrdPrice();
                    prdPriceUser.price = prd.fixedprice == null ? 0 : (int)prd.fixedprice;
                    prdPriceUser.type = CHType.PRICE_NORMAL;
                    prices.Add(prdPriceUser);

                    prdPriceUser = new PrdPrice();
                    prdPriceUser.price = prd.memberPrice == null ? 0 : (int)prd.memberPrice;
                    prdPriceUser.type = CHType.PRICE_MEMBER;
                    prices.Add(prdPriceUser);

                    if (UserGradeType.TRANS.Equals(tenantMember.gradeType))
                    {
                        prdPriceUser = new PrdPrice();
                        prdPriceUser.price = prd.transferPrice == null ? 0 : (int)prd.transferPrice;
                        prdPriceUser.type = CHType.PRICE_NTRANS;
                        prices.Add(prdPriceUser);
                    }
                    break;
                case UserType.ADMIN:
                    PrdPrice prdPriceAdmin = new PrdPrice();
                    prdPriceAdmin.price = prd.fixedprice == null ? 0 : (int)prd.fixedprice;
                    prdPriceAdmin.type = CHType.PRICE_NORMAL;
                    prices.Add(prdPriceAdmin);

                    prdPriceAdmin = new PrdPrice();
                    prdPriceAdmin.price = prd.memberPrice == null ? 0 : (int)prd.memberPrice;
                    prdPriceAdmin.type = CHType.PRICE_MEMBER;
                    prices.Add(prdPriceAdmin);

                    prdPriceAdmin = new PrdPrice();
                    prdPriceAdmin.price = prd.transferPrice == null ? 0 : (int)prd.transferPrice;
                    prdPriceAdmin.type = CHType.PRICE_NTRANS;
                    prices.Add(prdPriceAdmin);
                    break;
                default:
                    if (userType.StartsWith(UserType.SPC_MEMBER))
                    {
                        string custGrade = userType.Split(":")[1];
                        PrdPrice prdPriceSPCMember = new PrdPrice();
                        prdPriceSPCMember.price = prd.fixedprice == null ? 0 : (int)prd.fixedprice;
                        prdPriceSPCMember.type = CHType.PRICE_NORMAL;
                        prices.Add(prdPriceSPCMember);

                        prdPriceSPCMember = new PrdPrice();
                        prdPriceSPCMember.price = prd.memberPrice == null ? 0 : (int)prd.memberPrice;
                        prdPriceSPCMember.type = CHType.PRICE_MEMBER;
                        prices.Add(prdPriceSPCMember);

                        CustSpcPrice spc_price = DataManager.tenantPrdDao.getSpcTenantPrdPrice(prd.tenantId, prd.id, int.Parse(custGrade));
                        if (spc_price != null)
                        {
                            prdPriceSPCMember = new PrdPrice();
                            prdPriceSPCMember.price = spc_price.price;
                            prdPriceSPCMember.type = spc_price.name;
                            prices.Add(prdPriceSPCMember);
                        }
                    }
                    break;
            }
            return prices;
        }

        public static int getTenantPrdCount(Guid tenantId, long catId)
        {
            int count = DataManager.tenantPrdDao.getCountByCatId(tenantId, catId);
            return count;
        }

        public static Dictionary<string, object> getPrdItem(TenantPrd prd, string userType, int userId)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            Dictionary<string, string> prices = new Dictionary<string, string>();
            string price;

            List<PrdPrice> prdPrices = getPrdPrices(prd, userType, userId);
            foreach (PrdPrice prdPrice in prdPrices)
            {
                price = string.Format("${0}", prdPrice.price);
                prices.Add(prdPrice.type, price);
            }

            data.Add("prices", prices);
            data.Add("id", prd.id);
            data.Add("name", prd.name);
            data.Add("prdCode", prd.prdCode);
            data.Add("tenantId", prd.tenantId);
            data.Add("summary", prd.summary);
            data.Add("desc", prd.desc);
            data.Add("prdImages", prd.prdImages);
            data.Add("paymentType", prd.paymentType);
            data.Add("shipType", prd.shipType);
            data.Add("tags", prd.SpecialRule);
            data.Add("sepc", prd.prdSepc);
            data.Add("zeroStock", prd.zeroStockMessage);
            data.Add("count", prd.stockNum);
            return data;
        }

    }
}
