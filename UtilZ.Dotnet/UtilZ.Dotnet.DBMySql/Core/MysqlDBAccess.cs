using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilZ.Dotnet.DBIBase.Config;
using UtilZ.Dotnet.DBIBase.Core;
using UtilZ.Dotnet.DBIBase.Interaction;

namespace UtilZ.Dotnet.DBMySql.Core
{
    internal partial class MySqlDBAccess : DBAccessAbs
    {
        /// <summary>
        /// sql语句最大长度
        /// </summary>
        private const long SQL_MAX_LENGTH = 1048576;
        public MySqlDBAccess(IDBInteraction dbInteraction, DatabaseConfig config, string databaseTypeName)
            : base(dbInteraction, config, databaseTypeName, SQL_MAX_LENGTH)
        {

        }
    }
}
