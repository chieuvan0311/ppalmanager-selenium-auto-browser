using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAYPAL.Models
{
    public class Change_Paypal_Password_Model
    {
        public ChromeDriver Driver { get; set; }
        public string New_Paypal_Password { get; set; }
    }
}
