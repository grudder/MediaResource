﻿using System.Net;
using System.Web.Mvc;

using MediaResource.Web.Models.ViewModels;
using MediaResource.Web.Services;

using PagedList;

namespace MediaResource.Web.Controllers
{
	[Authorize]
	public class VideoController : Controller
	{
		private readonly VideoService _videoService = new VideoService();
		private readonly CategoryService _categoryService = new CategoryService();

		[ChildActionOnly]
		public ActionResult IndexPartial()
		{
			return PartialView("_IndexPartial", _videoService.GetTopImages(5));
		}

		//
		// GET: /Video/Detail
		public ActionResult Detail(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			VideoViewModel videoViewModel = _videoService.View(id);
			if (videoViewModel == null)
			{
				return HttpNotFound();
			}


			return View(videoViewModel);
		}

		//
		// GET: /Video/Category
		public ActionResult Category(int? id)
		{
			ViewBag.Id = id;

			return View();
		}

		//
		// GET: /Video/List
		public ActionResult List(int? id, int? pageSize, int? page)
		{
			if (id == null)
			{
				id = 0;
			}

			ViewBag.Category = _categoryService.Get(id);
			IPagedList<ImageViewModel> images = _videoService.GetImagesByCategory(id, pageSize, page);

			ViewBag.Id = id;

			return View(images);
		}

        //
        // GET: /Video/Search
        public ActionResult Search(string keyword, int? pageSize, int? page)
        {
            IPagedList<ImageViewModel> images = _videoService.Search(keyword, pageSize, page);

            ViewBag.Keyword = keyword;

            return View(images);
        }

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_videoService.Dispose();
				_categoryService.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
