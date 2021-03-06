// **********************************************************************
//
// Copyright (c) 2003-2016 ZeroC, Inc. All rights reserved.
//
// This copy of Ice is licensed to you under the terms described in the
// ICE_LICENSE file included in this distribution.
//
// **********************************************************************
//
// Ice version 3.6.3
//
// <auto-generated>
//
// Generated from file `IDevOpsNet.ice'
//
// Warning: do not edit this file.
//
// </auto-generated>
//


using _System = global::System;
using _Microsoft = global::Microsoft;

#pragma warning disable 1591

namespace IceCompactId
{
}

namespace UtilZ
{
    namespace Dotnet
    {
        namespace SHPDevOpsNet
        {
            [_System.Runtime.InteropServices.ComVisible(false)]
            [_System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704")]
            [_System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707")]
            [_System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709")]
            [_System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710")]
            [_System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711")]
            [_System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1715")]
            [_System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716")]
            [_System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720")]
            [_System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1722")]
            [_System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1724")]
            public partial interface IDevOpsControl : Ice.Object, IDevOpsControlOperations_, IDevOpsControlOperationsNC_
            {
            }
        }
    }
}

namespace UtilZ
{
    namespace Dotnet
    {
        namespace SHPDevOpsNet
        {
            [_System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.6.3")]
            public delegate void Callback_IDevOpsControl_SendCommand(string ret__);
        }
    }
}

namespace UtilZ
{
    namespace Dotnet
    {
        namespace SHPDevOpsNet
        {
            [_System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.6.3")]
            public interface IDevOpsControlPrx : Ice.ObjectPrx
            {
                /// <summary>
                /// 下达命令
                /// </summary>
                
                string SendCommand(string cmdStr);

                /// <summary>
                /// 下达命令
                /// </summary>
                /// <param name="ctx__">The Context map to send with the invocation.</param>
                
                string SendCommand(string cmdStr, _System.Collections.Generic.Dictionary<string, string> ctx__);

                /// <summary>
                /// 下达命令
                /// </summary>
                /// <returns>An asynchronous result object.</returns>
                Ice.AsyncResult<UtilZ.Dotnet.SHPDevOpsNet.Callback_IDevOpsControl_SendCommand> begin_SendCommand(string cmdStr);

                /// <summary>
                /// 下达命令
                /// </summary>
                /// <param name="ctx__">The Context map to send with the invocation.</param>
                /// <returns>An asynchronous result object.</returns>
                Ice.AsyncResult<UtilZ.Dotnet.SHPDevOpsNet.Callback_IDevOpsControl_SendCommand> begin_SendCommand(string cmdStr, _System.Collections.Generic.Dictionary<string, string> ctx__);

                /// <summary>
                /// 下达命令
                /// </summary>
                /// <param name="cb__">Asynchronous callback invoked when the operation completes.</param>
                /// <param name="cookie__">Application data to store in the asynchronous result object.</param>
                /// <returns>An asynchronous result object.</returns>
                Ice.AsyncResult begin_SendCommand(string cmdStr, Ice.AsyncCallback cb__, object cookie__);

                /// <summary>
                /// 下达命令
                /// </summary>
                /// <param name="ctx__">The Context map to send with the invocation.</param>
                /// <param name="cb__">Asynchronous callback invoked when the operation completes.</param>
                /// <param name="cookie__">Application data to store in the asynchronous result object.</param>
                /// <returns>An asynchronous result object.</returns>
                Ice.AsyncResult begin_SendCommand(string cmdStr, _System.Collections.Generic.Dictionary<string, string> ctx__, Ice.AsyncCallback cb__, object cookie__);

                /// <summary>
                /// 下达命令
                /// </summary>
                /// <param name="r__">The asynchronous result object for the invocation.</param>
                string end_SendCommand(Ice.AsyncResult r__);
            }
        }
    }
}

namespace UtilZ
{
    namespace Dotnet
    {
        namespace SHPDevOpsNet
        {
            [_System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.6.3")]
            public interface IDevOpsControlOperations_
            {
                /// <summary>
                /// 下达命令
                /// </summary>
                /// <param name="cb__">The callback object for the operation.</param>
                /// <param name="current__">The Current object for the invocation.</param>
                void SendCommand_async(UtilZ.Dotnet.SHPDevOpsNet.AMD_IDevOpsControl_SendCommand cb__, string cmdStr, Ice.Current current__);
            }

            [_System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.6.3")]
            public interface IDevOpsControlOperationsNC_
            {
                /// <summary>
                /// 下达命令
                /// </summary>
                /// <param name="cb__">The callback object for the operation.</param>
                void SendCommand_async(UtilZ.Dotnet.SHPDevOpsNet.AMD_IDevOpsControl_SendCommand cb__, string cmdStr);
            }
        }
    }
}

namespace UtilZ
{
    namespace Dotnet
    {
        namespace SHPDevOpsNet
        {
            [_System.Runtime.InteropServices.ComVisible(false)]
            [_System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.6.3")]
            public sealed class IDevOpsControlPrxHelper : Ice.ObjectPrxHelperBase, IDevOpsControlPrx
            {
                #region Synchronous operations

                public string SendCommand(string cmdStr)
                {
                    return this.SendCommand(cmdStr, null, false);
                }

                public string SendCommand(string cmdStr, _System.Collections.Generic.Dictionary<string, string> ctx__)
                {
                    return this.SendCommand(cmdStr, ctx__, true);
                }

                private string SendCommand(string cmdStr, _System.Collections.Generic.Dictionary<string, string> context__, bool explicitCtx__)
                {
                    checkTwowayOnly__(__SendCommand_name);
                    return end_SendCommand(begin_SendCommand(cmdStr, context__, explicitCtx__, true, null, null));
                }

                #endregion

                #region Asynchronous operations

                public Ice.AsyncResult<UtilZ.Dotnet.SHPDevOpsNet.Callback_IDevOpsControl_SendCommand> begin_SendCommand(string cmdStr)
                {
                    return begin_SendCommand(cmdStr, null, false, false, null, null);
                }

                public Ice.AsyncResult<UtilZ.Dotnet.SHPDevOpsNet.Callback_IDevOpsControl_SendCommand> begin_SendCommand(string cmdStr, _System.Collections.Generic.Dictionary<string, string> ctx__)
                {
                    return begin_SendCommand(cmdStr, ctx__, true, false, null, null);
                }

                public Ice.AsyncResult begin_SendCommand(string cmdStr, Ice.AsyncCallback cb__, object cookie__)
                {
                    return begin_SendCommand(cmdStr, null, false, false, cb__, cookie__);
                }

                public Ice.AsyncResult begin_SendCommand(string cmdStr, _System.Collections.Generic.Dictionary<string, string> ctx__, Ice.AsyncCallback cb__, object cookie__)
                {
                    return begin_SendCommand(cmdStr, ctx__, true, false, cb__, cookie__);
                }

                private const string __SendCommand_name = "SendCommand";

                public string end_SendCommand(Ice.AsyncResult r__)
                {
                    IceInternal.OutgoingAsync outAsync__ = IceInternal.OutgoingAsync.check(r__, this, __SendCommand_name);
                    try
                    {
                        if(!outAsync__.wait())
                        {
                            try
                            {
                                outAsync__.throwUserException();
                            }
                            catch(Ice.UserException ex__)
                            {
                                throw new Ice.UnknownUserException(ex__.ice_name(), ex__);
                            }
                        }
                        string ret__;
                        IceInternal.BasicStream is__ = outAsync__.startReadParams();
                        ret__ = is__.readString();
                        outAsync__.endReadParams();
                        return ret__;
                    }
                    finally
                    {
                        outAsync__.cacheMessageBuffers();
                    }
                }

                private Ice.AsyncResult<UtilZ.Dotnet.SHPDevOpsNet.Callback_IDevOpsControl_SendCommand> begin_SendCommand(string cmdStr, _System.Collections.Generic.Dictionary<string, string> ctx__, bool explicitContext__, bool synchronous__, Ice.AsyncCallback cb__, object cookie__)
                {
                    checkAsyncTwowayOnly__(__SendCommand_name);
                    IceInternal.TwowayOutgoingAsync<UtilZ.Dotnet.SHPDevOpsNet.Callback_IDevOpsControl_SendCommand> result__ =  getTwowayOutgoingAsync<UtilZ.Dotnet.SHPDevOpsNet.Callback_IDevOpsControl_SendCommand>(__SendCommand_name, SendCommand_completed__, cookie__);
                    if(cb__ != null)
                    {
                        result__.whenCompletedWithAsyncCallback(cb__);
                    }
                    try
                    {
                        result__.prepare(__SendCommand_name, Ice.OperationMode.Normal, ctx__, explicitContext__, synchronous__);
                        IceInternal.BasicStream os__ = result__.startWriteParams(Ice.FormatType.DefaultFormat);
                        os__.writeString(cmdStr);
                        result__.endWriteParams();
                        result__.invoke();
                    }
                    catch(Ice.Exception ex__)
                    {
                        result__.abort(ex__);
                    }
                    return result__;
                }

                private void SendCommand_completed__(Ice.AsyncResult r__, UtilZ.Dotnet.SHPDevOpsNet.Callback_IDevOpsControl_SendCommand cb__, Ice.ExceptionCallback excb__)
                {
                    string ret__;
                    try
                    {
                        ret__ = end_SendCommand(r__);
                    }
                    catch(Ice.Exception ex__)
                    {
                        if(excb__ != null)
                        {
                            excb__(ex__);
                        }
                        return;
                    }
                    if(cb__ != null)
                    {
                        cb__(ret__);
                    }
                }

                #endregion

                #region Checked and unchecked cast operations

                public static IDevOpsControlPrx checkedCast(Ice.ObjectPrx b)
                {
                    if(b == null)
                    {
                        return null;
                    }
                    IDevOpsControlPrx r = b as IDevOpsControlPrx;
                    if((r == null) && b.ice_isA(ice_staticId()))
                    {
                        IDevOpsControlPrxHelper h = new IDevOpsControlPrxHelper();
                        h.copyFrom__(b);
                        r = h;
                    }
                    return r;
                }

                public static IDevOpsControlPrx checkedCast(Ice.ObjectPrx b, _System.Collections.Generic.Dictionary<string, string> ctx)
                {
                    if(b == null)
                    {
                        return null;
                    }
                    IDevOpsControlPrx r = b as IDevOpsControlPrx;
                    if((r == null) && b.ice_isA(ice_staticId(), ctx))
                    {
                        IDevOpsControlPrxHelper h = new IDevOpsControlPrxHelper();
                        h.copyFrom__(b);
                        r = h;
                    }
                    return r;
                }

                public static IDevOpsControlPrx checkedCast(Ice.ObjectPrx b, string f)
                {
                    if(b == null)
                    {
                        return null;
                    }
                    Ice.ObjectPrx bb = b.ice_facet(f);
                    try
                    {
                        if(bb.ice_isA(ice_staticId()))
                        {
                            IDevOpsControlPrxHelper h = new IDevOpsControlPrxHelper();
                            h.copyFrom__(bb);
                            return h;
                        }
                    }
                    catch(Ice.FacetNotExistException)
                    {
                    }
                    return null;
                }

                public static IDevOpsControlPrx checkedCast(Ice.ObjectPrx b, string f, _System.Collections.Generic.Dictionary<string, string> ctx)
                {
                    if(b == null)
                    {
                        return null;
                    }
                    Ice.ObjectPrx bb = b.ice_facet(f);
                    try
                    {
                        if(bb.ice_isA(ice_staticId(), ctx))
                        {
                            IDevOpsControlPrxHelper h = new IDevOpsControlPrxHelper();
                            h.copyFrom__(bb);
                            return h;
                        }
                    }
                    catch(Ice.FacetNotExistException)
                    {
                    }
                    return null;
                }

                public static IDevOpsControlPrx uncheckedCast(Ice.ObjectPrx b)
                {
                    if(b == null)
                    {
                        return null;
                    }
                    IDevOpsControlPrx r = b as IDevOpsControlPrx;
                    if(r == null)
                    {
                        IDevOpsControlPrxHelper h = new IDevOpsControlPrxHelper();
                        h.copyFrom__(b);
                        r = h;
                    }
                    return r;
                }

                public static IDevOpsControlPrx uncheckedCast(Ice.ObjectPrx b, string f)
                {
                    if(b == null)
                    {
                        return null;
                    }
                    Ice.ObjectPrx bb = b.ice_facet(f);
                    IDevOpsControlPrxHelper h = new IDevOpsControlPrxHelper();
                    h.copyFrom__(bb);
                    return h;
                }

                public static readonly string[] ids__ =
                {
                    "::Ice::Object",
                    "::UtilZ::Dotnet::SHPDevOpsNet::IDevOpsControl"
                };

                public static string ice_staticId()
                {
                    return ids__[1];
                }

                #endregion

                #region Marshaling support

                public static void write__(IceInternal.BasicStream os__, IDevOpsControlPrx v__)
                {
                    os__.writeProxy(v__);
                }

                public static IDevOpsControlPrx read__(IceInternal.BasicStream is__)
                {
                    Ice.ObjectPrx proxy = is__.readProxy();
                    if(proxy != null)
                    {
                        IDevOpsControlPrxHelper result = new IDevOpsControlPrxHelper();
                        result.copyFrom__(proxy);
                        return result;
                    }
                    return null;
                }

                #endregion
            }
        }
    }
}

namespace UtilZ
{
    namespace Dotnet
    {
        namespace SHPDevOpsNet
        {
            [_System.Runtime.InteropServices.ComVisible(false)]
            [_System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.6.3")]
            public abstract class IDevOpsControlDisp_ : Ice.ObjectImpl, IDevOpsControl
            {
                #region Slice operations

                public void SendCommand_async(UtilZ.Dotnet.SHPDevOpsNet.AMD_IDevOpsControl_SendCommand cb__, string cmdStr)
                {
                    SendCommand_async(cb__, cmdStr, Ice.ObjectImpl.defaultCurrent);
                }

                public abstract void SendCommand_async(UtilZ.Dotnet.SHPDevOpsNet.AMD_IDevOpsControl_SendCommand cb__, string cmdStr, Ice.Current current__);

                #endregion

                #region Slice type-related members

                public static new readonly string[] ids__ = 
                {
                    "::Ice::Object",
                    "::UtilZ::Dotnet::SHPDevOpsNet::IDevOpsControl"
                };

                public override bool ice_isA(string s)
                {
                    return _System.Array.BinarySearch(ids__, s, IceUtilInternal.StringUtil.OrdinalStringComparer) >= 0;
                }

                public override bool ice_isA(string s, Ice.Current current__)
                {
                    return _System.Array.BinarySearch(ids__, s, IceUtilInternal.StringUtil.OrdinalStringComparer) >= 0;
                }

                public override string[] ice_ids()
                {
                    return ids__;
                }

                public override string[] ice_ids(Ice.Current current__)
                {
                    return ids__;
                }

                public override string ice_id()
                {
                    return ids__[1];
                }

                public override string ice_id(Ice.Current current__)
                {
                    return ids__[1];
                }

                public static new string ice_staticId()
                {
                    return ids__[1];
                }

                #endregion

                #region Operation dispatch

                [_System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011")]
                public static Ice.DispatchStatus SendCommand___(IDevOpsControl obj__, IceInternal.Incoming inS__, Ice.Current current__)
                {
                    Ice.ObjectImpl.checkMode__(Ice.OperationMode.Normal, current__.mode);
                    IceInternal.BasicStream is__ = inS__.startReadParams();
                    string cmdStr;
                    cmdStr = is__.readString();
                    inS__.endReadParams();
                    AMD_IDevOpsControl_SendCommand cb__ = new _AMD_IDevOpsControl_SendCommand(inS__);
                    try
                    {
                        obj__.SendCommand_async(cb__, cmdStr, current__);
                    }
                    catch(_System.Exception ex__)
                    {
                        cb__.ice_exception(ex__);
                    }
                    return Ice.DispatchStatus.DispatchAsync;
                }

                private static string[] all__ =
                {
                    "SendCommand",
                    "ice_id",
                    "ice_ids",
                    "ice_isA",
                    "ice_ping"
                };

                public override Ice.DispatchStatus dispatch__(IceInternal.Incoming inS__, Ice.Current current__)
                {
                    int pos = _System.Array.BinarySearch(all__, current__.operation, IceUtilInternal.StringUtil.OrdinalStringComparer);
                    if(pos < 0)
                    {
                        throw new Ice.OperationNotExistException(current__.id, current__.facet, current__.operation);
                    }

                    switch(pos)
                    {
                        case 0:
                        {
                            return SendCommand___(this, inS__, current__);
                        }
                        case 1:
                        {
                            return Ice.ObjectImpl.ice_id___(this, inS__, current__);
                        }
                        case 2:
                        {
                            return Ice.ObjectImpl.ice_ids___(this, inS__, current__);
                        }
                        case 3:
                        {
                            return Ice.ObjectImpl.ice_isA___(this, inS__, current__);
                        }
                        case 4:
                        {
                            return Ice.ObjectImpl.ice_ping___(this, inS__, current__);
                        }
                    }

                    _System.Diagnostics.Debug.Assert(false);
                    throw new Ice.OperationNotExistException(current__.id, current__.facet, current__.operation);
                }

                #endregion

                #region Marshaling support

                protected override void writeImpl__(IceInternal.BasicStream os__)
                {
                    os__.startWriteSlice(ice_staticId(), -1, true);
                    os__.endWriteSlice();
                }

                protected override void readImpl__(IceInternal.BasicStream is__)
                {
                    is__.startReadSlice();
                    is__.endReadSlice();
                }

                #endregion
            }
        }
    }
}

namespace UtilZ
{
    namespace Dotnet
    {
        namespace SHPDevOpsNet
        {
            /// <summary>
            /// 下达命令
            /// </summary>
            [_System.Runtime.InteropServices.ComVisible(false)]
            [_System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.6.3")]
            public interface AMD_IDevOpsControl_SendCommand : Ice.AMDCallback
            {
                /// <summary>
                /// ice_response indicates that
                /// the operation completed successfully.
                /// </summary>
                void ice_response(string ret__);
            }

            [_System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.6.3")]
            class _AMD_IDevOpsControl_SendCommand : IceInternal.IncomingAsync, AMD_IDevOpsControl_SendCommand
            {
                public _AMD_IDevOpsControl_SendCommand(IceInternal.Incoming inc) : base(inc)
                {
                }

                public void ice_response(string ret__)
                {
                    if(validateResponse__(true))
                    {
                        try
                        {
                            IceInternal.BasicStream os__ = startWriteParams__(Ice.FormatType.DefaultFormat);
                            os__.writeString(ret__);
                            endWriteParams__(true);
                        }
                        catch(Ice.LocalException ex__)
                        {
                            exception__(ex__);
                            return;
                        }
                        response__();
                    }
                }
            }
        }
    }
}
