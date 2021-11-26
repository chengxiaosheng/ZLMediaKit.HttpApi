using System.Text.Json.Serialization;
using ZLMediaKit.Common.Dtos.ApiInputDto;

namespace ZLMediaKit.Common.Dtos
{
    public interface IServerConfig : IApiResultDataBase
    {
        public IApiConfig Api { get; set; }
        public IFFmpegConfig FFmpeg { get; set; }
        public IGeneralConfig General { get; set; }
        public IHlsConfig Hls { get; set; }
        public IHookConfig Hook { get; set; }
        public IHttpConfig Http { get; set; }
        public IMulticastConfig Multicast { get; set; }
        public IRecordConfig Record { get; set; }
        public IRtmpConfig Rtmp { get; set; }
        public IRtpConfig Rtp { get; set; }
        public IRtpProxyConfig RtpProxy { get; set; }
        public IRtcConfig Rtc { get; set; }
        public IRtspConfig Rtsp { get; set; }
        public IShellConfig Shell { get; set; }
    }

    public class ServerConfig : IServerConfig
    {
        public IApiConfig Api { get; set; }
        public IFFmpegConfig FFmpeg { get; set; }
        public IGeneralConfig General { get; set; }
        public IHlsConfig Hls { get; set; }
        public IHookConfig Hook { get; set; }
        public IHttpConfig Http { get; set; }
        public IMulticastConfig Multicast { get; set; }
        public IRecordConfig Record { get; set; }
        public IRtmpConfig Rtmp { get; set; }
        public IRtpConfig Rtp { get; set; }
        public IRtpProxyConfig RtpProxy { get; set; }
        public IRtcConfig Rtc { get; set; }
        public IRtspConfig Rtsp { get; set; }
        public IShellConfig Shell { get; set; }
    }


    #region api config

    public interface IApiConfig
    {
        /// <summary>
        /// 是否调试http api,启用调试后，会打印每次http请求的内容和回复
        /// </summary>
        [JsonPropertyName("apiDebug")]
        [JsonConverter(typeof(ZLBoolConverter))]
        public bool ApiDebug { get; set; }
        /// <summary>
        ///  一些比较敏感的http api在访问时需要提供secret，否则无权限调用
        ///  如果是通过127.0.0.1访问,那么可以不提供secret
        /// </summary>
        [JsonPropertyName("secret")]
        public string Secret { get; set; }
        /// <summary>
        /// 截图保存路径根目录，截图通过http api(/index/api/getSnap)生成和获取
        /// </summary>
        [JsonPropertyName("snapRoot")]
        public string SnapRoot { get; set; }
        /// <summary>
        /// 默认截图图片，在启动FFmpeg截图后但是截图还未生成时，可以返回默认的预设图片
        /// </summary>
        [JsonPropertyName("defaultSnap")]
        public string DefaultSnap { get; set; }
    }

    public class ApiConfig : IApiConfig
    {
        [JsonConverter(typeof(ZLBoolConverter))]
        public bool ApiDebug { get; set; } = true;
        public string Secret { get; set; } = "035c73f7-bb6b-4889-a715-d9eb2d1925cc";
        public string SnapRoot { get; set; } = "./www/snap/";
        public string DefaultSnap { get; set; } = "./www/logo.png";
    }

    #endregion api config

    #region ffmpeg config 
    public interface IFFmpegConfig
    {
        /// <summary>
        /// FFmpeg可执行程序绝对路径
        /// </summary>
        [JsonPropertyName("bin")]
        public string Bin { get; set; }
        /// <summary>
        /// FFmpeg拉流再推流的命令模板，通过该模板可以设置再编码的一些参数
        /// </summary>
        [JsonPropertyName("cmd")]
        public string Cmd { get; set; }
        /// <summary>
        /// FFmpeg生成截图的命令，可以通过修改该配置改变截图分辨率或质量
        /// </summary>
        [JsonPropertyName("snap")]
        public string Snap { get; set; }
        /// <summary>
        /// FFmpeg日志的路径，如果置空则不生成FFmpeg日志
        /// 可以为相对(相对于本可执行程序目录)或绝对路径
        /// </summary>
        [JsonPropertyName("log")]
        public string Log { get; set; }
    }

    public class FFmpegConfig : IFFmpegConfig
    {
        public virtual string Bin { get; set; } = "/usr/bin/ffmpeg";
        public virtual string Cmd { get; set; } = "% s - re - i % s - c:a aac -strict -2 -ar 44100 -ab 48k -c:v libx264 -f flv %s";
        public virtual string Snap { get; set; } = "% s - i % s - y - f mjpeg -t 0.001 %s";
        public virtual string Log { get; set; } = "./ ffmpeg / ffmpeg.log";
    }
    #endregion ffmpeg config

    #region general config 

