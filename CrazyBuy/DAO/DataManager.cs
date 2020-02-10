namespace CrazyBuy.DAO
{
    public class DataManager
    {
        public static readonly MemberDAO memberDao = new MemberDAO();
        public static readonly TenantDAO tenantDao = new TenantDAO();
        public static readonly TenantMemberDAO tenantMemberDao = new TenantMemberDAO();
        public static readonly TenantPrdDAO tenantPrdDao = new TenantPrdDAO();
    }
}
