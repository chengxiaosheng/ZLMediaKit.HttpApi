using System.Text.Json.Serialization;

namespace ZLMediaKit.Common.Dtos.HookInputDto
{
    /// <summary>
    /// shell登录鉴权
    /// </summary>
    public interface IShellLoginInput : IHookInputWithClient
    {
        /// <summary>
        /// telnet 终端登录用户密码
        /// </summary>
        [JsonPropertyName("passwd")]
        public  string Password { get;  }
        /// <summary>
        /// 用户名
        /// </summary>
        [JsonPropertyName("user_name")]
        public  string Username { get;  }
    }

    public class ShellLoginInput : HookInputWithClient, IShellLoginInput
    {
        public string Password { get; set; }

        public string Username { get; set; }
    }
}
