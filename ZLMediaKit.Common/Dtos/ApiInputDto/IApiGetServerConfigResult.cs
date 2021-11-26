namespace ZLMediaKit.Common.Dtos.ApiInputDto
{
    public interface IApiGetServerConfigResult: IApiResultBase<IServerConfig>
    {
    }
    public class ApiGetServerConfigResult : ApiResultBase<IServerConfig>, IApiGetServerConfigResult
    {

    }
}
