using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ZLMediaKit.HttpApi
{
    /// <summary>
    /// 
    /// </summary>
    public static class ZLHttpClientRegister
    {
        /// <summary>
        /// ZLMediaKit Client 注册
        /// </summary>
        /// <param name="zLMediaKitSettings"></param>
        public static void Register(ZLMediaKitSettings zLMediaKitSettings)
        {
            if (zLMediaKitSettings == null) throw new InvalidDataException("ZLMediaKitSettings 不能为空");
            if (string.IsNullOrEmpty(zLMediaKitSettings.IpAddress)) throw new InvalidDataException("IpAddress 不能为空");
            if (zLMediaKitSettings.ApiPort == 0) throw new InvalidDataException("ApiPort 参数值错误");
            if (string.IsNullOrEmpty(zLMediaKitSettings.MediaServerId)) throw new InvalidDataException("MediaServerId 不能为空");
            if (string.IsNullOrEmpty(zLMediaKitSettings.ApiSecret)) throw new InvalidDataException("ApiSecret 不能为空");
            ZLMediaKitSettings.ZLMediaKitSettingsDict[zLMediaKitSettings.MediaServerId] = zLMediaKitSettings;
        }
    }
}
