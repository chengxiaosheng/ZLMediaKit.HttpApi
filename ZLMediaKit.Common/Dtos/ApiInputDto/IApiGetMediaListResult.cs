﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLMediaKit.Common.Dtos.ApiInputDto
{
    public interface IApiGetMediaListResult : IApiResultListBase<IMediaInfo>
    {

    }
    public class ApiGetMediaListResult : ApiResultListBase<IMediaInfo>, IApiGetMediaListResult
    {

    }
}
