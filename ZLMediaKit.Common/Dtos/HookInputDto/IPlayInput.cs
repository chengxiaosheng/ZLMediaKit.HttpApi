using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLMediaKit.Common.Dtos.HookInputDto
{
    /// <summary>
    /// 播放器鉴权事件
    /// </summary>
    public interface IPlayInput : IHookInputWithMediaBaseAndClient
    {

    }

    /// <summary>
    /// 播放器鉴权事件
    /// </summary>
    public class PlayInput: HookWithMediaBaseAndClient , IPlayInput
    {

    }
}
