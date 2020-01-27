namespace CrazyBuy.DAO
{
    public class DataManager
    {
        public static readonly MemberDAO memberDao = new MemberDAO();
        public static readonly TenantDAO tenantDao = new TenantDAO();
    }
}
