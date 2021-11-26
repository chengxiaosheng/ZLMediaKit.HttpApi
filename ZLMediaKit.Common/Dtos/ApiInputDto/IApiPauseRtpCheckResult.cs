using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLMediaKit.Common.Dtos.ApiInputDto
{
    public interface IApiPauseRtpCheckResult : IApiResultBase
    {
    }

    public class ApiPauseRtpCheckResult : ApiResultBase, IApiPauseRtpCheckResult
    {

    }
}
