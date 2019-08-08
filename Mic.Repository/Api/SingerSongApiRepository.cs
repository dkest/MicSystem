using Dapper;
using Mic.Entity;
using Mic.Repository.Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mic.Repository
{
    public class SingerSongApiRepository
    {
        DapperHelper<SqlConnection> helper;
        public SingerSongApiRepository()
        {
            helper = new DapperHelper<SqlConnection>(WebConfig.SqlConnection);
        }

        /// <summary>
        /// 上传歌曲，添加歌曲信息
        /// </summary>
        /// <param name="song"></param>
        /// <returns></returns>
        public int AddSongInfo(UploadSongParam song)
        {
            var p = new DynamicParameters();
            p.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var result = helper.Execute($@"insert into SongBook (SongName,SingerName,SingerId,
SongLength,CopyrightFilePath,SongPath,SongSize,UploadTime,AuditStatus,Status)
values ('{song.SongName}','{song.SingerName}',{song.SingerId},'{song.SongLength}',
'{song.CopyrightFilePath}','{song.SongPath}',{song.SongSize},'{DateTime.Now}',{0},{1}); SELECT @Id=SCOPE_IDENTITY()", p);
            helper.Execute($@"insert into SongOptDetail (SongId,AuditTimes,AuditStatus,Note,OptType,OptTime) values
({song.Id},{0},{0},'音乐人上传歌曲',{1},'{DateTime.Now}')");
            return p.Get<int>("@Id");
        }


        /// <summary>
        /// 更新歌曲信息
        /// </summary>
        /// <param name="song"></param>
        /// <returns></returns>
        public bool UpdateSongInfo(UploadSongParam song)
        {
            string sql = $@"update SongBook set SongName='{song.SongName}',SongLength='{song.SongLength}',
CopyrightFilePath='{song.CopyrightFilePath}',SongPath='{song.SongPath}',SongSize={song.SongSize} where Id={song.Id};
insert into SongOptDetail (SongId,AuditTimes,AuditStatus,Note,OptType,OptTime) values
({song.Id},{0},{0},'音乐人修改歌曲信息',{3},'{DateTime.Now}')";
            var result = helper.Execute(sql);
            return result > 0 ? true : false;
        }
        /// <summary>
        /// 发布歌曲
        /// </summary>
        /// <param name="songId"></param>
        /// <returns></returns>
        public bool PublishSong(int songId)
        {
            string sql = $@"update SongBook set AuditStatus ={1},PublishTimes=(PublishTimes+1) where Id={songId};
insert into SongOptDetail (SongId,AuditTimes,AuditStatus,Note,OptType,OptTime) values
({songId},{0},{0},'音乐人发布歌曲',{2},'{DateTime.Now}')";
            var result = helper.Execute(sql);
            return result > 0 ? true : false;
        }

        /// <summary>
        /// 删除歌曲
        /// </summary>
        /// <param name="songId"></param>
        /// <returns></returns>
        public bool DeleteSong(int songId)
        {
            string sql = $@"update SongBook set Status ={0} where Id={songId}";
            var result = helper.Execute(sql);
            return result > 0 ? true : false;
        }

        /// <summary>
        /// 显示歌曲上传审核详情
        /// </summary>
        /// <param name="songId"></param>
        /// <returns></returns>
        public List<SongOptDetail> SongAuditDeatil(int songId)
        {
            string sql = $@"select a.*,b.UserName as AuditUser from SongOptDetail a left join [Admin] b on a.AuditAdminId = b.Id
where SongId={songId} 
order by OptTime asc";
            var result = helper.Query<SongOptDetail>(sql).ToList();
            return result;
        }

        /// <summary>
        /// 根据音乐人Id，分页获取未审核的歌曲列表
        /// </summary>
        /// <param name="singerId"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public PagedResult<SongInfoParam> GetUnPublishSongList(int singerId, PageParam param)
        {
            string sql = $@"select top {param.PageSize} * from (select row_number() over(order by UploadTime desc) as rownumber, 
Id,SongName,SongLength,UploadTime,AuditStatus from SongBook where SingerId={singerId} and AuditStatus in (0,1,3)
and Status =1 ) temp_row  where temp_row.rownumber>(({param.PageIndex}-1)*{param.PageSize})";
            var result = helper.Query<SongInfoParam>(sql).ToList();
            int count = Convert.ToInt32(helper.QueryScalar($@"select Count(1) from SongBook 
where SingerId={singerId} and AuditStatus in (0,1,3)  and Status =1"));
            return new PagedResult<SongInfoParam>
            {
                Page = param.PageIndex,
                PageSize = param.PageSize,
                Total = count,
                Results = result
            };
        }

        /// <summary>
        /// 获取歌曲详情
        /// </summary>
        /// <param name="singerId"></param>
        /// <param name="songId"></param>
        /// <returns></returns>
        public SongBookEntity GetSongDetailInfo(int singerId, int songId)
        {
            string sql = $@"select * from SongBook where Id={songId} and SingerId={singerId}";
            return helper.Query<SongBookEntity>(sql).FirstOrDefault();
        }

        /// <summary>
        /// 根据音乐人Id，分页获取审核通过的歌曲列表
        /// </summary>
        /// <param name="singerId"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public PagedResult<SongInfoParam> GetPublishSongList(int singerId, PageParam param)
        {
            string sql = $@"select top {param.PageSize} * from (select row_number() over(order by UploadTime desc) as rownumber, 

 Id,SongName,SongLength,UploadTime,AuditStatus,
case when b.PlayTimes is null then 0
else b.PlayTimes end as PlayTimes,
case when b.TotalPlayTime is null then 0 else b.TotalPlayTime end TotalPlayTime
 from SongBook a left join
(select SongId,COUNT(1) as PlayTimes,SUM(BroadcastTime) as TotalPlayTime from SongPlayRecord group by SongId)  b on a.Id=b.SongId
where SingerId={singerId} and AuditStatus=2 and a.Status =1
) temp_row  where temp_row.rownumber>(({param.PageIndex}-1)*{param.PageSize})";
            var result = helper.Query<SongInfoParam>(sql).ToList();
            int count = Convert.ToInt32(helper.QueryScalar($@"select Count(1) from SongBook 
where SingerId={singerId} and AuditStatus=2 and Status =1"));
            return new PagedResult<SongInfoParam>
            {
                Page = param.PageIndex,
                PageSize = param.PageSize,
                Total = count,
                Results = result
            };
        }


        public SingerStatisticsParam GetSingerSongStatistics(int singerId)
        {
            var songCount = helper.QueryScalar($@"select Count(1) from SongBook where SingerId={singerId} AuditStatus=2");
            var playTimes = helper.QueryScalar($@"select Count(1) from SongPlayRecord a left join SongBook b on a.SongId=b.Id where b.SingerId={singerId} ");
            return new SingerStatisticsParam
            {
                PublishCount = Convert.ToInt32(songCount),
                PlayTimes = Convert.ToInt32(playTimes)
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="songId"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public PagedResult<SongInfoParam> SongStattisticsByStore(int songId, PageParam param)
        {
            string sql = $@"select top {param.PageSize} * from (select row_number() over(order by UpdateTime desc) as rownumber, 

 b.Id,c.StoreName,d.StoreTypeName, a.* from (
select StoreCode,COUNT(1) as PlayTimes ,SUM(BroadcastTime) as TotalPlayTime,
Count(distinct PlayUserId) as StoreCount  from SongPlayRecord where SongId={songId} group by StoreCode
) a
left join [User] b on a.StoreCode=b.StoreCode  left join StoreDetailInfo c on c.UserId=b.Id
left join StoreType d on d.Id=c.StoreTypeId
where b.UserType=2 and b.IsMain=1 

) temp_row  where temp_row.rownumber>(({param.PageIndex}-1)*{param.PageSize})";
            var result = helper.Query<SongInfoParam>(sql).ToList();
            int count = Convert.ToInt32(helper.QueryScalar($@"select Count(distinct StoreCode)  from SongPlayRecord where SongId={songId} group by StoreCode"));

            return new PagedResult<SongInfoParam>
            {
                Page = param.PageIndex,
                PageSize = param.PageSize,
                Total = count,
                Results = result
            };
        }

        public PagedResult<SongInfoParam> SongStattisticsDetailByStore(int songId, PageParam param)
        {
            string sql = $@"select top {param.PageSize} * from (select row_number() over(order by r.BeginPlayTime desc) as rownumber, 
* from (select ROW_NUMBER() OVER( PARTITION BY a.StoreCode ORDER BY a.StoreCode ) AS num, e.BeginPlayTime,c.StoreName,d.StoreTypeName from (select StoreCode from SongPlayRecord where SongId={songId}  group by StoreCode ) a
left join [User] b on a.StoreCode =b.StoreCode left join StoreDetailInfo c on c.UserId=b.Id
left join StoreType d on d.Id=c.StoreTypeId left join SongPlayRecord e on  a.StoreCode = e.StoreCode
where b.UserType=2 and IsMain=1) r where num<2 

) temp_row  where temp_row.rownumber>(({param.PageIndex}-1)*{param.PageSize})";
            var result = helper.Query<SongInfoParam>(sql).ToList();
            int count = Convert.ToInt32(helper.QueryScalar($@"select Count(1)  from SongPlayRecord where SongId={songId} group by StoreCode"));

            return new PagedResult<SongInfoParam>
            {
                Page = param.PageIndex,
                PageSize = param.PageSize,
                Total = count,
                Results = result
            };
        }


    }
}
