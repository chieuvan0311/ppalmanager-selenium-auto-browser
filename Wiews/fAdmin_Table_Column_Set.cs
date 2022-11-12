using PAYPAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PAYPAL.Wiews
{
    public partial class fAdmin_Table_Column_Set : Form
    {
        public delegate void Send_Back(FAdmin_Table_Column_Set_Model status);
        public Send_Back sendStatus;

        public fAdmin_Table_Column_Set(FAdmin_Table_Column_Set_Model status)
        {
            InitializeComponent();
            this.CenterToScreen();
            if (status != null) 
            {
                Load_Present_Status(status);
            }
        }
        
        private void Load_Present_Status(FAdmin_Table_Column_Set_Model stt) 
        {
            STT.Checked = stt.STT;
            Email.Checked = stt.Email;
            Balance.Checked = stt.Balance;
            TransactionTotal.Checked = stt.TransactionTotal;
            Profile_Created_Time.Checked = stt.Profile_Created_Time;

            Notification.Checked = stt.Notification;
            SessionResult.Checked = stt.SessionResult;
            UpdatedDateTime.Checked = stt.UpdatedDateTime;
            AccPassword.Checked = stt.AccPassword;
            TwoFA.Checked = stt.TwoFA;

            EmailPassword.Checked = stt.EmailPassword;
            Proxy.Checked = stt.Proxy;
            Profile.Checked = stt.Profile;
            ProfileId.Checked = stt.ProfileId;
            Email_2FA.Checked = stt.Email_2FA;

            AccName.Checked = stt.AccName;
            AccBirthDay.Checked = stt.AccBirthDay;
            Address.Checked = stt.Address;
            Phone.Checked = stt.Phone;
            BankCard.Checked = stt.BankCard;

            SeQuestion1.Checked = stt.SeQuestion1;
            SeQuestion2.Checked = stt.SeQuestion2;
            AccType.Checked = stt.AccType;
            AccOtherType.Checked = stt.AccOtherType;
            InputtedDate.Checked = stt.InputtedDate;

            RecoveryEmail.Checked = stt.RecoveryEmail;
            ForwordToEmail.Checked = stt.ForwordToEmail;
            SecondEmail.Checked = stt.SecondEmail;
            ProxyStatus.Checked = stt.ProxyStatus;
            Acc_ON_OFF.Checked = stt.Acc_ON_OFF;

            Set_ChangedPassPP.Checked = stt.Set_ChangedPassPP;
            Set_ChangedPassEmail.Checked = stt.Set_ChangedPassEmail;
            Set_Add_RecoveryEmail.Checked = stt.Set_Add_RecoveryEmail;
            Set_Deleted_FwEmail.Checked = stt.Set_Deleted_FwEmail;
            Set_Add_New_FwEmail.Checked = stt.Set_Add_New_FwEmail;

            Set_2FA.Checked = stt.Set_2FA;
            Set_Questions.Checked = stt.Set_Questions;
            Profile_Save.Checked = stt.Profile;
            AccPassword_Old.Checked = stt.AccPassword_Old;
            EmailPassword_Old.Checked = stt.EmailPassword_Old;
        }

        private void check_all_01_Click(object sender, EventArgs e)
        {
            STT.Checked = true;
            Email.Checked = true;
            Balance.Checked = true;
            TransactionTotal.Checked = true;
            Profile_Created_Time.Checked = true;
        }
        private void check_all_02_Click(object sender, EventArgs e)
        {
            Notification.Checked = true;
            SessionResult.Checked = true;
            UpdatedDateTime.Checked = true;
            AccPassword.Checked = true;
            TwoFA.Checked = true;
        }
        private void check_all_03_Click(object sender, EventArgs e)
        {
            EmailPassword.Checked = true;
            Proxy.Checked = true;
            Profile.Checked = true;
            ProfileId.Checked = true;
            Email_2FA.Checked = true;
        }
        private void check_all_04_Click(object sender, EventArgs e)
        {
            AccName.Checked = true;
            AccBirthDay.Checked = true;
            Address.Checked = true;
            Phone.Checked = true;
            BankCard.Checked = true;
        }
        private void check_all_05_Click(object sender, EventArgs e)
        {
            SeQuestion1.Checked = true;
            SeQuestion2.Checked = true;
            AccType.Checked = true;
            AccOtherType.Checked = true;
            InputtedDate.Checked = true;
        }
        private void check_all_06_Click(object sender, EventArgs e)
        {
            RecoveryEmail.Checked = true;
            ForwordToEmail.Checked = true;
            SecondEmail.Checked = true;
            ProxyStatus.Checked = true;
            Acc_ON_OFF.Checked = true;
        }
        private void check_all_07_Click(object sender, EventArgs e)
        {
            Set_ChangedPassPP.Checked = true;
            Set_ChangedPassEmail.Checked = true;
            Set_Add_RecoveryEmail.Checked = true;
            Set_Deleted_FwEmail.Checked = true;
            Set_Add_New_FwEmail.Checked = true;
        }
        private void check_all_08_Click(object sender, EventArgs e)
        {
            Set_2FA.Checked = true;
            Set_Questions.Checked = true;
            Profile_Save.Checked = true;
            AccPassword_Old.Checked = true;
            EmailPassword_Old.Checked = true;
        }

        private void uncheck_all_01_Click(object sender, EventArgs e)
        {
            STT.Checked = false;
            Email.Checked = false;
            Balance.Checked = false;
            TransactionTotal.Checked = false;
            Profile_Created_Time.Checked = false;
        }
        private void uncheck_all_02_Click(object sender, EventArgs e)
        {
            Notification.Checked = false;
            SessionResult.Checked = false;
            UpdatedDateTime.Checked = false;
            AccPassword.Checked = false;
            TwoFA.Checked = false;
        }
        private void uncheck_all_03_Click(object sender, EventArgs e)
        {
            EmailPassword.Checked = false;
            Proxy.Checked = false;
            Profile.Checked = false;
            ProfileId.Checked = false;
            Email_2FA.Checked = false;
        }
        private void uncheck_all_04_Click(object sender, EventArgs e)
        {
            AccName.Checked = false;
            AccBirthDay.Checked = false;
            Address.Checked = false;
            Phone.Checked = false;
            BankCard.Checked = false;
        }
        private void uncheck_all_05_Click(object sender, EventArgs e)
        {
            SeQuestion1.Checked = false;
            SeQuestion2.Checked = false;
            AccType.Checked = false;
            AccOtherType.Checked = false;
            InputtedDate.Checked = false;
        }
        private void uncheck_all_06_Click(object sender, EventArgs e)
        {
            RecoveryEmail.Checked = false;
            ForwordToEmail.Checked = false;
            SecondEmail.Checked = false;
            ProxyStatus.Checked = false;
            Acc_ON_OFF.Checked = false;
        }
        private void uncheck_all_07_Click(object sender, EventArgs e)
        {
            Set_ChangedPassPP.Checked = false;
            Set_ChangedPassEmail.Checked = false;
            Set_Add_RecoveryEmail.Checked = false;
            Set_Deleted_FwEmail.Checked = false;
            Set_Add_New_FwEmail.Checked = false;
        }
        private void uncheck_all_08_Click(object sender, EventArgs e)
        {
            Set_2FA.Checked = false;
            Set_Questions.Checked = false;
            Profile_Save.Checked = false;
            AccPassword_Old.Checked = false;
            EmailPassword_Old.Checked = false;

        }

        private void check_stt_email_only_Click(object sender, EventArgs e)
        {
            STT.Checked = true;
            Email.Checked = true;
            Balance.Checked = false;
            TransactionTotal.Checked = false;
            Profile_Created_Time.Checked = false;

            Notification.Checked = false;
            SessionResult.Checked = true;
            UpdatedDateTime.Checked = false;
            AccPassword.Checked = false;
            TwoFA.Checked = false;

            EmailPassword.Checked = false;
            Proxy.Checked = false;
            Profile.Checked = false;
            ProfileId.Checked = false;
            Email_2FA.Checked = false;

            AccName.Checked = false;
            AccBirthDay.Checked = false;
            Address.Checked = false;
            Phone.Checked = false;
            BankCard.Checked = false;

            SeQuestion1.Checked = false;
            SeQuestion2.Checked = false;
            AccType.Checked = false;
            AccOtherType.Checked = false;
            InputtedDate.Checked = false;

            RecoveryEmail.Checked = false;
            ForwordToEmail.Checked = false;
            SecondEmail.Checked = false;
            ProxyStatus.Checked = false;
            Acc_ON_OFF.Checked = false;

            Set_ChangedPassPP.Checked = false;
            Set_ChangedPassEmail.Checked = false;
            Set_Add_RecoveryEmail.Checked = false;
            Set_Deleted_FwEmail.Checked = false;
            Set_Add_New_FwEmail.Checked = false;

            Set_2FA.Checked = false;
            Set_Questions.Checked = false;
            Profile_Save.Checked = false;
            AccPassword_Old.Checked = false;
            EmailPassword_Old.Checked = false;
        }
        private void new_acc_change_info_status_Click(object sender, EventArgs e)
        {
            STT.Checked = true;
            Email.Checked = true;
            Balance.Checked = false;
            TransactionTotal.Checked = false;
            Profile_Created_Time.Checked = false;

            Notification.Checked = false;
            SessionResult.Checked = true;
            UpdatedDateTime.Checked = false;
            AccPassword.Checked = true;
            TwoFA.Checked = false;

            EmailPassword.Checked = true;
            Proxy.Checked = false;
            Profile.Checked = false;
            ProfileId.Checked = false;
            Email_2FA.Checked = false;

            AccName.Checked = false;
            AccBirthDay.Checked = false;
            Address.Checked = false;
            Phone.Checked = false;
            BankCard.Checked = false;

            SeQuestion1.Checked = false;
            SeQuestion2.Checked = false;
            AccType.Checked = true;
            AccOtherType.Checked = false;
            InputtedDate.Checked = false;

            RecoveryEmail.Checked = true;
            ForwordToEmail.Checked = true;
            SecondEmail.Checked = false;
            ProxyStatus.Checked = true;
            Acc_ON_OFF.Checked = true;

            Set_ChangedPassPP.Checked = true;
            Set_ChangedPassEmail.Checked = true;
            Set_Add_RecoveryEmail.Checked = true;
            Set_Deleted_FwEmail.Checked = true;
            Set_Add_New_FwEmail.Checked = true;

            Set_2FA.Checked = false;
            Set_Questions.Checked = false;
            Profile_Save.Checked = false;
            AccPassword_Old.Checked = true;
            EmailPassword_Old.Checked = true;
        }
        private void login_info_Click(object sender, EventArgs e)
        {
            STT.Checked = true;
            Email.Checked = true;
            Balance.Checked = false;
            TransactionTotal.Checked = false;
            Profile_Created_Time.Checked = false;

            Notification.Checked = false;
            SessionResult.Checked = true;
            UpdatedDateTime.Checked = false;
            AccPassword.Checked = true;
            TwoFA.Checked = true;

            EmailPassword.Checked = true;
            Proxy.Checked = false;
            Profile.Checked = false;
            ProfileId.Checked = false;
            Email_2FA.Checked = true;

            AccName.Checked = false;
            AccBirthDay.Checked = false;
            Address.Checked = false;
            Phone.Checked = false;
            BankCard.Checked = false;

            SeQuestion1.Checked = true;
            SeQuestion2.Checked = true;
            AccType.Checked = false;
            AccOtherType.Checked = false;
            InputtedDate.Checked = false;

            RecoveryEmail.Checked = false;
            ForwordToEmail.Checked = false;
            SecondEmail.Checked = false;
            ProxyStatus.Checked = true;
            Acc_ON_OFF.Checked = false;

            Set_ChangedPassPP.Checked = false;
            Set_ChangedPassEmail.Checked = false;
            Set_Add_RecoveryEmail.Checked = false;
            Set_Deleted_FwEmail.Checked = false;
            Set_Add_New_FwEmail.Checked = false;

            Set_2FA.Checked = false;
            Set_Questions.Checked = false;
            Profile_Save.Checked = true;
            AccPassword_Old.Checked = false;
            EmailPassword_Old.Checked = false;
        }
        private void profile_info_Click(object sender, EventArgs e)
        {
            STT.Checked = true;
            Email.Checked = true;
            Balance.Checked = false;
            TransactionTotal.Checked = false;
            Profile_Created_Time.Checked = true;

            Notification.Checked = false;
            SessionResult.Checked = false;
            UpdatedDateTime.Checked = false;
            AccPassword.Checked = false;
            TwoFA.Checked = false;

            EmailPassword.Checked = false;
            Proxy.Checked = true;
            Profile.Checked = true;
            ProfileId.Checked = true;
            Email_2FA.Checked = false;

            AccName.Checked = false;
            AccBirthDay.Checked = false;
            Address.Checked = false;
            Phone.Checked = false;
            BankCard.Checked = false;

            SeQuestion1.Checked = false;
            SeQuestion2.Checked = false;
            AccType.Checked = false;
            AccOtherType.Checked = false;
            InputtedDate.Checked = false;

            RecoveryEmail.Checked = false;
            ForwordToEmail.Checked = false;
            SecondEmail.Checked = false;
            ProxyStatus.Checked = true;
            Acc_ON_OFF.Checked = false;

            Set_ChangedPassPP.Checked = false;
            Set_ChangedPassEmail.Checked = false;
            Set_Add_RecoveryEmail.Checked = false;
            Set_Deleted_FwEmail.Checked = false;
            Set_Add_New_FwEmail.Checked = false;

            Set_2FA.Checked = false;
            Set_Questions.Checked = false;
            Profile_Save.Checked = true;
            AccPassword_Old.Checked = false;
            EmailPassword_Old.Checked = false;
        }
        private void acc_activities_Click(object sender, EventArgs e)
        {
            STT.Checked = true;
            Email.Checked = true;
            Balance.Checked = true;
            TransactionTotal.Checked = true;
            Profile_Created_Time.Checked = false;

            Notification.Checked = true;
            SessionResult.Checked = true;
            UpdatedDateTime.Checked = true;
            AccPassword.Checked = false;
            TwoFA.Checked = false;

            EmailPassword.Checked = false;
            Proxy.Checked = false;
            Profile.Checked = false;
            ProfileId.Checked = false;
            Email_2FA.Checked = false;

            AccName.Checked = false;
            AccBirthDay.Checked = false;
            Address.Checked = false;
            Phone.Checked = false;
            BankCard.Checked = false;

            SeQuestion1.Checked = false;
            SeQuestion2.Checked = false;
            AccType.Checked = false;
            AccOtherType.Checked = false;
            InputtedDate.Checked = false;

            RecoveryEmail.Checked = false;
            ForwordToEmail.Checked = false;
            SecondEmail.Checked = false;
            ProxyStatus.Checked = true;
            Acc_ON_OFF.Checked = false;

            Set_ChangedPassPP.Checked = false;
            Set_ChangedPassEmail.Checked = false;
            Set_Add_RecoveryEmail.Checked = false;
            Set_Deleted_FwEmail.Checked = false;
            Set_Add_New_FwEmail.Checked = false;

            Set_2FA.Checked = false;
            Set_Questions.Checked = false;
            Profile_Save.Checked = true;
            AccPassword_Old.Checked = false;
            EmailPassword_Old.Checked = false;
        }

        private void Acc_Info_Full_Click(object sender, EventArgs e)
        {
            STT.Checked = true;
            Email.Checked = true;
            Balance.Checked = true;
            TransactionTotal.Checked = true;
            Profile_Created_Time.Checked = false;

            Notification.Checked = false;
            SessionResult.Checked = true;
            UpdatedDateTime.Checked = false;
            AccPassword.Checked = true;
            TwoFA.Checked = true;

            EmailPassword.Checked = true;
            Proxy.Checked = false;
            Profile.Checked = false;
            ProfileId.Checked = false;
            Email_2FA.Checked = true;

            AccName.Checked = true;
            AccBirthDay.Checked = false;
            Address.Checked = true;
            Phone.Checked = false;
            BankCard.Checked = false;

            SeQuestion1.Checked = true;
            SeQuestion2.Checked = true;
            AccType.Checked = true;
            AccOtherType.Checked = false;
            InputtedDate.Checked = false;

            RecoveryEmail.Checked = true;
            ForwordToEmail.Checked = true;
            SecondEmail.Checked = false;
            ProxyStatus.Checked = false;
            Acc_ON_OFF.Checked = false;

            Set_ChangedPassPP.Checked = false;
            Set_ChangedPassEmail.Checked = false;
            Set_Add_RecoveryEmail.Checked = false;
            Set_Deleted_FwEmail.Checked = false;
            Set_Add_New_FwEmail.Checked = false;

            Set_2FA.Checked = false;
            Set_Questions.Checked = false;
            Profile_Save.Checked = false;
            AccPassword_Old.Checked = false;
            EmailPassword_Old.Checked = false;
        }
        private void set_up_btn_Click(object sender, EventArgs e)
        {
            FAdmin_Table_Column_Set_Model status_model = new FAdmin_Table_Column_Set_Model();

            status_model.STT = STT.Checked;
            status_model.Email = Email.Checked;
            status_model.Balance = Balance.Checked;
            status_model.TransactionTotal = TransactionTotal.Checked;
            status_model.Profile_Created_Time = Profile_Created_Time.Checked;

            status_model.Notification = Notification.Checked;
            status_model.SessionResult = SessionResult.Checked;
            status_model.UpdatedDateTime = UpdatedDateTime.Checked;
            status_model.AccPassword = AccPassword.Checked;
            status_model.TwoFA = TwoFA.Checked;

            status_model.EmailPassword = EmailPassword.Checked;
            status_model.Proxy = Proxy.Checked;
            status_model.Profile = Profile.Checked;
            status_model.ProfileId = ProfileId.Checked;
            status_model.Email_2FA = Email_2FA.Checked;

            status_model.AccName = AccName.Checked;
            status_model.AccBirthDay = AccBirthDay.Checked;
            status_model.Address = Address.Checked;
            status_model.Phone = Phone.Checked;
            status_model.BankCard = BankCard.Checked;

            status_model.SeQuestion1 = SeQuestion1.Checked;
            status_model.SeQuestion2 = SeQuestion2.Checked;
            status_model.AccType = AccType.Checked;
            status_model.AccOtherType = AccOtherType.Checked;
            status_model.InputtedDate = InputtedDate.Checked;

            status_model.RecoveryEmail = RecoveryEmail.Checked;
            status_model.ForwordToEmail = ForwordToEmail.Checked;
            status_model.SecondEmail = SecondEmail.Checked;
            status_model.ProxyStatus = ProxyStatus.Checked;
            status_model.Acc_ON_OFF = Acc_ON_OFF.Checked;

            status_model.Set_ChangedPassPP = Set_ChangedPassPP.Checked;
            status_model.Set_ChangedPassEmail = Set_ChangedPassEmail.Checked;
            status_model.Set_Add_RecoveryEmail = Set_Add_RecoveryEmail.Checked;
            status_model.Set_Deleted_FwEmail = Set_Deleted_FwEmail.Checked;
            status_model.Set_Add_New_FwEmail = Set_Add_New_FwEmail.Checked;

            status_model.Set_2FA = Set_2FA.Checked;
            status_model.Set_Questions = Set_Questions.Checked;
            status_model.Profile_Save = Profile_Save.Checked;
            status_model.AccPassword_Old = AccPassword_Old.Checked;
            status_model.EmailPassword_Old = EmailPassword_Old.Checked;

            sendStatus(status_model);
            this.Close();
        }
    }
}
