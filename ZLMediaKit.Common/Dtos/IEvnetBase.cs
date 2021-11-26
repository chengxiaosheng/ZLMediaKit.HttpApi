namespace ZLMediaKit.Common.Dtos
{
    public interface IEvnetBase
    {
        /// <summary>
        /// 服务器信息
        /// </summary>
        public IServerManager ServerManager { get; set; }

      
    }

    public class EvnetBase : IEvnetBase
    {
        public EvnetBase() { }
        public EvnetBase(IServerManager serverManager = null)
        {
            ServerManager = serverManager;
        }

        public IServerManager ServerManager { get; set; }
        
    }
}
