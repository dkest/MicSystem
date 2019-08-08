using System;

namespace Mic.Entity
{
    /// <summary>
    /// 歌曲操作明细-
    /// </summary>
    public class SongOptDetail
    {
        public int Id { get; set; }
        public int SongId { get; set; }
        public int AuditTimes { get; set; }
        public int AuditStatus { get; set; }//审核状态 0-待发布；1-待审核；2-已通过；3-未通过
        public string Note { get; set; }
        public DateTime AuditTime { get; set; }
        public int AuditAdminId { get; set; }
        public string AuditUser { get; set; }
        public int OptType { get; set; }//操作类型 1-上传，2-发布 3-更新信息 4-审核
        public DateTime OptTime { get; set; } //操作时间，操作先后顺序排序
        public string SongName { get; set; }
    }
}
