using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using MediaResource.Web.Helper;
using MediaResource.Web.Models;
using MediaResource.Web.Models.ViewModels;
using MediaResource.Web.Services;

using PagedList;

namespace MediaResource.Web.Controllers
{
	public class UserVideoController : Controller
	{
		private readonly UserVideoService _userVideoService = new UserVideoService();
		private readonly UserFolderService _userFolderService = new UserFolderService();
        private readonly GroupService _groupService = new GroupService();

        private const string UserUploadVideoPath = @"mov1\UserUpload\Video\";
        private const string UserUploadVideoUrl = @"mov1/UserUpload/Video/";

		// GET: UserVideo/ListPartial
		[ChildActionOnly]
		public ActionResult ListPartial()
		{
			int userId = WebHelper.Instance.CurrentUser.Id;
			List<UserVideo> userVideos = _userVideoService.GetList(userId);
			return PartialView("_ListPartial", userVideos);
		}

		// GET: UserVideo/GroupPartial
		[ChildActionOnly]
		public ActionResult GroupPartial(int? groupId)
		{
			ViewBag.GroupId = groupId;

			return PartialView("_GroupPartial", _userVideoService.GetTopImages(9, groupId));
		}

		//
		// GET: /UserVideo/Detail
		public ActionResult Detail(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			UserVideo userVideo = _userVideoService.View(id);
			if (userVideo == null)
			{
				return HttpNotFound();
			}


			return View(userVideo);
		}

		//
		// GET: /UserVideo/Group/5
		public ActionResult Group(int? id, int? pageSize, int? page)
		{
			Group group = _groupService.Find(id);

            ViewBag.GroupId = id;
            ViewBag.Group = group;

			IPagedList<ImageViewModel> images = _userVideoService.GetImagesByGroup(id, pageSize, page);

			return View(images);
		}

        //
        // GET: /UserVideo/Search
        public ActionResult Search(string keyword, int? pageSize, int? page)
        {
            ViewBag.Keyword = keyword;

            IPagedList<ImageViewModel> images = _userVideoService.Search(keyword, pageSize, page);

            return View(images);
        }

		// GET: UserVideo/Create
		public ActionResult Create()
		{
			// 获取用户的照片目录
			int userId = WebHelper.Instance.CurrentUser.Id;
			List<UserFolder> userVideoFolders = _userFolderService.GetList(ObjectType.Video, userId);

			var listItems = userVideoFolders.Select(userVideoFolder => new SelectListItem
			{
				Text = userVideoFolder.Name,
				Value = userVideoFolder.Id.ToString(CultureInfo.InvariantCulture)
			}).ToList();

			ViewBag.UserVideoFolders = listItems;

			return View();
		}

		// POST: UserVideo/Create
		// 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
		// 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "Title, FolderId, IsPublic")] UserVideo userVideo, HttpPostedFileBase videoFile)
		{
			if (videoFile != null && videoFile.ContentLength > 0)
            {
                string folderPath = WebHelper.Instance.RootPath + UserUploadVideoPath + WebHelper.Instance.CurrentUser.Name;
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string fileExtension = videoFile.FileName.Substring(videoFile.FileName.LastIndexOf('.'));
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + fileExtension;
                string filePath = Path.Combine(folderPath, fileName);
                videoFile.SaveAs(filePath);

                string fileUrl = UserUploadVideoUrl + WebHelper.Instance.CurrentUser.Name + "/" + fileName;
                userVideo.FileUrl = fileUrl;
			}

			userVideo.CreateBy = WebHelper.Instance.CurrentUser.Id;
			userVideo.CreateDate = DateTime.Now;

			if (ModelState.IsValid)
			{
				_userVideoService.Create(userVideo);
				return RedirectToAction("Index", "User", new {activeTab = 5});
			}

			return View(userVideo);
		}

		// GET: UserVideo/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			UserVideo userVideo = _userVideoService.Find(id);
			if (userVideo == null)
			{
				return HttpNotFound();
			}

			// 获取用户的照片目录
			int userId = WebHelper.Instance.CurrentUser.Id;
			List<UserFolder> userVideoFolders = _userFolderService.GetList(ObjectType.Video, userId);

			var listItems = userVideoFolders.Select(userVideoFolder => new SelectListItem
			{
				Text = userVideoFolder.Name,
				Value = userVideoFolder.Id.ToString(CultureInfo.InvariantCulture),
				Selected = (userVideoFolder.Id == userVideo.FolderId)
			}).ToList();

			ViewBag.UserVideoFolders = listItems;
			ViewBag.VideoUrl = WebHelper.Instance.RootUrl + userVideo.FileUrl;

			return View(userVideo);
		}

		// POST: UserVideo/Edit/5
		// 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
		// 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "Id, Title, FileUrl, FolderId, IsPublic, CreateBy, CreateDate")] UserVideo userVideo, HttpPostedFileBase videoFile)
		{
			if (ModelState.IsValid)
            {
                bool fileChanged = false;
				if (videoFile != null && videoFile.ContentLength > 0)
                {
                    fileChanged = true;

					// 删除原有文件
					string oldFilePath = Path.Combine(WebHelper.Instance.RootPath, userVideo.FileUrl);
					if (System.IO.File.Exists(oldFilePath))
					{
						System.IO.File.Delete(oldFilePath);
					}

					// 保存新上传的文件
                    string folderPath = WebHelper.Instance.RootPath + UserUploadVideoPath + WebHelper.Instance.CurrentUser.Name;
					if (!Directory.Exists(folderPath))
					{
						Directory.CreateDirectory(folderPath);
					}
					string fileExtension = videoFile.FileName.Substring(videoFile.FileName.LastIndexOf('.'));
					string fileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + fileExtension;
					string filePath = Path.Combine(folderPath, fileName);
					videoFile.SaveAs(filePath);

                    string fileUrl = UserUploadVideoUrl + WebHelper.Instance.CurrentUser.Name + "/" + fileName;
					userVideo.FileUrl = fileUrl;
				}

                _userVideoService.Update(userVideo, fileChanged);
				return RedirectToAction("Index", "User", new {activeTab = 5});
			}
			return View(userVideo);
		}

		// POST: UserVideo/Delete/5
		public JsonResult Delete(int id)
		{
			UserVideo userVideo = _userVideoService.Find(id);
			if (userVideo != null)
			{
				_userVideoService.Delete(userVideo);
			}

			var jsonResult = new
			{
				success = true,
				message = "删除成功！"
			};
			return Json(jsonResult);
		}
	}
}