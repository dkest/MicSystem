using System.Collections.Generic;

namespace Mic.Entity
{
    public class PagedResult<TResult>
    {
        public List<TResult> Results { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public int Total { get; set; }
    }
}
