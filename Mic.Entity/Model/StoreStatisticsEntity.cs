using System;

namespace Mic.Entity.Model
{
    public class StoreStatisticsEntity
    {
        /// <summary>
        /// 商家总数量
        /// </summary>
        public int StoreCount { get; set; }
        /// <summary>
        /// 昨日新增商家数量
        /// </summary>
        public int StoreIncreaseYes { get; set; }
        /// <summary>
        /// 昨日活跃商家数量
        /// </summary>
        public int ActiveStoreYes { get; set; }
        /// <summary>
        /// 上周的昨天的活跃商家数
        /// </summary>
        public int ActiveStoreYesLastWeek { get; set; }
        /// <summary>
        /// 活跃商家同比上周变化
        /// </summary>
        public int ActiveStoreComparedLastWeek
        {
            get {
                var result = 0;
                if (ActiveStoreYesLastWeek == 0)
                {
                    result = 101;
                }
                else
                {
                    result = Convert.ToInt32((ActiveStoreYes - ActiveStoreYesLastWeek) / ActiveStoreYesLastWeek * 100);
                }
                return result;
            }
        }

        /// <summary>
        /// 昨日播放时长
        /// </summary>
        public int PlayTimeYes { get; set; }

        /// <summary>
        /// 上周昨日播放时长
        /// </summary>
        public int PlayTimeYesLastWeek { get; set; }

        public string PlayTimeYesLastWeekStr
        {
            get {
                var temp = string.Empty;
                int hour = Convert.ToInt32(Math.Floor(PlayTimeYesLastWeek / 3600.0));
                int min = Convert.ToInt32(Math.Floor((PlayTimeYesLastWeek - 3600 * hour) / 60.0));
                int sec = PlayTimeYesLastWeek - 3600 * hour - 60 * min;
                return hour + ":" + min + ":" + sec;
            }
        }

        /// <summary>
        /// 播放时长同比上周
        /// </summary>
        public int PlayTimeComparedLastWeek
        {
            get {
                var result = 0;
                if (PlayTimeYesLastWeek == 0)
                {
                    result = 101;
                }
                else
                {
                    result = Convert.ToInt32((PlayTimeYes - PlayTimeYesLastWeek) / PlayTimeYesLastWeek * 100);
                }
                return result;
            }
        }
        /// <summary>
        /// 昨日播放次数
        /// </summary>
        public int PlayTimesYes { get; set; }

        /// <summary>
        /// 上周昨日播放次数
        /// </summary>
        public int PlayTimesYesLastWeek { get; set; }

        /// <summary>
        /// 播放次数 同比上周变化
        /// </summary>
        public double PlayTimesComparedLastWeek
        {
            get {
                var result = 0;
                if (PlayTimesYesLastWeek == 0)
                {
                    result = 101;
                }
                else
                {
                    result = Convert.ToInt32((PlayTimesYes - PlayTimesYesLastWeek) / PlayTimesYesLastWeek * 100);
                }
                return result;
            }
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class StoreSongStatisticsEntity
    {
        public string StoreName { get; set; }
        public int PlaySongCount { get; set; }
        public int PlayUserId { get; set; }
        public int PlayTime { get; set; }
        public string PlayTimeStr
        {
            get {
                var temp = string.Empty;
                int hour = Convert.ToInt32(Math.Floor(PlayTime / 3600.0));
                int min = Convert.ToInt32(Math.Floor((PlayTime - 3600 * hour) / 60.0));
                int sec = PlayTime - 3600 * hour - 60 * min;
                return hour + ":" + min + ":" + sec;
            }
        }
        public int PlayCount { get; set; }
    }

    public class StoreStatisticsInfoEntity
    {
        public int PlayTime { get; set; }
        public string PlayTimeStr
        {
            get {
                var temp = string.Empty;
                int hour = Convert.ToInt32(Math.Floor(PlayTime / 3600.0));
                int min = Convert.ToInt32(Math.Floor((PlayTime - 3600 * hour) / 60.0));
                int sec = PlayTime - 3600 * hour - 60 * min;
                return hour + ":" + min + ":" + sec;
            }
        }
        public int PlayCount { get; set; }
    }

    public class StorePlaySongEntity
    {
        public int SongId { get; set; }
        public string SongName { get; set; }
        public int PlayTime { get; set; }
        public string PlayTimeStr
        {
            get {
                var temp = string.Empty;
                int hour = Convert.ToInt32(Math.Floor(PlayTime / 3600.0));
                int min = Convert.ToInt32(Math.Floor((PlayTime - 3600 * hour) / 60.0));
                int sec = PlayTime - 3600 * hour - 60 * min;
                return hour + ":" + min + ":" + sec;
            }
        }
        public int PlayCount { get; set; }
    }



    public class SingerStatisticsEntity
    {
        public int SingerCount { get; set; }
        
        public int SingerIncreaseYes { get; set; }
        
        public int ActiveSingerYes { get; set; }
        
        public int ActiveSingerYesLastWeek { get; set; }
      
        public int ActiveSingerComparedLastWeek
        {
            get {
                var result = 0;
                if (ActiveSingerYesLastWeek == 0)
                {
                    result = 101;
                }
                else
                {
                    result = Convert.ToInt32((ActiveSingerYes - ActiveSingerYesLastWeek) / ActiveSingerYesLastWeek * 100);
                }
                return result;
            }
        }
    }

    /// <summary>
    /// 用来统计各个音乐人的作品情况
    /// </summary>
    public class SingerListStatisticsEntity
    {
        public int SingerId { get; set; }
        public string SingerName { get; set; }
        /// <summary>
        /// 上传作品数量
        /// </summary>
        public int UploadCount { get; set; }
        /// <summary>
        /// 发布作品数量
        /// </summary>
        public int PublishCount { get; set; }
        /// <summary>
        /// 播放商家数
        /// </summary>
        public int PlayStoreCount { get; set; }
        /// <summary>
        /// 播放作品数
        /// </summary>
        public int PlaySongCount { get; set; }
    }
}