    public interface IGeneralConfig
    {
        /// <summary>
        /// 是否启用虚拟主机
        /// </summary>
        [JsonPropertyName("enableVhost")]
        [JsonConverter(typeof(ZLBoolConverter))]
        public bool EnableVhost { get; set; }
        /// <summary>
        /// 播放器或推流器在断开后会触发hook.on_flow_report事件(使用多少流量事件)，
        /// flowThreshold参数控制触发hook.on_flow_report事件阈值，使用流量超过该阈值后才触发，单位KB
        /// </summary>
        [JsonPropertyName("flowThreshold")]
        public int FlowThreshold { get; set; }
        /// <summary>
        ///播放最多等待时间，单位毫秒
        ///播放在播放某个流时，如果该流不存在，
        ///ZLMediaKit会最多让播放器等待maxStreamWaitMS毫秒
        ///如果在这个时间内，该流注册成功，那么会立即返回播放器播放成功
        ///否则返回播放器未找到该流，该机制的目的是可以先播放再推流
        /// </summary>
        [JsonPropertyName("maxStreamWaitMS")]
        public int MaxStreamWaitMS { get; set; }
        /// <summary>
        ///某个流无人观看时，触发hook.on_stream_none_reader事件的最大等待时间，单位毫秒
        ///在配合hook.on_stream_none_reader事件时，可以做到无人观看自动停止拉流或停止接收推流
        /// </summary>
        [JsonPropertyName("streamNoneReaderDelayMS")]
        public int StreamNoneReaderDelayMS { get; set; }
        /// <summary>
        ///拉流代理是否添加静音音频(直接拉流模式本协议无效)
        /// </summary>
        [JsonPropertyName("addMuteAudio")]
        [JsonConverter(typeof(ZLBoolConverter))]
        public bool AddMuteAudio { get; set; }
        /// <summary>
        ///拉流代理时如果断流再重连成功是否删除前一次的媒体流数据，如果删除将重新开始，
        ///如果不删除将会接着上一次的数据继续写(录制hls/mp4时会继续在前一个文件后面写)
        /// </summary>
        [JsonPropertyName("resetWhenRePlay")]
        [JsonConverter(typeof(ZLBoolConverter))]
        public bool ResetWhenRePlay { get; set; }
        /// <summary>
        ///是否默认推流时转换成hls，hook接口(on_publish)中可以覆盖该设置
        /// </summary>
        [JsonPropertyName("publishToHls")]
        [JsonConverter(typeof(ZLBoolConverter))]
        public bool PublishToHls { get; set; }
        /// <summary>
        ///是否默认推流时mp4录像，hook接口(on_publish)中可以覆盖该设置
        /// </summary>
        [JsonPropertyName("publishToMP4")]
        [JsonConverter(typeof(ZLBoolConverter))]
        public bool PublishToMP4 { get; set; }
        /// <summary>
        ///合并写缓存大小(单位毫秒)，合并写指服务器缓存一定的数据后才会一次性写入socket，这样能提高性能，但是会提高延时
        ///开启后会同时关闭TCP_NODELAY并开启MSG_MORE
        /// </summary>
        [JsonPropertyName("mergeWriteMS")]
        [JsonConverter(typeof(ZLBoolConverter))]
        public bool MergeWriteMS { get; set; }
        /// <summary>
        ///全局的时间戳覆盖开关，在转协议时，对frame进行时间戳覆盖
        ///该开关对rtsp/rtmp/rtp推流、rtsp/rtmp/hls拉流代理转协议时生效
        ///会直接影响rtsp/rtmp/hls/mp4/flv等协议的时间戳
        ///同协议情况下不影响(例如rtsp/rtmp推流，那么播放rtsp/rtmp时不会影响时间戳)
        /// </summary>
        [JsonPropertyName("modifyStamp")]
        [JsonConverter(typeof(ZLBoolConverter))]
        public bool ModifyStamp { get; set; }
        /// <summary>
        ///服务器唯一id，用于触发hook时区别是哪台服务器
        /// </summary>
        [JsonPropertyName("mediaServerId")]
        public string MediaServerId { get; set; }
        /// <summary>
        ///转协议是否全局开启或关闭音频
        /// </summary>
        [JsonPropertyName("enable_audio")]
        [JsonConverter(typeof(ZLBoolConverter))]
        public bool EnableAudio { get; set; }

        /// <summary>
        ///hls协议是否按需生成，如果hls.segNum配置为0(意味着hls录制)，那么hls将一直生成(不管此开关)
        /// </summary>
        [JsonPropertyName("hls_demand")]
        [JsonConverter(typeof(ZLBoolConverter))]
        public bool HlsDemand { get; set; }
        /// <summary>
        ///rtsp[s]协议是否按需生成
        /// </summary>
        [JsonPropertyName("rtsp_demand")]
        [JsonConverter(typeof(ZLBoolConverter))]
        public bool RtspDemand { get; set; }
        /// <summary>
        ///rtmp[s]、http[s]-flv、ws[s]-flv协议是否按需生成
        /// </summary>
        [JsonPropertyName("rtmp_demand")]
        [JsonConverter(typeof(ZLBoolConverter))]
        public bool RtmpDemand { get; set; }
        /// <summary>
        ///http[s]-ts协议是否按需生成
        /// </summary>
        [JsonPropertyName("ts_demand")]
        [JsonConverter(typeof(ZLBoolConverter))]
        public bool TsDemand { get; set; }
        /// <summary>
        ///http[s]-fmp4、ws[s]-fmp4协议是否按需生成
        /// </summary>
        [JsonPropertyName("fmp4_demand")]
        [JsonConverter(typeof(ZLBoolConverter))]
        public bool Fmp4Demand { get; set; }
    }

    public class GeneralConfig : IGeneralConfig
    {
        public virtual bool EnableVhost { get; set; } = false;
        public virtual int FlowThreshold { get; set; } = 1024;
        public virtual int MaxStreamWaitMS { get; set; } = 15000;
        public virtual int StreamNoneReaderDelayMS { get; set; } = 20000;
        [JsonConverter(typeof(ZLBoolConverter))]
        public virtual bool AddMuteAudio { get; set; } = true;
        [JsonConverter(typeof(ZLBoolConverter))]
        public virtual bool ResetWhenRePlay { get; set; } = true;
        [JsonConverter(typeof(ZLBoolConverter))]
        public virtual bool PublishToHls { get; set; } = true;
        [JsonConverter(typeof(ZLBoolConverter))]
        public virtual bool PublishToMP4 { get; set; } = false;
        [JsonConverter(typeof(ZLBoolConverter))]
        public virtual bool MergeWriteMS { get; set; } = false;
        [JsonConverter(typeof(ZLBoolConverter))]
        public virtual bool ModifyStamp { get; set; } = false;
        public virtual string MediaServerId { get; set; } = "your_server_id";
        [JsonConverter(typeof(ZLBoolConverter))]
        public virtual bool EnableAudio { get; set; } = true;
        [JsonConverter(typeof(ZLBoolConverter))]
        public virtual bool HlsDemand { get; set; } = false;
        [JsonConverter(typeof(ZLBoolConverter))]
        public virtual bool RtspDemand { get; set; } = false;
        [JsonConverter(typeof(ZLBoolConverter))]
        public virtual bool RtmpDemand { get; set; } = false;
        [JsonConverter(typeof(ZLBoolConverter))]
        public virtual bool TsDemand { get; set; } = false;
        [JsonConverter(typeof(ZLBoolConverter))]
        public virtual bool Fmp4Demand { get; set; } = false;
    }

    #endregion

