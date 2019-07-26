namespace Mic.Entity
{
    public class PlaySongParam
    {
        public int SongId { get; set; }
        public int RecordId { get; set; }
        public string SongPath { get; set; }
        public string FullSongPath
        {
            get {
                return WebConfig.RootUrl + SongPath;
            }
        }
    }
}
