﻿using System;
using System.Collections.Generic;
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
		private readonly VisitLogService _visitLogService = new VisitLogService();

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

			// 记录点击日志
			_visitLogService.LogClick(ObjectType.TopicText, topicText.Id, topicText.TopicId);

			return View(topicText);
        }

        // GET: TopicText/PanelPartial
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
            StaticPagedList<TopicText> topicTexts = _topicTextService.AdvancedSearch(topicId, nodeId, userPlateId, keyword, pageSize, page, searchCondition);

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

            StaticPagedList<TopicText> topicTexts = _topicTextService.AdvancedSearch(topicId, nodeId, userPlateId, keyword, pageSize, page, searchCondition);

            return View(topicTexts);
        }

        // GET: TopicText/Download
        public FileResult Download(int? id)
        {
            TopicText topicText = _topicTextService.DownloadCount(id);

			// 记录下载日志
			_visitLogService.LogDownload(ObjectType.TopicText, topicText.Id, topicText.TopicId);

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
