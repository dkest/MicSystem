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
    public class StoreApiRepository
    {
        DapperHelper<SqlConnection> helper;
        public StoreApiRepository()
        {
            helper = new DapperHelper<SqlConnection>(WebConfig.SqlConnection);
        }

        /// <summary>
        /// 根据商家编码 获取商家详细信息
        /// </summary>
        /// <param name="storeCode"></param>
        /// <returns></returns>
        public Tuple<bool, StoreInfoParam> GetStoreDetailByStoreCode(string storeCode)
        {
            //根据storeCode 得到商家主账号的用户Id（StoreId）
            var storeId = helper.QueryScalar($@"select Id from [User] where StoreCode='{storeCode}' and UserType=2 and IsMain=1");
            if (storeId == null)
            {
                return Tuple.Create(false, new StoreInfoParam());
            }
            string sql = $@"select a.Id,a.Phone,a.Password, b.StoreName, b.BusinessLicense,b.LegalPerson,b.LegalPersonIdCardF,
b.LegalPersonIdCardB,b.Province,b.City,b.County,b.DetailAddress,b.Contacts,b.ContactsPhone,b.StoreTypeId,
c.StoreTypeName from [User] a  left join StoreDetailInfo b on a.Id= b.UserId 
left join StoreType c on c.Id=b.StoreTypeId  where a.Id = {Convert.ToInt32(storeId)}";
            var result = helper.Query<StoreInfoParam>(sql).FirstOrDefault();
            return Tuple.Create(result == null ? false : true, result);
        }

        /// <summary>
        /// 更新商家信息
        /// </summary>
        /// <param name="storeTypeEntity"></param>
        /// <returns></returns>
        public Tuple<bool, string> UpdateStoreInfo(StoreInfoParam storeInfo)
        {
            var count = helper.QueryScalar($@"select Count(1) from [User] where Phone='{storeInfo.Phone}' and Id not in ({storeInfo.Id})");
            if (Convert.ToInt32(count) > 0)
            {
                return Tuple.Create(false, "该手机号已经存在");
            }

            int result = 0;
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
                result = helper.Execute($@"update [User] set UserName='{storeInfo.StoreName}',
 Phone='{storeInfo.Phone}',Password='{password}' where Id={storeInfo.Id};
update StoreDetailInfo set StoreName='{storeInfo.StoreName}',BusinessLicense='{storeInfo.BusinessLicense}',
LegalPerson='{storeInfo.LegalPerson}',LegalPersonIdCardF='{storeInfo.LegalPersonIdCardF}',
LegalPersonIdCardB='{storeInfo.LegalPersonIdCardB}',Province='{storeInfo.Province}',City='{storeInfo.City}',
County='{storeInfo.County}',DetailAddress='{storeInfo.DetailAddress}',Contacts='{storeInfo.Contacts}',
ContactsPhone='{storeInfo.ContactsPhone}' where UserId={storeInfo.Id};");

            }

            return Tuple.Create(result > 0 ? true : false, string.Empty);
        }

        #region 分店管理模块
        /// <summary>
        /// 根据商家Id 获取 商家分店列表
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public Tuple<bool, string, int, List<SonStoreInfoParam>> GetSonStoreList(string token, PageParam pageParam)
        {
            UserEntity user = helper.Query<UserEntity>($@"select a.* from [User] a left join [UserAccessToken] b on a.Id=b.UserId where b.TokenId='{token}'").FirstOrDefault();
            if (user.UserType != 2)
            {
                return Tuple.Create(false, "当前账号不是商家，没有分店", 0, new List<SonStoreInfoParam>());
            }
            if (!user.StoreManage)
            {
                return Tuple.Create(false, "当前账号没有管理分店的权限", 0, new List<SonStoreInfoParam>());
            }
            if (string.IsNullOrWhiteSpace(user.StoreCode))
            {
                return Tuple.Create(false, string.Empty, 0, new List<SonStoreInfoParam>());
            }

            string likeSql = string.IsNullOrWhiteSpace(pageParam.Keyword) ? string.Empty : $@" and  b.StoreName like '%{pageParam.Keyword}%')";

            string sql = string.Format(@"
                select top {0} * from (select row_number() over(order by  b.CreateTime desc) as rownumber,
 a.Id,b.StoreName,a.Phone,a.Password,b.Enabled from [User] a left join StoreDetailInfo b on a.Id= b.UserId 
where a.StoreCode='{2}' and a.UserType=3 {3}) temp_row
                    where temp_row.rownumber>(({1}-1)*{0});", pageParam.PageSize, pageParam.PageIndex, user.StoreCode, likeSql);
            int count = Convert.ToInt32(helper.QueryScalar($@"select Count(1) from [User] a left join StoreDetailInfo b on a.Id= b.UserId  where a.Status=1 and a.UserType=3 and a.StoreCode= '{user.StoreCode}'  {likeSql} "));



            var result = helper.Query<SonStoreInfoParam>(sql).ToList();
            return Tuple.Create(true, string.Empty, count, result);
        }
        /// <summary>
        /// 添加分店
        /// </summary>
        /// <param name="token"></param>
        /// <param name="sonStore"></param>
        /// <returns></returns>
        public Tuple<bool, string> AddSonStore(string token, SonStoreInfoParam sonStore)
        {
            var temp = helper.QueryScalar($@"select Count(1) from [User] where Phone='{sonStore.Phone}'");
            if (Convert.ToInt32(temp) > 0)
            {
                return Tuple.Create(false, "该手机号已经存在，无法添加");
            }

            // 先根据token，Id,再获取，获取 StoreCode，然后再添加Staff
            UserEntity user = helper.Query<UserEntity>($@"select a.* from [User] a left join [UserAccessToken] b on a.Id=b.UserId where b.TokenId='{token}'").FirstOrDefault();
            if (user.UserType != 2)
            {
                return Tuple.Create(false, "当前账号不是商家，无法创建分店");
            }
            if (!user.StoreManage)
            {
                return Tuple.Create(false, "当前账号没有管理分店的权限");
            }
            if (string.IsNullOrWhiteSpace(user.StoreCode))
            {
                return Tuple.Create(false, "该账号无法添加员工");
            }

            var statistics = GetSonStoreStatistic(token);
            if (statistics.Item3 <= statistics.Item4)
            {
                return Tuple.Create(false, "该商家最多允许拥有" + statistics.Item3 + "个分店,已经拥有" + statistics.Item4 + "个，无法继续添加");
            }
            var storeId = helper.QueryScalar($@"select Id from [User] where StoreCode='{user.StoreCode}' and UserType=2 and IsMain=1;");

            var p = new DynamicParameters();
            p.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var result = helper.Execute($@"insert into [User] (StoreCode,StaffName,Phone,Password,Enable,Status,UserType)
values ('{user.StoreCode}','{sonStore.StoreName}','{sonStore.Phone}','{Util.MD5Encrypt(sonStore.Password)}',
'{true}','{true}',{3}); SELECT @Id=SCOPE_IDENTITY()", p);
            var id = p.Get<int>("@Id");
            string sql = $@"insert into StoreDetailInfo (UserId,StoreName,Enabled,CreateTime) 
values ({id},'{sonStore.StoreName}',{1},'{DateTime.Now}')";
            if (storeId != null)
            {
                helper.Execute($@"update StoreDetailInfo set ValidSonStoreNum = ValidSonStoreNum+1 where UserId={storeId}");
            }
            return Tuple.Create(helper.Execute(sql) > 0 ? true : false, string.Empty);
        }

        /// <summary>
        /// 更新分店信息
        /// </summary>
        /// <param name="sonStore"></param>
        /// <returns></returns>
        public Tuple<bool, string> UpdateSonStore(SonStoreInfoParam sonStore)
        {
            var count = helper.QueryScalar($@"select Count(1) from [User] where Phone='{sonStore.Phone}' and Id not in ({sonStore.Id})");
            if (Convert.ToInt32(count) > 0)
            {
                return Tuple.Create(false, "该手机号已经存在");
            }

            var temp = helper.QueryScalar($@"select [Password] from [User] where Id={sonStore.Id}");
            if (temp == null)
            {
                return Tuple.Create(false, "异常参数");
            }
            string password = temp.ToString();
            if (password != sonStore.Password)
            {
                password = Util.MD5Encrypt(sonStore.Password);
            }
            string sql = $@"update [User] set StaffName='{sonStore.StoreName}',Phone='{sonStore.Phone}',Password='{password}'
where Id={sonStore.Id};update StoreDetailInfo set StoreName='{sonStore.StoreName}' where UserId={sonStore.Id};";
            return Tuple.Create(helper.Execute(sql) > 0 ? true : false, string.Empty);
        }

        /// <summary>
        /// 设置分店账号可用状态
        /// </summary>
        /// <param name="enable"></param>
        /// <returns></returns>
        public bool UpdateSonStoreStatus(int id, bool enable)
        {
            return helper.Execute($@"update [User] set Enable='{enable}' where Id={id};
update StoreDetailInfo set Enabled='{enable}' where UserId={id}") > 0 ? true : false;
        }

        /// <summary>
        /// 获取商家最大分店数，和已开通分店数量
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public Tuple<bool, string, int, int> GetSonStoreStatistic(string token)
        {
            UserEntity user = helper.Query<UserEntity>($@"select a.* from [User] a left join [UserAccessToken] b on a.Id=b.UserId where b.TokenId='{token}'").FirstOrDefault();
            if (user.UserType != 2)
            {
                return Tuple.Create(false, "当前账号没有分店", 0, 0);
            }
            if (!user.StoreManage)
            {
                return Tuple.Create(false, "当前账号没有管理分店的权限", 0, 0);
            }
            if (string.IsNullOrWhiteSpace(user.StoreCode))
            {
                return Tuple.Create(false, "", 0, 0);
            }
            var max = helper.QueryScalar($@"select MaximumStore from StoreDetailInfo a left join [User] b on a.UserId=b.Id
where b.StoreCode='{user.StoreCode}' and b.UserType=2 and b.IsMain={1}");
            var temp = helper.QueryScalar($@"select Count(1) from [User]  
where StoreCode='{user.StoreCode}' and UserType=3 and Enable={1}");
            return Tuple.Create(true, "", Convert.ToInt32(max), Convert.ToInt32(temp));
        }

        #endregion

    }
}
