using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.SHPBase.ServiceBasic.Model;

namespace UtilZ.Dotnet.SHPBase.ServiceBasic.Command
{
    public class SHPServiceBasicResponseDataCommand : SHPServiceBasicCommandBase
    {
        public int DataCode { get; private set; }
        internal int ContextId { get; private set; }
        public byte[] Data { get; private set; }

        internal SHPServiceBasicResponseDataCommand(SHPServiceResponseData result, SHPServiceBasicRequestDataCommand serviceBasicRequestDataCommand)
             : base()
        {
            base._cmd = SHPServiceBasicTransferCommandDefine.RESPONSE;
            this.DataCode = result.DataCode;
            this.ContextId = serviceBasicRequestDataCommand.ContextId;
            this.Data = result.Data;
        }

        /// <summary>
        /// 构造函数-解析
        /// </summary>
        /// <param name="cmd"></param>
        public SHPServiceBasicResponseDataCommand()
            : base()
        {

        }


        public override byte[] ToBytes()
        {
            var data = this.Data;
            int pkLen = 1 + 4 + 4 + 4 + data.Length;
            byte[] buffer = new byte[pkLen];

            using (var ms = new MemoryStream(buffer))
            {
                var bw = new BinaryWriter(ms);
                bw.Write(base._cmd);
                bw.Write(this.DataCode);
                bw.Write(this.ContextId);
                bw.Write(data.Length);
                bw.Write(data, 0, data.Length);
                bw.Flush();
            }

            return buffer;
        }

        public override void Parse(byte[] buffer)
        {
            using (var ms = new MemoryStream(buffer))
            {
                var br = new BinaryReader(ms);
                base._cmd = br.ReadByte();
                this.DataCode = br.ReadInt32();
                this.ContextId = br.ReadInt32();
                var dataLen = br.ReadInt32();
                var data = new byte[dataLen];
                br.Read(data, 0, data.Length);
                this.Data = data;
            }
        }
    }
}
