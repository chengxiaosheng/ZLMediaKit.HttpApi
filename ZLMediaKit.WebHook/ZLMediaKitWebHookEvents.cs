using System.Threading.Tasks;
using ZLMediaKit.Common.Dtos.EventArgs;
using ZLMediaKit.Common.Dtos.HookInputDto;
using ZLMediaKit.Common.Dtos.HookResultDto;

namespace ZLMediaKit.WebHook
{
    public class ZLMediaKitWebHookEvents
    {

        /// <summary>
        /// 流量统计事件,播放器或推流器断开时并且耗用流量超过特定阈值时会触发此事件，阈值通过配置文件
        /// </summary>
        /// <param name="hookEventArgs"></param>
        public delegate Task FlowReportEventHandle(IHookEventArgs<IReportFlowInput> hookEventArgs);
        /// <summary>
        /// 流量统计事件,播放器或推流器断开时并且耗用流量超过特定阈值时会触发此事件，阈值通过配置文件
        /// </summary>
        /// <remarks> 详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </remarks>
        public static event FlowReportEventHandle OnFlowReport;
        internal static async Task<IHookReportFlowResult> OnFlowReport_Call(IHookEventArgs<IReportFlowInput> hookEventArgs)
        {
            _ = OnFlowReport?.Invoke(hookEventArgs).ConfigureAwait(false);
            return await Task.FromResult(RegisteredModelInstance.GetResultModelDefault<IHookReportFlowResult>());
        }

        /// <summary>
        /// 访问http文件服务器上hls之外的文件时触发
        /// </summary>
        /// <param name="hookEventArgs"></param>
        /// <returns></returns>
        public delegate Task<IHookHttpAccessResult> HttpAccessEventHandle(IHookEventArgs<IHttpAccessInput> hookEventArgs);

        /// <summary>
        /// 访问http文件服务器上hls之外的文件时触发。
        /// </summary>
        /// <remarks> 详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </remarks>
        public static event HttpAccessEventHandle OnHttpAccess;

        internal static bool OnHttpAccess_IsNull => OnHttpAccess == null;
        internal static async Task<IHookHttpAccessResult> OnHttpAccess_Call(IHookEventArgs<IHttpAccessInput> hookEventArgs)
        {
            if (OnHttpAccess == null) return await Task.FromResult(RegisteredModelInstance.GetResultModelDefault<IHookHttpAccessResult>());
            return await OnHttpAccess(hookEventArgs);
        }

        /// <summary>
        /// 播放器鉴权事件,rtsp/rtmp/http-flv/ws-flv/hls的播放都将触发此鉴权事件
        /// </summary>
        /// <remarks> 如果流不存在，那么先触发on_play事件然后触发on_stream_not_found事件;
        /// 播放rtsp流时，如果该流启动了rtsp专属鉴权(on_rtsp_realm)那么将不再触发on_play事件。
        /// <para>详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </para>
        /// </remarks>
        public delegate Task<IHookPlayResult> PlayEventHandle(IHookEventArgs<IPlayInput> hookEventArgs);
        /// <summary>
        /// 播放器鉴权事件,rtsp/rtmp/http-flv/ws-flv/hls的播放都将触发此鉴权事件
        /// </summary>
        /// <remarks> 如果流不存在，那么先触发on_play事件然后触发on_stream_not_found事件;
        /// 播放rtsp流时，如果该流启动了rtsp专属鉴权(on_rtsp_realm)那么将不再触发on_play事件。
        /// <para>详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </para>
        /// </remarks>
        public static event PlayEventHandle OnPlay;
        internal static async Task<IHookPlayResult> OnPlay_Call(IHookEventArgs<IPlayInput> hookEventArgs)
        {
            if (OnPlay == null) return RegisteredModelInstance.GetResultModelDefault<IHookPlayResult>();
            return await OnPlay(hookEventArgs);
        }
        /// <summary>
        /// rtsp/rtmp/rtp推流鉴权事件
        /// </summary>
        /// <remarks> 详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </remarks>
        public delegate Task<IHookPublishResult> PublishEventHandle(IHookEventArgs<IPublishInput> hookEventArgs);
        /// <summary>
        /// rtsp/rtmp/rtp推流鉴权事件
        /// </summary>
        /// <remarks> 详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </remarks>
        public static event PublishEventHandle OnPublish;
        internal static async Task<IHookPublishResult> OnPublish_Call(IHookEventArgs<IPublishInput> hookEventArgs)
        {
            if (OnPublish == null) return RegisteredModelInstance.GetResultModelDefault<IHookPublishResult>();
            return await OnPublish(hookEventArgs);
        }


