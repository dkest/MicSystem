using Mic.Entity;
using Mic.Repository.Dapper;
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

            return helper.Query<SongOptDetail>($@"select * from SongAuditDetail where SongId={songId}").ToList(); ;
           
        }

    }
}
