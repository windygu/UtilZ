using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Transfer.Net;

namespace UtilZ.Dotnet.SHPBase.ServiceBasic.Command
{
    public class SHPServiceBasicRequestDataCommand : SHPServiceBasicCommandBase
    {
        public int DataCode { get; private set; }
        internal int ContextId { get; private set; }
        public byte[] Data { get; private set; }
        public TransferProtocal TransferProtocal { get; private set; }

        /// <summary>
        /// 构造函数-创建
        /// </summary>
        /// <param name="dataCode"></param>
        /// <param name="contextId"></param>
        /// <param name="data"></param>
        internal SHPServiceBasicRequestDataCommand(int dataCode, int contextId, byte[] data, TransferProtocal transferProtocal)
              : base()
        {
            base._cmd = SHPServiceBasicTransferCommandDefine.REQUEST;
            this.DataCode = dataCode;
            this.ContextId = contextId;
            this.Data = data;
            this.TransferProtocal = transferProtocal;
        }

        /// <summary>
        /// 构造函数-解析
        /// </summary>
        /// <param name="cmd"></param>
        public SHPServiceBasicRequestDataCommand()
            : base()
        {

        }


        public override byte[] ToBytes()
        {
            var data = this.Data;
            int pkLen = 1 + 4 + 4 + 1 + 4 + data.Length;
            byte[] buffer = new byte[pkLen];

            using (var ms = new MemoryStream(buffer))
            {
                var bw = new BinaryWriter(ms);
                bw.Write(base._cmd);
                bw.Write(this.DataCode);
                bw.Write(this.ContextId);
                bw.Write((byte)this.TransferProtocal);
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
                this.TransferProtocal = (TransferProtocal)br.ReadByte();
                var dataLen = br.ReadInt32();
                var data = new byte[dataLen];
                br.Read(data, 0, data.Length);
                this.Data = data;
            }
        }
    }
}
