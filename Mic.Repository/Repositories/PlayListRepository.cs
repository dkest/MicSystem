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
        public Tuple<bool, string, PlayListEntity, List<SongBookEntity>> GetStoreSongListForAdmin(int storeId)
        {
            bool status = false;
            string message = string.Empty;
            string updateTimeStr = string.Empty;
            PlayListEntity listItem = new PlayListEntity();
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
                    updateTimeStr = unPublish.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    listItem = unPublish;

                    //else
                    //{
                    //    var temp = result.OrderByDescending(a => a.UpdateTime).FirstOrDefault();
                    //    songListStr = temp.ListContent;
                    //    updateTimeStr = temp.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    //    listItem = temp;
                    //}
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
                { //该商家没有后台歌单
                    status = false;
                    message = "该商家没有歌单";
                    //listItem = new PlayListEntity();
                }
            }
            else
            {
                status = false;
                message = "该商家没有歌单";

            }
            return Tuple.Create(status, message, listItem, list);
        }

        public List<SongBookEntity> GetStoreSongListForAdminByListContent(string listContent)
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

        /// <summary>
        /// 发布歌单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool PublishSongList(int id)
        {
            return helper.Execute($@"update PlayList set IsPublish=1,UpdateTime='{DateTime.Now}' where Id ={id}") > 0 ? true : false;
        }

        /// <summary>
        /// 添加歌单
        /// </summary>
        /// <returns></returns>
        public bool AddSongList(PlayListEntity playList)
        {
            string sql = $@"insert into PlayList ([ListName],[ListContent],[StoreCode],[IsPublish],[UpdateTime],[Status]) 
values ('{playList.ListName}','{playList.ListContent}','{playList.StoreCode}','{playList.IsPublish}','{DateTime.Now}','{1}')";
            return helper.Execute(sql) > 0 ? true : false;
        }

        /// <summary>
        /// 更新歌单
        /// </summary>
        /// <returns></returns>
        public bool UpdateSongList(PlayListEntity playList)
        {
            //            string sql = $@"update PlayList set [ListName]='{playList.ListName}',[ListContent]='{playList.ListContent}',
            //[StoreCode]='{playList.StoreCode}',[IsPublish]='{playList.IsPublish}',[UpdateTime]='{DateTime.Now}') ;";

            string sql = $@"update PlayList set [ListContent]='{playList.ListContent}',
[UpdateTime]='{DateTime.Now}' where Id = {playList.Id} ;";
            return helper.Execute(sql) > 0 ? true : false;
        }

    }
}
