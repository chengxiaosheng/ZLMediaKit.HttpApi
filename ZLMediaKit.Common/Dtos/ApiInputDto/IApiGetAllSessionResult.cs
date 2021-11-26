namespace ZLMediaKit.Common.Dtos.ApiInputDto
{
    public interface IApiGetAllSessionResult : IApiResultListBase<ISocketInfo>
    {

    }

    public class ApiGetAllSessionResult : ApiResultListBase<ISocketInfo>, IApiGetAllSessionResult
    {

    }
}
