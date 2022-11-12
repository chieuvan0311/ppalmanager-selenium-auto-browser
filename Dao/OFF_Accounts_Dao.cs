using PAYPAL.DataConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAYPAL.Dao
{
    public class OFF_Accounts_Dao
    {
        PaypalDbContext db = null;
        public OFF_Accounts_Dao()
        {
            db = new PaypalDbContext();
        }

        public List<Account> Get_All_OFF_Accounts() 
        {
            return db.Accounts.Where(x => x.Acc_ON_OFF != true).ToList();
        }
    }
}
