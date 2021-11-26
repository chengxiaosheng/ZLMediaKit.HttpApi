using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLMediaKit.Common.Dtos.HookInputDto
{
    /// <summary>
    /// 流无人观看时事件，用户可以通过此事件选择是否关闭无人看的流。
    /// </summary>
    public interface IStreamNoneReaderInput : IHookInputWithMediaBase
    {
    }

    /// <summary>
    /// 流无人观看时事件，用户可以通过此事件选择是否关闭无人看的流。
    /// </summary>
    public class StreamNoneReaderInput :HookWithMediaBase, IStreamNoneReaderInput { }
}
