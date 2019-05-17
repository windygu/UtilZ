using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using UtilZ.Dotnet.Ex.Base;

namespace UtilZ.Dotnet.SHPBase.Base
{
    /// <summary>
    /// 传输命令
    /// </summary>
    public class SHPTransferCommand
    {
        /// <summary>
        /// 命令上下文标识
        /// </summary>
        public Int32 ContextId { get; private set; }

        /// <summary>
        /// 执行命令超时时长
        /// </summary>
        public Int32 Timeout { get; private set; }

        /// <summary>
        /// 插件ID
        /// </summary>
        public Int32 PluginId { get; private set; }

        /// <summary>
        /// 命令字
        /// </summary>
        public Int32 Cmd { get; private set; }

        /// <summary>
        /// 命令数据
        /// </summary>
        public byte[] Data { get; private set; }

        /// <summary>
        /// 命令来源
        /// </summary>
        public IPEndPoint SrcEndPoint { get; private set; }

        /// <summary>
        /// 构造函数-创建
        /// </summary>
        /// <param name="transferCommand">传输命令</param>
        /// <param name="cmd">命令</param>
        public SHPTransferCommand(SHPTransferCommand transferCommand, ICommand cmd)
            : this(transferCommand.ContextId, transferCommand.PluginId, cmd.Cmd, cmd.GenerateCommandData(), transferCommand.Timeout)
        {

        }

        /// <summary>
        /// 构造函数-创建(同步执行响应)
        /// </summary>
        /// <param name="transferCommand">传输命令</param>
        /// <param name="resCmd">响应命令字</param>
        /// <param name="result">命令执行结果</param>
        /// <param name="data">结果数据</param>
        public SHPTransferCommand(SHPTransferCommand transferCommand, int resCmd, SHPCommandExcuteResult result, byte[] data)
            : this(transferCommand.ContextId, transferCommand.PluginId, resCmd, new SHPResult(result, data).ToBytes(), transferCommand.Timeout)
        {

        }

        /// <summary>
        /// 构造函数-创建
        /// </summary>
        /// <param name="contextId">命令上下文标识</param>
        /// <param name="pluginId">插件Id</param>
        /// <param name="cmd">命令</param>
        /// <param name="timeout">执行命令超时时长</param>
        public SHPTransferCommand(Int32 contextId, Int32 pluginId, ICommand cmd, Int32 timeout = System.Threading.Timeout.Infinite)
            : this(contextId, pluginId, cmd.Cmd, cmd.GenerateCommandData(), timeout)
        {

        }

        /// <summary>
        /// 构造函数-创建
        /// </summary>
        /// <param name="contextId">命令上下文标识</param>
        /// <param name="pluginId">插件Id</param>
        /// <param name="cmd">命令</param>
        /// <param name="timeout">执行命令超时时长</param>
        public SHPTransferCommand(Int32 contextId, Int32 pluginId, int cmd, byte[] data, Int32 timeout = System.Threading.Timeout.Infinite)
        {
            if (contextId == 0)
            {
                throw new ArgumentException("命令上下文标识不能为默认值");
            }

            if (timeout < System.Threading.Timeout.Infinite)
            {
                throw new ArgumentOutOfRangeException(nameof(timeout));
            }

            this.ContextId = contextId;
            this.PluginId = pluginId;
            this.Cmd = cmd;
            this.Data = data;
            this.Timeout = timeout;
        }

        /// <summary>
        /// 构造函数-序列化-解析
        /// </summary>
        public SHPTransferCommand()
        {

        }

        private const int _HEADER_SIZE = 20;
        /// <summary>
        /// 构造函数-创建
        /// </summary>
        /// <param name="br">解析</param>
        public void Parse(BinaryReader br, IPEndPoint srcEndPoint)
        {
            this.SrcEndPoint = srcEndPoint;
            if (br.BaseStream.Position != 0)
            {
                br.BaseStream.Seek(0, SeekOrigin.Begin);
            }

            this.ContextId = br.ReadInt32();
            this.Timeout = br.ReadInt32();
            this.PluginId = br.ReadInt32();
            this.Cmd = br.ReadInt32();
            Int32 dataLen = br.ReadInt32();
            if (dataLen > 0)
            {
                var data = new byte[dataLen];
                br.Read(data, 0, data.Length);
                this.Data = data;
            }
        }

        public byte[] GenerateBuffer()
        {
            Int32 dataLen = 0;
            var data = this.Data;
            bool hasData = data != null;
            if (hasData)
            {
                dataLen = data.Length;
            }

            var pkLen = _HEADER_SIZE + dataLen;
            var buffer = new byte[pkLen];
            using (var ms = new MemoryStream(buffer))
            {
                var bw = new BinaryWriter(ms);
                bw.Write(this.ContextId);
                bw.Write(this.Timeout);
                bw.Write(this.PluginId);
                bw.Write(this.Cmd);
                bw.Write(dataLen);
                if (hasData)
                {
                    bw.Write(data, 0, data.Length);
                }
            }

            return buffer;
        }

        public static int CreateContextId()
        {
            return GUIDEx.GetGUIDHashCode();
        }

        public static readonly int DefaultContextId = -1;
    }
}
