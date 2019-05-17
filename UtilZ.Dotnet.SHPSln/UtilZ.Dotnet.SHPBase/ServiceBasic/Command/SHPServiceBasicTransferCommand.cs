using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.SHPBase.ServiceBasic.Command
{
    public class SHPServiceBasicTransferCommand
    {
        public byte Cmd { get; private set; }

        public byte[] Data { get; private set; }

        /// <summary>
        /// 构造函数-创建
        /// </summary>
        /// <param name="command"></param>
        public SHPServiceBasicTransferCommand(ISHPServiceBasicCommand command)
        {
            this.Cmd = command.Cmd;
            this.Data = command.ToBytes();
        }

        /// <summary>
        /// 构造函数-解析
        /// </summary>
        /// <param name="buffer"></param>
        public SHPServiceBasicTransferCommand(byte[] buffer)
        {
            using (var ms = new MemoryStream(buffer))
            {
                var br = new BinaryReader(ms);
                this.Cmd = br.ReadByte();
                var dataLen = br.ReadInt32();
                var data = new byte[dataLen];
                br.Read(data, 0, data.Length);
                this.Data = data;
            }
        }

        public byte[] ToBytes()
        {
            var data = this.Data;
            int pkLen = 1 + 4 + data.Length;
            byte[] buffer = new byte[pkLen];

            using (var ms = new MemoryStream(buffer))
            {
                var bw = new BinaryWriter(ms);
                bw.Write(this.Cmd);
                bw.Write(data.Length);
                bw.Write(data, 0, data.Length);
                bw.Flush();
            }

            return buffer;
        }
    }
}
