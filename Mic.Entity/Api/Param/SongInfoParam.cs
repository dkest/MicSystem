﻿using System;

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
        //public string SongPath { get; set; }//歌曲文件在服务器的路径
        //public string FullSongPath
        //{
        //    get { return WebConfig.RootUrl + SongPath; }
        //}
        public int PlayTimes { get; set; } //累计播放次数
        public int TotalPlayTime { get; set; }//累计播放时长

        public string SongMarkStr { get; set; }//歌曲标签
        public DateTime ExpirationTime { get; set; } // 版权到期时间
        public DateTime UploadTime { get; set; } //上传时间
        public int AuditStatus { get; set; } //审核状态
    }


    public class SongByStoreParam
    {
        public int Id { get; set; }
        public string StoreName { get; set; }
        public string StoreTypeName { get; set; }
        public int PlayTimes { get; set; }
        public int TotalPlayTime { get; set; }
        public int StoreCount { get; set; }
    }

    public class SongByStoreRecordParam
    {
        public DateTime BeginPlayTime { get; set; }
        public string StoreName { get; set; }
        public string StoreTypeName { get; set; }
    }



    public class UploadSongParam
    {
        public int Id { get; set; }
        public string SongName { get; set; }
        public int SingerId { get; set; }
        public string SingerName { get; set; }
        public string SongLength { get; set; }
        public int SongSize { get; set; }
        public string SongPath { get; set; }
        public string CopyrightFilePath { get; set; }
    }


}