    #region 
    public interface IHlsConfig
    {
        /// <summary>
        /// hls写文件的buf大小，调整参数可以提高文件io性能
        /// </summary>
        [JsonPropertyName("fileBufSize")]
        public int FileBufSize { get; set; }
        /// <summary>
        /// hls保存文件路径
        /// 可以为相对(相对于本可执行程序目录)或绝对路径
        /// </summary>
        [JsonPropertyName("filePath")]
        public string FilePath { get; set; }
        /// <summary>
        /// hls最大切片时间
        /// </summary>
        [JsonPropertyName("segDur")]
        public int SegDur { get; set; }
        /// <summary>
        /// m3u8索引中,hls保留切片个数(实际保留切片个数大2~3个)
        /// 如果设置为0，则不删除切片，而是保存为点播
        /// </summary>
        [JsonPropertyName("segNum")]
        public int SegNum { get; set; }
        /// <summary>
        /// HLS切片从m3u8文件中移除后，继续保留在磁盘上的个数
        /// </summary>
        [JsonPropertyName("segRetain")]
        public int SegRetain { get; set; }
        /// <summary>
        /// 是否广播 ts 切片完成通知
        /// </summary>
        [JsonPropertyName("broadcastRecordTs")]
        [JsonConverter(typeof(ZLBoolConverter))]
        public bool BroadcastRecordTs { get; set; }
        /// <summary>
        /// 直播hls文件删除延时，单位秒，issue: //913
        /// </summary>
        [JsonPropertyName("deleteDelaySec")]
        public int DeleteDelaySec { get; set; }
    }

    public class HlsConfig : IHlsConfig
    {
        /// <summary>
        /// hls写文件的buf大小，调整参数可以提高文件io性能
        /// </summary>
        [JsonPropertyName("fileBufSize")]
        public virtual int FileBufSize { get; set; } = 65536;
        /// <summary>
        /// hls保存文件路径
        /// 可以为相对(相对于本可执行程序目录)或绝对路径
        /// </summary>
        [JsonPropertyName("filePath")]
        public virtual string FilePath { get; set; } = "./www";
        /// <summary>
        /// hls最大切片时间
        /// </summary>
        [JsonPropertyName("segDur")]
        public virtual int SegDur { get; set; } = 2;
        /// <summary>
        /// m3u8索引中,hls保留切片个数(实际保留切片个数大2~3个)
        /// 如果设置为0，则不删除切片，而是保存为点播
        /// </summary>
        [JsonPropertyName("segNum")]
        public virtual int SegNum { get; set; } = 3;
        /// <summary>
        /// HLS切片从m3u8文件中移除后，继续保留在磁盘上的个数
        /// </summary>
        [JsonPropertyName("segRetain")]
        public virtual int SegRetain { get; set; } = 5;
        /// <summary>
        /// 是否广播 ts 切片完成通知
        /// </summary>
        [JsonPropertyName("broadcastRecordTs")]
        [JsonConverter(typeof(ZLBoolConverter))]
        public virtual bool BroadcastRecordTs { get; set; } = false;
        /// <summary>
        /// 直播hls文件删除延时，单位秒，issue: //913
        /// </summary>
        [JsonPropertyName("deleteDelaySec")]
        public virtual int DeleteDelaySec { get; set; } = 0;
    }
    #endregion

    #region hook cofnig 

    public interface IHookConfig
    {
        /// <summary>
        /// 在推流时，如果url参数匹对admin_params，那么可以不经过hook鉴权直接推流成功，播放时亦然
        /// 该配置项的目的是为了开发者自己调试测试，该参数暴露后会有泄露隐私的安全隐患
        /// </summary>
        [JsonPropertyName("admin_params")]
        public string AdminParams { get; set; }
        /// <summary>
        /// 是否启用hook事件，启用后，推拉流都将进行鉴权
        /// </summary>
        [JsonPropertyName("enable")]
        [JsonConverter(typeof(ZLBoolConverter))]
        public bool Enable { get; set; }
        /// <summary>
        /// 播放器或推流器使用流量事件，置空则关闭
        /// </summary>
        [JsonPropertyName("on_flow_report")]
        public string OnFlowReport { get; set; }
        /// <summary>
        /// 访问http文件鉴权事件，置空则关闭鉴权
        /// </summary>
        [JsonPropertyName("on_http_access")]
        public string OnHttpAccess { get; set; }
        /// <summary>
        /// 播放鉴权事件，置空则关闭鉴权
        /// </summary>
        [JsonPropertyName("on_play")]
        public string OnPlay { get; set; }
        /// <summary>
        /// 推流鉴权事件，置空则关闭鉴权
        /// </summary>
        [JsonPropertyName("on_publish")]
        public string OnPublish { get; set; }
        /// <summary>
        /// 录制mp4切片完成事件
        /// </summary>
        [JsonPropertyName("on_record_mp4")]
        public string OnRecordMp4 { get; set; }
        /// <summary>
        /// 录制 hls ts 切片完成事件
        /// </summary>
        [JsonPropertyName("on_record_ts")]
        public string NnRecordTs { get; set; }
        /// <summary>
        /// rtsp播放鉴权事件，此事件中比对rtsp的用户名密码
        /// </summary>
        [JsonPropertyName("on_rtsp_auth")]
        public string NnRtspAuth { get; set; }
        /// <summary>
        /// rtsp播放是否开启专属鉴权事件，置空则关闭rtsp鉴权。rtsp播放鉴权还支持url方式鉴权
        /// 建议开发者统一采用url参数方式鉴权，rtsp用户名密码鉴权一般在设备上用的比较多
        /// 开启rtsp专属鉴权后，将不再触发on_play鉴权事件
        /// </summary>
        [JsonPropertyName("on_rtsp_realm")]
        public string OnRtspRealm { get; set; }
        /// <summary>
        /// 远程telnet调试鉴权事件
        /// </summary>
        [JsonPropertyName("on_shell_login")]
        public string OnShellLogin { get; set; }
        /// <summary>
        /// 直播流注册或注销事件
        /// </summary>
        [JsonPropertyName("on_stream_changed")]
        public string OnStreamChanged { get; set; }
        /// <summary>
        /// 无人观看流事件，通过该事件，可以选择是否关闭无人观看的流。配合general.streamNoneReaderDelayMS选项一起使用
        /// </summary>
        [JsonPropertyName("on_stream_none_reader")]
        public string OnStreamNoneReader { get; set; }
        /// <summary>
        /// 播放时，未找到流事件，通过配合hook.on_stream_none_reader事件可以完成按需拉流
        /// </summary>
        [JsonPropertyName("on_stream_not_found")]
        public string OnStreamNotFound { get; set; }
        /// <summary>
        /// 服务器启动报告，可以用于服务器的崩溃重启事件监听
        /// </summary>
        [JsonPropertyName("on_server_started")]
        public string OnServerStarted { get; set; }
        /// <summary>
        /// server保活上报
        /// </summary>
        [JsonPropertyName("on_server_keepalive")]
        public string OnServerKeepalive { get; set; }
        /// <summary>
        /// hook api最大等待回复时间，单位秒
        /// </summary>
        public int TimeoutSec { get; set; }
        /// <summary>
        /// keepalive hook触发间隔,单位秒，float类型
        /// </summary>
        [JsonPropertyName("alive_interval")]
        public float AliveInterval { get; set; }
    }

