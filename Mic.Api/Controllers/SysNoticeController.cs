using Mic.Api.Filter;
using Mic.Api.Models;
using Mic.Entity;
using Mic.Repository;
using Mic.Repository.Api;
using System.Collections.Generic;
using System.Web.Http;

namespace Mic.Api.Controllers
{
    /// <summary>
    /// 音乐人消息通知
    /// </summary>
    [RoutePrefix("notice")]
    public class SysNoticeController : ApiController
    {
        private NoticeRepository noticeRepository;
        public SysNoticeController()
        {
            noticeRepository = ClassInstance<NoticeRepository>.Instance;
        }

        /// <summary>
        /// 根据用户Id，按时间倒序获取系统消息[AUTH]
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        [HttpGet, Route("getNoticeList/{userId:int}/{pageSize:int}/{pageIndex:int}")]
        [AccessTokenAuthorize]
        public ResponseResultDto<PagedResult<SysNoticeEntity>> GetNoticeByUserId(int userId, int pageSize, int pageIndex)
        {
            var result = noticeRepository.GetNoticeByUserId(userId, pageSize, pageIndex);
            return new ResponseResultDto<PagedResult<SysNoticeEntity>>
            {
                IsSuccess = true,
                ErrorMessage = string.Empty,
                Result = new PagedResult<SysNoticeEntity>
                {
                    Page = pageIndex,
                    PageSize = pageSize,
                    Total = result.Item1,
                    Results = result.Item2
                }
            };
        }

        /// <summary>
        /// 根据用户Id，按时间倒序获取未读系统消息[AUTH]
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        [HttpGet, Route("getUnReadNoticeList/{userId:int}/{pageSize:int}/{pageIndex:int}")]
        [AccessTokenAuthorize]
        public ResponseResultDto<PagedResult<SysNoticeEntity>> GetUnReadNoticeByUserId(int userId, int pageSize, int pageIndex)
        {
            var result = noticeRepository.GetUnReadNoticeByUserId(userId, pageSize, pageIndex);
            return new ResponseResultDto<PagedResult<SysNoticeEntity>>
            {
                IsSuccess = true,
                ErrorMessage = string.Empty,
                Result = new PagedResult<SysNoticeEntity>
                {
                    Page = pageIndex,
                    PageSize = pageSize,
                    Total = result.Item1,
                    Results = result.Item2
                }
            };
        }

        /// <summary>
        /// 获取消息详情,将消息标记为已读[AUTH]
        /// </summary>
        /// <param name="noticeId">消息通知Id</param>
        /// <returns></returns>
        [HttpGet, Route("getNoticeDetail/{noticeId:int}")]
        [AccessTokenAuthorize]
        public ResponseResultDto<SysNoticeEntity> GetNoticeDetail(int noticeId)
        {
            var result = noticeRepository.GetNoticeDetail(noticeId);
            noticeRepository.MarkRead(noticeId);
            return new ResponseResultDto<SysNoticeEntity>
            {
                IsSuccess = true,
                ErrorMessage = string.Empty,
                Result = result
            };
        }

        /// <summary>
        /// 删除消息通知[AUTH]
        /// </summary>
        /// <param name="noticeId">消息通知Id</param>
        /// <returns></returns>
        [HttpPost, Route("deleteNotice/{noticeId:int}")]
        [AccessTokenAuthorize]
        public ResponseResultDto<bool> DeleteNotice(int noticeId)
        {
            var result = noticeRepository.DeleteNotice(noticeId);
            return new ResponseResultDto<bool>
            {
                IsSuccess = true,
                ErrorMessage = string.Empty,
                Result = result
            };
        }
        /// <summary>
        /// 标记为已读[AUTH]
        /// </summary>
        /// <param name="noticeId">消息通知Id</param>
        /// <returns></returns>
        [HttpPost, Route("markRead/{noticeId:int}")]
        [AccessTokenAuthorize]
        public ResponseResultDto<bool> MarkRead(int noticeId)
        {
            var result = noticeRepository.MarkRead(noticeId);
            return new ResponseResultDto<bool>
            {
                IsSuccess = true,
                ErrorMessage = string.Empty,
                Result = result
            };
        }

        /// <summary>
        /// 批量标记为已读[AUTH]
        /// </summary>
        /// <param name="noticeIdList">消息通知Idl列表</param>
        /// <returns></returns>
        [HttpPost, Route("markReadList")]
        [AccessTokenAuthorize]
        public ResponseResultDto<bool> MarkReadList(List<int> noticeIdList)
        {
            var result = noticeRepository.MarkReadMany(noticeIdList);
            return new ResponseResultDto<bool>
            {
                IsSuccess = true,
                ErrorMessage = string.Empty,
                Result = result
            };
        }
    }
}
