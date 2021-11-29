using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ZLMediaKit.Common;
using ZLMediaKit.Common.Dtos;
using ZLMediaKit.Common.Dtos.EventArgs;
using ZLMediaKit.Common.Dtos.HookInputDto;
using ZLMediaKit.Common.Dtos.HookResultDto;

namespace ZLMediaKit.WebHook.Services
{
    //[EnableCors]
    /// <summary>
    /// Web Hook Service
    /// </summary>
    [AllowAnonymous]
    public class ZLMediaKitWebHookServcies
    {
        private readonly IHttpContextAccessor _contextAccessor;
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="contextAccessor"></param>
        public ZLMediaKitWebHookServcies(IHttpContextAccessor contextAccessor)
        {
            this._contextAccessor = contextAccessor;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        private void SetServerInfo(IHookBase eventBase)
        {
            eventBase.ZlMediaKitAddress = _contextAccessor?.HttpContext?.Connection?.RemoteIpAddress.ToString();
            eventBase.ZlMediaKitPort = _contextAccessor?.HttpContext?.Connection?.RemotePort ?? 0;


        }

        private async Task<IActionResult> Execute<T>(T value, Func<T, Task<IActionResult>> func) where T : IHookBase
        {
            _contextAccessor.HttpContext.Request.Body.Position = 0;
            TypeMapping.TypeMappings.TryGetValue(typeof(T), out var type);
            //RegisteredModelInstance.ModelMappding.TryGetValue(typeof(T), out var type);

            // 如果有类型映射，则获取类型映射的值, 否则保持原样
            if (type != null)
            {
                value = (T)System.Text.Json.JsonSerializer.Deserialize(_contextAccessor.HttpContext.Request.Body, type, TypeMapping.SerializerOptions);
            }
            else value = System.Text.Json.JsonSerializer.Deserialize<T>(_contextAccessor.HttpContext.Request.Body, TypeMapping.SerializerOptions);
            value.ZlMediaKitPort = _contextAccessor?.HttpContext?.Connection?.RemotePort ?? 0;
            value.ZlMediaKitAddress = _contextAccessor?.HttpContext?.Connection?.RemoteIpAddress.ToString();
            return await func(value);
        }

        private async Task<IActionResult> Execute<T, T1>(T value, Func<T, Task<T1>> func) where T : IHookBase where T1 : IResultBase
        {
            //RegisteredModelInstance.ModelMappding.TryGetValue(typeof(T), out var type);
            TypeMapping.TypeMappings.TryGetValue(typeof(T), out var type);
            _contextAccessor.HttpContext.Request.EnableBuffering();
            _contextAccessor.HttpContext.Request.Body.Position = 0;
            var requestReader = new StreamReader(_contextAccessor.HttpContext.Request.Body);
            var requestContent = await requestReader.ReadToEndAsync();

            if (type != null)
            {
                value = (T)System.Text.Json.JsonSerializer.Deserialize(requestContent, type, TypeMapping.SerializerOptions);
            }
            else value = System.Text.Json.JsonSerializer.Deserialize<T>(requestContent, TypeMapping.SerializerOptions);

            // 如果有类型映射，则获取类型映射的值, 否则保持原样


            value.ZlMediaKitPort = _contextAccessor?.HttpContext?.Connection?.RemotePort ?? 0;
            value.ZlMediaKitAddress = _contextAccessor?.HttpContext?.Connection?.RemoteIpAddress.ToString();
            var result = await func(value);
            if (result != null) return Json(result);
            return Json(new HookCommonResult());

        }

        /// <summary>
        /// 流量统计事件,播放器或推流器断开时并且耗用流量超过特定阈值时会触发此事件，阈值通过配置文件
        /// </summary>
        /// <returns></returns>
        [HttpPost(Name = "on_flow_report")]
        public async Task<IActionResult> FlowReportAsync() =>
            await Execute(default(IReportFlowInput), model => ZLMediaKitWebHookEvents.OnFlowReport_Call(new HookEventArgs<IReportFlowInput>(_contextAccessor.HttpContext, model)));


        /// <summary>
        /// 访问http文件服务器上hls之外的文件时触发
        /// </summary>
        /// <returns></returns>
        [HttpPost(Name = "on_http_access")]
        public async Task<IActionResult> HttpAccessAsync()
        {

            return await Execute(default(IHttpAccessInput), model =>
            {
                var headers = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(_contextAccessor.HttpContext.Request.Body).Where(w => w.Key.StartsWith("header.", StringComparison.OrdinalIgnoreCase)).Select(s => new { Key = s.Key.Replace("header.", String.Empty, StringComparison.OrdinalIgnoreCase), Value = s.Value }).ToDictionary(k => k.Key, v => v.Value);
                model.InitHeader(headers);
                return ZLMediaKitWebHookEvents.OnHttpAccess_Call(new HookEventArgs<IHttpAccessInput>(_contextAccessor.HttpContext, model));
            });
        }

        /// <summary>
        /// 播放器鉴权事件,rtsp/rtmp/http-flv/ws-flv/hls的播放都将触发此鉴权事件
        /// </summary>
        /// <remarks> 如果流不存在，那么先触发on_play事件然后触发on_stream_not_found事件;
        /// 播放rtsp流时，如果该流启动了rtsp专属鉴权(on_rtsp_realm)那么将不再触发on_play事件。
        /// </remarks>
        [HttpPost(Name = "on_play")]
        public async Task<IActionResult> PlayAsync() =>
            await Execute(default(IPlayInput), model => ZLMediaKitWebHookEvents.OnPlay_Call(new HookEventArgs<IPlayInput>(_contextAccessor.HttpContext, model)));

        /// <summary>
        /// rtsp/rtmp/rtp推流鉴权事件
        /// </summary>
        /// <returns></returns>
        [HttpPost(Name = "on_publish")]
        public async Task<IActionResult> PublishAsync()
            => await Execute(default(IPublishInput), model => ZLMediaKitWebHookEvents.OnPublish_Call(new HookEventArgs<IPublishInput>(_contextAccessor.HttpContext, model)));

        /// <summary>
        /// 录制mp4完成后通知事件；此事件对回复不敏感
        /// </summary>
        /// <returns></returns>
        [HttpPost(Name = "on_record_mp4")]
        public async Task<IActionResult> RecordMp4Async()
            => await Execute(default(IRecordMp4Input), model => ZLMediaKitWebHookEvents.OnRecordMP4_Call(new HookEventArgs<IRecordMp4Input>(_contextAccessor.HttpContext, model)));

        /// <summary>
        /// 录制TS完成后通知事件；此事件对回复不敏感
        /// </summary>
        /// <returns></returns>
        [HttpPost(Name = "on_record_ts")]
        public async Task<IActionResult> RecordTsAsync()
            => await Execute(default(IRecordTsInput), model => ZLMediaKitWebHookEvents.OnRecordTS_Call(new HookEventArgs<IRecordTsInput>(_contextAccessor.HttpContext, model)));

        /// <summary>
        /// 该rtsp流是否开启rtsp专用方式的鉴权事件，开启后才会触发on_rtsp_auth事件
        /// </summary>
        /// <remarks>需要指出的是rtsp也支持url参数鉴权，它支持两种方式鉴权</remarks>
        [HttpPost(Name = "on_rtsp_realm")]
        public async Task<IActionResult> RtspRealmAsync()
            => await Execute(default(IRtspRealmInput), model => ZLMediaKitWebHookEvents.OnRtspRealm_Call(new HookEventArgs<IRtspRealmInput>(_contextAccessor.HttpContext, model)));


        /// <summary>
        /// rtsp专用的鉴权事件，先触发on_rtsp_realm事件然后才会触发on_rtsp_auth事件。
        /// </summary>
        /// <returns></returns>
        [HttpPost(Name = "on_rtsp_auth")]
        public async Task<IActionResult> RtspAuthAsync()
            => await Execute(default(IRtspAuthInput), model => ZLMediaKitWebHookEvents.OnRtspAuth_Call(new HookEventArgs<IRtspAuthInput>(_contextAccessor.HttpContext, model)));


        /// <summary>
        /// shell登录鉴权，ZLMediaKit提供简单的telnet调试方式;
        /// 使用telnet 127.0.0.1 9000能进入MediaServer进程的shell界面
        /// </summary>
        /// <returns></returns>
        [HttpPost(Name = "on_shell_login")]
        public async Task<IActionResult> ShellLoginAsync()
            => await Execute(default(IShellLoginInput), model => ZLMediaKitWebHookEvents.OnShellLogin_Call(new HookEventArgs<IShellLoginInput>(_contextAccessor.HttpContext, model)));


        /// <summary>
        /// rtsp/rtmp流注册或注销时触发此事件；此事件对回复不敏感。
        /// </summary>
        /// <returns></returns>
        [HttpPost(Name = "on_stream_changed")]
        public async Task<IActionResult> StreamChangedAsync()
            => await Execute(default(IStreamChangedInput), model => ZLMediaKitWebHookEvents.OnStreamChanged_Call(new HookEventArgs<IStreamChangedInput>(_contextAccessor.HttpContext, model)));


        /// <summary>
        /// 流无人观看时事件，用户可以通过此事件选择是否关闭无人看的流。
        /// </summary>
        /// <returns></returns>
        [HttpPost(Name = "on_stream_none_reader")]
        public async Task<IActionResult> StreamNoneReaderAsync()
            => await Execute(default(IStreamNoneReaderInput), model => ZLMediaKitWebHookEvents.OnStreamNoneReader_Call(new HookEventArgs<IStreamNoneReaderInput>(_contextAccessor.HttpContext, model)));


        /// <summary>
        /// 流未找到事件，用户可以在此事件触发时，立即去拉流，这样可以实现按需拉流；此事件对回复不敏感
        /// </summary>
        /// <returns></returns>
        [HttpPost(Name = "on_stream_not_found")]
        public async Task<IActionResult> StreamNotFoundAsync()
            => await Execute(default(IStreamNotFoundInuut), model => ZLMediaKitWebHookEvents.OnStreamNotFound_Call(new HookEventArgs<IStreamNotFoundInuut>(_contextAccessor.HttpContext, model)));

        /// <summary>
        /// 服务器启动事件，可以用于监听服务器崩溃重启；此事件对回复不敏感。
        /// </summary>
        /// <param name="dicts"></param>
        /// <returns></returns>
        [HttpPost(Name = "on_server_started")]
        public async Task<IActionResult> ServerStartedAsync([FromBody] Dictionary<string, string> dicts)
        {

            var groups = dicts.Select(s => new { Key = s.Key.Split('.'), Value = s.Value })
                .Select(s => new { ClassName = s.Key.Length == 2 ? s.Key[0].Replace("_", String.Empty) : "rootElement", Key = s.Key.LastOrDefault(), Value = s.Value })
                .GroupBy(x => x.ClassName);
            IDictionary<string, object> result = new ExpandoObject();
            foreach (var group in groups)
            {
                IDictionary<string, object> temp = new ExpandoObject();
                foreach (var item in group)
                {
                    if (group.Key == "rootElement")
                    {
                        result[item.Key] = item.Value;
                    }
                    else
                        temp.Add(item.Key, item.Value);
                }
                result.Add(group.Key, temp);
            }
            var jsonStr = System.Text.Json.JsonSerializer.Serialize(result as ExpandoObject);
            var serverCofnig = System.Text.Json.JsonSerializer.Deserialize<IServerStartedInput>(jsonStr, TypeMapping.SerializerOptions);
            var serviceManager = IServerManager.GetServerManager(serverCofnig);
            if (serviceManager == null)
            {
                serviceManager = new ServerManager()
                {
                    MediaServerId = serverCofnig.MediaServerId,
                };
                IServerManager.Instances[serverCofnig.MediaServerId] = serviceManager;
            }
            serviceManager.StartTime = DateTime.Now;
            serviceManager.ServerConfig = serverCofnig;

            serverCofnig.ZlMediaKitPort = _contextAccessor?.HttpContext?.Connection?.RemotePort ?? 0;
            serverCofnig.ZlMediaKitAddress = _contextAccessor?.HttpContext?.Connection?.RemoteIpAddress.ToString();

            return await ZLMediaKitWebHookEvents.OnServerStarted_Call(new HookEventArgs<IServerStartedInput>(_contextAccessor.HttpContext, serverCofnig)).ContinueWith(t => Json(t.Result));
        }

        /// <summary>
        /// 心跳
        /// </summary>
        /// <returns></returns>
        [HttpPost(Name = "on_server_keepalive")]
        public async Task<IActionResult> ServerKeepalive()
        {
            return await Execute(default(IServerKeepaliveInput), model =>
            {
                var serverManager = IServerManager.GetServerManager(model);
                if (serverManager != null)
                {
                    serverManager.Keepalive = model;
                    serverManager.KeepaliveTime = DateTime.Now;
                }
                return ZLMediaKitWebHookEvents.OnServerKeepalive_Call(new HookEventArgs<IServerKeepaliveInput>(_contextAccessor.HttpContext, model));
            });
        }


        private IActionResult Json(object data)
        {
            return new ContentResult()
            {
                Content = System.Text.Json.JsonSerializer.Serialize(data, TypeMapping.SerializerOptions),
                ContentType = "application/json;charset=utf-8",
            };
        }
    }
}
