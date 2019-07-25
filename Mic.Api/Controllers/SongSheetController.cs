using Mic.Api.Filter;
using Mic.Api.Models;
using Mic.Entity;
using Mic.Repository;
using Mic.Repository.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Mic.Api.Controllers
{
    /// <summary>
    /// 分店歌单管理
    /// </summary>
    [RoutePrefix("songSheetManage")]
    public class SongSheetController : ApiController
    {

        private SongSheetApiRepository songSheetApiRepository;
        public SongSheetController()
        {
            songSheetApiRepository = ClassInstance<SongSheetApiRepository>.Instance;
        }

        /// <summary>
        /// 获取商家所有分店，HasSongSheet=true，则有个歌单无法选择[AUTH]
        /// </summary>
        /// <param name="storeCode">商家编码</param>
        /// <returns></returns>
        [HttpPost, Route("getSonStoreList")]
        [AccessTokenAuthorize]
        public ResponseResultDto<List<SonStoreParam>> GetSonStoreList(string storeCode)
        {
            return new ResponseResultDto<List<SonStoreParam>>
            {
                IsSuccess = true,
                ErrorMessage = String.Empty,
                Result = null
            };
        }

        /// <summary>
        /// 获取商家分店歌单列表[AUTH]
        /// </summary>
        /// <param name="param">分页参数</param>
        /// <returns></returns>
        [HttpPost, Route("getSonSongSheetList")]
        [AccessTokenAuthorize]
        public ResponseResultDto<List<SonSongSheetParam>> GetSonSongSheetList(SonSongSheetListPageParam param)
        {
            return new ResponseResultDto<List<SonSongSheetParam>>
            {
                IsSuccess = true,
                ErrorMessage = String.Empty,
                Result = null
            };
        }

        /// <summary>
        /// 添加歌单,ListContent(歌曲Id，用逗号分隔)[AUTH]
        /// </summary>
        /// <param name="songSheet"></param>
        /// <returns></returns>
        [HttpPost, Route("add")]
        [AccessTokenAuthorize]
        public ResponseResultDto<int> AddSongSheet(PlayListEntity songSheet)
        {
            return new ResponseResultDto<int>
            {
                IsSuccess = true,
                ErrorMessage = string.Empty,
                Result = 0 //返回新添加的歌单的Id，便于直接查看歌单详情
            };
        }

        /// <summary>
        /// 更新歌单[AUTH]
        /// </summary>
        /// <param name="songSheet"></param>
        /// <returns></returns>
        [HttpPost, Route("update")]
        [AccessTokenAuthorize]
        public ResponseResultDto<bool> UpdateSongSheetById(PlayListEntity songSheet)
        {
            return new ResponseResultDto<bool>
            {
                IsSuccess = true,
                ErrorMessage = string.Empty,
                Result = true
            };
        }

        /// <summary>
        /// 删除歌单[AUTH]
        /// </summary>
        /// <param name="songSheetId">歌单Id</param>
        /// <returns></returns>
        [HttpPost, Route("delete/{songSheetId:int}")]
        [AccessTokenAuthorize]
        public ResponseResultDto<bool> UpdateSongSheetById(int songSheetId)
        {
            return new ResponseResultDto<bool>
            {
                IsSuccess = true,
                ErrorMessage = string.Empty,
                Result = true
            };
        }

        /// <summary>
        /// 复制歌单[AUTH]
        /// </summary>
        /// <param name="songSheetId">被复制的歌单Id</param>
        /// <param name="songSheetName">复制后的歌单名称</param>
        /// <param name="storeId">赋值给哪个分店Id</param>
        /// <returns></returns>
        [HttpPost, Route("copy/{songSheetId:int}/{songSheetName}/{storeId:int}")]
        [AccessTokenAuthorize]
        public ResponseResultDto<int> CopySongSheet(int songSheetId, string songSheetName, int storeId)
        {
            return new ResponseResultDto<int>
            {
                IsSuccess = true,
                ErrorMessage = string.Empty,
                Result = 0 //返回新添加的歌单的Id，便于直接查看歌单详情
            };
        }

        /// <summary>
        /// 根据分店Id，获取分店的歌单基本信息[AUTH]
        /// </summary>
        /// <param name="storeId">分店或商家Id</param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost, Route("getSongSheetInfo/{storeId:int}")]
        [AccessTokenAuthorize]
        public ResponseResultDto<SonSongSheetParam> GetSongSheetInfoByStoreId(int storeId, PageParam param)
        {
            return new ResponseResultDto<SonSongSheetParam>
            {
                IsSuccess = true,
                ErrorMessage = string.Empty,
                Result = null
            };
        }

        /// <summary>
        /// 根据分店Id，获取分店的歌单中的歌曲列表[AUTH]
        /// </summary>
        /// <param name="storeId">分店或商家Id</param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost, Route("getSongSheetContent/{storeId:int}")]
        [AccessTokenAuthorize]
        public ResponseResultDto<PagedResult<SongInfoParam>> GetSongSheetContentListByStoreId(int storeId, PageParam param)
        {
            return new ResponseResultDto<PagedResult<SongInfoParam>>
            {
                IsSuccess = true,
                ErrorMessage = string.Empty,
                Result = null
            };
        }

        /// <summary>
        /// 获取分店或商家播放歌曲统计[AUTH]
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost, Route("getPlayRecord/{storeId:int}")]
        [AccessTokenAuthorize]
        public ResponseResultDto<PagedResult<SongInfoParam>> GetPlayRecordByStoreId(int storeId, PageParam param)
        {
            return new ResponseResultDto<PagedResult<SongInfoParam>>
            {
                IsSuccess = true,
                ErrorMessage = string.Empty,
                Result = null
            };

        }
    }
}
