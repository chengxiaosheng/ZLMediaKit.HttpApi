using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLMediaKit.Common.Dtos.ApiInputDto
{
    public interface IApiGetStatisticResult : IApiResultBase<IKeepalive>
    {

    }
    public class ApiGetStatisticResult : ApiResultBase<IKeepalive>, IApiGetStatisticResult
    {

    }
}
