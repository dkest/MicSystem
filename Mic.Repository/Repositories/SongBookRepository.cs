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
    //ZNJSToolUtil.MD5Encrypt(password)

    public class SongBookRepository
    {
        DapperHelper<SqlConnection> helper;
        public SongBookRepository()
        {
            helper = new DapperHelper<SqlConnection>(WebConfig.SqlConnection);
        }



        public Tuple<int, List<SongBookEntity>> GetApprovedSongList(PageParam pageParam)
        {
            string likeSql = string.IsNullOrWhiteSpace(pageParam.Keyword) ? string.Empty : $@" and (SingerName like '%{pageParam.Keyword}%'  or SongName like '%{pageParam.Keyword}%')";
            string sql = string.Format(@"
                select top {0} * from (select row_number() over(order by {3} Id desc) as rownumber,SongBook.*
                    from SongBook  where AuditStatus=2   {2}) temp_row
                    where temp_row.rownumber>(({1}-1)*{0});", pageParam.PageSize, pageParam.PageIndex, likeSql,
                    string.IsNullOrWhiteSpace(pageParam.OrderField) ? string.Empty : (pageParam.OrderField + " " + pageParam.OrderType + ","));
            int count = Convert.ToInt32(helper.QueryScalar($@"select Count(1) from SongBook where AuditStatus=2  {likeSql}"));
            return Tuple.Create(count, helper.Query<SongBookEntity>(sql).ToList());
        }
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
                    from SongBook  where {2}  {3}) temp_row
                    where temp_row.rownumber>(({1}-1)*{0});", pageParam.PageSize, pageParam.PageIndex, auditSql, likeSql);
            int count = Convert.ToInt32(helper.QueryScalar($@"select Count(1) from SongBook where {auditSql} {likeSql}"));
            return Tuple.Create(count, helper.Query<SongBookEntity>(sql).ToList());

        }




        public User GetById(int id)
        {
            return helper.Query<User>($@"select * from [User] where Id={id}").FirstOrDefault();
        }

        public PageEnumerable<User> GetPage(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public void Remove(User item)
        {
            helper.Execute($@"delete from [User] where Id = {item.Id}");
        }

        public void Save(User item)
        {
            //helper.Execute(
            //    "UPDATE Cat SET BreedId = @BreedId, Name = @Name, Age = @Age WHERE CatId = @CatId",
            //    //param: new { CatId = entity.CatId, BreedId = entity.BreedId, Name = entity.Name, Age = entity.Age },

            //);
        }

    }
}
