using System;
namespace CrazyBuy.Models
{
    public class Member
    {
        public Guid id { get; set; }
        public Guid tenantId { get; set; }
        public string userId { get; set; }
        public string userPassword { get; set; }
        public string userName { get; set; }
        public string mobilePhone { get; set; }
        public string FAX { get; set; }
        public string mail { get; set; }
        public string lineID { get; set; }
        public string Address { get; set; }
        public int userGender { get; set; }
        public DateTime userBirthday { get; set; }
        public string userPortraitUrl { get; set; }
        public string MemberType { get; set; }
        public string ShopName { get; set; }
        public string Note { get; set; }
        public string PriceGrade { get; set; }
        public string status { get; set; }
        public string LastestLoginTime { get; set; }
        public string LastestOrderTime { get; set; }
    }
}
