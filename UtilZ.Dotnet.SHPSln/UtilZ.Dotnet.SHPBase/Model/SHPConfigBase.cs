using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.Log;

namespace UtilZ.Dotnet.SHPBase.Model
{
    [Serializable]
    public class SHPConfigBase
    {
        public int AgentPort { get; set; } = 15001;
        public int DevOpsPort { get; set; } = 15002;

        public SHPConfigBase()
        {

        }

        protected static SHPConfigBase _configBase = null;
        public static void Load<T>(string filePath) where T : SHPConfigBase, new()
        {
            T config;
            if (File.Exists(filePath))
            {
                try
                {
                    config = SerializeEx.XmlDeserializerFromFile<T>(filePath);
                }
                catch (Exception ex)
                {
                    Loger.Error(ex);
                    config = new T();
                    SerializeEx.XmlSerializer(config, filePath);
                }
            }
            else
            {
                config = new T();
                SerializeEx.XmlSerializer(config, filePath);
            }

            _configBase = config;
        }
    }
}
