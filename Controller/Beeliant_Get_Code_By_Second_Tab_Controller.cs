using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
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
    public class Beeliant_Get_Code_By_Second_Tab_Controller
    {
        public ChromeDriver_StringValues_Model Get_Code (ChromeDriver receive_driver, string recovery_email, bool chrome_OFF) 
        {
            ChromeDriver driver = receive_driver;
            string code = "";
            ChromeDriver_StringValues_Model model = new ChromeDriver_StringValues_Model();
            try 
            {
                IJavaScriptExecutor js_driver = driver as IJavaScriptExecutor;
                js_driver.ExecuteScript("window.open();");
                Thread.Sleep(RdTimes.T_500());
                driver.SwitchTo().Window(driver.WindowHandles.Last());
                Thread.Sleep(RdTimes.T_300());
                driver.Navigate().GoToUrl("https://mail.beeliant.com/");
                Thread.Sleep(RdTimes.T_3000());

                var beeliant_email_box = driver.FindElement(By.Id("passp-field-login"));
                Thread.Sleep(RdTimes.T_1000());
                beeliant_email_box.SendKeys("admin@beeliant.com");
                Thread.Sleep(RdTimes.T_1000());
                var beeliant_email_next_btn = driver.FindElement(By.Id("passp:sign-in"));
                Thread.Sleep(RdTimes.T_500());
                beeliant_email_next_btn.Click();
                Thread.Sleep(RdTimes.T_500());
                var beeliant_password_box = driver.FindElement(By.Id("passp-field-passwd"));
                Thread.Sleep(RdTimes.T_300());
                beeliant_password_box.SendKeys("erQE5PreTU-3eRS(nmJv");
                Thread.Sleep(RdTimes.T_1000());
                var beeliant_password_next_btn = driver.FindElement(By.Id("passp:sign-in"));
                Thread.Sleep(RdTimes.T_500());
                beeliant_password_next_btn.Click();
                Thread.Sleep(RdTimes.T_1000());

                driver.Url = "https://mail.yandex.ru/";
                driver.Navigate();
                Thread.Sleep(RdTimes.T_5000());

                var search_box = driver.FindElement(By.XPath("//div[@id='js-apps-container']//div[@class='search-input__text-bubble-container']//span//input"));
                Thread.Sleep(RdTimes.T_500());
                for (int irce_02 = 0; irce_02 < recovery_email.Length; irce_02++)
                {
                    var letter = recovery_email[irce_02].ToString();
                    search_box.SendKeys(letter);
                    Thread.Sleep(RdTimes.T_5_13());
                }
                Thread.Sleep(RdTimes.T_1500());

                var search_btn = driver.FindElement(By.XPath("//div[@id='js-apps-container']//form[@class='search-input__form']//button"));
                Thread.Sleep(RdTimes.T_1000());
                search_btn.Click();
                Thread.Sleep(RdTimes.T_3000());

                var new_email_btn = driver.FindElement(By.XPath("//div[@id='js-apps-container']//div[@class='ns-view-container-desc mail-MessagesList js-messages-list']//div[@data-key='box=messages-item-box']"));
                Thread.Sleep(RdTimes.T_1000());
                try { new_email_btn.Click(); } catch (Exception) { }
                Thread.Sleep(RdTimes.T_3000());

                var confirm_code_span = driver.FindElement(By.XPath("//div[@id='js-apps-container']//main[@class='mail-Layout-Content']//tbody//span"));
                Thread.Sleep(RdTimes.T_1000());
                code = confirm_code_span.Text;
                Thread.Sleep(RdTimes.T_1000());
                driver.Close();
                driver.SwitchTo().Window(driver.WindowHandles.Last());
                Thread.Sleep(RdTimes.T_1000());
                
                model.Driver = driver;
                model.String_Values_01 = code;
            }
            catch (Exception)
            {
                if(chrome_OFF == true) 
                {
                    driver.Quit();
                }
            }           
            return model;
        }
    }
}
