using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mic.Entity
{
    public class AccessToken
    {
        public Guid TokenId { get; set; }
        public int RoleId { get; set; }
        public int RoleName { get; set; }
        public int UserId { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime ExpireTime { get; set; }
    }
}
