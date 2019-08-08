using Mic.Api.Filter;
using Mic.Api.Models;
using Mic.Entity;
using Mic.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Mic.Api.Controllers
{
    /// <summary>
    /// 音乐人歌曲管理
    /// </summary>
    [RoutePrefix("singerSong")]
    public class SingerSongController : ApiController
    {
        private SingerSongApiRepository singerSongApiRepository;
        public SingerSongController()
        {
            singerSongApiRepository = ClassInstance<SingerSongApiRepository>.Instance;
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
            if (song.SingerId < 1 || string.IsNullOrWhiteSpace(song.SongName) ||
                string.IsNullOrWhiteSpace(song.SongPath) || string.IsNullOrWhiteSpace(song.SongLength)
                || song.SongSize < 0)
            {
                return new ResponseResultDto<int>
                {
                    IsSuccess = false,
                    ErrorMessage = "参数异常",
                    Result = 0
                };
            }

            HttpRequest request = HttpContext.Current.Request;
            string token = request.Headers.GetValues("Access-Token").FirstOrDefault();

            var result = singerSongApiRepository.AddSongInfo(song, token);
            //int songId = singerSongApiRepository.AddSongInfo(song,token);

            return new ResponseResultDto<int>
            {
                IsSuccess = result.Item1,
                ErrorMessage = result.Item2,
                Result = result.Item3 //上传歌曲后，返回歌曲Id
            };
        }

        /// <summary>
        /// 更新歌曲信息[AUTH]
        /// 操作类型 1-上传，2-发布 3-更新信息 4-审核
        /// </summary>
        /// <param name="song"></param>
        /// <returns></returns>
        [HttpPost, Route("updateSong")]
        [AccessTokenAuthorize]
        public ResponseResultDto<bool> UpdateSongInfo(UploadSongParam song)
        {
            if (song.Id < 1 || song.SingerId < 1 || string.IsNullOrWhiteSpace(song.SongName) ||
               string.IsNullOrWhiteSpace(song.SongPath) || string.IsNullOrWhiteSpace(song.SongLength)
               || song.SongSize < 0)
            {
                return new ResponseResultDto<bool>
                {
                    IsSuccess = false,
                    ErrorMessage = "参数异常",
                    Result = false
                };
            }

            var result = singerSongApiRepository.UpdateSongInfo(song);
            return new ResponseResultDto<bool>
            {
                IsSuccess = result,
                ErrorMessage = string.Empty,
                Result = result
            };
        }


        /// <summary>
        /// 音乐人发布歌曲[AUTH]
        /// </summary>
        /// <param name="songId"></param>
        /// <param name="songName"></param>
        /// <returns></returns>
        [HttpPost, Route("publishSong/{songId:int}/{songName}")]
        [AccessTokenAuthorize]
        public ResponseResultDto<bool> PublishSong(int songId,string songName)
        {
            if (songId < 0)//参数检查
            {
                return new ResponseResultDto<bool>
                {
                    IsSuccess = false,
                    ErrorMessage = "参数异常",
                    Result = false
                };
            }

            var result = singerSongApiRepository.PublishSong(songId,songName);

            return new ResponseResultDto<bool>
            {
                IsSuccess = result,
                ErrorMessage = string.Empty,
                Result = result
            };
        }


        /// <summary>
        /// 逻辑删除歌曲[AUTH]
        /// </summary>
        /// <param name="songId">歌曲Id</param>
        /// <returns></returns>
        [HttpPost, Route("deleteSong/{songId:int}")]
        [AccessTokenAuthorize]
        public ResponseResultDto<bool> DeleteSong(int songId)
        {
            if (songId < 0)//参数检查
            {
                return new ResponseResultDto<bool>
                {
                    IsSuccess = false,
                    ErrorMessage = "参数异常",
                    Result = false
                };
            }

            var result = singerSongApiRepository.DeleteSong(songId);
            return new ResponseResultDto<bool>
            {
                IsSuccess = result,
                ErrorMessage = string.Empty,
                Result = result
            };
        }

        /// <summary>
        /// 显示歌曲上传审核详情[AUTH]
        /// </summary>
        /// <param name="songId"></param>
        /// <returns></returns>
        [HttpPost, Route("songAuditDetail/{songId:int}")]
        [AccessTokenAuthorize]
        public ResponseResultDto<List<SongOptDetail>> SongAuditDeatil(int songId)
        {
            if (songId < 0)//参数检查
            {
                return new ResponseResultDto<List<SongOptDetail>>
                {
                    IsSuccess = false,
                    ErrorMessage = "参数异常",
                    Result = null
                };
            }

            var result = singerSongApiRepository.SongAuditDeatil(songId);
            return new ResponseResultDto<List<SongOptDetail>>
            {
                IsSuccess = true,
                ErrorMessage = string.Empty,
                Result = result
            };
        }

        /// <summary>
        /// 根据音乐人Id，分页获取音乐人未发布的歌曲，包括待发布/待审核/未通过[AUTH]
        /// </summary>
        /// <param name="singerId">音乐人Id</param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost, Route("getUnPublishSongList/{singerId:int}")]
        [AccessTokenAuthorize]
        public ResponseResultDto<PagedResult<SongInfoParam>> GetUnPublishSongList(int singerId, PageParam param)
        {
            if (singerId < 0)//参数检查
            {
                return new ResponseResultDto<PagedResult<SongInfoParam>>
                {
                    IsSuccess = false,
                    ErrorMessage = "参数异常",
                    Result = null
                };
            }
            var result = singerSongApiRepository.GetUnPublishSongList(singerId, param);
            return new ResponseResultDto<PagedResult<SongInfoParam>>
            {
                IsSuccess = true,
                ErrorMessage = string.Empty,
                Result = result
            };
        }


        /// <summary>
        /// 获取歌曲信息详情[AUTH]
        /// </summary>
        /// <param name="singerId">歌手Id</param>
        /// <param name="songId">歌曲Id</param>
        /// <returns></returns>
        [HttpPost, Route("songDetail/{singerId:int}/{songId:int}")]
        [AccessTokenAuthorize]
        public ResponseResultDto<SongInfo> GetSongDetail(int singerId, int songId)
        {
            if (singerId < 0 || songId < 0)//参数检查
            {
                return new ResponseResultDto<SongInfo>
                {
                    IsSuccess = false,
                    ErrorMessage = "参数异常",
                    Result = null
                };
            }
            var result = singerSongApiRepository.GetSongDetailInfo(singerId, songId);
            return new ResponseResultDto<SongInfo>
            {
                IsSuccess = result == null ? false : true,
                ErrorMessage = string.Empty,
                Result = result
            };
        }

        #endregion

        #region 音乐人-已发布的

        /// <summary>
        /// 分页获取音乐人已经发布的歌曲列表[AUTH]
        /// </summary>
        /// <param name="singerId">音乐人Id</param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost, Route("getPublishSongList/{singerId:int}")]
        [AccessTokenAuthorize]
        public ResponseResultDto<PagedResult<SongInfoParam>> GetPublishSongList(int singerId, PageParam param)
        {
            if (singerId < 0)//参数检查
            {
                return new ResponseResultDto<PagedResult<SongInfoParam>>
                {
                    IsSuccess = false,
                    ErrorMessage = "参数异常",
                    Result = null
                };
            }
            var result = singerSongApiRepository.GetPublishSongList(singerId, param);
            return new ResponseResultDto<PagedResult<SongInfoParam>>
            {
                IsSuccess = true,
                ErrorMessage = string.Empty,
                Result = result
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
            if (singerId < 0)//参数检查
            {
                return new ResponseResultDto<SingerStatisticsParam>
                {
                    IsSuccess = false,
                    ErrorMessage = "参数异常",
                    Result = null
                };
            }
            var result = singerSongApiRepository.GetSingerSongStatistics(singerId);
            return new ResponseResultDto<SingerStatisticsParam>
            {
                IsSuccess = true,
                ErrorMessage = string.Empty,
                Result = result
            };

        }

        /// <summary>
        /// 根据歌曲Id获取该歌曲按商家播放统计[AUTH]
        /// 播放时间倒序
        /// </summary>
        /// <param name="songId"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost, Route("songStattisticsByStore/{songId:int}")]
        [AccessTokenAuthorize]
        public ResponseResultDto<PagedResult<SongByStoreParam>> SongStattisticsByStore(int songId, PageParam param)
        {
            var result = singerSongApiRepository.SongStattisticsByStore(songId, param);
            return new ResponseResultDto<PagedResult<SongByStoreParam>>
            {
                IsSuccess = true,
                ErrorMessage = string.Empty,
                Result = result
            };
        }

        /// <summary>
        /// 根据歌曲Id获取该歌曲播放记录详情列表[AUTH]
        /// 播放时间倒序
        /// </summary>
        [HttpPost, Route("getSongPlayRecordDetail/{songId:int}")]
        [AccessTokenAuthorize]
        public ResponseResultDto<PagedResult<SongByStoreRecordParam>> SongStattisticsDetailByStore(int songId, PageParam param)
        {
            var result = singerSongApiRepository.SongStattisticsDetailByStore(songId, param);
            return new ResponseResultDto<PagedResult<SongByStoreRecordParam>>
            {
                IsSuccess = true,
                ErrorMessage = string.Empty,
                Result = result
            };
        }

        #endregion





    }
}
