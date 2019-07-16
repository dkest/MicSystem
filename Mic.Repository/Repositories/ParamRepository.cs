using Mic.Entity;
using Mic.Repository.Dapper;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace Mic.Repository.Repositories
{

    public class ParamRepository
    {
        DapperHelper<SqlConnection> helper;
        public ParamRepository()
        {
            helper = new DapperHelper<SqlConnection>(WebConfig.SqlConnection);
        }

        public Tuple<bool, ParamEntity> GetParamByTypeId(int typeId)
        {
            var result = helper.Query<ParamEntity>($@"select * from Param where ParamType={typeId}").FirstOrDefault();
            var status = result == null ? false : true;
            return Tuple.Create(status, result);
        }

        public bool UpdateParam(ParamEntity paramEntity)
        {
            string sql = $@"update Param set ParamContent='{paramEntity.ParamContent}' where ParamType={paramEntity.ParamType};";
            return helper.Execute(sql) > 0 ? true : false;
        }
    }
}
