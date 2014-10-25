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
	public class GraphicController : Controller
	{
		private readonly GraphicService _graphicService = new GraphicService();
		private readonly CategoryService _categoryService = new CategoryService();

		[ChildActionOnly]
		public ActionResult IndexPartial()
		{
			return PartialView("_IndexPartial", _graphicService.GetTopImages(4));
		}

		//
		// GET: /Graphic/Detail
		public ActionResult Detail(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			Graphic graphic = _graphicService.View(id);
			if (graphic == null)
			{
				return HttpNotFound();
			}


			return View(graphic);
		}

		//
		// GET: /Graphic/Category
		public ActionResult Category(int? id)
		{
			ViewBag.Id = id;

			return View();
		}

		//
		// GET: /Graphic/List
		public ActionResult List(int? id, int? pageSize, int? page)
		{
			if (id == null)
			{
				id = 0;
			}

			ViewBag.Category = _categoryService.Get(id);
			IPagedList<ImageViewModel> images = _graphicService.GetImagesByCategory(id, pageSize, page);

			ViewBag.Id = id;

			return View(images);
		}

        //
        // GET: /Graphic/Search
        public ActionResult Search(string keyword, int? pageSize, int? page)
        {
            IPagedList<ImageViewModel> images = _graphicService.Search(keyword, pageSize, page);

            ViewBag.Keyword = keyword;

            return View(images);
        }

        //
        // GET: /Graphic/Download
        public FileResult Download(int? id)
        {
            Graphic graphic = _graphicService.Get(id);
            string url = WebHelper.Instance.RootUrl + graphic.PreviewPath;
            var stream = new WebClient().OpenRead(url);
            string fileName = url.Substring(url.LastIndexOf(@"\"));
            return File(stream, "image/jpeg", fileName);
        }

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_graphicService.Dispose();
				_categoryService.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
