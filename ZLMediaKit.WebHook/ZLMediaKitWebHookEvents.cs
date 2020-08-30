using System;
using ZLMediaKit.WebHook.Dtos;

namespace ZLMediaKit.WebHook
{
    public class ZLMediaKitWebHookEvents
    {
        /// <summary>
        /// 流量统计事件,播放器或推流器断开时并且耗用流量超过特定阈值时会触发此事件，阈值通过配置文件
        /// </summary>
        /// <remarks> 详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </remarks>
        public static Action<FlowReport> OnFlowReport { get; set; }

        /// <summary>
        /// 访问http文件服务器上hls之外的文件时触发。
        /// </summary>
        /// <remarks> 详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </remarks>
        public static Func<HttpAccess,HttpAccessResult> OnHttpAccess { get; set; }

        /// <summary>
        /// 播放器鉴权事件,rtsp/rtmp/http-flv/ws-flv/hls的播放都将触发此鉴权事件
        /// </summary>
        /// <remarks> 如果流不存在，那么先触发on_play事件然后触发on_stream_not_found事件;
        /// 播放rtsp流时，如果该流启动了rtsp专属鉴权(on_rtsp_realm)那么将不再触发on_play事件。
        /// <para>详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </para>
        /// </remarks>
        public static Func<PlayInfo,PlayInfoResult> OnPlay { get; set; }

        /// <summary>
        /// rtsp/rtmp/rtp推流鉴权事件
        /// </summary>
        /// <remarks> 详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </remarks>
        public static Func<PublishInfo,PublishResult> OnPublish { get; set; }

        /// <summary>
        /// 录制mp4完成后通知事件
        /// </summary>
        /// <remarks> 详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </remarks>
        public static Action<RecordInfo> OnRecordMP4 { get; set; }

        /// <summary>
        /// 该rtsp流是否开启rtsp专用方式的鉴权事件，开启后才会触发on_rtsp_auth事件。
        /// <para>需要指出的是rtsp也支持url参数鉴权，它支持两种方式鉴权。</para>
        /// </summary>
        /// <remarks> 详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </remarks>
        public static Func<RtspRealmInfo,RtspRealmInfoResult> OnRtspRealm { get; set; }

        /// <summary>
        /// rtsp专用的鉴权事件，先触发on_rtsp_realm事件然后才会触发on_rtsp_auth事件。
        /// </summary>
        /// <remarks> 详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </remarks>
        public static Func<RtspAuthInfo,RtspAuthResult> OnRtspAuth { get; set; }

        /// <summary>
        /// shell登录鉴权，ZLMediaKit提供简单的telnet调试方式
        /// 使用telnet 127.0.0.1 9000能进入MediaServer进程的shell界面
        /// </summary>
        /// <remarks> 详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </remarks>
        public static Func<ShellLoginInfo,ShellLonginResult> OnShellLogin { get; set; }

        /// <summary>
        /// rtsp/rtmp流注册或注销时触发此事件
        /// </summary>
        /// <remarks> 详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </remarks>
        public static Action<StreamChangedInfo> OnStreamChanged { get; set; }

        /// <summary>
        /// 流无人观看时事件，用户可以通过此事件选择是否关闭无人看的流。
        /// </summary>
        /// <remarks> 详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </remarks>
        public static Func<StreamNoneReaderInfo,StreamNoneReaderInfoResult> OnStreamNoneReader { get; set; }

        /// <summary>
        /// 流未找到事件，用户可以在此事件触发时，立即去拉流，这样可以实现按需拉流
        /// </summary>
        /// <remarks> 详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </remarks>
        public static Action<StreamNotFoundInfo> OnStreamNotFound { get; set; }

        /// <summary>
        /// 服务器启动事件，可以用于监听服务器崩溃重启
        /// </summary>
        public static Action<ServerConfig> OnServerStarted { get; set; }
    }
}
