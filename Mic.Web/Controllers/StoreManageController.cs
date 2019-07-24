using Mic.Entity;
using Mic.Repository;
using Mic.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Mic.Web.Controllers
{
    public class StoreManageController : BaseController
    {
        private StoreTypeRepository storeTypeRepository;
        private StoreRepository storeRepository;
        private PlayListRepository playListRepository;
        private SongBookRepository songBookRepository;
        private SongMarkRepository songMarkRepository;
        public StoreManageController()
        {
            storeTypeRepository = ClassInstance<StoreTypeRepository>.Instance;
            storeRepository = ClassInstance<StoreRepository>.Instance;
            playListRepository = ClassInstance<PlayListRepository>.Instance;
            songBookRepository = ClassInstance<SongBookRepository>.Instance;
            songMarkRepository = ClassInstance<SongMarkRepository>.Instance;
        }
        public ActionResult StoreManageList()
        {
            return View();
        }
        public ActionResult StoreDetail()
        {
            ViewBag.storeId = GetIntValFromReq("storeId");
            return View();
        }

        public ActionResult HisSongList()
        {
            ViewBag.storeId = GetIntValFromReq("storeId");
            return View();
        }

        public ActionResult EditSongPlayList()
        {
            ViewBag.id = GetStrValFromReq("id");
            //ViewBag.listContent = GetStrValFromReq("listContent");
            ViewBag.storeId = GetIntValFromReq("storeId");
            
            return View();
        }

        #region 商家类型
        public ActionResult GetStoreTypeList()
        {
            var result = storeTypeRepository.GetStoreTypeList();
            return Json(new { code = 0, msg = "", count = result.Count, data = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddOrUpdateStoreType(StoreTypeEntity storeTypeEntity)
        {
            var result = storeTypeRepository.AddOrUpdateStoreType(storeTypeEntity);
            return Json(new { status = result.Item1, data = result.Item2 });
        }
        public ActionResult DeleteStoreType(int id)
        {
            var result = storeTypeRepository.DeleteStoreType(id);
            return Json(new { status = result.Item1, msg = result.Item2, JsonRequestBehavior.AllowGet });

        }
        #endregion

        #region 商家详细信息
        public ActionResult GetStoreList()
        {
            var page = GetIntValFromReq("page");
            var limit = GetIntValFromReq("limit");
            var keyword = GetStrValFromReq("keyword");
            var param = new PageParam
            {
                PageIndex = page,
                PageSize = limit,
                Keyword = keyword
            };

            var result = storeRepository.GetStoreList(param);
            return Json(new { code = 0, msg = "", count = result.Item1, data = result.Item2 }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 设置商家账号启用和禁用
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public ActionResult UpdateStoreStatus(int id, bool status)
        {
            var result = storeRepository.UpdateStoreStatus(id, status);
            return Json(new { status = result }, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 添加或更新商家信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ActionResult AddOrUpdateStoreInfo(StoreDetailInfoEntity entity)
        {
            var result = storeRepository.AddOrUpdateStoreInfo(entity);
            return Json(new { status = result.Item1, data = result.Item2 }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据商家ID，获取上将详细信息
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public ActionResult GetStoreDetailById(int storeId)
        {
            var result = storeRepository.GetStoreDetailById(storeId);
            return Json(new { status = result.Item1, data = result.Item2 }, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 根据商家Id，获取商家的所有分店
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public ActionResult GetSonStoreListByStoreId()
        {
            int storeId = GetIntValFromReq("storeId");
            var result = storeRepository.GetSonStoreListById(storeId);
            return Json(new { code = 0, msg = "", count = result.Count, data = result }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 管理员获取后台歌单
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPlayListByStoreId()
        {
            int storeId = GetIntValFromReq("storeId");
            var result = playListRepository.GetStoreSongListForAdmin(storeId);
            return Json(new { code = 0, msg = result.Item2, listItem = result.Item3, count = result.Item4.Count, data = result.Item4 }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 根据歌单Id获取
        /// </summary>
        /// <returns></returns>
        //public ActionResult GetPlayListByListContent()
        //{
        //    string listContent = GetStrValFromReq("listContent");
        //    var result = playListRepository.GetStoreSongListForAdminByListContent(listContent);
        //    return Json(new { code = 0, msg = string.Empty, count = result.Count, data = result }, JsonRequestBehavior.AllowGet);
        //}


        public ActionResult GetHisPlayList(int storeId)
        {
            //int storeId = GetIntValFromReq("storeId");
            var result = playListRepository.GetHisPlayList(storeId);
            return Json(new { status = true, data = result }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetHisSongList()
        {
            List<SongBookEntity> list = new List<SongBookEntity>();
            string listContent = GetStrValFromReq("listContent");
            var result = playListRepository.GetSongListByPlayListStr(listContent);

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
            return Json(new { code = 0, msg = string.Empty, count = result.Count, data = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PublishSongList(int playListId)
        {
            var result = playListRepository.PublishSongList(playListId);
            return Json(new { status = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateSongList(int id, string listContent)
        {
            PlayListEntity playListEntity = new PlayListEntity
            {
                Id = id,
                ListContent = listContent
            };
            var result = playListRepository.UpdateSongList(playListEntity);
            return Json(new { status = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AppendSongList(int id, string listContent,string storeCode,int storeId)
        {
            PlayListEntity playListEntity = new PlayListEntity
            {
                Id = id,
                ListContent = listContent,
                StoreCode = storeCode,
                StoreId = storeId
            };
            var result = playListRepository.AppendSongList(playListEntity);
            return Json(new { status = result }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取歌曲列表，添加歌单的时候，可供选择的歌曲
        /// </summary>
        /// <returns></returns>
        public ActionResult GetApprovedSongList()
        {
            try
            {
                List<SongBookEntity> list = new List<SongBookEntity>();
                string keyword = GetStrValFromReq("keyword");
                string searchMarkId = GetStrValFromReq("searchMarkId");
                string idStr = GetStrValFromReq("ids");
                var songIds =  idStr.Split(',');
                List<int> temp = new List<int>();
                foreach (var songId in songIds)
                {
                    if (!string.IsNullOrWhiteSpace(songId))
                    {
                        temp.Add(Convert.ToInt32(songId));
                    }
                }
                var result = songBookRepository.GetApprovedSongList(keyword, searchMarkId);
                List<SongMarkEntity> songMarkList = songMarkRepository.GetSongMakList();
                foreach (var item in result)
                {
                    if (!temp.Contains(item.Id))
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
                }

                return Json(new { code = 0, msg = string.Empty, count = list.Count, data = list }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion
    }
}