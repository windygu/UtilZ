using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using UtilZ.Dotnet.Ex.Log;

namespace UtilZ.Dotnet.DBModel.Config
{
    /// <summary>
    /// 配置管理器
    /// </summary>
    public class ConfigManager
    {
        /// <summary>
        /// 配置Section节点名称
        /// </summary>
        public const string DBConfigName = @"DBConfig";

        /// <summary>
        /// 配置项字典集合[key:DBID;value:配置项]
        /// </summary>
        private static readonly ConcurrentDictionary<int, DBConfigElement> _configItems = new ConcurrentDictionary<int, DBConfigElement>();

        /// <summary>
        /// 加载默认配置App.config
        /// </summary>
        //public static void Init()
        static ConfigManager()
        {
            try
            {
                //加载默认配置
                System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if (config == null)
                {
                    return;
                }

                //从配置中获取日志配置参数节点
                DBConfigSection configSection = config.GetSection(DBConfigName) as DBConfigSection;

                //加载配置
                LoadConfig(configSection);
            }
            catch (Exception ex)
            {
                Loger.Error(null, ex);
            }
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <param name="configSection">配置节</param>
        public static void LoadConfig(DBConfigSection configSection)
        {
            if (configSection == null || configSection.DBItems == null || configSection.DBItems.Count == 0)
            {
                return;
            }

            foreach (DBConfigElement dbItem in configSection.DBItems)
            {
                if (dbItem == null)
                {
                    continue;
                }

                _configItems[dbItem.DBID] = dbItem;
            }
        }

        /// <summary>
        /// 移除配置项
        /// </summary>
        /// <param name="dbid">要移除的配置项的数据库编号</param>
        public static void RemoveConfigItem(int dbid)
        {
            DBConfigElement dbItem;
            _configItems.TryRemove(dbid, out dbItem);
        }

        /// <summary>
        /// 移除配置项
        /// </summary>
        /// <param name="dbItem">要移除的配置项</param>
        public static void RemoveConfigItem(DBConfigElement dbItem)
        {
            if (dbItem == null)
            {
                return;
            }

            RemoveConfigItem(dbItem.DBID);
        }

        /// <summary>
        /// 添加配置项
        /// </summary>
        /// <param name="dbItem">要添加的配置项</param>
        public static void AddConfigItem(DBConfigElement dbItem)
        {
            if (dbItem == null)
            {
                return;
            }

            _configItems.TryAdd(dbItem.DBID, dbItem);
        }

        /// <summary>
        /// 是否包含指定数据库编号的配置项
        /// </summary>
        /// <param name="dbid">指定数据库编号</param>
        public static bool ContainsDBID(int dbid)
        {
            return _configItems.ContainsKey(dbid);
        }

        /// <summary>
        /// 获取配置项
        /// </summary>
        /// <param name="dbid">数据库编号</param>
        /// <returns>配置项</returns>
        public static DBConfigElement GetConfigItem(int dbid)
        {
            DBConfigElement dbItem;
            if (!_configItems.TryGetValue(dbid, out dbItem))
            {
                throw new ApplicationException(string.Format("配置中不包含数据库编号为{0}的配置", dbid));
            }
            else
            {
                if (dbItem == null)
                {
                    throw new ApplicationException(string.Format("获取数据库编号为{0}的配置项失败", dbid));
                }

                return dbItem;
            }
        }

        /// <summary>
        /// 获取全部配置项列表
        /// </summary>
        /// <returns>配置项</returns>
        public static List<DBConfigElement> GetAllConfigItems()
        {
            return _configItems.Values.ToList();
        }
    }
}
