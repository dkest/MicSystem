using Dapper;
using Mic.Entity;
using Mic.Repository.Dapper;
using Mic.Repository.Utils;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

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
        public Tuple<bool, string> ValidateSmsCode(string phone, string smsCode)
        {
            string sql = $@"select Count(1) from SmsRecord where Phone='{phone}'
and Code='{smsCode}' and Status={1} and UpdateTime>'{DateTime.Now.AddMinutes(-10)}';";
            return Tuple.Create(Convert.ToInt32(helper.QueryScalar(sql)) > 0 ? true : false, string.Empty);
        }

        public void DeleteSmsCode(string phone, string code)
        {
            string sql = $@"update SmsRecord set Status=0 where Phone='{phone}' and Code='{code}'";
        }

        public bool Register(RegisterParam storeInfo)
        {
            bool status = false;
            if (storeInfo.UserType == 1)//音乐人
            {
                var p = new DynamicParameters();
                p.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                helper.Execute($@"insert into [User] (Phone,[Password],UserType,Status) values ('{storeInfo.Phone}','{Util.MD5Encrypt(storeInfo.Password)}',{1},{1});SELECT @Id=SCOPE_IDENTITY()", p);
                int userId = p.Get<int>("@Id");
                var result = helper.Execute($@"insert into SingerDetailInfo (UserId,CreateTime,Enabled,SingerName) values ({userId},'{DateTime.Now}',{1},'{storeInfo.Phone}')");
                status = result > 0 ? true : false;
            }
            else if (storeInfo.UserType == 2)//商家
            {
                Guid storeCode = Guid.NewGuid();
                var p = new DynamicParameters();
                p.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                helper.Execute($@"insert into [User] (Phone,[Password],UserType,Status,IsMain,StoreCode,StoreManage,SongManage,UserManage) values ('{storeInfo.Phone}','{Util.MD5Encrypt(storeInfo.Password)}',{2},{1},{1},'{storeCode}',{1},{1},{1});SELECT @Id=SCOPE_IDENTITY()", p);
                int userId = p.Get<int>("@Id");
                var result = helper.Execute($@"insert into StoreDetailInfo (UserId,CreateTime,Enabled,StoreName,Province,City,County,DetailAddress,StoreTypeId) 
values ({userId},'{DateTime.Now}',{1},'{storeInfo.StoreName}',{storeInfo.Province},{storeInfo.City},{storeInfo.County},'{storeInfo.DetailAddress}',{storeInfo.StoreTypeId})");
                status = result > 0 ? true : false;
            }

            return status;
        }

        public bool UpdateUserPassword(UserParam user)
        {
            string sql = $@"update [User] set Password='{Util.MD5Encrypt(user.Password)}' where Phone='{user.Phone}'";
            return helper.Execute(sql) > 0 ? true : false;
        }
        /// <summary>
        /// 验证登录
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Tuple<bool, string, UserEntity> VerifyLogin(UserParam user)
        {
            bool isSuccess = true;
            string retMsg = string.Empty;
            UserEntity userEntity = null;
            var result = helper.Query<UserEntity>($@"select * from [User] where Phone='{user.Phone}';").FirstOrDefault();
            if (result == null)
            {
                isSuccess = false;
                retMsg = "不存在该用户。";
            }
            else
            {
                userEntity = helper.Query<UserEntity>($@"select * from [User] where Phone='{user.Phone}' and 
password='{Util.MD5Encrypt(user.Password)}' and Status=1").FirstOrDefault();
                if (userEntity == null)
                {
                    //isSuccess = false;
                    //retMsg = "密码不正确";
                    return Tuple.Create(false, "密码不正确", new UserEntity());
                }
                else
                {
                    if (!userEntity.Enable)
                    {
                        return Tuple.Create(false, "该账号不可用", new UserEntity());
                    }

                    if (userEntity.UserType == 1)
                    {
                        var temp = helper.Query<SingerDetailInfoEntity>($@"select SingerName,HeadImg, Enabled from SingerDetailInfo where UserId={userEntity.Id}").FirstOrDefault();


                        if (!Convert.ToBoolean(temp.Enabled))
                        {
                            userEntity = null;
                            isSuccess = false;
                            retMsg = "当前账号已经被禁用，请联系管理员启用该账号。";
                        }
                        else
                        {//登陆成功
                            userEntity.UserName = temp.SingerName;
                            userEntity.HeadImg = temp.HeadImg;
                            helper.Execute($@"update [User] set LastLoginTime='{DateTime.Now}' where Phone='{userEntity.Phone}'");
                        }
                    }
                    else //商家或者分店
                    {
                        var temp = helper.Query<StoreDetailInfoEntity>($@"select StoreName, Enabled from StoreDetailInfo where UserId={userEntity.Id}").FirstOrDefault();
                        if (temp == null || !Convert.ToBoolean(temp.Enabled))
                        {
                            userEntity = null;
                            isSuccess = false;
                            retMsg = "当前账号已经被禁用，请联系管理员启用该账号。";
                        }
                        else
                        {
                            userEntity.UserName = temp.StoreName;
                            helper.Execute($@"update [User] set LastLoginTime='{DateTime.Now}' where Phone='{userEntity.Phone}'");
                        }
                    }
                }

            }
            return Tuple.Create(isSuccess, retMsg, userEntity);

        }
    }
}
