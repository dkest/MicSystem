using Dapper;
using Mic.Entity;
using Mic.Repository.Dapper;
using System;
using System.Collections.Generic;
using System.Data;
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


        /// <summary>
        /// 获取商家所有分店
        /// </summary>
        /// <param name="storeCode"></param>
        /// <returns></returns>
        public List<SonStoreParam> GetSonStoreList(string storeCode)
        {
            string sql = $@"select a.Id as SonStoreId,b.StoreName as SonStoreName,a.StoreCode,
                            case when c.Id>0 then 1 else 0 end
                            as HasSongSheet
                            from [User] a left join StoreDetailInfo b on a.Id=b.UserId
                            left  join PlayList c on (c.StoreId=a.Id  and c.IsPublish=1 and c.Status=1)
                            where a.StoreCode='{storeCode}' and a.UserType=3 ";
            return helper.Query<SonStoreParam>(sql).ToList();
        }

        /// <summary>
        /// 获取商家分店歌单列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public PagedResult<SonSongSheetParam> GetSonStoreSongSheetList(SonSongSheetListPageParam param)
        {
            string sql = string.Format(@"
                select top {0} * from (select row_number() over(order by c.UpdateTime desc) as rownumber, 
c.Id,c.ListName,c.StoreId,c.StoreCode,b.StoreName,c.ListContent,c.UpdateTime  from PlayList c left join [User] a on c.StoreId = a.Id left join StoreDetailInfo b on a.Id=b.UserId
                            where a.StoreCode={2} and a.UserType=3 and c.IsPublish=1 and c.Status=1) temp_row
                    where temp_row.rownumber>(({1}-1)*{0})", param.PageSize, param.PageIndex, param.StoreCode);
            int count = Convert.ToInt32(helper.QueryScalar($@"select count(1) from PlayList c left join [User] a on c.StoreId = a.Id left join StoreDetailInfo b on a.Id=b.UserId
                            where a.StoreCode='{param.StoreCode}' and a.UserType=3 and c.IsPublish=1 and c.Status=1"));
            var result = helper.Query<SonSongSheetParam>(sql).ToList();
            return new PagedResult<SonSongSheetParam>
            {
                Total = count,
                Results = result,
                Page = param.PageIndex,
                PageSize = param.PageSize
            };
        }
        /// <summary>
        /// 添加歌单
        /// </summary>
        /// <param name="songSheet"></param>
        /// <returns>新增加的歌单的Id</returns>
        public int AddSongSheet(PlayListEntity songSheet)
        {
            var p = new DynamicParameters();
            p.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var result = helper.Execute($@"insert into PlayList (ListName,ListContent,StoreId,StoreCode,IsPublish,UpdateTime,Status)
values ('{songSheet.ListName}','{songSheet.ListContent}',{songSheet.StoreId},'{songSheet.StoreCode}',
{1},'{DateTime.Now}',{1}); SELECT @Id=SCOPE_IDENTITY()", p);
            return p.Get<int>("@Id");
        }

        /// <summary>
        /// 更新歌单
        /// </summary>
        /// <param name="songSheet"></param>
        /// <returns></returns>
        public bool UpdateSongSheetById(PlayListEntity songSheet)
        {
            string sql = $@"update PlayList set ListName='{songSheet.ListName}',StoreId={songSheet.StoreId},
ListContent='{songSheet.ListContent}',UpdateTime='{DateTime.Now}' where Id={songSheet.Id}";
            return helper.Execute(sql) > 0 ? true : false;
        }

        /// <summary>
        /// 删除歌单，逻辑删除
        /// </summary>
        /// <param name="songSheetId"></param>
        /// <returns></returns>
        public bool DeleteSongSheetById(int songSheetId)
        {
            string sql = $@"update PlayList set Status=0 where Id={songSheetId}";
            return helper.Execute(sql) > 0 ? true : false;
        }
        /// <summary>
        /// 赋值歌单
        /// </summary>
        /// <param name="songSheet"></param>
        /// <returns></returns>
        public Tuple<bool, string, int> CopySongSheet(int songSheetId, string songSheetName, int storeId)
        {
            var oldListContent = string.Empty;
            var storeCode = string.Empty;
            var temp = helper.Query<PlayListEntity>($@"selct ListContent,StoreCode from PlayList where Id={songSheetId}").FirstOrDefault();
            if (temp != null)
            {
                oldListContent = temp.ListContent;
                storeCode = temp.StoreCode;
            }
            else
            {
                return Tuple.Create(false, "要复制的歌单不存在", 0);
            }
            var p = new DynamicParameters();
            p.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var result = helper.Execute($@"insert into PlayList (ListName,ListContent,StoreId,StoreCode,IsPublish,UpdateTime,Status)
values ('{songSheetName}','{oldListContent}',{storeId},'{storeCode}',
{1},'{DateTime.Now}',{1}); SELECT @Id=SCOPE_IDENTITY()", p);
            int id = p.Get<int>("@Id");
            return Tuple.Create(true, string.Empty, id);
        }

        /// <summary>
        /// 根据商家或分店Id，获取歌单简要信息
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public SonSongSheetParam GetSongSheetInfoByStoreId(int storeId)
        {
            string sql = $@"select c.Id,c.ListName,c.StoreId,c.StoreCode,b.StoreName,c.ListContent,c.UpdateTime  from PlayList c 
left join [User] a on c.StoreId = a.Id left join StoreDetailInfo b on a.Id=b.UserId
                            where a.UserType=3 and c.IsPublish=1 and c.Status=1 and c.StoreId={storeId}";
            return helper.Query<SonSongSheetParam>(sql).FirstOrDefault();
        }





    }


}