    public class HookConfig : IHookConfig
    {
        /// <summary>
        /// 在推流时，如果url参数匹对admin_params，那么可以不经过hook鉴权直接推流成功，播放时亦然
        /// 该配置项的目的是为了开发者自己调试测试，该参数暴露后会有泄露隐私的安全隐患
        /// </summary>
        [JsonPropertyName("admin_params")]
        public virtual string AdminParams { get; set; } = "035c73f7-bb6b-4889-a715-d9eb2d1925cc";
        /// <summary>
        /// 是否启用hook事件，启用后，推拉流都将进行鉴权
        /// </summary>
        [JsonPropertyName("enable")]
        [JsonConverter(typeof(ZLBoolConverter))]
        public virtual bool Enable { get; set; } = false;
        /// <summary>
        /// 播放器或推流器使用流量事件，置空则关闭
        /// </summary>
        [JsonPropertyName("on_flow_report")]
        public virtual string OnFlowReport { get; set; } = "https://127.0.0.1/index/hook/on_flow_report";
        /// <summary>
        /// 访问http文件鉴权事件，置空则关闭鉴权
        /// </summary>
        [JsonPropertyName("on_http_access")]
        public virtual string OnHttpAccess { get; set; } = "https://127.0.0.1/index/hook/on_http_access";
        /// <summary>
        /// 播放鉴权事件，置空则关闭鉴权
        /// </summary>
        [JsonPropertyName("on_play")]
        public virtual string OnPlay { get; set; } = "https://127.0.0.1/index/hook/on_play";
        /// <summary>
        /// 推流鉴权事件，置空则关闭鉴权
        /// </summary>
        [JsonPropertyName("on_publish")]
        public virtual string OnPublish { get; set; } = "https://127.0.0.1/index/hook/on_publish";
        /// <summary>
        /// 录制mp4切片完成事件
        /// </summary>
        [JsonPropertyName("on_record_mp4")]
        public virtual string OnRecordMp4 { get; set; } = "https://127.0.0.1/index/hook/on_record_mp4";
        /// <summary>
        /// 录制 hls ts 切片完成事件
        /// </summary>
        [JsonPropertyName("on_record_ts")]
        public virtual string NnRecordTs { get; set; } = "https://127.0.0.1/index/hook/on_record_ts";
        /// <summary>
        /// rtsp播放鉴权事件，此事件中比对rtsp的用户名密码
        /// </summary>
        [JsonPropertyName("on_rtsp_auth")]
        public virtual string NnRtspAuth { get; set; } = "https://127.0.0.1/index/hook/on_rtsp_auth";
        /// <summary>
        /// rtsp播放是否开启专属鉴权事件，置空则关闭rtsp鉴权。rtsp播放鉴权还支持url方式鉴权
        /// 建议开发者统一采用url参数方式鉴权，rtsp用户名密码鉴权一般在设备上用的比较多
        /// 开启rtsp专属鉴权后，将不再触发on_play鉴权事件
        /// </summary>
        [JsonPropertyName("on_rtsp_realm")]
        public virtual string OnRtspRealm { get; set; } = "https://127.0.0.1/index/hook/on_rtsp_realm";
        /// <summary>
        /// 远程telnet调试鉴权事件
        /// </summary>
        [JsonPropertyName("on_shell_login")]
        public virtual string OnShellLogin { get; set; } = "https://127.0.0.1/index/hook/on_shell_login";
        /// <summary>
        /// 直播流注册或注销事件
        /// </summary>
        [JsonPropertyName("on_stream_changed")]
        public virtual string OnStreamChanged { get; set; } = "https://127.0.0.1/index/hook/on_stream_changed";
        /// <summary>
        /// 无人观看流事件，通过该事件，可以选择是否关闭无人观看的流。配合general.streamNoneReaderDelayMS选项一起使用
        /// </summary>
        [JsonPropertyName("on_stream_none_reader")]
        public virtual string OnStreamNoneReader { get; set; } = "https://127.0.0.1/index/hook/on_stream_none_reader";
        /// <summary>
        /// 播放时，未找到流事件，通过配合hook.on_stream_none_reader事件可以完成按需拉流
        /// </summary>
        [JsonPropertyName("on_stream_not_found")]
        public virtual string OnStreamNotFound { get; set; } = "https://127.0.0.1/index/hook/on_stream_not_found";
        /// <summary>
        /// 服务器启动报告，可以用于服务器的崩溃重启事件监听
        /// </summary>
        [JsonPropertyName("on_server_started")]
        public virtual string OnServerStarted { get; set; } = "https://127.0.0.1/index/hook/on_server_started";
        /// <summary>
        /// server保活上报
        /// </summary>
        [JsonPropertyName("on_server_keepalive")]
        public virtual string OnServerKeepalive { get; set; } = "https://127.0.0.1/index/hook/on_server_keepalive";
        /// <summary>
        /// hook api最大等待回复时间，单位秒
        /// </summary>
        public virtual int TimeoutSec { get; set; } = 10;
        /// <summary>
        /// keepalive hook触发间隔,单位秒，float类型
        /// </summary>
        [JsonPropertyName("alive_interval")]
        public virtual float AliveInterval { get; set; } = 10.0f;
    }

