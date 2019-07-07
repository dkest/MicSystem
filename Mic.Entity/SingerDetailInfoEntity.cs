namespace Mic.Entity
{
    public class SingerDetailInfoEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string SingerName { get; set; }
        public string SingerNameForStore { get; set; }
        public int SingerTypeId { get; set; }
        public string HeadImg { get; set; }
        public string IdCardNo { get; set; }
        public string IdCardFront { get; set; }
        public string IdCardBack { get; set; }
        public string Introduce { get; set; }
        public string Authentication { get; set; }
        public bool Status { get; set; }
    }
}
