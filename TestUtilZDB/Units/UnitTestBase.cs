using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TestUtilZDB.Model;
using UtilZ.Lib.Base.LocalMeseageQueue;
using UtilZ.Lib.DBBase.Core;
using UtilZ.Lib.DBBase.Interface;
using UtilZ.Lib.DBModel.Config;
using UtilZ.Lib.DBModel.Model;

namespace TestUtilZDB.Units
{
    public class UnitTestBase
    {
        //protected readonly IDBAccess _dbAccess;

        public UnitTestBase()
        {
            //_dbAccess = DBAccessManager.GetDBAccessInstance(dbid);
        }

        private IDBAccess GetDBAccess(int dbid)
        {
            return DBAccessManager.GetDBAccessInstance(dbid);
        }

        private void OutputValue(object value)
        {
            LMQCenter.Publish("xx", value);
        }

        public void Atom(DBConfigElement dbConfig)
        {
            IDBAccess dbAccess;
            List<Stu> stus = null;
            string sqlStr = null;
            object obj = null;
            NDbParameterCollection collection = null;
            try
            {
                dbAccess = this.GetDBAccess(dbConfig.DBID);
                List<DBConfigElement> configItems = ConfigManager.GetAllConfigItems();

                /******************************************************************************************************************/
                //collection = new NDbParameterCollection();
                //collection.Add("Name", "肖少林");
                //foreach (var config in configItems)
                //{
                //    dbAccess = this.GetDBAccess(config.DBID);
                //    sqlStr = @"insert into stu (Name,age,addr) values('陈天来',18,'东兴区')";
                //    obj = dbAccess.ExecuteNonQuery(sqlStr, DBVisitType.W);

                //    sqlStr = string.Format(@"insert into stu (Name,age,addr) values({0}Name,18,'东兴区')", dbAccess.ParaSign);
                //    obj = dbAccess.ExecuteNonQuery(sqlStr, DBVisitType.W, collection);
                //}
                /******************************************************************************************************************/


                /******************************************************************************************************************/
                collection = new NDbParameterCollection();
                collection.Add("ID", 1);
                foreach (var config in configItems)
                {
                    dbAccess = this.GetDBAccess(config.DBID);
                    sqlStr = string.Format(@"update Stu set age=101 where ID={0}ID", dbAccess.ParaSign);
                    obj = dbAccess.ExecuteScalar(sqlStr, DBVisitType.W, collection);
                }

                /******************************************************************************************************************/
            }
            catch (Exception ex)
            {

            }
        }

