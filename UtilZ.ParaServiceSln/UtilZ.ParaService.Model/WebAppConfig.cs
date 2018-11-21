using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UtilZ.ParaService.Model
{
    public class WebAppConfig
    {
        private static WebAppConfig _instance = new WebAppConfig();

        public static WebAppConfig Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// 单位毫秒
        /// </summary>
        public int TokenExpireTime { get; set; } = 10 * 60 * 1000;

        public int DBID { get; set; } = 5;
    }
}
