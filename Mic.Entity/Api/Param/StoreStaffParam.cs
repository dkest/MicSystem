namespace Mic.Entity
{

    /// <summary>
    /// 
    /// </summary>
    public class StoreStaffParam
    {
        public int Id { get; set; }
        public string StaffName { get; set; }

        public string Password { get; set; }
        public string Phone { get; set; }
        
      
        public bool StoreManage { get; set; }
        public bool SongManage { get; set; }
        public bool UserManage { get; set; }
        public bool Enable { get; set; }//是否启用
        
    }
}