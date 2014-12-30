using System;
using System.Net;
using System.Web.Mvc;

using MediaResource.Web.Helper;
using MediaResource.Web.Models;
using MediaResource.Web.Services;

using PagedList;

namespace MediaResource.Web.Controllers
{
    [Authorize]
    public class TopicNewsController : Controller
    {
        private readonly TopicNewsService _topicNewsService = new TopicNewsService();

        // GET: TopicNews/Detail
        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TopicNews topicNews = _topicNewsService.View(id);
            if (topicNews == null)
            {
                return HttpNotFound();
            }

            return View(topicNews);
        }

        // GET: TopicNews/PanelPartial
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

            StaticPagedList<TopicNews> topicNewss = _topicNewsService.AdvancedSearch(topicId, nodeId, userPlateId, keyword, pageSize, page);

            return PartialView("_PanelPartial", topicNewss);
        }

        // GET: TopicNews/List
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

            StaticPagedList<TopicNews> topicNewss = _topicNewsService.AdvancedSearch(topicId, nodeId, userPlateId, keyword, pageSize, page);

            return View(topicNewss);
        }

        // GET: TopicNews/Download
        public FileResult Download(int? id)
        {
            TopicNews topicNews = _topicNewsService.DownloadCount(id);
            string url = WebHelper.Instance.RootUrl + topicNews.Locations;
            var stream = new WebClient().OpenRead(url);
            string fileName = url.Substring(url.LastIndexOf(@"\"));
            return File(stream, "image/jpeg", fileName);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _topicNewsService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
