using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mic.Entity
{
    /// <summary>
    /// 商家详细信息
    /// </summary>
    public class StoreDetailInfoEntity : UserEntity
    {
        public int StoreId { get; set; }
        public new int Id { get; set; }
        public int UserId { get; set; }
        public string StoreName { get; set; }
        public int StoreTypeId { get; set; }
        public string StoreTypeName { get; set; }//商家类型名称
        public string BusinessLicense { get; set; }
        public string LegalPerson { get; set; }
        public string Provice { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string DetailAddress { get; set; }
        public int MaximumStore { get; set; } //最大分店数量
        public int ValidSonStoreNum { get; set; } //有效分店数量
        public string DelegatingContract { get; set; } //授权合同文件路径
        
        public DateTime CreateTime { get; set; }
        public bool Enabled { get; set; }
        //public bool Status { get; set; }
    }
}
