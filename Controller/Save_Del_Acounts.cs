using PAYPAL.DataConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAYPAL.Controller
{
    public class Save_Del_Acounts
    {
        public void Save(List<Account> accounts) 
        {
            foreach (var acc in accounts) 
            {
                Del_Account del_account = new Del_Account();

                del_account.Email = acc.Email;
                del_account.Balance = acc.Balance;
                del_account.Profile_Created_Time = acc.Profile_Created_Time;
                del_account.TransactionTotal = acc.TransactionTotal;
                del_account.Notification = acc.Notification;
                del_account.SessionResult = acc.SessionResult;
                del_account.UpdatedDateTime = acc.UpdatedDateTime;
                del_account.AccPassword = acc.AccPassword;
                del_account.EmailPassword = acc.EmailPassword;
                del_account.Proxy = acc.Proxy;
                del_account.Profile = acc.Profile;
                del_account.ProfileId = acc.ProfileId;
                del_account.Email_2FA = acc.Email_2FA;
                del_account.AccName = acc.AccName;
                del_account.AccBirthDay = acc.AccBirthDay;
                del_account.Address = acc.Address;
                del_account.Phone = acc.Phone;
                del_account.BankCard = acc.BankCard;
                del_account.SeQuestion1 = acc.SeQuestion1;
                del_account.SeQuestion2 = acc.SeQuestion2;
                del_account.TwoFA = acc.TwoFA;
                del_account.AccType = acc.AccType;
                del_account.AccOtherType = acc.AccOtherType;
                del_account.EmailType = acc.EmailType;
                del_account.AccCategory = acc.AccCategory;
                del_account.InputtedDate = acc.InputtedDate;
                del_account.RecoveryEmail = acc.RecoveryEmail;
                del_account.ForwordToEmail = acc.ForwordToEmail;
                del_account.SecondEmail = acc.SecondEmail;
                del_account.ProxyStatus = acc.ProxyStatus;
                del_account.Acc_ON_OFF = acc.Acc_ON_OFF;
                del_account.Set_ChangedPassPP = acc.Set_ChangedPassPP;
                del_account.Set_ChangedPassEmail = acc.Set_ChangedPassEmail;
                del_account.Set_Deleted_FwEmail = acc.Set_Deleted_FwEmail;
                del_account.Set_Add_New_FwEmail = acc.Set_Add_New_FwEmail;
                del_account.Set_Add_RecoveryEmail = acc.Set_Add_RecoveryEmail;
                del_account.Set_2FA = acc.Set_2FA;
                del_account.Set_Questions = acc.Set_Questions;
                del_account.Profile_Save = acc.Profile_Save;
                del_account.AccPassword_Old = acc.AccPassword_Old;
                del_account.EmailPassword_Old = acc.EmailPassword_Old;
                del_account.Acc_2FA_Old = acc.Acc_2FA_Old;
                del_account.Email_2FA_Old = acc.Email_2FA_Old;
                del_account.RecoveryEmail_Old = acc.RecoveryEmail_Old;
                del_account.Questions_Old = acc.Questions_Old;
                del_account.Random_AccPassword = acc.Random_AccPassword;
                del_account.Random_EmailPassword = acc.Random_EmailPassword;
                del_account.Random_Questions = acc.Random_Questions;

                using (PaypalDbContext db = new PaypalDbContext()) 
                {
                    db.Del_Account.Add(del_account);
                    db.SaveChanges();
                }    
            }       
        }
    }
}
