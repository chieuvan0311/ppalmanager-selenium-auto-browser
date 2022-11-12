using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using PAYPAL.RandomData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;
using PAYPAL.DataConnection;
using PAYPAL.GPM_API;
using Newtonsoft.Json.Linq;
using PAYPAL.ChromeDrivers;
using PAYPAL.Models;

namespace PAYPAL.Controller
{
    public class Get_Code_From_Beeliant
    {
        public Session_Result_Model Get_Code(string recoveryEmail, bool hold_on = false)
        {
            Session_Result_Model result = new Session_Result_Model();
            result.Status = false;
            try 
            {
                string bee_profileId = "";

                using (PaypalDbContext db = new PaypalDbContext())
                {
                    bee_profileId = db.Admins.Where(x => x.Name == "admin@beeliant.com").FirstOrDefault().Value;
                }

                GPMLoginAPI api = new GPMLoginAPI();
                JObject startedResult = api.Start(bee_profileId);

                if (startedResult != null)
                {
                    string browserLocation = Convert.ToString(startedResult["browser_location"]);
                    string seleniumRemoteDebugAddress = Convert.ToString(startedResult["selenium_remote_debug_address"]);
                    string gpmDriverPath = Convert.ToString(startedResult["selenium_driver_location"]);

                    // Init selenium
                    FileInfo gpmDriverFileInfo = new FileInfo(gpmDriverPath);

                    ChromeDriverService service = ChromeDriverService.CreateDefaultService(gpmDriverFileInfo.DirectoryName, gpmDriverFileInfo.Name);
                    service.HideCommandPromptWindow = true;
                    ChromeOptions options = new ChromeOptions();
                    options.BinaryLocation = browserLocation;
                    options.DebuggerAddress = seleniumRemoteDebugAddress;

                    UndectedChromeDriver beeliant_driver = new UndectedChromeDriver(service, options);

                    beeliant_driver.Get("https://mail.yandex.ru/");
                    Thread.Sleep(RdTimes.T_4000());

                    var search_box = beeliant_driver.FindElement(By.XPath("//div[@id='js-apps-container']//div[@class='search-input__text-bubble-container']//span//input"));
                    Thread.Sleep(RdTimes.T_500());
                    for (int irce_02 = 0; irce_02 < recoveryEmail.Length; irce_02++)
                    {
                        var letter = recoveryEmail[irce_02].ToString();
                        search_box.SendKeys(letter);
                        Thread.Sleep(RdTimes.T_3_8());
                    }
                    Thread.Sleep(RdTimes.T_1500());

                    var search_btn = beeliant_driver.FindElement(By.XPath("//div[@id='js-apps-container']//form[@class='search-input__form']//button"));
                    Thread.Sleep(RdTimes.T_1000());
                    search_btn.Click();
                    Thread.Sleep(RdTimes.T_3000());

                    var new_email_btn_03 = beeliant_driver.FindElement(By.XPath("//div[@id='js-apps-container']//div[@class='ns-view-container-desc mail-MessagesList js-messages-list']//div[@data-key='box=messages-item-box']"));
                    Thread.Sleep(RdTimes.T_1000());
                    try { new_email_btn_03.Click(); } catch (Exception) { }
                    Thread.Sleep(RdTimes.T_3000());

                    var confirm_code_span = beeliant_driver.FindElements(By.XPath("//div[@id='js-apps-container']//tbody//td//div[@class='8d761f5edc830eb1mdv2rw']//div//div"))[1];
                    Thread.Sleep(RdTimes.T_1000());

                    //return
                    result.Value_01 = confirm_code_span.Text;
                    result.Status = true;

                    Thread.Sleep(RdTimes.T_500());
                    if (hold_on == true)
                    {
                        beeliant_driver.Quit();
                        beeliant_driver.Dispose();
                    }
                    else
                    {
                        beeliant_driver.Close();
                        beeliant_driver.Quit();
                        beeliant_driver.Dispose();
                    }
                }
            }
            catch { }
            
            return result;
        }
    }
}
