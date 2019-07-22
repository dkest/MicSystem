using Mic.Entity;
using Mic.Repository;
using Mic.Repository.Repositories;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Caching;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Mic.Api.Filter
{
    public class AccessTokenAuthorizeAttribute : AuthorizeAttribute
    {
        private TokenRepository tokenRepository;
        public AccessTokenAuthorizeAttribute()
        {
            tokenRepository = ClassInstance<TokenRepository>.Instance;
        }
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var request = actionContext.Request;
            if (!request.Headers.Contains("Access-Token")) return false;

            Guid tokenId = Guid.Empty;
            AccessToken accessToken = null;
            if (Guid.TryParse(request.Headers.GetValues("Access-Token").SingleOrDefault(), out tokenId))
            {
                object value = HttpRuntime.Cache.Get(tokenId.ToString());
                if (value != null) accessToken = value as AccessToken;
                if (accessToken == null)
                {
                    accessToken = tokenRepository.GetAccessToken(tokenId);
                    if (accessToken != null)
                        HttpRuntime.Cache.Add(tokenId.ToString(), accessToken, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 20, 0), CacheItemPriority.Normal,
                            (k, v, r) => tokenRepository.RemoveExpired());
                }
            }

            bool passed = (accessToken != null);
            //if (passed && !string.IsNullOrWhiteSpace(this.Roles))
            //    passed &= this.Roles.Contains(accessToken.RoleName.ToString());
            if (passed && !string.IsNullOrWhiteSpace(this.Users))
                passed &= this.Roles.Contains(accessToken.UserId.ToString());

            if (passed)
            {
                var principal = new GenericPrincipal(new GenericIdentity(accessToken.UserId.ToString()), new string[] { string.Empty });
                Thread.CurrentPrincipal = principal;
                HttpContext.Current.User = principal;
            }

            return passed;
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            throw new HttpResponseException(actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "授权错误"));
        }
    }
}
