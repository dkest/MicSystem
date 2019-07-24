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
        public Tuple<bool, string, PagedResult<SongInfoParam>> GetSongSheet(string token, PageParam param)
        {
            UserEntity user = helper.Query<UserEntity>($@"select a.* from [User] a left join [UserAccessToken] b on a.Id=b.UserId where b.TokenId='{token}'").FirstOrDefault();
            if (user == null)
            {
                Tuple.Create(false, "用户还未登录", 0, new PagedResult<SongInfoParam>());
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
                return Tuple.Create(false, "该用户不是商家或者分店，没有歌单", new PagedResult<SongInfoParam>());
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

            return Tuple.Create(true, string.Empty, new PagedResult<SongInfoParam>
            {
                Page = param.PageIndex,
                PageSize = param.PageSize,
                Results = helper.Query<SongInfoParam>(sql).ToList(),
                Total = tempList.Count
            });

        }

        /// <summary>
        /// 获取我的播放列表，播放列表的歌曲需要在我的歌单中选择歌曲
        /// </summary>
        /// <param name="token"></param>
        /// <param name="param"></param>
        public Tuple<bool, string, PagedResult<SongInfoParam>> GetMyPlayList(string token, PageParam param)
        {

            UserEntity user = helper.Query<UserEntity>($@"select a.* from [User] a left join [UserAccessToken] b on a.Id=b.UserId where b.TokenId='{token}'").FirstOrDefault();
            if (user == null)
            {
                Tuple.Create(false, "授权失败", new PagedResult<SongInfoParam>());
            }

            var listContent = helper.QueryScalar($@"select PlayListStr  from StoreDetailInfo where UserId={user.Id}");
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

            return Tuple.Create(true, string.Empty, new PagedResult<SongInfoParam>
            {
                Page = param.PageIndex,
                PageSize = param.PageSize,
                Results = helper.Query<SongInfoParam>(sql).ToList(),
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
        /// 从播放列表中删除歌曲
        /// </summary>
        /// <param name="token"></param>
        /// <param name="songId"></param>
        /// <returns></returns>
        public Tuple<bool, string> DeleteSongFromMyPlayList(string token, int songId)
        {
            StringBuilder sb = new StringBuilder();
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
                if (!tempList.Contains(songId))
                {
                    return Tuple.Create(false, "当前歌曲不在播放列表中");
                }
                else
                {
                    foreach (var temp in tempList)
                    {
                        if (temp != songId)
                        {
                            sb.Append(temp).Append(",");
                        }
                    }

                }
            }
            //更新用户播放列表
            var result = helper.Execute($@"update StoreDetailInfo set PlayListStr='{sb.ToString()}' where UserId={user.Id}");
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
        public Tuple<bool, string, int> AddPlayRecord(string token, int songId, int playUserId, string storeCode)
        {
            UserEntity user = helper.Query<UserEntity>($@"select a.* from [User] a left join [UserAccessToken] b on a.Id=b.UserId where b.TokenId='{token}'").FirstOrDefault();
            if (user == null)
            {
                Tuple.Create(false, "用户还未登录", 0);
            }
            if (user.Id != playUserId || user.StoreCode != storeCode)
            {
                return Tuple.Create(false, "Token 信息和用户Id或商家编码不匹配", 0);
            }

            var p = new DynamicParameters();
            p.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var result = helper.Execute($@"insert into SongPlayRecord (SongId,PlayUserId,BeginPlayTime,BroadcastTime,StoreCode) values 
({songId},{playUserId},'{DateTime.Now}'{0},'{storeCode}');SELECT @Id=SCOPE_IDENTITY()", p);
            var recordId = p.Get<int>("@Id");
            return Tuple.Create(true, string.Empty, recordId);

        }

        /// <summary>
        /// 获取历史歌单简要信息列表
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public Tuple<bool, string, List<PlayListEntity>> GetHisPlayList(string token)
        {

            UserEntity user = helper.Query<UserEntity>($@"select a.* from [User] a left join [UserAccessToken] b on a.Id=b.UserId where b.TokenId='{token}'").FirstOrDefault();
            if (user == null)
            {
                Tuple.Create(false, "用户还未登录", 0, new PagedResult<SongInfoParam>());
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
                return Tuple.Create(false, "该用户不是商家或者分店，没有歌单", new List<PlayListEntity>());
            }
            //string storeCode = helper.QueryScalar($@"select StoreCode from [User] where Id={storeId}").ToString();
            string sql = $@"select Id,ListName,UpdateTime from PlayList where StoreId='{storeId}' and IsPublish=1 order by UpdateTime desc;";
            return Tuple.Create(true, string.Empty, helper.Query<PlayListEntity>(sql).ToList());
        }

        /// <summary>
        /// 根据历史歌单Id，获取歌单中的歌曲
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<SongInfoParam> GetSongListById(int id)
        {

            var listContent = helper.QueryScalar($@"select ListContent from [PlayList] where Id={id}");

            string[] arr = listContent.ToString().Split(',');
            StringBuilder sb = new StringBuilder(" (");
            foreach (var item in arr)
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    sb.Append(item).Append(",");
                }
            }
            string whereIn = sb.ToString();
            int length = whereIn.Length;
            whereIn = whereIn.Substring(0, length - 1);
            whereIn += ")";


            string sql = $@"select d.*,e.SingerName,e.SongLength,e.SongMark from (select a.SongId, b.SongName, Sum(BroadcastTime) as TotalPlayTime,count(1) as PlayTimes 
 from [dbo].[SongPlayRecord] a  left join SongBook b
on a.SongId = b.Id where  a.SongId in {whereIn}
 group by a.SongId,b.SongName) as d  left join SongBook e on d.SongId=e.Id";
            //return helper.Query<StorePlaySongEntity>(sql).ToList();

            return helper.Query<SongInfoParam>(whereIn).ToList();

        }
    }
}
