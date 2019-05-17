using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilZ.Dotnet.SHPDevOpsModel
{
    internal class ServiceInsPayloadAnalyzeResult
    {
        /// <summary>
        /// 服务实例Id
        /// </summary>
        public long Id { get; private set; }
        public int PayloadValue { get; private set; }
        public int DenyCount { get; private set; }

        public ServiceInsPayloadAnalyzeResult(long id, int payloadValue, int denyCount)
        {
            this.Id = id;
            this.PayloadValue = payloadValue;
            this.DenyCount = denyCount;
        }
    }
}
