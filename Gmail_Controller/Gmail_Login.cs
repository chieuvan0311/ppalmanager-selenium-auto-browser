using OpenQA.Selenium;
using PAYPAL.ChromeDrivers;
using PAYPAL.DataConnection;
using PAYPAL.Models;
using PAYPAL.RandomData;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace PAYPAL.Gmail_Controller
{
    public class Gmail_Login
    {
        PaypalDbContext db = null;
        public Gmail_Login()
        {
            db = new PaypalDbContext();
        }

        public Session_Result_Model Login(Account Acc, UndectedChromeDriver receive_driver, bool hold_on)
        {
            Session_Result_Model result = new Session_Result_Model();
            UndectedChromeDriver driver = receive_driver; ;
            result.Status = false;
            try
            {
                driver.Get("https://accounts.google.com");
                Thread.Sleep(RdTimes.T_3000());
                IWebElement check_login = null;
                try { check_login = driver.FindElement(By.XPath("//div[@class='wrDwse']//div[@class='tC9kZd']//nav[@class='ky9loc']")); } catch { }
                Thread.Sleep(RdTimes.T_1000());
                if (check_login != null)
                {
                    result.Status = true;
                }
                else
                {
                    var emailBox = driver.FindElement(By.Id("identifierId"));
                    for (int i = 0; i < Acc.Email.Length; i++)
                    {
                        emailBox.SendKeys(Acc.Email[i].ToString());
                        Thread.Sleep(RdTimes.T_5_13());
                    }
                    Thread.Sleep(RdTimes.T_600());
                    var emailNext = driver.FindElement(By.XPath("//div[@id='identifierNext']//button"));

                    Thread.Sleep(RdTimes.T_600());
                    emailNext.Click();

                    Thread.Sleep(RdTimes.T_3000());

                    var passBox = driver.FindElement(By.XPath("//div[@id='password']//input[@type='password']"));
                    Thread.Sleep(2000);

                    for (int i = 0; i < Acc.EmailPassword.Length; i++)
                    {
                        passBox.SendKeys(Acc.EmailPassword[i].ToString());
                        Thread.Sleep(RdTimes.T_5_13());
                    }
                    Thread.Sleep(RdTimes.T_1000());
                    var passwordNext = driver.FindElement(By.XPath("//div[@id='passwordNext']//button"));
                    Thread.Sleep(RdTimes.T_1500());
                    passwordNext.Click();
                    if (hold_on == true)
                    {
                        StringWriter login_info = new StringWriter();
                        login_info.Write(Acc.Email);
                        login_info.Write("|");
                        login_info.Write(Acc.EmailPassword);
                        login_info.Write("|");
                        if (!string.IsNullOrEmpty(Acc.RecoveryEmail)) { login_info.Write(Acc.RecoveryEmail); } else { login_info.Write("Chưa có email KP"); }

                        Clipboard.SetText(login_info.ToString());

                        MessageBox.Show("Chỉ click OK khi đã đang nhập email thành công, đã copy email + mật khẩu + email fw", "Tạm dừng", MessageBoxButtons.OK,

                                        MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                        Thread.Sleep(RdTimes.T_700());
                    }
                    else
                    {
                        Thread.Sleep(RdTimes.T_4000());
                    }
                    driver.Get("https://mail.google.com/mail/");
                    IWebElement check_again = null;
                    try { check_again = driver.FindElement(By.Id("loading")); } catch { }
                    Thread.Sleep(RdTimes.T_1000());
                    if (check_again != null)
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
