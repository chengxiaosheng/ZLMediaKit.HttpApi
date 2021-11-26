

namespace ZLMediaKit.Common.Dtos.ApiInputDto
{
    public interface ISetServerConfigInput
    {
        /// <summary>
        /// Config.ini 中的 [] 节点名称
        /// 如 api.apiDebug , 此处填写 api
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 具体配置项
        /// 如 api.apiDebug , 此处填写 apiDebug
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 配置值 如 0 
        /// </summary>
        public object Value { get; set; }
    }

    public class SetServerConfigInput : ISetServerConfigInput
    {
        /// <summary>
        /// Config.ini 中的 [] 节点名称
        /// 如 api.apiDebug , 此处填写 api
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 具体配置项
        /// 如 api.apiDebug , 此处填写 apiDebug
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 配置值 如 0 
        /// </summary>
        public object Value { get; set; }
    }
}
