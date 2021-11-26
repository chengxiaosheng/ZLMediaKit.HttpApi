using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLMediaKit.Common
{
    public enum OriginTypeEnum
    {
        unknown = 0, 
        rtmp_push = 1, 
        rtsp_push = 2, 
        rtp_push = 3, pull = 4, 
        ffmpeg_pull = 5, 
        mp4_vod = 6, 
        device_chn = 7, 
        rtc_push = 8
    }
}
