using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

}
