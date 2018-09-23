using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using UtilZ.Dotnet.DBBase.Common;

namespace UtilZ.Dotnet.DBBase.Model
{
    /// <summary>
    /// 配置管理器
    /// </summary>
    public class ConfigManager
    {
        /// <summary>
        /// 配置项字典集合[key:DBID;value:配置项]
        /// </summary>
        private static readonly ConcurrentDictionary<int, DBConfig> _configDic = new ConcurrentDictionary<int, DBConfig>();

        /// <summary>
        /// 加载默认配置App.config
        /// </summary>
        static ConfigManager()
        {
            try
            {
                //加载默认配置
                LoadConfig("dbConfig.xml");
            }
            catch (Exception ex)
            {
                DBLog.OutLog(ex);
            }
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <param name="configSection">配置节</param>
        public static void LoadConfig(string dbConfigFilePath)
        {
            if (string.IsNullOrWhiteSpace(dbConfigFilePath) || !File.Exists(dbConfigFilePath))
            {
                return;
            }

            var xdoc = XDocument.Load(dbConfigFilePath);
            IEnumerable<XElement> itemEles = xdoc.XPathSelectElements(@"DBConfig/DBItems/Item");
            foreach (var itemEle in itemEles)
            {
                try
                {
                    var dbItem = new DBConfig(itemEle);
                    _configDic[dbItem.DBID] = dbItem;
                }
                catch (Exception ex)
                {
                    DBLog.OutLog(ex);
                }
            }
        }

        /// <summary>
        /// 移除配置项
        /// </summary>
        /// <param name="dbid">要移除的配置项的数据库编号</param>
        public static void RemoveConfigByDBID(int dbid)
        {
            DBConfig dbItem;
            _configDic.TryRemove(dbid, out dbItem);
        }

        /// <summary>
        /// 移除配置项
        /// </summary>
        /// <param name="config">要移除的配置项</param>
        public static void RemoveConfig(DBConfig config)
        {
            if (config == null)
            {
                return;
            }

            RemoveConfigByDBID(config.DBID);
        }

        /// <summary>
        /// 添加配置项
        /// </summary>
        /// <param name="config">要添加的配置项</param>
        public static void AddConfig(DBConfig config)
        {
            if (config == null)
            {
                return;
            }

            _configDic.TryAdd(config.DBID, config);
        }

        /// <summary>
        /// 是否包含指定数据库编号的配置项
        /// </summary>
        /// <param name="dbid">指定数据库编号</param>
        public static bool ContainsConfigByDBID(int dbid)
        {
            return _configDic.ContainsKey(dbid);
        }

        /// <summary>
        /// 获取配置项
        /// </summary>
        /// <param name="dbid">数据库编号</param>
        /// <returns>配置项</returns>
        public static DBConfig GetConfigByDBID(int dbid)
        {
            DBConfig dbItem;
            if (!_configDic.TryGetValue(dbid, out dbItem))
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
        public static List<DBConfig> GetAllConfigs()
        {
            return _configDic.Values.ToList();
        }
    }
}
