namespace ZLMediaKit.Common.Dtos.ApiInputDto
{
    public interface IApiGetMediaInfo: IApiResultBase<IMediaInfo>
    {

    }

    public class ApiGetMediaInfo : ApiResultBase<IMediaInfo>, IApiGetMediaInfo
    {
       
    }
}
