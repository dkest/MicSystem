using Mic.Entity;
using Mic.Repository.Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Mic.Repository.Api
{
    public class NoticeRepository
    {
        DapperHelper<SqlConnection> helper;
        public NoticeRepository()
        {
            helper = new DapperHelper<SqlConnection>(WebConfig.SqlConnection);
        }

        /// <summary>
        /// 根据用户Id，按时间倒序获取系统消息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Tuple<int, List<SysNoticeEntity>> GetNoticeByUserId(int userId, int pageSize, int pageIndex)
        {

            string sql = string.Format(@"
                select top {0} * from (select row_number() over(order by NoticeTime desc) as rownumber, 
* from SysNotice where UserId={2} and Status=1 ) temp_row
                    where temp_row.rownumber>(({1}-1)*{0})", pageSize, pageIndex, userId);
            int count = Convert.ToInt32(helper.QueryScalar($@"select count(1) from SysNotice where UserId={userId} and Status=1"));

            return Tuple.Create(count, helper.Query<SysNoticeEntity>(sql).ToList());
        }

        /// <summary>
        /// 根据用户Id，按时间倒序获取未读系统消息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Tuple<int, List<SysNoticeEntity>> GetUnReadNoticeByUserId(int userId, int pageSize, int pageIndex)
        {
            string sql = string.Format(@"
                select top {0} * from (select row_number() over(order by NoticeTime desc) as rownumber, 
* from SysNotice where UserId={2} and Status=1 and IsRead=0 ) temp_row
                    where temp_row.rownumber>(({1}-1)*{0})", pageSize, pageIndex, userId);
            int count = Convert.ToInt32(helper.QueryScalar($@"select count(1) from SysNotice where UserId={userId} and Status=1 and IsRead=0"));

            return Tuple.Create(count, helper.Query<SysNoticeEntity>(sql).ToList());
        }

        /// <summary>
        /// 获取消息详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SysNoticeEntity GetNoticeDetail(int id)
        {
            var result = helper.Query<SysNoticeEntity>($@"select * from SysNotice where Id={id}").FirstOrDefault(); ;
            return result;
        }

        /// <summary>
        /// 删除消息通知
        /// </summary>
        /// <param name="noticeId"></param>
        /// <returns></returns>
        public bool DeleteNotice(int noticeId)
        {
            return helper.Execute($@"update SysNotice set Status=0 where Id={noticeId}") > 0 ? true : false;
        }
        /// <summary>
        /// 标记为已读
        /// </summary>
        /// <param name="noticeId"></param>
        /// <returns></returns>
        public bool MarkRead(int noticeId)
        {
            return helper.Execute($@"update SysNotice set IsRead=1 where Id={noticeId}") > 0 ? true : false;
        }

    }
}