    #endregion hook config 

    #region Http Config

    public interface IHttpConfig
    {
        /// <summary>
        /// http服务器字符编码，windows上默认gb2312
        /// </summary>
        [JsonPropertyName("charSet")]
        public string CharSet { get; set; }
        /// <summary>
        /// http链接超时时间
        /// </summary>
        [JsonPropertyName("keepAliveSecond")]
        public int KeepAliveSecond { get; set; }
        /// <summary>
        /// http请求体最大字节数，如果post的body太大，则不适合缓存body在内存
        /// </summary>
        [JsonPropertyName("maxReqSize")]
        public int MaxReqSize { get; set; }

        /// <summary>
        /// 404网页内容，用户可以自定义404网页
        /// </summary>
        [JsonPropertyName("notFound")]
        public string NotFound { get; set; }
        /// <summary>
        /// http服务器监听端口
        /// </summary>
        [JsonPropertyName("port")]
        public int Port { get; set; }
        /// <summary>
        /// http文件服务器根目录
        /// 可以为相对(相对于本可执行程序目录)或绝对路径
        /// </summary>
        [JsonPropertyName("rootPath")]
        public string RootPath { get; set; }
        /// <summary>
        /// http文件服务器读文件缓存大小，单位BYTE，调整该参数可以优化文件io性能
        /// </summary>
        [JsonPropertyName("sendBufSize")]
        public int SendBufSize { get; set; }
        /// <summary>
        /// https服务器监听端口
        /// </summary>
        [JsonPropertyName("sslport")]
        public int Sslport { get; set; }
        /// <summary>
        /// 是否显示文件夹菜单，开启后可以浏览文件夹
        /// </summary>
        [JsonPropertyName("dirMenu")]
        [JsonConverter(typeof(ZLBoolConverter))]
        public bool DirMenu { get; set; }
        /// <summary>
        /// 虚拟目录, 虚拟目录名和文件路径使用","隔开，多个配置路径间用";"隔开
        /// 例如赋值为 app_a,/path/to/a;app_b,/path/to/b 那么
        /// 访问 http://127.0.0.1/app_a/file_a 对应的文件路径为 /path/to/a/file_a
        /// 访问 http://127.0.0.1/app_b/file_b 对应的文件路径为 /path/to/b/file_b
        /// 访问其他http路径,对应的文件路径还是在rootPath内
        /// </summary>
        [JsonPropertyName("virtualPath")]
        public string VirtualPath { get; set; }
    }

    public class HttpConfig : IHttpConfig
    {
        /// <summary>
        /// http服务器字符编码，windows上默认gb2312
        /// </summary>
        [JsonPropertyName("charSet")]
        public virtual string CharSet { get; set; } = "utf-8";
        /// <summary>
        /// http链接超时时间
        /// </summary>
        [JsonPropertyName("keepAliveSecond")]
        public virtual int KeepAliveSecond { get; set; } = 30;
        /// <summary>
        /// http请求体最大字节数，如果post的body太大，则不适合缓存body在内存
        /// </summary>
        [JsonPropertyName("maxReqSize")]
        public virtual int MaxReqSize { get; set; } = 40960;

        /// <summary>
        /// 404网页内容，用户可以自定义404网页
        /// </summary>
        [JsonPropertyName("notFound")]
        public virtual string NotFound { get; set; } = "<html><head><title>404 Not Found</title></head><body bgcolor\"white\"><center><h1>您访问的资源不存在！</h1></center><hr><center>ZLMediaKit-4.0</center></body></html>";
        /// <summary>
        /// http服务器监听端口
        /// </summary>
        [JsonPropertyName("port")]
        public virtual int Port { get; set; } = 80;
        /// <summary>
        /// http文件服务器根目录
        /// 可以为相对(相对于本可执行程序目录)或绝对路径
        /// </summary>
        [JsonPropertyName("rootPath")]
        public virtual string RootPath { get; set; } = "./www";
        /// <summary>
        /// http文件服务器读文件缓存大小，单位BYTE，调整该参数可以优化文件io性能
        /// </summary>
        [JsonPropertyName("sendBufSize")]
        public virtual int SendBufSize { get; set; } = 65536;
        /// <summary>
        /// https服务器监听端口
        /// </summary>
        [JsonPropertyName("sslport")]
        public virtual int Sslport { get; set; } = 443;
        /// <summary>
        /// 是否显示文件夹菜单，开启后可以浏览文件夹
        /// </summary>
        [JsonPropertyName("dirMenu")]
        [JsonConverter(typeof(ZLBoolConverter))]
        public virtual bool DirMenu { get; set; } = true;
        /// <summary>
        /// 虚拟目录, 虚拟目录名和文件路径使用","隔开，多个配置路径间用";"隔开
        /// 例如赋值为 app_a,/path/to/a;app_b,/path/to/b 那么
        /// 访问 http://127.0.0.1/app_a/file_a 对应的文件路径为 /path/to/a/file_a
        /// 访问 http://127.0.0.1/app_b/file_b 对应的文件路径为 /path/to/b/file_b
        /// 访问其他http路径,对应的文件路径还是在rootPath内
        /// </summary>
        [JsonPropertyName("virtualPath")]
        public virtual string VirtualPath { get; set; }
    }

    #endregion http Config

    #region multicast config 

    public interface IMulticastConfig
    {
        /// <summary>
        /// rtp组播截止组播ip地址
        /// </summary>
        [JsonPropertyName("addrMax")]
        public string AddrMax { get; set; }
        /// <summary>
        /// rtp组播起始组播ip地址
        /// </summary>
        [JsonPropertyName("addrMin")]
        public string AddrMin { get; set; }
        /// <summary>
        /// 组播udp ttl
        /// </summary>
        [JsonPropertyName("udpTTL")]
        public int UdpTTL { get; set; }
    }

