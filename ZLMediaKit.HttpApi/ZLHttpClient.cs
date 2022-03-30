using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using RestSharp;
using ZLMediaKit.Common;
using ZLMediaKit.Common.Dtos.ApiInputDto;
using ZLMediaKit.Common.Dtos;
using System.Dynamic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using RestSharp.Serializers.Json;

namespace ZLMediaKit.HttpApi
{
    /// <summary>
    /// 
    /// </summary>
    public static class ZLHttpClient
    {
        //private static static ZLHttpClient _instance;

        ///// <summary>
        ///// 单例
        ///// </summary>
        //public static static ZLHttpClient Instance { get { 
        //    if (_instance == null) _instance = new ZLHttpClient();
        //    return _instance;
        //    } } 

        ///// <summary>
        ///// 
        ///// </summary>
        //public static ZLHttpClient()
        //{

        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="mediaServerId"></param>
        //public static ZLHttpClient([NotNull] string mediaServerId) => CreateClient(mediaServerId);

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="serverManager"></param>
        //public static ZLHttpClient([NotNull] IServerManager serverManager) => CreateClient(serverManager);


        //private static IServerManager _serverManager;

        private static Dictionary<string, RestClient> _restClientDict = new Dictionary<string, RestClient>();


        private static RestClient CreateClient(string mediaserverId) => CreateClient(IServerManager.GetServerManager(mediaserverId));

        private static RestClient CreateClient(IServerManager serverManager)
        {
            serverManager = serverManager ?? IServerManager.GetDefaultServerManager();
            if (serverManager == null) throw new ArgumentNullException(nameof(serverManager));
            if (_restClientDict.TryGetValue(serverManager.MediaServerId,out var client))
            {
                return client;
            }

            client = new RestClient( new RestClientOptions(serverManager.ApiBaseUri) { 
                Timeout = serverManager.Timeout,
                
            });
            client.UseSystemTextJson(TypeMapping.SerializerOptions);
            client.AddDefaultQueryParameter("secret", serverManager.ApiSecret);
            return client;
        }

        private static RestClient CreateClient() => CreateClient(IServerManager.GetDefaultServerManager());

        /// <summary>
        /// 在多ZLMediaKit部署模式下，调用ZLM接口前，应先调用此方法设置具体的ZLMediaKit
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <returns></returns>
        public static RestClient SetMediaServerId(string mediaServerId)
        {
            return CreateClient(mediaServerId);
            //if (string.IsNullOrEmpty(mediaServerId)) CreateClient();
            //CreateClient(mediaServerId);
            //return this;
        }

        /// <summary>
        /// 在多ZLMediaKit部署模式下，调用ZLM接口前，应先调用此方法设置具体的ZLMediaKit
        /// </summary>
        /// <param name="serverManager"></param>
        /// <returns></returns>
        public static RestClient SetMediaServerId(IServerManager serverManager)
        {
            return CreateClient(serverManager);
        }


        /// <summary>
        /// 获取各epoll(或select)线程负载以及延时
        /// </summary>
        /// <returns></returns>
        public static async Task<IThreadsLoadApiResult> GetThreadsLoad(this RestClient client)
        {
            var request = new RestRequest("getThreadsLoad");
            return await client.GetAsync<IThreadsLoadApiResult>(request);
        }


        /// <summary>
        /// 获取各epoll(或select)线程负载以及延时
        /// </summary>
        /// <returns></returns>
        public static async Task<IThreadsLoadApiResult> GetThreadsLoad(string mediaServerId)
        {
            return await CreateClient(mediaServerId).GetThreadsLoad();
        }

        /// <summary>
        /// 获取各后台epoll(或select)线程负载以及延时
        /// </summary>
        /// <returns></returns>
        public static async Task<IWorkThreadsLoadApiResult> GetWorkThreadsLoad(this RestClient client)
        {
            var request = new RestRequest("getWorkThreadsLoad");
            return await client.GetAsync<IWorkThreadsLoadApiResult>(request);
        }

        /// <summary>
        /// 获取各后台epoll(或select)线程负载以及延时
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <returns></returns>
        public static async Task<IWorkThreadsLoadApiResult> GetWorkThreadsLoad(string mediaServerId) => await CreateClient(mediaServerId).GetWorkThreadsLoad();

