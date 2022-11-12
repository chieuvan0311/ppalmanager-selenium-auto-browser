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

namespace PAYPAL.Controller
{
    public class Paypal_Get_Balance_Note_AccStatus_Controller
    {
        public Session_Result_Model Get (Account Acc, UndectedChromeDriver receive_driver) 
        {
            Session_Result_Model result = new Session_Result_Model();
            UndectedChromeDriver driver = receive_driver;
            result.Status = false;
            try 
            {              
                Thread.Sleep(RdTimes.T_500());
                //Lấy số dư
                var balance = driver.FindElement(By.XPath("//div[@id='cwBalance']//div[@data-test-id='available-balance']")).Text;
                var notiBTN = driver.FindElement(By.Id("header-notifications"));
                Thread.Sleep(RdTimes.T_1500());
                notiBTN.Click();
                Thread.Sleep(RdTimes.T_1000());
                IJavaScriptExecutor js_driver = driver as IJavaScriptExecutor;
                //Lấy thông báo
                var getNote = (string)js_driver.ExecuteScript("var content = document.getElementById('notificationCount').innerHTML;return content;");
                Thread.Sleep(RdTimes.T_1000());
                string note = "(" + getNote + ")" + " Thông báo";
                Thread.Sleep(RdTimes.T_600());
                //Check tình trạng acc
                driver.Get("https://www.paypal.com/restore/dashboard");
                Thread.Sleep(RdTimes.T_7000());
                IWebElement divLimit = null;
                IWebElement div180d = null;
                try { divLimit = (IWebElement)driver.FindElement(By.Id("limitationDetailsContainer")); } catch (Exception) { }
                try { div180d = (IWebElement)driver.FindElement(By.XPath("//div[@id='main']//div[@class='noappealHeading']")); } catch (Exception) { }
                string AccStatus = Acc.AccType;
                if (divLimit != null) { AccStatus = "Limit"; }
                if (div180d != null) { AccStatus = "180d"; }
                if (divLimit == null && div180d == null) { AccStatus = "Hoạt động"; }
                Thread.Sleep(RdTimes.T_1000());
                string dTime = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");
                new AccountDao().Update_Balance_Notifications_Status_DateTime(Acc.ID, balance, note, AccStatus, dTime);
                result.Status = true;
            }
            catch { }

            result.Driver = driver;
            return result;
        }
    }
}
