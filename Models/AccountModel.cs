using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAYPAL.Models
{
    public class AccountModel
    {
        public int STT { get; set; } // index 0
        public int ID { get; set; } // index 1, không hiển thị
        public string Email { get; set; } // index 2

        [DisplayName("Số dư")]
        public string Balance { get; set; } // index 3

        [DisplayName("Số giao dịch")]
        public string TransactionTotal { get; set; } // index 4

        [DisplayName("Profile time")]
        public string Profile_Created_Time { get; set; } // index 5

        [DisplayName("Thông báo")]
        public string Notification { get; set; } // index 6
   
        [DisplayName("Kết quả phiên")]
        public string SessionResult { get; set; } // index 7

        [DisplayName("Thời gian cập nhật")]
        public string UpdatedDateTime { get; set; } // index 8

        //Thông tin đăng  nhập
        [DisplayName("Mật khẩu PP")]
        public string AccPassword { get; set; } // index 9

        [DisplayName("2FA PP")]
        public string TwoFA { get; set; } // index 10

        [DisplayName("Mk email")]
        public string EmailPassword { get; set; } // index 11

        public string Proxy { get; set; } // index 12

        [DisplayName("Profile Tạm")]
        public string Profile { get; set; } // index 13

        public string ProfileId { get; set; } // index 14

        [DisplayName("2FA Email")]
        public string Email_2FA { get; set; } // index 15

        //Thông tin Acc
        [DisplayName("Họ tên")]
        public string AccName { get; set; } // index 16

        [DisplayName("Ngày sinh")]
        public string AccBirthDay { get; set; } // index 17

        [DisplayName("Địa chỉ")]
        public string Address { get; set; } // index 18

        [DisplayName("Số đt")]
        public string Phone { get; set; } // index 19

        [DisplayName("Thẻ ngân hàng")]
        public string BankCard { get; set; } // index 20

        [DisplayName("Câu hỏi 1")]
        public string SeQuestion1 { get; set; } // index 21

        [DisplayName("Câu hỏi 2")]
        public string SeQuestion2 { get; set; } // index 22

        //Quảng lý acc
        [DisplayName("Tình trạng Acc")]
        public string AccType { get; set; } // index 23, mặc định hiển thị

        [DisplayName("Loại Acc")]
        public string AccOtherType { get; set; } // Index 24

        public string AccCategory { get; set; } // Index 25, không hiển thị ra bảng
      
        [DisplayName("Ngày nhập APP")]
        public string InputtedDate { get; set; } // index 26

        [DisplayName("Loại email")]
        public string EmailType { get; set; } // index 27, không hiển thị ra bảng

        [DisplayName("Email KP")]
        public string RecoveryEmail { get; set; } // index 28

        [DisplayName("Forward đến Email")]
        public string ForwordToEmail { get; set; }  // Index 29

        [DisplayName("Email PP 2")]
        public string SecondEmail { get; set; } // Index 30

        [DisplayName("Tình trạng Proxy")]
        public string ProxyStatus { get; set; } // Index 31

        [DisplayName("Acc tắt mở")]
        public string Acc_ON_OFF { get; set; } // Index 32

        [DisplayName("Đổi Mk PP")]
        public string Set_ChangedPassPP { get; set; } // Index 33

        [DisplayName("Đổi Mk Email")]
        public string Set_ChangedPassEmail { get; set; } // Index 34

        [DisplayName("Thêm Email KP")]
        public string Set_Add_RecoveryEmail { get; set; } // Index 35

        [DisplayName("Xóa Email fw cũ")]
        public string Set_Deleted_FwEmail { get; set; } // Index 36

        [DisplayName("Thêm Email fw mới")]
        public string Set_Add_New_FwEmail { get; set; } // Index 37

        [DisplayName("Thêm 2FA")]
        public string Set_2FA { get; set; } // Index 38

        [DisplayName("Thêm câu hỏi")]
        public string Set_Questions { get; set; } // Index 39

        [DisplayName("Profile")]
        public string Profile_Save { get; set; } // Index 40

        [DisplayName("Mk PP - cũ")]
        public string AccPassword_Old { get; set; } // Index 41

        [DisplayName("Mk Email - cũ")]
        public string EmailPassword_Old { get; set; } // Index 42

        //public string Acc_2FA_Old { get; set; } // Index 43
        //public string Email_2FA_Old { get; set; } // Index 44
        //public string RecoveryEmail_Old { get; set; } // Index 45
        //public string Questions_Old { get; set; } // Index 46
        //public string Random_AccPassword { get; set; } // Index 47
        //public string Random_EmailPassword { get; set; } // Index 48
        //public string Random_Questions { get; set; } // Index 49
    }
}