    public class MulticastConfig : IMulticastConfig
    {
        /// <summary>
        /// rtp组播截止组播ip地址
        /// </summary>
        [JsonPropertyName("addrMax")]
        public virtual string AddrMax { get; set; } = "239.255.255.255";
        /// <summary>
        /// rtp组播起始组播ip地址
        /// </summary>
        [JsonPropertyName("addrMin")]
        public virtual string AddrMin { get; set; } = "239.0.0.0";
        /// <summary>
        /// 组播udp ttl
        /// </summary>
        [JsonPropertyName("udpTTL")]
        public virtual int UdpTTL { get; set; } = 64;
    }

    #endregion multicast config 

    #region record config 

    public interface IRecordConfig
    {
        /// <summary>
        /// mp4录制或mp4点播的应用名，通过限制应用名，可以防止随意点播 点播的文件必须放置在此文件夹下
        /// </summary>
        [JsonPropertyName("appName")]
        public string AppName { get; set; }
        /// <summary>
        /// mp4录制写文件缓存，单位BYTE,调整参数可以提高文件io性能
        /// </summary>
        [JsonPropertyName("fileBufSize")]
        public int FileBufSize { get; set; }
        /// <summary>
        /// mp4录制保存、mp4点播根路径 ，可以为相对(相对于本可执行程序目录)或绝对路径
        /// </summary>
        [JsonPropertyName("filePath")]
        public string FilePath { get; set; }
        /// <summary>
        /// mp4录制切片时间，单位秒
        /// </summary>
        [JsonPropertyName("fileSecond")]
        public int FileSecond { get; set; }
        /// <summary>
        /// mp4点播每次流化数据量，单位毫秒， 减少该值可以让点播数据发送量更平滑，增大该值则更节省cpu资源
        /// </summary>
        [JsonPropertyName("sampleMS")]
        public int SampleMS { get; set; }
        /// <summary>
        /// mp4录制完成后是否进行二次关键帧索引写入头部
        /// </summary>
        [JsonPropertyName("fastStart")]
        [JsonConverter(typeof(ZLBoolConverter))]
        public bool FastStart { get; set; }
        /// <summary>
        /// MP4点播(rtsp/rtmp/http-flv/ws-flv)是否循环播放文件
        /// </summary>
        [JsonPropertyName("fileRepeat")]
        [JsonConverter(typeof(ZLBoolConverter))]
        public bool FileRepeat { get; set; }
    }

    public class RecordConfig : IRecordConfig
    {
        /// <summary>
        /// mp4录制或mp4点播的应用名，通过限制应用名，可以防止随意点播 点播的文件必须放置在此文件夹下
        /// </summary>
        [JsonPropertyName("appName")]
        public virtual string AppName { get; set; } = "record";
        /// <summary>
        /// mp4录制写文件缓存，单位BYTE,调整参数可以提高文件io性能
        /// </summary>
        [JsonPropertyName("fileBufSize")]
        public virtual int FileBufSize { get; set; } = 65536;
        /// <summary>
        /// mp4录制保存、mp4点播根路径 ，可以为相对(相对于本可执行程序目录)或绝对路径
        /// </summary>
        [JsonPropertyName("filePath")]
        public virtual string FilePath { get; set; } = "./www";
        /// <summary>
        /// mp4录制切片时间，单位秒
        /// </summary>
        [JsonPropertyName("fileSecond")]
        public virtual int FileSecond { get; set; } = 3600;
        /// <summary>
        /// mp4点播每次流化数据量，单位毫秒， 减少该值可以让点播数据发送量更平滑，增大该值则更节省cpu资源
        /// </summary>
        [JsonPropertyName("sampleMS")]
        public virtual int SampleMS { get; set; } = 500;
        /// <summary>
        /// mp4录制完成后是否进行二次关键帧索引写入头部
        /// </summary>
        [JsonPropertyName("fastStart")]
        public virtual bool FastStart { get; set; } = false;
        /// <summary>
        /// MP4点播(rtsp/rtmp/http-flv/ws-flv)是否循环播放文件
        /// </summary>
        [JsonPropertyName("fileRepeat")]
        [JsonConverter(typeof(ZLBoolConverter))]
        public virtual bool FileRepeat { get; set; } = false;
    }

    #endregion record config

    #region Rtmp config 

    public interface IRtmpConfig
    {
        /// <summary>
        /// rtmp必须在此时间内完成握手，否则服务器会断开链接，单位秒
        /// </summary>
        [JsonPropertyName("handshakeSecond")]
        public int HandshakeSecond { get; set; }
        /// <summary>
        /// rtmp超时时间，如果该时间内未收到客户端的数据， 或者tcp发送缓存超过这个时间，则会断开连接，单位秒
        /// </summary>
        [JsonPropertyName("keepAliveSecond")]
        public int KeepAliveSecond { get; set; }
        /// <summary>
        /// 在接收rtmp推流时，是否重新生成时间戳(很多推流器的时间戳着实很烂)
        /// </summary>
        [JsonPropertyName("modifyStamp")]
        [JsonConverter(typeof(ZLBoolConverter))]
        public bool ModifyStamp { get; set; }
        /// <summary>
        /// rtmp服务器监听端口
        /// </summary>
        [JsonPropertyName("port")]
        public int Port { get; set; }
        /// <summary>
        /// rtmps服务器监听地址
        /// </summary>
        [JsonPropertyName("sslport")]
        public int Sslport { get; set; }
    }

    public class RtmpConfig : IRtmpConfig
    {
        /// <summary>
        /// rtmp必须在此时间内完成握手，否则服务器会断开链接，单位秒
        /// </summary>
        [JsonPropertyName("handshakeSecond")]
        public virtual int HandshakeSecond { get; set; } = 15;
        /// <summary>
        /// rtmp超时时间，如果该时间内未收到客户端的数据， 或者tcp发送缓存超过这个时间，则会断开连接，单位秒
        /// </summary>
        [JsonPropertyName("keepAliveSecond")]
        public virtual int KeepAliveSecond { get; set; } = 15;
        /// <summary>
        /// 在接收rtmp推流时，是否重新生成时间戳(很多推流器的时间戳着实很烂)
        /// </summary>
        [JsonPropertyName("modifyStamp")]
        [JsonConverter(typeof(ZLBoolConverter))]
        public virtual bool ModifyStamp { get; set; } = false;
        /// <summary>
        /// rtmp服务器监听端口
        /// </summary>
        [JsonPropertyName("port")]
        public virtual int Port { get; set; } = 1935;
        /// <summary>
        /// rtmps服务器监听地址
        /// </summary>
        [JsonPropertyName("sslport")]
        public virtual int Sslport { get; set; } = 0;
    }

