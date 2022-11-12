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
    public partial class fTableColumnSet : Form
    {
        public delegate void Send_Back (Table_Column_Status_Model status);
        public Send_Back sendStatus;

        public fTableColumnSet()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        public fTableColumnSet(Table_Column_Status_Model model)
        {
            InitializeComponent();
            this.CenterToScreen();
            if (model != null) 
            {
                Load_Check_Box_Status(model);
            }
        }

        private void Load_Check_Box_Status(Table_Column_Status_Model model) 
        {
            STT.Checked = model.STT;
            Email.Checked = model.Email;
            Balance.Checked = model.Balance;
            TransactionTotal.Checked = model.TransactionTotal;
            Profile_Created_Time.Checked = model.Profile_Created_Time;

            Notification.Checked = model.Notification;
            SessionResult.Checked = model.SessionResult;
            UpdatedDateTime.Checked = model.UpdatedDateTime;
            AccPassword.Checked = model.AccPassword;
            TwoFA.Checked = model.TwoFA;

            EmailPassword.Checked = model.EmailPassword;
            Proxy.Checked = model.Proxy;
            Profile.Checked = model.Profile;
            ProfileId.Checked = model.ProfileId;
            Email_2FA.Checked = model.Email_2FA;

            AccName.Checked = model.AccName;
            AccBirthDay.Checked = model.AccBirthDay;
            Address.Checked = model.Address;
            Phone.Checked = model.Phone;
            BankCard.Checked = model.BankCard;

            SeQuestion1.Checked = model.SeQuestion1;
            SeQuestion2.Checked = model.SeQuestion2;
            AccType.Checked = model.AccType;
            AccOtherType.Checked = model.AccOtherType;
            RecoveryEmail.Checked = model.RecoveryEmail;

            ForwordToEmail.Checked = model.ForwordToEmail;
            SecondEmail.Checked = model.SecondEmail;
            ProxyStatus.Checked = model.ProxyStatus;
            Acc_ON_OFF.Checked = model.Acc_ON_OFF;
            Profile_Save.Checked = model.Profile;
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
            RecoveryEmail.Checked = true;
        }
        private void check_all_06_Click(object sender, EventArgs e)
        {
            ForwordToEmail.Checked = true;
            SecondEmail.Checked = true;
            ProxyStatus.Checked = true;
            Acc_ON_OFF.Checked = true;
            Profile_Save.Checked = true;
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
            RecoveryEmail.Checked = false;
        }
        private void uncheck_all_06_Click(object sender, EventArgs e)
        {
            ForwordToEmail.Checked = false;
            SecondEmail.Checked = false;
            ProxyStatus.Checked = false;
            Acc_ON_OFF.Checked = false;
            Profile_Save.Checked = false;
        }

        private void activevities_info_check_all_Click(object sender, EventArgs e)
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
            AccType.Checked = true;
            AccOtherType.Checked = false;
            RecoveryEmail.Checked = false;

            ForwordToEmail.Checked = false;
            SecondEmail.Checked = false;
            ProxyStatus.Checked = true;
            Acc_ON_OFF.Checked = false;
            Profile_Save.Checked = true;
        }
        private void login_info_check_all_Click(object sender, EventArgs e)
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
            RecoveryEmail.Checked = false;
            
            ForwordToEmail.Checked = false;
            SecondEmail.Checked = false;
            ProxyStatus.Checked = true;
            Acc_ON_OFF.Checked = false;
            Profile_Save.Checked = true;
        }
        private void acc_info_check_all_Click(object sender, EventArgs e)
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
            AccBirthDay.Checked = true;
            Address.Checked = true;
            Phone.Checked = false;
            BankCard.Checked = false;

            SeQuestion1.Checked = true;
            SeQuestion2.Checked = true;
            AccType.Checked = true;
            AccOtherType.Checked = false;
            RecoveryEmail.Checked = true;

            ForwordToEmail.Checked = true;
            SecondEmail.Checked = false;
            ProxyStatus.Checked = true;
            Acc_ON_OFF.Checked = false;
            Profile_Save.Checked = true;
        }
        private void profile_info_check_all_Click(object sender, EventArgs e)
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
            RecoveryEmail.Checked = false;

            ForwordToEmail.Checked = false;
            SecondEmail.Checked = false;
            ProxyStatus.Checked = true;
            Acc_ON_OFF.Checked = false;
            Profile_Save.Checked = true;
        }
        private void button4_Click(object sender, EventArgs e)
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
            RecoveryEmail.Checked = false;

            ForwordToEmail.Checked = false;
            SecondEmail.Checked = false;
            ProxyStatus.Checked = false;
            Acc_ON_OFF.Checked = false;
            Profile_Save.Checked = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Table_Column_Status_Model status = new Table_Column_Status_Model();
            status.STT = STT.Checked;
            status.Email = Email.Checked;
            status.Balance = Balance.Checked;
            status.TransactionTotal = TransactionTotal.Checked;
            status.Profile_Created_Time = Profile_Created_Time.Checked;

            status.Notification = Notification.Checked;
            status.SessionResult = SessionResult.Checked;
            status.UpdatedDateTime = UpdatedDateTime.Checked;
            status.AccPassword = AccPassword.Checked;
            status.TwoFA = TwoFA.Checked;

            status.EmailPassword = EmailPassword.Checked;
            status.Proxy = Proxy.Checked;
            status.Profile = Profile.Checked;
            status.ProfileId = ProfileId.Checked;
            status.Email_2FA = Email_2FA.Checked;

            status.AccName = AccName.Checked;
            status.AccBirthDay = AccBirthDay.Checked;
            status.Address = Address.Checked;
            status.Phone = Phone.Checked;
            status.BankCard = BankCard.Checked;

            status.SeQuestion1 = SeQuestion1.Checked;
            status.SeQuestion2 = SeQuestion2.Checked;
            status.AccType = AccType.Checked;
            status.AccOtherType = AccOtherType.Checked;
            status.RecoveryEmail = RecoveryEmail.Checked;

            status.ForwordToEmail = ForwordToEmail.Checked;
            status.SecondEmail = SecondEmail.Checked;
            status.ProxyStatus = ProxyStatus.Checked;
            status.Acc_ON_OFF = Acc_ON_OFF.Checked;
            status.Profile_Save = Profile_Save.Checked;

            sendStatus(status);
            this.Close();
        }
    }
}
