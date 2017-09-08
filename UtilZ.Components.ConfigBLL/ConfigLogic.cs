using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UtilZ.Components.ConfigModel;
using UtilZ.Lib.DBBase.Core;
using UtilZ.Lib.DBBase.Interface;
using UtilZ.Lib.DBModel.Model;

namespace UtilZ.Components.ConfigBLL
{
    public class ConfigLogic
    {
        private IDBAccess _dbAccess;
        public ConfigLogic()
        {

        }

        public void Init()
        {
            this._dbAccess = DBAccessManager.GetDBAccessInstance(1);
        }

        public void ModifyConfigParaKeyValue(ConfigParaKeyValue configParaKeyValue, List<int> validDomainIds)
        {
            this._dbAccess.ExcuteAdoNetTransaction(new Tuple<ConfigParaKeyValue, List<int>>(configParaKeyValue, validDomainIds), this.ModifyConfigParaKeyValueTransaction);
        }

        private object ModifyConfigParaKeyValueTransaction(IDbConnection con, IDbTransaction transaction, object para)
        {
            var inPara = para as Tuple<ConfigParaKeyValue, List<int>>;
            ConfigParaKeyValue configParaKeyValue = inPara.Item1;
            List<int> validDomainIds = inPara.Item2;
            var interactionEx = this._dbAccess.InteractionEx;
            var paraSign = this._dbAccess.ParaSign;

            if (configParaKeyValue.ID > 0)
            {
                var cmdDel = this._dbAccess.CreateCommand(con);
                cmdDel.CommandText = string.Format("delete from ConfigParaValidDomain where CID={0}CID", paraSign);
                cmdDel.Parameters.Add(interactionEx.CreateDbParameter("CID", configParaKeyValue.ID));
                cmdDel.Transaction = transaction;
                cmdDel.ExecuteNonQuery();

                var cmdUpdate = this._dbAccess.CreateCommand(con);
                cmdUpdate.CommandText = string.Format("update ConfigParaKeyValue set Key={0}Key,Value={0}Value,GID={0}GID,Des={0}Des WHERE ID={0}ID", paraSign);
                cmdUpdate.Parameters.Add(interactionEx.CreateDbParameter("Key", configParaKeyValue.Key));
                cmdUpdate.Parameters.Add(interactionEx.CreateDbParameter("Value", configParaKeyValue.Value));
                cmdUpdate.Parameters.Add(interactionEx.CreateDbParameter("GID", configParaKeyValue.GID));
                cmdUpdate.Parameters.Add(interactionEx.CreateDbParameter("Des", configParaKeyValue.Des));
                cmdUpdate.Parameters.Add(interactionEx.CreateDbParameter("ID", configParaKeyValue.ID));
                cmdUpdate.Transaction = transaction;
                cmdUpdate.ExecuteNonQuery();

                var insertUpdate = this._dbAccess.CreateCommand(con);
                insertUpdate.CommandText = string.Format("insert into ConfigParaValidDomain (CID,SID) Values ({0}CID,{0}SID)", paraSign);
                insertUpdate.Transaction = transaction;
                insertUpdate.Parameters.Add(interactionEx.CreateDbParameter("CID", configParaKeyValue.ID));
                foreach (var validDomainId in validDomainIds)
                {
                    insertUpdate.Parameters.Add(interactionEx.CreateDbParameter("SID", validDomainId));
                    insertUpdate.ExecuteNonQuery();
                    insertUpdate.Parameters.RemoveAt(1);
                }
            }
            else
            {
                var cmdInsertConfigParaKeyValue = this._dbAccess.CreateCommand(con);
                cmdInsertConfigParaKeyValue.CommandText = string.Format("insert into ConfigParaKeyValue(Key,Value,GID,Des) Values({0}Key,{0}Value,{0}GID,{0}Des)", paraSign);
                cmdInsertConfigParaKeyValue.Parameters.Add(interactionEx.CreateDbParameter("Key", configParaKeyValue.Key));
                cmdInsertConfigParaKeyValue.Parameters.Add(interactionEx.CreateDbParameter("Value", configParaKeyValue.Value));
                cmdInsertConfigParaKeyValue.Parameters.Add(interactionEx.CreateDbParameter("GID", configParaKeyValue.GID));
                cmdInsertConfigParaKeyValue.Parameters.Add(interactionEx.CreateDbParameter("Des", configParaKeyValue.Des));
                cmdInsertConfigParaKeyValue.Parameters.Add(interactionEx.CreateDbParameter("ID", configParaKeyValue.ID));
                cmdInsertConfigParaKeyValue.Transaction = transaction;
                cmdInsertConfigParaKeyValue.ExecuteNonQuery();

                var insertUpdate = this._dbAccess.CreateCommand(con);
                insertUpdate.CommandText = string.Format("insert into ConfigParaValidDomain (CID,SID) Values ({0}CID,{0}SID)", paraSign);
                insertUpdate.Transaction = transaction;
                insertUpdate.Parameters.Add(interactionEx.CreateDbParameter("CID", configParaKeyValue.ID));
                foreach (var validDomainId in validDomainIds)
                {
                    insertUpdate.Parameters.Add(interactionEx.CreateDbParameter("SID", validDomainId));
                    insertUpdate.ExecuteNonQuery();
                    insertUpdate.Parameters.RemoveAt(1);
                }
            }

