using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
//using ZLMediaKit.HttpApi;

namespace ZLMediaKit.WebHook.Demo
{
    public class HookService : IHostedService
    {
        //private readonly ZLHttpClient _zLHttpClient;
        //public HookService(ZLHttpClient zLHttpClient)
        //{
        //    _zLHttpClient = zLHttpClient;
        //}

        CancellationTokenSource CancellationToken = default;
        public Task StartAsync(CancellationToken cancellationToken)
        {
            ZLMediaKitWebHookEvents.OnPublish += ZLMediaKitWebHookEvents_OnPublish;
            ZLMediaKitWebHookEvents.OnServerStarted += ZLMediaKitWebHookEvents_OnServerStarted;
            ZLMediaKitWebHookEvents.OnStreamChanged += ZLMediaKitWebHookEvents_OnStreamChanged;
            ZLMediaKitWebHookEvents.OnShellLogin += ZLMediaKitWebHookEvents_OnShellLogin;
            ZLMediaKitWebHookEvents.OnServerKeepalive += ZLMediaKitWebHookEvents_OnServerKeepalive;
            ZLMediaKitWebHookEvents.OnHttpAccess += ZLMediaKitWebHookEvents_OnHttpAccess;
            return Task.Run(() =>
            {
                CancellationToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                
            }, cancellationToken);
        }

        private Task<Common.Dtos.HookResultDto.IHookHttpAccessResult> ZLMediaKitWebHookEvents_OnHttpAccess(Common.Dtos.EventArgs.IHookEventArgs<Common.Dtos.HookInputDto.IHttpAccessInput> hookEventArgs)
        {
            Console.WriteLine($"收到http 访问 {Environment.NewLine} {System.Text.Json.JsonSerializer.Serialize(hookEventArgs.Payload)}");
            return Task.FromResult(new Common.Dtos.HookResultDto.HookHttpAccessResult
            {
                Code = 0,
                Message = "googd job",
                Path = String.Empty,
                Second = 1800
            } as Common.Dtos.HookResultDto.IHookHttpAccessResult);
        }

        private async Task ZLMediaKitWebHookEvents_OnServerKeepalive(Common.Dtos.EventArgs.IHookEventArgs<Common.Dtos.HookInputDto.IServerKeepaliveInput> hookEvent)
        {
            Console.WriteLine($"收到心跳数据 {Environment.NewLine} {System.Text.Json.JsonSerializer.Serialize(hookEvent.Payload)}");
            Console.WriteLine($"收到心跳数据 {Environment.NewLine} {System.Text.Json.JsonSerializer.Serialize(hookEvent.ServerManager?? default)}");
            return;
        }

        private Task<Common.Dtos.HookResultDto.IHookShellLoginResult> ZLMediaKitWebHookEvents_OnShellLogin(Common.Dtos.EventArgs.IHookEventArgs<Common.Dtos.HookInputDto.IShellLoginInput> hookEventArgs)
        {
            Console.WriteLine($"收到登录数据 {Environment.NewLine} {System.Text.Json.JsonSerializer.Serialize(hookEventArgs)}");
            return Task.FromResult( new Common.Dtos.HookResultDto.HookCommonResult()
            {
                Code = 0,
                Message = "允许登录"
            } as Common.Dtos.HookResultDto.IHookShellLoginResult);
        }

        private Task<Common.Dtos.HookResultDto.IHookStreamChangedResult> ZLMediaKitWebHookEvents_OnStreamChanged(Common.Dtos.EventArgs.IHookEventArgs<Common.Dtos.HookInputDto.IStreamChangedInput> hookEventArgs)
        {
            Console.WriteLine($"收到心跳数据 {Environment.NewLine} {System.Text.Json.JsonSerializer.Serialize(hookEventArgs)}");
            throw new NotImplementedException();
        }

        private async Task ZLMediaKitWebHookEvents_OnServerStarted(Common.Dtos.EventArgs.IHookEventArgs<Common.Dtos.HookInputDto.IServerStartedInput> hookEventArgs)
        {
            Console.WriteLine($"服务器启动 {Environment.NewLine} {System.Text.Json.JsonSerializer.Serialize(hookEventArgs.Payload)}");
            Console.WriteLine($"服务器启动 {Environment.NewLine} {System.Text.Json.JsonSerializer.Serialize(hookEventArgs.ServerManager)}");
            return ;
        }

        private Task<Common.Dtos.HookResultDto.IHookPublishResult> ZLMediaKitWebHookEvents_OnPublish(Common.Dtos.EventArgs.IHookEventArgs<Common.Dtos.HookInputDto.IPublishInput> hookEventArgs)
        {
            Console.WriteLine($"收到心跳数据 {Environment.NewLine} {System.Text.Json.JsonSerializer.Serialize(hookEventArgs)}");
            throw new NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            //Console.WriteLine($"收到心跳数据 {Environment.NewLine} {System.Text.Json.JsonSerializer.Serialize(hookEventArgs)}");
            return Task.CompletedTask;
        }
    }
}
