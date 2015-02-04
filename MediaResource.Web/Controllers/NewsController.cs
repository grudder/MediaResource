using System.Net;
using System.Web.Mvc;
using MediaResource.Web.Models;
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
        private readonly GroupService _groupService = new GroupService();

		[ChildActionOnly]
		public ActionResult IndexPartial()
		{
			return PartialView("_IndexPartial", _newsService.GetTopImages(4));
		}

        // GET: News/GroupPartial/5
        [ChildActionOnly]
        public ActionResult GroupPartial(int? groupId)
        {
            ViewBag.GroupId = groupId;

            return PartialView("_GroupPartial", _newsService.GetTopImages(6, groupId));
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
        public ActionResult Category(int? id, bool? advSearch)
		{
            ViewBag.Id = id;
            ViewBag.AdvSearch = advSearch;

			return View();
		}

		//
		// GET: /News/List
        public ActionResult List(int? id, int? pageSize, int? page, bool? advSearch)
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

            // 高级查询
            ViewBag.AdvSearch = advSearch;
            if (advSearch == true)
            {
                return View();
            }

            StaticPagedList<ImageViewModel> images = _newsService.AdvancedSearch(ViewBag.NameOrKeyword, ViewBag.Person, ViewBag.StartTime, ViewBag.EndTime, ViewBag.GroupIds, id, pageSize, page);

            return View(images);
		}

        //
        // GET: /Graphic/Group/5
        public ActionResult Group(int? id, int? pageSize, int? page)
        {
            Group group = _groupService.Find(id);

            ViewBag.GroupId = id;
            ViewBag.Group = group;

            IPagedList<ImageViewModel> images = _newsService.GetImagesByGroup(id, pageSize, page);

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
