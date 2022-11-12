using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAYPAL.Models
{
    public class AccType_Model
    {
        [DisplayName("Tất cả")]
        public int All { get; set; }

        [DisplayName("Hoạt động")]
        public int Working { get; set; }

        [DisplayName("Limit")]
        public int Limit { get; set; }

        [DisplayName("180d")]
        public int _180D { get; set; }

        [DisplayName("Set fail")]
        public int SetFailed { get; set; }

        [DisplayName("Sai thông tin")]
        public int WrongInfo { get; set; }

        [DisplayName("Cần xác minh")]
        public int Verified { get; set; }

        [DisplayName("Chưa set")]
        public int WaitToSet { get; set; }
    }
}
