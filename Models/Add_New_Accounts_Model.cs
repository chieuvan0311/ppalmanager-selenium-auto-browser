using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAYPAL.Models
{
    public class Add_New_Accounts_Model
    {
        public int STT { get; set; }
        public string Email { get; set; }

        [DisplayName("Mật khẩu Papyal")]
        public string AccPassword { get; set; }

        [DisplayName("2FA Paypal")]
        public string TwoFA { get; set; }


        [DisplayName("Mật khẩu Email")]
        public string EmailPassword { get; set; }

        [DisplayName("2FA Email")]
        public string Email_2FA { get; set; }

        public string RecoveryEmail { get; set; }
        public string ForwordToEmail { get; set; }

        public string Proxy { get; set; }

        [DisplayName("Kết quả")]
        public string Status { get; set; }
    }
}
