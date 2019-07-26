using Mic.Api.Filter;
using Mic.Api.Models;
using Mic.Entity;
using Mic.Repository;
using Mic.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Mic.Api.Controllers
{
    /// <summary>
    /// 商家及分店歌单、播放记录管理
    /// </summary>
    [RoutePrefix("storeSong")]
    public class SongPlayController : ApiController
    {
        private StoreSongManageApiRepository storeSongManageRspository;
        private SongMarkRepository songMarkRepository;
        public SongPlayController()
        {
            storeSongManageRspository = ClassInstance<StoreSongManageApiRepository>.Instance;
            songMarkRepository = ClassInstance<SongMarkRepository>.Instance;
        }

        /// <summary>
        /// 根据分页获取歌单[AUTH]
        /// </summary>
        /// <param name="param">分页参数</param>
        /// <returns></returns>
        [HttpPost, Route("getSongSheet")]
        [AccessTokenAuthorize]
        public ResponseResultDto<PagedResult<SongInfoParam>> GetSongSheet(PageParam param)
        {
            HttpRequest request = HttpContext.Current.Request;
            string token = request.Headers.GetValues("Access-Token").FirstOrDefault();
            var result = storeSongManageRspository.GetSongSheet(token, param);
            return new ResponseResultDto<PagedResult<SongInfoParam>>
            {
                IsSuccess = result.Item1,
                ErrorMessage = result.Item2,
                Result = result.Item3
            };
        }

        /// <summary>
        /// 分页播放列表[AUTH]
        /// </summary>
        /// <param name="param">分页参数</param>
        /// <returns></returns>
        [HttpPost, Route("getPlayList")]
        [AccessTokenAuthorize]
        public ResponseResultDto<PagedResult<SongInfoParam>> GetPlayList(PageParam param)
        {
            HttpRequest request = HttpContext.Current.Request;
            string token = request.Headers.GetValues("Access-Token").FirstOrDefault();
            var result = storeSongManageRspository.GetMyPlayList(token, param);
            return new ResponseResultDto<PagedResult<SongInfoParam>>
            {
                IsSuccess = result.Item1,
                ErrorMessage = result.Item2,
                Result = result.Item3
            };
        }

        /// <summary>
        /// 将歌单中歌曲添加到播放列表[AUTH]
        /// </summary>
        /// <param name="songId">歌曲Id</param>
        /// <returns></returns>
        [HttpPost, Route("addSong2PlayList/{songId:int}")]
        [AccessTokenAuthorize]
        public ResponseResultDto<bool> AddSong2PlayList(int songId)
        {
            HttpRequest request = HttpContext.Current.Request;
            string token = request.Headers.GetValues("Access-Token").FirstOrDefault();
            var result = storeSongManageRspository.AddSong2MyPlayList(token, songId);
            return new ResponseResultDto<bool>
            {
                IsSuccess = result.Item1,
                ErrorMessage = result.Item2,
                Result = result.Item1
            };
        }

        /// <summary>
        /// 从播放列表中删除歌曲[AUTH]
        /// </summary>
        /// <param name="songId">歌曲Id</param>
        /// <returns></returns>
        [HttpPost, Route("deleteFromPlayList/{songId:int}")]
        [AccessTokenAuthorize]
        public ResponseResultDto<bool> DeleteSongFromMyPlayList(int songId)
        {
            HttpRequest request = HttpContext.Current.Request;
            string token = request.Headers.GetValues("Access-Token").FirstOrDefault();
            var result = storeSongManageRspository.DeleteSongFromMyPlayList(token, songId);
            return new ResponseResultDto<bool>
            {
                IsSuccess = result.Item1,
                ErrorMessage = result.Item2,
                Result = result.Item1
            };
        }

        /// <summary>
        /// 播放歌曲，返回歌曲文件地址和播放记录Id，记录Id用于更新播放时长,如果返回false，前端不能播放歌曲[AUTH]
        /// </summary>
        /// <param name="songId">歌曲Id</param>
        /// <param name="playUserId">播放用户Id</param>
        /// <param name="storeCode">用户所属商家编码</param>
        /// <returns></returns>
        [HttpPost, Route("playSong/{songId:int}/{playUserId:int}/{storeCode:guid}")]
        [AccessTokenAuthorize]
        public ResponseResultDto<PlaySongParam> AddPlayRecord(int songId, int playUserId, string storeCode)
        {
            HttpRequest request = HttpContext.Current.Request;
            string token = request.Headers.GetValues("Access-Token").FirstOrDefault();
            var result = storeSongManageRspository.AddPlayRecord(token, songId, playUserId, storeCode);
            return new ResponseResultDto<PlaySongParam>
            {
                IsSuccess = result.Item1,
                ErrorMessage = result.Item2,
                Result = new PlaySongParam
                {
                    RecordId = result.Item3,
                    SongId = songId,
                    SongPath = result.Item4
                }
            };
        }

        /// <summary>
        /// 结束播放歌曲或切换歌曲时，需要调用，将播放时间更新到后台，用于统计[AUTH]
        /// </summary>
        /// <param name="recordId">播放记录Id</param>
        /// <param name="secondTime">播放时长（单位 秒）</param>
        /// <returns></returns>
        [HttpPost, Route("endPlaySong/{recordId:int}/{secondTime:int}")]
        [AccessTokenAuthorize]
        public ResponseResultDto<bool> UpdatePlayRecordTime(int recordId, int secondTime)
        {
            HttpRequest request = HttpContext.Current.Request;
            string token = request.Headers.GetValues("Access-Token").FirstOrDefault();
            var result = storeSongManageRspository.UpdatePlayRecordTime(recordId, secondTime);
            return new ResponseResultDto<bool>
            {
                IsSuccess = result,
                ErrorMessage = string.Empty,
                Result = result
            };
        }

        /// <summary>
        /// 获取历史歌单简要信息列表[AUTH]
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("getHisPlayList")]
        [AccessTokenAuthorize]
        public ResponseResultDto<List<PlayListEntity>> GetHisPlayList()
        {
            HttpRequest request = HttpContext.Current.Request;
            string token = request.Headers.GetValues("Access-Token").FirstOrDefault();
            var result = storeSongManageRspository.GetHisPlayList(token);
            return new ResponseResultDto<List<PlayListEntity>>
            {
                IsSuccess = result.Item1,
                ErrorMessage = result.Item2,
                Result = result.Item3
            };
        }

        /// <summary>
        /// 根据历史歌单Id，获取歌单中的歌曲。[AUTH]
        /// </summary>
        /// <param name="id">历史歌单Id</param>
        /// <returns></returns>
        [HttpPost, Route("getSongSheetSongs/{id:int}/{order}")]
        [AccessTokenAuthorize]
        public ResponseResultDto<List<SongInfoParam>> GetHisSongListById(int id)
        {
            List<SongInfoParam> list = new List<SongInfoParam>();
            var result = storeSongManageRspository.GetSongListById(id);

            List<SongMarkEntity> songMarkList = songMarkRepository.GetSongMakList();
            foreach (var item in result)
            {
                string[] arr = item.SongMark.Split(',');
                var markNames = string.Empty;
                foreach (var markId in arr)
                {
                    if (!string.IsNullOrWhiteSpace(markId))
                    {
                        var songmark = songMarkList.FirstOrDefault(a => a.Id == Convert.ToInt32(markId));
                        if (songmark != null)
                        {
                            markNames += (songmark.MarkName + "、");
                        }
                    }
                }
                if (markNames.Length > 0)
                {
                    item.SongMarkStr = markNames.Substring(0, markNames.Length - 1);
                }
                else
                {
                    item.SongMarkStr = string.Empty;
                }
                list.Add(item);

            }
            return new ResponseResultDto<List<SongInfoParam>>
            {
                IsSuccess = true,
                ErrorMessage = string.Empty,
                Result = list
            };
        }
    }
}
