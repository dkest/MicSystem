using Dapper;
using Mic.Entity;
using Mic.Repository.Dapper;
using Mic.Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Mic.Repository.Repositories
{
    public class PlayListRepository
    {
        DapperHelper<SqlConnection> helper;
        public PlayListRepository()
        {
            helper = new DapperHelper<SqlConnection>(WebConfig.SqlConnection);
        }

        /// <summary>
        /// 获取商家后台歌单，当不存在未发布歌单时，则为最新发布的歌单
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public Tuple<bool, string,string, List<SongBookEntity>> GetStoreSongListForAdmin(int storeId)
        {
            bool status = false;
            string message = string.Empty;
            string updateTimeStr = string.Empty;
            List<SongBookEntity> list = new List<SongBookEntity>();
            string storeCode = helper.QueryScalar($@"select StoreCode from [User] where Id={storeId}").ToString();
            string sql = $@"select * from PlayList where StoreCode='{storeCode}' and Status=1";
            var result = helper.Query<PlayListEntity>(sql).ToList();
            if (result.Count > 0)
            {
                string songListStr = string.Empty;
                var unPublish = result.Where(a => { return a.IsPublish == false; }).OrderByDescending(a => a.UpdateTime).FirstOrDefault();
                if (unPublish != null)
                {
                    songListStr = unPublish.ListContent;
                    updateTimeStr = unPublish.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss:ms");
                }
                else
                {
                    var temp = result.OrderByDescending(a => a.UpdateTime).FirstOrDefault();
                    songListStr = temp.ListContent;
                    updateTimeStr = temp.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss:ms");
                }
                string[] arr = songListStr.Split(',');
                StringBuilder sb = new StringBuilder("select * from SongBook where Id in (");
                foreach (var item in arr)
                {
                    sb.Append(item).Append(",");
                }
                string resultSql = sb.ToString();
                int length = resultSql.Length;
                resultSql = resultSql.Substring(0, length - 1);
                resultSql += ");";
                list = helper.Query<SongBookEntity>(resultSql).ToList();
            }
            else
            {
                status = false;
                message = "该商家没有歌单";
                
            }
            return Tuple.Create(status,message, updateTimeStr,list);
        }

        /// <summary>
        /// 获取历史歌单
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public List<PlayListEntity> GetHisPlayList(int storeId)
        {
            string storeCode = helper.QueryScalar($@"select StoreCode from [User] where Id={storeId}").ToString();
            string sql = $@"select * from PlayList where StoreCode='{storeCode}' and IsPublish=1 order by UpdateTime desc;";
            return helper.Query<PlayListEntity>(sql).ToList();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="listContent"></param>
        /// <returns></returns>
        public List<SongBookEntity> GetSongListByPlayListStr(string listContent)
        {
            string[] arr = listContent.Split(',');
            StringBuilder sb = new StringBuilder("select * from SongBook where Id in (");
            foreach (var item in arr)
            {
                sb.Append(item).Append(",");
            }
            string resultSql = sb.ToString();
            int length = resultSql.Length;
            resultSql = resultSql.Substring(0, length - 1);
            resultSql += ");";
            return helper.Query<SongBookEntity>(resultSql).ToList();
        }

    }
}
