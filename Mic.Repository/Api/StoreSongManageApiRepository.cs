using Dapper;
using Mic.Entity;
using Mic.Repository.Dapper;
using Mic.Repository.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Mic.Repository.Repositories
{
    public class StoreSongManageApiRepository
    {
        DapperHelper<SqlConnection> helper;

        public StoreSongManageApiRepository()
        {
            helper = new DapperHelper<SqlConnection>(WebConfig.SqlConnection);
        }


        /// <summary>
        /// 根据登录用户获取歌单
        /// 如果是 商家，显示商家歌单 UserType=2，IsMain=1则获取 PlayList中的 userId的歌单</summary>
        /// 如果是商家员工，显示商家歌单 
        /// 如果是分店，显示分店歌单
        /// 其他不显示歌单
        /// <param name="userId"></param>
        /// <param name="param"></param>
        public Tuple<bool, string, PagedResult<SongBookEntity>> GetSongPlayList(string token, PageParam param)
        {
            UserEntity user = helper.Query<UserEntity>($@"select a.* from [User] a left join [UserAccessToken] b on a.Id=b.UserId where b.TokenId='{token}'").FirstOrDefault();
            if (user == null)
            {
                Tuple.Create(false, "用户还未登录", 0, new PagedResult<SongBookEntity>());
            }
            int storeId = user.Id;
            if (user.UserType == 2)
            {
                if (user.IsMain)
                {
                    storeId = user.Id;
                }
                else
                {
                    var temp = helper.QueryScalar($@"select Id from [User] a left join [User] b on a.StoreCode=b.StoreCode where b.Id={user.Id} and a.UserType=2 and a.IsMain=1");
                    if (temp != null)
                    {
                        storeId = Convert.ToInt32(temp);
                    }
                }
            }
            if (user.UserType == 1)
            {
                return Tuple.Create(false, "该用户不是商家或者分店，没有歌单", new PagedResult<SongBookEntity>());
            }
            // 获取歌单 只获取最后增加的一个歌单
            var listContent = helper.QueryScalar($@"select ListContent from PlayList where StoreId={storeId} and IsPublish={1} order by Id desc");
            string[] arr = listContent.ToString().Split(',');
            StringBuilder sb = new StringBuilder(" (");
            List<int> tempList = new List<int>();
            foreach (var item in arr)
            {
                if (string.IsNullOrWhiteSpace(item))
                {
                    tempList.Add(Convert.ToInt32(item));
                    sb.Append(item).Append(",");
                }
            }

            string resultSql = sb.ToString();
            int length = resultSql.Length;
            resultSql = resultSql.Substring(0, length - 1);
            resultSql += ");";

            string likeSql = string.IsNullOrWhiteSpace(param.Keyword) ? string.Empty : $@" and (d.SingerName like '%{param.Keyword}%'  or d.SongName like '%{param.Keyword}%')";
            string sql = string.Format(@"
                select top {0} * from (select row_number() over(order by {4} d.UploadTime desc) as rownumber, 
* from SongBook d left join (select COUNT(a.Id) as PlayTimes , Sum(b.BroadcastTime) as TotalPlayTime ,a.Id as tempId
from SongPlayRecord b left join  SongBook a  on a.Id = b.SongId    where a.Status=1 and a.AuditStatus=2 
group by a.Id) c on c.tempId = d.Id where d.Status=1 and d.AuditStatus=2 and d.Id in {3}  {2}) temp_row
                    where temp_row.rownumber>(({1}-1)*{0});", param.PageSize, param.PageIndex, likeSql, resultSql,
                                string.IsNullOrWhiteSpace(param.OrderField) ? string.Empty : ("c." + param.OrderField + " " + param.OrderType + ","));

            return Tuple.Create(false, string.Empty, new PagedResult<SongBookEntity>
            {
                Page = param.PageIndex,
                PageSize = param.PageSize,
                Results = helper.Query<SongBookEntity>(sql).ToList(),
                Total = tempList.Count
            });

        }

        /// <summary>
        /// 获取我的播放列表，播放列表的歌曲需要在我的歌单中选择歌曲
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="param"></param>
        public Tuple<bool, string, PagedResult<SongBookEntity>> GetMyPlayList(int userId, PageParam param)
        {
            var listContent = helper.QueryScalar($@"select PlayListStr  from StoreDetailInfo where UserId={userId}");
            string[] arr = listContent.ToString().Split(',');
            StringBuilder sb = new StringBuilder(" (");
            List<int> tempList = new List<int>();
            foreach (var item in arr)
            {
                if (string.IsNullOrWhiteSpace(item))
                {
                    tempList.Add(Convert.ToInt32(item));
                    sb.Append(item).Append(",");
                }
            }

            string resultSql = sb.ToString();
            int length = resultSql.Length;
            resultSql = resultSql.Substring(0, length - 1);
            resultSql += ");";

            string likeSql = string.IsNullOrWhiteSpace(param.Keyword) ? string.Empty : $@" and (d.SingerName like '%{param.Keyword}%'  or d.SongName like '%{param.Keyword}%')";
            string sql = string.Format(@"
                select top {0} * from (select row_number() over(order by {4} d.UploadTime desc) as rownumber, 
* from SongBook d left join (select COUNT(a.Id) as PlayTimes , Sum(b.BroadcastTime) as TotalPlayTime ,a.Id as tempId
from SongPlayRecord b left join  SongBook a  on a.Id = b.SongId    where a.Status=1 and a.AuditStatus=2 
group by a.Id) c on c.tempId = d.Id where d.Status=1 and d.AuditStatus=2 and d.Id in {3}  {2}) temp_row
                    where temp_row.rownumber>(({1}-1)*{0});", param.PageSize, param.PageIndex, likeSql, resultSql,
                                string.IsNullOrWhiteSpace(param.OrderField) ? string.Empty : ("c." + param.OrderField + " " + param.OrderType + ","));

            return Tuple.Create(false, string.Empty, new PagedResult<SongBookEntity>
            {
                Page = param.PageIndex,
                PageSize = param.PageSize,
                Results = helper.Query<SongBookEntity>(sql).ToList(),
                Total = tempList.Count
            });
        }

        /// <summary>
        /// 添加歌曲到我的播放列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="songId"></param>
        public Tuple<bool, string> AddSong2MyPlayList(string token, int songId)
        {

            UserEntity user = helper.Query<UserEntity>($@"select a.* from [User] a left join [UserAccessToken] b on a.Id=b.UserId where b.TokenId='{token}'").FirstOrDefault();

            //先获取我的播放列表
            var playListStr = helper.QueryScalar($@"select PlayListStr from StoreDetailInfo where UserId={user.Id}").ToString();
            if (string.IsNullOrWhiteSpace(playListStr))// 直接将歌曲添加到列表
            {
                playListStr += ("," + songId);
            }
            else//判断该歌曲是否已经在歌单中
            {
                string[] playListArr = playListStr.Split(',');
                List<int> tempList = new List<int>();
                foreach (var item in playListArr)
                {
                    if (string.IsNullOrWhiteSpace(item))
                    {
                        tempList.Add(Convert.ToInt32(item));
                    }
                }
                if (tempList.Contains(songId))
                {
                    return Tuple.Create(false, "当前歌曲已经在播放列表中了，请勿重复添加");
                }
                else
                {
                    playListStr += ("," + songId);
                }
            }
            //更新用户播放列表
            var result = helper.Execute($@"update StoreDetailInfo set PlayListStr='{playListStr}' where UserId={user.Id}");
            return Tuple.Create(result > 0 ? true : false, string.Empty);
        }

        /// <summary>
        /// 结束一首歌时，更新播放时长
        /// </summary>
        /// <param name="recordId">播放时返回的Id</param>
        /// <param name="time">秒</param>
        public bool UpdatePlayRecordTime(int recordId, int time)
        {
            string sql = $@"update SongPlayRecord set BroadcastTime={time} where Id={recordId}";
            return helper.Execute(sql) > 0 ? true : false;
        }


        /// <summary>
        /// 播放歌曲时调用，添加播放记录,并返回播放记录Id，用于更新播放时长
        /// </summary>
        /// <param name="songId"></param>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public int AddPlayRecord(int songId, int storeId)
        {
            var p = new DynamicParameters();
            p.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var result = helper.Execute($@"insert into SongPlayRecord (SongId,PlayUserId,BeginPlayTime,BroadcastTime) values 
({songId},{storeId},'{DateTime.Now}'{0});SELECT @Id=SCOPE_IDENTITY()", p);
            return p.Get<int>("@Id");

        }

    }
}
