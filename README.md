# ZLMediaKit.HttpApi
基于优秀的开源项目 [ZLMediaKit](http://https://github.com/xia-chu/ZLMediaKit) 提供的[Restful Api](https://github.com/xia-chu/ZLMediaKit/wiki/MediaServer%E6%94%AF%E6%8C%81%E7%9A%84HTTP-API) 实现数据请求

## 实现功能
+ 封装 ZLMediaKit的Restful Api接口，提供快捷访问 

## 快速开始

1. 注入服务
```C#
    services.AddZLMediaKitHttpClient(option =>
    {
        //ZLMediaKit IP地址
        option.IpAddress = "127.0.0.1";
        //ZLMediaKit Http 端口
        option.ApiPort = 80;
        //ZLMediaKit 密钥
        option.ApiSecret = "035c73f7-bb6b-4889-a715-d9eb2d1925cc";
    });
```
2. 使用
```C#
    public class Test
    {
        private readonly ZLHttpClient _zlhttpClient;
        public Test(ZLHttpClient zlhttpClient)
        {
            this._zlhttpClient = ;zlhttpClient
        }

        public CallTest()
        {
            zlhttpClient.GetThreadsLoad();
            zlhttpClient.GetWorkThreadsLoad();
            zlhttpClient.GetServerConfig();
            zlhttpClient.SetServerConfig();
            zlhttpClient.RestartServer();
            zlhttpClient.GetMediaList();
           .
           .
           .
        }

    }
```

---

# ZLMediaKit.WebHook
基于优秀的开源项目 [ZLMediaKit](http://https://github.com/xia-chu/ZLMediaKit) 提供的 [Web Hook](https://github.com/xia-chu/ZLMediaKit/wiki/MediaServer%E6%94%AF%E6%8C%81%E7%9A%84HTTP-HOOK-API)实现接收请求

## 实现功能
+ 接入 ZLMediaKit WebHook
+ 自定义路由路径
+ 以事件的形式访问

## 快速开始
1. 注入服务
```C#
    //routePrefix 参数设定路由前缀
    //实际接口路径为 routePrefix/action
    //ps : index/hook/on_flow_report
    //routePrefix = app/index/api/test
    //ps : app/index/api/test/on_flow_report
    services.AddMediaKitWebHook(new APIRouteOptions(routePrefix : "index/hook"); 
```
2. 绑定事件
```
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
   ZLMediaKitWebHookEvents.OnStreamNoneReader += LMediaKitWebHookEvents_OnStreamNoneReader;
   ZLMediaKitWebHookEvents.OnStreamNotFound += LMediaKitWebHookEvents_OnStreamNotFound;
```


# 致敬

一个优秀而强大的流媒体服务器 [ZLMediaKit](https://github.com/xia-chu/ZLMediaKit)
