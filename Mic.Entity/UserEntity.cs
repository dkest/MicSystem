using System;

namespace Mic.Entity
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public int UserType { get; set; } // 1-音乐人；2-商家；3-分店
        public bool IsMain { get; set; } //只有当userType=2的时候，此字段才有效，1是商家主体，0则为商家员工
        public string StaffName { get; set; } //userType=2的时候，为员工名称，=3时，为分店名称
        public string StoreCode { get; set; } //只有当userType ！=2的时候，此字段才有效，所属商家编码
        public bool StoreManage { get; set; } //userType=2才有效，IsMain=1，是，该字段为0
        public bool SongManage { get; set; }
        public bool UserManage { get; set; }
        public DateTime LastLoginTime { get; set; }
        public bool Status { get; set; }
    }
}
