using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using PAYPAL.ChromeDrivers;
using PAYPAL.Controller;
using PAYPAL.Dao;
using PAYPAL.DataConnection;
using PAYPAL.DataTransfer;
using PAYPAL.Gmail_Controller;
using PAYPAL.GPM_API;
using PAYPAL.Hotmail_Controller;
using PAYPAL.Models;
using PAYPAL.RandomData;
using PAYPAL.Wiews;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace PAYPAL
{
    public partial class fTableManager : Form
    {
        private List<AccountModel> data = null;
        private Table_Column_Status_Model status = null;
        public fTableManager()
        {
            bool start = new Check_API().Start();
            if(start == true) 
            {
                InitializeComponent();
                this.CenterToScreen();
                Load_Form_TableManager();

                List<Account> accounts = null;
                PaypalDbContext db = new PaypalDbContext();
                string value = db.Admins.Where(x => x.Name == "Tem_Profiles_Litmited").FirstOrDefault().Value;
                int profile_limit = int.Parse(value);
                accounts = db.Accounts.Where(x => x.Profile == true && x.Profile_Save != true).OrderBy(x => x.Profile_Created_Time).ToList();
                if (accounts.Count > profile_limit)
                {
                    int del_profile = accounts.Count - profile_limit;
                    for (int i = 0; i < del_profile; i++) { new Delete_Profiles().Delete(accounts[i].ID, accounts[i].ProfileId); }
                }

                try
                {
                    new Google_Sheet_Controller().Update_Database();
                    new Google_Sheet_Controller().Update_Del_Account();
                }
                catch
                {
                    MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo", MessageBoxButtons.OK,
                                MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                }
            }
            else 
            {
                this.Close();
            }
        }

        private Account Get_Acc_Info(int i)
        {
            int ID = int.Parse(gvAccountTable.SelectedRows[i].Cells[1].Value.ToString());
            PaypalDbContext db = new PaypalDbContext();
            var Acc = db.Accounts.Where(x => x.ID == ID).FirstOrDefault();
            return Acc;
        }
        private void Temprory_Profile_BTN_Click(object sender, EventArgs e)
        {
            var check_api = new Check_API().Start();
            if (check_api == true)
            {
                List<Account> accounts = null;
                PaypalDbContext db = new PaypalDbContext();
                string value = db.Admins.Where(x => x.Name == "Tem_Profiles_Litmited").FirstOrDefault().Value;
                int profile_limit = int.Parse(value);
                accounts = db.Accounts.Where(x => x.Profile == true && x.Profile_Save != true && x.Acc_ON_OFF == true).OrderBy(x => x.Profile_Created_Time).ToList();
                if (accounts.Count > profile_limit)
                {
                    int del_profile = accounts.Count - profile_limit;
                    StringWriter stringWriter = new StringWriter();
                    stringWriter.WriteLine("- Số profile tạm hiện tại là: " + accounts.Count.ToString());
                    stringWriter.WriteLine("- Bạn muốn xóa : " + del_profile.ToString()+ " profile?");
                    DialogResult result = MessageBox.Show(stringWriter.ToString(), "Xác nhận", MessageBoxButtons.YesNo);
                    if(result == DialogResult.Yes) 
                    {
                        for (int i = 0; i < del_profile; i++)
                        {
                            new Delete_Profiles().Delete(accounts[i].ID, accounts[i].ProfileId);
                        }
                    }               
                }

                var accounts_01 = db.Accounts.Where(x => x.Profile == true && x.Profile_Save != true && x.Acc_ON_OFF == true).OrderBy(x => x.Profile_Created_Time).ToList();
                data = new Account_Table_Data().Get_Account_Table_Data(accounts_01);
                cbAccStatusList.Text = "Profile tạm";
                Load_Form_TableManager();
                tbCount.Text = accounts_01.Count.ToString();
            }
        }
        private void copy_cbox_CheckedChanged(object sender, EventArgs e)
        {
            if (copy_cbox.Checked == true)
            {
                DialogResult dialogResult = MessageBox.Show("Sau khi copy bạn cần bỏ 'Tick' nếu muốn trở về chế độ làm việc bình thường!", "Thông báo!", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    gvAccountTable.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                    gvAccountTable.MultiSelect = true;
                    gvAccountTable.RowHeadersWidth = 65;
                }
                else { copy_cbox.Checked = false; }
            }
            else
            {
                gvAccountTable.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                gvAccountTable.MultiSelect = true;
                gvAccountTable.RowHeadersWidth = 15;
            }
        }
        private void btn_Add_New_Accounts_Click(object sender, EventArgs e)
        {           
            fLogin form = new fLogin(2);
            this.Hide();
            form.send_back_status = new fLogin.Flogin_Send_Back(Receive_SendBack_And_Do);
            form.ShowDialog();
            this.Show();
        }
        private void Receive_SendBack_And_Do (int number)
        {
            if (number == 1)
            {
                this.Close();
            }
            else if (number == 2) 
            {
                tbCount.Text = null;
                tbSearch.Text = null;
                cbAccStatusList.Text = "Chọn danh mục";
                Load_Form_TableManager();
            }
        }
        private void btnAdmin_Click(object sender, EventArgs e)
        {
            fLogin form = new fLogin(1);
            this.Hide();
            form.send_back_status = new fLogin.Flogin_Send_Back(Receive_SendBack_And_Do);
            form.ShowDialog();
            this.Show();
        }
        private void Load_Form_TableManager()
        {
            tbSearch.Text = null;
            Account_Count();
            string cbName = cbAccStatusList.Text;
            Load_AccountTabale_GridView(cbName, status);
            Load_RightMouse_Click_Menu();
        }
        private void Account_Count()
        {
            string name = cbAccStatusList.Text;
            if (name != "Chọn danh mục")
            {
                tbCount.Text = new On_Accounts_Dao().Counting_On_Acc_By_AccTypeName(name).ToString();
            }
            else
            {
                tbCount.Text = null;
            }
        }
        private void btnCountAccount_Click(object sender, EventArgs e)
        {
            tbCount.Text = null;
            tbSearch.Text = null;
            cbAccStatusList.Text = "Chọn danh mục";

            List<AccType_Model> listCount = new List<AccType_Model>();
            AccType_Model count = new AccType_Model();

            count.All = new On_Accounts_Dao().Counting_On_Acc_By_AccTypeName("Tất cả");
            count.Working = new On_Accounts_Dao().Counting_On_Acc_By_AccTypeName("Hoạt động");
            count.Limit = new On_Accounts_Dao().Counting_On_Acc_By_AccTypeName("Limit");
            count._180D = new On_Accounts_Dao().Counting_On_Acc_By_AccTypeName("180d");
            count.SetFailed = new On_Accounts_Dao().Counting_On_Acc_By_AccTypeName("Set fail");
            count.WrongInfo = new On_Accounts_Dao().Counting_On_Acc_By_AccTypeName("Sai thông tin");
            count.Verified = new On_Accounts_Dao().Counting_On_Acc_By_AccTypeName("Xác minh tk");
            count.WaitToSet = new On_Accounts_Dao().Counting_On_Acc_By_AccTypeName("Chưa set");

            listCount.Add(count);
            gvAccountTable.DataSource = listCount;
        }
        private void cbAccStatusList_SelectedValueChanged(object sender, EventArgs e)
        {
            tbSearch.Text = null;
            string cbName = cbAccStatusList.Text;
            Account_Count();
            Load_AccountTabale_GridView(cbName, status);
        }
        private void Reset_Table_Column(Table_Column_Status_Model stt)
        {
            string cbName = cbAccStatusList.Text;
            Load_AccountTabale_GridView(cbName, stt);
        }
        private void btnTableColumnSet_Click(object sender, EventArgs e)
        {
            bool checking = new On_Accounts_Dao().Check_On_Acc_Email_Existing(tbSearch.Text.ToString());
            if (checking == false)
            {
                tbSearch.Text = null;
            }
            Account_Count();
            fTableColumnSet form = new fTableColumnSet(status);
            form.sendStatus = new fTableColumnSet.Send_Back(Reset_Table_Column);
            form.ShowDialog();
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            var email = tbSearch.Text.ToString();
            if (!string.IsNullOrEmpty(email))
            {
                var result = new On_Accounts_Dao().Check_On_Acc_Email_Existing(email);
                if (result == true)
                {
                    tbCount.Text = null;
                    cbAccStatusList.Text = "Chọn danh mục";
                    Load_AccountTabale_GridView(cbAccStatusList.Text, status);
                }
                else
                {
                    MessageBox.Show("Email không đúng hoặc tài khoản chưa kính hoạt!");
                }
            }
        }

        public void Load_AccountTabale_GridView(string name, Table_Column_Status_Model st)
        {
            status = st;
            string searchValue = tbSearch.Text;
            if (!string.IsNullOrEmpty(searchValue))
            {
                List<Account> list = new List<Account>();
                var account = new On_Accounts_Dao().Get_On_Account_By_Email(searchValue);
                list.Add(account);
                data = new Account_Table_Data().Get_Account_Table_Data(list);
            }
            else
            {
                if (name == "Chọn danh mục")
                {
                    data = new List<AccountModel>();
                }
                else if (name == "Tất cả")
                {
                    List<Account> listAcc = new On_Accounts_Dao().Get_All_On_Accounts();
                    data = new Account_Table_Data().Get_Account_Table_Data(listAcc);
                }
                else if(name == "Profiles")
                {
                    PaypalDbContext db = new PaypalDbContext();
                    var listAcc = db.Accounts.Where(x => x.Profile_Save == true && x.Acc_ON_OFF == true).ToList();
                    data = new Account_Table_Data().Get_Account_Table_Data(listAcc);
                }
                else if (name == "Hoạt động" || name == "Limit" || name == "180d" || name == "Set fail" || name == "Sai thông tin" || name == "Xác minh tk" || name == "Chưa set")
                {
                    var listAcc = new On_Accounts_Dao().Get_List_On_Acc_By_AccType(name);
                    data = new Account_Table_Data().Get_Account_Table_Data(listAcc);
                }
            }
            gvAccountTable.DataSource = data;
            gvAccountTable.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            gvAccountTable.Columns[0].Width = 50;
            gvAccountTable.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            gvAccountTable.Columns[2].Width = 275;
            gvAccountTable.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            gvAccountTable.Columns[3].Width = 70;
            gvAccountTable.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            gvAccountTable.Columns[6].Width = 120;


            // Hiển thị cột theo tùy chọn
            if (status == null)
            {
                //gvAccountTable.Columns[0].Visible = false;
                gvAccountTable.Columns[1].Visible = false;
                //gvAccountTable.Columns[2].Visible = false;
                //gvAccountTable.Columns[3].Visible = false;
                gvAccountTable.Columns[4].Visible = false;
                gvAccountTable.Columns[5].Visible = false;
                //gvAccountTable.Columns[6].Visible = false;
                //gvAccountTable.Columns[7].Visible = false;
                //gvAccountTable.Columns[8].Visible = false;
                gvAccountTable.Columns[9].Visible = false;
                gvAccountTable.Columns[10].Visible = false;
                gvAccountTable.Columns[11].Visible = false;
                gvAccountTable.Columns[12].Visible = false;
                gvAccountTable.Columns[13].Visible = false;
                gvAccountTable.Columns[14].Visible = false;
                gvAccountTable.Columns[15].Visible = false;
                gvAccountTable.Columns[16].Visible = false;
                gvAccountTable.Columns[17].Visible = false;
                gvAccountTable.Columns[18].Visible = false;
                gvAccountTable.Columns[19].Visible = false;
                gvAccountTable.Columns[20].Visible = false;
                gvAccountTable.Columns[21].Visible = false;
                gvAccountTable.Columns[22].Visible = false;
                //gvAccountTable.Columns[23].Visible = false;
                gvAccountTable.Columns[24].Visible = false;
                gvAccountTable.Columns[25].Visible = false;
                gvAccountTable.Columns[26].Visible = false;
                gvAccountTable.Columns[27].Visible = false;
                gvAccountTable.Columns[28].Visible = false;
                gvAccountTable.Columns[29].Visible = false;
                gvAccountTable.Columns[30].Visible = false;
                //gvAccountTable.Columns[31].Visible = false;
                gvAccountTable.Columns[32].Visible = false;
                gvAccountTable.Columns[33].Visible = false;
                gvAccountTable.Columns[34].Visible = false;
                gvAccountTable.Columns[35].Visible = false;
                gvAccountTable.Columns[36].Visible = false;
                gvAccountTable.Columns[37].Visible = false;
                gvAccountTable.Columns[38].Visible = false;
                gvAccountTable.Columns[39].Visible = false;
                //gvAccountTable.Columns[40].Visible = false;
                gvAccountTable.Columns[41].Visible = false;
                gvAccountTable.Columns[42].Visible = false;
            }
            else
            {
                gvAccountTable.Columns[1].Visible = false;
                if (status.STT == true) { gvAccountTable.Columns[0].Visible = true; } else { gvAccountTable.Columns[0].Visible = false; }
                if (status.Email == true) { gvAccountTable.Columns[2].Visible = true; } else { gvAccountTable.Columns[2].Visible = false; }
                if (status.Balance == true) { gvAccountTable.Columns[3].Visible = true; } else { gvAccountTable.Columns[3].Visible = false; }
                if (status.TransactionTotal == true) { gvAccountTable.Columns[4].Visible = true; } else { gvAccountTable.Columns[4].Visible = false; }
                if (status.Profile_Created_Time == true) { gvAccountTable.Columns[5].Visible = true; } else { gvAccountTable.Columns[5].Visible = false; }
                if (status.Notification == true) { gvAccountTable.Columns[6].Visible = true; } else { gvAccountTable.Columns[6].Visible = false; }
                if (status.SessionResult == true) { gvAccountTable.Columns[7].Visible = true; } else { gvAccountTable.Columns[7].Visible = false; }
                if (status.UpdatedDateTime == true) { gvAccountTable.Columns[8].Visible = true; } else { gvAccountTable.Columns[8].Visible = false; }
                if (status.AccPassword == true) { gvAccountTable.Columns[9].Visible = true; } else { gvAccountTable.Columns[9].Visible = false; }
                if (status.TwoFA == true) { gvAccountTable.Columns[10].Visible = true; } else { gvAccountTable.Columns[10].Visible = false; }
                if (status.EmailPassword == true) { gvAccountTable.Columns[11].Visible = true; } else { gvAccountTable.Columns[11].Visible = false; }
                if (status.Proxy == true) { gvAccountTable.Columns[12].Visible = true; } else { gvAccountTable.Columns[12].Visible = false; }
                if (status.Profile == true) { gvAccountTable.Columns[13].Visible = true; } else { gvAccountTable.Columns[13].Visible = false; }
                if (status.ProfileId == true) { gvAccountTable.Columns[14].Visible = true; } else { gvAccountTable.Columns[14].Visible = false; }
                if (status.Email_2FA == true) { gvAccountTable.Columns[15].Visible = true; } else { gvAccountTable.Columns[15].Visible = false; }
                if (status.AccName == true) { gvAccountTable.Columns[16].Visible = true; } else { gvAccountTable.Columns[16].Visible = false; }
                if (status.AccBirthDay == true) { gvAccountTable.Columns[17].Visible = true; } else { gvAccountTable.Columns[17].Visible = false; }
                if (status.Address == true) { gvAccountTable.Columns[18].Visible = true; } else { gvAccountTable.Columns[18].Visible = false; }
                if (status.Phone == true) { gvAccountTable.Columns[19].Visible = true; } else { gvAccountTable.Columns[19].Visible = false; }
                if (status.BankCard == true) { gvAccountTable.Columns[20].Visible = true; } else { gvAccountTable.Columns[20].Visible = false; }
                if (status.SeQuestion1 == true) { gvAccountTable.Columns[21].Visible = true; } else { gvAccountTable.Columns[21].Visible = false; }
                if (status.SeQuestion2 == true) { gvAccountTable.Columns[22].Visible = true; } else { gvAccountTable.Columns[22].Visible = false; }
                if (status.AccType == true) { gvAccountTable.Columns[23].Visible = true; } else { gvAccountTable.Columns[23].Visible = false; }
                if (status.AccOtherType == true) { gvAccountTable.Columns[24].Visible = true; } else { gvAccountTable.Columns[24].Visible = false; }
                gvAccountTable.Columns[25].Visible = false;
                gvAccountTable.Columns[26].Visible = false;
                gvAccountTable.Columns[27].Visible = false;
                if (status.RecoveryEmail == true) { gvAccountTable.Columns[28].Visible = true; } else { gvAccountTable.Columns[28].Visible = false; }
                if (status.ForwordToEmail == true) { gvAccountTable.Columns[29].Visible = true; } else { gvAccountTable.Columns[29].Visible = false; }
                if (status.SecondEmail == true) { gvAccountTable.Columns[30].Visible = true; } else { gvAccountTable.Columns[30].Visible = false; }
                if (status.ProxyStatus == true) { gvAccountTable.Columns[31].Visible = true; } else { gvAccountTable.Columns[31].Visible = false; }
                if (status.Acc_ON_OFF == true) { gvAccountTable.Columns[32].Visible = true; } else { gvAccountTable.Columns[32].Visible = false; }
                gvAccountTable.Columns[33].Visible = false;
                gvAccountTable.Columns[34].Visible = false;
                gvAccountTable.Columns[35].Visible = false;
                gvAccountTable.Columns[36].Visible = false;
                gvAccountTable.Columns[37].Visible = false;
                gvAccountTable.Columns[38].Visible = false;
                gvAccountTable.Columns[39].Visible = false;
                if (status.Profile == true) { gvAccountTable.Columns[40].Visible = true; } else { gvAccountTable.Columns[40].Visible = false; }
                gvAccountTable.Columns[41].Visible = false;
                gvAccountTable.Columns[42].Visible = false;
            }
        }

        private void Load_RightMouse_Click_Menu()
        {
            // Tạo menu
            ContextMenuStrip menu = new ContextMenuStrip();
            //Auto login
            ToolStripItem acc_login = menu.Items.Add("Đăng nhập Paypal");
            acc_login.Click += new EventHandler(Acc_Login);
            ToolStripItem get_balance_status = menu.Items.Add("Lấy số dư - tình trạng Acc");
            get_balance_status.Click += new EventHandler(Get_Acc_Balance_Status);
            ToolStripItem update_name_address = menu.Items.Add("Cập nhật họ tên - địa chỉ");
            update_name_address.Click += new EventHandler(Name_Address_Update);
            ToolStripItem remove_card = menu.Items.Add("Xóa thẻ ngân hàng");
            remove_card.Click += new EventHandler(Remove_Card);
            ToolStripItem email_login = menu.Items.Add("Đăng nhập Email");
            email_login.Click += new EventHandler(Email_Login_Click);

            ToolStripMenuItem profile = (ToolStripMenuItem)menu.Items.Add("Quản lý Profile");
            ToolStripItem create_profile = profile.DropDownItems.Add("Tạo Profile");
            create_profile.Click += new EventHandler(Create_Profile);
            ToolStripItem delete_profile = profile.DropDownItems.Add("Xóa Profile");
            delete_profile.Click += new EventHandler(Delete_Profile);
            ToolStripItem delete_tem_profile = profile.DropDownItems.Add("Xóa Profile - Tạm");
            delete_tem_profile.Click += new EventHandler(Delete_Temprory_Profile);

            //Dòng COPY
            ToolStripMenuItem copy = (ToolStripMenuItem)menu.Items.Add("Copy thông tin"); 
            ToolStripItem copy_email = copy.DropDownItems.Add("Email");
            copy_email.Click += new EventHandler(Copy_Email);
            ToolStripItem copy_proxy = copy.DropDownItems.Add("Proxy");
            copy_proxy.Click += new EventHandler(Copy_Proxy);
            ToolStripItem copy_name = copy.DropDownItems.Add("Họ Tên");
            copy_name.Click += new EventHandler(Copy_Name);
            ToolStripItem copy_email_pass = copy.DropDownItems.Add("Email- Mật khẩu - Mk Email");
            copy_email_pass.Click += new EventHandler(Copy_Email_Pass);

            //Set tình trạng Acc bằng tay
            ToolStripMenuItem note = (ToolStripMenuItem)menu.Items.Add("Note trạng thái Acc");
            ToolStripItem info_wrong = note.DropDownItems.Add("Sai thông tin");
            info_wrong.Click += new EventHandler(Note_Wrong_Info_Click);
            ToolStripItem verified = note.DropDownItems.Add("Xác minh tk");
            verified.Click += new EventHandler(Note_AccVirified_Click);
            ToolStripItem account_180d = note.DropDownItems.Add("180d");
            account_180d.Click += new EventHandler(Note_180d);
            ToolStripItem account_limited = note.DropDownItems.Add("Limit");
            account_limited.Click += new EventHandler(Note_Limited);
            ToolStripItem account_work = note.DropDownItems.Add("Hoạt động");
            account_work.Click += new EventHandler(Note_Working);
            ToolStripItem note_not_set = note.DropDownItems.Add("Chưa set");
            note_not_set.Click += new EventHandler(Note_Not_Set);
            ToolStripItem note_set_fail = note.DropDownItems.Add("Set fail");
            note_set_fail.Click += new EventHandler(Note_Set_Fail);

            //Admin dán email forward
            ToolStripMenuItem admin_fwEmail = (ToolStripMenuItem)menu.Items.Add("Admin - Set Forward Email");
            ToolStripItem past_fwEmail = admin_fwEmail.DropDownItems.Add("Dán Email Forward (Gmail)");
            past_fwEmail.Click += new EventHandler(Past_ForwardEmail_Click);
            ToolStripItem past_fwEmail_profileId = admin_fwEmail.DropDownItems.Add("Dán Email Forward - ProfileId (Gmail)");
            past_fwEmail_profileId.Click += new EventHandler(Past_ForwardEmail_Gmail_ProfileId);
            ToolStripItem add_fwEmail = admin_fwEmail.DropDownItems.Add("Set - thêm Email Forward");
            add_fwEmail.Click += new EventHandler(Add_Fw_Email);

            //Admin setup only
            ToolStripMenuItem admin = (ToolStripMenuItem)menu.Items.Add("Admin - Cập nhật thông tin TK");
            ToolStripItem past_acc_password = admin.DropDownItems.Add("Dán mật khẩu Paypal");
            past_acc_password.Click += new EventHandler(Past_Acc_Password_Click);
            ToolStripItem past_new_email = admin.DropDownItems.Add("Dán - cập nhật Email mới");
            past_new_email.Click += new EventHandler(Past_New_Email_Click);
            ToolStripItem past_email_password = admin.DropDownItems.Add("Dán mật khẩu Email");
            past_email_password.Click += new EventHandler(Past_Email_Password_Click);

            ToolStripMenuItem admin_Proxy = (ToolStripMenuItem)menu.Items.Add("Admin - Cập nhật Proxy");
            ToolStripItem update_one_proxy = admin_Proxy.DropDownItems.Add("Dán 1 Proxy");
            update_one_proxy.Click += new EventHandler(Update_One_Proxy);
            ToolStripItem update_list_proxy = admin_Proxy.DropDownItems.Add("Dán list Proxy - Google Sheet");
            update_list_proxy.Click += new EventHandler(Update_List_Proxy);

            gvAccountTable.ContextMenuStrip = menu;
        }

        private void Acc_Login (object sender, EventArgs e) //DONE
        {
            var check_api = new Check_API().Start();
            if (check_api == true)
            {
                Int32 selectedRowCount = gvAccountTable.Rows.GetRowCount(DataGridViewElementStates.Selected);
                if (selectedRowCount > 0)
                {
                    this.WindowState = FormWindowState.Minimized;
                    try 
                    {
                        for (int i = (selectedRowCount - 1); i >= 0; i--)
                        {
                            UndectedChromeDriver driver = null;
                            var Acc01 = Get_Acc_Info(i);
                            try
                            {
                                bool next_step = false;
                                var result_01 = new Create_Profiles().Create(Acc01);
                                driver = result_01.Driver;
                                gvAccountTable.SelectedRows[i].Cells[31].Value = result_01.Value_03;
                                if (result_01.Status == true)
                                {
                                    next_step = true;
                                    gvAccountTable.SelectedRows[i].Cells[13].Value = "YES";
                                    if (!string.IsNullOrEmpty(result_01.Value_01)) { gvAccountTable.SelectedRows[i].Cells[14].Value = result_01.Value_01; }
                                    if (!string.IsNullOrEmpty(result_01.Value_02)) { gvAccountTable.SelectedRows[i].Cells[5].Value = result_01.Value_02; }
                                }
                                else
                                {
                                    gvAccountTable.SelectedRows[i].Cells[7].Value = result_01.Value_01;
                                }

                                if (next_step == true)
                                {
                                    var result_02 = new Paypal_Login_Controller().Paypal_Login(Acc01, result_01.Driver, checkBox_hold_on.Checked); //Đăng nhập Chrome
                                    driver = result_02.Driver;

                                    if (result_02.Status == true)
                                    {
                                        gvAccountTable.SelectedRows[i].Cells[7].Value = "Đăng nhập thành công--";
                                    }
                                    else
                                    {
                                        gvAccountTable.SelectedRows[i].Cells[7].Value = "Đăng nhập thất bại--";
                                    }
                                }

                                if (chromeOFF_Box.Checked == true && driver != null) { driver.Close(); }
                                try { driver.Quit(); } catch { }
                                try { driver.Dispose(); } catch { }
                            }
                            catch (Exception)
                            {
                                gvAccountTable.SelectedRows[i].Cells[7].Value += "Lỗi hoặc bị gián đoạn--";
                                if (chromeOFF_Box.Checked == true && driver != null) { driver.Close(); }
                                try { driver.Quit(); } catch { }
                                try { driver.Dispose(); } catch { }
                            }
                        }
                        this.WindowState = FormWindowState.Maximized;
                    }
                    catch 
                    {
                        this.WindowState = FormWindowState.Maximized; 
                    }                 
                }
                else { MessageBox.Show("Cần chọn full dòng, click vào cell đầu tiên của dòng để chọn full dòng!", "Thông báo"); }
            }
        }
        private void Get_Acc_Balance_Status (object sender, EventArgs e)
        {
            var check_api = new Check_API().Start();
            if (check_api == true)
            {
                Int32 selectedRowCount = gvAccountTable.Rows.GetRowCount(DataGridViewElementStates.Selected);
                if (selectedRowCount > 0)
                {
                    try 
                    {
                        this.WindowState = FormWindowState.Minimized;
                        for (int i = (selectedRowCount - 1); i >= 0; i--)
                        {
                            UndectedChromeDriver driver = null;
                            var Acc = Get_Acc_Info(i);
                            try
                            {
                                bool next_step = false;
                                var result_01 = new Create_Profiles().Create(Acc);
                                driver = result_01.Driver;
                                gvAccountTable.SelectedRows[i].Cells[31].Value = result_01.Value_03;
                                if (result_01.Status == true)
                                {
                                    next_step = true;
                                    gvAccountTable.SelectedRows[i].Cells[13].Value = "YES";
                                    if (!string.IsNullOrEmpty(result_01.Value_01)) { gvAccountTable.SelectedRows[i].Cells[14].Value = result_01.Value_01; }
                                    if (!string.IsNullOrEmpty(result_01.Value_02)) { gvAccountTable.SelectedRows[i].Cells[5].Value = result_01.Value_02; }
                                }
                                else
                                {
                                    gvAccountTable.SelectedRows[i].Cells[7].Value = result_01.Value_01;
                                }

                                if (next_step == true)
                                {
                                    var result_02 = new Paypal_Login_Controller().Paypal_Login(Acc, driver, checkBox_hold_on.Checked);
                                    driver = result_02.Driver;
                                    if (result_02.Status == false)
                                    {
                                        gvAccountTable.SelectedRows[i].Cells[7].Value = "Đăng nhập thất bại--";
                                        next_step = false;
                                    }
                                }

                                if (next_step == true)
                                {
                                    var result_03 = new Paypal_Get_Balance_Note_AccStatus_Controller().Get(Acc, driver);
                                    driver = result_03.Driver;
                                    if (result_03.Status == true)
                                    {
                                        gvAccountTable.SelectedRows[i].Cells[7].Value = "Đã cập nhật số dư, thông báo, tình trạng Acc--";
                                        var Acc03 = Get_Acc_Info(i);
                                        gvAccountTable.SelectedRows[i].Cells[3].Value = Acc03.Balance;
                                        gvAccountTable.SelectedRows[i].Cells[6].Value = Acc03.Notification;
                                        gvAccountTable.SelectedRows[i].Cells[23].Value = Acc03.AccType;
                                        gvAccountTable.SelectedRows[i].Cells[8].Value = Acc03.UpdatedDateTime;
                                    }
                                    else
                                    {
                                        gvAccountTable.SelectedRows[i].Cells[7].Value = "Đã cập nhật số dư, thông báo, tình trạng Acc thất bại--";
                                    }
                                }

                                if (chromeOFF_Box.Checked == true && driver != null) { driver.Close(); }
                                try { driver.Quit(); } catch { }
                                try { driver.Dispose(); } catch { }
                            }
                            catch (Exception)
                            {
                                gvAccountTable.SelectedRows[i].Cells[7].Value += "Lỗi hoặc bị gián đoạn--";
                                if (chromeOFF_Box.Checked == true && driver != null) { driver.Close(); }
                                try { driver.Quit(); } catch { }
                                try { driver.Dispose(); } catch { }
                            }
                        }
                        try { new Google_Sheet_Controller().Update_Database(); }
                        catch
                        {
                            MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo", MessageBoxButtons.OK,
                                        MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                        }
                        this.WindowState = FormWindowState.Maximized;
                    }
                    catch 
                    {
                        try { new Google_Sheet_Controller().Update_Database(); }
                        catch
                        {
                            MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo", MessageBoxButtons.OK,
                                        MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                        }
                        this.WindowState = FormWindowState.Maximized;
                    }  
                }
                else { MessageBox.Show("Cần chọn full dòng, click vào cell đầu tiên của dòng để chọn full dòng!", "Thông báo"); }
            }
        }
        private void Name_Address_Update (object sender, EventArgs e)
        {
            var check_api = new Check_API().Start();
            if (check_api == true)
            {
                Int32 selectedRowCount = gvAccountTable.Rows.GetRowCount(DataGridViewElementStates.Selected);
                if (selectedRowCount > 0)
                {
                    try
                    {
                        this.WindowState = FormWindowState.Minimized;
                        for (int i = (selectedRowCount - 1); i >= 0; i--)
                        {
                            UndectedChromeDriver driver = null;
                            var Acc = Get_Acc_Info(i);
                            try
                            {
                                bool next_step = false;
                                var result_01 = new Create_Profiles().Create(Acc);
                                driver = result_01.Driver;
                                gvAccountTable.SelectedRows[i].Cells[31].Value = result_01.Value_03;
                                if (result_01.Status == true)
                                {
                                    next_step = true;
                                    gvAccountTable.SelectedRows[i].Cells[13].Value = "YES";
                                    if (!string.IsNullOrEmpty(result_01.Value_01)) { gvAccountTable.SelectedRows[i].Cells[14].Value = result_01.Value_01; }
                                    if (!string.IsNullOrEmpty(result_01.Value_02)) { gvAccountTable.SelectedRows[i].Cells[5].Value = result_01.Value_02; }
                                }
                                else
                                {
                                    gvAccountTable.SelectedRows[i].Cells[7].Value = result_01.Value_01;
                                }

                                if (next_step == true)
                                {
                                    var result_02 = new Paypal_Login_Controller().Paypal_Login(Acc, driver, checkBox_hold_on.Checked);
                                    driver = result_02.Driver;
                                    if (result_02.Status == false)
                                    {
                                        gvAccountTable.SelectedRows[i].Cells[7].Value = "Đăng nhập thất bại--";
                                        next_step = false;
                                    }
                                }

                                if (next_step == true)
                                {
                                    var result_03 = new Paypal_Get_Name_Address_Controller().Get_Name_Address(Acc, driver);
                                    driver = result_03.Driver;
                                    if (result_03.Status == true)
                                    {
                                        gvAccountTable.SelectedRows[i].Cells[7].Value = "Đã cập nhật họ tên, địa chỉ--";
                                        gvAccountTable.SelectedRows[i].Cells[16].Value = result_03.Value_01;
                                        gvAccountTable.SelectedRows[i].Cells[18].Value = result_03.Value_02;
                                        gvAccountTable.SelectedRows[i].Cells[8].Value = result_03.Value_03;
                                    }
                                    else
                                    {
                                        gvAccountTable.SelectedRows[i].Cells[7].Value = "Cập nhật họ tên, địa chỉ thất bại--";
                                    }
                                }

                                if (chromeOFF_Box.Checked == true && driver != null) { driver.Close(); }
                                try { driver.Quit(); } catch { }
                                try { driver.Dispose(); } catch { }
                            }
                            catch (Exception)
                            {
                                gvAccountTable.SelectedRows[i].Cells[7].Value += "Lỗi hoặc bị gián đoạn--";
                                if (chromeOFF_Box.Checked == true && driver != null) { driver.Close(); }
                                try { driver.Quit(); } catch { }
                                try { driver.Dispose(); } catch { }
                            }
                        }
                        try { new Google_Sheet_Controller().Update_Database(); }
                        catch
                        {
                            MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo", MessageBoxButtons.OK,
                                        MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                        }
                        this.WindowState = FormWindowState.Maximized;
                    }
                    catch
                    {
                        try { new Google_Sheet_Controller().Update_Database(); }
                        catch
                        {
                            MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo", MessageBoxButtons.OK,
                                        MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                        }
                        this.WindowState = FormWindowState.Maximized;
                    }
                }
                else { MessageBox.Show("Cần chọn full dòng, click vào cell đầu tiên của dòng để chọn full dòng!", "Thông báo"); }
            }
        }
        private void Remove_Card (object sender, EventArgs e) 
        {
            var check_api = new Check_API().Start();
            if (check_api == true)
            {
                Int32 selectedRowCount = gvAccountTable.Rows.GetRowCount(DataGridViewElementStates.Selected);
                if (selectedRowCount > 0)
                {
                    try
                    {
                        this.WindowState = FormWindowState.Minimized;
                        for (int i = (selectedRowCount - 1); i >= 0; i--)
                        {
                            UndectedChromeDriver driver = null;
                            var Acc = Get_Acc_Info(i);
                            try
                            {
                                bool next_step = false;
                                var result_01 = new Create_Profiles().Create(Acc);
                                driver = result_01.Driver;
                                gvAccountTable.SelectedRows[i].Cells[31].Value = result_01.Value_03;
                                if (result_01.Status == true)
                                {
                                    next_step = true;
                                    gvAccountTable.SelectedRows[i].Cells[13].Value = "YES";
                                    if (!string.IsNullOrEmpty(result_01.Value_01)) { gvAccountTable.SelectedRows[i].Cells[14].Value = result_01.Value_01; }
                                    if (!string.IsNullOrEmpty(result_01.Value_02)) { gvAccountTable.SelectedRows[i].Cells[5].Value = result_01.Value_02; }
                                }
                                else
                                {
                                    gvAccountTable.SelectedRows[i].Cells[7].Value = result_01.Value_01;
                                }

                                if (next_step == true)
                                {
                                    var result_02 = new Paypal_Login_Controller().Paypal_Login(Acc, driver, checkBox_hold_on.Checked);
                                    driver = result_02.Driver;
                                    if (result_02.Status == false)
                                    {
                                        gvAccountTable.SelectedRows[i].Cells[7].Value = "Đăng nhập thất bại--";
                                        next_step = false;
                                    }
                                }

                                if (next_step == true)
                                {
                                    var result_03 = new Remove_Card_Controller().Remove(Acc, driver);
                                    driver = result_03.Driver;
                                    if (result_03.Status == true)
                                    {
                                        gvAccountTable.SelectedRows[i].Cells[7].Value = result_03.Value_01;
                                    }
                                    else
                                    {
                                        gvAccountTable.SelectedRows[i].Cells[7].Value = result_03.Value_01;
                                    }
                                }

                                if (chromeOFF_Box.Checked == true && driver != null) { driver.Close(); }
                                try { driver.Quit(); } catch { }
                                try { driver.Dispose(); } catch { }
                            }
                            catch (Exception)
                            {
                                gvAccountTable.SelectedRows[i].Cells[7].Value += "Lỗi hoặc bị gián đoạn--";
                                if (chromeOFF_Box.Checked == true && driver != null) { driver.Close(); }
                                try { driver.Quit(); } catch { }
                                try { driver.Dispose(); } catch { }
                            }
                        }
                        this.WindowState = FormWindowState.Maximized;
                    }
                    catch
                    {
                        this.WindowState = FormWindowState.Maximized;
                    }
                }
                else { MessageBox.Show("Cần chọn full dòng, click vào cell đầu tiên của dòng để chọn full dòng!", "Thông báo"); }
            }
        }
        private void Email_Login_Click(object sender, EventArgs e) //DONE
        {
            var check_api = new Check_API().Start();
            if (check_api == true)
            {
                Int32 selectedRowCount = gvAccountTable.Rows.GetRowCount(DataGridViewElementStates.Selected);
                if (selectedRowCount > 0)
                {
                    this.WindowState = FormWindowState.Minimized;
                    try
                    {
                        for (int i = (selectedRowCount - 1); i >= 0; i--)
                        {
                            UndectedChromeDriver driver = null;
                            var Acc01 = Get_Acc_Info(i);
                            try
                            {
                                if(Acc01.EmailType == "gmail.com" || Acc01.EmailType == "hotmail.com" || Acc01.EmailType == "outlook.com") 
                                {
                                    bool next_step = false;
                                    var result_01 = new Create_Profiles().Create(Acc01);
                                    driver = result_01.Driver;
                                    gvAccountTable.SelectedRows[i].Cells[31].Value = result_01.Value_03;
                                    if (result_01.Status == true)
                                    {
                                        next_step = true;
                                        gvAccountTable.SelectedRows[i].Cells[13].Value = "YES";
                                        if (!string.IsNullOrEmpty(result_01.Value_01)) { gvAccountTable.SelectedRows[i].Cells[14].Value = result_01.Value_01; }
                                        if (!string.IsNullOrEmpty(result_01.Value_02)) { gvAccountTable.SelectedRows[i].Cells[5].Value = result_01.Value_02; }
                                    }
                                    else
                                    {
                                        gvAccountTable.SelectedRows[i].Cells[7].Value = result_01.Value_01;
                                    }

                                    if (next_step == true)
                                    {
                                        if(Acc01.EmailType == "gmail.com") 
                                        {
                                            var result_02 = new Gmail_Login().Login(Acc01, driver, checkBox_hold_on.Checked);
                                            driver = result_02.Driver;
                                            if(result_02.Status != true) { gvAccountTable.SelectedRows[i].Cells[7].Value = "Đăng nhập Email thất bại--"; }
                                        }
                                        else 
                                        {
                                            var result_02 = new Login_Hotmail().Login(Acc01, driver);
                                            driver = result_02.Driver;
                                            if (result_02.Status != true) { gvAccountTable.SelectedRows[i].Cells[7].Value = "Đăng nhập Email thất bại--"; }
                                        }
                                    }

                                    if (chromeOFF_Box.Checked == true && driver != null) { driver.Close(); }
                                    try { driver.Quit(); } catch { }
                                    try { driver.Dispose(); } catch { }
                                }
                                else 
                                {
                                    gvAccountTable.SelectedRows[i].Cells[7].Value = "Không thể đăng nhập đuôi email này--";
                                }
                            }
                            catch (Exception)
                            {
                                gvAccountTable.SelectedRows[i].Cells[7].Value += "Lỗi hoặc bị gián đoạn--";
                                if (chromeOFF_Box.Checked == true && driver != null) { driver.Close(); }
                                try { driver.Quit(); } catch { }
                                try { driver.Dispose(); } catch { }
                            }
                        }
                        this.WindowState = FormWindowState.Maximized;
                    }
                    catch
                    {
                        this.WindowState = FormWindowState.Maximized;
                    }
                }
                else { MessageBox.Show("Cần chọn full dòng, click vào cell đầu tiên của dòng để chọn full dòng!", "Thông báo"); }
            }
        }

        private void Create_Profile(object sender, EventArgs e)
        {
            var check_api = new Check_API().Start();
            if (check_api == true)
            {
                Int32 selectedRowCount = gvAccountTable.Rows.GetRowCount(DataGridViewElementStates.Selected);
                if (selectedRowCount > 0)
                {
                    try
                    {
                        this.WindowState = FormWindowState.Minimized;
                        for (int i = (selectedRowCount - 1); i >= 0; i--)
                        {
                            UndectedChromeDriver driver = null;
                            var Acc = Get_Acc_Info(i);
                            try
                            {
                                PaypalDbContext db = new PaypalDbContext();
                                var db_account = db.Accounts.Where(x => x.ID == Acc.ID).FirstOrDefault();
                                if (Acc.Profile_Save != true)
                                {
                                    if (Acc.Profile == true )
                                    {
                                        db_account.Profile_Save = true;
                                        db.SaveChanges();
                                        gvAccountTable.SelectedRows[i].Cells[40].Value = "YES";
                                        gvAccountTable.SelectedRows[i].Cells[7].Value = "Tạo profile thành công--";
                                    }
                                    else
                                    {
                                        var result_01 = new Create_Profiles().Create(Acc);
                                        driver = result_01.Driver;
                                        if (result_01.Status == true)
                                        {
                                            db_account.Profile_Save = true;
                                            db.SaveChanges();
                                            gvAccountTable.SelectedRows[i].Cells[13].Value = "YES";
                                            gvAccountTable.SelectedRows[i].Cells[14].Value = result_01.Value_01;
                                            gvAccountTable.SelectedRows[i].Cells[40].Value = "YES";
                                            gvAccountTable.SelectedRows[i].Cells[7].Value = "Tạo profile thành công--";
                                        }
                                        else
                                        {
                                            gvAccountTable.SelectedRows[i].Cells[7].Value = result_01.Value_01;
                                        }
                                    }
                                }
                                else
                                {
                                    gvAccountTable.SelectedRows[i].Cells[7].Value = "Đã tạo profile--";
                                }


                                if (chromeOFF_Box.Checked == true && driver != null) { driver.Close(); }
                                try { driver.Quit(); } catch { }
                                try { driver.Dispose(); } catch { }

                            }
                            catch (Exception)
                            {
                                gvAccountTable.SelectedRows[i].Cells[7].Value += "Lỗi hoặc bị gián đoạn--";
                                if (chromeOFF_Box.Checked == true && driver != null) { driver.Close(); }
                                try { driver.Quit(); } catch { }
                                try { driver.Dispose(); } catch { }
                            }
                        }
                        try { new Google_Sheet_Controller().Update_Database(); }
                        catch
                        {
                            MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo", MessageBoxButtons.OK,
                                        MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                        }
                        this.WindowState = FormWindowState.Maximized;
                    }
                    catch
                    {
                        try { new Google_Sheet_Controller().Update_Database(); }
                        catch
                        {
                            MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo", MessageBoxButtons.OK,
                                        MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                        }
                        this.WindowState = FormWindowState.Maximized;
                    }
                }
                else { MessageBox.Show("Cần chọn full dòng, click vào cell đầu tiên của dòng để chọn full dòng!", "Thông báo"); }
            }
        }
        private void Delete_Profile(object sender, EventArgs e)
        {
            var check_api = new Check_API().Start();
            if (check_api == true)
            {
                Int32 selectedRowCount = gvAccountTable.Rows.GetRowCount(DataGridViewElementStates.Selected);
                if (selectedRowCount > 0)
                {
                    DialogResult result = MessageBox.Show("- Bạn chắc chắn muốn xóa Profile?", "Xác nhận", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        try
                        {
                            for (int i = (selectedRowCount - 1); i >= 0; i--)
                            {
                                UndectedChromeDriver driver = null;
                                var Acc = Get_Acc_Info(i);
                                try
                                {
                                    PaypalDbContext db = new PaypalDbContext();
                                    var db_account = db.Accounts.Where(x => x.ID == Acc.ID).FirstOrDefault();
                                    if (Acc.Profile_Save == true)
                                    {
                                        new GPMLoginAPI().Delete(Acc.ProfileId);

                                        db_account.Profile = false;
                                        db_account.Profile_Created_Time = null;
                                        db_account.ProfileId = null;
                                        db_account.Profile_Save = false;
                                        db.SaveChanges();

                                        gvAccountTable.SelectedRows[i].Cells[5].Value = "";
                                        gvAccountTable.SelectedRows[i].Cells[13].Value = "NO";
                                        gvAccountTable.SelectedRows[i].Cells[14].Value = "";
                                        gvAccountTable.SelectedRows[i].Cells[40].Value = "NO";
                                        gvAccountTable.SelectedRows[i].Cells[7].Value = "Đã xóa profile--";
                                    }
                                    else
                                    {
                                        gvAccountTable.SelectedRows[i].Cells[7].Value = "Chưa tạo profile--";
                                    }

                                    if (chromeOFF_Box.Checked == true && driver != null) { driver.Close(); }
                                    try { driver.Quit(); } catch { }
                                    try { driver.Dispose(); } catch { }

                                }
                                catch (Exception)
                                {
                                    gvAccountTable.SelectedRows[i].Cells[7].Value += "Lỗi hoặc bị gián đoạn--";
                                    if (chromeOFF_Box.Checked == true && driver != null) { driver.Close(); }
                                    try { driver.Quit(); } catch { }
                                    try { driver.Dispose(); } catch { }
                                }
                            }
                            try { new Google_Sheet_Controller().Update_Database(); }
                            catch
                            {
                                MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo", MessageBoxButtons.OK,
                                            MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                            }
                        }
                        catch
                        {
                            try { new Google_Sheet_Controller().Update_Database(); }
                            catch
                            {
                                MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo", MessageBoxButtons.OK,
                                            MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                            }
                        }
                    }
                }
                else { MessageBox.Show("Cần chọn full dòng, click vào cell đầu tiên của dòng để chọn full dòng!", "Thông báo"); }
            }
        }
        private void Delete_Temprory_Profile(object sender, EventArgs e)
        {
            var check_api = new Check_API().Start();
            if (check_api == true)
            {
                Int32 selectedRowCount = gvAccountTable.Rows.GetRowCount(DataGridViewElementStates.Selected);
                if (selectedRowCount > 0)
                {
                    try
                    {
                        for (int i = (selectedRowCount - 1); i >= 0; i--)
                        {
                            UndectedChromeDriver driver = null;
                            var Acc = Get_Acc_Info(i);
                            try
                            {
                                PaypalDbContext db = new PaypalDbContext();
                                var db_account = db.Accounts.Where(x => x.ID == Acc.ID).FirstOrDefault();
                                if (Acc.Profile == true && Acc.Profile_Save != true)
                                {
                                    new GPMLoginAPI().Delete(Acc.ProfileId);
                                    db_account.Profile = false;
                                    db_account.Profile_Created_Time = null;
                                    db_account.ProfileId = null;
                                    db.SaveChanges();

                                    gvAccountTable.SelectedRows[i].Cells[5].Value = "";
                                    gvAccountTable.SelectedRows[i].Cells[13].Value = "NO";
                                    gvAccountTable.SelectedRows[i].Cells[14].Value = "";
                                    gvAccountTable.SelectedRows[i].Cells[7].Value = "Đã xóa--";
                                }
                                else
                                {
                                    gvAccountTable.SelectedRows[i].Cells[7].Value = "Chưa tạo profile tạm--";
                                }

                                if (chromeOFF_Box.Checked == true && driver != null) { driver.Close(); }
                                try { driver.Quit(); } catch { }
                                try { driver.Dispose(); } catch { }

                            }
                            catch (Exception)
                            {
                                gvAccountTable.SelectedRows[i].Cells[7].Value += "Lỗi hoặc bị gián đoạn--";
                                if (chromeOFF_Box.Checked == true && driver != null) { driver.Close(); }
                                try { driver.Quit(); } catch { }
                                try { driver.Dispose(); } catch { }
                            }
                        }
                        //try { new Google_Sheet_Controller().Update_Database(); }
                        //catch
                        //{
                        //    MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo", MessageBoxButtons.OK,
                        //                MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                        //}
                    }
                    catch
                    {
                        //try { new Google_Sheet_Controller().Update_Database(); }
                        //catch
                        //{
                        //    MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo", MessageBoxButtons.OK,
                        //                MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                        //}
                    }
                }
                else { MessageBox.Show("Cần chọn full dòng, click vào cell đầu tiên của dòng để chọn full dòng!", "Thông báo"); }
            }
        }

        private void Copy_Email(object sender, EventArgs e)
        {
            Int32 selectedRowCount =
            gvAccountTable.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount > 0)
            {
                string copy_email = gvAccountTable.SelectedRows[selectedRowCount - 1].Cells[2].Value.ToString();
                Clipboard.SetText(copy_email);
            }
            else { MessageBox.Show("Cần chọn full dòng, click vào cell đầu tiên của dòng để chọn full dòng!", "Thông báo"); }
        }
        private void Copy_Name(object sender, EventArgs e)
        {
            Int32 selectedRowCount =
            gvAccountTable.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount > 0)
            {
                string copy_name = "";
                try { } catch (Exception) { copy_name = gvAccountTable.SelectedRows[selectedRowCount - 1].Cells[16].Value.ToString(); }
                if (string.IsNullOrEmpty(copy_name))
                {
                    copy_name = "Chưa cập nhật họ tên";
                }
                Clipboard.SetText(copy_name);
            }
            else { MessageBox.Show("Cần chọn full dòng, click vào cell đầu tiên của dòng để chọn full dòng!", "Thông báo"); }
        }
        private void Copy_Proxy(object sender, EventArgs e)
        {
            Int32 selectedRowCount =
            gvAccountTable.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount > 0)
            {
                string proxy = gvAccountTable.SelectedRows[selectedRowCount - 1].Cells[12].Value.ToString();
                Clipboard.SetText(proxy);
            }
            else { MessageBox.Show("Cần chọn full dòng, click vào cell đầu tiên của dòng để chọn full dòng!", "Thông báo"); }
        }
        private void Copy_Email_Pass(object sender, EventArgs e)
        {
            Int32 selectedRowCount =
            gvAccountTable.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount > 0)
            {
                StringWriter strWriteLine = new StringWriter();
                string email = gvAccountTable.SelectedRows[selectedRowCount - 1].Cells[2].Value.ToString();
                string password = gvAccountTable.SelectedRows[selectedRowCount - 1].Cells[9].Value.ToString();
                string email_passowrd = "";
                try { email_passowrd = gvAccountTable.SelectedRows[selectedRowCount - 1].Cells[11].Value.ToString(); } catch (Exception) { }
                strWriteLine.Write(email + "|");
                strWriteLine.Write(password + "|");
                if (!string.IsNullOrEmpty(email_passowrd)) { strWriteLine.Write(email_passowrd); } else { strWriteLine.Write("Chưa có mk email"); }
                Clipboard.SetText(strWriteLine.ToString());
            }
            else { MessageBox.Show("Cần chọn full dòng, click vào cell đầu tiên của dòng để chọn full dòng!", "Thông báo"); }
        }

        private void Note_Wrong_Info_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 selectedRowCount =
                gvAccountTable.Rows.GetRowCount(DataGridViewElementStates.Selected);
                if (selectedRowCount > 0)
                {
                    try 
                    {
                        if (selectedRowCount > 1)
                        {
                            DialogResult result = MessageBox.Show("Tình trạng các hàng đang lựa chọn sẽ đổi thành 'Sai thông tin', bạn có muốn tiếp tục?", "Đổi tình trạng Acc", MessageBoxButtons.YesNo);
                            if (result == DialogResult.Yes)
                            {
                                for (int i = (selectedRowCount - 1); i >= 0; i--)
                                {
                                    ID_String_Model model = new ID_String_Model();
                                    model.ID = int.Parse(gvAccountTable.SelectedRows[i].Cells[1].Value.ToString());
                                    model.Str_Value = "Sai thông tin";
                                    new AccountDao().Update_AccType(model);
                                    gvAccountTable.SelectedRows[i].Cells[23].Value = "Sai thông tin";
                                }

                            }
                        }
                        else
                        {
                            ID_String_Model model = new ID_String_Model();
                            model.ID = int.Parse(gvAccountTable.SelectedRows[0].Cells[1].Value.ToString());
                            model.Str_Value = "Sai thông tin";
                            new AccountDao().Update_AccType(model);
                            gvAccountTable.SelectedRows[0].Cells[23].Value = "Sai thông tin";
                        }
                        try { new Google_Sheet_Controller().Update_Database(); }
                        catch
                        {
                            MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo", MessageBoxButtons.OK,
                                        MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                        }
                    }
                    catch 
                    {
                        try { new Google_Sheet_Controller().Update_Database(); }
                        catch
                        {
                            MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo", MessageBoxButtons.OK,
                                        MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                        }
                    } 
                }
                else { MessageBox.Show("Cần chọn full dòng, click vào cell đầu tiên của dòng để chọn full dòng!", "Thông báo"); }
            }
            catch (Exception) { }
        }
        private void Note_AccVirified_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 selectedRowCount =
                gvAccountTable.Rows.GetRowCount(DataGridViewElementStates.Selected);
                if (selectedRowCount > 0)
                {
                    try 
                    {
                        if (selectedRowCount > 1)
                        {
                            DialogResult result = MessageBox.Show("Tình trạng các hàng đang lựa chọn sẽ đổi thành 'Xác minh tk', bạn có muốn tiếp tục?", "Đổi tình trạng Acc", MessageBoxButtons.YesNo);
                            if (result == DialogResult.Yes)
                            {
                                for (int i = (selectedRowCount - 1); i >= 0; i--)
                                {
                                    ID_String_Model model = new ID_String_Model();
                                    model.ID = int.Parse(gvAccountTable.SelectedRows[i].Cells[1].Value.ToString());
                                    model.Str_Value = "Xác minh tk";
                                    new AccountDao().Update_AccType(model);
                                    gvAccountTable.SelectedRows[i].Cells[23].Value = "Xác minh tk";
                                }
                            }
                        }
                        else
                        {
                            ID_String_Model model = new ID_String_Model();
                            model.ID = int.Parse(gvAccountTable.SelectedRows[0].Cells[1].Value.ToString());
                            model.Str_Value = "Xác minh tk";
                            new AccountDao().Update_AccType(model);
                            gvAccountTable.SelectedRows[0].Cells[23].Value = "Xác minh tk";
                        }
                        try{ new Google_Sheet_Controller().Update_Database(); } 
                        catch {
                            MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo", MessageBoxButtons.OK,
                                        MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                        }
                    } 
                    catch 
                    {
                        try { new Google_Sheet_Controller().Update_Database(); }
                        catch
                        {
                            MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo", MessageBoxButtons.OK,
                                        MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                        }
                    }
                }
                else { MessageBox.Show("Cần chọn full dòng, click vào cell đầu tiên của dòng để chọn full dòng!", "Thông báo"); }
            }
            catch (Exception) { }
        }
        private void Note_180d(object sender, EventArgs e)
        {
            try
            {
                Int32 selectedRowCount =
                gvAccountTable.Rows.GetRowCount(DataGridViewElementStates.Selected);
                if (selectedRowCount > 0)
                {
                    try
                    {
                        if (selectedRowCount > 1)
                        {
                            DialogResult result = MessageBox.Show("Tình trạng các hàng đang lựa chọn sẽ đổi thành 'Xác minh tk', bạn có muốn tiếp tục?", "Đổi tình trạng Acc", MessageBoxButtons.YesNo);
                            if (result == DialogResult.Yes)
                            {
                                for (int i = (selectedRowCount - 1); i >= 0; i--)
                                {
                                    ID_String_Model model = new ID_String_Model();
                                    model.ID = int.Parse(gvAccountTable.SelectedRows[i].Cells[1].Value.ToString());
                                    model.Str_Value = "180d";
                                    new AccountDao().Update_AccType(model);
                                    gvAccountTable.SelectedRows[i].Cells[23].Value = "180d";
                                }
                            }
                        }
                        else
                        {
                            ID_String_Model model = new ID_String_Model();
                            model.ID = int.Parse(gvAccountTable.SelectedRows[0].Cells[1].Value.ToString());
                            model.Str_Value = "180d";
                            new AccountDao().Update_AccType(model);
                            gvAccountTable.SelectedRows[0].Cells[23].Value = "180d";
                        }
                        try { new Google_Sheet_Controller().Update_Database(); }
                        catch
                        {
                            MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo", MessageBoxButtons.OK,
                                        MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                        }
                    }
                    catch
                    {
                        try { new Google_Sheet_Controller().Update_Database(); }
                        catch
                        {
                            MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo", MessageBoxButtons.OK,
                                        MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                        }
                    }
                }
                else { MessageBox.Show("Cần chọn full dòng, click vào cell đầu tiên của dòng để chọn full dòng!", "Thông báo"); }
            }
            catch (Exception) { }
        }
        private void Note_Limited(object sender, EventArgs e)
        {
            try
            {
                Int32 selectedRowCount =
                gvAccountTable.Rows.GetRowCount(DataGridViewElementStates.Selected);
                if (selectedRowCount > 0)
                {
                    try
                    {
                        if (selectedRowCount > 1)
                        {
                            DialogResult result = MessageBox.Show("Tình trạng các hàng đang lựa chọn sẽ đổi thành 'Xác minh tk', bạn có muốn tiếp tục?", "Đổi tình trạng Acc", MessageBoxButtons.YesNo);
                            if (result == DialogResult.Yes)
                            {
                                for (int i = (selectedRowCount - 1); i >= 0; i--)
                                {
                                    ID_String_Model model = new ID_String_Model();
                                    model.ID = int.Parse(gvAccountTable.SelectedRows[i].Cells[1].Value.ToString());
                                    model.Str_Value = "Limit";
                                    new AccountDao().Update_AccType(model);
                                    gvAccountTable.SelectedRows[i].Cells[23].Value = "Limit";
                                }
                            }
                        }
                        else
                        {
                            ID_String_Model model = new ID_String_Model();
                            model.ID = int.Parse(gvAccountTable.SelectedRows[0].Cells[1].Value.ToString());
                            model.Str_Value = "Limit";
                            new AccountDao().Update_AccType(model);
                            gvAccountTable.SelectedRows[0].Cells[23].Value = "Limit";
                        }
                        try { new Google_Sheet_Controller().Update_Database(); }
                        catch
                        {
                            MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo", MessageBoxButtons.OK,
                                        MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                        }
                    }
                    catch
                    {
                        try { new Google_Sheet_Controller().Update_Database(); }
                        catch
                        {
                            MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo", MessageBoxButtons.OK,
                                        MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                        }
                    }
                }
                else { MessageBox.Show("Cần chọn full dòng, click vào cell đầu tiên của dòng để chọn full dòng!", "Thông báo"); }
            }
            catch (Exception) { }
        }
        private void Note_Working(object sender, EventArgs e)
        {
            try
            {
                Int32 selectedRowCount =
                gvAccountTable.Rows.GetRowCount(DataGridViewElementStates.Selected);
                if (selectedRowCount > 0)
                {
                    try
                    {
                        if (selectedRowCount > 1)
                        {
                            DialogResult result = MessageBox.Show("Tình trạng các hàng đang lựa chọn sẽ đổi thành 'Xác minh tk', bạn có muốn tiếp tục?", "Đổi tình trạng Acc", MessageBoxButtons.YesNo);
                            if (result == DialogResult.Yes)
                            {
                                for (int i = (selectedRowCount - 1); i >= 0; i--)
                                {
                                    ID_String_Model model = new ID_String_Model();
                                    model.ID = int.Parse(gvAccountTable.SelectedRows[i].Cells[1].Value.ToString());
                                    model.Str_Value = "Hoạt động";
                                    new AccountDao().Update_AccType(model);
                                    gvAccountTable.SelectedRows[i].Cells[23].Value = "Hoạt động";
                                }
                            }
                        }
                        else
                        {
                            ID_String_Model model = new ID_String_Model();
                            model.ID = int.Parse(gvAccountTable.SelectedRows[0].Cells[1].Value.ToString());
                            model.Str_Value = "Hoạt động";
                            new AccountDao().Update_AccType(model);
                            gvAccountTable.SelectedRows[0].Cells[23].Value = "Hoạt động";
                        }
                        try { new Google_Sheet_Controller().Update_Database(); }
                        catch
                        {
                            MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo", MessageBoxButtons.OK,
                                        MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                        }
                    }
                    catch
                    {
                        try { new Google_Sheet_Controller().Update_Database(); }
                        catch
                        {
                            MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo", MessageBoxButtons.OK,
                                        MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                        }
                    }
                }
                else { MessageBox.Show("Cần chọn full dòng, click vào cell đầu tiên của dòng để chọn full dòng!", "Thông báo"); }
            }
            catch (Exception) { }
        }
        private void Note_Not_Set(object sender, EventArgs e)
        {
            try
            {
                Int32 selectedRowCount =
                gvAccountTable.Rows.GetRowCount(DataGridViewElementStates.Selected);
                if (selectedRowCount > 0)
                {
                    try
                    {
                        if (selectedRowCount > 1)
                        {
                            DialogResult result = MessageBox.Show("Tình trạng các hàng đang lựa chọn sẽ đổi thành 'Xác minh tk', bạn có muốn tiếp tục?", "Đổi tình trạng Acc", MessageBoxButtons.YesNo);
                            if (result == DialogResult.Yes)
                            {
                                for (int i = (selectedRowCount - 1); i >= 0; i--)
                                {
                                    ID_String_Model model = new ID_String_Model();
                                    model.ID = int.Parse(gvAccountTable.SelectedRows[i].Cells[1].Value.ToString());
                                    model.Str_Value = "Chưa set";
                                    new AccountDao().Update_AccType(model);
                                    gvAccountTable.SelectedRows[i].Cells[23].Value = "Chưa set";
                                }
                            }
                        }
                        else
                        {
                            ID_String_Model model = new ID_String_Model();
                            model.ID = int.Parse(gvAccountTable.SelectedRows[0].Cells[1].Value.ToString());
                            model.Str_Value = "Chưa set";
                            new AccountDao().Update_AccType(model);
                            gvAccountTable.SelectedRows[0].Cells[23].Value = "Chưa set";
                        }
                        try { new Google_Sheet_Controller().Update_Database(); }
                        catch
                        {
                            MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo", MessageBoxButtons.OK,
                                        MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                        }
                    }
                    catch
                    {
                        try { new Google_Sheet_Controller().Update_Database(); }
                        catch
                        {
                            MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo", MessageBoxButtons.OK,
                                        MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                        }
                    }
                }
                else { MessageBox.Show("Cần chọn full dòng, click vào cell đầu tiên của dòng để chọn full dòng!", "Thông báo"); }
            }
            catch (Exception) { }
        }
        private void Note_Set_Fail(object sender, EventArgs e)
        {
            try
            {
                Int32 selectedRowCount =
                gvAccountTable.Rows.GetRowCount(DataGridViewElementStates.Selected);
                if (selectedRowCount > 0)
                {
                    try
                    {
                        if (selectedRowCount > 1)
                        {
                            DialogResult result = MessageBox.Show("Tình trạng các hàng đang lựa chọn sẽ đổi thành 'Xác minh tk', bạn có muốn tiếp tục?", "Đổi tình trạng Acc", MessageBoxButtons.YesNo);
                            if (result == DialogResult.Yes)
                            {
                                for (int i = (selectedRowCount - 1); i >= 0; i--)
                                {
                                    ID_String_Model model = new ID_String_Model();
                                    model.ID = int.Parse(gvAccountTable.SelectedRows[i].Cells[1].Value.ToString());
                                    model.Str_Value = "Set fail";
                                    new AccountDao().Update_AccType(model);
                                    gvAccountTable.SelectedRows[i].Cells[23].Value = "Set fail";
                                }
                            }
                        }
                        else
                        {
                            ID_String_Model model = new ID_String_Model();
                            model.ID = int.Parse(gvAccountTable.SelectedRows[0].Cells[1].Value.ToString());
                            model.Str_Value = "Set fail";
                            new AccountDao().Update_AccType(model);
                            gvAccountTable.SelectedRows[0].Cells[23].Value = "Set fail";
                        }
                        try { new Google_Sheet_Controller().Update_Database(); }
                        catch
                        {
                            MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo", MessageBoxButtons.OK,
                                        MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                        }
                    }
                    catch
                    {
                        try { new Google_Sheet_Controller().Update_Database(); }
                        catch
                        {
                            MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo", MessageBoxButtons.OK,
                                        MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                        }
                    }
                }
                else { MessageBox.Show("Cần chọn full dòng, click vào cell đầu tiên của dòng để chọn full dòng!", "Thông báo"); }
            }
            catch (Exception) { }
        }

        //Admin fw Email
        private void Past_ForwardEmail_Click(object sender, EventArgs e)
        {
            try
            {
                IDataObject iData = Clipboard.GetDataObject();
                var fw_email = (String)iData.GetData(DataFormats.Text);
                Int32 selectedRowCount =
                gvAccountTable.Rows.GetRowCount(DataGridViewElementStates.Selected);
                if (selectedRowCount > 0)
                {
                    for (int i = (selectedRowCount - 1); i >= 0; i--)
                    {
                        ID_String_Model id_string = new ID_String_Model();
                        if (iData.GetDataPresent(DataFormats.Text) == true)
                        {
                            gvAccountTable.SelectedRows[i].Cells[29].Value = fw_email;
                            id_string.Str_Value = fw_email;
                            id_string.ID = int.Parse(gvAccountTable.SelectedRows[i].Cells[1].Value.ToString());
                            new Set_New_Accounts_Dao().Past_ForwardEmail(id_string);
                        }
                    }
                    try { new Google_Sheet_Controller().Update_Database(); } catch { MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo"); }
                }
                else { MessageBox.Show("Cần chọn full dòng, click vào cell đầu tiên của dòng để chọn full dòng!", "Thông báo"); }
            }
            catch
            {
                try { new Google_Sheet_Controller().Update_Database(); } catch { MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo"); }
            }
        }
        private void Past_ForwardEmail_Gmail_ProfileId(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Bạn muốn dán ProfileId email forward (gmail)?", "Xác nhận!", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            if (dialogResult == DialogResult.Yes)
            {
                IDataObject iData = Clipboard.GetDataObject();
                var string_value = (String)iData.GetData(DataFormats.Text);
                if (iData.GetDataPresent(DataFormats.Text) == true)
                {
                    PaypalDbContext db = new PaypalDbContext();
                    db.Admins.Where(x => x.Name == "Forward_Email_Gmail").FirstOrDefault().Value = string_value;
                    db.SaveChanges();
                    StringWriter stringWriter = new StringWriter();
                    stringWriter.WriteLine("- Dán thành công!");
                    stringWriter.WriteLine("- ProfileId email forward (gmail) là: ");
                    stringWriter.WriteLine(string_value);
                    MessageBox.Show(stringWriter.ToString(), "Thông báo");
                }
            }
        }
        private void Add_Fw_Email(object sender, EventArgs e)
        {
            var check_api = new Check_API().Start();
            if (check_api == true)
            {
                Int32 selectedRowCount = gvAccountTable.Rows.GetRowCount(DataGridViewElementStates.Selected);
                if (selectedRowCount > 0)
                {
                    try
                    {
                        this.WindowState = FormWindowState.Minimized;
                        for (int i = (selectedRowCount - 1); i >= 0; i--)
                        {
                            UndectedChromeDriver driver = null;
                            var Acc = Get_Acc_Info(i);
                            try
                            {
                                if (Acc.EmailType == "gmail.com" || Acc.EmailType == "hotmail.com" || Acc.EmailType == "outlook.com")
                                {
                                    bool next_step = false;
                                    var result_01 = new Create_Profiles().Create(Acc);
                                    driver = result_01.Driver;
                                    gvAccountTable.SelectedRows[i].Cells[31].Value = result_01.Value_03;
                                    if (result_01.Status == true)
                                    {
                                        next_step = true;
                                        gvAccountTable.SelectedRows[i].Cells[13].Value = "YES";
                                        if (!string.IsNullOrEmpty(result_01.Value_01)) { gvAccountTable.SelectedRows[i].Cells[14].Value = result_01.Value_01; }
                                        if (!string.IsNullOrEmpty(result_01.Value_02)) { gvAccountTable.SelectedRows[i].Cells[5].Value = result_01.Value_02; }
                                    }
                                    else
                                    {
                                        gvAccountTable.SelectedRows[i].Cells[7].Value = result_01.Value_01;
                                    }

                                    if (next_step == true && Acc.EmailType == "gmail.com")
                                    {
                                        var result_02 = new Gmail_Login().Login(Acc, driver, checkBox_hold_on.Checked);
                                        driver = result_02.Driver;
                                        if (result_02.Status == true)
                                        {
                                            var result_07 = new Add_New_Forward_Email_Gmail().Add(Acc, driver);
                                            driver = result_07.Driver;
                                            if (result_07.Status == true)
                                            {
                                                gvAccountTable.SelectedRows[i].Cells[7].Value = "Đã add email forward--";
                                                gvAccountTable.SelectedRows[i].Cells[29].Value = Acc.ForwordToEmail;
                                                gvAccountTable.SelectedRows[i].Cells[37].Value = "YES";
                                            }
                                            else
                                            {
                                                gvAccountTable.SelectedRows[i].Cells[7].Value = "Add email forward fail--";
                                            }
                                        }
                                        else
                                        {
                                            gvAccountTable.SelectedRows[i].Cells[7].Value = "Đăng nhập email fail--";
                                        }
                                    }

                                    if (next_step == true && (Acc.EmailType == "hotmail.com" || Acc.EmailType == "outlook.com"))
                                    {
                                        var result_02 = new Login_Hotmail().Login(Acc, driver);
                                        driver = result_02.Driver;
                                        if (result_02.Status == true)
                                        {
                                            var result_03 = new Add_ForwardEmail_Hotmail().Add(Acc, driver, checkBox_hold_on.Checked);
                                            driver = result_03.Driver;
                                            if (result_03.Status == true)
                                            {
                                                gvAccountTable.SelectedRows[i].Cells[7].Value = "Đã add email forward--";
                                                gvAccountTable.SelectedRows[i].Cells[29].Value = Acc.ForwordToEmail;
                                                gvAccountTable.SelectedRows[i].Cells[37].Value = "YES";
                                            }
                                            else
                                            {
                                                gvAccountTable.SelectedRows[i].Cells[7].Value = "Add email forward fail--";
                                            }
                                        }
                                        else
                                        {
                                            gvAccountTable.SelectedRows[i].Cells[7].Value = "Đăng nhập email fail--";
                                        }
                                    }

                                    if (chromeOFF_Box.Checked == true && driver != null) { driver.Close(); }
                                    try { driver.Quit(); } catch { }
                                    try { driver.Dispose(); } catch { }
                                }
                                else
                                {
                                    gvAccountTable.SelectedRows[i].Cells[7].Value = "Chưa set up auto cho loại email này--";
                                }
                            }
                            catch (Exception)
                            {
                                gvAccountTable.SelectedRows[i].Cells[7].Value += "Lỗi hoặc bị gián đoạn--";
                                if (chromeOFF_Box.Checked == true && driver != null) { driver.Close(); }
                                try { driver.Quit(); } catch { }
                                try { driver.Dispose(); } catch { }
                            }
                        }
                        try { new Google_Sheet_Controller().Update_Database(); }
                        catch
                        {
                            MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo", MessageBoxButtons.OK,
                                        MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                        }
                        this.WindowState = FormWindowState.Maximized;
                    }
                    catch
                    {
                        try { new Google_Sheet_Controller().Update_Database(); }
                        catch
                        {
                            MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo", MessageBoxButtons.OK,
                                        MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                        }
                        this.WindowState = FormWindowState.Maximized;
                    }
                }
                else { MessageBox.Show("Cần chọn full dòng, click vào cell đầu tiên của dòng để chọn full dòng!", "Thông báo"); }
            }
        }

        private void Past_Acc_Password_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Bạn muốn dán khẩu paypal?", "Thông báo!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    IDataObject iData = Clipboard.GetDataObject();
                    var new_paypal_password = (String)iData.GetData(DataFormats.Text);
                    Int32 selectedRowCount = gvAccountTable.Rows.GetRowCount(DataGridViewElementStates.Selected);
                    if (selectedRowCount > 0)
                    {
                        if (iData.GetDataPresent(DataFormats.Text) == true)
                        {
                            PaypalDbContext db = new PaypalDbContext();
                            var AccID = int.Parse(gvAccountTable.SelectedRows[selectedRowCount - 1].Cells[1].Value.ToString());
                            var db_account = db.Accounts.Where(x => x.ID == AccID).FirstOrDefault();
                            db_account.AccPassword_Old += db_account.AccPassword + "--";
                            gvAccountTable.SelectedRows[selectedRowCount - 1].Cells[41].Value = db_account.AccPassword_Old;
                            db_account.AccPassword = new_paypal_password;
                            db.SaveChanges();
                            gvAccountTable.SelectedRows[selectedRowCount - 1].Cells[9].Value = new_paypal_password;

                            try { new Google_Sheet_Controller().Update_Database(); }
                            catch
                            {
                                MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo");
                            }
                        }
                    }
                    else { MessageBox.Show("Cần chọn full dòng, click vào cell đầu tiên của dòng để chọn full dòng!", "Thông báo"); }
                }
                catch (Exception) { }
            }
        }
        private void Past_New_Email_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Bạn muốn dán email?", "Thông báo!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    IDataObject iData = Clipboard.GetDataObject();
                    var new_email = (String)iData.GetData(DataFormats.Text);
                    Int32 selectedRowCount = gvAccountTable.Rows.GetRowCount(DataGridViewElementStates.Selected);
                    if (selectedRowCount > 0)
                    {
                        if (iData.GetDataPresent(DataFormats.Text) == true)
                        {
                            PaypalDbContext db = new PaypalDbContext();
                            var AccID = int.Parse(gvAccountTable.SelectedRows[selectedRowCount - 1].Cells[1].Value.ToString());
                            var db_account = db.Accounts.Where(x => x.ID == AccID).FirstOrDefault();
                            db_account.Email = new_email;
                            db_account.EmailType = new_email.Split('@')[1];
                            db.SaveChanges();
                            gvAccountTable.SelectedRows[selectedRowCount - 1].Cells[2].Value = new_email;
                            gvAccountTable.SelectedRows[selectedRowCount - 1].Cells[7].Value = "Đã dán email";
                            try { new Google_Sheet_Controller().Update_Database(); }
                            catch
                            {
                                MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo");
                            }
                        }
                    }
                    else { MessageBox.Show("Cần chọn full dòng, click vào cell đầu tiên của dòng để chọn full dòng!", "Thông báo"); }
                }
                catch (Exception) { }
            }
        }
        private void Past_Email_Password_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Bạn dán đổi mật khẩu email?", "Thông báo!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    IDataObject iData = Clipboard.GetDataObject();
                    var new_password = (String)iData.GetData(DataFormats.Text);
                    Int32 selectedRowCount = gvAccountTable.Rows.GetRowCount(DataGridViewElementStates.Selected);
                    if (selectedRowCount > 0)
                    {
                        if (iData.GetDataPresent(DataFormats.Text) == true)
                        {
                            PaypalDbContext db = new PaypalDbContext();
                            var AccID = int.Parse(gvAccountTable.SelectedRows[selectedRowCount - 1].Cells[1].Value.ToString());
                            var db_account = db.Accounts.Where(x => x.ID == AccID).FirstOrDefault();
                            db_account.EmailPassword_Old += db_account.EmailPassword + "--";
                            gvAccountTable.SelectedRows[selectedRowCount - 1].Cells[42].Value = db_account.EmailPassword_Old;
                            db_account.EmailPassword = new_password;
                            db.SaveChanges();
                            gvAccountTable.SelectedRows[selectedRowCount - 1].Cells[11].Value = new_password;

                            try { new Google_Sheet_Controller().Update_Database(); }
                            catch
                            {
                                MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo");
                            }
                        }
                    }
                    else { MessageBox.Show("Cần chọn full dòng, click vào cell đầu tiên của dòng để chọn full dòng!", "Thông báo"); }
                }
                catch (Exception) { }
            }
        }
    

        private void Update_One_Proxy(object sender, EventArgs e)
        {
            Int32 selected_row_count = gvAccountTable.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selected_row_count > 0)
            {
                IDataObject iData = Clipboard.GetDataObject();
                var proxy = (String)iData.GetData(DataFormats.Text);
                int AccID = int.Parse(gvAccountTable.SelectedRows[selected_row_count - 1].Cells[1].Value.ToString());
                PaypalDbContext db = new PaypalDbContext();
                var db_account = db.Accounts.Where(x => x.ID == AccID).FirstOrDefault();
                if (db_account.Profile == true)
                {
                    var check_api = new Check_API().Start();
                    if (check_api == true)
                    {
                        var result = new GPMLoginAPI().UpdateProxy(db_account.ProfileId, proxy);
                        if (result == true)
                        {
                            db_account.Proxy = proxy;
                            db.SaveChanges();
                            gvAccountTable.SelectedRows[selected_row_count - 1].Cells[12].Value = proxy;
                        }
                        else
                        {
                            gvAccountTable.SelectedRows[selected_row_count - 1].Cells[7].Value = "GMP-Longin cập nhật proxy fail--";
                        }
                    }
                }
                else
                {
                    db_account.Proxy = proxy;
                    db.SaveChanges();
                    gvAccountTable.SelectedRows[selected_row_count - 1].Cells[12].Value = proxy;
                }
                try { new Google_Sheet_Controller().Update_Database(); } catch { MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo"); }
            }
            else { MessageBox.Show("Cần chọn full dòng, click vào cell đầu tiên của dòng để chọn full dòng!", "Thông báo"); }
        }
        private void Update_List_Proxy(object sender, EventArgs e)
        {
            Int32 selected_row_count = gvAccountTable.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selected_row_count > 0)
            {
                var list_proxy = new Google_Sheet_Controller().Get_Proxy_List();
                int proxy_total = list_proxy.Count;

                if (proxy_total > 0)
                {
                    int index = 0;
                    if (proxy_total > selected_row_count) { index = selected_row_count; } else { index = proxy_total; }
                    int j = 0;
                    for (int i = selected_row_count - 1; i >= selected_row_count - index; i--)
                    {
                        string proxy = list_proxy[j][0].ToString();
                        int AccID = int.Parse(gvAccountTable.SelectedRows[i].Cells[1].Value.ToString());
                        PaypalDbContext db = new PaypalDbContext();
                        var db_account = db.Accounts.Where(x => x.ID == AccID).FirstOrDefault();
                        if (db_account.Profile == true)
                        {
                            var check_api = new Check_API().Start();
                            if (check_api == true)
                            {
                                var result = new GPMLoginAPI().UpdateProxy(db_account.ProfileId, proxy);
                                if (result == true)
                                {
                                    db_account.Proxy = proxy;
                                    db.SaveChanges();
                                    gvAccountTable.SelectedRows[i].Cells[12].Value = proxy;
                                }
                                else
                                {
                                    gvAccountTable.SelectedRows[i].Cells[7].Value = "GMP-Longin cập nhật proxy fail--";
                                }

                            }
                        }
                        else
                        {
                            db_account.Proxy = proxy;
                            db.SaveChanges();
                            gvAccountTable.SelectedRows[i].Cells[12].Value = proxy;
                        }
                        j++;
                    }
                }
                else
                {
                    MessageBox.Show("Google sheet không có dữ liệu", "Thông báo");
                }
                try { new Google_Sheet_Controller().Update_Database(); } catch { MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo"); }

            }
            else { MessageBox.Show("Cần chọn full dòng, click vào cell đầu tiên của dòng để chọn full dòng!", "Thông báo"); }
        }
    }
}