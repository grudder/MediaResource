using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;

using MediaResource.Web.Helper;
using MediaResource.Web.Models;
using MediaResource.Web.Services;

namespace MediaResource.Web.Controllers
{
    [Authorize]
	public class UserFolderController : Controller
	{
		private readonly UserFolderService _userFolderService = new UserFolderService();

		// GET: UserFolder/PhotoListPartial
		[ChildActionOnly]
		public ActionResult PhotoListPartial()
		{
			int userId = WebHelper.Instance.CurrentUser.Id;
			List<UserFolder> userFolders = _userFolderService.GetList(ObjectType.UserPhoto, userId);
			return PartialView("_PhotoListPartial", userFolders);
		}

		// GET: UserFolder/VideoListPartial
		[ChildActionOnly]
		public ActionResult VideoListPartial()
		{
			int userId = WebHelper.Instance.CurrentUser.Id;
            List<UserFolder> userFolders = _userFolderService.GetList(ObjectType.UserVideo, userId);
			return PartialView("_VideoListPartial", userFolders);
		}

		// GET: UserFolder/Create
		public ActionResult Create(ObjectType objectType)
		{
			var userFolder = new UserFolder
			{
				ObjectType = objectType
			};

			return View(userFolder);
		}

		// POST: UserFolder/Create
		// 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
		// 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "ObjectType, Name")] UserFolder userFolder)
		{
			userFolder.CreateBy = WebHelper.Instance.CurrentUser.Id;
			userFolder.CreateDate = DateTime.Now;

			if (ModelState.IsValid)
			{
				_userFolderService.Create(userFolder);

			    int activeTab = (userFolder.ObjectType == ObjectType.UserPhoto) ? 2 : 4;
				return RedirectToAction("Index", "User", new {activeTab});
			}

			return View(userFolder);
		}

		// GET: UserFolder/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			UserFolder userFolder = _userFolderService.Find(id);
			if (userFolder == null)
			{
				return HttpNotFound();
			}
			return View(userFolder);
		}

		// POST: UserFolder/Edit/5
		// 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
		// 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "Id, ObjectType, Name, CreateBy, CreateDate")] UserFolder userFolder)
		{
			if (ModelState.IsValid)
			{
                _userFolderService.Update(userFolder);

                int activeTab = (userFolder.ObjectType == ObjectType.UserPhoto) ? 2 : 4;
				return RedirectToAction("Index", "User", new {activeTab});
			}
			return View(userFolder);
		}

		// POST: UserFolder/Delete/5
		public JsonResult Delete(int id)
		{
			dynamic jsonResult;

			UserFolder userFolder = _userFolderService.Find(id);
			if (userFolder != null)
			{
				bool isUsed = _userFolderService.IsUsed(userFolder);
				if (isUsed)
				{
					jsonResult = new
					{
						success = false,
						message = "不能删除正在被使用的目录！"
					};
					return Json(jsonResult);
				}

				_userFolderService.Delete(userFolder);
			}

			jsonResult = new
			{
				success = true,
				message = "删除成功！"
			};
			return Json(jsonResult);
		}
	}
}