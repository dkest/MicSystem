using System;

namespace Mic.Entity
{
    public class SongAuditDetail
    {
        public int Id { get; set; }
        public int SongId { get; set; }
        public int AuditTimes { get; set; }
        public int AuditStatus { get; set; }
        public string Note { get; set; }
        public DateTime AuditTime { get; set; }
        public int AuditAdminId { get; set; }
    }
}