        internal void Insert(DBConfigElement dBConfigElement)
        {
            IDBAccess dbAccess = this.GetDBAccess(dBConfigElement.DBID);
            try
            {
                NDbParameterCollection collection = null;
                object obj;
                string sqlStr;
                List<DBConfigElement> configItems = ConfigManager.GetAllConfigItems();

                ///******************************************************************************************************************/
                //collection = new NDbParameterCollection();
                //collection.Add("Addr", "中国地球美国大家");
                //foreach (var config in configItems)
                //{
                //    sqlStr = @"insert into stu (Name,age,addr) values('zhn',23,'成都')";
                //    obj = dbAccess.Insert(sqlStr);

                //    sqlStr = string.Format(@"insert into stu (Name,age,addr) values('zhn',23,{0}Addr)", dbAccess.ParaSign);
                //    obj = dbAccess.Insert(sqlStr, collection);
                //}

                ///******************************************************************************************************************/
                //Stu stu = new Stu();
                //stu.Name = "金光露";
                //stu.Age = 28;
                //stu.Addr = "heb";
                //foreach (var conf in configItems)
                //{
                //    try
                //    {
                //        dbAccess = this.GetDBAccess(conf.DBID);
                //        obj = dbAccess.InsertT<Stu>(stu);
                //    }
                //    catch (Exception exi)
                //    {
                //        obj = string.Format("{0}.{1}", conf.ConName, exi.Message);
                //    }

                //    this.OutputValue(obj);
                //}

                ///******************************************************************************************************************/
                DataTable dt = new DataTable();
                dt.TableName = "Stu";
                dt.Columns.Add("Name", typeof(string));
                dt.Columns.Add("Age", typeof(int));
                dt.Columns.Add("Addr", typeof(string));
                dt.Rows.Add(new object[] { "th", 55, "四川" });
                dt.Rows.Add(new object[] { "jzm", 34, "广东" });
                dt.Rows.Add(new object[] { "hjt", 54, "北京" });
                foreach (var conf in configItems)
                {
                    try
                    {
                        dbAccess = this.GetDBAccess(conf.DBID);
                        obj = dbAccess.BatchInsert(dt.TableName, dt);
                    }
                    catch (Exception exi)
                    {
                        obj = string.Format("{0}.{1}", dBConfigElement.ConName, exi.Message);
                    }

                    this.OutputValue(obj);
                }

                /******************************************************************************************************************/
                List<string> cols = new List<string>() { "Name", "Age", "Addr" };
                List<object[]> data = new List<object[]>();
                data.Add(new object[] { "李岣", 39, "浙江2" });
                data.Add(new object[] { "张三", 27, "重庆2" });
                data.Add(new object[] { "无风", 18, "南京2" });

                foreach (var conf in configItems)
                {
                    try
                    {
                        dbAccess = this.GetDBAccess(conf.DBID);
                        obj = dbAccess.BatchInsert("Stu", cols, data);
                    }
                    catch (Exception exi)
                    {
                        obj = string.Format("{0}.{1}", dBConfigElement.ConName, exi.Message);
                    }

                    this.OutputValue(obj);
                }

                /******************************************************************************************************************/
                List<NDbParameterCollection> collections = new List<NDbParameterCollection>();
                collection = new NDbParameterCollection();
                collection.Add("Name", "张成");
                collection.Add("Age", 36);
                collection.Add("Addr", "内江2");
                collections.Add(collection);

                collection = new NDbParameterCollection();
                collection.Add("Name", "李小二");
                collection.Add("Age", 37);
                collection.Add("Addr", "内江444");
                collections.Add(collection);

                foreach (var conf in configItems)
                {
                    try
                    {
                        dbAccess = this.GetDBAccess(conf.DBID);
                        sqlStr = string.Format(@"insert into stu (Name,Age,Addr) Values({0}Name,{0}Age,{0}Addr)", dbAccess.ParaSign);
                        obj = dbAccess.BatchInsert(sqlStr, collections);
                    }
                    catch (Exception exi)
                    {
                        obj = string.Format("{0}.{1}", dBConfigElement.ConName, exi.Message);
                    }

                    this.OutputValue(obj);
                }

                /******************************************************************************************************************/
                List<Stu> stus = new List<Stu>();
                stus.Add(new Stu() { Name = "", Age = 25, Addr = "" });
                stus.Add(new Stu() { Name = "", Age = 25, Addr = "" });
                stus.Add(new Stu() { Name = "", Age = 25, Addr = "" });

                foreach (var conf in configItems)
                {
                    try
                    {
                        dbAccess = this.GetDBAccess(conf.DBID);
                        obj = dbAccess.BatchInsertT(stus);
                    }
                    catch (Exception exi)
                    {
                        obj = string.Format("{0}.{1}", dBConfigElement.ConName, exi.Message);
                    }

                    this.OutputValue(obj);
                }
            }
            catch (Exception ex)
            {

            }
        }

        internal void StoredProc(DBConfigElement dBConfigElement)
        {
            try
            {
                //IDBAccess dbAccess = this.GetDBAccess(dBConfigElement.DBID);
                //NDbParameterCollection collection = null;
                //object obj;
                //string sqlStr;
                //List<DBConfigElement> configItems = ConfigManager.GetAllConfigItems();

                ///******************************************************************************************************************/
                //collection = new NDbParameterCollection();
                //collection.Add("Addr", "中国地球美国大家");

                //for (int i = 0; i < 100; i++)
                //{
                //    sqlStr = @"insert into stu (Name,age,addr) values('zhn',23,'成都')";
                //    obj = dbAccess.Insert(sqlStr);

                //    sqlStr = string.Format(@"insert into stu (Name,age,addr) values('zhn',23,{0}Addr)", dbAccess.ParaSign);
                //    obj = dbAccess.Insert(sqlStr, collection);
                //}
            }
            catch (Exception ex)
            {
                this.OutputValue(ex.Message);
            }
        }

