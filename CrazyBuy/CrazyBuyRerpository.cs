using CrazyBuy.Common;

namespace CrazyBuy
{
    public class CrazyBuyRerpository
    {
        public CrazyBuyDbContext ContextInit()
        {
            //通常連線字串會放在config中
            return new CrazyBuyDbContext(Utils.GetConfiguration("DataBase"));
        }        
    }
}
