using System;
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
            TopicVideo topicVideo = _topicVideoService.DownloadCount(id);
            string url = WebHelper.Instance.RootUrl + topicVideo.Locations;
            var stream = new WebClient().OpenRead(url);
            string fileName = url.Substring(url.LastIndexOf(@"\"));
            return File(stream, "application/octet-stream", fileName);
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
