using Mic.Entity;
using Mic.Repository.Dapper;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace Mic.Repository.Repositories
{
    public class TokenRepository
    {
        private DapperHelper<SqlConnection> helper;
        public TokenRepository()
        {
            helper = new DapperHelper<SqlConnection>(WebConfig.SqlConnection);
        }
        public AccessToken UpdateAccessToken(AccessToken accessToken)
        {
            var result = helper.Query<AccessToken>($@"select * from UserAccessToken where UserId={accessToken.UserId}").FirstOrDefault();
            if (result != null)
            {
                accessToken.TokenId = result.TokenId;
                helper.Execute($@"update UserAccessToken set CreateTime='{accessToken.CreateTime}',
ExpireTime='{accessToken.ExpireTime}' where UserId={accessToken.UserId}");
            }
            else
            {
                helper.Execute($@"insert into UserAccessToken (TokenId,UserId,CreateTime,ExpireTime) 
values ('{accessToken.TokenId}',{accessToken.UserId},'{accessToken.CreateTime}','{accessToken.ExpireTime}')");
            }
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
