using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mic.Entity
{
    public class User : IEntity<int>
    {
        public int Id { get; private set; }

        public string UserId { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }
    }
}
