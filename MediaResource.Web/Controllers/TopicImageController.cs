using System;
using System.Collections.Generic;
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
    public class TopicImageController : Controller
    {
        private readonly TopicImageService _topicImageService = new TopicImageService();
		private readonly VisitLogService _visitLogService = new VisitLogService();

		private const string UserDownloadTopicImagePath = @"mov1\Download\TopicImage\";

        // GET: TopicImage/Detail
        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TopicImage topicImage = _topicImageService.View(id);
            if (topicImage == null)
            {
                return HttpNotFound();
			}

			// 记录点击日志
			_visitLogService.LogClick(ObjectType.TopicImage, topicImage.Id, topicImage.TopicId);

			return View(topicImage);
        }

        // GET: TopicImage/PanelPartial
        [ChildActionOnly]
        public ActionResult PanelPartial(string keyword, int? page)
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

            // 高级搜索的查询条件
            var searchCondition = new Dictionary<string, string>();
            string name = Request["Name"];
            if (!String.IsNullOrEmpty(Request["Name"]))
            {
                ViewBag.Name = name;
                searchCondition.Add("Name", name);
            }
            string keyWords = Request["KeyWords"];
            if (!String.IsNullOrEmpty(Request["KeyWords"]))
            {
                ViewBag.KeyWords = keyWords;
                searchCondition.Add("KeyWords", keyWords);
            }
            string staff = Request["Staff"];
            if (!String.IsNullOrEmpty(Request["Staff"]))
            {
                ViewBag.Staff = staff;
                searchCondition.Add("Staff", staff);
            }
            string source = Request["Source"];
            if (!String.IsNullOrEmpty(Request["Source"]))
            {
                ViewBag.Source = source;
                searchCondition.Add("Source", source);
            }
            string summary = Request["Summary"];
            if (!String.IsNullOrEmpty(Request["Summary"]))
            {
                ViewBag.Summary = summary;
                searchCondition.Add("Summary", summary);
            }
            string startTextDate = Request["StartTextDate"];
            if (!String.IsNullOrEmpty(Request["StartTextDate"]))
            {
                ViewBag.StartTextDate = startTextDate;
                searchCondition.Add("StartTextDate", startTextDate);
            }
            string endTextDate = Request["EndTextDate"];
            if (!String.IsNullOrEmpty(Request["EndTextDate"]))
            {
                ViewBag.EndTextDate = endTextDate;
                searchCondition.Add("EndTextDate", endTextDate);
            }

            const int pageSize = 8;
            StaticPagedList<ImageViewModel> images = _topicImageService.AdvancedSearch(topicId, nodeId, userPlateId, keyword, pageSize, page, searchCondition);
            
            return PartialView("_PanelPartial", images);
        }

        // GET: TopicImage/List
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

            // 高级搜索的查询条件
            var searchCondition = new Dictionary<string, string>();
            string name = Request["Name"];
            if (!String.IsNullOrEmpty(Request["Name"]))
            {
                ViewBag.Name = name;
                searchCondition.Add("Name", name);
            }
            string keyWords = Request["KeyWords"];
            if (!String.IsNullOrEmpty(Request["KeyWords"]))
            {
                ViewBag.KeyWords = keyWords;
                searchCondition.Add("KeyWords", keyWords);
            }
            string staff = Request["Staff"];
            if (!String.IsNullOrEmpty(Request["Staff"]))
            {
                ViewBag.Staff = staff;
                searchCondition.Add("Staff", staff);
            }
            string source = Request["Source"];
            if (!String.IsNullOrEmpty(Request["Source"]))
            {
                ViewBag.Source = source;
                searchCondition.Add("Source", source);
            }
            string summary = Request["Summary"];
            if (!String.IsNullOrEmpty(Request["Summary"]))
            {
                ViewBag.Summary = summary;
                searchCondition.Add("Summary", summary);
            }
            string startTextDate = Request["StartTextDate"];
            if (!String.IsNullOrEmpty(Request["StartTextDate"]))
            {
                ViewBag.StartTextDate = startTextDate;
                searchCondition.Add("StartTextDate", startTextDate);
            }
            string endTextDate = Request["EndTextDate"];
            if (!String.IsNullOrEmpty(Request["EndTextDate"]))
            {
                ViewBag.EndTextDate = endTextDate;
                searchCondition.Add("EndTextDate", endTextDate);
            }

            StaticPagedList<ImageViewModel> images = _topicImageService.AdvancedSearch(topicId, nodeId, userPlateId, keyword, pageSize, page, searchCondition);

            return View(images);
        }

        // GET: TopicImage/Download
        public FileResult Download(int? id)
        {
            TopicImage topicImage = _topicImageService.DownloadCount(id);

			// 记录下载日志
			_visitLogService.LogDownload(ObjectType.TopicImage, topicImage.Id, topicImage.TopicId);

			string url = WebHelper.Instance.RootUrl + topicImage.Locations;
            var stream = new WebClient().OpenRead(url);
            string fileName = url.Substring(url.LastIndexOf(@"\"));
            return File(stream, "image/jpeg", fileName);
        }

        // POST: TopicImage/CompressAndDownload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public FileResult CompressAndDownload()
        {
            string folderPath = WebHelper.Instance.RootPath1 + UserDownloadTopicImagePath + WebHelper.Instance.CurrentUser.Name;
            string folderName = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            folderPath = Path.Combine(folderPath, folderName);
            if (!Directory.Exists(folderPath))
            {
                // 创建存放待压缩文件的目录
                Directory.CreateDirectory(folderPath);
            }

            // 将要下载的文件拷贝到创建的目录下
            string ids = Request["cbxTopicImage"];
            string[] arrayId = ids.Split(',');
            foreach (string id in arrayId)
            {
                TopicImage topicImage = _topicImageService.DownloadCount(int.Parse(id));
                string fileUrl = topicImage.Locations.TrimStart('\\', '/');

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

				// 记录下载日志
				_visitLogService.LogDownload(ObjectType.TopicImage, topicImage.Id, topicImage.TopicId);
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
                _topicImageService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