    #endregion Rtmp config 

    #region rtp Config

    public interface IRtpConfig
    {
        /// <summary>
        /// 频mtu大小，该参数限制rtp最大字节数，推荐不要超过1400
        /// 加大该值会明显增加直播延时
        /// </summary>
        [JsonPropertyName("audioMtuSize")]
        public int AudioMtuSize { get; set; }
        /// <summary>
        /// 视频mtu大小，该参数限制rtp最大字节数，推荐不要超过1400
        /// </summary>
        [JsonPropertyName("videoMtuSize")]
        public int VideoMtuSize { get; set; }
        /// <summary>
        /// rtp包最大长度限制，单位KB,主要用于识别TCP上下文破坏时，获取到错误的rtp
        /// </summary>
        [JsonPropertyName("rtpMaxSize")]
        public int RtpMaxSize { get; set; }
    }

    public class RtpConfig : IRtpConfig
    {
        /// <summary>
        /// 频mtu大小，该参数限制rtp最大字节数，推荐不要超过1400
        /// 加大该值会明显增加直播延时
        /// </summary>
        [JsonPropertyName("audioMtuSize")]
        public virtual int AudioMtuSize { get; set; } = 600;
        /// <summary>
        /// 视频mtu大小，该参数限制rtp最大字节数，推荐不要超过1400
        /// </summary>
        [JsonPropertyName("videoMtuSize")]
        public virtual int VideoMtuSize { get; set; } = 1400;
        /// <summary>
        /// rtp包最大长度限制，单位KB,主要用于识别TCP上下文破坏时，获取到错误的rtp
        /// </summary>
        [JsonPropertyName("rtpMaxSize")]
        public virtual int RtpMaxSize { get; set; } = 10;
    }

    #endregion rtp config

    #region RtpProxy config 
    public interface IRtpProxyConfig
    {
        /// <summary>
        /// 导出调试数据(包括rtp/ps/h264)至该目录,置空则关闭数据导出
        /// </summary>
        [JsonPropertyName("dumpDir")]
        public string DumpDir { get; set; }
        /// <summary>
        /// udp和tcp代理服务器，支持rtp(必须是ts或ps类型)代理
        /// </summary>
        [JsonPropertyName("port")]
        public int Port { get; }
        /// <summary>
        /// rtp超时时间，单位秒
        /// </summary>
        [JsonPropertyName("timeoutSec")]
        public int TimeoutSec { get; set; }
    }
    public class RtpProxyConfig : IRtpProxyConfig
    {
        /// <summary>
        /// 导出调试数据(包括rtp/ps/h264)至该目录,置空则关闭数据导出
        /// </summary>
        [JsonPropertyName("dumpDir")]
        public virtual string DumpDir { get; set; }
        /// <summary>
        /// udp和tcp代理服务器，支持rtp(必须是ts或ps类型)代理
        /// </summary>
        [JsonPropertyName("port")]
        public virtual int Port { get; set; } = 10000;
        /// <summary>
        /// rtp超时时间，单位秒
        /// </summary>
        [JsonPropertyName("timeoutSec")]
        public virtual int TimeoutSec { get; set; } = 15;
    }
    #endregion RtpProxy config 

    #region Rtc config 

    public interface IRtcConfig
    {
        /// <summary>
        /// rtc播放推流、播放超时时间
        /// </summary>
        [JsonPropertyName("timeoutSec")]
        public int TimeoutSec { get; set; }
        /// <summary>
        /// 本机对rtc客户端的可见ip，作为服务器时一般为公网ip，置空时，会自动获取网卡ip
        /// </summary>
        [JsonPropertyName("externIP")]
        public string ExternIP { get; set; }
        /// <summary>
        /// rtc udp服务器监听端口号，所有rtc客户端将通过该端口传输stun/dtls/srtp/srtcp数据，
        /// 该端口是多线程的，同时支持客户端网络切换导致的连接迁移
        /// 需要注意的是，如果服务器在nat内，需要做端口映射时，必须确保外网映射端口跟该端口一致
        /// </summary>
        [JsonPropertyName("port")]
        public int Port { get; set; }
        /// <summary>
        /// 设置remb比特率，非0时关闭twcc并开启remb。该设置在rtc推流时有效，可以控制推流画质
        /// 目前已经实现twcc自动调整码率，关闭remb根据真实网络状况调整码率
        /// </summary>
        [JsonPropertyName("rembBitRate")]
        public int RembBitRate { get; set; }
        /// <summary>
        /// rtc支持的音频codec类型,在前面的优先级更高
        /// 以下范例为所有支持的音频codec
        /// </summary>
        [JsonPropertyName("preferredCodecA")]
        public string PreferredCodecA { get; set; }
        /// <summary>
        /// rtc支持的视频codec类型,在前面的优先级更高
        /// 以下范例为所有支持的视频codec
        /// </summary>
        [JsonPropertyName("preferredCodecV")]
        public string PreferredCodecV { get; set; }
    }

