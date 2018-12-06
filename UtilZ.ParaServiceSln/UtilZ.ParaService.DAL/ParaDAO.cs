using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UtilZ.Dotnet.DBBase.Interfaces;
using UtilZ.Dotnet.DBIBase.DBModel.Model;
using UtilZ.ParaService.DBModel;

namespace UtilZ.ParaService.DAL
{
    public class ParaDAO : BaseDAO
    {
        public ParaDAO() : base()
        {

        }

        public List<Para> QueryParas(long projectId, long paraGroupId, int pageSize, int pageIndex)
        {
            IDBAccess dbAccess = base.GetDBAccess();
            var parameters = new NDbParameterCollection();
            string paraSign = dbAccess.ParaSign;
            string sqlStr;
            if (paraGroupId > 0)
            {
                sqlStr = string.Format(@"SELECT ID,GroupID,Key,Name,Des FROM Para WHERE ProjectID={0}ProjectID AND GroupID={0}GroupID", paraSign);
                parameters.Add("GroupID", paraGroupId);
            }
            else
            {
                sqlStr = string.Format(@"SELECT ID,GroupID,Key,Name,Des FROM Para WHERE ProjectID={0}ProjectID", paraSign);
            }

            parameters.Add("ProjectID", projectId);
            DataTable dt;
            if (pageIndex > 0)
            {
                dt = dbAccess.QueryPagingData(sqlStr, "ID", pageSize, pageIndex, false, parameters);
            }
            else
            {
                dt = dbAccess.QueryData(sqlStr, parameters);
            }

            var paras = new List<Para>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    var para = new Para();
                    para.ProjectID = projectId;
                    para.ID = (long)(row[0]);
                    para.GroupID = (long)(row[1]);
                    para.Key = row[2].ToString();
                    para.Name = row[3].ToString();
                    para.Des = row[4].ToString();
                    paras.Add(para);
                }
            }

            return paras;
        }
    }
}
