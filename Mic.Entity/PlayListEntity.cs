using System;

namespace Mic.Entity
{
    //歌单表
    public class PlayListEntity
    {
        public int Id { get; set; }
        public string ListName { get; set; } //歌单名称
        public string ListContent { get; set; }//
        public int StoreId { get; set; } //歌单所属商家Id或分店Id
        public string StoreCode { get; set; }//歌单所属商家编码
        public bool IsPublish { get; set; } // 是否发布，没发布，则为后台管理员编辑的，可发布后同步商家
        public DateTime UpdateTime { get; set; } //更新时间
        public bool Status { get; set; } //是否删除
    }
}
