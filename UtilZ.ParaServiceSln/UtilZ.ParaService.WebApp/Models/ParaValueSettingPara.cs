using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtilZ.ParaService.DBModel;

namespace UtilZ.ParaService.WebApp.Models
{
    public class ParaValueSettingPara
    {
        public long PID { get; set; }

        public List<ParaValueSetting> ParaValueSettings { get; set; } = new List<ParaValueSetting>();

        public ParaValueSettingPara()
        {

        }

        internal List<ParaValue> ToParaValues()
        {
            var paraValues = new List<ParaValue>();
            //var paraValues = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ParaValue>>(value);
            foreach (var paraValueSetting in this.ParaValueSettings)
            {
                paraValues.Add(new ParaValue() { ParaID = paraValueSetting.Id, ProjectID = this.PID, Value = paraValueSetting.Value });
            }

            return paraValues;
        }
    }

    public class ParaValueSetting
    {
        public long Id { get; set; }
        public string Value { get; set; }

        public ParaValueSetting()
        {

        }
    }
}
