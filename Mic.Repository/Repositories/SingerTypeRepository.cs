using Dapper;
using Mic.Entity;
using Mic.Repository.Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Mic.Repository.Repositories
{
    /// <summary>
    /// 音乐人流派
    /// </summary>
    public class SingerTypeRepository
    {
        private DapperHelper<SqlConnection> helper;
        public SingerTypeRepository()
        {
            helper = new DapperHelper<SqlConnection>(WebConfig.SqlConnection);
        }
        /// <summary>
        /// 添加或更新流派
        /// </summary>
        /// <param name="storeTypeEntity"></param>
        /// <returns></returns>
        public Tuple<bool, SingerTypeEntity> AddOrUpdateSingerType(SingerTypeEntity singerTypeEntity)
        {
            SingerTypeEntity updateEntity = singerTypeEntity;
            int result = 0;
            if (singerTypeEntity.Id > 0)
            {
                var t = helper.QueryScalar($@"select count(1) from SingerType where SingerTypeName='{singerTypeEntity.SingerTypeName}' and Id not in ({singerTypeEntity.Id}) ");
                if (Convert.ToInt32(t) > 0)
                {
                    return Tuple.Create(false, new SingerTypeEntity());
                }
                result = helper.Execute($@"update SingerType set SingerTypeName='{singerTypeEntity.SingerTypeName}',UpdateTime='{DateTime.Now}' where Id={singerTypeEntity.Id}");
                updateEntity = singerTypeEntity;
            }
            else
            {
                var t = helper.QueryScalar($@"select count(1) from SingerType where SingerTypeName='{singerTypeEntity.SingerTypeName}' ");
                if (Convert.ToInt32(t) > 0)
                {
                    return Tuple.Create(false, new SingerTypeEntity());
                }
                var p = new DynamicParameters();
                p.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                result = helper.Execute($@"insert into SingerType (SingerTypeName,UpdateTime) values ('{singerTypeEntity.SingerTypeName}','{DateTime.Now}');SELECT @Id=SCOPE_IDENTITY()", p);
                var id = p.Get<int>("@Id");
                singerTypeEntity.Id = id;
            }
            return Tuple.Create(result > 0 ? true : false, updateEntity);
        }

        public Tuple<bool, string> DeleteSingerType(int id)
        {
            //删除之前判断该流派类型是否被使用
            List<int> usedIdList = new List<int>();
            bool hasUsed = false;
            var temp = helper.Query<SingerDetailInfoEntity>($@"select distinct SingerTypeId from SingerDetailInfo;");
            foreach (var item in temp)
            {
                if (item.SingerTypeId == id)
                {
                    hasUsed = true;
                    break;
                }
            }
            if (hasUsed)
            {
                return Tuple.Create(false, "该流派类型已经在使用，无法被删除。");
            }

            int result = helper.Execute($@"delete from SingerType where Id={id}");
            return Tuple.Create(result > 0 ? true : false, string.Empty);
        }

        public List<SingerTypeEntity> GetSingerTypeList()
        {
            return helper.Query<SingerTypeEntity>($@"select * from SingerType order by UpdateTime desc;").ToList();
        }
    }
}
