using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using RestSharp;
using ZLMediaKit.Common;
using RestSharp.Serializers.SystemTextJson;
using ZLMediaKit.Common.Dtos.ApiInputDto;
using ZLMediaKit.Common.Dtos;
using System.Dynamic;
using System.Diagnostics.CodeAnalysis;

namespace ZLMediaKit.HttpApi
{
    /// <summary>
    /// 
    /// </summary>
    public class ZLHttpClient
    {
        /// <summary>
        /// 
        /// </summary>
        public ZLHttpClient()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediaServerId"></param>
        public ZLHttpClient([NotNull] string mediaServerId) => CreateClient(mediaServerId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverManager"></param>
        public ZLHttpClient([NotNull] IServerManager serverManager) => CreateClient(serverManager);


        private IServerManager _serverManager;

        private IRestClient _restClient;


        private IRestClient CreateClient(string mediaserverId) => CreateClient(IServerManager.GetServerManager(mediaserverId));

        private IRestClient CreateClient(IServerManager serverManager)
        {
            _serverManager = serverManager;
            if (serverManager == null) throw new ArgumentNullException(nameof(serverManager));
            var client = new RestClient(_serverManager.ApiBaseUri);
            client.UseSystemTextJson(TypeMapping.SerializerOptions);
            this._restClient = client;
            client.Timeout = _serverManager.Timeout;
            client.AddDefaultQueryParameter("secret", _serverManager.ApiSecret);
            return client;
        }

        private IRestClient CreateClient() => CreateClient(IServerManager.GetDefaultServerManager());

        /// <summary>
        /// 在多ZLMediaKit部署模式下，调用ZLM接口前，应先调用此方法设置具体的ZLMediaKit
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <returns></returns>
        public ZLHttpClient SetMediaServerId(string mediaServerId)
        {
            if (string.IsNullOrEmpty(mediaServerId)) CreateClient();
            CreateClient(mediaServerId);
            return this;
        }

        /// <summary>
        /// 在多ZLMediaKit部署模式下，调用ZLM接口前，应先调用此方法设置具体的ZLMediaKit
        /// </summary>
        /// <param name="serverManager"></param>
        /// <returns></returns>
        public ZLHttpClient SetMediaServerId(IServerManager serverManager)
        {
            CreateClient(serverManager);
            return this;
        }


        /// <summary>
        /// 获取各epoll(或select)线程负载以及延时
        /// </summary>
        /// <returns></returns>
        public async Task<IThreadsLoadApiResult> GetThreadsLoad()
        {
            var request = new RestRequest("getThreadsLoad");
            return await (_restClient ?? CreateClient()).GetAsync<IThreadsLoadApiResult>(request);
        }

        /// <summary>
        /// 获取各epoll(或select)线程负载以及延时
        /// </summary>
        /// <returns></returns>
        public async Task<IThreadsLoadApiResult> GetThreadsLoad(string mediaServerId)
        {
            CreateClient(mediaServerId);
            return await GetThreadsLoad();
        }

        /// <summary>
        /// 获取各后台epoll(或select)线程负载以及延时
        /// </summary>
        /// <returns></returns>
        public async Task<IWorkThreadsLoadApiResult> GetWorkThreadsLoad()
        {
            var request = new RestRequest("getWorkThreadsLoad");
            return await (_restClient ?? CreateClient()).GetAsync<IWorkThreadsLoadApiResult>(request);
        }

        /// <summary>
        /// 获取各后台epoll(或select)线程负载以及延时
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <returns></returns>
        public async Task<IWorkThreadsLoadApiResult> GetWorkThreadsLoad(string mediaServerId)
        {
            CreateClient(mediaServerId);
            return await GetWorkThreadsLoad();
        }

        /// <summary>
        /// 获取服务器配置
        /// </summary>
        /// <returns></returns>
        public async Task<IApiGetServerConfigResult> GetServerConfig()
        {
            var request = new RestRequest("getServerConfig");
            var result = await (_restClient ?? CreateClient()).GetAsync<ApiResultBase<List<Dictionary<string,string>>>>(request);

            var groups = result.Data.FirstOrDefault()?.Select(s => new { Key = s.Key.Split('.'), Value = s.Value }).Select(s => new
            {
                ClassName = s.Key.Length == 2 ? s.Key.FirstOrDefault().Replace("_",String.Empty) : "RootElement",
                Key = s.Key.LastOrDefault(),
                Value = s.Value
            }).GroupBy(s => s.ClassName);
            IDictionary<string, object> configDict = new ExpandoObject();
            foreach (var group in groups)
            {
                IDictionary<string, object> temp = new ExpandoObject();
                foreach (var item in group)
                {
                    if (group.Key == "rootElement")
                    {
                        configDict[item.Key] = item.Value;
                    }
                    else
                        temp.Add(item.Key, item.Value);
                }
                configDict.Add(group.Key, temp);
            }
            var jsonStr = System.Text.Json.JsonSerializer.Serialize(configDict as ExpandoObject);
            var serverCofnig = System.Text.Json.JsonSerializer.Deserialize<IServerConfig>(jsonStr, TypeMapping.SerializerOptions);
            if(serverCofnig != null)
            {
                if (_serverManager.MediaServerId != serverCofnig.General.MediaServerId)
                {
                    IServerManager.RemoveServer(_serverManager);
                    _serverManager.MediaServerId = serverCofnig.General.MediaServerId;
                    IServerManager.AddServer(_serverManager);
                }
                _serverManager.ServerConfig = serverCofnig;
            }
            var resultInfo = new ApiGetServerConfigResult()
            {
                Code = result.Code,
                Messag = result.Messag,
                Data =  serverCofnig
            };
            return resultInfo;
        }

        /// <summary>
        /// 获取服务器配置
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <returns></returns>
        public async Task<IApiGetServerConfigResult> GetServerConfig(string mediaServerId)
        {
            CreateClient(mediaServerId);
            return await GetServerConfig();
        }

        /// <summary>
        /// 动态修改ZLM配置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ISetServerConfigResult> SetServerConfig(params ISetServerConfigInput[] input)
        {
            var request = new RestRequest("setServerConfig");
            request.AddOrUpdateParameters(input?.Select(s=> new Parameter($"{s.ClassName}.{s.Key}",s.Value, ParameterType.QueryStringWithoutEncode)));
            _ =  GetServerConfig(_serverManager.MediaServerId).ConfigureAwait(false);
            return await (_restClient ?? CreateClient()).GetAsync<ISetServerConfigResult>(request);
        }

        /// <summary>
        /// 动态修改ZLM配置
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ISetServerConfigResult> SetServerConfig(string mediaServerId, params ISetServerConfigInput[] input)
        {
            CreateClient(mediaServerId);
            return await SetServerConfig(input);
        }


        /// <summary>
        /// 启服务器,只有Daemon方式才能重启，否则是直接关闭！
        /// </summary>
        /// <returns></returns>
        public async Task<IApiRestartServerResult> RestartServer()
        {
            var request = new RestRequest("restartServer");
            return await (_restClient ?? CreateClient()).GetAsync<IApiRestartServerResult>(request);
        }

        /// <summary>
        /// 启服务器,只有Daemon方式才能重启，否则是直接关闭！
        /// </summary>
        /// <returns></returns>
        public async Task<IApiRestartServerResult> RestartServer(string mediaServerId)
        {
            CreateClient(mediaServerId);
            return await RestartServer();
        }

        /// <summary>
        /// 获取流列表，可选筛选参数
        /// </summary>
        /// <param name="schema">筛选协议，例如 rtsp或rtmp</param>
        /// <param name="vhost">筛选虚拟主机，例如__defaultVhost__</param>
        /// <param name="app">筛选应用名，例如 live</param>
        /// <returns></returns>
        public async Task<IApiGetMediaListResult> GetMediaList(string schema = null, string vhost = null, string app = null)
        {
            var request = new RestRequest("getMediaList")
                .AddQueryParameter("schema", schema, true)
                .AddQueryParameter("vhost", vhost, true)
                .AddQueryParameter("app", app, true);
            return await (_restClient ?? CreateClient()).GetAsync<IApiGetMediaListResult>(request);
        }

        /// <summary>
        /// 获取流列表，可选筛选参数
        /// </summary>
        /// <param name="schema">筛选协议，例如 rtsp或rtmp</param>
        /// <param name="vhost">筛选虚拟主机，例如__defaultVhost__</param>
        /// <param name="app">筛选应用名，例如 live</param>
        /// <returns></returns>
        public async Task<IApiGetMediaListResult> GetMediaList(string mediaServerId, string schema = null, string vhost = null, string app = null)
        {
            CreateClient(mediaServerId);
            return await GetMediaList(schema, vhost, app);
        }


        /// <summary>
        /// 关闭流(目前所有类型的流都支持关闭)
        /// </summary>
        /// <param name="schema">协议，例如 rtsp或rtmp</param>
        /// <param name="vhost">虚拟主机，例如__defaultVhost__</param>
        /// <param name="app">应用名，例如 live</param>
        /// <param name="stream">流id，例如 test</param>
        /// <param name="force">是否强制关闭(有人在观看是否还关闭)</param>
        /// <returns>result 
        /// <para>0 成功</para>
        /// <para>-1 关闭失败</para>
        /// <para>-2 该流不存在</para>
        /// </returns>
        /// <remarks>已过期，请使用 <see cref="CloseStreams(string, string, string, string, bool)"/>接口替代</remarks>
        [Obsolete("已过期，请使用close_streams接口替换")]
        public async Task<IApiCloseStreamResult> CloseStream(string schema, string vhost, string app, string stream, bool force)
        {
            var request = new RestRequest("close_stream")
                .AddQueryParameter("schema", schema, true)
                .AddQueryParameter("vhost", vhost, true)
                .AddQueryParameter("app", app, true)
                .AddQueryParameter("stream", stream, true)
                .AddQueryParameter("force", force ? "1" : "0");
            return await (_restClient ?? CreateClient()).GetAsync<IApiCloseStreamResult>(request);
        }

        /// <summary>
        /// 关闭流(目前所有类型的流都支持关闭)
        /// </summary>
        /// <param name="mediaServerId">协议，例如 rtsp或rtmp</param>
        /// <param name="schema">协议，例如 rtsp或rtmp</param>
        /// <param name="vhost">虚拟主机，例如__defaultVhost__</param>
        /// <param name="app">应用名，例如 live</param>
        /// <param name="stream">流id，例如 test</param>
        /// <param name="force">是否强制关闭(有人在观看是否还关闭)</param>
        /// <returns>result 
        /// <para>0 成功</para>
        /// <para>-1 关闭失败</para>
        /// <para>-2 该流不存在</para>
        /// </returns>
        /// <remarks>已过期，请使用 <see cref="CloseStreams(string, string, string, string, bool)"/>接口替代</remarks>
        [Obsolete("已过期，请使用close_streams接口替换")]
        public async Task<IApiCloseStreamResult> CloseStream(string mediaServerId, string schema, string vhost, string app, string stream, bool force)
        {
            CreateClient(mediaServerId);
            return await CloseStream(schema, vhost, app, stream, force);
        }

        /// <summary>
        /// 关闭流(目前所有类型的流都支持关闭)
        /// </summary>
        /// <param name="schema">协议，例如 rtsp或rtmp</param>
        /// <param name="vhost">虚拟主机，例如__defaultVhost__</param>
        /// <param name="app">应用名，例如 live</param>
        /// <param name="stream">流id，例如 test</param>
        /// <param name="force">是否强制关闭(有人在观看是否还关闭)</param>
        /// <returns></returns>
        public async Task<IApiClonseStreamsResult> CloseStreams(string schema = null, string vhost = null, string app = null, string stream = null, bool force = false)
        {
            var request = new RestRequest("close_streams")
               .AddQueryParameter("schema", schema, true)
               .AddQueryParameter("vhost", vhost, true)
               .AddQueryParameter("app", app, true)
               .AddQueryParameter("stream", stream, true)
               .AddQueryParameter("force", force ? "1" : "0");
            return await (_restClient ?? CreateClient()).GetAsync<IApiClonseStreamsResult>(request);
        }

        /// <summary>
        /// 关闭流(目前所有类型的流都支持关闭)
        /// </summary>
        /// <param name="mediaServerId">协议，例如 rtsp或rtmp</param>
        /// <param name="schema">协议，例如 rtsp或rtmp</param>
        /// <param name="vhost">虚拟主机，例如__defaultVhost__</param>
        /// <param name="app">应用名，例如 live</param>
        /// <param name="stream">流id，例如 test</param>
        /// <param name="force">是否强制关闭(有人在观看是否还关闭)</param>
        /// <returns></returns>
        public async Task<IApiClonseStreamsResult> CloseStreams(string mediaServerId, string schema = null, string vhost = null, string app = null, string stream = null, bool force = false)
        {
            CreateClient(mediaServerId);
            return await CloseStreams(schema, vhost, app, stream, force);
        }

        /// <summary>
        /// 获取所有TcpSession列表(获取所有tcp客户端相关信息)
        /// </summary>
        /// <param name="local_port">筛选本机端口，例如筛选rtsp链接：554</param>
        /// <param name="peer_ip">筛选客户端ip</param>
        /// <returns></returns>
        public async Task<IApiGetAllSessionResult> GetAllSession(int? local_port = null, string peer_ip = null)
        {

            IRestRequest request = new RestRequest("getAllSession");
            if (local_port.HasValue) request = request.AddQueryParameter("local_port", local_port.Value.ToString());
            if (!string.IsNullOrEmpty(peer_ip)) request.AddQueryParameter("peer_ip", peer_ip);
            return await (_restClient ?? CreateClient()).GetAsync<IApiGetAllSessionResult>(request);
        }

        /// <summary>
        /// 获取所有TcpSession列表(获取所有tcp客户端相关信息)
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="local_port">筛选本机端口，例如筛选rtsp链接：554</param>
        /// <param name="peer_ip">筛选客户端ip</param>
        /// <returns></returns>
        public async Task<IApiGetAllSessionResult> GetAllSession(string mediaServerId, int? local_port = null, string peer_ip = null)
        {
            CreateClient(mediaServerId);
            return await GetAllSession(local_port, peer_ip);
        }

        /// <summary>
        /// 断开tcp连接，比如说可以断开rtsp、rtmp播放器等
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IApiResultBase> KickSession(string id)
        {
            IRestRequest request = new RestRequest("getAllSession")
                .AddQueryParameter("id", id);
            return await (_restClient ?? CreateClient()).GetAsync<IApiResultBase>(request);
        }

        /// <summary>
        /// 断开tcp连接，比如说可以断开rtsp、rtmp播放器等
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IApiResultBase> KickSession(string mediaServerId, string id)
        {
            CreateClient(mediaServerId);
            return await KickSession(id);
        }

        /// <summary>
        /// 断开tcp连接，比如说可以断开rtsp、rtmp播放器
        /// </summary>
        /// <param name="local_port"></param>
        /// <param name="peer_ip"></param>
        /// <returns></returns>
        public async Task<IApKillSessionsResult> KickSessions(int? local_port = null, string peer_ip = null)
        {
            IRestRequest request = new RestRequest("kick_sessions");
            if (local_port.HasValue) request = request.AddQueryParameter("local_port", local_port.Value.ToString());
            if (!string.IsNullOrEmpty(peer_ip)) request.AddQueryParameter("peer_ip", peer_ip);
            return await (_restClient ?? CreateClient()).GetAsync<IApKillSessionsResult>(request);

        }

        /// <summary>
        /// 断开tcp连接，比如说可以断开rtsp、rtmp播放器
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="local_port"></param>
        /// <param name="peer_ip"></param>
        /// <returns></returns>
        public async Task<IApKillSessionsResult> KickSessions(string mediaServerId, int? local_port = null, string peer_ip = null)
        {
            CreateClient(mediaServerId);
            return await KickSessions(local_port, peer_ip);
        }

        /// <summary>
        /// 动态添加rtsp/rtmp/hls拉流代理(只支持H264/H265/aac/G711负载)
        /// </summary>
        /// <param name="vhost">添加的流的虚拟主机，例如__defaultVhost__</param>
        /// <param name="app">添加的流的应用名，例如live</param>
        /// <param name="stream">添加的流的id名，例如test</param>
        /// <param name="url">拉流地址，例如rtmp://live.hkstv.hk.lxdns.com/live/hks2</param>
        /// <param name="enable_hls">是否转hls</param>
        /// <param name="enable_mp4">是否mp4录制</param>
        /// <param name="rtp_type">rtsp拉流时，拉流方式，0：tcp，1：udp，2：组播</param>
        /// <param name="timeout_sec">拉流超时时间，单位秒，float类型</param>
        /// <param name="retry_count">拉流重试次数,不传此参数或传值小于等于0时，则无限重试</param>
        /// <returns></returns>
        public async Task<IApiAddStreamPorxyResult> AddStreamProxy([NotNull] string vhost, [NotNull] string app, [NotNull] string stream, [NotNull] string url, bool? enable_hls, bool? enable_mp4, int? rtp_type, float? timeout_sec, int? retry_count)
        {
            IRestRequest request = new RestRequest("addStreamProxy")
                .AddQueryParameter("vhost", vhost)
                .AddQueryParameter("app", app)
                .AddQueryParameter("stream", stream)
                .AddQueryParameter("url", url);
            if (enable_hls.HasValue) request = request.AddQueryParameter("enable_hls", enable_hls == true ? "1" : "0");
            if (enable_mp4.HasValue) request = request.AddQueryParameter("enable_mp4", enable_mp4 == true ? "1" : "0");
            if (rtp_type.HasValue) request = request.AddQueryParameter("rtp_type", rtp_type.ToString());
            if (timeout_sec.HasValue) request = request.AddQueryParameter("timeout_sec", timeout_sec.ToString());
            if (retry_count.HasValue) request = request.AddQueryParameter("retry_count", retry_count.ToString());
            return await (_restClient ?? CreateClient()).GetAsync<IApiAddStreamPorxyResult>(request);

        }

        /// <summary>
        /// 动态添加rtsp/rtmp/hls拉流代理(只支持H264/H265/aac/G711负载)
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="vhost">添加的流的虚拟主机，例如__defaultVhost__</param>
        /// <param name="app">添加的流的应用名，例如live</param>
        /// <param name="stream">添加的流的id名，例如test</param>
        /// <param name="url">拉流地址，例如rtmp://live.hkstv.hk.lxdns.com/live/hks2</param>
        /// <param name="enable_hls">是否转hls</param>
        /// <param name="enable_mp4">是否mp4录制</param>
        /// <param name="rtp_type">rtsp拉流时，拉流方式，0：tcp，1：udp，2：组播</param>
        /// <param name="timeout_sec">拉流超时时间，单位秒，float类型</param>
        /// <param name="retry_count">拉流重试次数,不传此参数或传值小于等于0时，则无限重试</param>
        /// <returns></returns>
        public async Task<IApiAddStreamPorxyResult> AddStreamProxy([NotNull] string mediaServerId, [NotNull] string vhost, [NotNull] string app, [NotNull] string stream, [NotNull] string url, bool? enable_hls, bool? enable_mp4, int? rtp_type, float? timeout_sec, int? retry_count)
        {
            CreateClient(mediaServerId);
            return await AddStreamProxy(vhost, app, stream, url, enable_hls, enable_mp4, rtp_type, timeout_sec, retry_count);
        }

        /// <summary>
        /// 关闭拉流代理
        /// </summary>
        /// <param name="key">addStreamProxy接口返回的key</param>
        /// <returns></returns>
        /// <remarks>流注册成功后，也可以使用close_streams接口替代</remarks>
        public async Task<IApiDelStreamProxyResultItem> DelStreamProxy([NotNull] string key)
        {
            IRestRequest request = new RestRequest("delStreamProxy")
                .AddQueryParameter("key", key);
            return await (_restClient ?? CreateClient()).GetAsync<IApiDelStreamProxyResultItem>(request);
        }

        /// <summary>
        /// 关闭拉流代理
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="key">addStreamProxy接口返回的key</param>
        /// <returns></returns>
        /// <remarks>流注册成功后，也可以使用close_streams接口替代</remarks>
        public async Task<IApiDelStreamProxyResultItem> DelStreamProxy([NotNull] string mediaServerId, [NotNull] string key)
        {
            CreateClient(mediaServerId);
            return await DelStreamProxy(key);
        }



        /// <summary>
        /// 通过fork FFmpeg进程的方式拉流代理，支持任意协议
        /// </summary>
        /// <param name="src_url">FFmpeg拉流地址,支持任意协议或格式(只要FFmpeg支持即可)</param>
        /// <param name="dst_url">FFmpeg rtmp推流地址，一般都是推给自己，<para><c>例如:<b>rtmp://127.0.0.1/live/stream_form_ffmpeg</b></c></para></param>
        /// <param name="timeout_ms">FFmpeg推流成功超时时间</param>
        /// <param name="enable_hls">是否开启hls录制</param>
        /// <param name="enable_mp4">是否开启mp4录制</param>
        /// <param name="ffmpeg_cmd_key">FFmpeg命令参数模板，置空则采用配置项:ffmpeg.cmd</param>
        /// <returns></returns>
        public async Task<IApiAddFFmpegSourceResult> AddFFmpegSource([NotNull] string src_url, [NotNull] string dst_url, [NotNull] int timeout_ms = 5000, [NotNull] bool enable_hls = false, [NotNull] bool enable_mp4 = false, string ffmpeg_cmd_key = null)
        {
            IRestRequest request = new RestRequest("addFFmpegSource")
                .AddQueryParameter("src_url", src_url)
                .AddQueryParameter("dst_url", dst_url)
                .AddQueryParameter("timeout_ms", timeout_ms.ToString())
                .AddQueryParameter("enable_hls", enable_hls ? "1" : "0")
                .AddQueryParameter("enable_mp4", enable_mp4 ? "1" : "0");
            if (string.IsNullOrEmpty(ffmpeg_cmd_key)) request = request.AddQueryParameter("ffmpeg_cmd_key", ffmpeg_cmd_key);

            return await (_restClient ?? CreateClient()).GetAsync<IApiAddFFmpegSourceResult>(request);
        }

        /// <summary>
        /// 通过fork FFmpeg进程的方式拉流代理，支持任意协议
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="src_url">FFmpeg拉流地址,支持任意协议或格式(只要FFmpeg支持即可)</param>
        /// <param name="dst_url">FFmpeg rtmp推流地址，一般都是推给自己，<para><c>例如:<b>rtmp://127.0.0.1/live/stream_form_ffmpeg</b></c></para></param>
        /// <param name="timeout_ms">FFmpeg推流成功超时时间</param>
        /// <param name="enable_hls">是否开启hls录制</param>
        /// <param name="enable_mp4">是否开启mp4录制</param>
        /// <param name="ffmpeg_cmd_key">FFmpeg命令参数模板，置空则采用配置项:ffmpeg.cmd</param>
        /// <returns></returns>
        public async Task<IApiAddFFmpegSourceResult> AddFFmpegSource([NotNull] string mediaServerId, [NotNull] string src_url, [NotNull] string dst_url, [NotNull] int timeout_ms = 5000, [NotNull] bool enable_hls = false, [NotNull] bool enable_mp4 = false, string ffmpeg_cmd_key = null)
        {
            CreateClient(mediaServerId);
            return await AddFFmpegSource(src_url, dst_url, timeout_ms, enable_hls, enable_mp4, ffmpeg_cmd_key);
        }

        /// <summary>
        /// 添加rtsp/rtmp推流(addStreamPusherProxy)
        /// </summary>
        /// <param name="schema">推流协议，支持rtsp、rtmp，大小写敏感</param>
        /// <param name="vhost">已注册流的虚拟主机，一般为__defaultVhost__</param>
        /// <param name="app">已注册流的应用名，例如live</param>
        /// <param name="stream">已注册流的id名，例如test</param>
        /// <param name="dst_url">推流地址，需要与schema字段协议一致</param>
        /// <param name="rtp_type">rtsp推流时，推流方式，0：tcp，1：udp</param>
        /// <param name="timeout_sec">推流超时时间，单位秒，float类型</param>
        /// <param name="retry_count">推流重试次数,不传此参数或传值小于等于0时，则无限重试</param>
        /// <returns></returns>
        public async Task<IApiAddStreamPusherProxyResult> AddStreamPusherProxy([NotNull] string schema, [NotNull] string vhost, [NotNull] string app, [NotNull] string stream, [NotNull] string dst_url, int rtp_type = 0, int timeout_sec = 10, int retry_count = 0)
        {
            IRestRequest request = new RestRequest("addStreamPusherProxy")
                .AddQueryParameter("schema", schema)
                .AddQueryParameter("vhost", vhost)
                .AddQueryParameter("app", app)
                .AddQueryParameter("stream", stream)
                .AddQueryParameter("dst_url", dst_url)
                .AddQueryParameter("rtp_type", rtp_type.ToString())
                .AddQueryParameter("timeout_sec", timeout_sec.ToString())
                .AddQueryParameter("retry_count", retry_count.ToString());

            return await (_restClient ?? CreateClient()).GetAsync<IApiAddStreamPusherProxyResult>(request);
        }

        /// <summary>
        /// 添加rtsp/rtmp推流(addStreamPusherProxy)
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="schema">推流协议，支持rtsp、rtmp，大小写敏感</param>
        /// <param name="vhost">已注册流的虚拟主机，一般为__defaultVhost__</param>
        /// <param name="app">已注册流的应用名，例如live</param>
        /// <param name="stream">已注册流的id名，例如test</param>
        /// <param name="dst_url">推流地址，需要与schema字段协议一致</param>
        /// <param name="rtp_type">rtsp推流时，推流方式，0：tcp，1：udp</param>
        /// <param name="timeout_sec">推流超时时间，单位秒，float类型</param>
        /// <param name="retry_count">推流重试次数,不传此参数或传值小于等于0时，则无限重试</param>
        /// <returns></returns>
        public async Task<IApiAddStreamPusherProxyResult> AddStreamPusherProxy([NotNull] string mediaServerId, [NotNull] string schema, [NotNull] string vhost, [NotNull] string app, [NotNull] string stream, [NotNull] string dst_url, int rtp_type = 0, int timeout_sec = 10, int retry_count = 0)
        {
            CreateClient(mediaServerId);
            return await AddStreamPusherProxy(schema, vhost, app, stream, dst_url, rtp_type, timeout_sec, retry_count);
        }

        /// <summary>
        /// 关闭推流(delStreamPusherProxy)
        /// </summary>
        /// <param name="key">addStreamPusherProxy接口返回的key</param>
        /// <returns></returns>
        public async Task<IApiDelStreamPusherProxyResult> DelStreamPusherProxy([NotNull] string key)
        {
            IRestRequest request = new RestRequest("delStreamPusherProxy")
                .AddQueryParameter("key", key);
            return await (_restClient ?? CreateClient()).GetAsync<IApiDelStreamPusherProxyResult>(request);
        }

        /// <summary>
        /// 关闭推流(delStreamPusherProxy)
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="key">addStreamPusherProxy接口返回的key</param>
        /// <returns></returns>
        public async Task<IApiDelStreamPusherProxyResult> DelStreamPusherProxy([NotNull] string mediaServerId, [NotNull] string key)
        {
            CreateClient(mediaServerId);
            return await DelStreamPusherProxy(key);
        }


        /// <summary>
        /// 关闭ffmpeg拉流代理
        /// </summary>
        /// <param name="key">addStreamProxy接口返回的key</param>
        /// <returns></returns>
        /// <remarks>流注册成功后，也可以使用close_streams接口替代</remarks>
        public async Task<IApiDelFFmpegSourceResult> DelFFmpegSource([NotNull] string key)
        {
            IRestRequest request = new RestRequest("delFFmpegSource")
                .AddQueryParameter("key", key);
            return await (_restClient ?? CreateClient()).GetAsync<IApiDelFFmpegSourceResult>(request);
        }

        /// <summary>
        /// 关闭ffmpeg拉流代理
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="key">addStreamProxy接口返回的key</param>
        /// <returns></returns>
        /// <remarks>流注册成功后，也可以使用close_streams接口替代</remarks>
        public async Task<IApiDelFFmpegSourceResult> DelFFmpegSource([NotNull] string mediaServerId, [NotNull] string key)
        {
            CreateClient(mediaServerId);
            return await DelFFmpegSource(key);
        }

        /// <summary>
        /// 下载可执行文件
        /// </summary>
        /// <returns></returns>
        public byte[] DownloadBin()
        {
            IRestRequest request = new RestRequest("downloadBin");
            return (_restClient ?? CreateClient()).DownloadData(request);
        }
        /// <summary>
        /// 下载可执行文件
        /// </summary>
        /// <returns></returns>
        public byte[] DownloadBin([NotNull] string mediaServerId)
        {
            CreateClient(mediaServerId);
            return DownloadBin();
        }

        /// <summary>
        /// 判断直播流是否在线
        /// </summary>
        /// <param name="schema">协议，例如 rtsp或rtmp</param>
        /// <param name="vhost">虚拟主机，例如__defaultVhost__</param>
        /// <param name="app">应用名，例如 live</param>
        /// <param name="stream">流id，例如 obs</param>
        /// <returns></returns>
        /// <remarks>已过期，请使用 <see cref="GetMediaList(string, string, string)"/>接口替代</remarks>
        [Obsolete("已过期，请使用getMediaList接口替代")]
        public async Task<IApiIsMediaOnlineResult> IsMediaOnline([NotNull] string schema, [NotNull] string vhost, [NotNull] string app, [NotNull] string stream)
        {
            IRestRequest request = new RestRequest("isMediaOnline")
                .AddQueryParameter("schema", schema)
                .AddQueryParameter("vhost", vhost)
                .AddQueryParameter("app", app)
                .AddQueryParameter("stream", stream);

            return await (_restClient ?? CreateClient()).GetAsync<IApiIsMediaOnlineResult>(request);
        }

        /// <summary>
        /// 判断直播流是否在线
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="schema">协议，例如 rtsp或rtmp</param>
        /// <param name="vhost">虚拟主机，例如__defaultVhost__</param>
        /// <param name="app">应用名，例如 live</param>
        /// <param name="stream">流id，例如 obs</param>
        /// <returns></returns>
        /// <remarks>已过期，请使用 <see cref="GetMediaList(string, string, string)"/>接口替代</remarks>
        [Obsolete("已过期，请使用getMediaList接口替代")]
        public async Task<IApiIsMediaOnlineResult> IsMediaOnline([NotNull] string mediaServerId, [NotNull] string schema, [NotNull] string vhost, [NotNull] string app, [NotNull] string stream)
        {
            CreateClient(mediaServerId);
            return await IsMediaOnline(schema, vhost, app, stream);
        }



        /// <summary>
        /// 获取流相关信息
        /// </summary>
        /// <param name="schema">协议，例如 rtsp或rtmp</param>
        /// <param name="vhost">虚拟主机，例如__defaultVhost__</param>
        /// <param name="app">应用名，例如 live</param>
        /// <param name="stream">流id，例如 obs</param>
        /// <returns></returns>
        /// <remarks>已过期，请使用 <see cref="GetMediaList(string, string, string)"/>接口替代</remarks>
        [Obsolete("已过期，请使用getMediaList接口替代")]
        public async Task<IApiGetMediaInfo> GetMediaInfo([NotNull] string schema, [NotNull] string vhost, [NotNull] string app, [NotNull] string stream)
        {
            IRestRequest request = new RestRequest("getMediaInfo")
                 .AddQueryParameter("schema", schema)
                .AddQueryParameter("vhost", vhost)
                .AddQueryParameter("app", app)
                .AddQueryParameter("stream", stream);
            return await (_restClient ?? CreateClient()).GetAsync<IApiGetMediaInfo>(request);
        }

        /// <summary>
        /// 获取流相关信息
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="schema">协议，例如 rtsp或rtmp</param>
        /// <param name="vhost">虚拟主机，例如__defaultVhost__</param>
        /// <param name="app">应用名，例如 live</param>
        /// <param name="stream">流id，例如 obs</param>
        /// <returns></returns>
        /// <remarks>已过期，请使用 <see cref="GetMediaList(string, string, string)"/>接口替代</remarks>
        [Obsolete("已过期，请使用getMediaList接口替代")]
        public async Task<IApiGetMediaInfo> GetMediaInfo([NotNull] string mediaServerId, [NotNull] string schema, [NotNull] string vhost, [NotNull] string app, [NotNull] string stream)
        {
            CreateClient(mediaServerId);
            return await GetMediaInfo(schema, vhost, app, stream);
        }


        /// <summary>
        /// 获取rtp代理时的某路ssrc rtp信息
        /// </summary>
        /// <param name="stream_id">RTP的ssrc，16进制字符串或者是流的id(openRtpServer接口指定)</param>
        /// <returns></returns>
        public async Task<IApiGetRtpInfoResult> GetRtpInfo([NotNull] string stream_id)
        {
            IRestRequest request = new RestRequest("getRtpInfo")
                .AddQueryParameter("stream_id", stream_id);

            return await (_restClient ?? CreateClient()).GetAsync<IApiGetRtpInfoResult>(request);
        }

        /// <summary>
        /// 获取rtp代理时的某路ssrc rtp信息
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="stream_id">RTP的ssrc，16进制字符串或者是流的id(openRtpServer接口指定)</param>
        /// <returns></returns>
        public async Task<IApiGetRtpInfoResult> GetRtpInfo([NotNull] string mediaServerId, [NotNull] string stream_id)
        {
            CreateClient(mediaServerId);
            return await GetRtpInfo(stream_id);
        }

        /// <summary>
        /// 搜索文件系统，获取流对应的录像文件列表或日期文件夹列表
        /// </summary>
        /// <param name="vhost">流的虚拟主机名</param>
        /// <param name="app">流的应用名</param>
        /// <param name="stream">流的ID</param>
        /// <param name="period">流的录像日期，格式为2020-02-01,如果不是完整的日期，那么是搜索录像文件夹列表，否则搜索对应日期下的mp4文件列表</param>
        /// <returns></returns>
        public async Task<IApiGetMp4RecordFileResult> GetMp4RecordFile([NotNull] string vhost, [NotNull] string app, [NotNull] string stream, [NotNull] string period)
        {
            IRestRequest request = new RestRequest("getMp4RecordFile")
                .AddQueryParameter("vhost", vhost)
                .AddQueryParameter("app", app)
                .AddQueryParameter("stream", stream)
                .AddQueryParameter("period", period);
            return await (_restClient ?? CreateClient()).GetAsync<IApiGetMp4RecordFileResult>(request);
        }

        /// <summary>
        /// 搜索文件系统，获取流对应的录像文件列表或日期文件夹列表
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="vhost">流的虚拟主机名</param>
        /// <param name="app">流的应用名</param>
        /// <param name="stream">流的ID</param>
        /// <param name="period">流的录像日期，格式为2020-02-01,如果不是完整的日期，那么是搜索录像文件夹列表，否则搜索对应日期下的mp4文件列表</param>
        /// <returns></returns>
        public async Task<IApiGetMp4RecordFileResult> GetMp4RecordFile([NotNull] string mediaServerId, [NotNull] string vhost, [NotNull] string app, [NotNull] string stream, [NotNull] string period)
        {
            CreateClient(mediaServerId);
            return await GetMp4RecordFile(vhost, app, stream, period);
        }

        /// <summary>
        /// 开始录制hls或MP4
        /// </summary>
        /// <param name="type">0为hls，1为mp4</param>
        /// <param name="vhost">流的虚拟主机名</param>
        /// <param name="app">流的应用名</param>
        /// <param name="stream">流的ID</param>
        /// <param name="customized_path">录像保存目录</param>
        /// <param name="max_second">mp4录像切片时间大小,单位秒，置0则采用配置项</param>
        /// <returns></returns>
        public async Task<IApiStartRecordResult> StartRecord([NotNull] int type, [NotNull] string vhost, [NotNull] string app, [NotNull] string stream, string customized_path, int? max_second = 0)
        {

            IRestRequest request = new RestRequest("startRecord")
                .AddQueryParameter("type", type.ToString())
                .AddQueryParameter("vhost", vhost)
                .AddQueryParameter("app", app)
                .AddQueryParameter("stream", stream);
            if (!string.IsNullOrEmpty(customized_path)) request = request.AddQueryParameter("customized_path", customized_path);
            if (max_second.HasValue) request = request.AddQueryParameter("max_second", max_second.ToString());
            return await (_restClient ?? CreateClient()).GetAsync<IApiStartRecordResult>(request);
        }



        /// <summary>
        /// 开始录制hls或MP4
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="type">0为hls，1为mp4</param>
        /// <param name="vhost">流的虚拟主机名</param>
        /// <param name="app">流的应用名</param>
        /// <param name="stream">流的ID</param>
        /// <param name="customized_path">录像保存目录</param>
        /// <param name="max_second">mp4录像切片时间大小,单位秒，置0则采用配置项</param>
        /// <returns></returns>
        public async Task<IApiStartRecordResult> StartRecord([NotNull] string mediaServerId, [NotNull] int type, [NotNull] string vhost, [NotNull] string app, [NotNull] string stream, string customized_path, int? max_second = 0)
        {
            CreateClient(mediaServerId);
            return await StartRecord(type, vhost, app, stream, customized_path, max_second);
        }

        /// <summary>
        /// 设置录像流播放速度
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="vhost"></param>
        /// <param name="app"></param>
        /// <param name="stream"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        public async Task<IApiSetRecordSpeedResult> StartRecordSpeed([NotNull] string schema, [NotNull] string vhost, [NotNull] string app, [NotNull] string stream, float speed)
        {
            IRestRequest request = new RestRequest("setRecordSpeed")
                .AddQueryParameter("schema", schema)
                .AddQueryParameter("vhost", vhost)
                .AddQueryParameter("app", app)
                .AddQueryParameter("stream", stream)
                .AddQueryParameter("speed", speed.ToString());
            return await (_restClient ?? CreateClient()).GetAsync<IApiSetRecordSpeedResult>(request);
        }

        /// <summary>
        /// 设置录像流播放速度
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="schema"></param>
        /// <param name="vhost"></param>
        /// <param name="app"></param>
        /// <param name="stream"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        public async Task<IApiSetRecordSpeedResult> StartRecordSpeed([NotNull] string mediaServerId, [NotNull] string schema, [NotNull] string vhost, [NotNull] string app, [NotNull] string stream, float speed)
        {
            CreateClient(mediaServerId);
            return await StartRecordSpeed(schema, vhost, app, stream, speed);
        }

        /// <summary>
        /// 跳转到指定位置
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="vhost"></param>
        /// <param name="app"></param>
        /// <param name="stream"></param>
        /// <param name="stamp"></param>
        /// <returns></returns>
        public async Task<IApiSeekRecordStampResult> SeekRecordStamp([NotNull] string schema, [NotNull] string vhost, [NotNull] string app, [NotNull] string stream, long stamp)
        {
            IRestRequest request = new RestRequest("seekRecordStamp")
                .AddQueryParameter("schema", schema)
                .AddQueryParameter("vhost", vhost)
                .AddQueryParameter("app", app)
                .AddQueryParameter("stream", stream)
                .AddQueryParameter("stamp", stamp.ToString());
            return await (_restClient ?? CreateClient()).GetAsync<IApiSeekRecordStampResult>(request);
        }

        /// <summary>
        /// 跳转到指定位置
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="schema"></param>
        /// <param name="vhost"></param>
        /// <param name="app"></param>
        /// <param name="stream"></param>
        /// <param name="stamp"></param>
        /// <returns></returns>
        public async Task<IApiSeekRecordStampResult> SeekRecordStamp([NotNull] string mediaServerId, [NotNull] string schema, [NotNull] string vhost, [NotNull] string app, [NotNull] string stream, long stamp)
        {
            CreateClient(mediaServerId);
            return await SeekRecordStamp(schema, vhost, app, stream, stamp);
        }

        /// <summary>
        /// 开始录制hls或MP4
        /// </summary>
        /// <param name="type">0为hls，1为mp4</param>
        /// <param name="vhost">流的虚拟主机名</param>
        /// <param name="app">流的应用名</param>
        /// <param name="stream">流的ID</param>
        /// <returns></returns>
        public async Task<IApiStopRecordResult> StopRecord([NotNull] int type, [NotNull] string vhost, [NotNull] string app, [NotNull] string stream)
        {
            IRestRequest request = new RestRequest("stopRecord")
                .AddQueryParameter("type", type.ToString())
                .AddQueryParameter("vhost", vhost)
                .AddQueryParameter("app", app)
                .AddQueryParameter("stream", stream);
            return await (_restClient ?? CreateClient()).GetAsync<IApiStopRecordResult>(request);
        }

        /// <summary>
        /// 开始录制hls或MP4
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="type">0为hls，1为mp4</param>
        /// <param name="vhost">流的虚拟主机名</param>
        /// <param name="app">流的应用名</param>
        /// <param name="stream">流的ID</param>
        /// <returns></returns>
        public async Task<IApiStopRecordResult> StopRecord([NotNull] string mediaServerId, [NotNull] int type, [NotNull] string vhost, [NotNull] string app, [NotNull] string stream)
        {
            CreateClient(mediaServerId);
            return await StopRecord(type, vhost, app, stream);
        }

        /// <summary>
        /// 获取流录制状态
        /// </summary>
        /// <param name="type">0为hls，1为mp4</param>
        /// <param name="vhost">流的虚拟主机名</param>
        /// <param name="app">流的应用名</param>
        /// <param name="stream">流的ID</param>
        /// <returns></returns>
        public async Task<IApiIsRecordingResult> IsRecording([NotNull] int type, [NotNull] string vhost, [NotNull] string app, [NotNull] string stream)
        {
            IRestRequest request = new RestRequest("isRecording")
                .AddQueryParameter("type", type.ToString())
                .AddQueryParameter("vhost", vhost)
                .AddQueryParameter("app", app)
                .AddQueryParameter("stream", stream);
            return await (_restClient ?? CreateClient()).GetAsync<IApiIsRecordingResult>(request);

        }

        /// <summary>
        /// 获取流录制状态
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="type">0为hls，1为mp4</param>
        /// <param name="vhost">流的虚拟主机名</param>
        /// <param name="app">流的应用名</param>
        /// <param name="stream">流的ID</param>
        /// <returns></returns>
        public async Task<IApiIsRecordingResult> IsRecording([NotNull] string mediaServerId, [NotNull] int type, [NotNull] string vhost, [NotNull] string app, [NotNull] string stream)
        {
            CreateClient(mediaServerId);
            return await IsRecording(type, vhost, app, stream);
        }

        /// <summary>
        /// 获取截图或生成实时截图并返回
        /// </summary>
        /// <param name="url">需要截图的url，可以是本机的，也可以是远程主机的</param>
        /// <param name="timeout_sec">截图失败超时时间，防止FFmpeg一直等待截图</param>
        /// <param name="expire_sec">截图的过期时间，该时间内产生的截图都会作为缓存返回</param>
        /// <returns>jpeg格式的图片，可以在浏览器直接打开</returns>
        public byte[] GetSnap([NotNull] string url, int timeout_sec = 5, int expire_sec = 5)
        {
            IRestRequest request = new RestRequest("getSnap")
                .AddQueryParameter("url", url)
                .AddQueryParameter("timeout_sec", timeout_sec.ToString())
                .AddQueryParameter("expire_sec", expire_sec.ToString());
            return (_restClient ?? CreateClient()).DownloadData(request);
        }

        /// <summary>
        /// 获取截图或生成实时截图并返回
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="url">需要截图的url，可以是本机的，也可以是远程主机的</param>
        /// <param name="timeout_sec">截图失败超时时间，防止FFmpeg一直等待截图</param>
        /// <param name="expire_sec">截图的过期时间，该时间内产生的截图都会作为缓存返回</param>
        /// <returns>jpeg格式的图片，可以在浏览器直接打开</returns>
        public byte[] GetSnap([NotNull] string mediaServerId, [NotNull] string url, int timeout_sec = 5, int expire_sec = 5)
        {
            CreateClient(mediaServerId);
            return GetSnap(url, timeout_sec, expire_sec);
        }

        /// <summary>
        /// 创建GB28181 RTP接收端口，如果该端口接收数据超时，则会自动被回收(不用调用closeRtpServer接口)
        /// </summary>
        /// <param name="port">接收端口，0则为随机端口</param>
        /// <param name="enable_tcp">启用UDP监听的同时是否监听TCP端口</param>
        /// <param name="stream_id">该端口绑定的流ID，该端口只能创建这一个流(而不是根据ssrc创建多个)</param>
        /// <returns></returns>
        public async Task<IApiOpenRtpServerResult> OpenRtpServer(int port = 0, bool enable_tcp = false, string stream_id = null)
        {
            IRestRequest request = new RestRequest("openRtpServer")
                .AddQueryParameter("port", port.ToString())
                .AddQueryParameter("enable_tcp", enable_tcp.ToString())
                .AddQueryParameter("stream_id", stream_id);

            return await (_restClient ?? CreateClient()).GetAsync<IApiOpenRtpServerResult>(request);
        }

        /// <summary>
        /// 创建GB28181 RTP接收端口，如果该端口接收数据超时，则会自动被回收(不用调用closeRtpServer接口)
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="port">接收端口，0则为随机端口</param>
        /// <param name="enable_tcp">启用UDP监听的同时是否监听TCP端口</param>
        /// <param name="stream_id">该端口绑定的流ID，该端口只能创建这一个流(而不是根据ssrc创建多个)</param>
        /// <returns></returns>
        public async Task<IApiOpenRtpServerResult> OpenRtpServer([NotNull] string mediaServerId, int port = 0, bool enable_tcp = false, string stream_id = null)
        {
            CreateClient(mediaServerId);
            return await OpenRtpServer(port, enable_tcp, stream_id);

        }

        /// <summary>
        /// 关闭GB28181 RTP接收端口
        /// </summary>
        /// <param name="stream_id"></param>
        /// <returns></returns>
        public async Task<IApiCloseRtpServerResult> CloseRtpServer([NotNull] string stream_id)
        {
            IRestRequest request = new RestRequest("closeRtpServer")
               .AddQueryParameter("stream_id", stream_id);

            return await (_restClient ?? CreateClient()).GetAsync<IApiCloseRtpServerResult>(request);
        }

        /// <summary>
        /// 关闭GB28181 RTP接收端口
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="stream_id"></param>
        /// <returns></returns>
        public async Task<IApiCloseRtpServerResult> CloseRtpServer([NotNull] string mediaServerId, [NotNull] string stream_id)
        {
            CreateClient(mediaServerId);
            return await CloseRtpServer(stream_id);
        }

        /// <summary>
        /// 获取openRtpServer接口创建的所有RTP服务器
        /// </summary>
        /// <returns></returns>
        public async Task<IApiListRtpServerResult> ListRtpServer()
        {
            IRestRequest request = new RestRequest("listRtpServer");
            return await (_restClient ?? CreateClient()).GetAsync<IApiListRtpServerResult>(request);
        }

        /// <summary>
        /// 获取openRtpServer接口创建的所有RTP服务器
        /// </summary>
        /// <returns></returns>
        public async Task<IApiListRtpServerResult> ListRtpServer([NotNull] string mediaServerId)
        {
            CreateClient(mediaServerId);
            return await ListRtpServer();
        }

        /// <summary>
        /// 作为GB28181客户端，启动ps-rtp推流，支持rtp/udp方式；该接口支持rtsp/rtmp等协议转ps-rtp推流。第一次推流失败会直接返回错误，成功一次后，后续失败也将无限重试
        /// </summary>
        /// <param name="vhost">虚拟主机，例如__defaultVhost__</param>
        /// <param name="app">应用名，例如 live</param>
        /// <param name="stream">流id，例如 test</param>
        /// <param name="ssrc">推流的rtp的ssrc</param>
        /// <param name="dst_url">目标ip或域名</param>
        /// <param name="dst_port">目标端口</param>
        /// <param name="is_udp">是否为udp模式,否则为tcp模式</param>
        /// <param name="src_port">使用的本机端口，为0或不传时默认为随机端口</param>
        /// <returns></returns>
        public async Task<IApiStartSendRtpResult> StartSendRtp([NotNull] string vhost, [NotNull] string app, [NotNull] string stream, [NotNull] string ssrc, [NotNull] string dst_url, int dst_port, bool is_udp, int src_port = 0)
        {
            IRestRequest request = new RestRequest("startSendRtp")
                .AddQueryParameter("vhost", vhost)
                .AddQueryParameter("app	", app)
                .AddQueryParameter("stream", stream)
                .AddQueryParameter("ssrc	", ssrc)
                .AddQueryParameter("dst_url", dst_url)
                .AddQueryParameter("dst_port", dst_port.ToString())
                .AddQueryParameter("is_udp", is_udp ? "1" : "0")
                .AddQueryParameter("src_port", src_port.ToString());
            return await (_restClient ?? CreateClient()).GetAsync<IApiStartSendRtpResult>(request);
        }

        /// <summary>
        /// 作为GB28181客户端，启动ps-rtp推流，支持rtp/udp方式；该接口支持rtsp/rtmp等协议转ps-rtp推流。第一次推流失败会直接返回错误，成功一次后，后续失败也将无限重试
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="vhost">虚拟主机，例如__defaultVhost__</param>
        /// <param name="app">应用名，例如 live</param>
        /// <param name="stream">流id，例如 test</param>
        /// <param name="ssrc">推流的rtp的ssrc</param>
        /// <param name="dst_url">目标ip或域名</param>
        /// <param name="dst_port">目标端口</param>
        /// <param name="is_udp">是否为udp模式,否则为tcp模式</param>
        /// <param name="src_port">使用的本机端口，为0或不传时默认为随机端口</param>
        /// <returns></returns>
        public async Task<IApiStartSendRtpResult> StartSendRtp([NotNull] string mediaServerId, [NotNull] string vhost, [NotNull] string app, [NotNull] string stream, [NotNull] string ssrc, [NotNull] string dst_url, int dst_port, bool is_udp, int src_port = 0)
        {
            CreateClient(mediaServerId);
            return await StartSendRtp(vhost, app, stream, ssrc, dst_url, dst_port, is_udp, src_port);
        }

        /// <summary>
        /// 停止GB28181 ps-rtp推流
        /// </summary>
        /// <param name="vhost">虚拟主机，例如__defaultVhost__</param>
        /// <param name="app">应用名，例如 live</param>
        /// <param name="stream">流id，例如 test</param>
        /// <param name="ssrc"></param>
        /// <returns></returns>
        public async Task<IApiStopSendRtpResult> StopSendRtp([NotNull] string vhost, [NotNull] string app, [NotNull] string stream, string ssrc = null)
        {
            IRestRequest request = new RestRequest("stopSendRtp")
                .AddQueryParameter("vhost", vhost)
                .AddQueryParameter("app	", app)
                .AddQueryParameter("stream", stream);
            if (!string.IsNullOrEmpty(ssrc)) request = request.AddQueryParameter("ssrc", ssrc);
            return await (_restClient ?? CreateClient()).GetAsync<IApiStopSendRtpResult>(request);
        }

        /// <summary>
        /// 停止GB28181 ps-rtp推流
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="vhost">虚拟主机，例如__defaultVhost__</param>
        /// <param name="app">应用名，例如 live</param>
        /// <param name="stream">流id，例如 test</param>
        /// <param name="ssrc"></param>
        /// <returns></returns>
        public async Task<IApiStopSendRtpResult> StopSendRtp([NotNull] string mediaServerId, [NotNull] string vhost, [NotNull] string app, [NotNull] string stream, string ssrc = null)
        {
            CreateClient(mediaServerId);
            return await StopSendRtp(vhost, app, stream, ssrc);
        }

        /// <summary>
        /// 停止RTP代理超时检查
        /// </summary>
        /// <param name="streamid"></param>
        /// <returns></returns>
        public async Task<IApiPauseRtpCheckResult> PauseRtpCheck([NotNull] string streamid)
        {
            IRestRequest request = new RestRequest("pauseRtpCheck")
                .AddQueryParameter("stream_id", streamid);
            return await (_restClient ?? CreateClient()).GetAsync<IApiPauseRtpCheckResult>(request);
        }

        /// <summary>
        /// 停止RTP代理超时检查
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="streamid"></param>
        /// <returns></returns>
        public async Task<IApiPauseRtpCheckResult> PauseRtpCheck([NotNull] string mediaServerId, [NotNull] string streamid)
        {
            CreateClient(mediaServerId);
            return await PauseRtpCheck(streamid);
        }

        /// <summary>
        /// 恢复RTP代理超时检查
        /// </summary>
        /// <param name="streamid"></param>
        /// <returns></returns>
        public async Task<IApiResumeRtpCheckResult> ResumeRtpCheck([NotNull] string streamid)
        {
            IRestRequest request = new RestRequest("resumeRtpCheck")
                .AddQueryParameter("stream_id", streamid);
            return await (_restClient ?? CreateClient()).GetAsync<IApiResumeRtpCheckResult>(request);
        }
        /// <summary>
        /// 恢复RTP代理超时检查
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="streamid"></param>
        /// <returns></returns>
        public async Task<IApiResumeRtpCheckResult> ResumeRtpCheck([NotNull] string mediaServerId, [NotNull] string streamid)
        {
            CreateClient(mediaServerId);
            return await ResumeRtpCheck(streamid);
        }



        /// <summary>
        /// 获取ZLMediaKit对象统计
        /// </summary>
        /// <returns></returns>
        public async Task<IApiGetStatisticResult> GetStatistic()
        {
            IRestRequest request = new RestRequest("getStatistic");
            return await (_restClient ?? CreateClient()).GetAsync<IApiGetStatisticResult>(request);
        }

        /// <summary>
        /// 获取ZLMediaKit对象统计
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <returns></returns>
        public async Task<IApiGetStatisticResult> GetStatistic([NotNull] string mediaServerId)
        {
            CreateClient(mediaServerId);
            return await GetStatistic();
        }
        /// <summary>
        /// 根据方法名调用接口获取数据
        /// </summary>
        /// <param name="methodName">方法名词</param>
        /// <param name="paras">请求参数</param>
        /// <typeparam name="T">返回参数</typeparam>
        /// <returns></returns>
        public async Task<T> GetByMethod<T>(string methodName, Dictionary<string, string> paras)
        {

            IRestRequest request = new RestRequest(methodName);
            if (paras != null && paras.Count > 0)
            {

                request = request.AddOrUpdateParameters(paras.Select(s => new RestSharp.Parameter(s.Key, s.Value, ParameterType.QueryStringWithoutEncode)));
            }
            return await (_restClient ?? CreateClient()).GetAsync<T>(request);
        }

        /// <summary>
        /// 根据方法名调用接口获取数据
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="methodName">方法名词</param>
        /// <param name="paras">请求参数</param>
        /// <typeparam name="T">返回参数</typeparam>
        /// <returns></returns>
        public async Task<T> GetByMethod<T>([NotNull] string mediaServerId, string methodName, Dictionary<string, string> paras)
        {
            CreateClient(mediaServerId);
            return await GetByMethod<T>(methodName, paras);
        }
    }
}