        internal void TestDBInfo(DBConfigElement dBConfigElement)
        {
            try
            {
                IDBAccess dbAccess = this.GetDBAccess(dBConfigElement.DBID);
                List<DBConfigElement> configItems = ConfigManager.GetAllConfigItems();
                object value = null;
                foreach (var conf in configItems)
                {
                    try
                    {
                        dbAccess = this.GetDBAccess(conf.DBID);
                        value = dbAccess.IsExistTable("STU");
                        value = dbAccess.IsExistField("STU", "Name");
                        value = dbAccess.GetTableBinaryFieldInfo("STU").Count();
                        value = dbAccess.GetTableFieldInfos("STU").Count();

                        value = dbAccess.GetTableInfos(true).Count();
                        value = dbAccess.QueryPrikeyColumns("STU").Count();
                        value = dbAccess.GetTableInfo("stu", true);
                        value = dbAccess.GetDataBaseVersion();
                        value = dbAccess.GetDataBaseSysTime();
                    }
                    catch (Exception exi)
                    {
                        value = string.Format("{0}.{1}", conf.ConName, exi.Message);
                    }

                    this.OutputValue(value);
                }
            }
            catch (Exception ex)
            {
                this.OutputValue(ex.Message);
            }
        }

        internal void TestUpdate(DBConfigElement dBConfigElement)
        {
            IDBAccess dbAccess = this.GetDBAccess(dBConfigElement.DBID);
            try
            {
                List<DBConfigElement> configItems = ConfigManager.GetAllConfigItems();
                object value = null;
                string sqlStr;

                /******************************************************************************************************************/
                foreach (var conf in configItems)
                {
                    try
                    {
                        dbAccess = this.GetDBAccess(conf.DBID);
                        value = dbAccess.Update("Stu", "ID", 5, "Name", "张无风100");
                    }
                    catch (Exception exi)
                    {
                        value = string.Format("{0}.{1}", dBConfigElement.ConName, exi.Message);
                    }

                    this.OutputValue(value);
                }

                /******************************************************************************************************************/
                Dictionary<string, object> priKeyColValues = new Dictionary<string, object>();
                Dictionary<string, object> colValues = new Dictionary<string, object>();
                priKeyColValues.Add("ID", 12);
                colValues.Add("Name", "张辰汐");
                foreach (var conf in configItems)
                {
                    try
                    {
                        dbAccess = this.GetDBAccess(conf.DBID);
                        value = dbAccess.Update("Stu", priKeyColValues, colValues);
                    }
                    catch (Exception exi)
                    {
                        value = string.Format("{0}.{1}", dBConfigElement.ConName, exi.Message);
                    }

                    this.OutputValue(value);
                }

                /******************************************************************************************************************/
                NDbParameterCollection collection = new NDbParameterCollection();
                collection.Add("Age", 55);
                collection.Add("ID", 13);
                foreach (var conf in configItems)
                {
                    try
                    {
                        dbAccess = this.GetDBAccess(conf.DBID);
                        sqlStr = string.Format(@"update stu SET Age={0}Age Where ID={0}ID", dbAccess.ParaSign);
                        value = dbAccess.Update(sqlStr, collection);
                    }
                    catch (Exception exi)
                    {
                        value = string.Format("{0}.{1}", dBConfigElement.ConName, exi.Message);
                    }

                    this.OutputValue(value);
                }

                /******************************************************************************************************************/
                Stu stu = new Stu() { ID = 3, Name = "刘宇2", Age = 55, Addr = "中国四川省成都市金牛区你妹的" };
                List<string> updateProperties = null;
                //updateProperties = new List<string>() { "Name" };
                foreach (var conf in configItems)
                {
                    try
                    {
                        dbAccess = this.GetDBAccess(conf.DBID);
                        value = dbAccess.UpdateT(stu, updateProperties);
                    }
                    catch (Exception exi)
                    {
                        value = string.Format("{0}.{1}", dBConfigElement.ConName, exi.Message);
                    }

                    this.OutputValue(value);
                }

                /******************************************************************************************************************/
                List<NDbParameterCollection> collections = new List<NDbParameterCollection>();
                var collection2 = new NDbParameterCollection();
                collection2.Add("ID", 1);
                collections.Add(collection2);

                collection2 = new NDbParameterCollection();
                collection2.Add("ID", 2);
                collections.Add(collection2);

                collection2 = new NDbParameterCollection();
                collection2.Add("ID", 3);
                collections.Add(collection2);

                foreach (var conf in configItems)
                {
                    try
                    {
                        dbAccess = this.GetDBAccess(conf.DBID);
                        sqlStr = string.Format(@"update stu SET Age=33 Where ID={0}ID", dbAccess.ParaSign);
                        value = dbAccess.BatchUpdate(sqlStr, collections);
                    }
                    catch (Exception exi)
                    {
                        value = string.Format("{0}.{1}", dBConfigElement.ConName, exi.Message);
                    }

                    this.OutputValue(value);
                }

                /******************************************************************************************************************/
                List<string> sqlStrs = new List<string>();
                sqlStrs.Add(@"update stu SET Age=41 Where ID=4");
                sqlStrs.Add(@"update stu SET Age=42 Where ID=5");
                foreach (var conf in configItems)
                {
                    try
                    {
                        dbAccess = this.GetDBAccess(conf.DBID);
                        value = dbAccess.BatchUpdate(sqlStrs);
                    }
                    catch (Exception exi)
                    {
                        value = string.Format("{0}.{1}", dBConfigElement.ConName, exi.Message);
                    }

                    this.OutputValue(value);
                }

                /******************************************************************************************************************/
                List<Stu> stus = new List<Stu>();
                stus.Add(new Stu() { ID = 3, Name = "孙耀琦11", Age = 31, Addr = "四川省11" });
                stus.Add(new Stu() { ID = 4, Name = "孙耀琦22", Age = 22, Addr = "四川省22" });
                stus.Add(new Stu() { ID = 5, Name = "孙耀琦33", Age = 33, Addr = "四川省33" });
                updateProperties = null;
                updateProperties = new List<string>() { "Name" };
                foreach (var conf in configItems)
                {
                    try
                    {
                        dbAccess = this.GetDBAccess(conf.DBID);
                        value = dbAccess.BatchUpdateT(stus, updateProperties);
                    }
                    catch (Exception exi)
                    {
                        value = string.Format("{0}.{1}", dBConfigElement.ConName, exi.Message);
                    }

                    this.OutputValue(value);
                }
            }
            catch (Exception ex)
            {

            }
        }

