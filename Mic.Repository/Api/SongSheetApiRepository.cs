using Mic.Repository.Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mic.Repository.Api
{
    /// <summary>
    /// 分店歌单 相关逻辑
    /// </summary>
    public class SongSheetApiRepository
    {
        DapperHelper<SqlConnection> helper;
        public SongSheetApiRepository()
        {
            helper = new DapperHelper<SqlConnection>(WebConfig.SqlConnection);
        }

    }
}
