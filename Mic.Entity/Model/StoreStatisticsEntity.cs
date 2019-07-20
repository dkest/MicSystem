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
        /// 同比上周变化
        /// </summary>
        public double ActiveStoreComparedLastWeek { get; set; }

        /// <summary>
        /// 昨日播放时长
        /// </summary>
        public int PlayTimeYes { get; set; }
       
        /// <summary>
        /// 上周昨日播放时长
        /// </summary>
        public int PlayTimeYesLastWeek { get; set; }
     
        public double PlayTimeComparedLastWeek { get; set; }

        /// <summary>
        /// 昨日播放次数
        /// </summary>
        public int PlayTimesYes { get; set; }

        /// <summary>
        /// 上周昨日播放次数
        /// </summary>
        public int PlayTimesYesLastWeek { get; set; }

        public double PlayTimesComparedLastWeek { get; set; }
    }
}