            return null;
        }

        public void SaveConfigParaServiceMap(List<ConfigParaServiceMap> addItems, List<ConfigParaServiceMap> delItems, List<ConfigParaServiceMap> updateItems)
        {
            this._dbAccess.ExcuteAdoNetTransaction(new Tuple<List<ConfigParaServiceMap>, List<ConfigParaServiceMap>, List<ConfigParaServiceMap>>(addItems, delItems, updateItems), this.SaveConfigParaServiceMapTransaction);
        }

        private object SaveConfigParaServiceMapTransaction(IDbConnection con, IDbTransaction transaction, object para)
        {
            var inPara = para as Tuple<List<ConfigParaServiceMap>, List<ConfigParaServiceMap>, List<ConfigParaServiceMap>>;
            List<ConfigParaServiceMap> addItems = inPara.Item1;
            List<ConfigParaServiceMap> delItems = inPara.Item2;
            List<ConfigParaServiceMap> updateItems = inPara.Item3;

            var interactionEx = this._dbAccess.InteractionEx;
            var paraSign = this._dbAccess.ParaSign;

            if (delItems.Count > 0)
            {
                var cmdDel = this._dbAccess.CreateCommand(con);
                cmdDel.Transaction = transaction;
                cmdDel.CommandText = string.Format("delete from ConfigParaServiceMap where ID={0}ID", paraSign);
                foreach (var delItem in delItems)
                {
                    cmdDel.Parameters.Add(interactionEx.CreateDbParameter("ID", delItem.ID));
                    cmdDel.ExecuteNonQuery();
                }
            }

            if (addItems.Count > 0)
            {
                var cmdInsert = this._dbAccess.CreateCommand(con);
                cmdInsert.Transaction = transaction;
                cmdInsert.CommandText = string.Format("INSERT INTO ConfigParaServiceMap (ServiceMapID, Name, Des) VALUES ({0}ServiceMapID, {0}Name, {0}Des)", paraSign);
                foreach (var addItem in addItems)
                {
                    cmdInsert.Parameters.Add(interactionEx.CreateDbParameter("ServiceMapID", addItem.ServiceMapID));
                    cmdInsert.Parameters.Add(interactionEx.CreateDbParameter("Name", addItem.Name));
                    cmdInsert.Parameters.Add(interactionEx.CreateDbParameter("Des", addItem.Des));
                    cmdInsert.ExecuteNonQuery();
                    cmdInsert.Parameters.Clear();
                }
            }

            if (updateItems.Count > 0)
            {
                var cmdUpdate = this._dbAccess.CreateCommand(con);
                cmdUpdate.Transaction = transaction;
                cmdUpdate.CommandText = string.Format("UPDATE ConfigParaServiceMap SET ServiceMapID={0}ServiceMapID, Name={0}Name,Des={0}Des Where ID={0}ID", paraSign);
                foreach (var updateItem in updateItems)
                {
                    cmdUpdate.Parameters.Add(interactionEx.CreateDbParameter("ServiceMapID", updateItem.ServiceMapID));
                    cmdUpdate.Parameters.Add(interactionEx.CreateDbParameter("Name", updateItem.Name));
                    cmdUpdate.Parameters.Add(interactionEx.CreateDbParameter("Des", updateItem.Des));
                    cmdUpdate.Parameters.Add(interactionEx.CreateDbParameter("ID", updateItem.ID));
                    cmdUpdate.ExecuteNonQuery();
                    cmdUpdate.Parameters.Clear();
                }
            }

            return null;
        }

        public void SaveGroup(List<ConfigParaGroup> addItems, List<ConfigParaGroup> delItems, List<ConfigParaGroup> updateItems)
        {
            this._dbAccess.ExcuteAdoNetTransaction(new Tuple<List<ConfigParaGroup>, List<ConfigParaGroup>, List<ConfigParaGroup>>(addItems, delItems, updateItems), this.SaveGroupTransaction);
        }