        internal void TestDelete(DBConfigElement dBConfigElement)
        {
            IDBAccess dbAccess = this.GetDBAccess(dBConfigElement.DBID);
            try
            {
                List<DBConfigElement> configItems = ConfigManager.GetAllConfigItems();
                NDbParameterCollection collection = new NDbParameterCollection();
                object value = null;
                string sqlStr;

                /******************************************************************************************************************/
                collection.Add("ID", 26);
                long ret = 0;
                foreach (var conf in configItems)
                {
                    try
                    {
                        dbAccess = this.GetDBAccess(conf.DBID);
                        sqlStr = @"delete from stu Where ID=25";
                        ret = dbAccess.Delete(sqlStr);

                        sqlStr = string.Format(@"delete from stu Where ID={0}ID", dbAccess.ParaSign);
                        ret += dbAccess.Delete(sqlStr, collection);
                        value = ret;
                    }
                    catch (Exception exi)
                    {
                        value = string.Format("{0}.{1}", dBConfigElement.ConName, exi.Message);
                    }

                    this.OutputValue(value);
                }

                /******************************************************************************************************************/
                foreach (var conf in configItems)
                {
                    try
                    {
                        dbAccess = this.GetDBAccess(conf.DBID);
                        value = dbAccess.Delete("Stu", "ID", 27);
                    }
                    catch (Exception exi)
                    {
                        value = string.Format("{0}.{1}", dBConfigElement.ConName, exi.Message);
                    }

                    this.OutputValue(value);
                }

                /******************************************************************************************************************/
                Dictionary<string, object> priKeyColValues = new Dictionary<string, object>();
                priKeyColValues.Add("ID", 28);
                foreach (var conf in configItems)
                {
                    try
                    {
                        dbAccess = this.GetDBAccess(conf.DBID);
                        value = dbAccess.Delete("Stu", priKeyColValues);
                    }
                    catch (Exception exi)
                    {
                        value = string.Format("{0}.{1}", dBConfigElement.ConName, exi.Message);
                    }

                    this.OutputValue(value);
                }

                /******************************************************************************************************************/
                Stu stu = new Stu { ID = 46, Name = "zhn", Age = 29 };
                foreach (var conf in configItems)
                {
                    try
                    {
                        dbAccess = this.GetDBAccess(conf.DBID);
                        value = dbAccess.DeleteT(stu);
                    }
                    catch (Exception exi)
                    {
                        value = string.Format("{0}.{1}", dBConfigElement.ConName, exi.Message);
                    }

                    this.OutputValue(value);
                }

                /******************************************************************************************************************/
                List<string> sqlStrs = new List<string>();
                foreach (var conf in configItems)
                {
                    try
                    {
                        dbAccess = this.GetDBAccess(conf.DBID);
                        sqlStrs.Clear();
                        sqlStrs.Add(@"delete from stu Where ID=30");
                        sqlStrs.Add(@"delete from stu Where ID=31");
                        value = dbAccess.BatchDelete(sqlStrs);
                    }
                    catch (Exception exi)
                    {
                        value = string.Format("{0}.{1}", dBConfigElement.ConName, exi.Message);
                    }

                    this.OutputValue(value);
                }

                /******************************************************************************************************************/
                List<Dictionary<string, object>> priKeyColValues2 = new List<Dictionary<string, object>>();
                var priKeyValue = new Dictionary<string, object>();
                priKeyValue.Add("ID", 32);
                priKeyColValues2.Add(priKeyValue);

                priKeyValue = new Dictionary<string, object>();
                priKeyValue.Add("ID", 33);
                priKeyColValues2.Add(priKeyValue);

                foreach (var conf in configItems)
                {
                    try
                    {
                        dbAccess = this.GetDBAccess(conf.DBID);
                        value = dbAccess.BatchDelete("Stu", priKeyColValues2);
                    }
                    catch (Exception exi)
                    {
                        value = string.Format("{0}.{1}", dBConfigElement.ConName, exi.Message);
                    }

                    this.OutputValue(value);
                }

                /******************************************************************************************************************/
                List<NDbParameterCollection> collections = new List<NDbParameterCollection>();
                collection = new NDbParameterCollection();
                collection.Add("ID", 34);
                collections.Add(collection);

                collection = new NDbParameterCollection();
                collection.Add("ID", 35);
                collections.Add(collection);

                foreach (var conf in configItems)
                {
                    try
                    {
                        dbAccess = this.GetDBAccess(conf.DBID);
                        sqlStr = string.Format(@"delete from stu Where ID={0}ID", dbAccess.ParaSign);
                        value = dbAccess.BatchDelete(sqlStr, collections);
                    }
                    catch (Exception exi)
                    {
                        value = string.Format("{0}.{1}", dBConfigElement.ConName, exi.Message);
                    }

                    this.OutputValue(value);
                }

                /******************************************************************************************************************/
                List<Stu> stus = new List<Stu>();
                stus.Add(new Stu() { ID = 36, Name = "孙耀琦1", Age = 21, Addr = "四川省1" });
                stus.Add(new Stu() { ID = 37, Name = "孙耀琦2", Age = 22, Addr = "四川省2" });
                stus.Add(new Stu() { ID = 38, Name = "孙耀琦3", Age = 23, Addr = "四川省3" });
                foreach (var conf in configItems)
                {
                    try
                    {
                        dbAccess = this.GetDBAccess(conf.DBID);
                        value = dbAccess.BatchDeleteT(stus);
                    }
                    catch (Exception exi)
                    {
                        value = string.Format("{0}.{1}", dBConfigElement.ConName, exi.Message);
                    }

                    this.OutputValue(value);
                }
            }
            catch (Exception ex)
            {

            }
        }


