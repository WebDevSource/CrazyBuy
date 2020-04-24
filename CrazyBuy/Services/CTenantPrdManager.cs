using System;
using System.Collections.Generic;
using System.Diagnostics;
using CrazyBuy.DAO;
using CrazyBuy.Models;

namespace CrazyBuy.Services
{
    public class CTenantPrdManager
    {
        public static IEnumerable<Dictionary<string, object>> getPrdSearchListByCat(PrdSearchQuery query, int userId)
        {
            List<TenantPrd> data = DataManager.tenantPrdDao.getSearchTenandPrdByCatId(query, userId);
            IEnumerable<Dictionary<string, object>> result = getPrdList(data, query.userType, userId);
            return result;
        }

        public static IEnumerable<Dictionary<string, object>> getPrdListByCat(PrdPageQuery query, int userId)
        {
            List<TenantPrd> data = DataManager.tenantPrdDao.getTenandPrdByCatId(query, userId);
            IEnumerable<Dictionary<string, object>> result = getPrdList(data, query.userType, userId);
            return result;
        }

        public static IEnumerable<Dictionary<string, object>> getPrdList(List<TenantPrd> prds, string userType, int userId)
        {
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            string type = "團媽";
            if (prds.Count > 0)
            {
                TenantGrade tenantGrade = DataManager.tenantDao.GetTenantGrade(prds[0].tenantId);
                if (tenantGrade != null)
                {
                    type = tenantGrade.tenantGrade;
                }
            }

            foreach (TenantPrd prd in prds)
            {
                result.Add(getPrdItem(prd, userType, userId, type));
            }
            return result;
        }

        //顯示獨立價格
        public static PrdPrice getPrdPrice(TenantPrd prd, string userType, int userId)
        {
            PrdPrice prdPrice = new PrdPrice();
            string type = UserType.GUEST;
            int price = int.MaxValue;
            int custPriceGradeId = 0;
            string priceGradeType = "";
            TenantGrade tenantGrade = DataManager.tenantDao.GetTenantGrade(prd.tenantId);
            string tenantGradeType = "團媽";
            if (tenantGrade != null)
            {
                tenantGradeType = tenantGrade.tenantGrade;
            }
            List<PrdPrice> prices = getPrdPrices(prd, userType, userId, tenantGradeType);
            foreach (PrdPrice itemPrice in prices)
            {
                if (itemPrice.price <= price)
                {
                    price = itemPrice.price;
                    type = itemPrice.type;
                    custPriceGradeId = itemPrice.custPriceGradeId;
                    priceGradeType = itemPrice.priceGradeType;
                }
            }
            prdPrice.price = price;
            prdPrice.type = type;
            prdPrice.priceGradeType = priceGradeType;
            prdPrice.custPriceGradeId = custPriceGradeId;

            return prdPrice;
        }

