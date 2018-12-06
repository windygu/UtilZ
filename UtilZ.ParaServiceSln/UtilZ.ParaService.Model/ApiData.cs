using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace UtilZ.ParaService.Model
{
    public class ApiData
    {
        public int Status { get; set; }

        public object Value { get; set; }

        public string Reason { get; set; }

        protected ApiData(int status)
        {
            this.Status = status;
        }

        public ApiData(int status, object value) : this(status)
        {
            this.Value = value;
        }

        public ApiData(int status, string reason) : this(status)
        {
            this.Reason = reason;
        }
    }
}
