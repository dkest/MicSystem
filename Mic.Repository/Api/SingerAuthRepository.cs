using Mic.Entity;
using Mic.Repository.Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Mic.Repository
{
    public class SingerAuthRepository
    {
        DapperHelper<SqlConnection> helper;
        public SingerAuthRepository()
        {
            helper = new DapperHelper<SqlConnection>(WebConfig.SqlConnection);
        }

        public string GetAuthNote()
        {
            var result = helper.QueryScalar("select ParamContent from Param where ParamType=1");
            if (result != null)
            {
                return result.ToString();
            }
            return string.Empty;
        }

        public bool AddAuthInfo(SingerAuthParam singer)
        {
            string sql = $@"update SingerDetailInfo set SingerName='{singer.SingerName}',
SingerTypeId={singer.SingerTypeId},HeadImg='{singer.HeadImg}',IdCardNo='{singer.IdCardNo}',
IdCardFront='{singer.IdCardFront}',IdCardBack='{singer.IdCardBack}',Introduce='{singer.Introduce}',
Authentication={1} where UserId={singer.SingerId}";
            return helper.Execute(sql) > 0 ? true : false;
        }

        public SingerAuthParam GetSingerAuthInfo(int singerId)
        {
            string sql = $@"select * from SingerDetailInfo where Singer={singerId}";
            return helper.Query<SingerAuthParam>(sql).FirstOrDefault();
        }


        public List<SingerTypeEntity> GetAllSingerTypeList()
        {
            string sql = $@"select * from SingerType";
            return helper.Query<SingerTypeEntity>(sql).ToList();
        }
    }
}
