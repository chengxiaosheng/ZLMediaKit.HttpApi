using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLMediaKit.Common.Dtos.HookInputDto
{
    /// <summary>
    /// 该rtsp流是否开启rtsp专用方式的鉴权事件，开启后才会触发on_rtsp_auth事件
    /// </summary>
    public interface IRtspRealmInput : IHookInputWithMediaBaseAndClient
    {

    }

    /// <summary>
    /// 该rtsp流是否开启rtsp专用方式的鉴权事件，开启后才会触发on_rtsp_auth事件
    /// </summary>
    public class RtspRealmInput: HookWithMediaBaseAndClient, IRtspRealmInput { }
}
