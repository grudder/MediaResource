using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Web.Mvc;

using MediaResource.Web.Helper;
using MediaResource.Web.Models;
using MediaResource.Web.Models.ViewModels;
using MediaResource.Web.Services;

using PagedList;

namespace MediaResource.Web.Controllers
{
    [Authorize]
    public class PhotoController : Controller
    {
        private readonly PhotoService _photoService = new PhotoService();
        private readonly CategoryService _categoryService = new CategoryService();
        private readonly GroupService _groupService = new GroupService();

        private const string UserUploadPhotoPath = @"mov1\Download\Photo\";

        [ChildActionOnly]
        public ActionResult IndexPartial()
        {
            return PartialView("_IndexPartial", _photoService.GetTopImages(5));
        }

        //
        // GET: /Photo/Detail
        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Photo photo = _photoService.View(id);
            if (photo == null)
            {
                return HttpNotFound();
            }


            return View(photo);
        }

        //
        // GET: /Photo/Category
        public ActionResult Category(int? id)
        {
            ViewBag.Id = id;

            return View();
        }

        //
        // GET: /Photo/List
        public ActionResult List(int? id, int? pageSize, int? page)
        {
            if (id == null)
            {
                id = 0;
            }

            ViewBag.Category = _categoryService.Get(id);
            ViewBag.Id = id;
            ViewBag.Groups = _groupService.GetTopVisibleList(-1);

            ViewBag.NameOrKeyword = Request["nameOrKeyword"];
            ViewBag.Person = Request["person"];
            ViewBag.StartTime = Request["startTime"];
            ViewBag.EndTime = Request["endTime"];
            ViewBag.GroupIds = Request["groupIds"];

            StaticPagedList<ImageViewModel> images = _photoService.AdvancedSearch(ViewBag.NameOrKeyword, ViewBag.Person, ViewBag.StartTime, ViewBag.EndTime, ViewBag.GroupIds, id, pageSize, page);

            return View(images);
        }

        //
        // GET: /Photo/Search
        public ActionResult Search(string keyword, int? pageSize, int? page)
        {
            IPagedList<ImageViewModel> images = _photoService.Search(keyword, pageSize, page);

            ViewBag.Keyword = keyword;

            return View(images);
        }

        //
        // GET: /Photo/Download
        public FileResult Download(int? id)
        {
            Photo photo = _photoService.Get(id);
            string url = WebHelper.Instance.RootUrl + photo.FileUrl;
            var stream = new WebClient().OpenRead(url);
            string fileName = url.Substring(url.LastIndexOf(@"\"));
            return File(stream, "image/jpeg", fileName);
        }

        // POST: Photo/CompressAndDownload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public FileResult CompressAndDownload()
        {
            string folderPath = WebHelper.Instance.RootPath1 + UserUploadPhotoPath + WebHelper.Instance.CurrentUser.Name;
            string folderName = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            folderPath = Path.Combine(folderPath, folderName);
            if (!Directory.Exists(folderPath))
            {
                // 创建存放待压缩文件的目录
                Directory.CreateDirectory(folderPath);
            }

            // 将要下载的文件拷贝到创建的目录下
            string ids = Request["cbxPhoto"];
            string[] arrayId = ids.Split(',');
            foreach (string id in arrayId)
            {
                Photo photo = _photoService.Get(int.Parse(id));
                string fileUrl = photo.FileUrl.TrimStart('\\', '/');

                // 不同目录的文件存放在不同的磁盘根路径
                string rootPath;
                if (fileUrl.StartsWith(@"mov\") || fileUrl.StartsWith(@"mov//"))
                {
                    rootPath = WebHelper.Instance.RootPath;
                }
                else
                {
                    rootPath = WebHelper.Instance.RootPath1;
                }

                string sourceFilePath = Path.Combine(rootPath, fileUrl);
                if (System.IO.File.Exists(sourceFilePath))
                {
                    string destFilePath = Path.Combine(folderPath, Path.GetFileName(sourceFilePath));
                    System.IO.File.Copy(sourceFilePath, destFilePath);
                }
            }

            // 生成压缩文件
            string zipFilePath = folderPath + ".zip";
            ZipFile.CreateFromDirectory(folderPath, zipFilePath);

            var zipFileContents = System.IO.File.ReadAllBytes(zipFilePath);

            // 删除临时文件目录
            Directory.Delete(folderPath, true);

            // 返回供下载的压缩文件
            string zipFileName = Path.GetFileName(zipFilePath);
            return File(zipFileContents, "application/x-zip-compressed", zipFileName);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _photoService.Dispose();
                _categoryService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