        /// <summary>
        /// 获取服务器配置
        /// </summary>
        /// <returns></returns>
        public static async Task<IApiGetServerConfigResult> GetServerConfig(this RestClient client, string MediaServerId = null)
        {
            var request = new RestRequest("getServerConfig");
            var result = await client.GetAsync<ApiResultBase<List<Dictionary<string,string>>>>(request);

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
                if (!string.IsNullOrEmpty(MediaServerId) && IServerManager.Instances.TryGetValue(MediaServerId, out var serverManager))
                {
                    if (serverManager.MediaServerId != serverCofnig.General.MediaServerId)
                    {
                        IServerManager.RemoveServer(serverManager);
                    }
                    serverManager.MediaServerId = serverCofnig.General.MediaServerId;
                    serverManager.ServerConfig = serverCofnig;
                    IServerManager.AddServer(serverManager);
                }
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
        public static async Task<IApiGetServerConfigResult> GetServerConfig(string mediaServerId) => await CreateClient(mediaServerId).GetServerConfig();

        /// <summary>
        /// 动态修改ZLM配置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static async Task<ISetServerConfigResult> SetServerConfig(this RestClient client, params ISetServerConfigInput[] input)
        {
            var request = new RestRequest("setServerConfig");

            request.AddOrUpdateParameters(input?.Select(s=> Parameter.CreateParameter($"{s.ClassName}.{s.Key}",s.Value, ParameterType.QueryString)));

            //_ =  GetServerConfig(client).ConfigureAwait(false);

            return await client.GetAsync<ISetServerConfigResult>(request);
        }

        /// <summary>
        /// 动态修改ZLM配置
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static async Task<ISetServerConfigResult> SetServerConfig(string mediaServerId, params ISetServerConfigInput[] input)
        {
            return await CreateClient(mediaServerId).SetServerConfig();
        }


        /// <summary>
        /// 启服务器,只有Daemon方式才能重启，否则是直接关闭！
        /// </summary>
        /// <returns></returns>
        public static async Task<IApiRestartServerResult> RestartServer(this RestClient client)
        {
            var request = new RestRequest("restartServer");
            return await client.GetAsync<IApiRestartServerResult>(request);
        }

        /// <summary>
        /// 启服务器,只有Daemon方式才能重启，否则是直接关闭！
        /// </summary>
        /// <returns></returns>
        public static async Task<IApiRestartServerResult> RestartServer(string mediaServerId)
        {
            return await CreateClient(mediaServerId).RestartServer();
        }

        /// <summary>
        /// 获取流列表，可选筛选参数
        /// </summary>
        /// <param name="schema">筛选协议，例如 rtsp或rtmp</param>
        /// <param name="vhost">筛选虚拟主机，例如__defaultVhost__</param>
        /// <param name="app">筛选应用名，例如 live</param>
        /// <returns></returns>
        public static async Task<IApiGetMediaListResult> GetMediaList(this RestClient client,string schema = null, string vhost = null, string app = null)
        {
            var request = new RestRequest("getMediaList")
                .AddQueryParameter("schema", schema, true)
                .AddQueryParameter("vhost", vhost, true)
                .AddQueryParameter("app", app, true);
            return await client.GetAsync<IApiGetMediaListResult>(request);
        }

        /// <summary>
        /// 获取流列表，可选筛选参数
        /// </summary>
        /// <param name="schema">筛选协议，例如 rtsp或rtmp</param>
        /// <param name="vhost">筛选虚拟主机，例如__defaultVhost__</param>
        /// <param name="app">筛选应用名，例如 live</param>
        /// <returns></returns>
        public static async Task<IApiGetMediaListResult> GetMediaList(string mediaServerId, string schema = null, string vhost = null, string app = null)
        {
            var result = await CreateClient(mediaServerId).GetMediaList(schema, vhost, app);
            return result;
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
        public static async Task<IApiCloseStreamResult> CloseStream(this RestClient client,string schema, string vhost, string app, string stream, bool force)
        {
            var request = new RestRequest("close_stream")
                .AddQueryParameter("schema", schema, true)
                .AddQueryParameter("vhost", vhost, true)
                .AddQueryParameter("app", app, true)
                .AddQueryParameter("stream", stream, true)
                .AddQueryParameter("force", force ? "1" : "0");
            return await client.GetAsync<IApiCloseStreamResult>(request);
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
        public static async Task<IApiCloseStreamResult> CloseStream(string mediaServerId, string schema, string vhost, string app, string stream, bool force)
        {
            return await CreateClient(mediaServerId).CloseStream(schema, vhost, app, stream, force);
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
        public static async Task<IApiClonseStreamsResult> CloseStreams(this RestClient client, string schema = null, string vhost = null, string app = null, string stream = null, bool force = false)
        {
            var request = new RestRequest("close_streams")
               .AddQueryParameter("schema", schema, true)
               .AddQueryParameter("vhost", vhost, true)
               .AddQueryParameter("app", app, true)
               .AddQueryParameter("stream", stream, true)
               .AddQueryParameter("force", force ? "1" : "0");
            return await client.GetAsync<IApiClonseStreamsResult>(request);
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
        public static async Task<IApiClonseStreamsResult> CloseStreams(string mediaServerId, string schema = null, string vhost = null, string app = null, string stream = null, bool force = false)
        {
            return await CreateClient(mediaServerId).CloseStreams(schema, vhost, app, stream, force);
        }

        /// <summary>
        /// 获取所有TcpSession列表(获取所有tcp客户端相关信息)
        /// </summary>
        /// <param name="local_port">筛选本机端口，例如筛选rtsp链接：554</param>
        /// <param name="peer_ip">筛选客户端ip</param>
        /// <returns></returns>
        public static async Task<IApiGetAllSessionResult> GetAllSession(this RestClient client, int? local_port = null, string peer_ip = null)
        {

            var request = new RestRequest("getAllSession");
            if (local_port.HasValue) request = request.AddQueryParameter("local_port", local_port.Value.ToString());
            if (!string.IsNullOrEmpty(peer_ip)) request.AddQueryParameter("peer_ip", peer_ip);
            return await client.GetAsync<IApiGetAllSessionResult>(request);
        }

        /// <summary>
        /// 获取所有TcpSession列表(获取所有tcp客户端相关信息)
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="local_port">筛选本机端口，例如筛选rtsp链接：554</param>
        /// <param name="peer_ip">筛选客户端ip</param>
        /// <returns></returns>
        public static async Task<IApiGetAllSessionResult> GetAllSession(string mediaServerId, int? local_port = null, string peer_ip = null)
        {
            return await CreateClient(mediaServerId).GetAllSession(local_port, peer_ip);
        }

        /// <summary>
        /// 断开tcp连接，比如说可以断开rtsp、rtmp播放器等
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<IApiResultBase> KickSession(this RestClient client, string id)
        {
            var request = new RestRequest("getAllSession")
                .AddQueryParameter("id", id);
            return await client.GetAsync<IApiResultBase>(request);
        }

        /// <summary>
        /// 断开tcp连接，比如说可以断开rtsp、rtmp播放器等
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<IApiResultBase> KickSession(string mediaServerId, string id)
        {
            return await CreateClient(mediaServerId).KickSession( id);
        }

        /// <summary>
        /// 断开tcp连接，比如说可以断开rtsp、rtmp播放器
        /// </summary>
        /// <param name="local_port"></param>
        /// <param name="peer_ip"></param>
        /// <returns></returns>
        public static async Task<IApKillSessionsResult> KickSessions(this RestClient client, int? local_port = null, string peer_ip = null)
        {
            var request = new RestRequest("kick_sessions");
            if (local_port.HasValue) request = request.AddQueryParameter("local_port", local_port.Value.ToString());
            if (!string.IsNullOrEmpty(peer_ip)) request.AddQueryParameter("peer_ip", peer_ip);
            return await client.GetAsync<IApKillSessionsResult>(request);

        }

        /// <summary>
        /// 断开tcp连接，比如说可以断开rtsp、rtmp播放器
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="local_port"></param>
        /// <param name="peer_ip"></param>
        /// <returns></returns>
        public static async Task<IApKillSessionsResult> KickSessions(string mediaServerId, int? local_port = null, string peer_ip = null)
        {
            return await CreateClient(mediaServerId).KickSessions( local_port, peer_ip);
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
        public static async Task<IApiAddStreamPorxyResult> AddStreamProxy(this RestClient client, [NotNull] string vhost, [NotNull] string app, [NotNull] string stream, [NotNull] string url, bool? enable_hls = null, bool? enable_mp4 = null, int? rtp_type = null, float? timeout_sec = null, int? retry_count = null)
        {
            var request = new RestRequest("addStreamProxy")
                .AddQueryParameter("vhost", vhost)
                .AddQueryParameter("app", app)
                .AddQueryParameter("stream", stream)
                .AddQueryParameter("url", url);
            if (enable_hls.HasValue) request = request.AddQueryParameter("enable_hls", enable_hls == true ? "1" : "0");
            if (enable_mp4.HasValue) request = request.AddQueryParameter("enable_mp4", enable_mp4 == true ? "1" : "0");
            if (rtp_type.HasValue) request = request.AddQueryParameter("rtp_type", rtp_type.ToString());
            if (timeout_sec.HasValue) request = request.AddQueryParameter("timeout_sec", timeout_sec.ToString());
            if (retry_count.HasValue) request = request.AddQueryParameter("retry_count", retry_count.ToString());
            return await client.GetAsync<IApiAddStreamPorxyResult>(request);

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
        public static async Task<IApiAddStreamPorxyResult> AddStreamProxy([NotNull] string mediaServerId, [NotNull] string vhost, [NotNull] string app, [NotNull] string stream, [NotNull] string url, bool? enable_hls = null, bool? enable_mp4 = null, int? rtp_type = null, float? timeout_sec = null, int? retry_count = null)
        {
            return await CreateClient(mediaServerId).AddStreamProxy( vhost, app, stream, url, enable_hls, enable_mp4, rtp_type, timeout_sec, retry_count);
        }

        /// <summary>
        /// 关闭拉流代理
        /// </summary>
        /// <param name="key">addStreamProxy接口返回的key</param>
        /// <returns></returns>
        /// <remarks>流注册成功后，也可以使用close_streams接口替代</remarks>
        public static async Task<IApiDelStreamProxyResultItem> DelStreamProxy(this RestClient client, [NotNull] string key)
        {
            var request = new RestRequest("delStreamProxy")
                .AddQueryParameter("key", key);
            return await client.GetAsync<IApiDelStreamProxyResultItem>(request);
        }

        /// <summary>
        /// 关闭拉流代理
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="key">addStreamProxy接口返回的key</param>
        /// <returns></returns>
        /// <remarks>流注册成功后，也可以使用close_streams接口替代</remarks>
        public static async Task<IApiDelStreamProxyResultItem> DelStreamProxy([NotNull] string mediaServerId, [NotNull] string key)
        {
            return await CreateClient(mediaServerId).DelStreamProxy( key);
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
        public static async Task<IApiAddFFmpegSourceResult> AddFFmpegSource(this RestClient client, [NotNull] string src_url, [NotNull] string dst_url, [NotNull] int timeout_ms = 5000, [NotNull] bool enable_hls = false, [NotNull] bool enable_mp4 = false, string ffmpeg_cmd_key = null)
        {
            var request = new RestRequest("addFFmpegSource")
                .AddQueryParameter("src_url", src_url)
                .AddQueryParameter("dst_url", dst_url)
                .AddQueryParameter("timeout_ms", timeout_ms.ToString())
                .AddQueryParameter("enable_hls", enable_hls ? "1" : "0")
                .AddQueryParameter("enable_mp4", enable_mp4 ? "1" : "0");
            if (string.IsNullOrEmpty(ffmpeg_cmd_key)) request = request.AddQueryParameter("ffmpeg_cmd_key", ffmpeg_cmd_key);

            return await client.GetAsync<IApiAddFFmpegSourceResult>(request);
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
        public static async Task<IApiAddFFmpegSourceResult> AddFFmpegSource([NotNull] string mediaServerId, [NotNull] string src_url, [NotNull] string dst_url, [NotNull] int timeout_ms = 5000, [NotNull] bool enable_hls = false, [NotNull] bool enable_mp4 = false, string ffmpeg_cmd_key = null)
        {
            return await CreateClient(mediaServerId).AddFFmpegSource( src_url, dst_url, timeout_ms, enable_hls, enable_mp4, ffmpeg_cmd_key);
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
        public static async Task<IApiAddStreamPusherProxyResult> AddStreamPusherProxy(this RestClient client, [NotNull] string schema, [NotNull] string vhost, [NotNull] string app, [NotNull] string stream, [NotNull] string dst_url, int rtp_type = 0, int timeout_sec = 10, int retry_count = 0)
        {
            var request = new RestRequest("addStreamPusherProxy")
                .AddQueryParameter("schema", schema)
                .AddQueryParameter("vhost", vhost)
                .AddQueryParameter("app", app)
                .AddQueryParameter("stream", stream)
                .AddQueryParameter("dst_url", dst_url)
                .AddQueryParameter("rtp_type", rtp_type.ToString())
                .AddQueryParameter("timeout_sec", timeout_sec.ToString())
                .AddQueryParameter("retry_count", retry_count.ToString());

            return await client.GetAsync<IApiAddStreamPusherProxyResult>(request);
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
        public static async Task<IApiAddStreamPusherProxyResult> AddStreamPusherProxy([NotNull] string mediaServerId, [NotNull] string schema, [NotNull] string vhost, [NotNull] string app, [NotNull] string stream, [NotNull] string dst_url, int rtp_type = 0, int timeout_sec = 10, int retry_count = 0)
        {
            return await CreateClient(mediaServerId).AddStreamPusherProxy( schema, vhost, app, stream, dst_url, rtp_type, timeout_sec, retry_count);
        }

        /// <summary>
        /// 关闭推流(delStreamPusherProxy)
        /// </summary>
        /// <param name="key">addStreamPusherProxy接口返回的key</param>
        /// <returns></returns>
        public static async Task<IApiDelStreamPusherProxyResult> DelStreamPusherProxy(this RestClient client, [NotNull] string key)
        {
            var request = new RestRequest("delStreamPusherProxy")
                .AddQueryParameter("key", key);
            return await client.GetAsync<IApiDelStreamPusherProxyResult>(request);
        }

        /// <summary>
        /// 关闭推流(delStreamPusherProxy)
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="key">addStreamPusherProxy接口返回的key</param>
        /// <returns></returns>
        public static async Task<IApiDelStreamPusherProxyResult> DelStreamPusherProxy([NotNull] string mediaServerId, [NotNull] string key)
        {
            return await CreateClient(mediaServerId).DelStreamPusherProxy( key);
        }


        /// <summary>
        /// 关闭ffmpeg拉流代理
        /// </summary>
        /// <param name="key">addStreamProxy接口返回的key</param>
        /// <returns></returns>
        /// <remarks>流注册成功后，也可以使用close_streams接口替代</remarks>
        public static async Task<IApiDelFFmpegSourceResult> DelFFmpegSource(this RestClient client, [NotNull] string key)
        {
            var request = new RestRequest("delFFmpegSource")
                .AddQueryParameter("key", key);
            return await client.GetAsync<IApiDelFFmpegSourceResult>(request);
        }

        /// <summary>
        /// 关闭ffmpeg拉流代理
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="key">addStreamProxy接口返回的key</param>
        /// <returns></returns>
        /// <remarks>流注册成功后，也可以使用close_streams接口替代</remarks>
        public static async Task<IApiDelFFmpegSourceResult> DelFFmpegSource([NotNull] string mediaServerId, [NotNull] string key)
        {
            return await CreateClient(mediaServerId).DelFFmpegSource( key);
        }

        /// <summary>
        /// 下载可执行文件
        /// </summary>
        /// <returns></returns>
        public static async Task<byte[]> DownloadBin(this RestClient client)
        {
            var request = new RestRequest("downloadBin");
            return await client.DownloadDataAsync(request);
        }
        /// <summary>
        /// 下载可执行文件
        /// </summary>
        /// <returns></returns>
        public static async Task<byte[]> DownloadBin([NotNull] string mediaServerId)
        {
            return await CreateClient(mediaServerId).DownloadBin();
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
        public static async Task<IApiIsMediaOnlineResult> IsMediaOnline(this RestClient client, [NotNull] string schema, [NotNull] string vhost, [NotNull] string app, [NotNull] string stream)
        {
            var request = new RestRequest("isMediaOnline")
                .AddQueryParameter("schema", schema)
                .AddQueryParameter("vhost", vhost)
                .AddQueryParameter("app", app)
                .AddQueryParameter("stream", stream);

            return await client.GetAsync<IApiIsMediaOnlineResult>(request);
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
        public static async Task<IApiIsMediaOnlineResult> IsMediaOnline([NotNull] string mediaServerId, [NotNull] string schema, [NotNull] string vhost, [NotNull] string app, [NotNull] string stream)
        {
            return await CreateClient(mediaServerId).IsMediaOnline( schema, vhost, app, stream);
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
        public static async Task<IApiGetMediaInfo> GetMediaInfo(this RestClient client, [NotNull] string schema, [NotNull] string vhost, [NotNull] string app, [NotNull] string stream)
        {
            var request = new RestRequest("getMediaInfo")
                 .AddQueryParameter("schema", schema)
                .AddQueryParameter("vhost", vhost)
                .AddQueryParameter("app", app)
                .AddQueryParameter("stream", stream);
            return await client.GetAsync<IApiGetMediaInfo>(request);
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
        public static async Task<IApiGetMediaInfo> GetMediaInfo([NotNull] string mediaServerId, [NotNull] string schema, [NotNull] string vhost, [NotNull] string app, [NotNull] string stream)
        {
            return await CreateClient(mediaServerId).GetMediaInfo( schema, vhost, app, stream);
        }


        /// <summary>
        /// 获取rtp代理时的某路ssrc rtp信息
        /// </summary>
        /// <param name="stream_id">RTP的ssrc，16进制字符串或者是流的id(openRtpServer接口指定)</param>
        /// <returns></returns>
        public static async Task<IApiGetRtpInfoResult> GetRtpInfo(this RestClient client, [NotNull] string stream_id)
        {
            var request = new RestRequest("getRtpInfo")
                .AddQueryParameter("stream_id", stream_id);

            return await client.GetAsync<IApiGetRtpInfoResult>(request);
        }

        /// <summary>
        /// 获取rtp代理时的某路ssrc rtp信息
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="stream_id">RTP的ssrc，16进制字符串或者是流的id(openRtpServer接口指定)</param>
        /// <returns></returns>
        public static async Task<IApiGetRtpInfoResult> GetRtpInfo([NotNull] string mediaServerId, [NotNull] string stream_id)
        {
            return await CreateClient(mediaServerId).GetRtpInfo( stream_id);
        }

        /// <summary>
        /// 搜索文件系统，获取流对应的录像文件列表或日期文件夹列表
        /// </summary>
        /// <param name="vhost">流的虚拟主机名</param>
        /// <param name="app">流的应用名</param>
        /// <param name="stream">流的ID</param>
        /// <param name="period">流的录像日期，格式为2020-02-01,如果不是完整的日期，那么是搜索录像文件夹列表，否则搜索对应日期下的mp4文件列表</param>
        /// <returns></returns>
        public static async Task<IApiGetMp4RecordFileResult> GetMp4RecordFile(this RestClient client, [NotNull] string vhost, [NotNull] string app, [NotNull] string stream, [NotNull] string period)
        {
            var request = new RestRequest("getMp4RecordFile")
                .AddQueryParameter("vhost", vhost)
                .AddQueryParameter("app", app)
                .AddQueryParameter("stream", stream)
                .AddQueryParameter("period", period);
            return await client.GetAsync<IApiGetMp4RecordFileResult>(request);
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
        public static async Task<IApiGetMp4RecordFileResult> GetMp4RecordFile([NotNull] string mediaServerId, [NotNull] string vhost, [NotNull] string app, [NotNull] string stream, [NotNull] string period)
        {
            return await CreateClient(mediaServerId).GetMp4RecordFile( vhost, app, stream, period);
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
        public static async Task<IApiStartRecordResult> StartRecord(this RestClient client, [NotNull] int type, [NotNull] string vhost, [NotNull] string app, [NotNull] string stream, string customized_path, int? max_second = 0)
        {

            var request = new RestRequest("startRecord")
                .AddQueryParameter("type", type.ToString())
                .AddQueryParameter("vhost", vhost)
                .AddQueryParameter("app", app)
                .AddQueryParameter("stream", stream);
            if (!string.IsNullOrEmpty(customized_path)) request = request.AddQueryParameter("customized_path", customized_path);
            if (max_second.HasValue) request = request.AddQueryParameter("max_second", max_second.ToString());
            return await client.GetAsync<IApiStartRecordResult>(request);
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
        public static async Task<IApiStartRecordResult> StartRecord([NotNull] string mediaServerId, [NotNull] int type, [NotNull] string vhost, [NotNull] string app, [NotNull] string stream, string customized_path, int? max_second = 0)
        {
            return await CreateClient(mediaServerId).StartRecord( type, vhost, app, stream, customized_path, max_second);
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
        public static async Task<IApiSetRecordSpeedResult> StartRecordSpeed(this RestClient client, [NotNull] string schema, [NotNull] string vhost, [NotNull] string app, [NotNull] string stream, float speed)
        {
            var request = new RestRequest("setRecordSpeed")
                .AddQueryParameter("schema", schema)
                .AddQueryParameter("vhost", vhost)
                .AddQueryParameter("app", app)
                .AddQueryParameter("stream", stream)
                .AddQueryParameter("speed", speed.ToString());
            return await client.GetAsync<IApiSetRecordSpeedResult>(request);
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
        public static async Task<IApiSetRecordSpeedResult> StartRecordSpeed([NotNull] string mediaServerId, [NotNull] string schema, [NotNull] string vhost, [NotNull] string app, [NotNull] string stream, float speed)
        {
            return await CreateClient(mediaServerId).StartRecordSpeed( schema, vhost, app, stream, speed);
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
        public static async Task<IApiSeekRecordStampResult> SeekRecordStamp(this RestClient client, [NotNull] string schema, [NotNull] string vhost, [NotNull] string app, [NotNull] string stream, long stamp)
        {
            var request = new RestRequest("seekRecordStamp")
                .AddQueryParameter("schema", schema)
                .AddQueryParameter("vhost", vhost)
                .AddQueryParameter("app", app)
                .AddQueryParameter("stream", stream)
                .AddQueryParameter("stamp", stamp.ToString());
            return await client.GetAsync<IApiSeekRecordStampResult>(request);
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
        public static async Task<IApiSeekRecordStampResult> SeekRecordStamp([NotNull] string mediaServerId, [NotNull] string schema, [NotNull] string vhost, [NotNull] string app, [NotNull] string stream, long stamp)
        {
            return await CreateClient(mediaServerId).SeekRecordStamp( schema, vhost, app, stream, stamp);
        }

        /// <summary>
        /// 开始录制hls或MP4
        /// </summary>
        /// <param name="type">0为hls，1为mp4</param>
        /// <param name="vhost">流的虚拟主机名</param>
        /// <param name="app">流的应用名</param>
        /// <param name="stream">流的ID</param>
        /// <returns></returns>
        public static async Task<IApiStopRecordResult> StopRecord(this RestClient client, [NotNull] int type, [NotNull] string vhost, [NotNull] string app, [NotNull] string stream)
        {
            var request = new RestRequest("stopRecord")
                .AddQueryParameter("type", type.ToString())
                .AddQueryParameter("vhost", vhost)
                .AddQueryParameter("app", app)
                .AddQueryParameter("stream", stream);
            return await client.GetAsync<IApiStopRecordResult>(request);
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
        public static async Task<IApiStopRecordResult> StopRecord([NotNull] string mediaServerId, [NotNull] int type, [NotNull] string vhost, [NotNull] string app, [NotNull] string stream)
        {
            return await CreateClient(mediaServerId).StopRecord( type, vhost, app, stream);
        }

        /// <summary>
        /// 获取流录制状态
        /// </summary>
        /// <param name="type">0为hls，1为mp4</param>
        /// <param name="vhost">流的虚拟主机名</param>
        /// <param name="app">流的应用名</param>
        /// <param name="stream">流的ID</param>
        /// <returns></returns>
        public static async Task<IApiIsRecordingResult> IsRecording(this RestClient client, [NotNull] int type, [NotNull] string vhost, [NotNull] string app, [NotNull] string stream)
        {
            var request = new RestRequest("isRecording")
                .AddQueryParameter("type", type.ToString())
                .AddQueryParameter("vhost", vhost)
                .AddQueryParameter("app", app)
                .AddQueryParameter("stream", stream);
            return await client.GetAsync<IApiIsRecordingResult>(request);

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
        public static async Task<IApiIsRecordingResult> IsRecording([NotNull] string mediaServerId, [NotNull] int type, [NotNull] string vhost, [NotNull] string app, [NotNull] string stream)
        {
            return await CreateClient(mediaServerId).IsRecording( type, vhost, app, stream);
        }

        /// <summary>
        /// 获取截图或生成实时截图并返回
        /// </summary>
        /// <param name="url">需要截图的url，可以是本机的，也可以是远程主机的</param>
        /// <param name="timeout_sec">截图失败超时时间，防止FFmpeg一直等待截图</param>
        /// <param name="expire_sec">截图的过期时间，该时间内产生的截图都会作为缓存返回</param>
        /// <returns>jpeg格式的图片，可以在浏览器直接打开</returns>
        public static async Task<byte[]> GetSnap(this RestClient client, [NotNull] string url, int timeout_sec = 5, int expire_sec = 5)
        {
            var request = new RestRequest("getSnap")
                .AddQueryParameter("url", url)
                .AddQueryParameter("timeout_sec", timeout_sec.ToString())
                .AddQueryParameter("expire_sec", expire_sec.ToString());
            return await client.DownloadDataAsync(request);
        }

        /// <summary>
        /// 获取截图或生成实时截图并返回
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="url">需要截图的url，可以是本机的，也可以是远程主机的</param>
        /// <param name="timeout_sec">截图失败超时时间，防止FFmpeg一直等待截图</param>
        /// <param name="expire_sec">截图的过期时间，该时间内产生的截图都会作为缓存返回</param>
        /// <returns>jpeg格式的图片，可以在浏览器直接打开</returns>
        public static async Task<byte[]> GetSnap([NotNull] string mediaServerId, [NotNull] string url, int timeout_sec = 5, int expire_sec = 5)
        {
            return await CreateClient(mediaServerId).GetSnap( url, timeout_sec, expire_sec);
        }

        /// <summary>
        /// 创建GB28181 RTP接收端口，如果该端口接收数据超时，则会自动被回收(不用调用closeRtpServer接口)
        /// </summary>
        /// <param name="port">接收端口，0则为随机端口</param>
        /// <param name="enable_tcp">启用UDP监听的同时是否监听TCP端口</param>
        /// <param name="stream_id">该端口绑定的流ID，该端口只能创建这一个流(而不是根据ssrc创建多个)</param>
        /// <returns></returns>
        public static async Task<IApiOpenRtpServerResult> OpenRtpServer(this RestClient client, int port = 0, bool enable_tcp = false, string stream_id = null)
        {
            var request = new RestRequest("openRtpServer")
                .AddQueryParameter("port", port.ToString())
                .AddQueryParameter("enable_tcp", enable_tcp.ToString())
                .AddQueryParameter("stream_id", stream_id);

            return await client.GetAsync<IApiOpenRtpServerResult>(request);
        }

        /// <summary>
        /// 创建GB28181 RTP接收端口，如果该端口接收数据超时，则会自动被回收(不用调用closeRtpServer接口)
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="port">接收端口，0则为随机端口</param>
        /// <param name="enable_tcp">启用UDP监听的同时是否监听TCP端口</param>
        /// <param name="stream_id">该端口绑定的流ID，该端口只能创建这一个流(而不是根据ssrc创建多个)</param>
        /// <returns></returns>
        public static async Task<IApiOpenRtpServerResult> OpenRtpServer([NotNull] string mediaServerId, int port = 0, bool enable_tcp = false, string stream_id = null)
        {
            return await CreateClient(mediaServerId).OpenRtpServer(port, enable_tcp, stream_id);
        }

        /// <summary>
        /// 关闭GB28181 RTP接收端口
        /// </summary>
        /// <param name="stream_id"></param>
        /// <returns></returns>
        public static async Task<IApiCloseRtpServerResult> CloseRtpServer(this RestClient client, [NotNull] string stream_id)
        {
            var request = new RestRequest("closeRtpServer")
               .AddQueryParameter("stream_id", stream_id);

            return await client.GetAsync<IApiCloseRtpServerResult>(request);
        }

        /// <summary>
        /// 关闭GB28181 RTP接收端口
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="stream_id"></param>
        /// <returns></returns>
        public static async Task<IApiCloseRtpServerResult> CloseRtpServer([NotNull] string mediaServerId, [NotNull] string stream_id)
        {
            return await CreateClient(mediaServerId).CloseRtpServer( stream_id);
        }

        /// <summary>
        /// 获取openRtpServer接口创建的所有RTP服务器
        /// </summary>
        /// <returns></returns>
        public static async Task<IApiListRtpServerResult> ListRtpServer(this RestClient client)
        {
            var request = new RestRequest("listRtpServer");
            return await client.GetAsync<IApiListRtpServerResult>(request);
        }

        /// <summary>
        /// 获取openRtpServer接口创建的所有RTP服务器
        /// </summary>
        /// <returns></returns>
        public static async Task<IApiListRtpServerResult> ListRtpServer([NotNull] string mediaServerId)
        {
            return await CreateClient(mediaServerId).ListRtpServer();
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
        public static async Task<IApiStartSendRtpResult> StartSendRtp(this RestClient client, [NotNull] string vhost, [NotNull] string app, [NotNull] string stream, [NotNull] string ssrc, [NotNull] string dst_url, int dst_port, bool is_udp, int src_port = 0)
        {
            var request = new RestRequest("startSendRtp")
                .AddQueryParameter("vhost", vhost)
                .AddQueryParameter("app	", app)
                .AddQueryParameter("stream", stream)
                .AddQueryParameter("ssrc	", ssrc)
                .AddQueryParameter("dst_url", dst_url)
                .AddQueryParameter("dst_port", dst_port.ToString())
                .AddQueryParameter("is_udp", is_udp ? "1" : "0")
                .AddQueryParameter("src_port", src_port.ToString());
            return await client.GetAsync<IApiStartSendRtpResult>(request);
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
        public static async Task<IApiStartSendRtpResult> StartSendRtp([NotNull] string mediaServerId, [NotNull] string vhost, [NotNull] string app, [NotNull] string stream, [NotNull] string ssrc, [NotNull] string dst_url, int dst_port, bool is_udp, int src_port = 0)
        {
            return await CreateClient(mediaServerId).StartSendRtp( vhost, app, stream, ssrc, dst_url, dst_port, is_udp, src_port);
        }

        /// <summary>
        /// 停止GB28181 ps-rtp推流
        /// </summary>
        /// <param name="vhost">虚拟主机，例如__defaultVhost__</param>
        /// <param name="app">应用名，例如 live</param>
        /// <param name="stream">流id，例如 test</param>
        /// <param name="ssrc"></param>
        /// <returns></returns>
        public static async Task<IApiStopSendRtpResult> StopSendRtp(this RestClient client, [NotNull] string vhost, [NotNull] string app, [NotNull] string stream, string ssrc = null)
        {
            var request = new RestRequest("stopSendRtp")
                .AddQueryParameter("vhost", vhost)
                .AddQueryParameter("app	", app)
                .AddQueryParameter("stream", stream);
            if (!string.IsNullOrEmpty(ssrc)) request = request.AddQueryParameter("ssrc", ssrc);
            return await client.GetAsync<IApiStopSendRtpResult>(request);
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
        public static async Task<IApiStopSendRtpResult> StopSendRtp([NotNull] string mediaServerId, [NotNull] string vhost, [NotNull] string app, [NotNull] string stream, string ssrc = null)
        {
            return await CreateClient(mediaServerId).StopSendRtp( vhost, app, stream, ssrc);
        }

        /// <summary>
        /// 停止RTP代理超时检查
        /// </summary>
        /// <param name="streamid"></param>
        /// <returns></returns>
        public static async Task<IApiPauseRtpCheckResult> PauseRtpCheck(this RestClient client, [NotNull] string streamid)
        {
            var request = new RestRequest("pauseRtpCheck")
                .AddQueryParameter("stream_id", streamid);
            return await client.GetAsync<IApiPauseRtpCheckResult>(request);
        }

        /// <summary>
        /// 停止RTP代理超时检查
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="streamid"></param>
        /// <returns></returns>
        public static async Task<IApiPauseRtpCheckResult> PauseRtpCheck([NotNull] string mediaServerId, [NotNull] string streamid)
        {
            return await CreateClient(mediaServerId).PauseRtpCheck( streamid);
        }

        /// <summary>
        /// 恢复RTP代理超时检查
        /// </summary>
        /// <param name="streamid"></param>
        /// <returns></returns>
        public static async Task<IApiResumeRtpCheckResult> ResumeRtpCheck(this RestClient client, [NotNull] string streamid)
        {
            var request = new RestRequest("resumeRtpCheck")
                .AddQueryParameter("stream_id", streamid);
            return await client.GetAsync<IApiResumeRtpCheckResult>(request);
        }
        /// <summary>
        /// 恢复RTP代理超时检查
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="streamid"></param>
        /// <returns></returns>
        public static async Task<IApiResumeRtpCheckResult> ResumeRtpCheck([NotNull] string mediaServerId, [NotNull] string streamid)
        {
            return await CreateClient(mediaServerId).ResumeRtpCheck( streamid);
        }



        /// <summary>
        /// 获取ZLMediaKit对象统计
        /// </summary>
        /// <returns></returns>
        public static async Task<IApiGetStatisticResult> GetStatistic(this RestClient client)
        {
            var request = new RestRequest("getStatistic");
            return await client.GetAsync<IApiGetStatisticResult>(request);
        }

        /// <summary>
        /// 获取ZLMediaKit对象统计
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <returns></returns>
        public static async Task<IApiGetStatisticResult> GetStatistic([NotNull] string mediaServerId)
        {
            return await CreateClient(mediaServerId).GetStatistic();
        }
        /// <summary>
        /// 根据方法名调用接口获取数据
        /// </summary>
        /// <param name="methodName">方法名词</param>
        /// <param name="paras">请求参数</param>
        /// <typeparam name="T">返回参数</typeparam>
        /// <returns></returns>
        public static async Task<T> GetByMethod<T>(this RestClient client, string methodName, Dictionary<string, string> paras)
        {

            var request = new RestRequest(methodName);
            if (paras != null && paras.Count > 0)
            {

                request = request.AddOrUpdateParameters(paras.Select(s => Parameter.CreateParameter(s.Key, s.Value, ParameterType.QueryString)));
            }
            return await client.GetAsync<T>(request);
        }

        /// <summary>
        /// 根据方法名调用接口获取数据
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="methodName">方法名词</param>
        /// <param name="paras">请求参数</param>
        /// <typeparam name="T">返回参数</typeparam>
        /// <returns></returns>
        public static async Task<T> GetByMethod<T>([NotNull] string mediaServerId, string methodName, Dictionary<string, string> paras)
        {
            return await CreateClient(mediaServerId).GetByMethod<T>( methodName, paras);
        }
    }
}
