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
    public class Remove_ForwardEmail_Hotmail
    {
        public Session_Result_Model Remove (Account Acc, UndectedChromeDriver receive_driver)
        {
            Session_Result_Model result = new Session_Result_Model();
            UndectedChromeDriver driver = receive_driver;
            result.Status = false;
            try 
            {
                driver.Url = "https://outlook.live.com/mail/0/options/mail/layout";
                driver.Navigate();
                Thread.Sleep(RdTimes.T_3000());

                IWebElement confirm_new_password_box_check = null;
                Thread.Sleep(RdTimes.T_700());
                try
                {
                    confirm_new_password_box_check = (IWebElement)driver.FindElement(By.Id("i0118"));
                }
                catch (Exception) { }

                if (confirm_new_password_box_check != null)
                {
                    var confirm_new_password_box = driver.FindElement(By.Id("i0118"));
                    for (int newp = 0; newp < Acc.EmailPassword.Length; newp++)
                    {
                        var letter = Acc.EmailPassword[newp].ToString();
                        confirm_new_password_box.SendKeys(letter);
                        Thread.Sleep(RdTimes.T_10_20());
                    }
                    Thread.Sleep(RdTimes.T_300());
                    var confirm_btn_next = driver.FindElement(By.Id("idSIButton9"));
                    Thread.Sleep(RdTimes.T_500());
                    confirm_btn_next.Click();
                    Thread.Sleep(RdTimes.T_2000());
                }
                Thread.Sleep(RdTimes.T_10000());
                driver.Url = "https://outlook.live.com/mail/0/options/mail/rules";
                driver.Navigate();
                Thread.Sleep(RdTimes.T_10000());

                var old_forward_Elements = driver.FindElements(By.XPath("//div[@class='b48_E']//div[@draggable='true']//button[@title='Delete rule']"));
                if (old_forward_Elements.Count > 0)
                {
                    for (int de = 0; de < old_forward_Elements.Count; de++)
                    {
                        if (de >= 1)
                        {
                            driver.Url = "https://outlook.live.com/mail/0/options/mail/rules";
                            driver.Navigate();
                            Thread.Sleep(RdTimes.T_13000());
                            var delElement = driver.FindElements(By.XPath("//div[@class='b48_E']//div[@draggable='true']//button[@title='Delete rule']"));
                            delElement[0].Click();
                            Thread.Sleep(RdTimes.T_1500());
                            var deleteButton = driver.FindElement(By.Id("ok-1"));
                            Thread.Sleep(RdTimes.T_1000());
                            deleteButton.Click();
                            Thread.Sleep(RdTimes.T_500());
                        }
                        else
                        {
                            old_forward_Elements[0].Click();
                            Thread.Sleep(RdTimes.T_1000());
                            var deleteButton = driver.FindElement(By.Id("ok-1"));
                            Thread.Sleep(RdTimes.T_1000());
                            deleteButton.Click();
                            Thread.Sleep(RdTimes.T_500());
                        }
                    }
                }
                Thread.Sleep(RdTimes.T_2000());

                var checkElement = driver.FindElements(By.XPath("//div[@class='b48_E']//div[@draggable='true']//button[@title='Delete rule']"));
                Thread.Sleep(RdTimes.T_500());
                if (checkElement.Count == 0)
                {
                    using (PaypalDbContext db = new PaypalDbContext())
                    {
                        var db_account = db.Accounts.Where(x => x.ID == Acc.ID).FirstOrDefault();
                        db_account.Set_Deleted_FwEmail = true;
                        db_account.Acc_ON_OFF = new Update_Set_Acc_Status().Update(db_account);
                        result.SetAcc_All_Status = db_account.Acc_ON_OFF;
                        db.SaveChanges();
                    }
                    result.Status = true;
                }
            }
            catch { }
            
            result.Driver = driver;
            return result;
        }
    }
}
