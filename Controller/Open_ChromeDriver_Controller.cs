using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using PAYPAL.Dao;
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
    public class Open_ChromeDriver_Controller
    {
        ChromeDriver driver = null;
        private string ExtensionFolderPath = "Extension";
        public Open_Chrome_With_Proxy_Model Open_Chrome_With_Proxy (Acc_Proxy_Info_Model Acc)
        {
            Open_Chrome_With_Proxy_Model model = new Open_Chrome_With_Proxy_Model();
            try
            {
                string proxy_note = Acc.ProxyStatus;
                bool proxy_status_good = false;
                if (!string.IsNullOrEmpty(Acc.ProxyIP) && !string.IsNullOrEmpty(Acc.ProxyPort) && !string.IsNullOrEmpty(Acc.ProxyName) && !string.IsNullOrEmpty(Acc.ProxyPassword))
                {
                    string ip = Acc.ProxyIP + ":" + Acc.ProxyPort;
                    ChromeOptions options = new ChromeOptions();
                    options.AddExtension(ExtensionFolderPath + "\\Proxy Auto Auth.crx");
                    options.AddExtension(ExtensionFolderPath + "\\WebRTC Control.crx");
                    options.AddExtension(ExtensionFolderPath + "\\WebGL Defender.crx");
                    options.AddExtension(ExtensionFolderPath + "\\Canvas Defender.crx");
                    options.AddExtension(ExtensionFolderPath + "\\AudioContext Defender.crx");
                    options.AddExtension(ExtensionFolderPath + "\\Font Defender.crx");
                    options.AddArguments("start-maximized");
                    options.AddArgument(string.Format("--proxy-server={0}", ip));
                    var driverService = ChromeDriverService.CreateDefaultService();
                    driverService.HideCommandPromptWindow = true;
                    driver = new ChromeDriver(driverService, options);
                    Thread.Sleep(RdTimes.T_400());
                    driver.Close();
                    Thread.Sleep(RdTimes.T_1000());
                    driver.SwitchTo().Window(driver.WindowHandles.Last());
                    Thread.Sleep(RdTimes.T_300());

                    //Set proxy
                    driver.Url = "chrome-extension://ggmdpepbjljkkkdaklfihhngmmgmpggp/options.html";
                    Thread.Sleep(RdTimes.T_500());
                    driver.Navigate();
                    var userNameBox = driver.FindElement(By.Id("login"));
                    Thread.Sleep(RdTimes.T_500());
                    userNameBox.SendKeys(Acc.ProxyName);
                    Thread.Sleep(RdTimes.T_600());
                    var proxy_password_Box = driver.FindElement(By.Id("password"));
                    Thread.Sleep(RdTimes.T_500());
                    proxy_password_Box.SendKeys(Acc.ProxyPassword);
                    Thread.Sleep(RdTimes.T_400());
                    var retry = driver.FindElement(By.Id("retry"));
                    Thread.Sleep(RdTimes.T_600());
                    retry.Clear();
                    Thread.Sleep(RdTimes.T_500());
                    retry.SendKeys("2");
                    Thread.Sleep(RdTimes.T_600());
                    var saveBTN = driver.FindElement(By.Id("save"));
                    Thread.Sleep(RdTimes.T_700());
                    saveBTN.Click();
                    Thread.Sleep(RdTimes.T_2000());

                    //Check IP
                    driver.Url = "https://myip.link/";
                    driver.Navigate();
                    Thread.Sleep(RdTimes.T_4000());
                    var ip_myip = driver.FindElement(By.XPath("html/body/section/div/div/div/div/div[1]/div[1]/div/div/div[2]/div[1]/div[2]/span")).Text;
                    var ip_myip_webRTC = driver.FindElement(By.XPath("//div[@id='platforms']//div[@class='enabled-status__wrapper']//span[@class='cont']")).Text;
                    Thread.Sleep(RdTimes.T_500());
                    if (ip_myip == Acc.ProxyIP && ip_myip_webRTC == "") //Proxy good
                    {
                        proxy_status_good = true;
                        proxy_note = "Good";
                    }
                    else
                    {
                        proxy_note = "ProxyIP không khớp, hoặc chưa chặn WebRtc!";

                    }
                }
                else
                {
                    proxy_note = "Chưa có Proxy, hoặc Proxy thiếu thông tin!";
                }
                if (Acc.ProxyStatus != proxy_note) { new AccountDao().Proxy_Status_Update(Acc.ID, proxy_note); }               
                model.Driver = driver;
                model.Proxy_Status_Good = proxy_status_good;
                model.Proxy_Status_Note = proxy_note;
                return model;
            }
            catch (Exception) 
            { 
                if (driver != null && Acc.Chrome_OFF == true)
                {
                    driver.Quit();
                }
                model.Driver = driver;
                model.Proxy_Status_Good = false;
                model.Proxy_Status_Note = Acc.ProxyStatus;              
            }
            return model;
        }
    }
}
