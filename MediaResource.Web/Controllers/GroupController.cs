using System;
using System.Collections.Generic;
using System.Web.Mvc;

using MediaResource.Web.Models;
using MediaResource.Web.Models.ViewModels;
using MediaResource.Web.Services;

namespace MediaResource.Web.Controllers
{
	[Authorize]
	public class GroupController : Controller
	{
		private readonly GroupService _groupService = new GroupService();
		private readonly StatService _statService = new StatService();
		private readonly UserPhotoService _userPhotoService = new UserPhotoService();
		private readonly UserVideoService _userVideoService = new UserVideoService();

		//
		// GET: /Group/Index/5
		public ActionResult Index(int? id)
		{
			Group group = _groupService.Find(id);

			return View(group);
		}

		[ChildActionOnly]
		public ActionResult IndexPartial(string actionUrl)
		{
			ViewBag.ActionUrl = actionUrl;

		    int maxCount = (ViewBag.ActionUrl == Url.Action("Index", "Home")) ? 23 : -1;
            return PartialView("_IndexPartial", _groupService.GetTopVisibleList(maxCount));
		}

		[ChildActionOnly]
		public ActionResult StatPartial(int? groupId)
		{
			return PartialView("_StatPartial", _statService.GetStats(groupId));
		}

		[ChildActionOnly]
		public ActionResult RankPartial(int? groupId)
		{
            var models = new Tuple<List<ImageViewModel>, List<ImageViewModel>>(_userPhotoService.GetTopRankList(8, groupId), _userVideoService.GetTopRankList(12, groupId));
			return PartialView("_RankPartial", models);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_groupService.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
