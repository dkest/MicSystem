using System;

namespace Mic.Entity
{
    public class SonSongSheetParam
    {
        public int Id { get; set; }
        public string ListName { get; set; } //歌单名称

        public int StoreId { get; set; } //歌单所属商家Id或分店Id
        public string StoreName { get; set; } //分店名称
        public string StoreCode { get; set; }//歌单所属商家编码
        public int SongCount { get; set; } //包含歌曲数量
        public DateTime UpdateTime { get; set; } //更新时间
    }
}
