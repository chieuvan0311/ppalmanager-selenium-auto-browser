using PAYPAL.DataConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAYPAL.Dao
{
    public class On_Accounts_Dao
    {
        PaypalDbContext db = null;
        public On_Accounts_Dao()
        {
            db = new PaypalDbContext();
        }

        // Get Data
        public List<Account> Get_All_On_Accounts() //lấy tất cả
        {
            return db.Accounts.Where(x=>x.Acc_ON_OFF == true).ToList();
        }

        public Account Get_On_Account_By_Email(string email)
        {
            return db.Accounts.Where(x => x.Email == email && x.Acc_ON_OFF == true).FirstOrDefault();
        }


        public List<Account> Get_List_On_Acc_By_AccType (string accTypeName) //Chọn theo Tình Trạng Acc
        { 
            return db.Accounts.Where(x => x.AccType == accTypeName && x.Acc_ON_OFF == true).ToList();
        }

        //Check data

        public bool Check_On_Acc_Email_Existing(string email)
        {
            var account = db.Accounts.Where(x => x.Email == email && x.Acc_ON_OFF == true).FirstOrDefault();
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
        public int Counting_On_Acc_By_AccTypeName(string name)
        {
            if (name == "Tất cả")
            {
                return db.Accounts.Where(x=>x.Acc_ON_OFF == true).Count();
            }
            else if(name == "Profiles")
            {
                return db.Accounts.Where(x => x.Profile_Save == true && x.Acc_ON_OFF == true).Count();
            }
            else
            {
                return db.Accounts.Where(x => x.AccType == name && x.Acc_ON_OFF == true).Count();
            }
        }

        public int Counting_All_Acc_By_CboxName (string name)
        {
            if(name == "Chọn danh mục") 
            {
                return 0;
            }
            else if (name == "Tất cả")
            {
                return db.Accounts.Count();
            }
            else if(name == "Acc cũ")
            {
                return db.Accounts.Where(x => x.Acc_ON_OFF == true).Count();
            }
            else if (name == "Acc mới")
            {
                return db.Accounts.Where(x => x.Acc_ON_OFF != true).Count();
            }
            else if(name == "Profiles") 
            {
                return db.Accounts.Where(x => x.Profile_Save == true).Count();
            }
            else
            {
                var listAcc = db.Accounts.Where(x => x.AccType == name && x.Acc_ON_OFF == true);
                return listAcc.Count();
            }
        }
    }
}
