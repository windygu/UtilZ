using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Transfer.Net;

namespace UtilZ.Dotnet.SHPBase.ServiceBasic.Command
{
    public class SHPServiceBasicRevDataRequestCommand : SHPServiceBasicCommandBase
    {
        public int DataCode { get; private set; }
        public int ContextId { get; private set; }
        public long Size { get; private set; }
        public TransferProtocal TransferProtocal { get; private set; }

        /// <summary>
        /// 构造函数-创建
        /// </summary>
        /// <param name="dataCode"></param>
        /// <param name="size"></param>
        /// <param name="contextId"></param>
        public SHPServiceBasicRevDataRequestCommand(int dataCode, long size, TransferProtocal transferProtocal, int contextId)
         : base()
        {
            base._cmd = SHPServiceBasicTransferCommandDefine.ACCEPT_CHECK_REQ;
            this.DataCode = dataCode;
            this.Size = size;
            this.TransferProtocal = transferProtocal;
            this.ContextId = contextId;
        }


        /// <summary>
        /// 构造函数-解析
        /// </summary>
        public SHPServiceBasicRevDataRequestCommand()
            : base()
        {

        }


        public override byte[] ToBytes()
        {
            int pkLen = 1 + 4 + 4 + 8 + 1;
            byte[] buffer = new byte[pkLen];

            using (var ms = new MemoryStream(buffer))
            {
                var bw = new BinaryWriter(ms);
                bw.Write(base._cmd);
                bw.Write(this.DataCode);
                bw.Write(this.ContextId);
                bw.Write(this.Size);
                bw.Write((byte)this.TransferProtocal);
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
                this.Size = br.ReadInt64();
                this.TransferProtocal = (TransferProtocal)br.ReadByte();
            }
        }
    }
}
