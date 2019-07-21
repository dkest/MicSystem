using System;

namespace Mic.Entity
{
    public class Admin
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Enabled { get; set; }
        public bool Status { get; set; }
    }
}
