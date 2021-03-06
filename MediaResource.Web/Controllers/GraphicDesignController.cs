﻿using System.Net;
using System.Web.Mvc;
using MediaResource.Web.Helper;
using MediaResource.Web.Models;
using MediaResource.Web.Models.ViewModels;
using MediaResource.Web.Services;

using PagedList;

namespace MediaResource.Web.Controllers
{
	[Authorize]
	public class GraphicDesignController : Controller
	{
		private readonly GraphicDesignService _graphicDesignService = new GraphicDesignService();
        private readonly CategoryService _categoryService = new CategoryService();
        private readonly GroupService _gruopService = new GroupService();
		private readonly VisitLogService _visitLogService = new VisitLogService();

		[ChildActionOnly]
		public ActionResult IndexPartial()
		{
			return PartialView("_IndexPartial", _graphicDesignService.GetTopImages(4));
		}

		//
		// GET: /GraphicDesign/Detail
		public ActionResult Detail(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			GraphicDesign graphicDesign = _graphicDesignService.View(id);
			if (graphicDesign == null)
			{
				return HttpNotFound();
			}

			// 记录点击日志
			_visitLogService.LogClick(ObjectType.GraphicDesign, graphicDesign.Id);

			return View(graphicDesign);
		}

		//
		// GET: /GraphicDesign/Category
        public ActionResult Category(int? id, bool? advSearch)
		{
            ViewBag.Id = id;
            ViewBag.AdvSearch = advSearch;

			return View();
		}

		//
		// GET: /GraphicDesign/List
        public ActionResult List(int? id, int? pageSize, int? page, bool? advSearch)
        {
            if (id == null)
            {
                id = 0;
            }

            ViewBag.Category = _categoryService.Get(id);
            ViewBag.Id = id;
            ViewBag.Groups = _gruopService.GetTopVisibleList(-1);

            ViewBag.NameOrKeyword = Request["nameOrKeyword"];
            ViewBag.Person = Request["person"];
            ViewBag.StartTime = Request["startTime"];
            ViewBag.EndTime = Request["endTime"];
            ViewBag.GroupIds = Request["groupIds"];

            // 高级查询
            ViewBag.AdvSearch = advSearch;
            if (advSearch == true)
            {
                return View();
            }

            StaticPagedList<ImageViewModel> images = _graphicDesignService.AdvancedSearch(ViewBag.NameOrKeyword, ViewBag.Person, ViewBag.StartTime, ViewBag.EndTime, ViewBag.GroupIds, id, pageSize, page);

            return View(images);
		}

        //
        // GET: /GraphicDesign/Search
        public ActionResult Search(string keyword, int? pageSize, int? page)
        {
            IPagedList<ImageViewModel> images = _graphicDesignService.Search(keyword, pageSize, page);

            ViewBag.Keyword = keyword;

            return View(images);
        }

        //
        // GET: /GraphicDesign/Download
        public FileResult Download(int? id)
        {
            GraphicDesign graphicDesign = _graphicDesignService.DownloadCount(id);

			// 记录下载日志
			_visitLogService.LogDownload(ObjectType.GraphicDesign, graphicDesign.Id);

			string url = WebHelper.Instance.RootUrl + graphicDesign.FileUrl;
            var stream = new WebClient().OpenRead(url);
            string fileName = url.Substring(url.LastIndexOf(@"\"));
            return File(stream, "image/jpeg", fileName);
        }

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_graphicDesignService.Dispose();
				_categoryService.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