        internal DBPageInfo QueryPage(DBConfigElement dBConfigElement, int pageSize)
        {
            IDBAccess dbAccess = this.GetDBAccess(dBConfigElement.DBID);
            return dbAccess.QueryPageInfo(pageSize, "select count(0) from Stu");
        }

        private readonly List<string> _priKeyCols = new List<string>() { "ID" };
        private readonly List<DBOrderInfo> _orderInfos = new List<DBOrderInfo>() { new DBOrderInfo("ID", true) };

        internal object QueryData(DBConfigElement dBConfigElement, int pageIndex, int pageSize, out int count)
        {
            IDBAccess dbAccess = this.GetDBAccess(dBConfigElement.DBID);
            DataTable dt;
            //dt= dbAccess.QueryPagingData("select * from Stu", "ID", pageSize, pageIndex, true, null, _priKeyCols);
            //dt = dbAccess.QueryPagingData("select * from Stu", _orderInfos, pageSize, pageIndex, true, null, _priKeyCols);
            //count = dt.Rows.Count;
            //return dt;

            List<Stu> stus;
            //stus = dbAccess.Query<Stu>("ID", pageSize, pageIndex, true, null, _priKeyCols);
            stus = dbAccess.QueryT<Stu>(_orderInfos, pageSize, pageIndex, true, null, _priKeyCols);
            count = stus.Count;
            return stus;
        }
    }
}
