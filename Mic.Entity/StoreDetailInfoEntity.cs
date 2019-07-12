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
    public class StoreDetailInfoEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string StoreName { get; set; }
        public int StoreTypeId { get; set; }
        public string BusinessLicense { get; set; }
        public string LegalPerson { get; set; }
        public int Provice { get; set; }
        public int City { get; set; }
        public int County { get; set; }
        public string DetailAddress { get; set; }
        public int MaximumStore { get; set; } //最大分店数量
        public string DelegatingContract { get; set; } //授权合同文件路径
        public bool Status { get; set; }
    }
}
