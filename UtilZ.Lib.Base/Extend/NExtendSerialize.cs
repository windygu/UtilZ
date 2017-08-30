using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace UtilZ.Lib.Base.Extend
{
    /// <summary>
    /// 序列化扩展类[注:当对象中有继承后重写父类的属性时,不适用]
    /// </summary>
    public static class NExtendSerialize
    {
        #region XML序列化
        /// <summary>
        /// XML序列化
        /// </summary>
        /// <param name="obj">待序列化对象</param>
        /// <param name="filePath">序列化文件路径</param>
        public static void XmlSerializer(object obj, string filePath)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException("filePath");
            }

            XmlSerializer se = new XmlSerializer(obj.GetType());
            using (TextWriter tw = new StreamWriter(filePath))
            {
                se.Serialize(tw, obj);
            }
        }

        /// <summary>        
        /// XML反序列化
        /// </summary>
        /// <param name="filePath">序列化文件路径</param>
        /// <returns>反序列化后的对象</returns>
        public static T XmlDeSerializer<T>(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException("filePath");
            }

            XmlSerializer se = new XmlSerializer(typeof(T));
            using (TextReader tr = new StreamReader(filePath))
            {
                return (T)se.Deserialize(tr);
            }
        }
        #endregion

        #region 二进制序列化
        /// <summary>
        /// 二进制序列化
        /// </summary>
        /// <param name="obj">待序列化对象</param>
        /// <param name="filePath">序列化文件路径</param>
        public static void BinarySerialize(object obj, string filePath)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException("filePath");
            }

            BinaryFormatter bf = new BinaryFormatter();
            using (Stream stream = File.OpenWrite(filePath))
            {
                bf.Serialize(stream, obj);
            }
        }

        /// <summary>        
        /// 二进制反序列化
        /// </summary>
        /// <param name="filePath">序列化文件路径</param>
        /// <returns>反序列化后的对象</returns>
        public static T BinaryDeSerialize<T>(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException("filePath");
            }

            BinaryFormatter bf = new BinaryFormatter();
            using (Stream stream = File.OpenRead(filePath))
            {
                return (T)bf.Deserialize(stream);
            }
        }
        #endregion

        #region  JSON序列化
        /// <summary>
        /// JSON序列化
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <returns>json序列化之后的字符串</returns>
        public static string RuntimeSerializerObject(this object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            DataContractJsonSerializer json = new DataContractJsonSerializer(obj.GetType());
            //序列化
            using (MemoryStream stream = new MemoryStream())
            {
                json.WriteObject(stream, obj);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        /// <summary>
        /// JSON反序列化
        /// </summary>
        /// <typeparam name="T">反序列化之类的类型</typeparam>
        /// <param name="json">json字符串</param>
        /// <returns>反序列化之后的对象</returns>
        public static T RuntimeDeserializeObject<T>(this string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                throw new ArgumentNullException("json");
            }

            //反序列化
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                return (T)serializer.ReadObject(ms);
            }
        }

        /// <summary>
        /// JSON序列化
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <returns>json序列化之后的字符串</returns>
        public static string WebScriptSerializerObject(this object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(obj);
        }

        /// <summary>
        /// JSON反序列化
        /// </summary>
        /// <typeparam name="T">反序列化之类的类型</typeparam>
        /// <param name="json">待反序列化的json字符串</param>
        /// <returns>反序列化之后的对象</returns>
        public static T WebScriptDeserializeObject<T>(this string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                throw new ArgumentNullException("json");
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Deserialize<T>(json);
        }
        #endregion
    }
}
