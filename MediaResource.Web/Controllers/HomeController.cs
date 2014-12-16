using System;
using System.Collections.Generic;
using System.Web.Mvc;

using MediaResource.Web.Models;
using MediaResource.Web.Models.ViewModels;
using MediaResource.Web.Services;

namespace MediaResource.Web.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		private readonly PhotoService _photoService = new PhotoService();
		private readonly VideoService _videoService = new VideoService();
		private readonly StatService _statService = new StatService();

		//
		// GET: /Home/Index
		public ActionResult Index()
		{
			return View();
		}

        //
        // GET: /Home/ContactUs
        public ActionResult ContactUs()
        {
            return View();
        }

		[ChildActionOnly]
		public ActionResult StatPartial()
        {
            var models = new Tuple<List<StatViewModel>, List<StatViewModel>, List<StatViewModel>>(_statService.GetTotalStats(), _statService.GetMonthStats(), _statService.GetYearStats());
            return PartialView("_StatPartial", models);
		}

		[ChildActionOnly]
		public ActionResult RankPartial()
		{
			var models = new Tuple<List<Photo>, List<Video>>(_photoService.GetTopRankList(5), _videoService.GetTopRankList(5));
			return PartialView("_RankPartial", models);
		}
	}
}