        /// <summary>
        /// 录制mp4完成后通知事件
        /// </summary>
        /// <remarks> 详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </remarks>
        public delegate Task RecordMp4EventHandle(IHookEventArgs<IRecordMp4Input> hookEventArgs);

        /// <summary>
        /// 录制mp4完成后通知事件
        /// </summary>
        /// <remarks> 详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </remarks>
        public static event RecordMp4EventHandle OnRecordMP4;
        internal static async Task<IHookRecordMp4Result> OnRecordMP4_Call(IHookEventArgs<IRecordMp4Input> hookEventArgs)
        {
            _ = OnRecordMP4?.Invoke(hookEventArgs).ConfigureAwait(false);
            return await Task.FromResult(RegisteredModelInstance.GetResultModelDefault<IHookRecordMp4Result>());
        }

        public delegate Task RecordTsEventHandle(IHookEventArgs<IRecordTsInput> hookEventArgs);

        /// <summary>
        /// 录制TS完成后通知事件
        /// </summary>
        /// <remarks> 详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </remarks>
        public static event RecordTsEventHandle OnRecordTS;
        internal static async Task<IHookRecordTsResult> OnRecordTS_Call(IHookEventArgs<IRecordTsInput> hookEventArgs)
        {
            _ = OnRecordTS?.Invoke(hookEventArgs).ConfigureAwait(false);
            return await Task.FromResult(RegisteredModelInstance.GetResultModelDefault<IHookRecordTsResult>());
        }


        /// <summary>
        /// 该rtsp流是否开启rtsp专用方式的鉴权事件，开启后才会触发on_rtsp_auth事件。
        /// <para>需要指出的是rtsp也支持url参数鉴权，它支持两种方式鉴权。</para>
        /// </summary>
        /// <remarks> 详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </remarks>
        public delegate Task<IHookRtspRealmResult> RtspRealmEventHandle(IHookEventArgs<IRtspRealmInput> hookEventArgs);
        /// <summary>
        /// 该rtsp流是否开启rtsp专用方式的鉴权事件，开启后才会触发on_rtsp_auth事件。
        /// <para>需要指出的是rtsp也支持url参数鉴权，它支持两种方式鉴权。</para>
        /// </summary>
        /// <remarks> 详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </remarks>
        public static event RtspRealmEventHandle OnRtspRealm;
        internal static async Task<IHookRtspRealmResult> OnRtspRealm_Call(IHookEventArgs<IRtspRealmInput> hookEventArgs)
        {
            if (OnRtspRealm == null) return await Task.FromResult(RegisteredModelInstance.GetResultModelDefault<IHookRtspRealmResult>());
            return await OnRtspRealm(hookEventArgs);
        }

        /// <summary>
        /// rtsp专用的鉴权事件，先触发on_rtsp_realm事件然后才会触发on_rtsp_auth事件。
        /// </summary>
        /// <remarks> 详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </remarks>
        public delegate Task<IHookRtspAuthResult> RtspAuthEventHandle(IHookEventArgs<IRtspAuthInput> hookEventArgs);


        /// <summary>
        /// rtsp专用的鉴权事件，先触发on_rtsp_realm事件然后才会触发on_rtsp_auth事件。
        /// </summary>
        /// <remarks> 详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </remarks>
        public static event RtspAuthEventHandle OnRtspAuth;
        internal static async Task<IHookRtspAuthResult> OnRtspAuth_Call(IHookEventArgs<IRtspAuthInput> hookEventArgs)
        {
            if (OnRtspAuth == null) return await Task.FromResult(RegisteredModelInstance.GetResultModelDefault<IHookRtspAuthResult>());
            return await OnRtspAuth(hookEventArgs);
        }

        /// <summary>
        /// shell登录鉴权，ZLMediaKit提供简单的telnet调试方式
        /// 使用telnet 127.0.0.1 9000能进入MediaServer进程的shell界面
        /// </summary>
        /// <remarks> 详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </remarks>
        public delegate Task<IHookShellLoginResult> ShellLoginEventHandle(IHookEventArgs<IShellLoginInput> hookEventArgs);

        /// <summary>
        /// shell登录鉴权，ZLMediaKit提供简单的telnet调试方式
        /// 使用telnet 127.0.0.1 9000能进入MediaServer进程的shell界面
        /// </summary>
        /// <remarks> 详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </remarks>
        public static event ShellLoginEventHandle OnShellLogin;
        internal static async Task<IHookShellLoginResult> OnShellLogin_Call(IHookEventArgs<IShellLoginInput> hookEventArgs)
        {
            if (OnShellLogin == null) return await Task.FromResult(RegisteredModelInstance.GetResultModelDefault<IHookShellLoginResult>());
            return await OnShellLogin(hookEventArgs);
        }

