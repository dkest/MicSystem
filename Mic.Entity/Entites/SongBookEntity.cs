using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mic.Entity
{
    public class SongBookEntity
    {
        public int Id { get; set; }
        public string SongName { get; set; }
        public string SingerName { get; set; }
        public int SingerId { get; set; }
        public string SongLength { get; set; } //歌曲时长，00:03:26
        public DateTime ExpirationTime { get; set; } // 版权到期时间
        public string CopyrightFilePath { get; set; } //版权到期证明
        public string FullCopyrightFilePath { get { return   WebConfig.RootUrl + CopyrightFilePath; } }
        public int PlayTimes { get; set; } // 播放次数
        public string SongMark { get; set; } //关联歌曲标签表Id，多个标签用逗号分割开
        public string SongPath { get; set; }//歌曲文件在服务器的路径
        public string FullSongPath
        {
            get { return WebConfig.RootUrl + SongPath; }
        }
        public int SongSize { get; set; } //歌曲文件大小
        public string SongBPM { get; set; } //歌曲BPM

        public DateTime UploadTime { get; set; } //歌曲上传时间
        public string UploadTimeStr
        {
            get {
                return UploadTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
        public int PublishTimes { get; set; } // 歌曲第几次发布
        public int AuditStatus { get; set; }//审核状态 0-待发布；1-待审核；2-已通过；3-未通过
        public string AuditStatusStr
        {
            get {
                string result = string.Empty;
                switch (AuditStatus)
                {
                    case 0:
                        result = "待发布";
                        break;
                    case 1:
                        result = "待审核";
                        break;
                    case 2:
                        result = "已通过";
                        break;
                    case 3:
                        result = "未通过";
                        break;
                    default:
                        result = "未知";
                        break;
                }
                return result;
            }
        }


        //StoreIdList { get; set; }
        public bool Status { get; set; }
        public string Memo { get; set; } //

        public int TotalPlayTime { get; set; }//累计播放时长
        public string TotalPlayTimeStr
        {
            get {
                var temp = string.Empty;
                int hour = Convert.ToInt32(Math.Floor(TotalPlayTime / 3600.0));
                int min = Convert.ToInt32(Math.Floor((TotalPlayTime - 3600 * hour) / 60.0));
                int sec = TotalPlayTime - 3600 * hour - 60 * min;
                return hour + ":" + min + ":" + sec;
            }
        }
        public string SongMarkStr { get; set; }

    }
}
