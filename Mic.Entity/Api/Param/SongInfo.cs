using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mic.Entity
{
    public class SongInfo
    {
        public int Id { get; set; }
        public string SongName { get; set; }
        public string SongLength { get; set; }
        public DateTime ExpirationTime { get; set; }
        public string CopyrightFilePath { get; set; }
        public string FullCopyrightFilePath { get { return WebConfig.RootUrl + CopyrightFilePath; } }
        public string SongPath { get; set; }
        public string FullSongPath { get { return WebConfig.RootUrl + SongPath; } }
        public string SongMark { get; set; }
        public int SongSize { get; set; }
        public string SongBPM { get; set; }
        public DateTime UploadTime { get; set; }
        public int PlayTimes { get; set; }
        public int TotalPlayTime { get; set; }
        public int AuditStatus { get; set; }
        public string AuditStatusStr
        {
            get {
                string result = string.Empty;
                switch (AuditStatus)
                {
                    case 0:
                        result = "待发布";
                        break;
                    case 1:
                        result = "待审核";
                        break;
                    case 2:
                        result = "已通过";
                        break;
                    case 3:
                        result = "未通过";
                        break;
                    default:
                        result = "未知";
                        break;
                }
                return result;
            }

        }
    }
}