        private object SaveGroupTransaction(IDbConnection con, IDbTransaction transaction, object para)
        {
            var inPara = para as Tuple<List<ConfigParaGroup>, List<ConfigParaGroup>, List<ConfigParaGroup>>;
            List<ConfigParaGroup> addItems = inPara.Item1;
            List<ConfigParaGroup> delItems = inPara.Item2;
            List<ConfigParaGroup> updateItems = inPara.Item3;

            var interactionEx = this._dbAccess.InteractionEx;
            var paraSign = this._dbAccess.ParaSign;
            if (delItems.Count > 0)
            {
                var cmdDel = this._dbAccess.CreateCommand(con);
                cmdDel.Transaction = transaction;
                cmdDel.CommandText = string.Format("delete from ConfigParaGroup where ID={0}ID", paraSign);
                foreach (var delItem in delItems)
                {
                    cmdDel.Parameters.Add(interactionEx.CreateDbParameter("ID", delItem.ID));
                    cmdDel.ExecuteNonQuery();
                }
            }

            if (addItems.Count > 0)
            {
                var cmdInsert = this._dbAccess.CreateCommand(con);
                cmdInsert.Transaction = transaction;
                cmdInsert.CommandText = string.Format("INSERT INTO ConfigParaGroup (Name, Des) VALUES ({0}Name,{0}Des)", paraSign);
                foreach (var addItem in addItems)
                {
                    cmdInsert.Parameters.Add(interactionEx.CreateDbParameter("Name", addItem.Name));
                    cmdInsert.Parameters.Add(interactionEx.CreateDbParameter("Des", addItem.Des));
                    cmdInsert.ExecuteNonQuery();
                    cmdInsert.Parameters.Clear();
                }
            }

            if (updateItems.Count > 0)
            {
                var cmdUpdate = this._dbAccess.CreateCommand(con);
                cmdUpdate.Transaction = transaction;
                cmdUpdate.CommandText = string.Format("UPDATE ConfigParaGroup SET Name={0}Name,Des={0}Des Where ID={0}ID", paraSign);
                foreach (var updateItem in updateItems)
                {
                    cmdUpdate.Parameters.Add(interactionEx.CreateDbParameter("Name", updateItem.Name));
                    cmdUpdate.Parameters.Add(interactionEx.CreateDbParameter("Des", updateItem.Des));
                    cmdUpdate.Parameters.Add(interactionEx.CreateDbParameter("ID", updateItem.ID));
                    cmdUpdate.ExecuteNonQuery();
                    cmdUpdate.Parameters.Clear();
                }
            }

            return null;
        }

        public void SaveConfigParaKeyValueEdit(List<ConfigParaKeyValue> addItems, List<ConfigParaKeyValue> delItems, List<ConfigParaKeyValue> updateItems)
        {
            this._dbAccess.ExcuteAdoNetTransaction(new Tuple<List<ConfigParaKeyValue>, List<ConfigParaKeyValue>, List<ConfigParaKeyValue>>(addItems, delItems, updateItems), this.SaveConfigParaKeyValueEditTransaction);
        }

        private object SaveConfigParaKeyValueEditTransaction(IDbConnection con, IDbTransaction transaction, object para)
        {
            var inPara = para as Tuple<List<ConfigParaKeyValue>, List<ConfigParaKeyValue>, List<ConfigParaKeyValue>>;
            List<ConfigParaKeyValue> addItems = inPara.Item1;
            List<ConfigParaKeyValue> delItems = inPara.Item2;
            List<ConfigParaKeyValue> updateItems = inPara.Item3;

