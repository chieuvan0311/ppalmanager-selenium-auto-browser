using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using PAYPAL.ChromeDrivers;
using PAYPAL.Controller;
using PAYPAL.Dao;
using PAYPAL.DataConnection;
using PAYPAL.Models;
using PAYPAL.RandomData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PAYPAL.Hotmail_Controller
{
    public class ChangePassword_Hotmail_Add_RecoveryEmail_Beeliant
    {
        public Session_Result_Model ChangePassword_AddRecoveryEmail (Account Acc, UndectedChromeDriver receive_driver, bool hold_on = false)
        {
            Session_Result_Model result = new Session_Result_Model();
            UndectedChromeDriver driver = receive_driver;
            result.Status = false;
            try 
            {
                driver.Url = "https://account.live.com/password/change?";
                driver.Navigate();
                Thread.Sleep(RdTimes.T_4000());
                IWebElement confirm_password_box_check = null;
                Thread.Sleep(RdTimes.T_700());
                try
                {
                    confirm_password_box_check = driver.FindElement(By.Id("i0118"));
                    Thread.Sleep(RdTimes.T_300());
                    for (int i = 0; i < Acc.EmailPassword.Length; i++)
                    {
                        confirm_password_box_check.SendKeys(Acc.EmailPassword[i].ToString());
                        Thread.Sleep(RdTimes.T_10_20());
                    }
                    Thread.Sleep(RdTimes.T_1000());
                }
                catch (Exception) { }

                //Check Required
                bool next_step = false;
                bool change_password = false;
                string recoveryEmail = Acc.Email.Split('@')[0] + "beeliant.com";

                IWebElement present_password_box_check = null;
                try { present_password_box_check = driver.FindElement(By.Id("iCurPassword")); } catch { }
                Thread.Sleep(RdTimes.T_400());
                IWebElement confirm_recovery_email_button_check = null;
                try { confirm_recovery_email_button_check = driver.FindElement(By.XPath("//div[@id='idDiv_SAOTCS_Proofs']//div[@role='button']")); } catch (Exception) { }
                Thread.Sleep(RdTimes.T_400());
                IWebElement recoverEmail_box_check = null;
                try { recoverEmail_box_check = driver.FindElement(By.Id("EmailAddress")); } catch (Exception) { }
                Thread.Sleep(RdTimes.T_400());

                if (present_password_box_check != null) { change_password = true; }
                if (confirm_recovery_email_button_check != null) { next_step = true; }

                if (recoverEmail_box_check != null) //Thêm email KP
                {
                    var recovery_email_box = driver.FindElement(By.Id("EmailAddress"));
                    Thread.Sleep(RdTimes.T_1000());
                    for (int irce = 0; irce < recoveryEmail.Length; irce++)
                    {
                        var letter = recoveryEmail[irce].ToString();
                        recovery_email_box.SendKeys(letter);
                        Thread.Sleep(RdTimes.T_3_8());
                    }
                    Thread.Sleep(RdTimes.T_800());
                    var btn_next = driver.FindElement(By.Id("iNext"));
                    Thread.Sleep(RdTimes.T_300());
                    btn_next.Click();
                    Thread.Sleep(RdTimes.T_2000());

                    //Get code

                    var result_01 = new Get_Code_From_Beeliant().Get_Code(recoveryEmail, hold_on);

                    if (result_01.Status == true)
                    {
                        string first_code = result_01.Value_01;
                        var btn_input_code_box_01 = driver.FindElement(By.Id("iOttText"));
                        Thread.Sleep(RdTimes.T_500());
                        for (int inp_code = 0; inp_code < first_code.Length; inp_code++)
                        {
                            string letter = first_code[inp_code].ToString();
                            btn_input_code_box_01.SendKeys(letter);
                            Thread.Sleep(RdTimes.T_10_20());
                        }
                        Thread.Sleep(RdTimes.T_500());
                        var code_inext_btn = driver.FindElement(By.Id("iNext"));
                        Thread.Sleep(RdTimes.T_1000());
                        code_inext_btn.Click();
                        Thread.Sleep(RdTimes.T_1000());
                        using (PaypalDbContext db = new PaypalDbContext())
                        {
                            var db_account = db.Accounts.Where(x => x.ID == Acc.ID).FirstOrDefault();
                            db_account.RecoveryEmail = recoveryEmail;
                            db_account.Set_Add_RecoveryEmail = true;
                            db_account.Acc_ON_OFF = new Update_Set_Acc_Status().Update(db_account);
                            result.SetAcc_All_Status = db_account.Acc_ON_OFF;
                            db.SaveChanges();
                        }
                        next_step = true;
                    }
                }

                // Step_02 Xác nhận email khôi phục
                if (next_step == true)
                {
                    var confirm_recovery_email_button = driver.FindElement(By.XPath("//div[@id='idDiv_SAOTCS_Proofs']//div[@role='button']"));
                    Thread.Sleep(RdTimes.T_500());
                    confirm_recovery_email_button.Click();
                    Thread.Sleep(RdTimes.T_1000());
                    var second_time_input_recovery_email_box = driver.FindElement(By.Id("idTxtBx_SAOTCS_ProofConfirmation"));
                    Thread.Sleep(RdTimes.T_500());

                    for (int irce = 0; irce < Acc.RecoveryEmail.Length; irce++)
                    {
                        var letter = Acc.RecoveryEmail[irce].ToString();
                        second_time_input_recovery_email_box.SendKeys(letter);
                        Thread.Sleep(RdTimes.T_5_13());
                    }
                    Thread.Sleep(RdTimes.T_500());
                    var agree_send_code_to_recovery_email_button = driver.FindElement(By.Id("idSubmit_SAOTCS_SendCode"));
                    Thread.Sleep(RdTimes.T_300());
                    agree_send_code_to_recovery_email_button.Click();
                    Thread.Sleep(RdTimes.T_2000());

                    //Get code
                    var result_02 = new Get_Code_From_Beeliant().Get_Code(Acc.RecoveryEmail, hold_on);
                    if (result_02.Status == true)
                    {
                        string second_code = result_02.Value_01;
                        var second_time_input_code_from_recovery_email_box = driver.FindElement(By.Id("idTxtBx_SAOTCC_OTC"));
                        Thread.Sleep(RdTimes.T_500());
                        for (int inp_code_02 = 0; inp_code_02 < second_code.Length; inp_code_02++)
                        {
                            string letter = second_code[inp_code_02].ToString();
                            second_time_input_code_from_recovery_email_box.SendKeys(letter);
                            Thread.Sleep(RdTimes.T_10_20());
                        }
                        Thread.Sleep(RdTimes.T_700());
                        var submit_second_code_from_recovery_email_button = driver.FindElement(By.Id("idSubmit_SAOTCC_Continue"));
                        Thread.Sleep(RdTimes.T_300());
                        submit_second_code_from_recovery_email_button.Click();
                        Thread.Sleep(RdTimes.T_2000());
                        change_password = true;
                    }
                }

                if (change_password == true)
                {
                    string new_email_password = new Get_Random_Password().Get_One_Random_Password();
                    using (PaypalDbContext db = new PaypalDbContext())
                    {
                        var db_account = db.Accounts.Where(x => x.ID == Acc.ID).FirstOrDefault();
                        db_account.Random_EmailPassword += new_email_password + "--";
                        db.SaveChanges();
                    }


                    var present_password_box = driver.FindElement(By.Id("iCurPassword"));
                    Thread.Sleep(RdTimes.T_300());
                    for (int e_pass = 0; e_pass < Acc.EmailPassword.Length; e_pass++)
                    {
                        string letter = Acc.EmailPassword[e_pass].ToString();
                        present_password_box.SendKeys(letter);
                        Thread.Sleep(RdTimes.T_10_20());
                    }
                    Thread.Sleep(RdTimes.T_1000());

                    var new_password_box = driver.FindElement(By.Id("iPassword"));
                    Thread.Sleep(RdTimes.T_700());
                    for (int e_pass = 0; e_pass < new_email_password.Length; e_pass++)
                    {
                        string letter = new_email_password[e_pass].ToString();
                        new_password_box.SendKeys(letter);
                        Thread.Sleep(RdTimes.T_10_20());
                    }
                    Thread.Sleep(RdTimes.T_1000());

                    var retype_new_password_box = driver.FindElement(By.Id("iRetypePassword"));
                    Thread.Sleep(RdTimes.T_800());
                    for (int e_pass = 0; e_pass < new_email_password.Length; e_pass++)
                    {
                        string letter = new_email_password[e_pass].ToString();
                        retype_new_password_box.SendKeys(letter);
                        Thread.Sleep(RdTimes.T_10_20());
                    }
                    Thread.Sleep(RdTimes.T_1000());
                    var update_new_password_button = driver.FindElement(By.Id("UpdatePasswordAction"));
                    Thread.Sleep(RdTimes.T_800());
                    update_new_password_button.Click();
                    Thread.Sleep(RdTimes.T_500());
                    
                    driver.Url = "https://outlook.live.com/mail/0/options/mail/layout";
                    driver.Navigate();
                    Thread.Sleep(RdTimes.T_2000());

                    IWebElement confirm_new_password_box_check = null;
                    Thread.Sleep(RdTimes.T_700());
                    try
                    {
                        confirm_new_password_box_check = driver.FindElement(By.Id("i0118"));
                    }
                    catch (Exception) { }

                    if (confirm_new_password_box_check != null)
                    {
                        var confirm_new_password_box = driver.FindElement(By.Id("i0118"));
                        for (int newp = 0; newp < new_email_password.Length; newp++)
                        {
                            var letter = new_email_password[newp].ToString();
                            confirm_new_password_box.SendKeys(letter);
                            Thread.Sleep(RdTimes.T_10_20());
                        }
                        Thread.Sleep(RdTimes.T_300());
                        var confirm_btn_next = driver.FindElement(By.Id("idSIButton9"));
                        Thread.Sleep(RdTimes.T_500());
                        confirm_btn_next.Click();
                        Thread.Sleep(RdTimes.T_2000());
                        result.Status = true;
                    }

                    using (PaypalDbContext db = new PaypalDbContext())
                    {
                        var db_account = db.Accounts.Where(x => x.ID == Acc.ID).FirstOrDefault();
                        db_account.EmailPassword_Old += Acc.EmailPassword + "--";
                        db_account.EmailPassword = new_email_password;
                        db_account.Acc_ON_OFF = new Update_Set_Acc_Status().Update(db_account);
                        result.SetAcc_All_Status = db_account.Acc_ON_OFF;
                        db.SaveChanges();
                    }
                }
            }
            catch { }   

            result.Driver = driver;
            return result;
        }
    }
}
