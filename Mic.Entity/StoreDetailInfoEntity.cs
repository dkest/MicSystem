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
        public int Province { get; set; }
        public string ProvinceName { get; set; }
        public int City { get; set; }
        public string CityName { get; set; }
        public int County { get; set; }
        public string CountyName { get; set; }
        public string DetailAddress { get; set; }
        public int MaximumStore { get; set; } //最大分店数量
        public int ValidSonStoreNum { get; set; } //有效分店数量
        public string DelegatingContract { get; set; } //授权合同文件路径
        public string FullDelegatingContractPath
        {
            get { return "http://152.136.227.143" + DelegatingContract; }
        }
        public DateTime CreateTime { get; set; }
        public bool Enabled { get; set; }
        public string Contacts { get; set; } //联系人
        public string ContactsPhone { get; set; } //联系人电话
        //public bool Status { get; set; }
    }
}
