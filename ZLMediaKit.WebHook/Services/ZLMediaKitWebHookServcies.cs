using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLMediaKit.WebHook.Dtos;

namespace ZLMediaKit.WebHook.Services
{
    //[EnableCors]
    public class ZLMediaKitWebHookServcies 
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public ZLMediaKitWebHookServcies(IHttpContextAccessor contextAccessor)
        {
            this._contextAccessor = contextAccessor;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        private void SetServerInfo(EventBase eventBase)
        {
            eventBase.ServerIp = _contextAccessor?.HttpContext?.Connection?.RemoteIpAddress;
            eventBase.ServerPort = (_contextAccessor?.HttpContext?.Connection?.RemotePort)??0;
        }

        /// <summary>
        /// 流量统计事件,播放器或推流器断开时并且耗用流量超过特定阈值时会触发此事件，阈值通过配置文件
        /// </summary>
        /// <param name="flowReport"></param>
        /// <returns></returns>
        [HttpPost(Name = "on_flow_report")]
        public async Task<IActionResult> FlowReportAsync([FromBody]Dtos.FlowReport flowReport)
        {
            if(!ZLMediaKitWebHookEvents.OnFlowReport_IsNull)
            {
                SetServerInfo(flowReport);
                _ = Task.Run(() => {
                    ZLMediaKitWebHookEvents.OnFlowReport_Call(flowReport);
                });
            }
            return await Task.FromResult( Json(new ResultBase() ));
        }

        /// <summary>
        /// 访问http文件服务器上hls之外的文件时触发
        /// </summary>
        /// <param name="httpAccess"></param>
        /// <returns></returns>
        [HttpPost(Name = "on_http_access")]
        public async Task<IActionResult> HttpAccessAsync([FromBody] Dtos.HttpAccess httpAccess)
        {
            if(ZLMediaKitWebHookEvents.OnHttpAccess_IsNull)
            {
                return Json(new FlowReport());
            }
            SetServerInfo(httpAccess);
            httpAccess.Header = _contextAccessor.HttpContext.Request.Form.Where(w => w.Key.StartsWith("header.")).ToDictionary(k => k.Key, v => v.Value.ToString());
            return Json(ZLMediaKitWebHookEvents.OnHttpAccess_Call(httpAccess));
        }

        /// <summary>
        /// 播放器鉴权事件,rtsp/rtmp/http-flv/ws-flv/hls的播放都将触发此鉴权事件
        /// </summary>
        /// <remarks> 如果流不存在，那么先触发on_play事件然后触发on_stream_not_found事件;
        /// 播放rtsp流时，如果该流启动了rtsp专属鉴权(on_rtsp_realm)那么将不再触发on_play事件。
        /// </remarks>
        [HttpPost(Name = "on_play")]
        public async Task<IActionResult> PlayAsync([FromBody]PlayInfo playInfo)
        {
            if (ZLMediaKitWebHookEvents.OnPlay_IsNull) return Json(new ResultBase());
            SetServerInfo(playInfo);
            return Json(ZLMediaKitWebHookEvents.OnPlay_Call(playInfo));

        }

        /// <summary>
        /// rtsp/rtmp/rtp推流鉴权事件
        /// </summary>
        /// <param name="publishInfo"></param>
        /// <returns></returns>
        [HttpPost(Name = "on_publish")]
        public async Task<IActionResult> PublishAsync([FromBody]PublishInfo publishInfo)
        {
            if (ZLMediaKitWebHookEvents.OnPlay_IsNull) return Json(new PublishResult());
            SetServerInfo(publishInfo);
            return Json(ZLMediaKitWebHookEvents.OnPublish_Call(publishInfo));
        }

        /// <summary>
        /// 录制mp4完成后通知事件；此事件对回复不敏感
        /// </summary>
        /// <param name="recordInfo"></param>
        /// <returns></returns>
        [HttpPost(Name = "on_record_mp4")]
        public async Task<IActionResult> RecordMp4Async([FromBody]RecordInfo recordInfo)
        {
            if(!ZLMediaKitWebHookEvents.OnRecordMP4_IsNull)
            {
                SetServerInfo(recordInfo);
                _ = Task.Run(() =>
                {
                    ZLMediaKitWebHookEvents.OnRecordMP4_Call(recordInfo);
                });
            }
            return Json(new ResultBase());
        }
        /// <summary>
        /// 该rtsp流是否开启rtsp专用方式的鉴权事件，开启后才会触发on_rtsp_auth事件
        /// </summary>
        /// <remarks>需要指出的是rtsp也支持url参数鉴权，它支持两种方式鉴权</remarks>
        [HttpPost(Name = "on_rtsp_realm")]
        public async Task<IActionResult> RtspRealmAsync([FromBody]RtspRealmInfo rtspRealmInfo)
        {
            if (ZLMediaKitWebHookEvents.OnRtspRealm_IsNull) return Json(new RtspRealmInfoResult());
            SetServerInfo(rtspRealmInfo);
            return Json(ZLMediaKitWebHookEvents.OnRtspRealm_Call(rtspRealmInfo));

        }
        /// <summary>
        /// rtsp专用的鉴权事件，先触发on_rtsp_realm事件然后才会触发on_rtsp_auth事件。
        /// </summary>
        /// <param name="rtspAuthInfo"></param>
        /// <returns></returns>
        [HttpPost(Name = "on_rtsp_auth")]
        public async Task<IActionResult> RtspAuthAsync([FromBody]RtspAuthInfo rtspAuthInfo)
        {
            if (ZLMediaKitWebHookEvents.OnRtspAuth_IsNull) return Json(new RtspAuthResult { Code = 0,Encrypted = false});
            SetServerInfo(rtspAuthInfo);
            return Json(ZLMediaKitWebHookEvents.OnRtspAuth_Call(rtspAuthInfo));
        }

        /// <summary>
        /// shell登录鉴权，ZLMediaKit提供简单的telnet调试方式;
        /// 使用telnet 127.0.0.1 9000能进入MediaServer进程的shell界面
        /// </summary>
        /// <param name="shellLoginInfo"></param>
        /// <returns></returns>
        [HttpPost(Name = "on_shell_login")]
        public async Task<IActionResult> ShellLoginAsync([FromBody]ShellLoginInfo shellLoginInfo)
        {
            if(ZLMediaKitWebHookEvents.OnShellLogin_IsNull) return Json(new ShellLonginResult());
            SetServerInfo(shellLoginInfo);
            return Json(ZLMediaKitWebHookEvents.OnShellLogin_Call(shellLoginInfo));
        }

        /// <summary>
        /// rtsp/rtmp流注册或注销时触发此事件；此事件对回复不敏感。
        /// </summary>
        /// <param name="streamChangedInfo"></param>
        /// <returns></returns>
        [HttpPost(Name = "on_stream_changed")]
        public async Task<IActionResult> StreamChangedAsync([FromBody]StreamChangedInfo streamChangedInfo)
        {
            if(!ZLMediaKitWebHookEvents.OnStreamChanged_IsNull)
            {
                SetServerInfo(streamChangedInfo);
                _ = Task.Run(() => {
                    ZLMediaKitWebHookEvents.OnStreamChanged_Call(streamChangedInfo);
                });
            }
            return Json(new ResultBase());
        }

        /// <summary>
        /// 流无人观看时事件，用户可以通过此事件选择是否关闭无人看的流。
        /// </summary>
        /// <param name="streamNoneReaderInfo"></param>
        /// <returns></returns>
        [HttpPost(Name = "on_stream_none_reader")]
        public async Task<IActionResult> StreamNoneReaderAsync([FromBody]StreamNoneReaderInfo streamNoneReaderInfo)
        {
            if (ZLMediaKitWebHookEvents.OnStreamNoneReader_IsNull) return Json(new StreamNoneReaderInfoResult());
            SetServerInfo(streamNoneReaderInfo);
            return Json(ZLMediaKitWebHookEvents.OnStreamNoneReader_Call(streamNoneReaderInfo));
        }

        /// <summary>
        /// 流未找到事件，用户可以在此事件触发时，立即去拉流，这样可以实现按需拉流；此事件对回复不敏感
        /// </summary>
        /// <param name="streamNotFoundInfo"></param>
        /// <returns></returns>
        [HttpPost(Name = "on_stream_not_found")]
        public async Task<IActionResult> StreamNotFoundAsync([FromBody]StreamNotFoundInfo streamNotFoundInfo)
        {
            if(!ZLMediaKitWebHookEvents.OnStreamNotFound_IsNull)
            {
                SetServerInfo(streamNotFoundInfo);
                _ = Task.Run(() => ZLMediaKitWebHookEvents.OnStreamNotFound_Call(streamNotFoundInfo));
            }
            return Json(new ResultBase());
        }

        /// <summary>
        /// 服务器启动事件，可以用于监听服务器崩溃重启；此事件对回复不敏感。
        /// </summary>
        /// <param name="dicts"></param>
        /// <returns></returns>
        [HttpPost(Name = "on_server_started")]
        public async Task<IActionResult> ServerStartedAsync([FromBody]Dictionary<string,string> dicts)
        {

            if (!ZLMediaKitWebHookEvents.OnServerStarted_IsNull)
            {
                var config = new ServerConfig();
                SetServerInfo(config);
                _ = Task.Run(() =>
                {
                    config.Api = GetModel<ServerConfig.ApiConfig>(dicts, ServerConfig.ApiConfig.PrefixName);
                    config.Ffmpeg = GetModel<ServerConfig.FfmpegConfig>(dicts, ServerConfig.FfmpegConfig.PrefixName);
                    config.General = GetModel<ServerConfig.GeneralConfig>(dicts, ServerConfig.GeneralConfig.PrefixName);
                    config.Hls = GetModel<ServerConfig.HlsConfig>(dicts, ServerConfig.HlsConfig.PrefixName);
                    config.Hook = GetModel<ServerConfig.HookConfig>(dicts, ServerConfig.HookConfig.PrefixName);
                    config.Http = GetModel<ServerConfig.HttpConfig>(dicts, ServerConfig.HttpConfig.PrefixName);
                    config.Multicast = GetModel<ServerConfig.MulticastConfig>(dicts, ServerConfig.MulticastConfig.PrefixName);
                    config.Record = GetModel<ServerConfig.RecordConfig>(dicts, ServerConfig.RecordConfig.PrefixName);
                    config.Rtmp = GetModel<ServerConfig.RtmpConfig>(dicts, ServerConfig.RtmpConfig.PrefixName);
                    config.Rtp = GetModel<ServerConfig.RtpConfig>(dicts, ServerConfig.RtpConfig.PrefixName);
                    config.RtpProxy = GetModel<ServerConfig.RtpProxyConfig>(dicts, ServerConfig.RtpProxyConfig.PrefixName);
                    config.Rtsp = GetModel<ServerConfig.RtspConfig>(dicts, ServerConfig.RtspConfig.PrefixName);
                    config.Shell = GetModel<ServerConfig.ShellConfig>(dicts, ServerConfig.ShellConfig.PrefixName);
                    ZLMediaKitWebHookEvents.OnServerStarted_Call(config);
                });
            }
            
            return Json(new ResultBase());

        }

        private T GetModel<T>(Dictionary<string, string> dicts, string name) where T : new()
        {
            var objDict = dicts.Where(w => w.Key.StartsWith(name)).ToDictionary(f => f.Key.Replace(name, ""), v => v.Value);
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(objDict));
        }


        private IActionResult Json(object data)
        {
            return new ContentResult()
            {
                Content = JsonConvert.SerializeObject(data, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }),
                ContentType = "application/json;charset=utf-8",
            };
        }
    }
}
