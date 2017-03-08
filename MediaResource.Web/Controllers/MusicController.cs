using System.Net;
using System.Web.Mvc;

using MediaResource.Web.Models;
using MediaResource.Web.Services;

using PagedList;

namespace MediaResource.Web.Controllers
{
	[Authorize]
	public class MusicController : Controller
	{
		private readonly MusicService _musicService = new MusicService();
		private readonly CategoryService _categoryService = new CategoryService();
		private readonly VisitLogService _visitLogService = new VisitLogService();

		[ChildActionOnly]
		public ActionResult IndexPartial()
		{
			return PartialView("_IndexPartial", _musicService.GetTopList(9));
		}

		//
		// GET: /Music/Detail
		public ActionResult Detail(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			Music music = _musicService.View(id);
			if (music == null)
			{
				return HttpNotFound();
			}

			// 记录点击日志
			_visitLogService.LogClick(ObjectType.Music, music.Id);

			return View(music);
		}

		//
		// GET: /Music/Category
		public ActionResult Category(int? id)
		{
			ViewBag.Id = id;

			return View();
		}

		//
		// GET: /Music/List
		public ActionResult List(int? id, int? pageSize, int? page)
		{
			if (id == null)
			{
				id = 0;
			}

			ViewBag.Category = _categoryService.Get(id);
			IPagedList<Music> images = _musicService.GetByCategory(id, pageSize, page);

			ViewBag.Id = id;

			return View(images);
		}

        //
        // GET: /Music/Search
        public ActionResult Search(string keyword, int? pageSize, int? page)
        {
            IPagedList<Music> images = _musicService.Search(keyword, pageSize, page);

            ViewBag.Keyword = keyword;

            return View(images);
        }

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_musicService.Dispose();
				_categoryService.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