        /// <summary>
        /// rtsp/rtmp流注册或注销时触发此事件
        /// </summary>
        /// <remarks> 详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </remarks>
        public delegate Task<IHookStreamChangedResult> StreamChangedEventHandle(IHookEventArgs<IStreamChangedInput> hookEventArgs);
        /// <summary>
        /// rtsp/rtmp流注册或注销时触发此事件
        /// </summary>
        /// <remarks> 详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </remarks>
        public static event StreamChangedEventHandle OnStreamChanged;
        internal static async Task<IHookStreamChangedResult> OnStreamChanged_Call(IHookEventArgs<IStreamChangedInput> hookEventArgs)
        {
            _ = OnStreamChanged?.Invoke(hookEventArgs).ConfigureAwait(false);
            return await Task.FromResult(RegisteredModelInstance.GetResultModelDefault<IHookStreamChangedResult>());
        }

        /// <summary>
        /// 流无人观看时事件，用户可以通过此事件选择是否关闭无人看的流。
        /// </summary>
        /// <remarks> 详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </remarks>
        public delegate Task<IHookStreamNonReaderResult> StreamNonReaderEventHandle(IHookEventArgs<IStreamNoneReaderInput> hookEventArgs);

        /// <summary>
        /// 流无人观看时事件，用户可以通过此事件选择是否关闭无人看的流。
        /// </summary>
        /// <remarks> 详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </remarks>
        public static event StreamNonReaderEventHandle OnStreamNoneReader;
        internal static async Task<IHookStreamNonReaderResult> OnStreamNoneReader_Call(IHookEventArgs<IStreamNoneReaderInput> hookEventArgs)
        {
            if (OnStreamNoneReader == null) return await Task.FromResult(RegisteredModelInstance.GetResultModelDefault<IHookStreamNonReaderResult>());
            return await OnStreamNoneReader(hookEventArgs);
        }

        /// <summary>
        /// 流未找到事件，用户可以在此事件触发时，立即去拉流，这样可以实现按需拉流
        /// </summary>
        /// <remarks> 详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </remarks>
        public delegate Task StreamNotFoundEventHandle(IHookEventArgs<IStreamNotFoundInuut> hookEventArgs);

        /// <summary>
        /// 流未找到事件，用户可以在此事件触发时，立即去拉流，这样可以实现按需拉流
        /// </summary>
        /// <remarks> 详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </remarks>
        public static event StreamNotFoundEventHandle OnStreamNotFound;
        internal static async Task<IHookStreamNotFoundResult> OnStreamNotFound_Call(IHookEventArgs<IStreamNotFoundInuut> hookEventArgs)
        {
            _ = OnStreamNotFound?.Invoke(hookEventArgs).ConfigureAwait(false);
            return await Task.FromResult(RegisteredModelInstance.GetResultModelDefault<IHookStreamNotFoundResult>());
        }

        /// <summary>
        /// 服务器启动事件，可以用于监听服务器崩溃重启
        /// </summary>
        /// <param name="hookEventArgs"></param>
        /// <returns></returns>
        /// <remarks> 详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </remarks>
        public delegate Task SeverStartedEventHandle(IHookEventArgs<IServerStartedInput> hookEventArgs);

        /// <summary>
        /// 服务器启动事件，可以用于监听服务器崩溃重启
        /// </summary>
        /// <remarks> 详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </remarks>
        public static event SeverStartedEventHandle OnServerStarted;
        internal static async Task<IHookServerStartedResult> OnServerStarted_Call(IHookEventArgs<IServerStartedInput> hookEventArgs)
        {
            _ = OnServerStarted?.Invoke(hookEventArgs).ConfigureAwait(false);
            return await Task.FromResult(RegisteredModelInstance.GetResultModelDefault<IHookServerStartedResult>());
        }

        /// <summary>
        /// 服务器定时上报时间，上报间隔可配置，默认10s上报一次
        /// </summary>
        /// <param name="hookEvent"></param>
        /// <returns></returns>
        /// <remarks> 详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </remarks>
        public delegate Task ServerKeepaliveEventHandle(IHookEventArgs<IServerKeepaliveInput> hookEvent);
        /// <summary>
        /// 服务器定时上报时间，上报间隔可配置，默认10s上报一次
        /// </summary>
        /// <remarks> 详情请查看 https://github.com/xia-chu/ZLMediaKit/wiki </remarks>
        public static event ServerKeepaliveEventHandle OnServerKeepalive;
        internal static async Task<IHookServerKeepaliveResult> OnServerKeepalive_Call(IHookEventArgs<IServerKeepaliveInput> hookEventArgs)
        {
            _ = OnServerKeepalive?.Invoke(hookEventArgs);
            return await Task.FromResult(RegisteredModelInstance.GetResultModelDefault<IHookServerKeepaliveResult>());
        }
    }
}
