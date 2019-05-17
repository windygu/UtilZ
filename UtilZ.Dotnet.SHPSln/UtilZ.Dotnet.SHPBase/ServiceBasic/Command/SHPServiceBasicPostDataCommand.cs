using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.SHPBase.ServiceBasic.Command
{
    public class SHPServiceBasicPostDataCommand : SHPServiceBasicCommandBase
    {
        public int DataCode { get; private set; }
        public byte[] Data { get; private set; }

        /// <summary>
        /// 构造函数-创建
        /// </summary>
        /// <param name="dataCode"></param>
        /// <param name="data"></param>
        internal SHPServiceBasicPostDataCommand(int dataCode, byte[] data)
               : base()
        {
            base._cmd = SHPServiceBasicTransferCommandDefine.POST;
            this.DataCode = dataCode;
            this.Data = data;
        }


        /// <summary>
        /// 构造函数-解析
        /// </summary>
        /// <param name="cmd"></param>
        public SHPServiceBasicPostDataCommand()
            : base()
        {

        }


        public override byte[] ToBytes()
        {
            var data = this.Data;
            int pkLen = 1 + 4 + 4 + data.Length;
            byte[] buffer = new byte[pkLen];

            using (var ms = new MemoryStream(buffer))
            {
                var bw = new BinaryWriter(ms);
                bw.Write(base._cmd);
                bw.Write(this.DataCode);
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
                var dataLen = br.ReadInt32();
                var data = new byte[dataLen];
                br.Read(data, 0, data.Length);
                this.Data = data;
            }
        }
    }
}
