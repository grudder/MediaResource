using System;
using System.Web.Mvc;

using MediaResource.Web.Helper;
using MediaResource.Web.Models;
using MediaResource.Web.Services;

namespace MediaResource.Web.Controllers
{
	public class ApplyForDownloadController : Controller
	{
		private readonly ApplyForDownloadService _applyForDownloadService = new ApplyForDownloadService();

		// POST: ApplyForDownload/Create
		[HttpPost]
		public JsonResult Create(string reason, string detail)
		{
			var applyForDownload = new ApplyForDownload
			{
				Reason = reason,
				ApplyDetail = detail,
				CreateBy = WebHelper.Instance.CurrentUser.Id,
				CreateDate = DateTime.Now
			};
			_applyForDownloadService.Create(applyForDownload);

			return Json(new
			{
			});
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_applyForDownloadService.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}