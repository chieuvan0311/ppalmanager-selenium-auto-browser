using PAYPAL.DataConnection;
using PAYPAL.GPM_API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAYPAL.Controller
{
    public static class Del_Temporary_Profiles
    {
        public static void Delete() 
        {
            PaypalDbContext db = new PaypalDbContext();
            var list_tem_profile = db.Accounts.Where(x => x.Profile == true && x.Profile_Save != true).ToList();
            if(list_tem_profile.Count > 0) 
            {
                foreach (var acc in list_tem_profile)
                {
                    new Delete_Profiles().Delete(acc.ID, acc.ProfileId);
                }
            }
        }
    }
}
