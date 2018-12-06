using System;
using System.Collections.Generic;
using System.Text;

namespace UtilZ.ParaService.Model
{
    public class DBException : Exception
    {
        public int Status { get; set; }
        public DBException(int status, string message) : base(message)
        {
            this.Status = status;
        }

        public DBException(int status, string message, Exception innerEx) : base(message, innerEx)
        {
            this.Status = status;
        }
    }
}
