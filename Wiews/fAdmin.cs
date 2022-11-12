using Google.Apis.Sheets.v4;
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
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace PAYPAL.Wiews
{
    public partial class fAdmin : Form
    {
        //Khai báo biến google sheet
        private List<AccountModel> data = null;
        private FAdmin_Table_Column_Set_Model status = null;   
        private static string[] Scopes = { SheetsService.Scope.Spreadsheets };
        private static string ApplicationName = "GoogleSheetsHelper";
        
        public fAdmin()
        {
            InitializeComponent();
            this.CenterToScreen();
            Load_Form_TableManager();
            Load_RightMouse_Click_Menu();     
        }

        private Account Get_Acc_Info(int i)
        {
            Account Acc = null;
            int ID = int.Parse(gvAccountTable.SelectedRows[i].Cells[1].Value.ToString());
            using (PaypalDbContext db = new PaypalDbContext()) 
            {
                Acc = db.Accounts.Where(x => x.ID == ID).FirstOrDefault();
            }
            return Acc;
        }

        private void tem_profile_BTN_Click(object sender, EventArgs e)
        {
            var check_api = new Check_API().Start();
            if (check_api == true)
            {
                List<Account> accounts = null;
                PaypalDbContext db = new PaypalDbContext();
                string value = db.Admins.Where(x => x.Name == "Tem_Profiles_Litmited").FirstOrDefault().Value;
                int profile_limit = int.Parse(value);
                accounts = db.Accounts.Where(x => x.Profile == true && x.Profile_Save != true).OrderBy(x => x.Profile_Created_Time).ToList();
                if (accounts.Count > profile_limit)
                {
                    int del_profile = accounts.Count - profile_limit;
                    StringWriter stringWriter = new StringWriter();
                    stringWriter.WriteLine("- Số profile tạm hiện tại là: " + accounts.Count.ToString());
                    stringWriter.WriteLine("- Bạn muốn xóa : " + del_profile.ToString() + " profile?");
                    DialogResult result = MessageBox.Show(stringWriter.ToString(), "Xác nhận", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        for (int i = 0; i < del_profile; i++)
                        {
                            new Delete_Profiles().Delete(accounts[i].ID, accounts[i].ProfileId);
                        }
                    }
                }

                var accounts_01 = db.Accounts.Where(x => x.Profile == true && x.Profile_Save != true).OrderBy(x => x.Profile_Created_Time).ToList();
                data = new Account_Table_Data().Get_Account_Table_Data(accounts_01);
                cbAccStatusList.Text = "Profile tạm";
                Load_Form_TableManager();
                tb_count_counts_by_group.Text = accounts_01.Count.ToString();
            }
        }
        //Load giao diện
        private void Load_Form_TableManager()
        {
            tbSearch.Text = null;
            Counting_All_Accounts();
            Counting_Acounts_By_ComboxList_Name();
            Load_AccountTabale_GridView(cbAccStatusList.Text, status);        
        }
        private void btn_add_new_accounts_Click(object sender, EventArgs e)
        {
            cbAccStatusList.Text = "Chọn danh mục";
            Load_Form_TableManager();
            fAddNewAccounts form = new fAddNewAccounts();
            this.Hide();
            form.ShowDialog();
            this.Show();
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
        private void btnSearch_Click(object sender, EventArgs e)
        {
            var email = "";
            try { email = tbSearch.Text.ToString(); } catch { }
            if (!string.IsNullOrEmpty(email))
            {
                var result = new AccountDao().Get_Account_By_Email(email);
                if (result != null)
                {
                    //tbCount.Text = null;
                    cbAccStatusList.Text = "Chọn danh mục";
                    Counting_Acounts_By_ComboxList_Name();
                    Load_AccountTabale_GridView(cbAccStatusList.Text, status);
                }
                else
                {
                    MessageBox.Show("Email không đúng, hoặc chưa nhập hệ thống!");
                }
            }
        }    
        private void Counting_Acounts_By_ComboxList_Name()
        {
            int counting_result = new On_Accounts_Dao().Counting_All_Acc_By_CboxName(cbAccStatusList.Text);
            tb_count_counts_by_group.Text = counting_result.ToString();
        }
        private void Counting_All_Accounts()
        {
            List<Admin_Accounts_Count_Model> model_list = new List<Admin_Accounts_Count_Model>();
            Admin_Accounts_Count_Model model = new Admin_Accounts_Count_Model();
            model.All_Accounts = new On_Accounts_Dao().Counting_All_Acc_By_CboxName("Tất cả");
            model.New_Accounts = new On_Accounts_Dao().Counting_All_Acc_By_CboxName("Acc mới");
            model.Old_Accounts = new On_Accounts_Dao().Counting_All_Acc_By_CboxName("Acc cũ");
            model.Old_Working = new On_Accounts_Dao().Counting_All_Acc_By_CboxName("Hoạt động");
            model.Old_Limit = new On_Accounts_Dao().Counting_All_Acc_By_CboxName("Limit");
            model.Old_180D = new On_Accounts_Dao().Counting_All_Acc_By_CboxName("180d");
            model.Old_SetFailed = new On_Accounts_Dao().Counting_All_Acc_By_CboxName("Set fail");
            model.Old_WrongInfo = new On_Accounts_Dao().Counting_All_Acc_By_CboxName("Sai thông tin");
            model.Old_Verified = new On_Accounts_Dao().Counting_All_Acc_By_CboxName("Xác minh tk");
            model.Old_WaitToSet = new On_Accounts_Dao().Counting_All_Acc_By_CboxName("Chưa set");
            model_list.Add(model);
            grv_acounts_count.DataSource = model_list;
        }
        //Thao tác trên menu giao diện
        private void delete_account_BTN_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn muốn xóa tài khoản?", "Thông báo!", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                var check_api = new Check_API().Start();
                if (check_api == true)
                {
                    Int32 selectedRowCount = gvAccountTable.Rows.GetRowCount(DataGridViewElementStates.Selected);
                    if (selectedRowCount > 0)
                    {
                        List<Account> accounts_list = new List<Account>();
                        try
                        {
                            for (int i = selectedRowCount - 1; i >= 0; i--)
                            {
                                int AccID = int.Parse(gvAccountTable.SelectedRows[i].Cells[1].Value.ToString());
                                PaypalDbContext db = new PaypalDbContext();
                                var Acc = db.Accounts.Where(x => x.ID == AccID).FirstOrDefault();
                                if (Acc.Profile == true)
                                {
                                    try
                                    {
                                        new Delete_Profiles().Delete(Acc.ID, Acc.ProfileId);
                                        accounts_list.Add(Acc);
                                        db.Accounts.Remove(Acc);
                                        db.SaveChanges();
                                    }
                                    catch
                                    {
                                        MessageBox.Show("Lỗi - vui lòng kiểm tra lại kết nỗi GMP-Login", "Thông báo");
                                    }
                                }
                                else
                                {
                                    accounts_list.Add(Acc);
                                    db.Accounts.Remove(Acc);
                                    db.SaveChanges();
                                }
                            }

                            if (accounts_list.Count > 0) 
                            { 
                                new Save_Del_Acounts().Save(accounts_list);
                                try
                                {
                                    if (accounts_list.Count > 0) { new Google_Sheet_Controller().Save_Del_Accounts(accounts_list); }
                                    new Google_Sheet_Controller().Update_Database();
                                    MessageBox.Show("Đã xóa!", "Thông báo");
                                }
                                catch { MessageBox.Show("- Cập nhật google sheet lỗi", "Thông báo"); }
                                Load_Form_TableManager();
                            }
                        }
                        catch
                        {
                            if (accounts_list.Count > 0) 
                            { 
                                new Save_Del_Acounts().Save(accounts_list);
                                try
                                {
                                    if (accounts_list.Count > 0) { new Google_Sheet_Controller().Save_Del_Accounts(accounts_list); }
                                    new Google_Sheet_Controller().Update_Database();
                                }
                                catch { MessageBox.Show("- Cập nhật google sheet lỗi", "Thông báo"); }
                                Load_Form_TableManager();
                            }
                                                     
                        }
                    }
                }
            }
        }
        private void back_to_fmanager_Click(object sender, EventArgs e)
        {
            fTableManager form = new fTableManager();
            this.Hide();
            form.ShowDialog();
            this.Close();
        }
        private void cbAccStatusList_SelectedValueChanged(object sender, EventArgs e)
        {
            tbSearch.Text = null;
            string cbName = cbAccStatusList.Text;
            Counting_Acounts_By_ComboxList_Name();
            Load_AccountTabale_GridView(cbName, status);
        }
        private void Reset_Table_Column(FAdmin_Table_Column_Set_Model stt)
        {
            string cbName = cbAccStatusList.Text;
            Load_AccountTabale_GridView(cbName, stt);
        }
        private void btnTableColumnSet_Click(object sender, EventArgs e)
        {
            var email = tbSearch.Text.ToString();
            bool result = true;
            if (!string.IsNullOrEmpty(email))
            {
                result = new AccountDao().Check_Email_Existing(email);
            }
            if (result == false)
            {
                tbSearch.Text = null;
            }
            fAdmin_Table_Column_Set form = new fAdmin_Table_Column_Set(status);
            form.sendStatus = new fAdmin_Table_Column_Set.Send_Back(Reset_Table_Column);
            form.ShowDialog();
        }
        //Load data gridview
        public void Load_AccountTabale_GridView(string name, FAdmin_Table_Column_Set_Model st)
        {
            status = st;
            string searchValue = tbSearch.Text;
            if (!string.IsNullOrEmpty(searchValue))
            {
                cbAccStatusList.Text = "Chọn danh mục";
                var account = new AccountDao().Get_Account_By_Email(searchValue);
                if(account != null) 
                {
                    List<Account> list = new List<Account>();
                    list.Add(account);
                    data = new Account_Table_Data().Get_Account_Table_Data(list);
                }
                else 
                {
                    data = new List<AccountModel>();
                }               
            }
            else
            {
                if (name == "Chọn danh mục")
                {
                    data = new List<AccountModel>();
                }
                else if (name == "Tất cả")
                {
                    List<Account> listAcc = new AccountDao().Get_All_Accounts();
                    data = new Account_Table_Data().Get_Account_Table_Data(listAcc);
                }
                else if (name == "Acc cũ")
                {
                    List<Account> listAcc = new On_Accounts_Dao().Get_All_On_Accounts();
                    data = new Account_Table_Data().Get_Account_Table_Data(listAcc);
                }
                else if (name == "Hoạt động" || name == "Limit" || name == "180d" || name == "Set fail" || name == "Sai thông tin" || name == "Xác minh tk" || name == "Chưa set")
                {
                    var listAcc = new On_Accounts_Dao().Get_List_On_Acc_By_AccType(name);
                    data = new Account_Table_Data().Get_Account_Table_Data(listAcc);
                }
                else if (name == "Acc mới")
                {
                    var listAcc = new OFF_Accounts_Dao().Get_All_OFF_Accounts();
                    data = new Account_Table_Data().Get_Account_Table_Data(listAcc);
                }
                else if(name == "Profiles") 
                {
                    PaypalDbContext db = new PaypalDbContext();
                    var listAcc = db.Accounts.Where(x => x.Profile_Save == true).ToList();
                    data = new Account_Table_Data().Get_Account_Table_Data(listAcc);
                }
            }
            gvAccountTable.DataSource = data;
            // Hiển thị cột theo tùy chọn

            if (status == null)
            {
                //gvAccountTable.Columns[0].Visible = false;
                gvAccountTable.Columns[1].Visible = false;
                //gvAccountTable.Columns[2].Visible = false;
                //gvAccountTable.Columns[3].Visible = false;
                gvAccountTable.Columns[4].Visible = false;
                gvAccountTable.Columns[5].Visible = false;
                gvAccountTable.Columns[6].Visible = false;
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
                if (status.STT == true) { gvAccountTable.Columns[0].Visible = true; } else { gvAccountTable.Columns[0].Visible = false; }
                gvAccountTable.Columns[1].Visible = false; //Mặc định ẩn
                if (status.Email == true) { gvAccountTable.Columns[2].Visible = true; } else { gvAccountTable.Columns[2].Visible = false; }
                if (status.Balance == true) { gvAccountTable.Columns[3].Visible = true; } else { gvAccountTable.Columns[3].Visible = false; }
                if (status.TransactionTotal == true) { gvAccountTable.Columns[4].Visible = false; } else { gvAccountTable.Columns[4].Visible = false; }
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
                gvAccountTable.Columns[25].Visible = false; //Mặc định ẩn
                if (status.InputtedDate == true) { gvAccountTable.Columns[26].Visible = true; } else { gvAccountTable.Columns[26].Visible = false; }
                gvAccountTable.Columns[27].Visible = false;
                if (status.RecoveryEmail == true) { gvAccountTable.Columns[28].Visible = true; } else { gvAccountTable.Columns[28].Visible = false; }
                if (status.ForwordToEmail == true) { gvAccountTable.Columns[29].Visible = true; } else { gvAccountTable.Columns[29].Visible = false; }
                if (status.SecondEmail == true) { gvAccountTable.Columns[30].Visible = true; } else { gvAccountTable.Columns[30].Visible = false; }
                if (status.ProxyStatus == true) { gvAccountTable.Columns[31].Visible = true; } else { gvAccountTable.Columns[31].Visible = false; }
                if (status.Acc_ON_OFF == true) { gvAccountTable.Columns[32].Visible = true; } else { gvAccountTable.Columns[32].Visible = false; }
                if (status.Set_ChangedPassPP == true) { gvAccountTable.Columns[33].Visible = true; } else { gvAccountTable.Columns[33].Visible = false; }
                if (status.Set_ChangedPassEmail == true) { gvAccountTable.Columns[34].Visible = true; } else { gvAccountTable.Columns[34].Visible = false; }
                if (status.Set_Add_RecoveryEmail == true) { gvAccountTable.Columns[35].Visible = true; } else { gvAccountTable.Columns[35].Visible = false; }
                if (status.Set_Deleted_FwEmail == true) { gvAccountTable.Columns[36].Visible = true; } else { gvAccountTable.Columns[36].Visible = false; }
                if (status.Set_Add_New_FwEmail == true) { gvAccountTable.Columns[37].Visible = true; } else { gvAccountTable.Columns[37].Visible = false; }
                if (status.Set_2FA == true) { gvAccountTable.Columns[38].Visible = true; } else { gvAccountTable.Columns[38].Visible = false; }
                if (status.Set_Questions == true) { gvAccountTable.Columns[39].Visible = true; } else { gvAccountTable.Columns[39].Visible = false; }
                if (status.Profile == true) { gvAccountTable.Columns[40].Visible = true; } else { gvAccountTable.Columns[40].Visible = false; }
                if (status.AccPassword_Old == true) { gvAccountTable.Columns[41].Visible = true; } else { gvAccountTable.Columns[41].Visible = false; }
                if (status.EmailPassword_Old == true) { gvAccountTable.Columns[42].Visible = true; } else { gvAccountTable.Columns[42].Visible = false; }
            }
        }
        private void Load_RightMouse_Click_Menu()
        {          
            ContextMenuStrip menu = new ContextMenuStrip(); // Tạo menu
 
            ToolStripItem check_acc_all = menu.Items.Add("Check Acc - Cập nhật TT");
            check_acc_all.Click += new EventHandler(Check_Acc_All);
            ToolStripItem change_account_info = menu.Items.Add("Đổi thông tin Acc mới - all");
            change_account_info.Click += new EventHandler(Set_Acc_All);
            ToolStripItem change_papyal_passoword = menu.Items.Add("Đổi mật khẩu paypal");
            change_papyal_passoword.Click += new EventHandler(Change_Paypal_Password);
            ToolStripItem email_login = menu.Items.Add("Đăng nhập Email");
            email_login.Click += new EventHandler(Email_Login_Click);
            //Gmail
            ToolStripMenuItem gmail_manager = (ToolStripMenuItem)menu.Items.Add("Đổi thông tin Email");
            ToolStripItem remove_email_fw = gmail_manager.DropDownItems.Add("Xóa Email Forward");
            remove_email_fw.Click += new EventHandler(Remove_Fw_Email);
            ToolStripItem add_email_fw = gmail_manager.DropDownItems.Add("Thêm Email Forward");
            add_email_fw.Click += new EventHandler(Add_Fw_Email);
            ToolStripItem add_recovery_email = gmail_manager.DropDownItems.Add("Thêm Email KP - Beeliant");
            add_email_fw.Click += new EventHandler(Add_Recovery_Email);
            ToolStripItem change_email_password = gmail_manager.DropDownItems.Add("Thêm Email KP - Beeliant");
            change_email_password.Click += new EventHandler(Change_Email_Password);

            ToolStripMenuItem profile = (ToolStripMenuItem)menu.Items.Add("Quản lý Profile");
            ToolStripItem create_profile = profile.DropDownItems.Add("Tạo Profile");
            create_profile.Click += new EventHandler(Create_Profile);
            ToolStripItem delete_profile = profile.DropDownItems.Add("Xóa Profile");
            delete_profile.Click += new EventHandler(Delete_Profile);
            ToolStripItem delete_tem_profile = profile.DropDownItems.Add("Xóa Profile - Tạm");
            delete_tem_profile.Click += new EventHandler(Delete_Temprory_Profile);
            ToolStripItem check_canvas = profile.DropDownItems.Add("Check Canvas");
            check_canvas.Click += new EventHandler(Check_Canvas);
            ToolStripItem canvas_on = profile.DropDownItems.Add("Set Canvas - ON");
            canvas_on.Click += new EventHandler(Set_Canvas_ON);
            ToolStripItem canvas_off = profile.DropDownItems.Add("Set Canvas - OFF");
            canvas_off.Click += new EventHandler(Set_Canvas_OFF);
            
            ToolStripMenuItem proxy_update = (ToolStripMenuItem)menu.Items.Add("Cập nhật Proxy");
            ToolStripItem update_one_proxy = proxy_update.DropDownItems.Add("Dán 1 Proxy");
            update_one_proxy.Click += new EventHandler(Update_One_Proxy);
            ToolStripItem update_list_proxy = proxy_update.DropDownItems.Add("Dán list Proxy - Google Sheet");
            update_list_proxy.Click += new EventHandler(Update_List_Proxy);
            
            //Past

            ToolStripMenuItem past = (ToolStripMenuItem)menu.Items.Add("Dán - cập nhật thông tin");
            ToolStripItem past_forward_email = past.DropDownItems.Add("Dán Email forward");
            past_forward_email.Click += new EventHandler(Past_ForwardEmail_Click);
            ToolStripItem past_acc_password = past.DropDownItems.Add("Dán mật khẩu Paypal");
            past_acc_password.Click += new EventHandler(Past_Acc_Password_Click);
            ToolStripItem past_new_email = past.DropDownItems.Add("Dán - cập nhật Email mới");
            past_new_email.Click += new EventHandler(Past_New_Email_Click);
            ToolStripItem past_email_password = past.DropDownItems.Add("Dán mật khẩu Email"); 
            past_email_password.Click += new EventHandler(Past_Email_Password_Click);

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

            ToolStripMenuItem set_tool_gmp_ggsheet = (ToolStripMenuItem)menu.Items.Add("Set Tool - GMP - Google Sheet");
            ToolStripItem past_tem_profile_limit = set_tool_gmp_ggsheet.DropDownItems.Add("Dán giới hạn profile tạm");
            past_tem_profile_limit.Click += new EventHandler(Past_Tem_Profile_Limit);
            ToolStripItem space_01 = set_tool_gmp_ggsheet.DropDownItems.Add("---");

            ToolStripItem past_api_url = set_tool_gmp_ggsheet.DropDownItems.Add("Dán GMP-Login API URL");
            past_api_url.Click += new EventHandler(Past_API_URL_CLick);
            ToolStripItem past_beeliant_profileId = set_tool_gmp_ggsheet.DropDownItems.Add("Dán GMP Beeliant ProfileId");
            past_beeliant_profileId.Click += new EventHandler(Past_Beeliant_ProfileID_Click);
            ToolStripItem past_forward_email_gmail_profileId = set_tool_gmp_ggsheet.DropDownItems.Add("Dán GMP ProfileId Email-Forward-Gmail");
            past_forward_email_gmail_profileId.Click += new EventHandler(Past_ForwardEmail_Gmail_ProfileId);
            ToolStripItem space_02 = set_tool_gmp_ggsheet.DropDownItems.Add("---");

            ToolStripItem past_google_sheet_id = set_tool_gmp_ggsheet.DropDownItems.Add("Dán Google Sheet ID");
            past_google_sheet_id.Click += new EventHandler(Past_Google_Sheet_ID);
            ToolStripItem past_database_sheet_name = set_tool_gmp_ggsheet.DropDownItems.Add("Dán tên sheet database");
            past_database_sheet_name.Click += new EventHandler(Past_Database_Sheet_Name);
            ToolStripItem past_backup_info_sheet_name = set_tool_gmp_ggsheet.DropDownItems.Add("Dán tên sheet backup data");
            past_backup_info_sheet_name.Click += new EventHandler(Past_Backup_Info_Sheet_Name);
            ToolStripItem past_add_new_acc_sheet_name = set_tool_gmp_ggsheet.DropDownItems.Add("Dán tên sheet nhập Acc mới");
            past_add_new_acc_sheet_name.Click += new EventHandler(Past_Add_New_Accounts_Sheet_Name);
            ToolStripItem past_update_proxy_sheet_name = set_tool_gmp_ggsheet.DropDownItems.Add("Dán tên sheet cập nhật proxy");
            past_update_proxy_sheet_name.Click += new EventHandler(Past_Update_Proxy_Sheet_Name);

            //Dòng COPY
            ToolStripMenuItem copy = (ToolStripMenuItem)menu.Items.Add("Copy thông tin");
            ToolStripItem copy_email = copy.DropDownItems.Add("Email");
            copy_email.Click += new EventHandler(Copy_Email);
            ToolStripItem copy_name = copy.DropDownItems.Add("Họ Tên");
            copy_name.Click += new EventHandler(Copy_Name);
            ToolStripItem copy_proxy = copy.DropDownItems.Add("Proxy");
            copy_proxy.Click += new EventHandler(Copy_Proxy);
            ToolStripItem copy_email_pass = copy.DropDownItems.Add("Email- Mật khẩu - 2FA - Mk Email");
            copy_email_pass.Click += new EventHandler(Copy_Email_Pass);

            gvAccountTable.ContextMenuStrip = menu;
        }
        //Check_Acc_All

        private void Check_Acc_All (object sender, EventArgs e)
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
                                        next_step = false;
                                    }
                                    else 
                                    {
                                        gvAccountTable.SelectedRows[i].Cells[7].Value = "Đăng nhập thất bại--";
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

                                if (next_step == true)
                                {
                                    var result_04 = new Paypal_Get_Name_Address_Controller().Get_Name_Address(Acc, driver);
                                    driver = result_04.Driver;
                                    if (result_04.Status == true)
                                    {
                                        gvAccountTable.SelectedRows[i].Cells[7].Value += "Đã cập nhật họ tên, địa chỉ--";
                                        gvAccountTable.SelectedRows[i].Cells[16].Value = result_04.Value_01;
                                        gvAccountTable.SelectedRows[i].Cells[18].Value = result_04.Value_02;
                                        gvAccountTable.SelectedRows[i].Cells[8].Value = result_04.Value_03;
                                    }
                                    else
                                    {
                                        gvAccountTable.SelectedRows[i].Cells[7].Value += "Cập nhật họ tên, địa chỉ thất bại--";
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
        private void Set_Acc_All (object sender, EventArgs e) 
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
                            if(Acc.Acc_ON_OFF != true) 
                            {
                                if (Acc.EmailType == "hotmail.com" || Acc.EmailType == "outlook.com")
                                {
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

                                        bool next_step_email = false;
                                        if (next_step == true)
                                        {
                                            var result_02 = new Login_Hotmail().Login(Acc, driver);
                                            driver = result_02.Driver;
                                            if (result_02.Status == false)
                                            {
                                                next_step_email = true;
                                            }
                                            else
                                            {
                                                gvAccountTable.SelectedRows[i].Cells[7].Value = "Đăng email nhập thất bại--";
                                            }
                                        }


                                        if (Acc.Set_Deleted_FwEmail != true && next_step_email == true)
                                        {
                                            var result_03 = new Remove_ForwardEmail_Hotmail().Remove(Acc, driver);
                                            driver = result_03.Driver;
                                            if (result_03.Status == true)
                                            {
                                                gvAccountTable.SelectedRows[i].Cells[7].Value = "Đã xóa email forward--";
                                            }
                                            else
                                            {
                                                gvAccountTable.SelectedRows[i].Cells[7].Value = "Xóa email forward fail--";
                                            }
                                        }
                                       
                                        if (Acc.Set_Add_New_FwEmail != true && next_step_email == true)
                                        {
                                            var result_04 = new Add_ForwardEmail_Hotmail().Add(Acc, driver);
                                            driver = result_04.Driver;
                                            if (result_04.Status == true)
                                            {
                                                gvAccountTable.SelectedRows[i].Cells[7].Value += "Đã thêm email forward--";
                                                gvAccountTable.SelectedRows[i].Cells[37].Value = "YES";
                                                if(result_04.SetAcc_All_Status == true) { gvAccountTable.SelectedRows[i].Cells[32].Value = "ON"; }                                              
                                            }
                                            else
                                            {
                                                gvAccountTable.SelectedRows[i].Cells[7].Value += "Thêm email forward fail--";
                                            }
                                        }

                                        if (Acc.Set_ChangedPassEmail != true && next_step_email == true)
                                        {
                                            bool hold_on = false;
                                            var result_05 = new ChangePassword_Hotmail_Add_RecoveryEmail_Beeliant().ChangePassword_AddRecoveryEmail(Acc, driver);
                                            driver = result_05.Driver;
                                            if(result_05.Status == true) 
                                            {
                                                gvAccountTable.SelectedRows[i].Cells[7].Value += "Đã đổi mật khẩu email--";
                                                gvAccountTable.SelectedRows[i].Cells[34].Value = "YES";
                                                var Acc05 = Get_Acc_Info(i);
                                                gvAccountTable.SelectedRows[i].Cells[11].Value = Acc05.EmailPassword;
                                                gvAccountTable.SelectedRows[i].Cells[42].Value = Acc05.EmailPassword_Old;
                                                gvAccountTable.SelectedRows[i].Cells[28].Value = Acc05.RecoveryEmail;
                                                if(Acc05.Acc_ON_OFF == true) { gvAccountTable.SelectedRows[i].Cells[32].Value = "ON"; }
                                            }
                                            else 
                                            {
                                                gvAccountTable.SelectedRows[i].Cells[7].Value += "Đổi mật khẩu email fail--";
                                            }
                                        }

                                        if(Acc.Set_ChangedPassPP != true && next_step == true) 
                                        {
                                            var result_06 = new Paypal_Login_Controller().Paypal_Login(Acc, driver, checkBox_hold_on.Checked);
                                            driver = result_06.Driver;
                                            if(result_06.Status == true) 
                                            {
                                                var result_07 = new Change_Paypal_Password_Controller().Change(Acc, driver);
                                                driver = result_07.Driver;
                                                if(result_07.Status == true) 
                                                {
                                                    gvAccountTable.SelectedRows[i].Cells[7].Value += "Đổi mật khẩu paypal thành công--";
                                                    gvAccountTable.SelectedRows[i].Cells[33].Value = "YES";
                                                    var Acc06 = Get_Acc_Info(i);
                                                    gvAccountTable.SelectedRows[i].Cells[9].Value = Acc06.AccPassword;
                                                    gvAccountTable.SelectedRows[i].Cells[41].Value = Acc06.AccPassword_Old;
                                                    if(Acc06.Acc_ON_OFF == true) { gvAccountTable.SelectedRows[i].Cells[32].Value = "ON"; }
                                                }
                                                else 
                                                {
                                                    gvAccountTable.SelectedRows[i].Cells[7].Value += "Đổi mật khẩu paypal fail--";
                                                }
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
                            }
                            else 
                            {
                                gvAccountTable.SelectedRows[i].Cells[7].Value += "Đã đổi thông tin (tất cả)--";
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
        private void Change_Paypal_Password (object sender, EventArgs e)
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
                                    var result_06 = new Paypal_Login_Controller().Paypal_Login(Acc, driver, checkBox_hold_on.Checked);
                                    driver = result_06.Driver;
                                    if (result_06.Status == true)
                                    {
                                        var result_07 = new Change_Paypal_Password_Controller().Change(Acc, driver);
                                        driver = result_07.Driver;
                                        if (result_07.Status == true)
                                        {
                                            gvAccountTable.SelectedRows[i].Cells[7].Value += "Đổi mật khẩu paypal thành công--";
                                            gvAccountTable.SelectedRows[i].Cells[33].Value = "YES";
                                            var Acc06 = Get_Acc_Info(i);
                                            gvAccountTable.SelectedRows[i].Cells[9].Value = Acc06.AccPassword;
                                            gvAccountTable.SelectedRows[i].Cells[43].Value = Acc06.AccPassword_Old;
                                            if (Acc06.Acc_ON_OFF == true) { gvAccountTable.SelectedRows[i].Cells[32].Value = "ON"; }
                                        }
                                        else
                                        {
                                            gvAccountTable.SelectedRows[i].Cells[7].Value += "Đổi mật khẩu paypal fail--";
                                        }
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
                                if (Acc01.EmailType == "gmail.com" || Acc01.EmailType == "hotmail.com" || Acc01.EmailType == "outlook.com")
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
                                        if (Acc01.EmailType == "gmail.com")
                                        {
                                            var result_02 = new Gmail_Login().Login(Acc01, driver, checkBox_hold_on.Checked);
                                            driver = result_02.Driver;
                                            if (result_02.Status != true) { gvAccountTable.SelectedRows[i].Cells[7].Value = "Đăng nhập Email thất bại--"; }
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
        //
        private void Remove_Fw_Email(object sender, EventArgs e)
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
                                if(Acc.EmailType == "gmail.com" || Acc.EmailType == "hotmail.com" || Acc.EmailType == "outlook.com") 
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
                                            var result_07 = new Remove_Forward_Email_Gmail().Remove(Acc, driver);
                                            driver = result_07.Driver;
                                            if (result_07.Status == true)
                                            {
                                                gvAccountTable.SelectedRows[i].Cells[7].Value = "Đã xóa email forward--";
                                                gvAccountTable.SelectedRows[i].Cells[29].Value = "";
                                                gvAccountTable.SelectedRows[i].Cells[36].Value = "YES";
                                            }
                                            else
                                            {
                                                gvAccountTable.SelectedRows[i].Cells[7].Value = "Xóa email forward fail--";
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
                                            var result_07 = new Remove_ForwardEmail_Hotmail().Remove(Acc, driver);
                                            driver = result_07.Driver;
                                            if (result_07.Status == true)
                                            {
                                                gvAccountTable.SelectedRows[i].Cells[7].Value = "Đã xóa email forward--";
                                                gvAccountTable.SelectedRows[i].Cells[29].Value = "";
                                                gvAccountTable.SelectedRows[i].Cells[36].Value = "YES";
                                            }
                                            else
                                            {
                                                gvAccountTable.SelectedRows[i].Cells[7].Value = "Xóa email forward fail--";
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
                                if(Acc.EmailType == "gmail.com" || Acc.EmailType == "hotmail.com" || Acc.EmailType == "outlook.com") 
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
        private void Add_Recovery_Email(object sender, EventArgs e)
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
                                if (Acc.EmailType == "gmail.com")
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
                                        var result_02 = new Gmail_Login().Login(Acc, driver, checkBox_hold_on.Checked);
                                        driver = result_02.Driver;
                                        if (result_02.Status == true)
                                        {
                                            var result_07 = new Add_New_Recovery_Email_Gmail_Beeliant().Add(Acc, driver, checkBox_hold_on.Checked);
                                            driver = result_07.Driver;
                                            if (result_07.Status == true)
                                            {
                                                gvAccountTable.SelectedRows[i].Cells[7].Value = "Đã add email khôi phục--";
                                                gvAccountTable.SelectedRows[i].Cells[28].Value = Acc.Email.Split('@')[0] + "@beeliant.com";
                                                gvAccountTable.SelectedRows[i].Cells[35].Value = "YES";
                                            }
                                            else
                                            {
                                                gvAccountTable.SelectedRows[i].Cells[7].Value = "Add email khôi phục fail--";
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
                                else if(Acc.EmailType == "hotmail.com" || Acc.EmailType == "outlook.com")
                                {
                                    gvAccountTable.SelectedRows[i].Cells[7].Value = "Tạm chưa set up cho hotmail, outlook--";
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
        private void Change_Email_Password(object sender, EventArgs e)
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

                                if (next_step == true && Acc.EmailType == "gmail.com")
                                {
                                    var result_02 = new Gmail_Login().Login(Acc, driver, checkBox_hold_on.Checked);
                                    driver = result_02.Driver;
                                    if (result_02.Status == true)
                                    {
                                        var result_03 = new Change_Gmail_Password().Change_Password(Acc, driver);
                                        driver = result_03.Driver;
                                        if (result_03.Status == true)
                                        {
                                            var Acc03 = Get_Acc_Info(i);
                                            gvAccountTable.SelectedRows[i].Cells[7].Value = "Đã đổi mật khẩu email--";
                                            gvAccountTable.SelectedRows[i].Cells[11].Value = Acc03.EmailPassword;
                                            gvAccountTable.SelectedRows[i].Cells[42].Value = Acc03.EmailPassword_Old;
                                            gvAccountTable.SelectedRows[i].Cells[34].Value = "YES";
                                        }
                                        else
                                        {
                                            gvAccountTable.SelectedRows[i].Cells[7].Value = "Đổi mật khẩu email fail--";
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
                                        var result_03 = new ChangePassword_Hotmail_Add_RecoveryEmail_Beeliant().ChangePassword_AddRecoveryEmail(Acc, driver, checkBox_hold_on.Checked);
                                        driver = result_03.Driver;
                                        if (result_03.Status == true)
                                        {
                                            var Acc03 = Get_Acc_Info(i);
                                            gvAccountTable.SelectedRows[i].Cells[7].Value = "Đã đổi mật khẩu email--";
                                            gvAccountTable.SelectedRows[i].Cells[11].Value = Acc03.EmailPassword;
                                            gvAccountTable.SelectedRows[i].Cells[42].Value = Acc03.EmailPassword_Old;
                                            gvAccountTable.SelectedRows[i].Cells[34].Value = "YES";
                                        }
                                        else
                                        {
                                            gvAccountTable.SelectedRows[i].Cells[7].Value = "Đổi mật khẩu email fail--";
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

        private void Create_Profile (object sender, EventArgs e)
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
                        for (int i = selectedRowCount - 1; i >= 0; i--)
                        {
                            UndectedChromeDriver driver = null;
                            var Acc = Get_Acc_Info(i);
                            try
                            {
                                PaypalDbContext db = new PaypalDbContext();
                                var db_account = db.Accounts.Where(x => x.ID == Acc.ID).FirstOrDefault();
                                if(Acc.Profile_Save == true) 
                                {
                                    if (Acc.Profile == true)
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
                                MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo");
                            }
                        }
                        catch
                        {
                            try { new Google_Sheet_Controller().Update_Database(); }
                            catch
                            {
                                MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo");
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
                    }
                    catch { }
                }
                else { MessageBox.Show("Cần chọn full dòng, click vào cell đầu tiên của dòng để chọn full dòng!", "Thông báo"); }
            }
        }
        //Set_Canvas_ON Set_Canvas_OFF
        private void Check_Canvas (object sender, EventArgs e)
        {
            Int32 selectedRowCount = gvAccountTable.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount > 0)
            {
                try
                {
                    for (int i = (selectedRowCount - 1); i >= 0; i--)
                    {
                        int AccID = int.Parse(gvAccountTable.SelectedRows[i].Cells[1].Value.ToString());
                        PaypalDbContext db = new PaypalDbContext();
                        var db_account = db.Accounts.Where(x => x.ID == AccID).FirstOrDefault();
                        if(db_account.Canvas_profile == false) 
                        {
                            gvAccountTable.SelectedRows[i].Cells[7].Value = "Canvas = OFF --";
                        }
                        else 
                        {
                            gvAccountTable.SelectedRows[i].Cells[7].Value = "Canvas = ON --";
                        }
                    }
                }
                catch { }
            }
            else { MessageBox.Show("Cần chọn full dòng, click vào cell đầu tiên của dòng để chọn full dòng!", "Thông báo"); }
        }
        private void Set_Canvas_ON (object sender, EventArgs e)
        {
            Int32 selectedRowCount = gvAccountTable.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount > 0)
            {
                try
                {
                    for (int i = (selectedRowCount - 1); i >= 0; i--)
                    {
                        int AccID = int.Parse(gvAccountTable.SelectedRows[i].Cells[1].Value.ToString());
                        PaypalDbContext db = new PaypalDbContext();
                        var db_account = db.Accounts.Where(x => x.ID == AccID).FirstOrDefault();
                        db_account.Canvas_profile = true;
                        db.SaveChanges();
                        gvAccountTable.SelectedRows[i].Cells[7].Value = "Canvas = ON --";
                    }
                }
                catch { }
            }
            else { MessageBox.Show("Cần chọn full dòng, click vào cell đầu tiên của dòng để chọn full dòng!", "Thông báo"); }
        }
        private void Set_Canvas_OFF(object sender, EventArgs e)
        {
            Int32 selectedRowCount = gvAccountTable.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount > 0)
            {
                try
                {
                    for (int i = (selectedRowCount - 1); i >= 0; i--)
                    {
                        int AccID = int.Parse(gvAccountTable.SelectedRows[i].Cells[1].Value.ToString());
                        PaypalDbContext db = new PaypalDbContext();
                        var db_account = db.Accounts.Where(x => x.ID == AccID).FirstOrDefault();
                        db_account.Canvas_profile = false;
                        db.SaveChanges();
                        gvAccountTable.SelectedRows[i].Cells[7].Value = "Canvas = OFF --";
                    }
                            
                }
                catch { } 
            }
            else { MessageBox.Show("Cần chọn full dòng, click vào cell đầu tiên của dòng để chọn full dòng!", "Thông báo"); }
        }
        //Past_New_Email_Click

        private void Past_Acc_Password_Click(object sender, EventArgs e) 
        {
            DialogResult dialogResult = MessageBox.Show("Bạn muốn dán khẩu paypal?", "Thông báo!", MessageBoxButtons.YesNo);
            if(dialogResult == DialogResult.Yes) 
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

        private void Update_One_Proxy (object sender, EventArgs e)
        {
            Int32 selected_row_count = gvAccountTable.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if(selected_row_count > 0) 
            {
                IDataObject iData = Clipboard.GetDataObject();
                var proxy = (String)iData.GetData(DataFormats.Text);
                int AccID = int.Parse(gvAccountTable.SelectedRows[selected_row_count - 1].Cells[1].Value.ToString());
                PaypalDbContext db = new PaypalDbContext();
                var db_account = db.Accounts.Where(x => x.ID == AccID).FirstOrDefault();
                if(db_account.Profile == true) 
                {
                    var check_api = new Check_API().Start();
                    if (check_api == true) 
                    {
                        var result = new GPMLoginAPI().UpdateProxy(db_account.ProfileId, proxy);
                        if(result == true) 
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
                
                if(proxy_total > 0) 
                {
                    int index = 0;
                    if (proxy_total > selected_row_count) { index = selected_row_count; } else { index = proxy_total; }
                    int j = 0;
                    for(int i = selected_row_count - 1; i >= selected_row_count - index; i--) 
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
                        try { new Google_Sheet_Controller().Update_Database(); } catch { MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo"); }
                    }
                    catch
                    {
                        try { new Google_Sheet_Controller().Update_Database(); } catch { MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo"); }
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
                        try { new Google_Sheet_Controller().Update_Database(); } catch { MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo"); }
                    }
                    catch
                    {
                        try { new Google_Sheet_Controller().Update_Database(); } catch { MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo"); }
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
                        try { new Google_Sheet_Controller().Update_Database(); } catch { MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo"); }
                    }
                    catch
                    {
                        try { new Google_Sheet_Controller().Update_Database(); } catch { MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo"); }
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
                        try { new Google_Sheet_Controller().Update_Database(); } catch { MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo"); }
                    }
                    catch
                    {
                        try { new Google_Sheet_Controller().Update_Database(); } catch { MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo"); }
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
                        try { new Google_Sheet_Controller().Update_Database(); } catch { MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo"); }
                    }
                    catch
                    {
                        try { new Google_Sheet_Controller().Update_Database(); } catch { MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo"); }
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
                        try { new Google_Sheet_Controller().Update_Database(); } catch { MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo"); }
                    }
                    catch
                    {
                        try { new Google_Sheet_Controller().Update_Database(); } catch { MessageBox.Show("- Cập nhật google sheet bị lỗi", "Thông báo"); }
                    }
                }
                else { MessageBox.Show("Cần chọn full dòng, click vào cell đầu tiên của dòng để chọn full dòng!", "Thông báo"); }
            }
            catch (Exception) { }
        }

        private void Past_Tem_Profile_Limit(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Bạn muốn dán API URL?", "Xác nhận!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                IDataObject iData = Clipboard.GetDataObject();
                var string_value = (String)iData.GetData(DataFormats.Text);
                if (iData.GetDataPresent(DataFormats.Text) == true)
                {
                    PaypalDbContext db = new PaypalDbContext();
                    db.Admins.Where(x => x.Name == "Tem_Profiles_Litmited").FirstOrDefault().Value = string_value;
                    db.SaveChanges();
                    StringWriter stringWriter = new StringWriter();
                    stringWriter.WriteLine("- Dán thành công!");
                    stringWriter.WriteLine("- Giới hạn profile tạm là: " + string_value);
                    MessageBox.Show(stringWriter.ToString(), "Thông báo");
                }
            }
        }
        private void Past_API_URL_CLick(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Bạn muốn dán API URL?", "Xác nhận!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                IDataObject iData = Clipboard.GetDataObject();
                var string_value = (String)iData.GetData(DataFormats.Text);
                if (iData.GetDataPresent(DataFormats.Text) == true)
                {
                    PaypalDbContext db = new PaypalDbContext();
                    //API_URL
                    db.Admins.Where(x => x.Name == "API_URL").FirstOrDefault().Value = string_value;
                    db.SaveChanges();

                    StringWriter stringWriter = new StringWriter();
                    stringWriter.WriteLine("- Dán thành công!");
                    stringWriter.WriteLine("- API URL là: ");
                    stringWriter.WriteLine(string_value);
                    MessageBox.Show(stringWriter.ToString(), "Thông báo");
                }
            }
        }
        private void Past_Beeliant_ProfileID_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Bạn muốn dán Beeliant ProfileId?", "Xác nhận!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                IDataObject iData = Clipboard.GetDataObject();
                var string_value = (String)iData.GetData(DataFormats.Text);
                if (iData.GetDataPresent(DataFormats.Text) == true)
                {
                    PaypalDbContext db = new PaypalDbContext();
                    //API_URL
                    db.Admins.Where(x => x.Name == "admin@beeliant.com").FirstOrDefault().Value = string_value;
                    db.SaveChanges();

                        StringWriter stringWriter = new StringWriter();
                        stringWriter.WriteLine("- Dán thành công!");
                        stringWriter.WriteLine("- Beeliant ProfileId là: ");
                        stringWriter.WriteLine(string_value);
                        MessageBox.Show(stringWriter.ToString(), "Thông báo");
                }
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
        private void Past_Google_Sheet_ID(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Bạn muốn dán Google Sheet ID?", "Xác nhận!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                IDataObject iData = Clipboard.GetDataObject();
                var string_value = (String)iData.GetData(DataFormats.Text);
                if (iData.GetDataPresent(DataFormats.Text) == true)
                {
                    PaypalDbContext db = new PaypalDbContext();
                    db.Admins.Where(x => x.Name == "Google_Sheet_ID").FirstOrDefault().Value = string_value;
                    db.SaveChanges();
                    StringWriter stringWriter = new StringWriter();
                    stringWriter.WriteLine("- Dán thành công!");
                    stringWriter.WriteLine("- Goolge Sheet ID là:");
                    stringWriter.WriteLine(string_value);
                    MessageBox.Show(stringWriter.ToString(), "Thông báo");
                }
            }
        }
        private void Past_Database_Sheet_Name(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Bạn muốn dán tên sheet database?", "Xác nhận!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                IDataObject iData = Clipboard.GetDataObject();
                var string_value = (String)iData.GetData(DataFormats.Text);
                if (iData.GetDataPresent(DataFormats.Text) == true)
                {
                    PaypalDbContext db = new PaypalDbContext();
                    db.Admins.Where(x => x.Name == "Database_Sheet_Name").FirstOrDefault().Value = string_value;
                    db.SaveChanges();
                    StringWriter stringWriter = new StringWriter();
                    stringWriter.WriteLine("- Dán thành công!");
                    stringWriter.WriteLine("- Tên sheet database là: " + string_value);
                    MessageBox.Show(stringWriter.ToString(), "Thông báo");
                }
            }
        }
        private void Past_Backup_Info_Sheet_Name(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Bạn muốn dán tên sheet backup info?", "Xác nhận!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                IDataObject iData = Clipboard.GetDataObject();
                var string_value = (String)iData.GetData(DataFormats.Text);
                if (iData.GetDataPresent(DataFormats.Text) == true)
                {
                    PaypalDbContext db = new PaypalDbContext();
                    db.Admins.Where(x => x.Name == "Del_Accounts_Sheet_Name").FirstOrDefault().Value = string_value;
                    db.SaveChanges();
                    StringWriter stringWriter = new StringWriter();
                    stringWriter.WriteLine("- Dán thành công!");
                    stringWriter.WriteLine("- Tên sheet backup info là: " + string_value);
                    MessageBox.Show(stringWriter.ToString(), "Thông báo");
                }
            }
        }
        private void Past_Add_New_Accounts_Sheet_Name(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Bạn muốn dán tên sheet nhập tài khoản mới?", "Xác nhận!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                IDataObject iData = Clipboard.GetDataObject();
                var string_value = (String)iData.GetData(DataFormats.Text);
                if (iData.GetDataPresent(DataFormats.Text) == true)
                {
                    PaypalDbContext db = new PaypalDbContext();
                    db.Admins.Where(x => x.Name == "Input_Accounts_Sheet_Name").FirstOrDefault().Value = string_value;
                    db.SaveChanges();
                    StringWriter stringWriter = new StringWriter();
                    stringWriter.WriteLine("- Dán thành công!");
                    stringWriter.WriteLine("- Tên sheet nhập tài khoản mới là: " + string_value);
                    MessageBox.Show(stringWriter.ToString(), "Thông báo");
                }
            }
        }
        private void Past_Update_Proxy_Sheet_Name(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Bạn muốn dán tên sheet cập nhật proxy?", "Xác nhận!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                IDataObject iData = Clipboard.GetDataObject();
                var string_value = (String)iData.GetData(DataFormats.Text);
                if (iData.GetDataPresent(DataFormats.Text) == true)
                {
                    PaypalDbContext db = new PaypalDbContext();
                    db.Admins.Where(x => x.Name == "Update_Proxy_Sheet_Name").FirstOrDefault().Value = string_value;
                    db.SaveChanges();
                    StringWriter stringWriter = new StringWriter();
                    stringWriter.WriteLine("- Dán thành công!");
                    stringWriter.WriteLine("- Tên sheet cập nhật proxy là: " + string_value);
                    MessageBox.Show(stringWriter.ToString(), "Thông báo");
                }
            }
        }
        //Copy
        private void Copy_Email(object sender, EventArgs e)
        {
            Int32 selectedRowCount =
            gvAccountTable.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount > 0)
            {
                StringWriter strWriteLine = new StringWriter();
                string copy_email = gvAccountTable.SelectedRows[selectedRowCount - 1].Cells[2].Value.ToString();
                strWriteLine.Write(copy_email);
                Clipboard.SetText(strWriteLine.ToString());
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
    }
}
