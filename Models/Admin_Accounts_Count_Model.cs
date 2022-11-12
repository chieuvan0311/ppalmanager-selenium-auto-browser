using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAYPAL.Models
{
    public class Admin_Accounts_Count_Model
    {
        [DisplayName("Tất cả")]
        public int All_Accounts { get; set; }

        [DisplayName("Tài khoản mới")]
        public int New_Accounts { get; set; }

        [DisplayName("Tài khoản cũ")]
        public int Old_Accounts { get; set; }

        [DisplayName("Tk cũ - hoạt động")]
        public int Old_Working { get; set; }

        [DisplayName("Tk cũ - Limit")]
        public int Old_Limit { get; set; }

        [DisplayName("Tk cũ - 180d")]
        public int Old_180D { get; set; }

        [DisplayName("Tk cũ - Set fail")]
        public int Old_SetFailed { get; set; }

        [DisplayName("Tk cũ - sai thông tin")]
        public int Old_WrongInfo { get; set; }

        [DisplayName("Tk cũ - xác minh tk")]
        public int Old_Verified { get; set; }

        [DisplayName("Tk cũ - chờ set")]
        public int Old_WaitToSet { get; set; }
    }
}
