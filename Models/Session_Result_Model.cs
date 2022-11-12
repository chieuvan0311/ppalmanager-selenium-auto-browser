using PAYPAL.ChromeDrivers;

namespace PAYPAL.Models
{
    public class Session_Result_Model
    {
        public UndectedChromeDriver Driver { get; set; }
        public bool Status { get; set; }
        public bool? SetAcc_All_Status { get; set; }
        public string Value_01 { get; set; }
        public string Value_02 { get; set; }
        public string Value_03 { get; set; }
    }
}
