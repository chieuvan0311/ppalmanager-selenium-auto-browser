using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAYPAL.Models
{
    public class Proxy_Model
    {
        public int ID { get; set; }
        public string ProxyIP { get; set; }
        public string ProxyPort { get; set; }
        public string ProxyName { get; set; }
        public string ProxyPassword { get; set; }
    }
}
