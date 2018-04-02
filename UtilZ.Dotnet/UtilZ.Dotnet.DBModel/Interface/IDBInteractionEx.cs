using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.DBModel.Model;

namespace UtilZ.Dotnet.DBModel.Interface
{
    /// <summary>
    /// 外部数据库交互接口
    /// </summary>
    public interface IDBInteractionEx
    {
        /// <summary>
        /// 创建DbDataAdapter
        /// </summary>
        /// <returns>创建好的DbDataAdapter</returns>
        IDbDataAdapter CreateDbDataAdapter();

        /// <summary>
        /// 创建命令参数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">值</param>
        /// <returns>创建好的命令参数</returns>
        IDbDataParameter CreateDbParameter(string parameterName, object value);

        /// <summary>
        /// 创建命令参数
        /// </summary>
        /// <param name="parameter">命令参数</param>
        /// <returns>创建好的命令参数</returns>
        IDbDataParameter CreateDbParameter(NDbParameter parameter);

        /// <summary>
        /// 设置命令参数
        /// </summary>
        /// <param name="cmdParameter">命令参数</param>
        /// <param name="parameter">参数</param>
        void SetParameter(IDbDataParameter cmdParameter, NDbParameter parameter);

        /// <summary>
        /// 设置命令参数
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <param name="collection">参数集合</param>
        /// <returns>创建好的命令参数</returns>
        void SetParameter(IDbCommand cmd, NDbParameterCollection collection);

        /// <summary>
        /// 设置命令参数
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>创建好的命令参数</returns>
        void SetParameter(IDbCommand cmd, IEnumerable<IDbDataParameter> parameters);

        /// <summary>
        /// 设置命令参数
        /// </summary>
        /// <param name="cmd">命令参数</param>
        /// <param name="paraNames">参数名集合</param>
        /// <param name="values">参数值集合</param>
        void SetParameter(IDbCommand cmd, IEnumerable<string> paraNames, IEnumerable<object> values);

        /// <summary>
        /// 设置命令参数
        /// </summary>
        /// <param name="cmd">命令参数</param>
        /// <param name="paraValues">参数名及值字典集合</param>
        void SetParameter(IDbCommand cmd, Dictionary<string, object> paraValues);
    }
}
