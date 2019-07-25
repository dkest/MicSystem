using System;

namespace Mic.Entity
{
    public class SongInfoParam
    {
        public int Id { get; set; }
        public string SongName { get; set; }
        public string SingerName { get; set; }
        public int SingerId { get; set; }
        public string SongLength { get; set; } //歌曲时长，00:03:26
        public string SongMark { get; set; } //关联歌曲标签表Id，多个标签用逗号分割开
        public string SongPath { get; set; }//歌曲文件在服务器的路径
        public string FullSongPath
        {
            get { return WebConfig.RootUrl + SongPath; }
        }
        public int PlayTimes { get; set; } //累计播放次数
        public int TotalPlayTime { get; set; }//累计播放时长
       
        public string SongMarkStr { get; set; }//歌曲标签
        public DateTime ExpirationTime { get; set; } // 版权到期时间
    }
}
