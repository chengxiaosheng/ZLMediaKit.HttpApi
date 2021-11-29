public interface IZLMediaKitSettings
{

    /// <summary>
    /// ip地址
    /// </summary>
    public string IpAddress { get; set; }
    /// <summary>
    /// api 端口
    /// </summary>
    /// <value></value>
    public int ApiPort { get; set; }
    /// <summary>
    /// 密钥 
    /// </summary>
    /// <value></value>
    public string ApiSecret { get; set; }
    /// <summary>
    /// api 秦艽超时时间 
    /// </summary>
    /// <value></value>
    public int Timeout { get; set; }
    /// <summary>
    /// 服务唯一编码 
    /// </summary>
    /// <value></value>
    public string MediaServerId { get; set; }
    /// <summary>
    /// api 请求模式， http / https 
    /// </summary>
    /// <value></value>   
    public HttpSchema HttpSchema { get; set; }

    public string ApiBaseUri { get; }
}

/// <summary>
/// 基本配置
/// </summary>
public class ZLMediaKitSettings : IZLMediaKitSettings
{
    public string IpAddress { get; set; } = "127.0.0.1";
    public int ApiPort { get; set; } = 80;
    public string ApiSecret { get; set; } = "035c73f7-bb6b-4889-a715-d9eb2d1925cc";
    public int Timeout { get; set; } = 2000;
    public string MediaServerId { get; set; } = "your_server_id";
    public HttpSchema HttpSchema { get; set; } = HttpSchema.Http;

    public string ApiBaseUri => $"{HttpSchema.ToString()}://{this.IpAddress}:{this.ApiPort}/index/api";
}


/// <summary>
/// http 请求模式
/// </summary>
public enum HttpSchema : byte
{
    Http,
    Https
}