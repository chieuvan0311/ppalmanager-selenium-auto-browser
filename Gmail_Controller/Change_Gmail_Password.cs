using OpenQA.Selenium;
using PAYPAL.ChromeDrivers;
using PAYPAL.Controller;
using PAYPAL.DataConnection;
using PAYPAL.Models;
using PAYPAL.RandomData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PAYPAL.Gmail_Controller
{
    public class Change_Gmail_Password
    {
        PaypalDbContext db = null;

        public Change_Gmail_Password()
        {
            db = new PaypalDbContext();
        }
        public Session_Result_Model Change_Password(Account Acc, UndectedChromeDriver receive_driver)
        {
            Session_Result_Model result = new Session_Result_Model();
            UndectedChromeDriver driver = receive_driver;
            result.Status = false;

            driver.Get("https://myaccount.google.com/signinoptions/password?");
            Thread.Sleep(RdTimes.T_4000());

            var passwordBox = driver.FindElement(By.XPath("//div[@id='password']//input[@type='password']"));
            for (int i = 0; i < Acc.EmailPassword.Length; i++)
            {
                passwordBox.SendKeys(Acc.EmailPassword[i].ToString());
                Thread.Sleep(RdTimes.T_5_13());
            }
            Thread.Sleep(RdTimes.T_1000());
            var passwordNext = driver.FindElement(By.XPath("//div[@id='passwordNext']//button"));
            Thread.Sleep(RdTimes.T_1000());
            passwordNext.Click();
            Thread.Sleep(RdTimes.T_3000());

            var new_passwordBox = driver.FindElement(By.Id("i6"));
            string new_password = new Get_Random_Password().Get_One_Random_Password();

            Thread.Sleep(RdTimes.T_500());
            for (int i = 0; i < new_password.Length; i++)
            {
                new_passwordBox.SendKeys(new_password[i].ToString());
                Thread.Sleep(RdTimes.T_5_13());
            }
            var new_passwordBox_again = driver.FindElement(By.Id("i10"));
            Thread.Sleep(RdTimes.T_1000());
            for (int i = 0; i < new_password.Length; i++)
            {
                new_passwordBox_again.SendKeys(new_password[i].ToString());
                Thread.Sleep(RdTimes.T_5_13());
            }
            var change_password_btn = driver.FindElement(By.XPath("//div[@class='N1UXxf']//div[@class='VfPpkd-dgl2Hf-ppHlrf-sM5MNb']//button"));
            Thread.Sleep(RdTimes.T_700());
            change_password_btn.Click();

            db.Accounts.Where(x => x.ID == Acc.ID).FirstOrDefault().Random_EmailPassword += new_password + "--";
            db.SaveChanges();

            Thread.Sleep(RdTimes.T_3000());
            driver.Get("https://mail.google.com/mail/");
            IWebElement check_agian = null;
            Thread.Sleep(RdTimes.T_3000());
            try { check_agian = driver.FindElement(By.Id("loading")); } catch { }
            if (check_agian != null)
            {
                //Save data
                var data_account = db.Accounts.Where(x => x.ID == Acc.ID).FirstOrDefault();
                data_account.EmailPassword_Old += Acc.EmailPassword + "--";
                data_account.EmailPassword = new_password;
                data_account.Set_ChangedPassEmail = true;
                data_account.Acc_ON_OFF = new Update_Set_Acc_Status().Update(data_account);
                result.SetAcc_All_Status = data_account.Acc_ON_OFF;
                db.SaveChanges();
                result.Status = true;
            }
            result.Driver = driver;
            return result;
        }
    }
}
