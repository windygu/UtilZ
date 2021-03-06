﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Base;

namespace UtilZ.Dotnet.ILWindowEx.Winform.PageGrid
{
    /// <summary>
    /// 分页显示控件公共类
    /// </summary>
    public class PageGridControlCommon
    {
        /// <summary>
        /// 显示列设置窗口标题
        /// </summary>
        public const string DisplayColSettingFormText = "隐藏列列表";

        /// <summary>
        /// 当前程序集所在目录
        /// </summary>
        private static string _currentAssemblyDirectory = null;

        /// <summary>
        /// 获取设置默认存放目录
        /// </summary>
        /// <param name="defaultDirFlag">设置默认目录标识[true:应用程序目录;false:用户目录;默认为true]</param>
        /// <returns>设置默认存放目录</returns>
        public static string GetDefaultSettingDirectory(bool defaultDirFlag = true)
        {
            string settingDirectory = null;
            if (defaultDirFlag)
            {
                if (string.IsNullOrEmpty(PageGridControlCommon._currentAssemblyDirectory))
                {
                    PageGridControlCommon._currentAssemblyDirectory = ObjectEx.GetAssemblyDirectory<PageGridControlCommon>();
                }

                settingDirectory = System.IO.Path.Combine(PageGridControlCommon._currentAssemblyDirectory, "Setting");
            }
            else
            {
                //获取列设置存放目录
                string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string appName = System.IO.Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName);
                //目录格式:appDataPath/appName/Setting/xx.xml
                settingDirectory = System.IO.Path.Combine(appDataPath, appName, "Setting");
            }

            return settingDirectory;
        }

        /// <summary>
        /// 获取表格列设置文件路径
        /// </summary>
        /// <param name="settingDirectory">用户设置数据存放目录</param>
        /// <param name="dataSourceName">数据源名称</param>
        /// <returns>表格列设置文件路径</returns>
        public static string GetGridColumnSettingFilePath(string settingDirectory, string dataSourceName)
        {
            return System.IO.Path.Combine(settingDirectory, dataSourceName + ".xml");
        }

        /// <summary>
        /// 判断数据源是否改变[改变返回true,否则返回false]
        /// </summary>
        /// <param name="oldDataSourceName">旧的数据源名称</param>
        /// <param name="oldTable">旧的数据表</param>
        /// <param name="dataSourceName">数据源名称</param>
        /// <param name="dt">数据表</param>
        /// <returns>改变返回true,否则返回false</returns>
        public static bool IsDataSourceChanged(string oldDataSourceName, DataTable oldTable, string dataSourceName, DataTable dt)
        {
            //新数据源名称为空或null抛出异常
            if (string.IsNullOrEmpty(dataSourceName))
            {
                throw new ArgumentNullException("dataSourceName");
            }

            //上次数据源名称为空.本次不为空,返回true
            if (string.IsNullOrEmpty(oldDataSourceName))
            {
                return true;
            }

            //如果上次数据源名称与本次数据源名称不同返回true,否则再判断数据源结构是否有差异
            if (dataSourceName.Equals(oldDataSourceName))
            {
                //上次数据源名称与本次数据源名称相同,再验证表结构是否一样
                //如果原始数据源为null则返回true
                if (oldTable == null)
                {
                    return true;
                }

                //列数不同,已改变
                if (dt.Columns.Count != oldTable.Columns.Count)
                {
                    return true;
                }

                DataColumn oldCol = null;
                //遍历列判断类型
                foreach (DataColumn newCol in dt.Columns)
                {
                    //如果新数据源列名在老数据源中不包含,返回true
                    if (!oldTable.Columns.Contains(newCol.ColumnName))
                    {
                        return true;
                    }

                    oldCol = oldTable.Columns[newCol.ColumnName];
                    //如果新数据源列数据类型与老数据源中列数据类型不同,返回true
                    if (oldCol.DataType != newCol.DataType)
                    {
                        return true;
                    }

                    //如果在行中的顺序不同则返回true
                    if (oldCol.Ordinal != newCol.Ordinal)
                    {
                        return true;
                    }
                }

                return false;
            }
            else
            {
                //上次数据源名称与本次数据源名称不同返回true
                return true;
            }
        }

        /// <summary>
        /// 设置DXGridControl控件中的列宽度为自适应内容宽度
        /// </summary>
        /// <param name="gridControl">GridControl</param>
        public static void SetDXGridViewColumnAutoSizeToDisplayedHeaders(dynamic gridControl)
        {
            if (gridControl.GridView == null)
            {
                return;
            }

            gridControl.GridView.BestFitColumns();
        }

        /// <summary>
        /// 获取页信息字符串
        /// </summary>
        /// <param name="pageInfo">页信息</param>
        /// <returns>页信息字符串</returns>
        public static string GetPageInfoStr(PageInfo pageInfo)
        {
            string pageStr = null;
            if (pageInfo == null)
            {
                pageStr = "第0页/共0页";
            }
            else
            {
                pageStr = string.Format("第{0}页/共{1}页", pageInfo.PageIndex, pageInfo.PageCount);
            }

            return pageStr;
        }

        /// <summary>
        /// 获取记录信息字符串
        /// </summary>
        /// <param name="pageInfo">页信息</param>
        /// <param name="recordIndex">当前页索引</param>
        /// <returns>记录信息字符串</returns>
        public static string GetRecordInfoStr(PageInfo pageInfo, int recordIndex)
        {
            string recordInfo = null;
            if (pageInfo == null)
            {
                recordInfo = "第0条/共0条";
            }
            else
            {
                if (pageInfo.TotalCount > 0)
                {
                    if (pageInfo.IsCurrentPage)
                    {
                        //当前页
                        recordInfo = string.Format("当前页第{0}条/共{1}条", recordIndex, pageInfo.Count);
                    }
                    else
                    {
                        //总记录索引
                        recordIndex = pageInfo.PageSize * (pageInfo.PageIndex - 1) + recordIndex;
                        recordInfo = string.Format("第{0}条/共{1}条", recordIndex, pageInfo.TotalCount);
                    }
                }
                else
                {
                    if (pageInfo.IsCurrentPage)
                    {
                        recordInfo = "当前页第0条/共0条";
                    }
                    else
                    {
                        recordInfo = "第0条/共0条";
                    }
                }
            }

            return recordInfo;
        }
    }
}
