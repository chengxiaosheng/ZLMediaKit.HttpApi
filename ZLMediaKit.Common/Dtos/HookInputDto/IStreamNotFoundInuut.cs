using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLMediaKit.Common.Dtos.HookInputDto
{
    /// <summary>
    /// 流未找到事件，用户可以在此事件触发时，立即去拉流，这样可以实现按需拉流；此事件对回复不敏感。
    /// </summary>
    public interface IStreamNotFoundInuut : IHookInputWithMediaBaseAndClient
    {
    }

    /// <summary>
    /// 流未找到事件，用户可以在此事件触发时，立即去拉流，这样可以实现按需拉流；此事件对回复不敏感。
    /// </summary>
    public class StreamNotFoundInuut :HookWithMediaBaseAndClient, IStreamNotFoundInuut { }
}
