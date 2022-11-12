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
using System.Windows.Forms;

namespace PAYPAL.Gmail_Controller
{
    public class Add_New_Forward_Email_Gmail
    {
        PaypalDbContext db = null;
        public Add_New_Forward_Email_Gmail()
        {
            db = new PaypalDbContext();
        }

        public Session_Result_Model Add(Account Acc, UndectedChromeDriver receive_driver)
        {
            Session_Result_Model result = new Session_Result_Model();
            result.Status = false;
            UndectedChromeDriver driver = receive_driver;
            try
            {
                driver.Get("https://mail.google.com/mail/u/0/#settings/fwdandpop");
                Thread.Sleep(RdTimes.T_4000());
                var add_fw_email_btn = driver.FindElement(By.XPath("//div[@class='nH Tv1JD']//div[@class='nH r4']//table//td//input[@act='add']"));
                Thread.Sleep(RdTimes.T_900());
                add_fw_email_btn.Click();
                var add_fw_email_input = driver.FindElement(By.XPath("//div[@class='Kj-JD']//div[@class='Kj-JD-Jz']//div[@class='PN']//input"));
                Thread.Sleep(RdTimes.T_1500());
                add_fw_email_input.SendKeys(Acc.ForwordToEmail);
                Thread.Sleep(RdTimes.T_1500());
                var next_btn = driver.FindElement(By.XPath("//div[@class='Kj-JD-Jl']//button[@name='next']"));
                Thread.Sleep(RdTimes.T_900());
                next_btn.Click();
                Thread.Sleep(RdTimes.T_1000());

                driver.SwitchTo().Window(driver.WindowHandles.Last());
                Thread.Sleep(RdTimes.T_1000());
                var accept_btn = driver.FindElements(By.XPath("//table//td//input[@type='submit']"));
                Thread.Sleep(RdTimes.T_1000());
                var countaaa = accept_btn.Count();
                accept_btn[0].Click();
                Thread.Sleep(RdTimes.T_1000());
                driver.SwitchTo().Window(driver.WindowHandles.Last());
                Thread.Sleep(RdTimes.T_500());
                var ok_button = driver.FindElements(By.XPath("//div[@class='Kj-JD']//div[@class='Kj-JD-Jl']//button[@name='ok']"));
                Thread.Sleep(RdTimes.T_1000());
                var countttttt = ok_button.Count();
                ok_button[0].Click();
                Thread.Sleep(RdTimes.T_1000());

                string code_number = new Get_Code_From_Gmail_To_Gmail().Get_Code(Acc.Email);
                var input_code_box = driver.FindElements(By.XPath("//div[@class='nH Tv1JD']//div[@class='nH r4']//table//table//td//input[@type='text']"));
                Thread.Sleep(RdTimes.T_1000());
                input_code_box[0].Clear();
                Thread.Sleep(RdTimes.T_300());
                for (int i = 0; i < code_number.Length; i++)
                {
                    input_code_box[0].SendKeys(code_number[i].ToString());
                    Thread.Sleep(RdTimes.T_5_13());
                }

                Thread.Sleep(RdTimes.T_1000());
                var submit_code_box = driver.FindElements(By.XPath("//div[@class='nH Tv1JD']//div[@class='nH r4']//table//table//td//input[@type='button']"));
                Thread.Sleep(RdTimes.T_1000());
                submit_code_box[0].Click();
                Thread.Sleep(RdTimes.T_4000());

                var filter_btn = driver.FindElement(By.XPath("//div[@class='nH Tv1JD']//div[@class='nH r4']//table//tbody//td//div//div//span[@role='link']"));
                Thread.Sleep(RdTimes.T_3000());
                filter_btn.Click();
                Thread.Sleep(RdTimes.T_2000());

                var input_keyword_box = driver.FindElements(By.XPath("//div[@class='ZF-Av']//div[@class='SK ZF-zT']//div[@class='w-Nw boo']//span//input[@type='text']"));
                Thread.Sleep(RdTimes.T_1000());
                input_keyword_box[3].SendKeys("paypal");
                Thread.Sleep(RdTimes.T_1000());
                var btn_next = driver.FindElement(By.XPath("//div[@class='ZF-Av']//div[@class='SK ZF-zT']//div[@role='link']"));
                Thread.Sleep(RdTimes.T_700());
                btn_next.Click();

                Thread.Sleep(RdTimes.T_1000());
                var check_box = driver.FindElement(By.XPath("//div[@class='ZF-Av']//div[@class='SK ZF-zT']//span//label"));
                Thread.Sleep(RdTimes.T_1000());
                check_box.Click();
                Thread.Sleep(RdTimes.T_1000());

                var list_box = driver.FindElement(By.XPath("//div[@class='ZF-Av']//div[@class='SK ZF-zT']//span//div[@role='listbox']"));
                Thread.Sleep(RdTimes.T_1000());
                list_box.Click();
                Thread.Sleep(RdTimes.T_1000());
                var list_box_option = driver.FindElement(By.XPath("//div[@class='ZF-Av']//div[@class='SK ZF-zT']//span//div[@role='option']//div"));
                Thread.Sleep(RdTimes.T_1000());
                list_box_option.Click();
                Thread.Sleep(RdTimes.T_1000());
                var create_filter_btn = driver.FindElement(By.XPath("//div[@class='ZF-Av']//div[@class='SK ZF-zT']//div[@class='ZZ']//div[@class='btl bti']//div[@role='button']"));
                Thread.Sleep(RdTimes.T_1000());
                create_filter_btn.Click();
                Thread.Sleep(RdTimes.T_3000());
                driver.Get("https://mail.google.com/mail/u/0/#settings/fwdandpop");
                Thread.Sleep(RdTimes.T_4000());

                var check_text = driver.FindElement(By.XPath("//table[@class='cf bA1']//tbody//td//span//select//option[@value='0']")).Text;

                if (check_text.Length > (Acc.ForwordToEmail.Length + 5))
                {
                    var account = db.Accounts.Where(x => x.ID == Acc.ID).FirstOrDefault();
                    account.Set_Add_New_FwEmail = true;
                    account.Acc_ON_OFF = new Update_Set_Acc_Status().Update(account);
                    result.SetAcc_All_Status = account.Acc_ON_OFF;
                    db.SaveChanges();
                    result.Status = true;
                }
            }
            catch { }
            //Trả về kết quả
            result.Driver = driver;
            return result;
        }
    }
}
