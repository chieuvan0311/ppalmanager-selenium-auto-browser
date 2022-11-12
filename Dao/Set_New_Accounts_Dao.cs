using PAYPAL.Controller;
using PAYPAL.DataConnection;
using PAYPAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAYPAL.Dao
{
    public class Set_New_Accounts_Dao
    {
        PaypalDbContext db = null;
        public Set_New_Accounts_Dao()
        {
            db = new PaypalDbContext();
        }

        public void Update_Balance_Notifications_Status_Name_Adress_DateTime (int id, string balance, string note, string acctype, string name, string address, string datetime)
        {
            var acc = db.Accounts.Where(x => x.ID == id).FirstOrDefault();
            if (!string.IsNullOrEmpty(balance)) { acc.Balance = balance; }
            if (!string.IsNullOrEmpty(note)) { acc.Notification = note; }
            if (!string.IsNullOrEmpty(acctype)) { acc.AccType = acctype; }
            if (!string.IsNullOrEmpty(name)) { acc.AccName = name; }
            if (!string.IsNullOrEmpty(address)) { acc.Address = address; }
            if (!string.IsNullOrEmpty(datetime)) { acc.UpdatedDateTime = datetime; }
            db.SaveChanges();           
        }
        public void Past_Paypal_Password_Save (ID_String_Model md) 
        {
            var account = db.Accounts.Where(x => x.ID == md.ID).FirstOrDefault();
            account.AccPassword_Old = account.AccPassword;
            account.AccPassword = md.Str_Value;
            db.SaveChanges();
        }
        public void Past_Paypal_2FA_Save(ID_String_Model md)
        {
            var account = db.Accounts.Where(x => x.ID == md.ID).FirstOrDefault();
            account.Acc_2FA_Old = account.TwoFA;
            account.TwoFA = md.Str_Value;
            db.SaveChanges();
        }
        public void Past_Email_Password_Save(ID_String_Model md)
        {
            var account = db.Accounts.Where(x => x.ID == md.ID).FirstOrDefault();
            account.EmailPassword_Old = account.EmailPassword;
            account.EmailPassword = md.Str_Value;
            db.SaveChanges();
        }
        public void Past_ForwardEmail(ID_String_Model md)
        {
            var account = db.Accounts.Where(x => x.ID == md.ID).FirstOrDefault();
            account.ForwordToEmail = md.Str_Value;
            db.SaveChanges();
        }

        //public string Update_Acc_ON_OFF_By_ID (int id)
        //{
        //    var acc = db.Accounts.Where(x => x.ID == id).FirstOrDefault();
        //    string result = "OFF";
        //    if (acc.Set_ChangedPassPP == true && acc.Set_ChangedPassEmail == true && acc.Set_Add_RecoveryEmail == true && acc.Set_Deleted_FwEmail == true && acc.Set_Add_New_FwEmail == true)
        //    {
        //        result = true;
        //    }
        //    return result;
        //}

        //public string Update_Acc_ON_OFF(Account acc)
        //{
        //    bool result = false;
        //    if (acc.Set_ChangedPassPP == true && acc.Set_ChangedPassEmail == true && acc.Set_Add_RecoveryEmail == true && acc.Set_Deleted_FwEmail == true && acc.Set_Add_New_FwEmail == true)
        //    {
        //        result = true;
        //    }
        //    return result;
        //}
        //public bool Deleted_Old_ForwardEmail_Save (int id)
        //{
        //    bool sendback = false;
        //    var account = db.Accounts.Where(x => x.ID == id).FirstOrDefault();
        //    account.Set_Deleted_FwEmail = true;
        //    account.Acc_ON_OFF = Update_Acc_ON_OFF(account);
        //    db.SaveChanges();
        //    if(account.Acc_ON_OFF == true) { sendback = true; }
        //    return sendback;
        //}
        //public bool Add_New_ForwardEmail_Save (int id)
        //{
        //    bool sendback = false;
        //    var account = db.Accounts.Where(x => x.ID == id).FirstOrDefault();
        //    account.Set_Add_New_FwEmail = true;
        //    account.Acc_ON_OFF = Update_Acc_ON_OFF(account);
        //    db.SaveChanges();
        //    if(account.Acc_ON_OFF == "YES") { sendback = true; }
        //    return sendback;
        //}
        //public void Add_RecoveryEmail_Save (int id, string recoveryEmail)
        //{
        //    var account = db.Accounts.Where(x => x.ID == id).FirstOrDefault();
        //    account.RecoveryEmail = recoveryEmail;
        //    account.Set_Add_RecoveryEmail = true;
        //    account.Acc_ON_OFF = Update_Acc_ON_OFF(account);
        //    db.SaveChanges();
        //}
        //public void Change_Email_Password_Save (int id, string password)
        //{
        //    var account = db.Accounts.Where(x => x.ID == id).FirstOrDefault();
        //    account.EmailPassword_Old = account.EmailPassword;
        //    account.EmailPassword = password;           
        //    account.Set_ChangedPassEmail = true;
        //    account.Acc_ON_OFF = Update_Acc_ON_OFF(account);
        //    db.SaveChanges();
        //}
        //public void Change_Paypal_Password_Save(int id, string new_password)
        //{
        //    var acc = db.Accounts.Where(x => x.ID == id).FirstOrDefault();
        //    acc.AccPassword_Old = acc.AccPassword;
        //    acc.AccPassword = new_password;
        //    acc.Set_ChangedPassPP = true;
        //    acc.Acc_ON_OFF = Update_Acc_ON_OFF(acc);
        //    db.SaveChanges();
        //}
    }
}