    public class RtcConfig : IRtcConfig
    {
        /// <summary>
        /// rtc播放推流、播放超时时间
        /// </summary>
        [JsonPropertyName("timeoutSec")]
        public virtual int TimeoutSec { get; set; } = 15;
        /// <summary>
        /// 本机对rtc客户端的可见ip，作为服务器时一般为公网ip，置空时，会自动获取网卡ip
        /// </summary>
        [JsonPropertyName("externIP")]
        public virtual string ExternIP { get; set; }
        /// <summary>
        /// rtc udp服务器监听端口号，所有rtc客户端将通过该端口传输stun/dtls/srtp/srtcp数据，
        /// 该端口是多线程的，同时支持客户端网络切换导致的连接迁移
        /// 需要注意的是，如果服务器在nat内，需要做端口映射时，必须确保外网映射端口跟该端口一致
        /// </summary>
        [JsonPropertyName("port")]
        public virtual int Port { get; set; } = 8000;
        /// <summary>
        /// 设置remb比特率，非0时关闭twcc并开启remb。该设置在rtc推流时有效，可以控制推流画质
        /// 目前已经实现twcc自动调整码率，关闭remb根据真实网络状况调整码率
        /// </summary>
        [JsonPropertyName("rembBitRate")]
        public virtual int RembBitRate { get; set; } = 0;
        /// <summary>
        /// rtc支持的音频codec类型,在前面的优先级更高
        /// 以下范例为所有支持的音频codec
        /// </summary>
        [JsonPropertyName("preferredCodecA")]
        public virtual string PreferredCodecA { get; set; } = "PCMU,PCMA,opus,mpeg4-generic";
        /// <summary>
        /// rtc支持的视频codec类型,在前面的优先级更高
        /// 以下范例为所有支持的视频codec
        /// </summary>
        [JsonPropertyName("preferredCodecV")]
        public virtual string PreferredCodecV { get; set; } = "H264, H265, AV1X, VP9, VP8";
    }

    #endregion rtc config 

    #region rtsp config

    public interface IRtspConfig
    {
        /// <summary>
        /// rtsp专有鉴权方式是采用base64还是md5方式
        /// </summary>
        [JsonPropertyName("authBasic")]
        [JsonConverter(typeof(ZLBoolConverter))]
        public  bool AuthBasic { get; set; } 
        /// <summary>
        /// rtsp拉流、推流代理是否是直接代理模式
        /// 直接代理后支持任意编码格式，但是会导致GOP缓存无法定位到I帧，可能会导致开播花屏
        /// 并且如果是tcp方式拉流，如果rtp大于mtu会导致无法使用udp方式代理
        /// 假定您的拉流源地址不是264或265或AAC，那么你可以使用直接代理的方式来支持rtsp代理
        /// 如果你是rtsp推拉流，但是rtc播放，也建议关闭直接代理模式，
        /// 因为直接代理时，rtp中可能没有sps pps,会导致rtc无法播放
        /// 默认开启rtsp直接代理，rtmp由于没有这些问题，是强制开启直接代理的
        /// </summary>
        [JsonPropertyName("directProxy")]
        [JsonConverter(typeof(ZLBoolConverter))]
        public  bool DirectProxy { get; set; } 
        /// <summary>
        /// rtsp必须在此时间内完成握手，否则服务器会断开链接，单位秒
        /// </summary>
        [JsonPropertyName("handshakeSecond")]
        public  int HandshakeSecond { get; set; }
        /// <summary>
        /// rtsp超时时间，如果该时间内未收到客户端的数据，
        /// 或者tcp发送缓存超过这个时间，则会断开连接，单位秒
        /// </summary>
        [JsonPropertyName("keepAliveSecond")]
        public  int KeepAliveSecond { get; set; }
        /// <summary>
        /// rtsp服务器监听地址
        /// </summary>
        [JsonPropertyName("port")]
        public  int Port { get; set; }
        /// <summary>
        /// rtsps服务器监听地址
        /// </summary>
        [JsonPropertyName("sslport")]
        public  int Sslport { get; set; }
    }

    public class RtspConfig : IRtspConfig
    {
        /// <summary>
        /// rtsp专有鉴权方式是采用base64还是md5方式
        /// </summary>
        [JsonPropertyName("authBasic")]
        [JsonConverter(typeof(ZLBoolConverter))]
        public virtual bool AuthBasic { get; set; } = false;
        /// <summary>
        /// rtsp拉流、推流代理是否是直接代理模式
        /// 直接代理后支持任意编码格式，但是会导致GOP缓存无法定位到I帧，可能会导致开播花屏
        /// 并且如果是tcp方式拉流，如果rtp大于mtu会导致无法使用udp方式代理
        /// 假定您的拉流源地址不是264或265或AAC，那么你可以使用直接代理的方式来支持rtsp代理
        /// 如果你是rtsp推拉流，但是rtc播放，也建议关闭直接代理模式，
        /// 因为直接代理时，rtp中可能没有sps pps,会导致rtc无法播放
        /// 默认开启rtsp直接代理，rtmp由于没有这些问题，是强制开启直接代理的
        /// </summary>
        [JsonPropertyName("directProxy")]
        [JsonConverter(typeof(ZLBoolConverter))]
        public virtual bool DirectProxy { get; set; } = true;
        /// <summary>
        /// rtsp必须在此时间内完成握手，否则服务器会断开链接，单位秒
        /// </summary>
        [JsonPropertyName("handshakeSecond")]
        public virtual int HandshakeSecond { get; set; } = 15;
        /// <summary>
        /// rtsp超时时间，如果该时间内未收到客户端的数据，
        /// 或者tcp发送缓存超过这个时间，则会断开连接，单位秒
        /// </summary>
        [JsonPropertyName("keepAliveSecond")]
        public virtual int KeepAliveSecond { get; set; } = 15;
        /// <summary>
        /// rtsp服务器监听地址
        /// </summary>
        [JsonPropertyName("port")]
        public virtual int Port { get; set; } = 554;
        /// <summary>
        /// rtsps服务器监听地址
        /// </summary>
        [JsonPropertyName("sslport")]
        public virtual int Sslport { get; set; } = 0;
    }

    #endregion rtsp config

    #region shell config 

    public interface IShellConfig
    {
        /// <summary>
        /// 调试telnet服务器接受最大bufffer大小
        /// </summary>
        [JsonPropertyName("maxReqSize")]
        public int MaxReqSize { get; set; }
        /// <summary>
        /// 调试telnet服务器监听端口
        /// </summary>
        [JsonPropertyName("port")]
        public int Port { get; set; }
    }

    public class ShellConfig : IShellConfig
    {
        /// <summary>
        /// 调试telnet服务器接受最大bufffer大小
        /// </summary>
        [JsonPropertyName("maxReqSize")]
        public virtual int MaxReqSize { get; set; } = 1024;
        /// <summary>
        /// 调试telnet服务器监听端口
        /// </summary>
        [JsonPropertyName("port")]
        public virtual int Port { get; set; } = 0;
    }

    #endregion shell config
}
