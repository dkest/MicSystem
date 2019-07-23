using Mic.Entity;
using Mic.Repository.Dapper;
using Mic.Repository.Utils;
using System;
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
            string sql = $@"select * from [User] a  left join StoreDetailInfo b on a.Id= b.UserId 
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

        //        /// <summary>
        //        /// 根据商家Id 获取 商家分店列表
        //        /// </summary>
        //        /// <param name="storeId"></param>
        //        /// <returns></returns>
        //        public List<StoreDetailInfoEntity> GetSonStoreListById(int storeId)
        //        {
        //            string storeCode = helper.QueryScalar($@"select StoreCode from [User] where Id={storeId}").ToString();

        //            var result = helper.Query<StoreDetailInfoEntity>($@"select * from [User] a  left join StoreDetailInfo b on a.Id= b.UserId 
        //left join StoreType c on c.Id=b.StoreTypeId  where a.StoreCode = '{storeCode}' and UserType=3;").ToList();
        //            return result;
        //        }
    }
}
