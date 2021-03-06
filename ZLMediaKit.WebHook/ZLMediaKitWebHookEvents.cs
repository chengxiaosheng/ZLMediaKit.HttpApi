﻿using System;
using ZLMediaKit.WebHook.Dtos;

namespace ZLMediaKit.WebHook
{
    public class ZLMediaKitWebHookEvents
    {
        /// <summary>
        /// 流量统计事件,播放器或推流器断开时并且耗用流量超过特定阈值时会触发此事件，阈值通过配置文件
        /// </summary>
        /// <remarks> 详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </remarks>
        public static event Action<FlowReport> OnFlowReport;
        internal static bool OnFlowReport_IsNull => OnFlowReport == null;
        internal static void OnFlowReport_Call(FlowReport flowReport) => OnFlowReport?.Invoke(flowReport);

        /// <summary>
        /// 访问http文件服务器上hls之外的文件时触发。
        /// </summary>
        /// <remarks> 详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </remarks>
        public static event Func<HttpAccess, HttpAccessResult> OnHttpAccess;
        internal static bool OnHttpAccess_IsNull => OnHttpAccess == null;
        internal static HttpAccessResult OnHttpAccess_Call(HttpAccess httpAccess) => OnHttpAccess?.Invoke(httpAccess);

        /// <summary>
        /// 播放器鉴权事件,rtsp/rtmp/http-flv/ws-flv/hls的播放都将触发此鉴权事件
        /// </summary>
        /// <remarks> 如果流不存在，那么先触发on_play事件然后触发on_stream_not_found事件;
        /// 播放rtsp流时，如果该流启动了rtsp专属鉴权(on_rtsp_realm)那么将不再触发on_play事件。
        /// <para>详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </para>
        /// </remarks>
        public static event Func<PlayInfo, PlayInfoResult> OnPlay;
        internal static bool OnPlay_IsNull => OnPlay == null;
        internal static PlayInfoResult OnPlay_Call(PlayInfo playInfo) => OnPlay?.Invoke(playInfo);

        /// <summary>
        /// rtsp/rtmp/rtp推流鉴权事件
        /// </summary>
        /// <remarks> 详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </remarks>
        public static event Func<PublishInfo, PublishResult> OnPublish;
        internal static bool OnPublish_IsNull => OnPublish == null;
        internal static PublishResult OnPublish_Call(PublishInfo publishInfo) => OnPublish?.Invoke(publishInfo);

        /// <summary>
        /// 录制mp4完成后通知事件
        /// </summary>
        /// <remarks> 详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </remarks>
        public static event Action<RecordInfo> OnRecordMP4;
        internal static bool OnRecordMP4_IsNull => OnRecordMP4 == null;
        internal static void OnRecordMP4_Call(RecordInfo recordInfo) => OnRecordMP4?.Invoke(recordInfo);

        /// <summary>
        /// 录制TS完成后通知事件
        /// </summary>
        /// <remarks> 详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </remarks>
        public static event Action<RecordInfo> OnRecordTS;
        internal static bool OnRecordTS_IsNull => OnRecordTS == null;
        internal static void OnRecordTS_Call(RecordInfo recordInfo) => OnRecordTS?.Invoke(recordInfo);


        /// <summary>
        /// 该rtsp流是否开启rtsp专用方式的鉴权事件，开启后才会触发on_rtsp_auth事件。
        /// <para>需要指出的是rtsp也支持url参数鉴权，它支持两种方式鉴权。</para>
        /// </summary>
        /// <remarks> 详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </remarks>
        public static event Func<RtspRealmInfo, RtspRealmInfoResult> OnRtspRealm;
        internal static bool OnRtspRealm_IsNull => OnRtspRealm == null;
        internal static RtspRealmInfoResult OnRtspRealm_Call(RtspRealmInfo rtspRealmInfo) => OnRtspRealm?.Invoke(rtspRealmInfo);

        /// <summary>
        /// rtsp专用的鉴权事件，先触发on_rtsp_realm事件然后才会触发on_rtsp_auth事件。
        /// </summary>
        /// <remarks> 详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </remarks>
        public static event Func<RtspAuthInfo, RtspAuthResult> OnRtspAuth;
        internal static bool OnRtspAuth_IsNull => OnRtspAuth == null;
        internal static RtspAuthResult OnRtspAuth_Call(RtspAuthInfo rtspAuthInfo) => OnRtspAuth?.Invoke(rtspAuthInfo);

        /// <summary>
        /// shell登录鉴权，ZLMediaKit提供简单的telnet调试方式
        /// 使用telnet 127.0.0.1 9000能进入MediaServer进程的shell界面
        /// </summary>
        /// <remarks> 详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </remarks>
        public static event Func<ShellLoginInfo, ShellLonginResult> OnShellLogin;
        internal static bool OnShellLogin_IsNull => OnShellLogin == null;
        internal static ShellLonginResult OnShellLogin_Call(ShellLoginInfo shellLoginInfo) => OnShellLogin?.Invoke(shellLoginInfo);

        /// <summary>
        /// rtsp/rtmp流注册或注销时触发此事件
        /// </summary>
        /// <remarks> 详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </remarks>
        public static event Action<StreamChangedInfo> OnStreamChanged;
        internal static bool OnStreamChanged_IsNull => OnStreamChanged == null;
        internal static void OnStreamChanged_Call(StreamChangedInfo streamChangedInfo) => OnStreamChanged?.Invoke(streamChangedInfo);

        /// <summary>
        /// 流无人观看时事件，用户可以通过此事件选择是否关闭无人看的流。
        /// </summary>
        /// <remarks> 详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </remarks>
        public static event Func<StreamNoneReaderInfo, StreamNoneReaderInfoResult> OnStreamNoneReader;
        internal static bool OnStreamNoneReader_IsNull => OnStreamNoneReader == null;
        internal static StreamNoneReaderInfoResult OnStreamNoneReader_Call(StreamNoneReaderInfo streamNoneReaderInfo) => OnStreamNoneReader?.Invoke(streamNoneReaderInfo);

        /// <summary>
        /// 流未找到事件，用户可以在此事件触发时，立即去拉流，这样可以实现按需拉流
        /// </summary>
        /// <remarks> 详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </remarks>
        public static event Action<StreamNotFoundInfo> OnStreamNotFound;
        internal static bool OnStreamNotFound_IsNull => OnStreamNotFound == null;
        internal static void OnStreamNotFound_Call(StreamNotFoundInfo streamNotFoundInfo) => OnStreamNotFound?.Invoke(streamNotFoundInfo);

        /// <summary>
        /// 服务器启动事件，可以用于监听服务器崩溃重启
        /// </summary>
        public static event Action<ServerConfig> OnServerStarted;
        internal static bool OnServerStarted_IsNull => OnServerStarted == null;
        internal static void OnServerStarted_Call(ServerConfig serverConfig) => OnServerStarted?.Invoke(serverConfig);
    }
}
