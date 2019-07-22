using Dapper;
using Mic.Entity;
using Mic.Repository.Dapper;
using Mic.Repository.Utils;
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
                string password = string.Empty;
                var temp = helper.QueryScalar($@"select [Password] from [User] where Id={storeInfo.Id}");
                if (temp.ToString() == storeInfo.Password)
                {
                    password = temp.ToString();
                }
                else
                {
                    password = Util.MD5Encrypt(storeInfo.Password);
                }
                result = helper.Execute($@"update [User] set UserName='{storeInfo.UserName}',
 Phone='{storeInfo.Phone}',Password='{password}' where Id={storeInfo.StoreId};
update StoreDetailInfo set StoreName='{storeInfo.StoreName}',StoreTypeId={storeInfo.StoreTypeId},MaximumStore={storeInfo.MaximumStore},
DelegatingContract='{storeInfo.DelegatingContract}',Province='{storeInfo.Province}',City='{storeInfo.City}',
County='{storeInfo.County}',DetailAddress='{storeInfo.DetailAddress}' where UserId={storeInfo.StoreId};");
                id = storeInfo.StoreId;
            }
            else
            {
                var p = new DynamicParameters();
                p.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                string sql = $@"insert into [User] (UserName,Password,Phone,UserType,IsMain,
StoreCode,StoreManage,SongManage,UserManage,Status) values ('{storeInfo.UserName}','{Util.MD5Encrypt(storeInfo.Password)}',
'{storeInfo.Phone}',{2},{1},'{Guid.NewGuid().ToString()}',{1},{1},{1},{1});SELECT @Id=SCOPE_IDENTITY()";
                result = helper.Execute(sql, p);

                id = p.Get<int>("@Id");
                var detailSql = $@"insert into StoreDetailInfo (UserId,StoreName,StoretypeId,Province,City,County,
DetailAddress,MaximumStore,DelegatingContract,CreateTime,Enabled) values ({id},'{storeInfo.StoreName}',
{storeInfo.StoreTypeId},'{storeInfo.Province}','{storeInfo.City}','{storeInfo.County}','{storeInfo.DetailAddress}',
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

        /// <summary>
        /// 根据商家Id 获取商家详细信息
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public Tuple<bool, StoreDetailInfoEntity> GetStoreDetailById(int storeId)
        {
            string sql = $@"select * from [User] a  left join StoreDetailInfo b on a.Id= b.UserId 
left join StoreType c on c.Id=b.StoreTypeId  where a.Id = {storeId}";
            var result = helper.Query<StoreDetailInfoEntity>(sql).FirstOrDefault();
            return Tuple.Create(result == null ? false : true, result);
        }

        /// <summary>
        /// 根据商家Id 获取 商家分店列表
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public List<StoreDetailInfoEntity> GetSonStoreListById(int storeId)
        {
            string storeCode = helper.QueryScalar($@"select StoreCode from [User] where Id={storeId}").ToString();

            var result = helper.Query<StoreDetailInfoEntity>($@"select * from [User] a  left join StoreDetailInfo b on a.Id= b.UserId 
left join StoreType c on c.Id=b.StoreTypeId  where a.StoreCode = '{storeCode}' and UserType=3;").ToList();
            return result;
        }

    }
}
