using Mic.Entity;
using Mic.Repository.Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Mic.Repository.Repositories
{
    public class SongAuditRepository
    {
        private DapperHelper<SqlConnection> helper;
        public SongAuditRepository()
        {
            helper = new DapperHelper<SqlConnection>(WebConfig.SqlConnection);
        }
        /// <summary>
        /// 获取歌曲的审核明细
        /// </summary>
        /// <param name="songId"></param>
        /// <returns></returns>
        public List<SongOptDetail> GetAuditDetail(int songId)
        {
            string sql = $@"select * from SongOptDetail where SongId={songId} order by OptTime asc;";
            return helper.Query<SongOptDetail>(sql).ToList(); ;
           
        }

        /// <summary>
        /// 审核歌曲
        /// </summary>
        /// <param name="song"></param>
        /// <returns></returns>
        public bool AuditSong(SongBookEntity song,int adminId)
        {
            if (song.AuditStatus==3)//不通过 需要通知歌手
            {
                helper.Execute($@"insert into SysNotice (Title,Content,UserId,NoticeTime,SongName,AuditAdminId) 
values ('歌曲审核未通过','{song.Memo}',{song.SingerId},'{DateTime.Now}','{song.SongName}',{adminId})");
            }
            else if (song.AuditStatus == 2) //审核通过
            {
                helper.Execute($@"insert into SysNotice (Title,Content,UserId,NoticeTime,SongName,AuditAdminId) 
values ('歌曲审核通过','{song.Memo}',{song.SingerId},'{DateTime.Now}','{song.SongName}',{adminId})");
            }
            string sql = $@"update SongBook set AuditStatus={song.AuditStatus},SongMark='{song.SongMark}',SongBPM='{song.SongBPM}'
 where Id={song.Id};
insert into SongOptDetail (SongId,Note,AuditStatus,OptType,OptTime) values(
                {song.Id},'{song.Memo}',{song.AuditStatus},{4},'{DateTime.Now}')";
            return helper.Execute(sql) > 0 ? true : false;
        }


    }
}
