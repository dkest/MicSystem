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
    public class SingerRepository
    {
        DapperHelper<SqlConnection> helper;
        public SingerRepository()
        {
            helper = new DapperHelper<SqlConnection>(WebConfig.SqlConnection);
        }

        /// <summary>
        /// 分页查询音乐人信息
        /// </summary>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        public Tuple<int, List<SingerDetailInfoEntity>> GetSingerList(PageParam pageParam)
        {
            string likeSql = string.IsNullOrWhiteSpace(pageParam.Keyword) ? string.Empty : $@" and (a.Phone like '%{pageParam.Keyword}%'  or b.SingerName like '%{pageParam.Keyword}%')";
            string sql = string.Format(@"
                select top {0} * from (select row_number() over(order by  b.CreateTime desc) as rownumber,a.UserName,a.Phone,
a.UserType,a.Status,a.Password,a.Id as SingerId,b.*
                    from [User] a  left join SingerDetailInfo b on a.Id= b.UserId 
where a.Status=1 and a.UserType=1   {2} ) temp_row
                    where temp_row.rownumber>(({1}-1)*{0});", pageParam.PageSize, pageParam.PageIndex, likeSql);
            int count = Convert.ToInt32(helper.QueryScalar($@"select Count(1) from [User] a  left join SingerDetailInfo b on a.Id= b.UserId 
where a.Status=1 and a.UserType=1  {likeSql}"));
            return Tuple.Create(count, helper.Query<SingerDetailInfoEntity>(sql).ToList());
        }

        /// <summary>
        /// 设定音乐人禁用启用方法
        /// </summary>
        /// <param name="id">商家用户Id</param>
        /// <param name="status">设定状态</param>
        /// <returns></returns>
        public bool UpdateSingerStatus(int id, bool status)
        {
            string sql = $@"update [SingerDetailInfo] set  Enabled='{status}' where UserId={id};";
            return helper.Execute(sql) > 0 ? true : false;
        }

        /// <summary>
        /// 后台更新音乐人艺名
        /// </summary>
        /// <param name="singerInfo"></param>
        /// <returns></returns>
        public bool UpdateSingerName(int singerId, string name)
        {
            string sql = $@"update [SingerDetailInfo] set  SingerNameForStore='{name}' where UserId={singerId};";
            return helper.Execute(sql) > 0 ? true : false;
        }

        /// <summary>
        /// 更新音乐人状态
        /// </summary>
        /// <param name="singerId"></param>
        /// <param name="auth"></param>
        /// <returns></returns>
        public bool UpdateAuthStatus(int singerId, int auth)
        {
            string sql = $@"update [SingerDetailInfo] set  Authentication='{auth}' where UserId={singerId};";
            return helper.Execute(sql) > 0 ? true : false;
        }

    }
}
