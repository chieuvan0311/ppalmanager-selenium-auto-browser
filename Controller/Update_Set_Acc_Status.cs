using PAYPAL.DataConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAYPAL.Controller
{
    public class Update_Set_Acc_Status
    {
        public bool Update (Account Acc) 
        {
            bool result = false;

            if(Acc.Set_Deleted_FwEmail == true && Acc.Set_Add_New_FwEmail == true && Acc.Set_Add_RecoveryEmail == true && 
                Acc.Set_ChangedPassEmail == true && Acc.Set_ChangedPassPP == true) 
            {
                result = true;
            }
            return result;
        }
    }
}
