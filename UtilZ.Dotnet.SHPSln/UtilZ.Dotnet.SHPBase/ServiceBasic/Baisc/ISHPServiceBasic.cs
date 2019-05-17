using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using UtilZ.Dotnet.SHPBase.ServiceBasic.Model;

namespace UtilZ.Dotnet.SHPBase.ServiceBasic.Baisc
{
    //public delegate TResult FuncEventHandler<TEventArgs, TResult>(object sender, TEventArgs e) where TEventArgs : EventArgs;
    //public delegate TResult FuncEventHandler<TResult>(object sender, EventArgs e);

    /// <summary>
    /// 服务基础接口
    /// </summary>
    public interface ISHPServiceBasic : IDisposable
    {
        ///// <summary>
        ///// 处理Post数据事件
        ///// </summary>
        //event EventHandler<RevDataOutputArgs> ProPostData;

        ///// <summary>
        ///// 处理Request数据事件
        ///// </summary>
        //event FuncEventHandler<RevDataOutputArgs, RevRequestProResult> ProRequestData;

        ///// <summary>
        ///// 上报服务状态事件
        ///// </summary>
        //event FuncEventHandler<ServiceStatusInfo> UploadServiceStatusInfo;

        ///// <summary>
        ///// 初始化
        ///// </summary>
        //void Init();

        /// <summary>
        /// 获取服务实例名称
        /// </summary>
        string ServiceInsName { get; }

        /// <summary>
        /// 请求数据
        /// </summary>
        /// <param name="data">要发送的数据</param>
        /// <param name="transferBasicPolicy">传输基础策略</param>
        /// <returns>请求结果</returns>
        SHPServiceTransferData Request(byte[] data, TransferBasicPolicy transferBasicPolicy);

        /// <summary>
        /// 提交数据
        /// </summary>
        /// <param name="data">要发送的数据</param>
        /// <param name="transferBasicPolicy">传输基础策略</param>
        void Post(byte[] data, TransferBasicPolicy transferBasicPolicy);

        /// <summary>
        /// 提交文件
        /// </summary>
        /// <param name="filePath">本地文件路径</param>
        /// <param name="transferBasicPolicy">传输基础策略</param>
        void PostFile(string filePath, TransferBasicPolicy transferBasicPolicy);
    }

    //public class RevDataOutputArgs : EventArgs
    //{
    //    public SHPServiceTransferModeData Data { get; private set; }

    //    internal RevDataOutputArgs(SHPServiceTransferModeData data)
    //    {
    //        this.Data = data;
    //    }
    //}
}
