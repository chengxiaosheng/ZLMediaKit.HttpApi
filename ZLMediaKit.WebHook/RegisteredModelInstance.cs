using System;
using System.Collections.Generic;
using ZLMediaKit.Common.Dtos;
using ZLMediaKit.Common.Dtos.HookResultDto;
using ZLMediaKit.Common;

namespace ZLMediaKit.WebHook
{
    /// <summary>
    /// 
    /// </summary>
    public static class RegisteredModelInstance
    {


        internal static Dictionary<Type, object> ResultModelMapping = new Dictionary<Type, object>()
        {
            { typeof(IHookReportFlowResult), new HookReportFlowResult() {  Code = 0 } },
            { typeof(IHookHttpAccessResult), new HookHttpAccessResult() },
            { typeof(IHookPlayResult), new HookPlayResult() },
            { typeof(IHookPublishResult), new HookPublishResult() },
            { typeof(IHookRecordMp4Result), new HookRecordMp4Result() },
            { typeof(IHookRecordTsResult), new HookRecordTsResult() },
            { typeof(IHookRtspRealmResult), new HookRtspRealmResult() },
            { typeof(IHookRtspAuthResult), new HookRtspAuthResult() },
            { typeof(IHookShellLoginResult), new HookShellLoginResult () {  Code = -1,  Message = "禁止登录" } },
            { typeof(IHookStreamChangedResult), new HookStreamChangedResult () },
            { typeof(IHookStreamNonReaderResult), new HookStreamNonReaderResult () },
            { typeof(IHookStreamNotFoundResult), new HookStreamNotFoundResult () },
            { typeof(IHookServerStartedResult), new HookServerStartedResult () },
            { typeof(IHookServerKeepaliveResult), new HookServerKeepaliveResult () },
            { typeof(IHookCommonResult), new HookCommonResult() {  Code = 0 } },
        };

        /// <summary>
        /// 一切为了序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        public static void RegisterModelMapping<T, T1>() where T : IHookBase where T1 : T, new()
        {
            TypeMapping.TypeMappings[typeof(T)] = typeof(T1);
        }

        /// <summary>
        /// 替换返回结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        public static void RegisterResultModelMapping<T, T1>(T1 defaultValue) where T : IResultBase where T1 : T, new()
        {
            ResultModelMapping[typeof(T)] = defaultValue;
        }

        internal static T GetResultModelDefault<T>() where T : IResultBase
        {
            ResultModelMapping.TryGetValue(typeof(T), out var value);
            return (T)value;
        }
    }
}
