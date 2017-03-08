using System.Net;
using System.Web.Mvc;

using MediaResource.Web.Models;
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
        private readonly GroupService _gruopService = new GroupService();
		private readonly VisitLogService _visitLogService = new VisitLogService();

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

			// 记录点击日志
			_visitLogService.LogClick(ObjectType.Video, videoViewModel.Video.Id);

			return View(videoViewModel);
		}

		//
		// GET: /Video/Category
        public ActionResult Category(int? id, bool? advSearch)
		{
            ViewBag.Id = id;
            ViewBag.AdvSearch = advSearch;

			return View();
		}

		//
		// GET: /Video/List
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

            StaticPagedList<ImageViewModel> images = _videoService.AdvancedSearch(ViewBag.NameOrKeyword, ViewBag.Person, ViewBag.StartTime, ViewBag.EndTime, ViewBag.GroupIds, id, pageSize, page);

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
