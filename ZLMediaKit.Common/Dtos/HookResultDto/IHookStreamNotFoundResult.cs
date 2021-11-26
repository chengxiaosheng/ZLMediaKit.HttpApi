using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLMediaKit.Common.Dtos.HookResultDto
{
    public interface IHookStreamNotFoundResult : IHookCommonResult
    {
    }
    public class HookStreamNotFoundResult : HookCommonResult, IHookStreamNotFoundResult
    {

    }
}
