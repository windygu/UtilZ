using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.SHPBase.ServiceBasic.Command
{
    public class SHPServiceBasicRevDataResponseCommand : SHPServiceBasicCommandBase
    {
        internal int ContextId { get; private set; }
        public bool Accept { get; private set; }

        /// <summary>
        /// 构造函数-创建
        /// </summary>
        /// <param name="accept"></param>
        /// <param name="contextId"></param>
        public SHPServiceBasicRevDataResponseCommand(bool accept, int contextId)
         : base()
        {
            base._cmd = SHPServiceBasicTransferCommandDefine.ACCEPT_CHECK_RES;
            this.ContextId = contextId;
            this.Accept = accept;
        }

        /// <summary>
        /// 构造函数-解析
        /// </summary>
        public SHPServiceBasicRevDataResponseCommand()
            : base()
        {

        }


        public override byte[] ToBytes()
        {
            int pkLen = 1 + 4 + 1;
            byte[] buffer = new byte[pkLen];

            using (var ms = new MemoryStream(buffer))
            {
                var bw = new BinaryWriter(ms);
                bw.Write(base._cmd);
                bw.Write(this.ContextId);
                bw.Write(this.Accept);
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
                this.ContextId = br.ReadInt32();
                this.Accept = br.ReadBoolean();
            }
        }
    }
}
