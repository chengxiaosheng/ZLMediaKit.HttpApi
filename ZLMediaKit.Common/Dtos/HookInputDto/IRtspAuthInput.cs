using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ZLMediaKit.Common.Dtos.HookInputDto
{
    /// <summary>
    /// rtsp专用的鉴权事件，先触发on_rtsp_realm事件然后才会触发on_rtsp_auth事件
    /// </summary>
    public interface IRtspAuthInput : IHookInputWithMediaBaseAndClient
    {
        /// <summary>
        /// 请求的密码是否必须为明文(base64鉴权需要明文密码)
        /// </summary>
        [JsonPropertyName("must_no_encrypt")]
        public bool MustNoEncrypt { get; set; }

        /// <summary>
        /// rtsp播放鉴权加密realm
        /// </summary>
        [JsonPropertyName("realm")]
        public string Realm { get; set; }

        /// <summary>
        /// 播放用户名
        /// </summary>
        [JsonPropertyName("user_name")]
        public string UserName { get; set; }
    }

    /// <summary>
    /// rtsp专用的鉴权事件，先触发on_rtsp_realm事件然后才会触发on_rtsp_auth事件
    /// </summary>
    public class RtspAuthInput : HookWithMediaBaseAndClient, IRtspAuthInput
    {
        public bool MustNoEncrypt { get; set; }

        public string Realm { get; set; }

        public string UserName { get; set; }
    }
}
