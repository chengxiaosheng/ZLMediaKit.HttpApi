using Flurl.Http.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ZLMediaKit.WebHook;
using ZLMediaKit.WebHook.Dtos;
using Flurl.Http;
using Flurl;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Net;
using Microsoft.Extensions.Hosting;

namespace ZLMediaKit.HttpApi.Test
{
    [TestFixture]
    public class WebHookEventTest
    {
        private const string BaseUrl = "https://localhost:5001/index/hook";
        public WebHookEventTest()
        {
            
        }

        [SetUp]
        public void Init()
        {
            ZLMediaKitWebHookEvents.OnFlowReport += ZLMediaKitWebHookEvents_OnFlowReport;
            ZLMediaKitWebHookEvents.OnHttpAccess += ZLMediaKitWebHookEvents_OnHttpAccess;
            ZLMediaKitWebHookEvents.OnPlay += ZLMediaKitWebHookEvents_OnPlay;
            ZLMediaKitWebHookEvents.OnPublish += ZLMediaKitWebHookEvents_OnPublish;
            ZLMediaKitWebHookEvents.OnRecordMP4 += ZLMediaKitWebHookEvents_OnRecordMP4;
            ZLMediaKitWebHookEvents.OnRtspAuth += ZLMediaKitWebHookEvents_OnRtspAuth;
            ZLMediaKitWebHookEvents.OnRtspRealm += ZLMediaKitWebHookEvents_OnRtspRealm;
            ZLMediaKitWebHookEvents.OnServerStarted += ZLMediaKitWebHookEvents_OnServerStarted;
            ZLMediaKitWebHookEvents.OnShellLogin += ZLMediaKitWebHookEvents_OnShellLogin;
            ZLMediaKitWebHookEvents.OnStreamChanged += ZLMediaKitWebHookEvents_OnStreamChanged;
            ZLMediaKitWebHookEvents.OnStreamNoneReader += ZLMediaKitWebHookEvents_OnStreamNoneReader;
            ZLMediaKitWebHookEvents.OnStreamNotFound += ZLMediaKitWebHookEvents_OnStreamNotFound;

        }

        [Test]
        public void OnFlowReport_Test()
        {
            var dict = new Dictionary<string, object>
            {
               { "app" , "live" },
               { "duration" , 6 },
               { "params" , "token=1677193e-1244-49f2-8868-13b3fcc31b17" },
               { "player" , false },
               { "schema" , "rtmp" },
               { "stream" , "obs" },
               { "totalBytes" , 1508161 },
               { "vhost" , "__defaultVhost__" },
               { "ip" , "192.168.0.21" },
               { "port" , 55345 },
               { "id" , "140259799100960" }
            };
            var http = BaseUrl.AppendPathSegment("on_flow_report")
                .PostJsonAsync(dict).ContinueWith(task =>
                {
                    Assert.IsFalse(task.IsFaulted || task.Exception != null, message: "流量上报失败");
                    Assert.AreEqual(task.Result.StatusCode, HttpStatusCode.OK, message: "状态码不对");
                });
            http.Wait();

                
        }


        private void ZLMediaKitWebHookEvents_OnStreamNotFound(StreamNotFoundInfo obj)
        {
            Console.WriteLine("收到事件");
        }

        private StreamNoneReaderInfoResult ZLMediaKitWebHookEvents_OnStreamNoneReader(StreamNoneReaderInfo arg)
        {
            throw new NotImplementedException();
        }

        private void ZLMediaKitWebHookEvents_OnStreamChanged(StreamChangedInfo obj)
        {
            throw new NotImplementedException();
        }

        private ShellLonginResult ZLMediaKitWebHookEvents_OnShellLogin(ShellLoginInfo arg)
        {
            throw new NotImplementedException();
        }

        private void ZLMediaKitWebHookEvents_OnServerStarted(ServerConfig obj)
        {
            throw new NotImplementedException();
        }

        private RtspRealmInfoResult ZLMediaKitWebHookEvents_OnRtspRealm(RtspRealmInfo arg)
        {
            throw new NotImplementedException();
        }

        private RtspAuthResult ZLMediaKitWebHookEvents_OnRtspAuth(RtspAuthInfo arg)
        {
            throw new NotImplementedException();
        }

        private void ZLMediaKitWebHookEvents_OnRecordMP4(RecordInfo obj)
        {
            throw new NotImplementedException();
        }

        private PublishResult ZLMediaKitWebHookEvents_OnPublish(PublishInfo arg)
        {
            throw new NotImplementedException();
        }

        private PlayInfoResult ZLMediaKitWebHookEvents_OnPlay(PlayInfo arg)
        {
            throw new NotImplementedException();
        }

        private HttpAccessResult ZLMediaKitWebHookEvents_OnHttpAccess(HttpAccess arg)
        {
            throw new NotImplementedException();
        }

        private void ZLMediaKitWebHookEvents_OnFlowReport(FlowReport obj)
        {
            throw new NotImplementedException();
        }
    }
}
