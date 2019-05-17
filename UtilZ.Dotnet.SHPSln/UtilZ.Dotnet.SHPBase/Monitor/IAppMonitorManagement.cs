using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Base;

namespace UtilZ.Dotnet.SHPBase.Monitor
{
    /// <summary>
    /// 应用监视管理接口
    /// </summary>
    public interface IAppMonitorManagement
    {
        /// <summary>
        /// 监视应用列表
        /// </summary>
        BindingCollection<AppMonitorItem> AppMonitorList { get; }

        /// <summary>
        /// 监视应用列表线程锁
        /// </summary>
        object AppMonitorListLock { get; }

        /// <summary>
        /// 添加监视项
        /// </summary>
        /// <param name="monitorItem">监视项</param>
        void AddMonitorItem(AppMonitorItem appMonitorItem);

        /// <summary>
        /// 修改监视项
        /// </summary>
        /// <param name="oldAppMonitorItem">旧监视项</param>
        /// <param name="newAppMonitorItem">新监视项</param>
        void ModifyMonitorItem(AppMonitorItem oldAppMonitorItem, AppMonitorItem newAppMonitorItem);

        /// <summary>
        /// 移除监视项
        /// </summary>
        /// <param name="monitorItem">监视项</param>
        void RemoveMonitorItem(AppMonitorItem appMonitorItem);

        /// <summary>
        /// 是否包含监视项[包含返回true;否则返回false]
        /// </summary>
        /// <param name="appMonitorItem">目标监视项</param>
        /// <returns>包含返回true;否则返回false</returns>
        bool ContainsMonitorItem(IAppMonitor appMonitorItem);

        /// <summary>
        /// 启动监视项
        /// </summary>
        /// <param name="monitorItem">监视项</param>
        void StartMonitorItem(AppMonitorItem monitorItem);

        /// <summary>
        /// 停止监视项
        /// </summary>
        /// <param name="monitorItem">监视项</param>
        void StopMonitorItem(AppMonitorItem monitorItem);

        /// <summary>
        /// 重启监视项
        /// </summary>
        /// <param name="monitorItem">监视项</param>
        void RestartMonitorItem(AppMonitorItem monitorItem);
    }
}
