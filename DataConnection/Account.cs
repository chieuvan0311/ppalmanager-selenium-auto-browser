namespace PAYPAL.DataConnection
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Account")]
    public partial class Account
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(50)]
        public string Balance { get; set; }

        [StringLength(250)]
        public string TransactionTotal { get; set; }

        public DateTime? Profile_Created_Time { get; set; }

        [StringLength(250)]
        public string Notification { get; set; }

        [StringLength(250)]
        public string SessionResult { get; set; }

        [StringLength(250)]
        public string UpdatedDateTime { get; set; }

        [StringLength(50)]
        public string AccPassword { get; set; }

        [StringLength(50)]
        public string EmailPassword { get; set; }

        [StringLength(50)]
        public string Proxy { get; set; }

        public bool? Profile { get; set; }

        [StringLength(100)]
        public string ProfileId { get; set; }

        [StringLength(50)]
        public string Email_2FA { get; set; }

        [StringLength(50)]
        public string AccName { get; set; }

        [StringLength(250)]
        public string AccBirthDay { get; set; }

        [StringLength(250)]
        public string Address { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string BankCard { get; set; }

        [StringLength(50)]
        public string SeQuestion1 { get; set; }

        [StringLength(50)]
        public string SeQuestion2 { get; set; }

        [StringLength(50)]
        public string TwoFA { get; set; }

        [StringLength(50)]
        public string AccType { get; set; }

        [StringLength(50)]
        public string AccOtherType { get; set; }

        [StringLength(50)]
        public string EmailType { get; set; }

        [StringLength(50)]
        public string AccCategory { get; set; }

        [StringLength(250)]
        public string InputtedDate { get; set; }

        [StringLength(50)]
        public string RecoveryEmail { get; set; }

        [StringLength(50)]
        public string ForwordToEmail { get; set; }

        [StringLength(50)]
        public string SecondEmail { get; set; }

        [StringLength(250)]
        public string ProxyStatus { get; set; }

        public bool? Acc_ON_OFF { get; set; }

        public bool? Set_ChangedPassPP { get; set; }

        public bool? Set_ChangedPassEmail { get; set; }

        public bool? Set_Deleted_FwEmail { get; set; }

        public bool? Set_Add_New_FwEmail { get; set; }

        public bool? Set_Add_RecoveryEmail { get; set; }

        public bool? Set_2FA { get; set; }

        public bool? Set_Questions { get; set; }

        public bool? Profile_Save { get; set; }

        [StringLength(500)]
        public string AccPassword_Old { get; set; }

        [StringLength(500)]
        public string EmailPassword_Old { get; set; }

        [StringLength(500)]
        public string Acc_2FA_Old { get; set; }

        [StringLength(500)]
        public string Email_2FA_Old { get; set; }

        [StringLength(500)]
        public string RecoveryEmail_Old { get; set; }

        [StringLength(1500)]
        public string Questions_Old { get; set; }

        [StringLength(500)]
        public string Random_AccPassword { get; set; }

        [StringLength(500)]
        public string Random_EmailPassword { get; set; }

        [StringLength(1500)]
        public string Random_Questions { get; set; }

        public bool? Canvas_profile { get; set; }
    }
}
