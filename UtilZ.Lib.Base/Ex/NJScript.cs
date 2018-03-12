using Microsoft.JScript.Vsa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.Base.Ex
{
    /// <summary>
    /// JScript扩展方法类
    /// </summary>
    public class NJScript
    {
        /// <summary>
        /// 执行JS,只返回最后一条执行的语句结果
        /// </summary>
        /// <param name="jscript">要执行的JS</param>
        /// <returns>执行结果值</returns>
        public static object Eval(string jscript)
        {
            try
            {
                return Microsoft.JScript.Eval.JScriptEvaluate(jscript, VsaEngine.CreateEngine());
            }
            catch (Exception ex)
            {
                throw new Exception("执行JS异常", ex);
            }
        }

        private static void Demo()
        {
            //只返回最后一条执行的语句结果,与直接调用C#方法相比,此方式要慢几百倍
            var ret00 = NJScript.Eval("var obj = {};obj.name='jimmy';obj.sex='Male';");

            var ret0 = NJScript.Eval("function f(){ var obj = {};obj.name='jimmy';obj.sex='Male';return obj};f();");

            var ret = NJScript.Eval(@"12+23");

            string jsFStr = @"function f1(lon,lat){
lon=lon*3-10;
lat=lat*2-20;
return {'lon':lon,'lat':lat}
}";
            int lon = 20;
            int lat = 30;
            string jsStr = jsFStr + string.Format(@";f1({0},{1})", lon, lat);
            var ret2 = NJScript.Eval(jsStr);

            Microsoft.JScript.JSObject obj = (Microsoft.JScript.JSObject)ret2;
            var ts = obj["lon"].ToString();
            var dv = obj["lat"].ToString();
        }
    }
}
