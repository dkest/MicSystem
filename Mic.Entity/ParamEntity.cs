using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mic.Entity
{
    public class ParamEntity
    {
        public int Id { get; set; }
        public int ParamType { get; set; } //1:音乐人入驻条款
        public string ParamContent { get; set; }
        public string Memo { get; set; }
    }
}
