using System;
using System.Web.Mvc;
using System.Web.Security;

using MediaResource.Web.Helper;
using MediaResource.Web.Models;
using MediaResource.Web.Services;

using PagedList;

namespace MediaResource.Web.Controllers
{
	public class UserController : Controller
	{
		private readonly UserService _userService = new UserService();

		// GET: User
		public ActionResult Index(int? activeTab)
		{
            ViewBag.ActiveTab = activeTab;

            return View();
		}

		// GET: User/Edit
		public ActionResult Edit()
		{
			int userId = WebHelper.Instance.CurrentUser.Id;
			User user = _userService.Find(userId);
			if (user == null)
			{
				return HttpNotFound();
			}

			user.Password = String.Empty;
			return View(user);
		}

		// POST: User/Edit
		// 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
		// 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "Id,Name,Password,GroupId,FullName,Ext,Mobile,Email,Remark,Locked,CreateBy,CreateDate,Ip,IsApproved,ApproveDate,Approver,Contact,IsDisplay,IsLogin,OrderNum,CanApproveTask")] User user)
		{
			if (ModelState.IsValid)
			{
				string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(user.Password, "SHA1");
				user.Password = hashedPassword;
				_userService.Update(user);

				Session["User"] = user;

				return RedirectToAction("Edit");
			}
			return View(user);
		}

		[ChildActionOnly]
		// GET: User/ContactListPartial
		public ActionResult ContactListPartial(int? pageSize, int? page)
		{
			// 分页
			IPagedList<User> pagedUsers = _userService.GetPagedList(pageSize, page);
			return PartialView("_ContactListPartial", pagedUsers);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_userService.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
