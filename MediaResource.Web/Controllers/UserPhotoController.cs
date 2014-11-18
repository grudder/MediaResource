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
    public class UserPhotoController : Controller
    {
        private readonly UserPhotoService _userPhotoService = new UserPhotoService();
        private readonly UserFolderService _userFolderService = new UserFolderService();
        private readonly GroupService _groupService = new GroupService();

        private const string UserUploadPhotoPath = @"mov\UserUpload\Photo\";
        private const string UserUploadPhotoUrl = @"mov/UserUpload/Photo/";

        // GET: UserPhoto/ListPartial
        [ChildActionOnly]
        public ActionResult ListPartial()
        {
            int userId = WebHelper.Instance.CurrentUser.Id;
            List<UserPhoto> userPhotos = _userPhotoService.GetList(userId);
            return PartialView("_ListPartial", userPhotos);
        }

        // GET: UserPhoto/GroupPartial/5
        [ChildActionOnly]
        public ActionResult GroupPartial(int? groupId)
        {
            ViewBag.GroupId = groupId;

            return PartialView("_GroupPartial", _userPhotoService.GetTopImages(6, groupId));
        }

        //
        // GET: /UserPhoto/Detail
        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            UserPhoto userPhoto = _userPhotoService.View(id);
            if (userPhoto == null)
            {
                return HttpNotFound();
            }


            return View(userPhoto);
        }

        //
        // GET: /UserPhoto/Group/5
        public ActionResult Group(int? id, int? pageSize, int? page)
        {
            Group group = _groupService.Find(id);

            ViewBag.GroupId = id;
            ViewBag.Group = group;

            IPagedList<ImageViewModel> images = _userPhotoService.GetImagesByGroup(id, pageSize, page);

            return View(images);
        }

        //
        // GET: /UserPhoto/Search
        public ActionResult Search(string keyword, int? pageSize, int? page)
        {
            ViewBag.Keyword = keyword;

            IPagedList<ImageViewModel> images = _userPhotoService.Search(keyword, pageSize, page);

            return View(images);
        }

        // GET: UserPhoto/Create
        public ActionResult Create()
        {
            // 获取用户的照片目录
            int userId = WebHelper.Instance.CurrentUser.Id;
            List<UserFolder> userPhotoFolders = _userFolderService.GetList(ObjectType.Photo, userId);

            var listItems = userPhotoFolders.Select(userPhotoFolder => new SelectListItem
            {
                Text = userPhotoFolder.Name,
                Value = userPhotoFolder.Id.ToString(CultureInfo.InvariantCulture)
            }).ToList();

            ViewBag.UserPhotoFolders = listItems;

            return View();
        }

        // POST: UserPhoto/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name, FolderId, IsPublic")] UserPhoto userPhoto, HttpPostedFileBase photoFile)
        {
            if (photoFile != null && photoFile.ContentLength > 0)
            {
                string folderPath = WebHelper.Instance.RootPath + UserUploadPhotoPath + WebHelper.Instance.CurrentUser.Name;
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string fileExtension = photoFile.FileName.Substring(photoFile.FileName.LastIndexOf('.'));
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + fileExtension;
                string filePath = Path.Combine(folderPath, fileName);
                photoFile.SaveAs(filePath);

                string fileUrl = UserUploadPhotoUrl + WebHelper.Instance.CurrentUser.Name + "/" + fileName;
                userPhoto.FileUrl = fileUrl;
            }

            userPhoto.CreateBy = WebHelper.Instance.CurrentUser.Id;
            userPhoto.CreateDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                _userPhotoService.Create(userPhoto);
                return RedirectToAction("Index", "User", new {activeTab = 3});
            }

            return View(userPhoto);
        }

        // GET: UserPhoto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserPhoto userPhoto = _userPhotoService.Find(id);
            if (userPhoto == null)
            {
                return HttpNotFound();
            }

            // 获取用户的照片目录
            int userId = WebHelper.Instance.CurrentUser.Id;
            List<UserFolder> userPhotoFolders = _userFolderService.GetList(ObjectType.Photo, userId);

            var listItems = userPhotoFolders.Select(userPhotoFolder => new SelectListItem
            {
                Text = userPhotoFolder.Name,
                Value = userPhotoFolder.Id.ToString(CultureInfo.InvariantCulture),
                Selected = (userPhotoFolder.Id == userPhoto.FolderId)
            }).ToList();

            ViewBag.UserPhotoFolders = listItems;
            ViewBag.PhotoUrl = WebHelper.Instance.RootUrl + userPhoto.FileUrl;

            return View(userPhoto);
        }

        // POST: UserPhoto/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id, Name, FileUrl, FolderId, IsPublic, CreateBy, CreateDate")] UserPhoto userPhoto, HttpPostedFileBase photoFile)
        {
            if (ModelState.IsValid)
            {
                bool fileChanged = false;
                if (photoFile != null && photoFile.ContentLength > 0)
                {
                    fileChanged = true;

                    // 删除原有文件
                    string oldFilePath = Path.Combine(WebHelper.Instance.RootPath, userPhoto.FileUrl);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }

                    // 保存新上传的文件
                    string folderPath = WebHelper.Instance.RootPath + UserUploadPhotoPath + WebHelper.Instance.CurrentUser.Name;
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    string fileExtension = photoFile.FileName.Substring(photoFile.FileName.LastIndexOf('.'));
                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + fileExtension;
                    string filePath = Path.Combine(folderPath, fileName);
                    photoFile.SaveAs(filePath);

                    string fileUrl = UserUploadPhotoUrl + WebHelper.Instance.CurrentUser.Name + "/" + fileName;
                    userPhoto.FileUrl = fileUrl;
                }

                _userPhotoService.Update(userPhoto, fileChanged);
                return RedirectToAction("Index", "User", new {activeTab = 3});
            }
            return View(userPhoto);
        }

        // POST: UserPhoto/Delete/5
        public JsonResult Delete(int id)
        {
            UserPhoto userPhoto = _userPhotoService.Find(id);
            if (userPhoto != null)
            {
                _userPhotoService.Delete(userPhoto);
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