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
    public class StoreRepository
    {
        private DapperHelper<SqlConnection> helper;
        public StoreRepository()
        {
            helper = new DapperHelper<SqlConnection>(WebConfig.SqlConnection);
        }

        /// <summary>
        /// 获取商家列表
        /// </summary>
        /// <param name="pageParam">分页查询参数</param>
        /// <returns></returns>
        public Tuple<int, List<StoreDetailInfoEntity>> GetStoreList(PageParam pageParam)
        {
            string likeSql = string.IsNullOrWhiteSpace(pageParam.Keyword) ? string.Empty : $@" and (a.Phone like '%{pageParam.Keyword}%'  or b.StoreName like '%{pageParam.Keyword}%')";
            string sql = string.Format(@"
                select top {0} * from (select row_number() over(order by {3}  b.CreateTime desc) as rownumber,a.UserName,a.Phone,
a.UserType,a.IsMain,a.StaffName,a.StoreCode,a.Status,a.Password,a.Id as StoreId,b.*,c.StoreTypeName
                    from [User] a  left join StoreDetailInfo b on a.Id= b.UserId left join StoreType c on c.Id=b.StoreTypeId
where a.Status=1 and a.UserType=2  and a.IsMain=1  {2}) temp_row
                    where temp_row.rownumber>(({1}-1)*{0});", pageParam.PageSize, pageParam.PageIndex, likeSql,
                    string.IsNullOrWhiteSpace(pageParam.OrderField) ? string.Empty : (pageParam.OrderField + " " + pageParam.OrderType + ","));
            int count = Convert.ToInt32(helper.QueryScalar($@"select Count(1) from [User] a  left join StoreDetailInfo b on a.Id= b.UserId left join StoreType c on c.Id=b.StoreTypeId
where a.Status=1 and a.UserType=2  and a.IsMain=1  {likeSql}"));
            return Tuple.Create(count, helper.Query<StoreDetailInfoEntity>(sql).ToList());
        }





        /// <summary>
        /// 添加或更新商家
        /// </summary>
        /// <param name="storeTypeEntity"></param>
        /// <returns></returns>
        public Tuple<bool, int> AddOrUpdateStoreInfo(StoreDetailInfoEntity storeInfo)
        {
            int result = 0;
            var id = -1;
            if (storeInfo.Id > 0)
            {
                result = helper.Execute($@"update [User] set UserName='{storeInfo.UserName}',
 Phone='{storeInfo.Phone}',Password='{storeInfo.Password}' where Id={storeInfo.StoreId};
update StoreDetailInfo set StoreName='{storeInfo.StoreName}',StoreTypeId={storeInfo.StoreTypeId},MaximumStore={storeInfo.MaximumStore},
DelegatingContract='{storeInfo.DelegatingContract}',Provice='{storeInfo.Provice}',City='{storeInfo.City}',
County='{storeInfo.County}',DetailAddress='{storeInfo.DetailAddress}' where UserId={storeInfo.StoreId};");
                id = storeInfo.StoreId;
            }
            else
            {
                var p = new DynamicParameters();
                p.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                string sql = $@"insert into [User] (UserName,Password,Phone,UserType,IsMain,
StoreCode,StoreManage,SongManage,UserManage,Status) values ('{storeInfo.UserName}','{storeInfo.Password}',
'{storeInfo.Phone}',{2},{1},'{Guid.NewGuid().ToString()}',{1},{1},{1},{1});SELECT @Id=SCOPE_IDENTITY()";
                result = helper.Execute(sql,p);

                id = p.Get<int>("@Id");
                var detailSql = $@"insert into StoreDetailInfo (UserId,StoreName,StoretypeId,Provice,City,County,
DetailAddress,MaximumStore,DelegatingContract,CreateTime,Enabled) values ({id},'{storeInfo.StoreName}',
{storeInfo.StoreTypeId},'{storeInfo.Provice}','{storeInfo.City}','{storeInfo.County}','{storeInfo.DetailAddress}',
{storeInfo.MaximumStore},'{storeInfo.DelegatingContract}','{DateTime.Now}',{1});";
                helper.Execute(detailSql);
                
            }
            return Tuple.Create(true, id);
        }

        /// <summary>
        /// 设定商家禁用启用方法
        /// </summary>
        /// <param name="id">商家用户Id</param>
        /// <param name="status">设定状态</param>
        /// <returns></returns>
        public bool UpdateStoreStatus(int id, bool status)
        {
            string sql = $@"update [StoreDetailInfo] set  Enabled='{status}' where UserId={id};";
            return helper.Execute(sql) > 0 ? true : false;
        }

    }
}
