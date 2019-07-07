using System;

namespace Mic.Entity
{
    public class SongPlayRecordEntity
    {
        public int Id { get; set; }
        public int SongId { get; set; }
        public int PlayUserId { get; set; }
        public DateTime BeginPlayTime { get; set; }
        public int BroadcastTime { get; set; } // 这首歌放了多长时间
    }
}
