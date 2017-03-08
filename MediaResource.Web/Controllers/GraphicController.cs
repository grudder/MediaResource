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
        private readonly GroupService _groupService = new GroupService();
		private readonly VisitLogService _visitLogService = new VisitLogService();

		[ChildActionOnly]
		public ActionResult IndexPartial()
		{
			return PartialView("_IndexPartial", _graphicService.GetTopImages(4));
		}

        // GET: Graphic/GroupPartial/5
        [ChildActionOnly]
        public ActionResult GroupPartial(int? groupId)
        {
            ViewBag.GroupId = groupId;

            return PartialView("_GroupPartial", _graphicService.GetTopImages(6, groupId));
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

			// 记录点击日志
			_visitLogService.LogClick(ObjectType.Graphic, graphic.Id);

			return View(graphic);
		}

		//
		// GET: /Graphic/Category
        public ActionResult Category(int? id, bool? advSearch)
		{
            ViewBag.Id = id;
            ViewBag.AdvSearch = advSearch;

			return View();
		}

		//
		// GET: /Graphic/List
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

            StaticPagedList<ImageViewModel> images = _graphicService.AdvancedSearch(ViewBag.NameOrKeyword, ViewBag.Person, ViewBag.StartTime, ViewBag.EndTime, ViewBag.GroupIds, id, pageSize, page);

            return View(images);
		}

        //
        // GET: /Graphic/Group/5
        public ActionResult Group(int? id, int? pageSize, int? page)
        {
            Group group = _groupService.Find(id);

            ViewBag.GroupId = id;
            ViewBag.Group = group;

            IPagedList<ImageViewModel> images = _graphicService.GetImagesByGroup(id, pageSize, page);

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
            Graphic graphic = _graphicService.DownloadCount(id);

			// 记录下载日志
			_visitLogService.LogDownload(ObjectType.Graphic, graphic.Id);

			string url = WebHelper.Instance.RootUrl + graphic.FileUrl;
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
