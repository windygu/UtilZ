using System;
using System.Collections.Generic;
using System.Text;

namespace UtilZ.ParaService.Model
{
    public class ServicePara
    {
        public long Version { get; set; }

        public List<ServiceParaItem> Items { get; set; } = new List<ServiceParaItem>();

        public ServicePara()
        {

        }
    }

    public class ServiceParaItem
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public ServiceParaItem()
        {

        }
    }
}
