﻿using System;
using System.Net;
using System.Web.Mvc;

using MediaResource.Web.Helper;
using MediaResource.Web.Models;
using MediaResource.Web.Services;

using PagedList;

namespace MediaResource.Web.Controllers
{
    [Authorize]
    public class TopicTextController : Controller
    {
        private readonly TopicTextService _topicTextService = new TopicTextService();

        // GET: TopicText/Detail
        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TopicText topicText = _topicTextService.View(id);
            if (topicText == null)
            {
                return HttpNotFound();
            }

            return View(topicText);
        }

        // GET: TopicText/PanelPartial
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

            StaticPagedList<TopicText> topicTexts = _topicTextService.AdvancedSearch(topicId, nodeId, userPlateId, keyword, pageSize, page);

            return PartialView("_PanelPartial", topicTexts);
        }

        // GET: TopicText/List
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

            StaticPagedList<TopicText> topicTexts = _topicTextService.AdvancedSearch(topicId, nodeId, userPlateId, keyword, pageSize, page);

            return View(topicTexts);
        }

        // GET: TopicText/Download
        public FileResult Download(int? id)
        {
            TopicText topicText = _topicTextService.DownloadCount(id);
            string url = WebHelper.Instance.RootUrl + topicText.Locations;
            var stream = new WebClient().OpenRead(url);
            string fileName = url.Substring(url.LastIndexOf(@"\"));
            return File(stream, "image/jpeg", fileName);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _topicTextService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
