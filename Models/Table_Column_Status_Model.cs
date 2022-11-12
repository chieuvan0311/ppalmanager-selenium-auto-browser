﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAYPAL.Models
{
    public class Table_Column_Status_Model
    {
        //Mặc định hiển thị
        public bool STT { get; set; }
        public bool Email { get; set; }
        public bool Balance { get; set; }
        public bool TransactionTotal { get; set; } 
        public bool Profile_Created_Time { get; set; } 

        public bool Notification { get; set; }       
        public bool SessionResult { get; set; }
        public bool UpdatedDateTime { get; set; }
        public bool AccPassword { get; set; }
        public bool TwoFA { get; set; }

        public bool EmailPassword { get; set; }
        public bool Proxy { get; set; }
        public bool Profile { get; set; }
        public bool ProfileId { get; set; }
        public bool Email_2FA { get; set; }

        public bool AccName { get; set; }
        public bool AccBirthDay { get; set; }
        public bool Address { get; set; }
        public bool Phone { get; set; }
        public bool BankCard { get; set; }


        public bool SeQuestion1 { get; set; }
        public bool SeQuestion2 { get; set; }
        public bool AccType { get; set; } 
        public bool AccOtherType { get; set; } 
        public bool RecoveryEmail { get; set; }

        public bool ForwordToEmail { get; set; }
        public bool SecondEmail { get; set; }
        public bool ProxyStatus { get; set; }
        public bool Acc_ON_OFF { get; set; }
        public bool Profile_Save { get; set; }
    }
}
