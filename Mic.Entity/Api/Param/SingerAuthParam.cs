namespace Mic.Entity
{
    public class SingerAuthParam
    {
        public int SingerId { get; set; }
        public string HeadImg { get; set; }
        public string SingerName { get; set; }

        public int SingerTypeId { get; set; }
        //public string SingerTypeName { get; set; }
        public string IdCardNo { get; set; }

        public string IdCardFront { get; set; }
        public string IdCardFrontFullPath
        {
            get {
                return WebConfig.RootUrl + IdCardFront;
            }
        }
        public string IdCardBack { get; set; }
        public string IdCardBackFullPath
        {
            get {
                return WebConfig.RootUrl + IdCardBack;
            }
        }

        public string Introduce { get; set; }

        public int Authentication { get; set; }// 0-未申请(未认证)，1-已申请（待审核），2审核不通过，3审核通过（已认证）

    }
}
