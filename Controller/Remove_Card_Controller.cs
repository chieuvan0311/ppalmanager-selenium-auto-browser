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
    public class Remove_Card_Controller
    {
        public Session_Result_Model Remove (Account Acc, UndectedChromeDriver receive_driver)
        {
            Session_Result_Model result = new Session_Result_Model();
            UndectedChromeDriver driver = receive_driver;
            result.Status = false;
            try
            {
                driver.Get("https://www.paypal.com/myaccount/money/");
                Thread.Sleep(RdTimes.T_4000());
                IWebElement chec_card_element = null;
                try
                {
                    chec_card_element = driver.FindElement(By.XPath("//section[@id='contents']//section[@class='fiListGroup_testTreatment nemo_fiListGroup']//li[@class='fiList-item_testTreatment ']//a"));
                }
                catch { }

                Thread.Sleep(RdTimes.T_500());
                if (chec_card_element != null)
                {
                    int chec_card_element_count = driver.FindElements(By.XPath("//section[@id='contents']//section[@class='fiListGroup_testTreatment nemo_fiListGroup']//li[@class='fiList-item_testTreatment ']//a")).Count();
                    Thread.Sleep(RdTimes.T_1000());

                    for (int i = 0; i < chec_card_element_count; i++)
                    {
                        var de_element = driver.FindElement(By.XPath("//section[@id='contents']//section[@class='fiListGroup_testTreatment nemo_fiListGroup']//li[@class='fiList-item_testTreatment ']//a"));
                        Thread.Sleep(RdTimes.T_800());
                        de_element.Click();
                        Thread.Sleep(RdTimes.T_2000());
                        var remove_btn = driver.FindElement(By.XPath("//div[@class='cardDetails fiDetails']//div[@class='fiDetails-actionLinks updateRemoveCard-links fiDetailsSection']//a[@data-name='removeCard']"));
                        Thread.Sleep(RdTimes.T_1000());
                        remove_btn.Click();
                        Thread.Sleep(RdTimes.T_2000());
                        var remove_confirm = driver.FindElement(By.XPath("//div[@class='vx_modal-content']//div[@class='removeFi-container']//button[@name='removeCard']"));
                        Thread.Sleep(RdTimes.T_1000());
                        remove_confirm.Click();
                        Thread.Sleep(RdTimes.T_2000());
                        driver.Get("https://www.paypal.com/myaccount/money/");
                        Thread.Sleep(RdTimes.T_3000());
                    }

                    IWebElement chec_card_element_again = null;
                    try
                    {
                        chec_card_element_again = driver.FindElement(By.XPath("//section[@id='contents']//section[@class='fiListGroup_testTreatment nemo_fiListGroup']//li[@class='fiList-item_testTreatment ']//a"));
                    }
                    catch { }
                    if (chec_card_element_again == null)
                    {
                        result.Status = true;
                        result.Value_01 = "Đã xóa " + chec_card_element_count.ToString() + " card--";
                    }
                    else
                    {
                        result.Value_01 = "Không thể xóa card--";
                    }
                }
                else
                {
                    result.Status = true;
                    result.Value_01 = "Không có card nào--";
                }
            }
            catch { }
            
            result.Driver = driver;
            return result;
        }
    }
}