        //畫面呈現價格
        public static List<PrdPrice> getPrdPrices(TenantPrd prd, string userType, int userId, string tenantGrade)
        {
            List<PrdPrice> prices = new List<PrdPrice>();
            switch (userType)
            {
                case LoginType.LOGIN_USER:
                    TenantMember tenantMember = DataManager.tenantMemberDao.getTenantMemberByMemberId(userId);
                    PrdPrice prdPriceUser = new PrdPrice();
                    prdPriceUser.price = prd.fixedprice == null ? 0 : (int)prd.fixedprice;
                    prdPriceUser.type = CHType.PRICE_NORMAL;
                    prdPriceUser.priceGradeType = "";
                    prdPriceUser.custPriceGradeId = 0;
                    prices.Add(prdPriceUser);

                    prdPriceUser = new PrdPrice();
                    prdPriceUser.price = prd.memberPrice == null ? 0 : (int)prd.memberPrice;
                    prdPriceUser.type = CHType.PRICE_MEMBER;
                    prdPriceUser.priceGradeType = "";
                    prdPriceUser.custPriceGradeId = 0;
                    prices.Add(prdPriceUser);

                    if (UserGradeType.TRANS.Equals(tenantMember.gradeType))
                    {
                        prdPriceUser = new PrdPrice();
                        prdPriceUser.price = prd.transferPrice == null ? 0 : (int)prd.transferPrice;
                        prdPriceUser.type = CHType.PRICE_NTRANS;
                        prdPriceUser.priceGradeType = "轉批價";
                        prdPriceUser.custPriceGradeId = 0;
                        prices.Add(prdPriceUser);
                    }
                    break;
                case UserType.ADMIN:
                    PrdPrice prdPriceAdmin = new PrdPrice();
                    prdPriceAdmin.price = prd.fixedprice == null ? 0 : (int)prd.fixedprice;
                    prdPriceAdmin.type = CHType.PRICE_NORMAL;
                    prdPriceAdmin.priceGradeType = "";
                    prdPriceAdmin.custPriceGradeId = 0;
                    prices.Add(prdPriceAdmin);

                    prdPriceAdmin = new PrdPrice();
                    prdPriceAdmin.price = prd.memberPrice == null ? 0 : (int)prd.memberPrice;
                    prdPriceAdmin.type = CHType.PRICE_MEMBER;
                    prdPriceAdmin.priceGradeType = "";
                    prdPriceAdmin.custPriceGradeId = 0;
                    prices.Add(prdPriceAdmin);
                    Debug.WriteLine("[CMemberManager-addMember] error:" + tenantGrade);
                    if (tenantGrade != "轉批媽" && tenantGrade != "批發商")
                    {
                        break;
                    }
                    prdPriceAdmin = new PrdPrice();
                    prdPriceAdmin.price = prd.transferPrice == null ? 0 : (int)prd.transferPrice;
                    prdPriceAdmin.type = CHType.PRICE_NTRANS;
                    prdPriceAdmin.priceGradeType = "轉批價";
                    prdPriceAdmin.custPriceGradeId = 0;
                    prices.Add(prdPriceAdmin);
                    break;
                default:
                    if (userType.StartsWith(UserType.SPC_MEMBER))
                    {
                        string custGrade = userType.Split(":")[1];
                        PrdPrice prdPriceSPCMember = new PrdPrice();
                        prdPriceSPCMember.price = prd.fixedprice == null ? 0 : (int)prd.fixedprice;
                        prdPriceSPCMember.type = CHType.PRICE_NORMAL;
                        prdPriceSPCMember.priceGradeType = "";
                        prdPriceSPCMember.custPriceGradeId = 0;
                        prices.Add(prdPriceSPCMember);

                        prdPriceSPCMember = new PrdPrice();
                        prdPriceSPCMember.price = prd.memberPrice == null ? 0 : (int)prd.memberPrice;
                        prdPriceSPCMember.type = CHType.PRICE_MEMBER;
                        prdPriceSPCMember.priceGradeType = "";
                        prdPriceSPCMember.custPriceGradeId = 0;
                        prices.Add(prdPriceSPCMember);

                        CustSpcPrice spc_price = DataManager.tenantPrdDao.getSpcTenantPrdPrice(prd.tenantId, prd.id, int.Parse(custGrade));
                        if (spc_price != null)
                        {
                            prdPriceSPCMember = new PrdPrice();
                            prdPriceSPCMember.price = spc_price.price;
                            prdPriceSPCMember.type = spc_price.name;
                            prdPriceSPCMember.priceGradeType = "自訂價";
                            prdPriceSPCMember.custPriceGradeId = spc_price.id;
                            prices.Add(prdPriceSPCMember);
                        }
                    }
                    break;
            }
            return prices;
        }

        public static int getTenantPrdCount(Guid tenantId, long catId, int memberId)
        {
            int count = DataManager.tenantPrdDao.getCountByCatId(tenantId, catId, memberId);
            return count;
        }

        public static bool isUserAdvanced(int memberId)
        {
            bool isV = false;
            TenantMember tenantMember = DataManager.tenantMemberDao.getTenantMemberByMemberId(memberId);
            if (tenantMember != null)
            {
                isV = tenantMember.levelId != null;
            }
            return isV;
        }

        public static Dictionary<string, object> getPrdItem(TenantPrd prd, string userType, int userId, string type)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            Dictionary<string, string> prices = new Dictionary<string, string>();
            string price;

            List<PrdPrice> prdPrices = getPrdPrices(prd, userType, userId, type);
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
            data.Add("isOpenOrder", prd.isOpenOrder);
            data.Add("isTakeOff", DateTime.Now > prd.dtSellEnd);
            data.Add("takeOffMessage", prd.takeOffMessage);
            return data;
        }

    }
}
