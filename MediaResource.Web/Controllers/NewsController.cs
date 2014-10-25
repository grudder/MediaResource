using System.Net;
using System.Web.Mvc;

using MediaResource.Web.Models.ViewModels;
using MediaResource.Web.Services;

using PagedList;

namespace MediaResource.Web.Controllers
{
	[Authorize]
	public class NewsController : Controller
	{
		private readonly NewsService _newsService = new NewsService();
		private readonly CategoryService _categoryService = new CategoryService();

		[ChildActionOnly]
		public ActionResult IndexPartial()
		{
			return PartialView("_IndexPartial", _newsService.GetTopImages(4));
		}

		//
		// GET: /News/Detail
		public ActionResult Detail(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			NewsViewModel newsViewModel = _newsService.View(id);
			if (newsViewModel == null)
			{
				return HttpNotFound();
			}


			return View(newsViewModel);
		}

		//
		// GET: /News/Category
		public ActionResult Category(int? id)
		{
			ViewBag.Id = id;

			return View();
		}

		//
		// GET: /News/List
		public ActionResult List(int? id, int? pageSize, int? page)
		{
			if (id == null)
			{
				id = 0;
			}

			ViewBag.Category = _categoryService.Get(id);
			IPagedList<ImageViewModel> images = _newsService.GetImagesByCategory(id, pageSize, page);

			ViewBag.Id = id;

			return View(images);
		}

        //
        // GET: /News/Search
        public ActionResult Search(string keyword, int? pageSize, int? page)
        {
            IPagedList<ImageViewModel> images = _newsService.Search(keyword, pageSize, page);

            ViewBag.Keyword = keyword;

            return View(images);
        }

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_newsService.Dispose();
				_categoryService.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
