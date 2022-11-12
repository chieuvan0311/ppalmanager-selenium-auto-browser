using PAYPAL.DataConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAYPAL.Dao
{
    public class UserDao
    {
        PaypalDbContext db = null;
        public UserDao()
        {
            db = new PaypalDbContext();
        }

        public bool Admin_Login(string name, string password)
        {
            var user = db.Users.Where(x => x.UserName == name && x.Password == password).FirstOrDefault();
            if(user != null) 
            {
                return true;
            }
            else 
            {
                return false;
            }
        }
    } 
}
