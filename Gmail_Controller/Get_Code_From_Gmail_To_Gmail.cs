using OpenQA.Selenium;
using PAYPAL.ChromeDrivers;
using PAYPAL.DataConnection;
using PAYPAL.GPM_API;
using PAYPAL.RandomData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PAYPAL.Gmail_Controller
{
    public class Get_Code_From_Gmail_To_Gmail
    {
        PaypalDbContext db = null;
        public Get_Code_From_Gmail_To_Gmail()
        {
            db = new PaypalDbContext();
        }
        public string Get_Code(string search_keyword)
        {
            UndectedChromeDriver gmail_driver = null;
            string code = "";
            try
            {
                string fw_email_profileId = db.Admins.Where(x => x.Name == "Forward_Email_Gmail").FirstOrDefault().Value;
                var result = new Open_Profiles().Open(fw_email_profileId);
                gmail_driver = result.Driver;
                gmail_driver.Manage().Window.Position = new Point(-3200, -3200);
                gmail_driver.Manage().Window.Size = new Size(1800, 900);

                gmail_driver.Get("https://mail.google.com/mail");
                Thread.Sleep(RdTimes.T_1000());
                var search_box = gmail_driver.FindElement(By.XPath("//div[@id='gs_lc50']//input"));
                Thread.Sleep(RdTimes.T_1000());
                search_box.SendKeys(search_keyword);
                Thread.Sleep(RdTimes.T_500());
                var search_BTN = gmail_driver.FindElement(By.XPath("//form[@id='aso_search_form_anchor']//button[@class='gb_mf gb_nf']"));
                Thread.Sleep(RdTimes.T_400());
                search_BTN.Click();

                Thread.Sleep(RdTimes.T_800());
                var code_email_BTN_count = gmail_driver.FindElements(By.XPath("//div[@id=':1']//div[@class='ae4 UI UJ nH oy8Mbf id']//table[@role='grid']//tbody//tr[@role='row']"));

                Thread.Sleep(RdTimes.T_1500());
                var code_email_BTN = code_email_BTN_count[0];
                Thread.Sleep(RdTimes.T_1000());
                code_email_BTN.Click();
                Thread.Sleep(RdTimes.T_1000());

                var get_code_element = gmail_driver.FindElement(By.XPath("//div[@class='nH V8djrc byY']//div[@class='nH']//div[@class='ha']//h2"));
                //nH V8djrc byY
                Thread.Sleep(RdTimes.T_600());
                string h2_text = get_code_element.Text;
                Thread.Sleep(RdTimes.T_400());

                for (int i = 2; i <= 9; i++)
                {
                    code += h2_text[i].ToString();
                }

                var num10 = h2_text[10].ToString();
                if (num10 == "0" || num10 == "1" || num10 == "2" || num10 == "3" || num10 == "4" || num10 == "5" || num10 == "6" || num10 == "7" || num10 == "8" || num10 == "9")
                {
                    code += num10;
                }

                var num11 = h2_text[11].ToString();
                if (num11 == "0" || num11 == "1" || num11 == "2" || num11 == "3" || num11 == "4" || num11 == "5" || num11 == "6" || num11 == "7" || num11 == "8" || num11 == "9")
                {
                    code += num11;
                }

                Thread.Sleep(RdTimes.T_500());

                try { gmail_driver.Close(); } catch { }
                try { gmail_driver.Quit(); } catch { }
                try { gmail_driver.Dispose(); } catch { }
            }
            catch
            {
                try { gmail_driver.Close(); } catch { }
                try { gmail_driver.Quit(); } catch { }
                try { gmail_driver.Dispose(); } catch { }
            }

            return code;
        }
    }
}
