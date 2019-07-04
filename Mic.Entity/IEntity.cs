using System.Collections.Generic;

namespace Mic.Entity
{
    public interface IEntity<TKey> where TKey : struct
    {
        TKey Id { get; }
    }

    public sealed class PageEnumerable<T>
    {
        public int CurrentPage { get; set; }
        public IEnumerable<T> Items { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }
}
