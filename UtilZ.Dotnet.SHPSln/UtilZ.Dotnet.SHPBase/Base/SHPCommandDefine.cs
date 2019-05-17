using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.SHPBase.Base
{
    /// <summary>
    /// 平台命令定义类
    /// </summary>
    public class SHPCommandDefine
    {
        private static readonly Dictionary<int, SHPCommandAttribute> _cmdSHPCommandAttributeDic = new Dictionary<int, SHPCommandAttribute>();
        public static void Init<T>() where T : SHPCommandDefine
        {
            var fields = typeof(T).GetFields();
            var shpCommandAttributeType = typeof(SHPCommandAttribute);
            Int32 cmd;
            SHPCommandAttribute nameAttri;
            foreach (var field in fields)
            {
                cmd = (Int32)field.GetValue(null);
                nameAttri = ((SHPCommandAttribute)field.GetCustomAttributes(shpCommandAttributeType, true)[0]);
                if (_cmdSHPCommandAttributeDic.ContainsKey(cmd))
                {
                    throw new ArgumentException($"重复的命令[{cmd }]");
                }

                _cmdSHPCommandAttributeDic.Add(cmd, nameAttri);
            }
        }

        public static string GetCommandNameByCommand(Int32 cmd)
        {
            if (_cmdSHPCommandAttributeDic.ContainsKey(cmd))
            {
                return _cmdSHPCommandAttributeDic[cmd].Name;
            }

            throw new ArgumentException($"不存在的命令[{cmd}]");
        }

        /************************************************
         * 命令字定义范围(0-10000):
         * 0-100:主机相关
         * 101-200:监视应用相关
         * 201-250:脚本相关
         * 251-500:内部命令
         * 501-550:升级相关(Agent+监视应用)
         ************************************************/

        #region 主机相关 1-100
        /// <summary>
        /// 添加主机
        /// </summary>
        [SHPCommandAttribute("添加主机")]
        public const Int32 ADD_HOST_REQ = 1;

        /// <summary>
        /// 添加主机响应
        /// </summary>
        [SHPCommandAttribute("添加主机响应")]
        public const Int32 ADD_HOST_RES = 2;

        /// <summary>
        /// 删除主机
        /// </summary>
        [SHPCommandAttribute("删除主机")]
        public const Int32 DELETE_HOST = 3;

        /// <summary>
        /// 环境改变通知
        /// </summary>
        [SHPCommandAttribute("环境改变通知")]
        public const Int32 EVN_CHANGED_NOTIFY = 4;

        /// <summary>
        /// 环境改变通知响应
        /// </summary>
        [SHPCommandAttribute("环境改变通知响应")]
        public const Int32 EVN_CHANGED_NOTIFY_RES = 5;

        /// <summary>
        /// 上报主机状态信息
        /// </summary>
        [SHPCommandAttribute("上报主机状态")]
        public const Int32 UP_HOST_STATUS = 6;

        /// <summary>
        /// 运控迁移通知
        /// </summary>
        [SHPCommandAttribute("运控迁移通知")]
        public const Int32 DevOpsMigrateNotify = 7;

        /// <summary>
        /// 运控迁移响应
        /// </summary>
        [SHPCommandAttribute("运控迁移响应")]
        public const Int32 DevOpsMigrateNotifyRes = 8;

        /// <summary>
        /// 关闭主机
        /// </summary>
        [SHPCommandAttribute("关闭主机")]
        public const Int32 SHUTDOWN = 9;

        /// <summary>
        /// 重启主机
        /// </summary>
        [SHPCommandAttribute("重启主机")]
        public const Int32 RESTART = 10;

        /// <summary>
        /// 注销主机
        /// </summary>
        [SHPCommandAttribute("注销主机")]
        public const Int32 LOGOUT = 11;

        /// <summary>
        /// 结束进程
        /// </summary>
        [SHPCommandAttribute("结束进程")]
        public const Int32 KILL_PROCESS = 12;

        /// <summary>
        /// 结束进程树
        /// </summary>
        [SHPCommandAttribute("结束进程树")]
        public const Int32 KILL_PROCESS_TREE = 13;

        /// <summary>
        /// 结束进程响应
        /// </summary>
        [SHPCommandAttribute("结束进程响应")]
        public const Int32 KILL_PROCESS_RES = 14;

        ///// <summary>
        ///// 设置主机时间
        ///// </summary>
        //  [SHPCommandAttribute("设置主机时间")]
        //public const Int32 SET_TIME = 13;
        #endregion

        #region 应用相关 101-200
        /// <summary>
        /// 控制程序
        /// </summary>
        [SHPCommandAttribute("控制程序")]
        public const Int32 CONTROL_APP = 101;

        /// <summary>
        /// 控制程序响应
        /// </summary>
        [SHPCommandAttribute("控制程序响应")]
        public const Int32 CONTROL_APP_RES = 102;

        /// <summary>
        /// 进程添加到监视
        /// </summary>
        [SHPCommandAttribute("进程添加到监视")]
        public const Int32 PROCESS_ADD_TO_MONITOR = 103;

        /// <summary>
        /// 进程添加到监视响应
        /// </summary>
        [SHPCommandAttribute("进程添加到监视响应")]
        public const Int32 PROCESS_ADD_TO_MONITOR_RES = 104;

        /// <summary>
        /// 移除监视应用
        /// </summary>
        [SHPCommandAttribute("移除监视应用")]
        public const Int32 REMOVE_MONITOR_APP = 105;

        /// <summary>
        /// 移除监视应用响应
        /// </summary>
        [SHPCommandAttribute("移除监视应用响应")]
        public const Int32 REMOVE_MONITOR_APP_RES = 106;
        #endregion

        #region 脚本相关 201-250
        /// <summary>
        /// 执行脚本
        /// </summary>
        [SHPCommandAttribute("执行脚本请求")]
        public const Int32 EXCUTE_SCRPT_REQ = 201;

        /// <summary>
        /// 执行脚本响应
        /// </summary>
        [SHPCommandAttribute("执行脚本响应")]
        public const Int32 EXCUTE_SCRPT_RES = 202;
        #endregion

        #region 路由命令 251-300
        /// <summary>
        /// 服务路由添加请求
        /// </summary>
        [SHPCommandAttribute("服务部署请求")]
        public const Int32 SERVICE_DEPLOY_REQ = 251;

        /// <summary>
        /// 服务路由添加响应
        /// </summary>
        [SHPCommandAttribute("服务部署响应")]
        public const Int32 SERVICE_DEPLOY_RES = 252;

        /// <summary>
        /// 服务实例删除请求
        /// </summary>
        [SHPCommandAttribute("服务实例删除请求")]
        public const Int32 SERVICE_INS_DELETE_REQ = 253;

        /// <summary>
        /// 服务实例删除响应
        /// </summary>
        [SHPCommandAttribute("服务实例删除响应")]
        public const Int32 SERVICE_INS_DELETE_RES = 254;

        /// <summary>
        /// 服务实例监听信息变更
        /// </summary>
        [SHPCommandAttribute("服务实例监听信息变更")]
        public const Int32 SERVICE_LISTEN_CHANGED_NOTIFY = 255;
        #endregion

        #region 内部命令 301-500



        #endregion

        #region 升级相关
        /// <summary>
        /// 查询Agent版本
        /// </summary>
        [SHPCommandAttribute("查询Agent版本")]
        public const Int32 QUERY_AGENT = 501;

        /// <summary>
        /// 升级Agent
        /// </summary>
        [SHPCommandAttribute("升级Agent")]
        public const Int32 UPGRADE_AGENT = 502;

        /// <summary>
        /// 升级应用
        /// </summary>
        [SHPCommandAttribute("升级应用")]
        public const Int32 UPGRADE_APP = 503;
        #endregion
    }
}
