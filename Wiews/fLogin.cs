using PAYPAL.Dao;
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
    public partial class fLogin : Form
    {
        int stt = 0;

        public delegate void Flogin_Send_Back(int number);
        public Flogin_Send_Back send_back_status;

        public fLogin()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        public fLogin(int fManager_stt)
        {
            InitializeComponent();
            this.CenterToScreen();
            stt = fManager_stt;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string name = tbName.Text;
            string pass = tbPassword.Text;
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(pass))
            {
                bool result = new UserDao().Admin_Login(name, pass);
                if (result == true)
                {
                    if (stt == 1)
                    {
                        fAdmin form1 = new fAdmin();
                        this.Hide();
                        form1.ShowDialog();
                        send_back_status(1);
                        this.Close();
                    }
                
                    else if (stt == 2)
                    {
                        fAddNewAccounts form2 = new fAddNewAccounts();
                        this.Hide();
                        form2.ShowDialog();
                        send_back_status(2);
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Tên, hoặc mật khẩu không đúng!");
                }
            }
            else
            {
                MessageBox.Show("Cần nhập đủ tên và mật khẩu!");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
