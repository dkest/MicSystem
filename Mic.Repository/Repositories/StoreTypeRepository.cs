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
    public class StoreTypeRepository
    {
        private DapperHelper<SqlConnection> helper;
        public StoreTypeRepository()
        {
            helper = new DapperHelper<SqlConnection>(WebConfig.SqlConnection);
        }
        /// <summary>
        /// 添加或更新商家类型
        /// </summary>
        /// <param name="storeTypeEntity"></param>
        /// <returns></returns>
        public Tuple<bool, StoreTypeEntity> AddOrUpdateStoreType(StoreTypeEntity storeTypeEntity)
        {
            StoreTypeEntity updateEntity = storeTypeEntity;
            int result = 0;
            if (storeTypeEntity.Id > 0)
            {
                result = helper.Execute($@"update StoreType set StoreTypeName='{storeTypeEntity.StoreTypeName}',UpdateTime='{DateTime.Now}' where Id={storeTypeEntity.Id}");
                updateEntity = storeTypeEntity;
            }
            else
            {
                var p = new DynamicParameters();
                p.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                result = helper.Execute($@"insert into StoreType (StoreTypeName,UpdateTime) values ('{storeTypeEntity.StoreTypeName}','{DateTime.Now}');SELECT @Id=SCOPE_IDENTITY()", p);
                var id = p.Get<int>("@Id");
                storeTypeEntity.Id = id;
            }
            return Tuple.Create(result > 0 ? true : false, updateEntity);
        }

        public Tuple<bool, string> DeleteStoreType(int id)
        {
            //删除之前判断该商家类型是否被使用
            List<int> usedIdList = new List<int>();
            bool hasUsed = false;
            var temp = helper.Query<StoreDetailInfoEntity>($@"select distinct StoreTypeId from StoreDetailInfo;");
            foreach (var item in temp)
            {
                if (item.StoreTypeId == id)
                {
                    hasUsed = true;
                    break;
                }
            }
            if (hasUsed)
            {
                return Tuple.Create(false, "该商家类型已经在使用，无法被删除。");
            }

            int result = helper.Execute($@"delete from StoreType where Id={id}");
            return Tuple.Create(result > 0 ? true : false, string.Empty);
        }

        public List<StoreTypeEntity> GetStoreTypeList()
        {
            return helper.Query<StoreTypeEntity>($@"select * from StoreType order by UpdateTime desc;").ToList();
        }
    }
}
