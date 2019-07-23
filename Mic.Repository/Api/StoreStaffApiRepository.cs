using Mic.Entity;
using Mic.Repository.Dapper;
using Mic.Repository.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Mic.Repository.Repositories
{
    public class StoreStaffApiRepository
    {
        DapperHelper<SqlConnection> helper;
        public StoreStaffApiRepository()
        {
            helper = new DapperHelper<SqlConnection>(WebConfig.SqlConnection);
        }

        /// <summary>
        /// 添加员工
        /// </summary>
        /// <param name="token"></param>
        /// <param name="staff"></param>
        /// <returns></returns>
        public Tuple<bool, string> AddStoreStaff(string token, StoreStaffParam staff)
        {
            var temp = helper.QueryScalar($@"select Count(1) from [User] where Phone='{staff.Phone}'");
            if (Convert.ToInt32(temp) >0 )
            {
                return Tuple.Create(false, "该手机号已经存在，无法添加");
            }
            // 先根据token，Id,再获取，获取 StoreCode，然后再添加Staff
            string storeCode = helper.QueryScalar($@"select StoreCode from UserAccessToken a left join [User] b on a.UserId=b.Id where TokenId='{token}'").ToString();
            if (string.IsNullOrWhiteSpace(storeCode))
            {
                return Tuple.Create(false, "该账号无法添加员工");
            }
            string sql = $@"insert into [User] (StoreCode,StaffName,Phone,Password,StoreManage,SongManage,UserManage,Enable,Status,UserType,IsMain)
values ('{storeCode}','{staff.StaffName}','{staff.Phone}','{Util.MD5Encrypt(staff.Password)}',
'{staff.StoreManage}','{staff.SongManage}','{staff.UserManage}',{1},{1},{2},{0})";

            return Tuple.Create(helper.Execute(sql) > 0 ? true : false, string.Empty);
        }

        /// <summary>
        /// 更新员工信息
        /// </summary>
        /// <param name="staff"></param>
        /// <returns></returns>
        public Tuple<bool, string> UpdateStoreStaff(StoreStaffParam staff)
        {
            var count = helper.QueryScalar($@"select Count(1) from [User] where Phone='{staff.Phone}' and Id not in ({staff.Id})");
            if (Convert.ToInt32(count) > 0)
            {
                return Tuple.Create(false, "该手机号已经存在");
            }

            var temp = helper.QueryScalar($@"select [Password] from [User] where Id={staff.Id}");
            if (temp == null)
            {
                return Tuple.Create(false, "异常参数");
            }
            string password = temp.ToString();
            if (password != staff.Password)
            {
                password = Util.MD5Encrypt(staff.Password);
            }
            string sql = $@"update [User] set StaffName='{staff.StaffName}',Phone='{staff.Phone}',Password='{password}',
StoreManage='{staff.StoreManage}',SongManage='{staff.SongManage}',UserManage='{staff.UserManage}' where Id={staff.Id}";
            return Tuple.Create(helper.Execute(sql) > 0 ? true : false, string.Empty);
        }

        /// <summary>
        /// 设置员工账号可用状态
        /// </summary>
        /// <param name="enable"></param>
        /// <returns></returns>
        public bool UpdateStaffStatus(int id, bool enable)
        {
            return helper.Execute($@"update [User] set Enable='{enable}' where Id={id}") > 0 ? true : false;
        }

        /// <summary>
        /// 获取员工列表
        /// </summary>
        /// <param name="token"></param>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        public Tuple<int, List<StoreStaffParam>> GetStaffList(string token, PageParam pageParam)
        {
            string storeCode = helper.QueryScalar($@"select StoreCode from UserAccessToken a left join [User] b on a.UserId=b.Id where TokenId='{token}'").ToString();
            if (string.IsNullOrWhiteSpace(storeCode))
            {
                return Tuple.Create(0, new List<StoreStaffParam>());
            }
            string sql = string.Format(@"
                select top {0} * from (select row_number() over(order by  Id desc) as rownumber,
 * from [User] where Status=1 and StoreCode= '{2}' and UserType=2 and IsMain=0) temp_row
                    where temp_row.rownumber>(({1}-1)*{0});", pageParam.PageSize, pageParam.PageIndex, storeCode);
            int count = Convert.ToInt32(helper.QueryScalar($@"select Count(1) from [User] where Status=1 and StoreCode= '{storeCode}' and UserType=2 and IsMain=0 "));
            return Tuple.Create(count, helper.Query<StoreStaffParam>(sql).ToList());
        }

    }
}
