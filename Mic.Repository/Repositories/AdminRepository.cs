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
    //ZNJSToolUtil.MD5Encrypt(password)

    public class AdminRepository
    {
        DapperHelper<SqlConnection> helper;
        public AdminRepository()
        {
            helper = new DapperHelper<SqlConnection>(WebConfig.SqlConnection);
        }
        /// <summary>
        /// 后台管理员登录
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Tuple<bool, string, Admin> VerifyLogin(string username, string password)
        {
            bool isSuccess = true;
            string retMsg = string.Empty;
            Admin admin = null;
            var adminUserName = helper.Query<Admin>($@"select * from [Admin] where UserName='{username}';").FirstOrDefault();
            if (adminUserName == null)
            {
                isSuccess = false;
                retMsg = "不存在该用户,请重新输入。";
            }
            else
            {
                admin = helper.Query<Admin>($@"select * from [Admin] where UserName='{username}' and password='{password}'").FirstOrDefault();
                if (admin == null)
                {
                    isSuccess = false;
                    retMsg = "密码不正确。";
                }
            }
            return Tuple.Create(isSuccess, retMsg, admin);
        }

        public Tuple<int, List<Admin>> GetAdminList(PageParam pageParam)
        {
            string sql = string.Format(@"
                select top {0} * from (select row_number() over(order by   Id desc) as rownumber,
 * from Admin where Status=1) temp_row
                    where temp_row.rownumber>(({1}-1)*{0});", pageParam.PageSize, pageParam.PageIndex);
            int count = Convert.ToInt32(helper.QueryScalar($@"select Count(1) from [Admin] where Status=1 "));
            return Tuple.Create(count, helper.Query<Admin>(sql).ToList());
        }

        public bool UpdateAdminStatus(int id, bool status)
        {
            return helper.Execute($@"update [Admin] set Enabled='{status}' where Id={id}") > 0 ? true : false;
        }

        public Tuple<bool,string> AddOrUpdateAdmin(Admin admin)
        {
            string sql = string.Empty;
            if (admin.Id > 0)
            {
                sql = $@"update [Admin] set [UserName]='{admin.UserName}',
[Password]='{admin.Password}',UpdateTime='{DateTime.Now}' where Id={admin.Id}";
            }
            else
            {
                var a = helper.QueryScalar($@"select Count(1) from [Admin] where UserName='{admin.UserName}'");
                if (Convert.ToInt32(a) > 0)
                {
                    return Tuple.Create(false, "已经存在该用户名的用户了，无法重复添加");

                }
                sql = $@"insert into [Admin] ([UserName],[Password],[Enabled],CreateTime,Status)
values ('{admin.UserName}','{admin.Password}','{1}','{DateTime.Now}','{1}')";
            }
            return Tuple.Create(helper.Execute(sql) > 0 ? true : false,string.Empty);
        }


    }
}
