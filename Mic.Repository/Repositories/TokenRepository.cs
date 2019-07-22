using Mic.Entity;
using Mic.Repository.Dapper;
using Mic.Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mic.Repository.Repositories
{
    public class TokenRepository 
    {
        private DapperHelper<SqlConnection> helper;
        public TokenRepository()
        {
            helper = new DapperHelper<SqlConnection>(WebConfig.SqlConnection);
        }
        public AccessToken AddAccessToken(AccessToken accessToken)
        {
            
            helper.Execute($@"insert into UserAccessToken (TokenId,UserId,CreateTime,ExpireTime) 
values ('{accessToken.TokenId}',{accessToken.UserId},'{accessToken.CreateTime}','{accessToken.ExpireTime}')");
            return accessToken;
        }

        public AccessToken GetAccessToken(Guid tokenId)
        {
            return helper.Query<AccessToken>($@"select * from UserAccessToken where TokenId='{tokenId}'").FirstOrDefault();
        }

        public void RemoveExpired()
        {
            //WebConfig.DatabaseInstance.Delete<AccessToken>("WHERE ExpireTime < @0", DateTime.Now);
        }

        //public AccessToken GetAccessToken(Guid tokenId)
        //{
        //    //return WebConfig.DatabaseInstance.FirstOrDefault<AccessToken>("WHERE TokenId = @0 AND ExpireTime > @1", tokenId, DateTime.Now);
        //}
    }
}
