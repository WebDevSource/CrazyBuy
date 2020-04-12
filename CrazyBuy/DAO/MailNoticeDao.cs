using CrazyBuy.Models;

namespace CrazyBuy.DAO
{
    public class MailNoticeDao : CrazyBuyRerpository
    {
        public void add(MailNotice mailNotice)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                dbContext.MailNotice.Add(mailNotice);
                dbContext.SaveChanges();
            }
        }
    }
}
