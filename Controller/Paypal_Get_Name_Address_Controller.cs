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
    public class Paypal_Get_Name_Address_Controller
    {
        public Session_Result_Model Get_Name_Address (Account Acc, UndectedChromeDriver receive_driver) 
        {
            Session_Result_Model result = new Session_Result_Model();
            UndectedChromeDriver driver = receive_driver;
            result.Status = false;
            try 
            {
                driver.Get("https://www.paypal.com/myaccount/settings/");
                Thread.Sleep(RdTimes.T_1500()); //Time_1500s
                var acc_name = driver.FindElement(By.XPath("//div[@id='accountTab']//div[@class='row profileDetail-container']//p[@class='vx_h3']")).Text;
                Thread.Sleep(RdTimes.T_300());
                var acc_address_all = driver.FindElements(By.XPath("//div[@id='accountTab']//div[@class='address']//div"));
                string acc_address = "";
                Thread.Sleep(RdTimes.T_1000());
                if (acc_address_all != null)
                {
                    for (int aind = 0; aind < acc_address_all.Count; aind++)
                    {
                        if (aind == acc_address_all.Count - 1)
                        {
                            acc_address += acc_address_all[aind].Text;
                        }
                        else
                        {
                            acc_address += acc_address_all[aind].Text + ", ";
                        }
                    }
                }
                Thread.Sleep(RdTimes.T_600()); //Time_600s
                string dTime = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");
                new AccountDao().Update_Name_Address(Acc.ID, acc_name, acc_address, dTime);
                //return
                result.Status = true;
                result.Value_01 = acc_name;
                result.Value_02 = acc_address;
                result.Value_03 = dTime;
            }
            catch { }
            
            result.Driver = driver;
            return result;
        }
    }
}
