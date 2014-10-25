using System;
using System.Web.Mvc;

using MediaResource.Web.Helper;

namespace MediaResource.Web.Controllers
{
	public class VerificationCodeController : Controller
	{
		// GET: VerificationCode/Create
		public ActionResult Create()
		{
			string verificationCode = ImageHelper.GenerateVerificationCode();
			Session["VerificationCode"] = verificationCode;

			byte[] imageBytes = ImageHelper.CreateImageForString(verificationCode);
			return File(imageBytes, "image/jpg", "VerificationCode.jpg");
		}

		// GET: VerificationCode/Get
		[HttpPost]
		public JsonResult Verify(string verificationCode)
		{
			bool valid = string.Equals(verificationCode, (string)Session["VerificationCode"], StringComparison.CurrentCultureIgnoreCase);
			return Json(new
			{
				success = valid
			});
		}
	}
}
