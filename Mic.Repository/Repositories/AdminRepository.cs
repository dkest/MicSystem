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
        public Tuple<bool,string,Admin> VerifyLogin(string username, string password)
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
            else {
                admin = helper.Query<Admin>($@"select * from [Admin] where UserName='{username}' and password='{password}'").FirstOrDefault();
                if (admin == null)
                {
                    isSuccess = false;
                    retMsg = "密码不正确。";
                }
            }
            return Tuple.Create(isSuccess, retMsg,admin);
        }
       

            public IEnumerable<User> GetAll()
        {
            return helper.Query<User>("select * from [User]");
        }

        public User GetByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public User GetById(int id)
        {
            return helper.Query<User>($@"select * from [User] where Id={id}").FirstOrDefault();
        }

        public PageEnumerable<User> GetPage(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public void Remove(User item)
        {
            helper.Execute($@"delete from [User] where Id = {item.Id}");
        }

        public void Save(User item)
        {
            //helper.Execute(
            //    "UPDATE Cat SET BreedId = @BreedId, Name = @Name, Age = @Age WHERE CatId = @CatId",
            //    //param: new { CatId = entity.CatId, BreedId = entity.BreedId, Name = entity.Name, Age = entity.Age },
               
            //);
        }

    }
}
