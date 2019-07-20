using Dapper;
using Mic.Entity;
using Mic.Entity.Model;
using Mic.Repository.Dapper;
using Mic.Repository.IRepositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Mic.Repository.Repositories
{

    public class StoreStatisticsRepository
    {
        DapperHelper<SqlConnection> helper;
        public StoreStatisticsRepository()
        {
            helper = new DapperHelper<SqlConnection>(WebConfig.SqlConnection);
        }


        /// <summary>
        /// 获取商家统计总览
        /// </summary>
        /// <returns></returns>
        public StoreStatisticsEntity GetStoreStatistics()
        {
            var yesNow = DateTime.Now.AddDays(-1);
            var yes = new DateTime(yesNow.Year, yesNow.Month, yesNow.Day, 0, 0, 0);
            var yesLastWeek = yes.AddDays(-7);
            var storeStatistics = new StoreStatisticsEntity();
            List<string> sqlList = new List<string>();
            //商家总数量
            string storeCountSql = "select Count(1) as total from [User] where UserType=2 and IsMain=1;";
            sqlList.Add(storeCountSql);
            //昨日新增商家数量
            string storeCountYesSql = $@"select Count(1)  as total from [User] a left join [StoreDetailInfo] b on a.Id=b.UserId 
where a.UserType=2 and a.IsMain=1 and b.CreateTime>'{yes}' and b.CreateTime<'{yes.AddDays(1).AddSeconds(-1)}';";
            sqlList.Add(storeCountYesSql);
            //昨日活跃商家数量
            string activeStoreYesSql = $@"select Count(1)  as total from [User]  
where UserType=2 and IsMain=1 and LastLoginTime>'{yes}' and LastLoginTime<'{yes.AddDays(1).AddSeconds(-1)}';";
            sqlList.Add(activeStoreYesSql);
            //上周的昨日活跃商家数量
            string activeStoreYesLastWeekSql = $@"select Count(1)  as total from [User]  
where UserType=2 and IsMain=1 and LastLoginTime>'{yesLastWeek}' and LastLoginTime<'{yesLastWeek.AddDays(1).AddSeconds(-1)}';";
            sqlList.Add(activeStoreYesLastWeekSql);


            //昨日播放时长
            string playTimeStoreYesSql = $@"select SUM([BroadcastTime])  as total from [dbo].[SongPlayRecord]  
where  [BeginPlayTime]>'{yes}' and [BeginPlayTime]<'{yes.AddDays(1).AddSeconds(-1)}';";
            sqlList.Add(playTimeStoreYesSql);
            //上周的昨日播放时长
            string playTimeStoreYesLastWeekSql = $@"select SUM([BroadcastTime])  as total from [dbo].[SongPlayRecord]  
where  [BeginPlayTime]>'{yesLastWeek}' and [BeginPlayTime]<'{yesLastWeek.AddDays(1).AddSeconds(-1)}';";
            sqlList.Add(playTimeStoreYesLastWeekSql);


            //昨日播放次数
            string playTimesStoreYesSql = $@"select Count(1)  as total from [dbo].[SongPlayRecord]  
where  [BeginPlayTime]>'{yes}' and [BeginPlayTime]<'{yes.AddDays(1).AddSeconds(-1)}';";
            sqlList.Add(playTimesStoreYesSql);
            //上周的昨日播放次数
            string playTimesStoreYesLastWeekSql = $@"select Count(1)  as total from [dbo].[SongPlayRecord]  
where  [BeginPlayTime]>'{yesLastWeek}' and [BeginPlayTime]<'{yesLastWeek.AddDays(1).AddSeconds(-1)}';";
            sqlList.Add(playTimesStoreYesLastWeekSql);

            var result = helper.QueryMultiple(sqlList).ToList();

            var data0 = (IDictionary<string, object>)result[0].FirstOrDefault(); ;
            storeStatistics.StoreCount = Convert.ToInt32(data0["total"]);

            var data1 = (IDictionary<string, object>)result[1].FirstOrDefault(); ;
            storeStatistics.StoreIncreaseYes = Convert.ToInt32(data1["total"]);

            var data2 = (IDictionary<string, object>)result[2].FirstOrDefault(); ;
            storeStatistics.ActiveStoreYes = Convert.ToInt32(data2["total"]);

            var data3 = (IDictionary<string, object>)result[3].FirstOrDefault(); ;
            storeStatistics.ActiveStoreYesLastWeek = Convert.ToInt32(data3["total"]);

            var data4 = (IDictionary<string, object>)result[4].FirstOrDefault(); ;
            storeStatistics.PlayTimeYes = Convert.ToInt32(data4["total"]);

            var data5 = (IDictionary<string, object>)result[5].FirstOrDefault(); ;
            storeStatistics.PlayTimeYesLastWeek = Convert.ToInt32(data5["total"]);

            var data6 = (IDictionary<string, object>)result[6].FirstOrDefault(); ;
            storeStatistics.PlayTimesYes = Convert.ToInt32(data6["total"]);

            var data7 = (IDictionary<string, object>)result[7].FirstOrDefault(); ;
            storeStatistics.PlayTimesYesLastWeek = Convert.ToInt32(data7["total"]);


            return storeStatistics;
        }

        /// <summary>
        /// 获取商家播放歌曲列表信息
        /// </summary>
        /// <returns></returns>
        public List<StoreSongStatisticsEntity> GetStorePlaySongInfo(StorePlaySongPageParam param)
        {
            string order = string.Empty;
            switch (param.OrderField)
            {
                case "PlayTime":
                    order = "Sum(BroadcastTime)";
                    break;
                case "PlaySongCount":
                    order = "count( distinct a.SongId) ";
                    break;
                case "PlayCount":
                    order = "count(1)";
                    break;
            }
            string sql = $@"select top {param.PageSize} * from (select row_number() over(order by {order} desc) as rownumber,
b.StoreName, count( distinct a.SongId) as  PlaySongCount, PlayUserId,Sum(BroadcastTime) as PlayTime,count(1) as PlayCount from [dbo].[SongPlayRecord] a  left join StoreDetailInfo b
on a.PlayUserId = b.UserId where a.BeginPlayTime >= '{param.BeginDate}' and a.BeginPlayTime <= '{param.EndDate}'
group by a.PlayUserId,b.StoreName ) temp_row
                    where temp_row.rownumber>(({param.PageIndex}-1)*{param.PageSize}) ;";
            return helper.Query<StoreSongStatisticsEntity>(sql).ToList();
        }


        public StoreStatisticsInfoEntity GetStoreStatisticsInfo(int storeId,DateTime beginDate,DateTime endDate)
        {
            string sql = $@" select Sum(BroadcastTime) as PlayTime,count(1) as PlayCount from [dbo].[SongPlayRecord] 
where BeginPlayTime >= '{beginDate}' and BeginPlayTime <= '{endDate.AddDays(1).AddSeconds(-1)}' and PlayUserId= {storeId}";
            return helper.Query<StoreStatisticsInfoEntity>(sql).FirstOrDefault();
        }

        public List<StorePlaySongEntity> GetStorePlaySongList(StorePlaySongPageParam param)
        {
            string order = string.Empty;
            switch (param.OrderField)
            {
                case "PlayTime":
                    order = "Sum(BroadcastTime)";
                    break;
                case "PlayCount":
                    order = "count(1)";
                    break;
            }
            string sql = $@" select top {param.PageSize} * from (select row_number() over(order by {order} desc) as rownumber, a.SongId, b.SongName,Sum(BroadcastTime) as PlayTime,count(1) as PlayCount 
 from [dbo].[SongPlayRecord] a  left join SongBook b
on a.SongId = b.Id where a.BeginPlayTime >= '{param.BeginDate}' and  a.BeginPlayTime <='{param.EndDate}' and PlayUserId={param.PlayUserId}
 group by a.SongId,b.SongName   ) temp_row
                    where temp_row.rownumber>(({param.PageIndex}-1)*{param.PageSize}) ;";
            return helper.Query<StorePlaySongEntity>(sql).ToList();
        }
    }
}
