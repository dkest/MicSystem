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

    public class SingerStatisticsRepository
    {
        DapperHelper<SqlConnection> helper;
        public SingerStatisticsRepository()
        {
            helper = new DapperHelper<SqlConnection>(WebConfig.SqlConnection);
        }


        /// <summary>
        /// 获取音乐人统计总览
        /// </summary>
        /// <returns></returns>
        public SingerStatisticsEntity GetSingerStatistics()
        {
            var yesNow = DateTime.Now.AddDays(-1);
            var yes = new DateTime(yesNow.Year, yesNow.Month, yesNow.Day, 0, 0, 0);
            var yesLastWeek = yes.AddDays(-7);
            var singerStatistics = new SingerStatisticsEntity();
            List<string> sqlList = new List<string>();
            //音乐人总数量
            string singerCountSql = "select Count(1) as total from [User] where UserType=1;";
            sqlList.Add(singerCountSql);
            //昨日新增音乐人数量
            string singerCountYesSql = $@"select Count(1)  as total from [User] a left join [SingerDetailInfo] b on a.Id=b.UserId 
where a.UserType=1  and b.CreateTime>'{yes}' and b.CreateTime<'{yes.AddDays(1).AddSeconds(-1)}';";
            sqlList.Add(singerCountYesSql);
            //昨日活跃音乐人数量
            string activeSingerYesSql = $@"select Count(1)  as total from [User]  
where UserType=1  and LastLoginTime>'{yes}' and LastLoginTime<'{yes.AddDays(1).AddSeconds(-1)}';";
            sqlList.Add(activeSingerYesSql);
            //上周的昨日活跃音乐人数量
            string activeSingerYesLastWeekSql = $@"select Count(1)  as total from [User]  
where UserType=1 and LastLoginTime>'{yesLastWeek}' and LastLoginTime<'{yesLastWeek.AddDays(1).AddSeconds(-1)}';";
            sqlList.Add(activeSingerYesLastWeekSql);


            

            var result = helper.QueryMultiple(sqlList).ToList();

            var data0 = (IDictionary<string, object>)result[0].FirstOrDefault(); ;
            singerStatistics.SingerCount = Convert.ToInt32(data0["total"]);

            var data1 = (IDictionary<string, object>)result[1].FirstOrDefault(); ;
            singerStatistics.SingerIncreaseYes = Convert.ToInt32(data1["total"]);

            var data2 = (IDictionary<string, object>)result[2].FirstOrDefault(); ;
            singerStatistics.ActiveSingerYes = Convert.ToInt32(data2["total"]);

            var data3 = (IDictionary<string, object>)result[3].FirstOrDefault(); ;
            singerStatistics.ActiveSingerYesLastWeek = Convert.ToInt32(data3["total"]);

            return singerStatistics;
        }

        /// <summary>
        /// 统计音乐人的各类作品数量 列表
        /// </summary>
        /// <returns></returns>
        public List<SingerListStatisticsEntity> GetSingerStatisticsList(StorePlaySongPageParam param)
        {
            string order = string.Empty;
            switch (param.OrderField)
            {
                case "PlaySongCount":
                    order = "COUNT(distinct b.SongId)";
                    break;
                case "PlayStoreCount":
                    order = "COUNT( b.PlayUserId)  ";
                    break;
            }
            string sql = $@"select top {param.PageSize} * from (select row_number() over(order by {order} desc) as rownumber,
SingerId,SingerName,
SUM(CASE  WHEN a.AuditStatus in ('0','1','2','3') THEN 1 ELSE 0 END) AS UploadCount,
SUM(CASE a.AuditStatus WHEN '2' THEN 1 ELSE 0 END) AS  PublishCount,
COUNT( b.PlayUserId) PlayStoreCount,
COUNT(distinct b.SongId) PlaySongCount
  from SongBook a left join SongPlayRecord b on a.Id=b.SongId 
 where a.SingerId is not  null and a.UploadTime >'{param.BeginDate}' and a.UploadTime <'{param.EndDate}'  group by SingerId,SingerName ) temp_row
                    where temp_row.rownumber>(({param.PageIndex}-1)*{param.PageSize}) ;";
            return helper.Query<SingerListStatisticsEntity>(sql).ToList();
        }


//        public StoreStatisticsInfoEntity GetStoreStatisticsInfo(int storeId,DateTime beginDate,DateTime endDate)
//        {
//            string sql = $@" select Sum(BroadcastTime) as PlayTime,count(1) as PlayCount from [dbo].[SongPlayRecord] 
//where BeginPlayTime >= '{beginDate}' and BeginPlayTime <= '{endDate.AddDays(1).AddSeconds(-1)}' and PlayUserId= {storeId}";
//            return helper.Query<StoreStatisticsInfoEntity>(sql).FirstOrDefault();
//        }

//        public List<StorePlaySongEntity> GetStorePlaySongList(StorePlaySongPageParam param)
//        {
//            string order = string.Empty;
//            switch (param.OrderField)
//            {
//                case "PlayTime":
//                    order = "Sum(BroadcastTime)";
//                    break;
//                case "PlayCount":
//                    order = "count(1)";
//                    break;
//            }
//            string sql = $@" select top {param.PageSize} * from (select row_number() over(order by {order} desc) as rownumber, a.SongId, b.SongName,Sum(BroadcastTime) as PlayTime,count(1) as PlayCount 
// from [dbo].[SongPlayRecord] a  left join SongBook b
//on a.SongId = b.Id where a.BeginPlayTime >= '{param.BeginDate}' and  a.BeginPlayTime <='{param.EndDate}' and PlayUserId={param.PlayUserId}
// group by a.SongId,b.SongName   ) temp_row
//                    where temp_row.rownumber>(({param.PageIndex}-1)*{param.PageSize}) ;";
//            return helper.Query<StorePlaySongEntity>(sql).ToList();
//        }
    }
}
