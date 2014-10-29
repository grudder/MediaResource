using System.Net;
using System.Web.Mvc;

using MediaResource.Web.Models.ViewModels;
using MediaResource.Web.Services;
using PagedList;

namespace MediaResource.Web.Controllers
{
	[Authorize]
	public class FilmController : Controller
	{
		private readonly FilmService _filmService = new FilmService();
        private readonly CategoryService _categoryService = new CategoryService();
        private readonly GroupService _groupService = new GroupService();

		[ChildActionOnly]
		public ActionResult IndexPartial()
		{
			return PartialView("_IndexPartial", _filmService.GetTopImages(4));
		}

		//
		// GET: /Film/Detail
		public ActionResult Detail(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			FilmViewModel filmViewModel = _filmService.View(id);
			if (filmViewModel == null)
			{
				return HttpNotFound();
			}


			return View(filmViewModel);
		}

		//
		// GET: /Film/Category
		public ActionResult Category(int? id)
		{
			ViewBag.Id = id;

			return View();
		}

		//
		// GET: /Film/List
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

            StaticPagedList<ImageViewModel> images = _filmService.AdvancedSearch(ViewBag.NameOrKeyword, ViewBag.Person, ViewBag.StartTime, ViewBag.EndTime, ViewBag.GroupIds, id, pageSize, page);

            return View(images);
		}

        //
        // GET: /Film/Search
        public ActionResult Search(string keyword, int? pageSize, int? page)
        {
            IPagedList<ImageViewModel> images = _filmService.Search(keyword, pageSize, page);

            ViewBag.Keyword = keyword;

            return View(images);
        }

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_filmService.Dispose();
				_categoryService.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
