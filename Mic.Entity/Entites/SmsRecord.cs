using System;

namespace Mic.Entity
{
    public class SmsRecord
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Phone { get; set; }
        public string Code { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Status { get; set; }
    }
}