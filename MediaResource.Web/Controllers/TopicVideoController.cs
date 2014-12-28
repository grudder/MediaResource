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
    public class TopicVideoController : Controller
    {
        private readonly TopicVideoService _topicVideoService = new TopicVideoService();

        private const string UserDownloadTopicVideoPath = @"mov1\Download\TopicVideo\";

        // GET: TopicVideo/Detail
        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TopicVideoViewModel topicVideoViewModel = _topicVideoService.View(id);
            if (topicVideoViewModel == null)
            {
                return HttpNotFound();
            }

            return View(topicVideoViewModel);
        }

        // GET: TopicVideo/PanelPartial
        [ChildActionOnly]
        public ActionResult PanelPartial(string keyword, int? pageSize, int? page)
        {
            int? topicId = null;
            if (!String.IsNullOrEmpty(Request["TopicId"]))
            {
                ViewBag.TopicId = topicId = int.Parse(Request["TopicId"]);
            }
            int? nodeId = null;
            if (!String.IsNullOrEmpty(Request["NodeId"]))
            {
                ViewBag.NodeId = nodeId = int.Parse(Request["NodeId"]);
            }
            int? userPlateId = null;
            if (!String.IsNullOrEmpty(Request["UserPlateId"]))
            {
                ViewBag.UserPlateId = userPlateId = int.Parse(Request["UserPlateId"]);
            }
            ViewBag.Keyword = keyword;

            StaticPagedList<ImageViewModel> images = _topicVideoService.AdvancedSearch(topicId, nodeId, userPlateId, keyword, pageSize, page);

            return PartialView("_PanelPartial", images);
        }

        // GET: TopicVideo/List
        public ActionResult List(string keyword, int? pageSize, int? page)
        {
            int? topicId = null;
            if (!String.IsNullOrEmpty(Request["TopicId"]))
            {
                ViewBag.TopicId = topicId = int.Parse(Request["TopicId"]);
            }
            int? nodeId = null;
            if (!String.IsNullOrEmpty(Request["NodeId"]))
            {
                ViewBag.NodeId = nodeId = int.Parse(Request["NodeId"]);
            }
            int? userPlateId = null;
            if (!String.IsNullOrEmpty(Request["UserPlateId"]))
            {
                ViewBag.UserPlateId = userPlateId = int.Parse(Request["UserPlateId"]);
            }
            ViewBag.Keyword = keyword;

            StaticPagedList<ImageViewModel> images = _topicVideoService.AdvancedSearch(topicId, nodeId, userPlateId, keyword, pageSize, page);

            return View(images);
        }

        // GET: TopicVideo/Download
        public FileResult Download(int? id)
        {
            TopicVideo topicVideo = _topicVideoService.Get(id);
            string url = WebHelper.Instance.RootUrl + topicVideo.Locations;
            var stream = new WebClient().OpenRead(url);
            string fileName = url.Substring(url.LastIndexOf(@"\"));
            return File(stream, "application/octet-stream", fileName);
        }

        // POST: TopicVideo/CompressAndDownload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public FileResult CompressAndDownload()
        {
            string folderPath = WebHelper.Instance.RootPath1 + UserDownloadTopicVideoPath + WebHelper.Instance.CurrentUser.Name;
            string folderName = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            folderPath = Path.Combine(folderPath, folderName);
            if (!Directory.Exists(folderPath))
            {
                // 创建存放待压缩文件的目录
                Directory.CreateDirectory(folderPath);
            }

            // 将要下载的文件拷贝到创建的目录下
            string ids = Request["cbxTopicVideo"];
            string[] arrayId = ids.Split(',');
            foreach (string id in arrayId)
            {
                TopicVideo topicVideo = _topicVideoService.Get(int.Parse(id));
                string fileUrl = topicVideo.Locations.TrimStart('\\', '/');

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
                _topicVideoService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
