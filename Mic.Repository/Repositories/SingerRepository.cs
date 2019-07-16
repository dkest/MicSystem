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

        public Tuple<int, List<SingerDetailInfoEntity>> GetSingerList(PageParam pageParam)
        {
            string likeSql = string.IsNullOrWhiteSpace(pageParam.Keyword) ? string.Empty : $@" and (a.Phone like '%{pageParam.Keyword}%'  or b.SingerName like '%{pageParam.Keyword}%')";
            string sql = string.Format(@"
                select top {0} * from (select row_number() over(order by  b.CreateTime desc) as rownumber,a.UserName,a.Phone,
a.UserType,a.Status,a.Password,a.Id as SingerId,b.*
                    from [User] a  left join SingerDetailInfo b on a.Id= b.UserId 
where a.Status=1 and a.UserType=1   {2} ) temp_row
                    where temp_row.rownumber>(({1}-1)*{0});", pageParam.PageSize, pageParam.PageIndex, likeSql);
            int count = Convert.ToInt32(helper.QueryScalar($@"select Count(1) from [User] a  left join StoreDetailInfo b on a.Id= b.UserId 
where a.Status=1 and a.UserType=1  {likeSql}"));
            return Tuple.Create(count, helper.Query<SingerDetailInfoEntity>(sql).ToList());
        }

    }
}