            var interactionEx = this._dbAccess.InteractionEx;
            var paraSign = this._dbAccess.ParaSign;
            try
            {
                if (delItems.Count > 0)
                {
                    var cmdDel = this._dbAccess.CreateCommand(con);
                    cmdDel.Transaction = transaction;
                    cmdDel.CommandText = string.Format("delete from ConfigParaKeyValue where ID={0}ID", paraSign);
                    foreach (var delItem in delItems)
                    {
                        cmdDel.Parameters.Add(interactionEx.CreateDbParameter("ID", delItem.ID));
                        cmdDel.ExecuteNonQuery();
                    }
                }

                if (addItems.Count > 0)
                {
                    var cmdInsert = this._dbAccess.CreateCommand(con);
                    cmdInsert.Transaction = transaction;
                    cmdInsert.CommandText = string.Format("INSERT INTO ConfigParaKeyValue (Key, Value, GID, Des) VALUES ({0}Key,{0}Value,{0}GID,{0}Des)", paraSign);
                    foreach (var addItem in addItems)
                    {
                        cmdInsert.Parameters.Add(interactionEx.CreateDbParameter("Key", addItem.Key));
                        cmdInsert.Parameters.Add(interactionEx.CreateDbParameter("Value", addItem.Value));
                        cmdInsert.Parameters.Add(interactionEx.CreateDbParameter("GID", addItem.GID));
                        cmdInsert.Parameters.Add(interactionEx.CreateDbParameter("Des", addItem.Des));
                        cmdInsert.ExecuteNonQuery();
                        cmdInsert.Parameters.Clear();
                    }
                }

                if (updateItems.Count > 0)
                {
                    var cmdUpdate = this._dbAccess.CreateCommand(con);
                    cmdUpdate.Transaction = transaction;
                    cmdUpdate.CommandText = string.Format("UPDATE ConfigParaKeyValue SET Key={0}Key,Value={0}Value,GID={0}GID,Des={0}Des Where ID={0}ID", paraSign);
                    foreach (var updateItem in updateItems)
                    {
                        cmdUpdate.Parameters.Add(interactionEx.CreateDbParameter("Key", updateItem.Key));
                        cmdUpdate.Parameters.Add(interactionEx.CreateDbParameter("Value", updateItem.Value));
                        cmdUpdate.Parameters.Add(interactionEx.CreateDbParameter("GID", updateItem.GID));
                        cmdUpdate.Parameters.Add(interactionEx.CreateDbParameter("Des", updateItem.Des));
                        cmdUpdate.Parameters.Add(interactionEx.CreateDbParameter("ID", updateItem.ID));
                        cmdUpdate.ExecuteNonQuery();
                        cmdUpdate.Parameters.Clear();
                    }
                }
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }

            return null;
        }

        public List<ConfigParaServiceMap> GetValidDomainConfigParaServiceMap(ConfigParaKeyValue configParaKeyValue)
        {
            var collection = new NDbParameterCollection();
            string sqlStr = string.Format(@"SELECT * from ConfigParaServiceMap WHERE ID in (select sid from ConfigParaValidDomain WHERE cid={0}cid)", this._dbAccess.ParaSign);
            collection.Add("cid", configParaKeyValue.ID);
            return this._dbAccess.QueryT<ConfigParaServiceMap>(sqlStr, collection);
        }

        public List<ConfigParaKeyValue> GetGroupConfigParaKeyValue(ConfigParaGroup selectedItem)
        {
            var query = new ConfigParaKeyValue();
            query.GID = selectedItem.ID;
            var conditionProperties = new List<string>();
            conditionProperties.Add(nameof(ConfigParaKeyValue.GID));
            var cobfigParas = this._dbAccess.QueryT<ConfigParaKeyValue>(query, conditionProperties);
            foreach (var item in cobfigParas)
            {
                item.Group = selectedItem;
            }

            return cobfigParas;
        }

        public List<ConfigParaGroup> GetAllConfigParaGroup()
        {
            var groups = this._dbAccess.QueryT<ConfigParaGroup>();
            if (groups.Count == 0)
            {
                var defaultGroup = new ConfigParaGroup();
                defaultGroup.Name = "默认组";
                defaultGroup.Des = "默认组";
                this._dbAccess.InsertT<ConfigParaGroup>(defaultGroup);
                groups = this._dbAccess.QueryT<ConfigParaGroup>();
            }

            return groups;
        }

        public List<ConfigParaKeyValue> GetAllConfigParaKeyValue()
        {
            return this._dbAccess.QueryT<ConfigParaKeyValue>();
        }

        public List<ConfigParaServiceMap> GetAllConfigParaServiceMap()
        {
            return this._dbAccess.QueryT<ConfigParaServiceMap>();
        }

        public List<ConfigParaServiceMap> GetConfigParaServiceMap(int id)
        {
            string sqlStr = string.Format("SELECT * FROM ConfigParaServiceMap WHERE ID IN (SELECT SID from ConfigParaValidDomain WHERE CID={0}CID)", this._dbAccess.ParaSign);
            NDbParameterCollection collection = new NDbParameterCollection();
            collection.Add("CID", id);
            var validServiceDic = this._dbAccess.QueryT<ConfigParaServiceMap>(sqlStr, collection).ToDictionary((tmpItem) => { return tmpItem.ID; });

            var allServcieMap = this._dbAccess.QueryT<ConfigParaServiceMap>();
            foreach (var servcieMap in allServcieMap)
            {
                servcieMap.IsSelected = validServiceDic.ContainsKey(servcieMap.ID);
            }

            return allServcieMap;
        }
    }
}
