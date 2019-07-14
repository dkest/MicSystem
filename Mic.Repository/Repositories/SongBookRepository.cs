using Dapper;
using Mic.Entity;
using Mic.Repository.Dapper;
using Mic.Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Mic.Repository.Repositories
{
    public class SongBookRepository
    {
        DapperHelper<SqlConnection> helper;
        public SongBookRepository()
        {
            helper = new DapperHelper<SqlConnection>(WebConfig.SqlConnection);
        }


        /// <summary>
        /// 分页获取已经审核了的歌曲
        /// </summary>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        public Tuple<int, List<SongBookEntity>> GetApprovedSongList(PageParam pageParam)
        {
            string likeSql = string.IsNullOrWhiteSpace(pageParam.Keyword) ? string.Empty : $@" and (SingerName like '%{pageParam.Keyword}%'  or SongName like '%{pageParam.Keyword}%')";
            string sql = string.Format(@"
                select top {0} * from (select row_number() over(order by {3} Id desc) as rownumber,SongBook.*
                    from SongBook  where Status=1 and AuditStatus=2   {2}) temp_row
                    where temp_row.rownumber>(({1}-1)*{0});", pageParam.PageSize, pageParam.PageIndex, likeSql,
                    string.IsNullOrWhiteSpace(pageParam.OrderField) ? string.Empty : (pageParam.OrderField + " " + pageParam.OrderType + ","));
            int count = Convert.ToInt32(helper.QueryScalar($@"select Count(1) from SongBook where Status=1 and AuditStatus=2  {likeSql}"));
            return Tuple.Create(count, helper.Query<SongBookEntity>(sql).ToList());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        public Tuple<int, List<SongBookEntity>> GetAuditSongList(AuditSongPageParam pageParam)
        {
            string likeSql = string.IsNullOrWhiteSpace(pageParam.Keyword) ? string.Empty : $@" and (SingerName like '%{pageParam.Keyword}%'  or SongName like '%{pageParam.Keyword}%')";
            string auditSql = string.Empty;
            switch (pageParam.AuditStatus)
            {
                case -1:
                    auditSql = "(AuditStatus=1 or AuditStatus=2 or AuditStatus=3)";
                    break;
                default:
                    auditSql = $@"AuditStatus={pageParam.AuditStatus}";
                    break;
            }
            string sql = string.Format(@"
                select top {0} * from (select row_number() over(order by Id) as rownumber,SongBook.*
                    from SongBook  where Status=1 and {2}  {3}) temp_row
                    where temp_row.rownumber>(({1}-1)*{0})
order by   
    case AuditStatus   
    when 1 then 1     
    when 3 then 2     
    when 2 then 3     
    end  
asc     
;", pageParam.PageSize, pageParam.PageIndex, auditSql, likeSql);
            int count = Convert.ToInt32(helper.QueryScalar($@"select Count(1) from SongBook where Status=1 and  {auditSql} {likeSql}"));
            return Tuple.Create(count, helper.Query<SongBookEntity>(sql).ToList());

        }

        public bool DeleteSongById(int id)
        {
            //int result = helper.Execute($@"delete from SongBook where Id={id}");
            int result = helper.Execute($@"update  SongBook set Status=0 where Id={id}");
            return result > 0 ? true : false;
        }


        /// <summary>
        /// 主要用于后台上传和修改歌曲
        /// </summary>
        /// <param name="songBookEntity"></param>
        /// <returns></returns>
        public Tuple<bool, SongBookEntity> AddOrUpdateSong(SongBookEntity songBookEntity)
        {
            SongBookEntity updateEntity = songBookEntity;
            int result = 0;
            if (songBookEntity.Id > 0)
            {
                result = helper.Execute($@"update SongBook set SongName='{songBookEntity.SongName}',
                    SingerName='{songBookEntity.SingerName}',SongMark='{songBookEntity.SongMark}',
                    SongPath='{songBookEntity.SongPath}',SongBPM='{songBookEntity.SongBPM}',
                    ExpirationTime='{songBookEntity.ExpirationTime}' where Id={songBookEntity.Id}");

            }
            else
            {

                var p = new DynamicParameters();
                p.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                result = helper.Execute($@"insert into SongBook (SongName,SingerName,SongMark,SongPath,SongBPM,
ExpirationTime,AuditStatus,SongLength,UploadTime,Status) 
                    values ('{songBookEntity.SongName}','{songBookEntity.SingerName}','{songBookEntity.SongMark}',
'{songBookEntity.SongPath}','{songBookEntity.SongBPM}','{songBookEntity.ExpirationTime}',{2},
'{songBookEntity.SongLength}','{DateTime.Now}',{1});SELECT @Id=SCOPE_IDENTITY()", p);
                var id = p.Get<int>("@Id");
                songBookEntity.Id = id;
                helper.Execute($@"insert into SongOptDetail (SongId,Note,OptType,OptTime) values(
                {id},'上传歌曲',{1},'{DateTime.Now}')");


            }
            return Tuple.Create(result > 0 ? true : false, updateEntity);
        }

    }
}
