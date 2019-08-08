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
        public int SongMark { get; set; }
        public int SongSize { get; set; }
        public int SongBPM { get; set; }
        public DateTime UploadTime { get; set; }
        public int PlayTimes { get; set; }
        public int TotalPlayTime { get; set; }
    }
}
