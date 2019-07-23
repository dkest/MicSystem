using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mic.Entity
{
    public class StoreInfoParam
    {
        public int Id { get; set; }// 商家Id，用户表中的主键 Id
        public string StoreName { get; set; } //商家名称
        public string Phone { get; set; }
        public string Password { get; set; }
        public string BusinessLicense { get; set; }//营业执照
        public string LegalPerson { get; set; } //企业法人
        public string LegalPersonIdCardF { get; set; } //企业法人身份证正面路径
        public string FullLegalPersonIdCardF
        {
            get { return "http://152.136.227.143" + LegalPersonIdCardF; }
        }
        public string LegalPersonIdCardB { get; set; } //企业法人身份证反面路径
        public string FullLegalPersonIdCardB
        {
            get { return "http://152.136.227.143" + LegalPersonIdCardB; }
        }

        public int Province { get; set; }
        //public string ProvinceName { get; set; }
        public int City { get; set; }
        //public string CityName { get; set; }
        public int County { get; set; }
        //public string CountyName { get; set; }
        public string DetailAddress { get; set; }


        public string Contacts { get; set; } //联系人
        public string ContactsPhone { get; set; } //联系人电话

        public int StoreTypeId { get; set; }
        public string StoreTypeName { get; set; }//商家类型名称
              
        //public int UserType { get; set; } // 1-音乐人；2-商家；3-分店
        //public bool IsMain { get; set; } //只有当userType=2的时候，此字段才有效，1是商家主体，0则为商家员工
        //public string StaffName { get; set; } //userType=2的时候，为员工名称，=3时，为分店名称
        //public string StoreCode { get; set; } //只有当userType ！=2的时候，此字段才有效，所属商家编码
        //public bool StoreManage { get; set; } //userType=2才有效，IsMain=1，是，该字段为0
        //public bool SongManage { get; set; }
        //public bool UserManage { get; set; }
        //public DateTime LastLoginTime { get; set; }
        //public bool Status { get; set; }

        
    }

    public class StoreStatistic
    {
        /// <summary>
        /// 最大分店数量
        /// </summary>
        public int MaxCount { get; set; }
        /// <summary>
        /// 已启用分店数量
        /// </summary>
        public int ValidCount { get; set; }
    }
}
