using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using PAYPAL.ChromeDrivers;
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

namespace PAYPAL.Controller
{
        public class Change_Paypal_Password_Controller
        {
        public Session_Result_Model Change (Account Acc, UndectedChromeDriver receive_driver)
        {
            Session_Result_Model result = new Session_Result_Model();
            UndectedChromeDriver driver = receive_driver;
            result.Status = false;
            try
            {
                driver.Url = "https://www.paypal.com/myaccount/security/password/change";
                driver.Navigate();
                Thread.Sleep(RdTimes.T_2000());
                var paypal_present_pass_box = driver.FindElement(By.Id("password_change_current"));
                Thread.Sleep(RdTimes.T_400());
                for (int p = 0; p < Acc.AccPassword.Length; p++)
                {
                    var pletter = Acc.AccPassword[p].ToString();
                    paypal_present_pass_box.SendKeys(pletter);
                    Thread.Sleep(RdTimes.T_10_20());
                }
                Thread.Sleep(RdTimes.T_900());

                var paypal_new_pass_box = driver.FindElement(By.Id("password_change_new"));
                Thread.Sleep(RdTimes.T_500());
                string new_password = new Get_Random_Password().Get_One_Random_Password();
                using (PaypalDbContext db = new PaypalDbContext())
                {
                    var db_account = db.Accounts.Where(x => x.ID == Acc.ID).FirstOrDefault();
                    db_account.Random_AccPassword += new_password + "--";
                    db.SaveChanges();
                }

                for (int p = 0; p < new_password.Length; p++)
                {
                    var pletter = new_password[p].ToString();
                    paypal_new_pass_box.SendKeys(pletter);
                    Thread.Sleep(RdTimes.T_10_20());
                }
                Thread.Sleep(RdTimes.T_1000());

                var paypal_new_pass_confirm_box = driver.FindElement(By.Id("password_change_confirm"));
                for (int p = 0; p < new_password.Length; p++)
                {
                    var pletter = new_password[p].ToString();
                    paypal_new_pass_confirm_box.SendKeys(pletter);
                    Thread.Sleep(RdTimes.T_10_20());
                }
                Thread.Sleep(RdTimes.T_1000());
                var paypal_change_pass_submit_btn = driver.FindElement(By.XPath("//div[@id='_flowModal-container']//button[@data-nemo='change_password_submit']"));
                Thread.Sleep(RdTimes.T_1000());
                paypal_change_pass_submit_btn.Click();
                Thread.Sleep(RdTimes.T_1500());

                driver.Url = "https://www.paypal.com/myaccount/summary";
                driver.Navigate();
                Thread.Sleep(RdTimes.T_2000());
                IWebElement check_again_01 = null;
                IWebElement check_again_02 = null;
                try { check_again_01 = driver.FindElement(By.Id("password")); } catch (Exception) { }
                try { check_again_02 = driver.FindElement(By.Id("btnLogin")); } catch (Exception) { }
                Thread.Sleep(RdTimes.T_500());
                if (check_again_01 != null && check_again_02 != null)
                {
                    for (int p = 0; p < new_password.Length; p++)
                    {
                        var pletter = new_password[p].ToString();
                        check_again_01.SendKeys(pletter);
                        Thread.Sleep(RdTimes.T_10_20());
                    }
                    Thread.Sleep(RdTimes.T_700());
                    check_again_02.Click();
                }

                using (PaypalDbContext db = new PaypalDbContext())
                {
                    var db_account = db.Accounts.Where(x => x.ID == Acc.ID).FirstOrDefault();
                    db_account.AccPassword_Old += Acc.AccPassword + "--";
                    db_account.AccPassword = new_password;
                    db_account.Acc_ON_OFF = new Update_Set_Acc_Status().Update(db_account);
                    result.SetAcc_All_Status = db_account.Acc_ON_OFF;
                    db.SaveChanges();
                }

                result.Status = true;
            }
            catch { }

            result.Driver = driver;
            return result;
        }
    }
}
