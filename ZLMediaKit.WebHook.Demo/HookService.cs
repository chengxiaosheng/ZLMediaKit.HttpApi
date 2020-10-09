using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZLMediaKit.HttpApi;

namespace ZLMediaKit.WebHook.Demo
{
    public class HookService : IHostedService
    {
        private readonly ZLHttpClient _zLHttpClient;
        public HookService(ZLHttpClient zLHttpClient)
        {
            _zLHttpClient = zLHttpClient;
        }

        CancellationTokenSource CancellationToken = default;
        public Task StartAsync(CancellationToken cancellationToken)
        {
            ZLMediaKitWebHookEvents.OnPublish += ZLMediaKitWebHookEvents_OnPublish;
            ZLMediaKitWebHookEvents.OnServerStarted += ZLMediaKitWebHookEvents_OnServerStarted;
            ZLMediaKitWebHookEvents.OnStreamChanged += ZLMediaKitWebHookEvents_OnStreamChanged;
            return Task.Run(() =>
            {
                CancellationToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                
            }, cancellationToken);
            
            
        }

        private void ZLMediaKitWebHookEvents_OnStreamChanged(Dtos.StreamChangedInfo arg)
        {
            Console.WriteLine($"收到OnPublish事件，服务器id：{arg.MediaServerId}");
            Task.Delay(2000).ContinueWith(t =>
            {
                var mediaSource = _zLHttpClient.GetMediaList(arg.Schema, arg.Vhost, arg.App).Result;
                if (mediaSource != null)
                {
                    Console.WriteLine(JsonConvert.SerializeObject(mediaSource));
                }
            });
        }

        private void ZLMediaKitWebHookEvents_OnServerStarted(Dtos.ServerConfig obj)
        {
        }

        private Dtos.PublishResult ZLMediaKitWebHookEvents_OnPublish(Dtos.PublishInfo arg)
        {
            return new Dtos.PublishResult();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            CancellationToken.Cancel(false);
            return Task.Run(()=> { 
                ZLMediaKitWebHookEvents.OnPublish -= ZLMediaKitWebHookEvents_OnPublish;
            });
        }
    }
}
