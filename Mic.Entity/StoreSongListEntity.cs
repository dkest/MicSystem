using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mic.Entity
{
    /// <summary>
    /// 分店歌单关系表
    /// </summary>
    public class StoreSongListEntity
    {
        public int Id { get; set; }
        public int PlayListId { get; set; } // 歌单表Id
        public int StoreId { get; set; }
        public bool Status { get; set; }
    }
}
