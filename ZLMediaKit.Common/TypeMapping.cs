using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using ZLMediaKit.Common.Dtos;
using ZLMediaKit.Common.Dtos.ApiInputDto;
using ZLMediaKit.Common.Dtos.HookInputDto;

namespace ZLMediaKit.Common
{
    /// <summary>
    /// 全局统一的类型映射 
    /// </summary>
    public static class TypeMapping
    {
        static TypeMapping()
        {
            // HookInput
            SerializerOptions.Converters.Add(new ZLBoolConverter());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IHttpAccessInput, HttpAccessInput>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IPlayInput, PlayInput>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IPublishInput, PublishInpu>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IRecordMp4Input, RecordMp4Input>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IRecordTsInput, RecordTsInput>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IReportFlowInput, ReportFlowInput>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IRtspAuthInput, RtspAuthInput>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IRtspRealmInput, RtspRealmInput>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IServerKeepaliveInput, ServerKeepaliveInput>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IShellLoginInput, ShellLoginInput>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IServerStartedInput, ServerStartedInput>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IStreamChangedInput, StreamChangedInput>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IStreamNoneReaderInput, StreamNoneReaderInput>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IStreamNotFoundInuut, StreamNotFoundInuut>());

            SerializerOptions.Converters.Add(new TypeMappingConvert<IEvnetBase, EvnetBase>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IHookBase, HookBase>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IKeepalive, Keepalive>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IMediaBase, MediaBase>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IMediaInfo, MediaInfo>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IResultBase, ResultBase>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<ISocketInfo, SocketInfo>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IVideoMediaTrack, VideoMediaTrack>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IAudioMediaTrack, AudioMediaTrack>());
            // ServerConfig 
            SerializerOptions.Converters.Add(new TypeMappingConvert<IServerConfig, ServerConfig>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IApiConfig, ApiConfig>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IFFmpegConfig, FFmpegConfig>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IGeneralConfig, GeneralConfig>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IHlsConfig, HlsConfig>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IHookConfig, HookConfig>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IHttpConfig, HttpConfig>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IMulticastConfig, MulticastConfig>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IRecordConfig, RecordConfig>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IRtmpConfig, RtmpConfig>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IRtpConfig, RtpConfig>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IRtpProxyConfig, RtpProxyConfig>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IRtcConfig, RtcConfig>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IRtspConfig, RtspConfig>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IShellConfig, ShellConfig>());

            // Api

            SerializerOptions.Converters.Add(new TypeMappingConvert<IApiResultBase, ApiResultBase>());
            //SerializerOptions.Converters.Add(new TypeMappingConvert<IApiResultDictBase<Dictionary<string,string>>, ApiResultDictBase<Dictionary<string, string>>>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IApiAddFFmpegSourceResult, ApiAddFFmpegSourceResult>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IApiAddStreamPorxyResult, ApiAddStreamPorxyResult>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IApiAddStreamPusherProxyResult, ApiAddStreamPusherProxyResult>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IApiAddStreamPusherProxyResultItem, ApiAddStreamPusherProxyResultItem>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IApiClonseStreamsResult, ApiClonseStreamsResult>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IApiCloseRtpServerResult, ApiCloseRtpServerResult>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IApiCloseStreamResult, ApiCloseStreamResult>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IApiDelFFmpegSourceResult, ApiDelFFmpegSourceResult>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IApiDelFFmpegSourceResultItem, ApiDelFFmpegSourceResultItem>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IApiDelStreamProxyResult, ApiDelStreamProxyResult>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IApiDelStreamProxyResultItem, ApiDelStreamProxyResultItem>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IApiDelStreamPusherProxyResult, ApiDelStreamPusherProxyResult>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IApiDelStreamPusherProxyResultItem, ApiDelStreamPusherProxyResultItem>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IApiGetAllSessionResult, ApiGetAllSessionResult>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IApiGetMediaInfo, ApiGetMediaInfo>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IApiGetMediaListResult, ApiGetMediaListResult>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IApiGetMp4RecordFileResult, ApiGetMp4RecordFileResult>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IApiGetMp4RecordFileResultItem, ApiGetMp4RecordFileResultItem>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IApiGetRtpInfoResult, ApiGetRtpInfoResult>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IApiGetServerConfigResult, ApiGetServerConfigResult>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IApiGetStatisticResult, ApiGetStatisticResult>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IApiIsMediaOnlineResult, ApiIsMediaOnlineResult>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IApiIsRecordingResult, ApiIsRecordingResult>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IApiListRtpServerResult, ApiListRtpServerResult>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IApiListRtpServerResultItem, ApiListRtpServerResultItem>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IApiOpenRtpServerResult, ApiOpenRtpServerResult>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IApiPauseRtpCheckResult, ApiPauseRtpCheckResult>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IApiRestartServerResult, ApiRestartServerResult>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IApiResumeRtpCheckResult, ApiResumeRtpCheckResult>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IApiSeekRecordStampResult, ApiSeekRecordStampResult>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IApiSetRecordSpeedResult, ApiSetRecordSpeedResult>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IApiStartRecordResult, ApiStartRecordResult>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IApiStartSendRtpResult, ApiStartSendRtpResult>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IApiStopRecordResult, ApiStopRecordResult>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IApiStopSendRtpResult, ApiStopSendRtpResult>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IApKillSessionsResult, ApKillSessionsResult>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<ISetServerConfigInput, SetServerConfigInput>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<ISetServerConfigResult, SetServerConfigResult>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IThreadsLoadApiResult, ThreadsLoadApiResult>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IThreadsLoadApiResultItem, ThreadsLoadApiResultItem>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IWorkThreadsLoadApiResult, WorkThreadsLoadApiResult>());
            SerializerOptions.Converters.Add(new TypeMappingConvert<IWorkThreadsLoadApiResultItem, WorkThreadsLoadApiResultItem>());
        }
        /// <summary>
        /// 类型映射，用于序列化数据时采用其他类型进行，以便处理一些特殊化的内容 
        /// </summary>
        /// <typeparam name="Type">接口类型</typeparam>
        /// <typeparam name="Type">对接口的实现类型</typeparam>
        /// <returns></returns>
        public static Dictionary<Type, Type> TypeMappings = new Dictionary<Type, Type>();
        /// <summary>
        /// 全局统一的json 序列化配置 
        /// </summary>
        /// <value></value>
        public static JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping

        };
    }
}
