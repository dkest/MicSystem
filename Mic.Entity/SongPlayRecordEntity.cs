using System;

namespace Mic.Entity
{
    public class SongPlayRecordEntity : SongBookEntity
    {
        public new int Id { get; set; }
        public int SongId { get; set; }
        public int PlayUserId { get; set; }
        public DateTime BeginPlayTime { get; set; }
        public string BeginPlayTimeStr
        {
            get {
                return BeginPlayTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
        public int BroadcastTime { get; set; } // 这首歌放了多长时间

        public string StoreName { get; set; }
        public string StoreTypeName { get; set; }


    }
}
