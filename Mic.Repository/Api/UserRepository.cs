using Dapper;
using Mic.Entity;
using Mic.Repository.Dapper;
using Mic.Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Mic.Repository.Repositories
{
    //ZNJSToolUtil.MD5Encrypt(password)

    public class UserRepository
    {
        DapperHelper<SqlConnection> helper;
        public UserRepository()
        {
            helper = new DapperHelper<SqlConnection>(WebConfig.SqlConnection);
        }

        /// <summary>
        /// 判断手机号是否存在
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public bool PhoneIsExist(string phone)
        {
            var result = helper.QueryScalar($@"select Count(1) from [User] where Phone='{phone}'");
            return Convert.ToInt32(result) > 0 ? true : false;
        }
        /// <summary>
        /// 将验证码保存到数据库
        /// </summary>
        /// <param name="smsRecord"></param>
        /// <returns></returns>
        public bool SaveSmsCode(SmsRecord smsRecord)
        {
            var result = helper.Execute($@"insert into SmsRecord values 
({smsRecord.UserId},'{smsRecord.Phone}','{smsRecord.Code}','{DateTime.Now}',{1})");
            return result > 0 ? true : false;
        }
        /// <summary>
        /// 校验验证码
        /// </summary>
        /// <param name="registerParam"></param>
        /// <returns></returns>
        public Tuple<bool, string> ValidateSmsCode(RegisterParam registerParam)
        {
            string sql = $@"select Count(1) from SmsRecord where Phone='{registerParam.Phone}'
and Code='{registerParam.SmsCode}' and Status={1} and UpdateTime<='{DateTime.Now.AddMinutes(-2)}';";
            return Tuple.Create(Convert.ToInt32(helper.QueryScalar(sql)) > 0 ? true : false, string.Empty);
        }

        public bool Register(RegisterParam registerParam)
        {
            bool status = false;
            if (registerParam.UserType == 1)
            {
                var p = new DynamicParameters();
                p.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                helper.Execute($@"insert into [User] (Phone,[Password],UserType,Status) values ('{registerParam.Phone}','{registerParam.Password}',{1},{1});SELECT @Id=SCOPE_IDENTITY()", p);
                int userId = p.Get<int>("@Id");
                var result = helper.Execute($@"insert into SingerDetailInfo (UserId,CreateTime,Enabled,SingerName) values ({userId},'{DateTime.Now}',{1},'{registerParam.Phone}')");
                status = result > 0 ? true : false;
            }
            else if (registerParam.UserType == 2)
            {
                Guid storeCode = Guid.NewGuid();
                var p = new DynamicParameters();
                p.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                helper.Execute($@"insert into [User] (Phone,[Password],UserType,Status,IsMain,StoreCode) values ('{registerParam.Phone}','{registerParam.Password}',{2},{1},{1},'{storeCode}');SELECT @Id=SCOPE_IDENTITY()", p);
                int userId = p.Get<int>("@Id");
                var result = helper.Execute($@"insert into StoreDetailInfo (UserId,CreateTime,Enabled,StoreName) values ({userId},'{DateTime.Now}',{1},'{registerParam.Phone}')");
                status = result > 0 ? true : false;
            }

            return status;
        }
    }
}
