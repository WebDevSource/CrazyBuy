namespace CrazyBuy.DAO
{
    public class DataManager
    {
        public static readonly MemberDAO memberDao = new MemberDAO();
        public static readonly TenantDAO tenantDao = new TenantDAO();
        public static readonly TenantMemberDAO tenantMemberDao = new TenantMemberDAO();
        public static readonly TenantPrdDAO tenantPrdDao = new TenantPrdDAO();
        public static readonly TenantPrdCatDAO tenantPrdCatDao = new TenantPrdCatDAO();
        public static readonly ShopCartDAO shopCartDao = new ShopCartDAO();
        public static readonly OrderDAO orderDao = new OrderDAO();
        public static readonly TenantBulletinDAO tenantBulletinDao = new TenantBulletinDAO();
        public static readonly TenantFAQDAO tenantFAQDao = new TenantFAQDAO();
        public static readonly OrderContactItemDAO orderContactItemDAO = new OrderContactItemDAO();
        public static readonly CityDao cityDao = new CityDao();
        public static readonly OrderAmountHistoryDao orderAmountHistoryDao = new OrderAmountHistoryDao();
        public static readonly MailNoticeDao mailNoticeDao = new MailNoticeDao();
        public static readonly TenantBillDAO tenantBillDAO = new TenantBillDAO();
    }
}
