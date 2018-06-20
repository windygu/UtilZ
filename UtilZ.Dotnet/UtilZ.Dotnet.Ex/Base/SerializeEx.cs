using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace UtilZ.Dotnet.Ex.Base
{
    /// <summary>
    /// 序列化扩展类[注:当对象中有继承后重写父类的属性时,不适用]
    /// </summary>
    public static class SerializeEx
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

            string dir = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrWhiteSpace(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            XmlSerializer se = new XmlSerializer(obj.GetType());
            using (TextWriter tw = new StreamWriter(filePath))
            {
                se.Serialize(tw, obj);
            }
        }

        /// <summary>
        /// XML序列化
        /// </summary>
        /// <param name="obj">待序列化对象</param>
        /// <returns>xml</returns>
        public static string XmlSerializer(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            XmlSerializer se = new XmlSerializer(obj.GetType());
            using (var ms = new MemoryStream())
            {
                se.Serialize(ms, obj);
                return Encoding.UTF8.GetString(ms.GetBuffer());
            }
        }

        /// <summary>        
        /// XML反序列化
        /// </summary>
        /// <param name="filePath">序列化文件路径</param>
        /// <returns>反序列化后的对象</returns>
        public static T XmlDeserializerFromFile<T>(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException("filePath");
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("序列化文件不存在", filePath);
            }

            XmlSerializer se = new XmlSerializer(typeof(T));
            using (TextReader tr = new StreamReader(filePath))
            {
                return (T)se.Deserialize(tr);
            }
        }

        /// <summary>        
        /// XML反序列化
        /// </summary>
        /// <param name="filePath">序列化文件路径</param>
        /// <param name="type">目标类型</param>
        /// <returns>反序列化后的对象</returns>
        public static object XmlDeserializerFromFile(string filePath, Type type)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException("filePath");
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("序列化文件不存在", filePath);
            }

            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            XmlSerializer se = new XmlSerializer(type);
            using (TextReader tr = new StreamReader(filePath))
            {
                return se.Deserialize(tr);
            }
        }

        /// <summary>        
        /// XML反序列化
        /// </summary>
        /// <param name="xmlStr">xml</param>
        /// <returns>反序列化后的对象</returns>
        public static T XmlDeserializerFromString<T>(string xmlStr)
        {
            if (string.IsNullOrEmpty(xmlStr))
            {
                throw new ArgumentNullException("xmlStr");
            }

            XmlSerializer se = new XmlSerializer(typeof(T));
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(xmlStr)))
            {
                return (T)se.Deserialize(ms);
            }
        }

        /// <summary>        
        /// XML反序列化
        /// </summary>
        /// <param name="xmlStr">xml</param>
        /// <param name="type">目标类型</param>
        /// <returns>反序列化后的对象</returns>
        public static object XmlDeserializerFromString(string xmlStr, Type type)
        {
            if (string.IsNullOrEmpty(xmlStr))
            {
                throw new ArgumentNullException("xmlStr");
            }

            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            XmlSerializer se = new XmlSerializer(type);
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(xmlStr)))
            {
                return se.Deserialize(ms);
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

            string dir = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrWhiteSpace(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            BinaryFormatter bf = new BinaryFormatter();
            using (Stream stream = File.OpenWrite(filePath))
            {
                bf.Serialize(stream, obj);
            }
        }

        /// <summary>
        /// 可序列化对象序列化为byte数组
        /// </summary>
        /// <param name="obj">可序列化对象</param>
        /// <returns>byte数组</returns>
        public static byte[] BinarySerialize(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            using (var memoryStream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(memoryStream, obj);
                return memoryStream.GetBuffer();
            }
        }

        /// <summary>        
        /// 二进制反序列化
        /// </summary>
        /// <param name="filePath">序列化文件路径</param>
        /// <returns>反序列化后的对象</returns>
        public static T BinaryDeserialize<T>(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException("filePath");
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("序列化文件不存在", filePath);
            }

            BinaryFormatter bf = new BinaryFormatter();
            using (Stream stream = File.OpenRead(filePath))
            {
                return (T)bf.Deserialize(stream);
            }
        }

        /// <summary>
        /// 二进制转换为可序列化的对象
        /// </summary>
        /// <typeparam name="T">可序列化的类型 </typeparam>
        /// <param name="buffer">byte数组</param>
        /// <returns>可序列化的类型实例</returns>
        public static T BinaryDeserialize<T>(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0)
            {
                return default(T);
            }

            using (var memoryStream = new MemoryStream(buffer))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                memoryStream.Position = 0;
                return (T)formatter.Deserialize(memoryStream);
            }
        }
        #endregion

        #region  JSON序列化
        #region DataContractJsonSerializer
        /// <summary>
        /// JSON序列化
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <returns>json序列化之后的字符串</returns>
        public static string RuntimeJsonSerializerObject(object obj)
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
        public static T RuntimeJsonDeserializeObject<T>(string json)
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
        #endregion

        #region JavaScriptSerializer
        /// <summary>
        /// JSON序列化
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <returns>json序列化之后的字符串</returns>
        public static string WebScriptJsonSerializerObject(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(obj);
        }

        /// <summary>
        /// 序列化对象并将生成的 JSON 字符串写入指定的 System.Text.StringBuilder 对象;
        /// 异常:
        ///   T:System.InvalidOperationException:
        ///     生成的 JSON 字符串超过最值 System.Web.Script.Serialization.JavaScriptSerializer.MaxJsonLength。-
        ///     或 - obj 包含循环引用。 当子对象具有对父对象的引用，而且父对象具有子对象的引用时，将出现循环引用。
        ///   T:System.ArgumentException:
        ///     定义的递归限制 System.Web.Script.Serialization.JavaScriptSerializer.RecursionLimit 超出了。
        ///   T:System.ArgumentNullException:
        ///     output 为 null。</summary>
        /// <param name="obj">要序列化的对象</param>
        /// <param name="output">用于写入 JSON 字符串的 System.Text.StringBuilder 对象</param>
        public static void WebScriptJsonSerializerObject(object obj, StringBuilder output)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.Serialize(obj, output);
        }

        /// <summary>
        /// JSON反序列化
        /// </summary>
        /// <typeparam name="T">反序列化之类的类型</typeparam>
        /// <param name="json">待反序列化的json字符串</param>
        /// <returns>反序列化之后的对象</returns>
        public static T WebScriptJsonDeserializeObject<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                throw new ArgumentNullException("json");
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Deserialize<T>(json);
        }

        /// <summary>
        /// JSON反序列化
        /// </summary>
        /// <param name="json">待反序列化的json字符串</param>
        /// <param name="targetType">反序列化之类的类型</param>
        /// <returns>反序列化之后的对象</returns>
        public static object WebScriptJsonDeserializeObject(string json, Type targetType)
        {
            if (string.IsNullOrEmpty(json))
            {
                throw new ArgumentNullException("json");
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Deserialize(json, targetType);
        }

        /// <summary>
        /// 将指定的 JSON 字符串转换为对象图
        /// 异常:
        ///   T:System.ArgumentNullException:
        ///     input 为 null。
        /// 
        ///   T:System.ArgumentException:
        ///     input 长度超过了值的 System.Web.Script.Serialization.JavaScriptSerializer.MaxJsonLength。-
        ///     或 - 定义的递归限制 System.Web.Script.Serialization.JavaScriptSerializer.RecursionLimit
        ///     超出了。- 或 - input 包含意外的字符序列。- 或 - input 是一种词典类型，但遇到了非字符串的键值。- 或 - input 包含在目标类型不可用的成员定义。
        /// 
        ///   T:System.InvalidOperationException:
        ///     input 包含表示自定义的类型，但当前与序列化程序关联的类型解析器找不到相应的托管的类型的"__type"属性。- 或 - input 包含一个"__type"属性，指示是自定义的类型，而对应的
        ///     JSON 字符串反序列化的结果不能分配给预期的目标类型。- 或 - input 包含一个"__type"属性，指示是 System.Object 或非可实例化的类型
        ///     （例如，抽象类型或接口）。- 或 - 尝试将一个 JSON 数组转换为不支持以用作 JSON 反序列化目标的类似数组的托管类型。- 或 - 不能转换 input
        ///     为目标类型。
        /// </summary>
        /// <param name="json">要进行反序列化的 JSON 字符串</param>
        /// <returns>反序列化的对象</returns>
        public static object WebScriptJsonDeserializeObject(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                throw new ArgumentNullException("json");
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.DeserializeObject(json);
        }

        /// <summary>
        /// 将指定的 JSON 字符串转换为对象图
        /// 异常:
        ///   T:System.ArgumentNullException:
        ///     input 为 null。
        /// 
        ///   T:System.ArgumentException:
        ///     input 长度超过了值的 System.Web.Script.Serialization.JavaScriptSerializer.MaxJsonLength。-
        ///     或 - 定义的递归限制 System.Web.Script.Serialization.JavaScriptSerializer.RecursionLimit
        ///     超出了。- 或 - input 包含意外的字符序列。- 或 - input 是一种词典类型，但遇到了非字符串的键值。- 或 - input 包含在目标类型不可用的成员定义。
        /// 
        ///   T:System.InvalidOperationException:
        ///     input 包含表示自定义的类型，但当前与序列化程序关联的类型解析器找不到相应的托管的类型的"__type"属性。- 或 - input 包含一个"__type"属性，指示是自定义的类型，而对应的
        ///     JSON 字符串反序列化的结果不能分配给预期的目标类型。- 或 - input 包含一个"__type"属性，指示是 System.Object 或非可实例化的类型
        ///     （例如，抽象类型或接口）。- 或 - 尝试将一个 JSON 数组转换为不支持以用作 JSON 反序列化目标的类似数组的托管类型。- 或 - 不能转换 input
        ///     为目标类型。
        /// </summary>
        /// <param name="serializer">JavaScriptSerializer</param>
        /// <param name="json">要进行反序列化的 JSON 字符串</param>
        /// <returns>反序列化的对象</returns>
        public static object WebScriptJsonDeserializeObject(JavaScriptSerializer serializer, string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                throw new ArgumentNullException("json");
            }

            return serializer.DeserializeObject(json);
        }
        #endregion
        #endregion
    }
}
