using PAYPAL.DataConnection;
using PAYPAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAYPAL.Dao
{
    public class AccountDao
    {
        PaypalDbContext db = null;
        public AccountDao()
        {
            db = new PaypalDbContext();
        }

        // Get Data

        public List<Account> Get_All_Accounts()
        {
            return db.Accounts.ToList();
        }

        public Account Get_Account_By_Email(string email)
        {
            return db.Accounts.Where(x=> x.Email == email).FirstOrDefault();
        }


        public List<Account> Get_List_Acc_By_AccType (string accTypeName) 
        {
            return db.Accounts.Where(x => x.AccType == accTypeName).ToList();
        }

        public List<string> Get_All_Account_Emails_List()
        {
            List<string> listEmail = new List<string>();
            var account = db.Accounts;
            foreach (var item in account)
            {
                string accountEmail = item.Email;
                listEmail.Add(accountEmail);
            }
            return listEmail;
        }

        //Check data

        public bool Check_Email_Existing (string email)
        {
            var account = db.Accounts.Where(x => x.Email == email).FirstOrDefault();
            if (account != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Counting
        public int Get_Total_By_AccTypeName(string name) 
        {
            if (name == "Tất cả")
            {
                return db.Accounts.Count();
            }
            else
            {
                var listAcc = db.Accounts.Where(x => x.AccType == name);
                return listAcc.Count();
            }
        }

        //Update data

        public void Past_Proxy_Update(Proxy_Model model) 
        {
            var account = db.Accounts.Where(x => x.ID == model.ID).FirstOrDefault();
            //account.ProxyIP = model.ProxyIP;
            //account.ProxyPort = model.ProxyPort;
            //account.ProxyName = model.ProxyName;
            //account.ProxyPassword = model.ProxyPassword;
            db.SaveChanges();
        }

        public void Update_Balance_Notifications_Status_DateTime(int id, string balance, string note, string acctype, string datetime)
        {
            var acc = db.Accounts.Where(x => x.ID == id).FirstOrDefault();
            if (!string.IsNullOrEmpty(balance)) { acc.Balance = balance; }
            if (!string.IsNullOrEmpty(note)) { acc.Notification = note; }
            if (!string.IsNullOrEmpty(acctype)) { acc.AccType = acctype; }
            if (!string.IsNullOrEmpty(datetime)) { acc.UpdatedDateTime = datetime; }
            db.SaveChanges();
        }

        public void Update_AccType(ID_String_Model model) 
        {
            var account = db.Accounts.Where(x => x.ID == model.ID).FirstOrDefault();
            account.AccType = model.Str_Value;
            db.SaveChanges();
        }

        public void Proxy_Status_Update (int id, string note)
        {
            var account = db.Accounts.Where(x => x.ID == id).FirstOrDefault();
            account.ProxyStatus = note;
            db.SaveChanges();
        }

        public void Update_Profile_Created (ID_String_Model model) 
        {
            var account = db.Accounts.Where(x => x.ID == model.ID).FirstOrDefault();
            account.Profile = true;
            db.SaveChanges();
        }
        
        public void Update_Name_Address(int id, string name, string address, string datetime) 
        {
            var account = db.Accounts.Where(x => x.ID == id).FirstOrDefault();
            account.AccName = name;
            account.Address = address;
            account.UpdatedDateTime = datetime;
            db.SaveChanges();
        }
    }
}
