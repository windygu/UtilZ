using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.Base.LocalMessageCenter.LocalMeseageReqRes
{
    /// <summary>
    /// 本地消息请求响应中心
    /// </summary>
    public class LMReqResCenter
    {
        /// <summary>
        /// 响应字典集合
        /// </summary>
        private readonly static ConcurrentDictionary<int, LMRes> _dicRes = new ConcurrentDictionary<int, LMRes>();

        /// <summary>
        /// 线程锁
        /// </summary>
        private readonly static object _lock = new object();

        /// <summary>
        /// 注册响应
        /// </summary>
        /// <param name="res">要注册的响应</param>
        public static void RegisteRes(LMRes res)
        {
            if (res == null)
            {
                return;
            }

            int id = res.ID;
            lock (_lock)
            {
                if (_dicRes.ContainsKey(id))
                {
                    throw new ArgumentException(string.Format("已注册ID为{0}的响应", id));
                }

                _dicRes.TryAdd(id, res);
            }
        }

        /// <summary>
        /// 取消响应注册
        /// </summary>
        /// <param name="res">已注册的响应</param>
        public static void UnRegisteRes(LMRes res)
        {
            if (res == null)
            {
                return;
            }

            int id = res.ID;
            lock (_lock)
            {
                if (_dicRes.ContainsKey(id))
                {
                    _dicRes.TryRemove(id, out res);
                }
            }
        }

        /// <summary>
        /// 清空响应
        /// </summary>
        public static void ClearRes()
        {
            lock (_lock)
            {
                _dicRes.Clear();
            }
        }

        /// <summary>
        /// 本地消息请求
        /// </summary>
        /// <param name="req">请求对象</param>
        public static void Req(LMReq req)
        {
            LMRes res;
            if (_dicRes.TryGetValue(req.ID, out res))
            {
                var handler = res.Res;
                if (handler != null)
                {
                    req.ResResult = handler(req.ReqPara);
                }
            }
        }
    }
}
