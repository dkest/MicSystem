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
        public string FullCopyrightFilePath { get { return "http://152.136.227.143" + CopyrightFilePath; } }
        public int PlayTimes { get; set; } // 播放次数
        public string SongMark { get; set; } //关联歌曲标签表Id，多个标签用逗号分割开
        public string SongPath { get; set; }//歌曲文件在服务器的路径
        public string FullSongPath
        {
            get { return "http://152.136.227.143" + SongPath; }
        }
        public int SongSize { get; set; } //歌曲文件大小
        public string SongBPM { get; set; } //歌曲BPM

        public DateTime UploadTime { get; set; } //歌曲上传时间
        public int PublishTimes { get; set; } // 歌曲第几次发布
        public int AuditStatus { get; set; }//审核状态 0-待发布；1-待审核；2-已通过；3-未通过

        //StoreIdList { get; set; }
        public bool Status { get; set; }
        public string Memo { get; set; } //
    }
}
