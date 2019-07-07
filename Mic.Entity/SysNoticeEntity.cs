using System;

namespace Mic.Entity
{
    public class SysNoticeEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
        public DateTime NoticeTime { get; set; }
        public bool IsRead { get; set; }
    }
}
