using System;

namespace Mic.Entity
{
    public class LoginLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int UserType { get; set; }
        public string StoreCode { get; set; }
        public DateTime LoginTime { get; set; }
    }
}
