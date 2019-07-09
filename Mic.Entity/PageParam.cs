using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mic.Repository
{
    public class PageParam
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Keyword { get; set; }
        public string OrderField { get; set; }
        public string OrderType { get; set; }
    }

    public class AuditSongPageParam : PageParam
    {
        public int AuditStatus { get; set; } = -1;
    }
}
