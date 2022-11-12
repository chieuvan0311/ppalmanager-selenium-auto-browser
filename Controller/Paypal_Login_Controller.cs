using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using PAYPAL.ChromeDrivers;
using PAYPAL.DataConnection;
using PAYPAL.Models;
using PAYPAL.RandomData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PAYPAL.Controller
{
    public class Paypal_Login_Controller
    {
        public Session_Result_Model Paypal_Login (Account Acc, UndectedChromeDriver receive_driver, bool hold_on = false)
        {
            Session_Result_Model result = new Session_Result_Model();
            UndectedChromeDriver driver = receive_driver;
            result.Status = false;
            try 
            {
                driver.Get("https://www.paypal.com/signin/");
                Thread.Sleep(RdTimes.T_3000());
                IWebElement check_login = null;
                try 
                {
                    check_login = driver.FindElement(By.XPath("//div[@id='cwBalance']//div[@data-test-id='available-balance']"));
                }
                catch { }

                if(check_login != null) 
                {
                    result.Status = true;
                }
                else 
                {
                    var emailBox = driver.FindElement(By.Id("email"));
                    var email_text = emailBox.GetAttribute("value");
                    Thread.Sleep(RdTimes.T_300());

                    if (email_text != Acc.Email)
                    {
                        Thread.Sleep(RdTimes.T_1500());
                        emailBox.Clear();
                        Thread.Sleep(RdTimes.T_100());
                        for (int h = 0; h < Acc.Email.Length; h++)
                        {
                            var letter = Acc.Email[h].ToString();
                            emailBox.SendKeys(letter);
                            Thread.Sleep(RdTimes.T_10_20()); //Time_10_20s
                        }
                        Thread.Sleep(RdTimes.T_900());  //Time_900s
                    }

                    try
                    {
                        var btnNext = driver.FindElement(By.Id("btnNext"));
                        btnNext.Click();
                        Thread.Sleep(RdTimes.T_2000()); //Time_2000s
                    }
                    catch { }

                    var paypal_password_Box = driver.FindElement(By.Id("password"));
                    Thread.Sleep(RdTimes.T_1000());

                    for (int p = 0; p < Acc.AccPassword.Length; p++)
                    {
                        var pletter = Acc.AccPassword[p].ToString();
                        paypal_password_Box.SendKeys(pletter);
                        Thread.Sleep(RdTimes.T_10_20()); 
                    }
                    Thread.Sleep(RdTimes.T_500()); 
                    var btnLogin = driver.FindElement(By.Id("btnLogin"));
                    Thread.Sleep(RdTimes.T_1000()); 
                    btnLogin.Click();

                    if (hold_on == true) 
                    {
                        MessageBox.Show("- Lưu ý: chỉ click OK sau khi đăng nhập thành công!", "Form hướng dẫn", MessageBoxButtons.OK,
                                        MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                        Thread.Sleep(RdTimes.T_500());
                        driver.Get("https://www.paypal.com/myaccount/summary");
                        Thread.Sleep(RdTimes.T_2000());
                    }
                    else
                    {
                        Thread.Sleep(RdTimes.T_1500());
                        driver.Get("https://www.paypal.com/myaccount/summary");
                        Thread.Sleep(RdTimes.T_4000());
                    }   
                    
                    IWebElement check_login_01 = null;
                    try { check_login_01 = driver.FindElement(By.XPath("//div[@id='cwBalance']//div[@data-test-id='available-balance']")); } catch (Exception) { }
                    if (check_login_01 != null)
                    {
                        result.Status = true;
                    }
                    else 
                    { 
                        if(Acc.AccType == "Chưa set")
                        {
                            PaypalDbContext db = new PaypalDbContext();
                            var db_account = db.Accounts.Where(x => x.ID == Acc.ID).FirstOrDefault();
                            db_account.AccType = "Set fail";
                            db.SaveChanges();
                            result.Value_01 = "Set fail";
                        }
                    }
                }
            }
            catch { }

            result.Driver = driver;
            return result;
        }
    }
}
