using Mic.Entity;
using Mic.Entity.Model;
using Mic.Repository.Dapper;
using System;
using System.Collections.Generic;
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
            //            string activeSingerYesSql = $@"select Count(1)  as total from [User]  
            //where UserType=1  and LastLoginTime>'{yes}' and LastLoginTime<'{yes.AddDays(1).AddSeconds(-1)}';";
            string activeSingerYesSql = $@"SELECT Count(distinct UserId)  as total
  FROM [LoginLog] where UserType=1  and LoginTime>'{yes}' and LoginTime<'{yes.AddDays(1).AddSeconds(-1)}';";
            sqlList.Add(activeSingerYesSql);
            //上周的昨日活跃音乐人数量
            string activeSingerYesLastWeekSql = $@"SELECT Count(distinct UserId)  as total FROM [LoginLog] 
where UserType=1 and LoginTime>'{yesLastWeek}' and LoginTime<'{yesLastWeek.AddDays(1).AddSeconds(-1)}';";
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
u.Id as SingerId,c.SingerName,
 SUM(CASE  WHEN a.AuditStatus in ('0','1','2','3') THEN 1 ELSE 0 END) AS UploadCount,
SUM(CASE a.AuditStatus WHEN '2' THEN 1 ELSE 0 END) AS  PublishCount,
COUNT( b.PlayUserId) PlayStoreCount, COUNT(distinct b.SongId) PlaySongCount
  from [User] u left join   SongBook a on u.Id=a.SingerId left join SingerDetailInfo c on u.Id=c.UserId
  left join SongPlayRecord b on a.Id=b.SongId 
  where u.UserType=1 and u.LastLoginTime >'{param.BeginDate}' and u.LastLoginTime <'{param.EndDate}'  
 group by u.Id,c.SingerName ) temp_row
                    where temp_row.rownumber>(({param.PageIndex}-1)*{param.PageSize}) ;";
            return helper.Query<SingerListStatisticsEntity>(sql).ToList();
        }

        public Tuple<int, List<SongBookEntity>> GetUploadSongListBySingerId(SingerSongPageParam param)
        {
            string sql = string.Format(@"
                select top {0} * from (select row_number() over(order by UploadTime desc) as rownumber, 
* from SongBook where SingerId={2} and Status=1 and (AuditStatus=0 or AuditStatus=1 or AuditStatus=3)
) temp_row
                    where temp_row.rownumber>(({1}-1)*{0});", param.PageSize, param.PageIndex, param.SingerId);
            int count = Convert.ToInt32(helper.QueryScalar($@"select Count(1) from SongBook where SingerId={param.SingerId} and Status=1 and (AuditStatus=0 or AuditStatus=1 or AuditStatus=3)"));
            return Tuple.Create(count, helper.Query<SongBookEntity>(sql).ToList());
        }

        public Tuple<int, List<SongBookEntity>> GetPublishSongListBySingerId(SingerSongPageParam param)
        {
            string sql = string.Format(@"
                select top {0} * from (select row_number() over(order by {2} d.UploadTime desc) as rownumber, 
* from SongBook d left join (select COUNT(a.Id) as PlayTimes , Sum(b.BroadcastTime) as TotalPlayTime ,a.Id as tempId
from SongPlayRecord b left join  SongBook a  on a.Id = b.SongId    where a.Status=1 and a.AuditStatus=2  and a.SingerId={3}
group by a.Id) c on c.tempId = d.Id where d.SingerId={3} and d.Status=1 and d.AuditStatus=2) temp_row
                    where temp_row.rownumber>(({1}-1)*{0});", param.PageSize, param.PageIndex, 
                     string.IsNullOrWhiteSpace(param.OrderField) ? string.Empty : ("c." + param.OrderField + " " + param.OrderType + ",")
                     ,param.SingerId);
            int count = Convert.ToInt32(helper.QueryScalar($@"select Count(1) from SongBook where Status=1 and AuditStatus=2 and SingerId={param.SingerId};"));
            return Tuple.Create(count, helper.Query<SongBookEntity>(sql).ToList());
        }

        public Tuple<int, List<SongPlayRecordEntity>> GetSongRecordBySingerId(SingerSongPageParam param)
        {
            string sql = string.Format(@"
                select top {0} * from (select row_number() over(order by  a.BeginPlayTime desc) as rownumber, 
a.StoreCode,BeginPlayTime,b.SongName,b.SongLength,d.StoreName,e.StoreTypeName from SongPlayRecord a left join  SongBook b 
					on a.SongId = b.Id 
					left join [User] c on c.StoreCode=a.StoreCode and c.IsMain=1 left join StoreDetailInfo d
					on d.UserId=c.Id left join StoreType e on e.Id= d.StoreTypeId
where b.SingerId = {2} 
) temp_row
                    where temp_row.rownumber>(({1}-1)*{0});", param.PageSize, param.PageIndex, param.SingerId);
            int count = Convert.ToInt32(helper.QueryScalar($@"select Count(1) from SongBook where Status=1 and AuditStatus=2 and SingerId={param.SingerId};"));
            return Tuple.Create(count, helper.Query<SongPlayRecordEntity>(sql).ToList());

        }
    }
}
