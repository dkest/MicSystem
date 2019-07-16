using System;

namespace Mic.Entity
{
    public class SingerDetailInfoEntity : UserEntity
    {
        public int SingerId { get; set; }
        public new int Id { get; set; }
        public int UserId { get; set; }
        public string SingerName { get; set; }
        public string SingerNameForStore { get; set; }
        public int SingerTypeId { get; set; }
        public string HeadImg { get; set; }
        public string IdCardNo { get; set; }
        public string IdCardFront { get; set; }
        public string IdCardBack { get; set; }
        public string Introduce { get; set; }
        public int Authentication { get; set; }// 0-未申请(未认证)，1-已申请（待审核），2审核不通过，3审核通过（已认证）
        public string AuthenticationStr
        {
            get {
                string result = string.Empty;
                switch (Authentication)
                {
                    case 0:
                        result= "未认证";
                        break;
                    case 1:
                        result= "待审核";
                        break;
                    case 2:
                        result= "未通过";
                        break;
                    case 3:
                        result= "已认证";
                        break;
                }
                return result;
            }
        }
        //public bool AuthStatus { get; set; }
        public DateTime CreateTime { get; set; }
        public bool Enabled { get; set; } //是否启用
    }
}
