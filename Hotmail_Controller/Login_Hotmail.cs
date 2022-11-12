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
using System.Windows.Forms;

namespace PAYPAL.Hotmail_Controller
{
    public class Login_Hotmail
    {
        public Session_Result_Model Login (Account Acc, UndectedChromeDriver receive_driver)
        {
            Session_Result_Model result = new Session_Result_Model();
            UndectedChromeDriver driver = receive_driver;
            result.Status = false;
            try 
            {
                driver.Url = "https://login.live.com/login.srf?";
                driver.Navigate();
                Thread.Sleep(RdTimes.T_4000());

                IWebElement check_login = null;
                try { check_login = driver.FindElement(By.Id("main-content-landing")); } catch { }
                Thread.Sleep(RdTimes.T_600());
                if(check_login != null) 
                {
                    result.Status = true;
                }
                else 
                {
                    var emailBox = driver.FindElement(By.Id("i0116"));
                    Thread.Sleep(RdTimes.T_500());
                    emailBox.Clear();
                    Thread.Sleep(RdTimes.T_200());
                    for (int h = 0; h < Acc.Email.Length; h++)
                    {
                        var letter = Acc.Email[h].ToString();
                        emailBox.SendKeys(letter);
                        Thread.Sleep(RdTimes.T_3_8());
                    }
                    var btnNext = driver.FindElement(By.Id("idSIButton9"));
                    Thread.Sleep(RdTimes.T_1000());
                    btnNext.Click();
                    Thread.Sleep(RdTimes.T_1500());
                    var login_password_Box = driver.FindElement(By.Id("i0118"));
                    Thread.Sleep(RdTimes.T_1500());
                    login_password_Box.Clear();
                    Thread.Sleep(RdTimes.T_200());
                    for (int p = 0; p < Acc.EmailPassword.Length; p++)
                    {
                        var letter = Acc.EmailPassword[p].ToString();
                        login_password_Box.SendKeys(letter);
                        Thread.Sleep(RdTimes.T_10_20()); 
                    }
                    var btnNext1 = driver.FindElement(By.Id("idSIButton9"));
                    Thread.Sleep(RdTimes.T_500()); 
                    btnNext1.Click();
                    Thread.Sleep(RdTimes.T_1000());
                    var btnNo = driver.FindElement(By.Id("idBtn_Back"));
                    Thread.Sleep(RdTimes.T_600());
                    btnNo.Click();
                    Thread.Sleep(RdTimes.T_4000());

                    IWebElement check_login_01 = null;
                    try { check_login_01 = driver.FindElement(By.Id("main-content-landing")); } catch { }
                    Thread.Sleep(RdTimes.T_600());
                    if (check_login_01 != null)
                    {
                        result.Status = true;
                    }
                }            
            }
            catch { }

            result.Driver = driver;
            return result;
        }
    }
}
