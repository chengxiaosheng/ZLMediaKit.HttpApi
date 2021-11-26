using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLMediaKit.Common.Dtos.HookInputDto
{
    /// <summary>
    /// rtsp/rtmp/rtp推流鉴权事
    /// </summary>
    public interface IPublishInput : IHookInputWithMediaBaseAndClient
    {

    }

    /// <summary>
    /// rtsp/rtmp/rtp推流鉴权事
    /// </summary>
    public class PublishInpu : HookWithMediaBaseAndClient, IPublishInput
    {

    }
}
