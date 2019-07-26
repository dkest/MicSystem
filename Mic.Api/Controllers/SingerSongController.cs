using Mic.Api.Filter;
using Mic.Api.Models;
using Mic.Entity;
using Mic.Repository;
using System.Collections.Generic;
using System.Web.Http;

namespace Mic.Api.Controllers
{
    /// <summary>
    /// 音乐人歌曲管理
    /// </summary>
    [RoutePrefix("singerSong")]
    public class SingerSongController : ApiController
    {
        public SingerSongController()
        {

        }
        #region 音乐人-未发布作品


        /// <summary>
        /// 上传歌曲，添加歌曲记录（先上传文件，然后得到文件路径和时长等信息）[AUTH]
        /// </summary>
        /// <param name="song">歌曲信息</param>
        /// <returns></returns>
        [HttpPost, Route("addSong")]
        [AccessTokenAuthorize]
        public ResponseResultDto<int> AddSongInfo(UploadSongParam song)
        {
            if (true)//参数检查
            {

            }

            return new ResponseResultDto<int>
            {
                IsSuccess = true,
                ErrorMessage = string.Empty,
                Result = 0 //上传歌曲后，返回歌曲Id
            };
        }

        /// <summary>
        /// 更新歌曲信息[AUTH]
        /// </summary>
        /// <param name="song"></param>
        /// <returns></returns>
        [HttpPost, Route("updateSong")]
        [AccessTokenAuthorize]
        public ResponseResultDto<bool> UpdateSongInfo(UploadSongParam song)
        {
            if (true)//参数检查
            {

            }

            return new ResponseResultDto<bool>
            {
                IsSuccess = true,
                ErrorMessage = string.Empty,
                Result = true
            };
        }


        /// <summary>
        /// 音乐人发布歌曲[AUTH]
        /// </summary>
        /// <param name="songId"></param>
        /// <returns></returns>
        [HttpPost, Route("publishSong")]
        [AccessTokenAuthorize]
        public ResponseResultDto<bool> PublishSong(int songId)
        {
            if (true)//参数检查
            {

            }

            return new ResponseResultDto<bool>
            {
                IsSuccess = true,
                ErrorMessage = string.Empty,
                Result = true
            };
        }


        /// <summary>
        /// 逻辑删除歌曲[AUTH]
        /// </summary>
        /// <param name="songId">歌曲Id</param>
        /// <returns></returns>
        [HttpPost, Route("deleteSong")]
        [AccessTokenAuthorize]
        public ResponseResultDto<bool> DeleteSong(int songId)
        {
            if (true)//参数检查
            {

            }

            return new ResponseResultDto<bool>
            {
                IsSuccess = true,
                ErrorMessage = string.Empty,
                Result = true
            };
        }

        /// <summary>
        /// 显示歌曲上传审核详情[AUTH]
        /// </summary>
        /// <param name="songId"></param>
        /// <returns></returns>
        [HttpPost, Route("songDetail")]
        [AccessTokenAuthorize]
        public ResponseResultDto<List<SongOptDetail>> SongAuditDeatil(int songId)
        {
            if (true)//参数检查
            {

            }

            return new ResponseResultDto<List<SongOptDetail>>
            {
                IsSuccess = true,
                ErrorMessage = string.Empty,
                Result = new List<SongOptDetail>()
            };
        }

        /// <summary>
        /// 获取音乐人未发布的歌曲，包括待发布/待审核/未通过
        /// </summary>
        /// <param name="singId"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost, Route("getUnPublishSongList/{singerId:int}")]
        [AccessTokenAuthorize]
        public ResponseResultDto<PagedResult<SongInfoParam>> GetUnPublishSongList(int singId,PageParam param)
        {
            if (true)//参数检查
            {

            }

            return new ResponseResultDto<PagedResult<SongInfoParam>>
            {
                IsSuccess = true,
                ErrorMessage = string.Empty,
                Result = new PagedResult<SongInfoParam>()
                { }
            };
        }

        #endregion

        #region 音乐人-已发布的

        /// <summary>
        /// 分页获取音乐人已经发布的歌曲列表[AUTH]
        /// </summary>
        /// <param name="singId">音乐人Id</param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost, Route("getPublishSongList/{singerId:int}")]
        [AccessTokenAuthorize]
        public ResponseResultDto<PagedResult<SongInfoParam>> GetPublishSongList(int singId, PageParam param)
        {
            if (true)//参数检查
            {

            }

            return new ResponseResultDto<PagedResult<SongInfoParam>>
            {
                IsSuccess = true,
                ErrorMessage = string.Empty,
                Result = new PagedResult<SongInfoParam>()
                { }
            };
        }

        /// <summary>
        /// 获取音乐人统计数据，已经发布歌曲数和累计播放次数[AUTH]
        /// </summary>
        /// <param name="singerId">音乐人Id</param>
        /// <returns></returns>
        [HttpPost, Route("getSongStatistics/{singerId:int}")]
        [AccessTokenAuthorize]
        public ResponseResultDto<SingerStatisticsParam> GetSingerSongStatistics(int singerId)
        {
            if (true)//参数检查
            {

            }

            return new ResponseResultDto<SingerStatisticsParam>
            {
                IsSuccess = true,
                ErrorMessage = string.Empty,
                Result = new SingerStatisticsParam()
            };

        }

        /// <summary>
        /// 根据歌曲Id获取该歌曲播放统计[AUTH]
        /// 播放时间倒序
        /// </summary>
        [HttpPost, Route("getSongPlayRecord/{songId:int}")]
        [AccessTokenAuthorize]
        public void Get(int songId,PageParam param)
        {

        }

        /// <summary>
        /// 根据歌曲Id获取该歌曲播放记录详情列表[AUTH]
        /// 播放时间倒序
        /// </summary>
        [HttpPost, Route("getSongPlayRecordDetail/{songId:int}")]
        [AccessTokenAuthorize]
        public void Get2(int songId, PageParam param)
        {

        }

        #endregion





    }
}
