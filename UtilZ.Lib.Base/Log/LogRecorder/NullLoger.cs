using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Lib.Base.NLog.Config.Framework;
using UtilZ.Lib.Base.NLog.Config.Interface;
using UtilZ.Lib.Base.NLog.LogRecorderInterface;
using UtilZ.Lib.Base.NLog.Model;

namespace UtilZ.Lib.Base.NLog.LogRecorder
{
    /// <summary>
    /// 空日志记录器
    /// </summary>
    public class NullLoger : BaseLogRecorder
    {
        private NullLogIConfigElement _config;
        /// <summary>
        /// 基础配置
        /// </summary>
        public override IConfig BaseConfig
        {
            get { return this._config; }
            set
            {
                if (value is NullLogIConfigElement)
                {
                    this._config = (NullLogIConfigElement)value;
                }
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public NullLoger()
        {
            var config = new NullLogIConfigElement();
            config.Name = LogConstant.DefaultNullLogRecorderName;
            this._config = config;
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="item">日志项</param>
        public override void WriteLog(Model.LogItem item)
        {
            if (item == null)
            {
                return;
            }

            this.WriteLog(new List<LogItem>() { item });
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="items">日志项集合</param>
        public override void WriteLog(List<Model.LogItem> items)
        {
            if (items == null || items.Count == 0)
            {
                return;
            }

            var config = this._config;
            if (config == null || !config.Enable)
            {
                return;
            }

            try
            {
                var groups = items.GroupBy((item) => { return item.Level; });
                foreach (var group in groups)
                {
                    //过滤条件验证
                    if (!base.FilterValidate(group.Key))
                    {
                        continue;
                    }

                    foreach (var item in group)
                    {
                        this.OutputLog(config.Name, item);
                    }
                }
            }
            catch (Exception ex)
            {
                LogSysInnerLog.OnRaiseLog(this, ex);
            }
            finally
            {
                //追加日志
                base.AppenderLog(items);
            }
        }
    }
}
