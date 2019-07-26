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
            helper.Query<AccessToken>($@"Delete  from UserAccessToken where ExpireTime<'{DateTime.Now}'").FirstOrDefault();
        }

        public UserEntity GetUserInfoByToken(string token)
        {
            return helper.Query<UserEntity>($@"select a.* from [User] a left join [UserAccessToken] b on a.Id=b.UserId where b.TokenId='{token}'").FirstOrDefault();
        }

    }
}
