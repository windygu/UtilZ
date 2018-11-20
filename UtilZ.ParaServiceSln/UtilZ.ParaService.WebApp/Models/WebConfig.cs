using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UtilZ.ParaService.WebApp.Models
{
    public class WebConfig
    {
        private static WebConfig _instance = new WebConfig();

        public static WebConfig Instance
        {
            get { return _instance; }
        }

        public int TokenExpireTime { get; set; } = 10 * 60 * 1000;
    }
}
