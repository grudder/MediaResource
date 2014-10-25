using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

using MediaResource.Web.DataAccess;
using MediaResource.Web.Models;
using MediaResource.Web.Models.ViewModels;

namespace MediaResource.Web.Controllers
{
	[Authorize]
	public class AccountController : Controller
	{
		private readonly ApplicationDbContext _db = new ApplicationDbContext();

		//
		// GET: /Account/Login
		[AllowAnonymous]
		public ActionResult Login(string returnUrl)
		{
			ViewBag.ReturnUrl = returnUrl;
			return View();
		}

		//
		// POST: /Account/Login
		[HttpPost]
		[AllowAnonymous]
		public ActionResult Login(LoginViewModel model, string returnUrl)
		{
			if (ModelState.IsValid)
			{
				string name = model.Name;
				string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(model.Password, "SHA1");
				var users = from u in _db.Users
							where u.Name == name
							&& u.IsApproved
							&& !u.Locked
							select u;
				User user = users.SingleOrDefault();
				if (user != null && user.Password == hashedPassword)
				{
					Session["User"] = user;
					FormsAuthentication.SetAuthCookie(name, false);
					return RedirectToLocal(returnUrl);
				}
				ModelState.AddModelError("", "用户名或密码无效。");
			}

			// 如果我们进行到这一步时某个地方出错，则重新显示表单
			return View(model);
		}

		private ActionResult RedirectToLocal(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}

			return RedirectToAction("Index", "Home");
		}

		//
		// POST: /Account/LogOff
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult LogOff()
		{
			Session.Remove("User");
			FormsAuthentication.SignOut();
			return RedirectToAction("Index", "Home");
		}

		//
		// GET: /Account/Register
		[AllowAnonymous]
		public ActionResult Register()
		{
			var groups = from g in _db.Groups
						 where g.IsDisplay
						 select g;
			ViewBag.Groups = groups.ToList();

			return View();
		}

		//
		// POST: /Account/Register
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(model.Password, "SHA1");

				var user = new User()
				{
					Name = model.Name,
					Password = hashedPassword,
					GroupId = model.GroupId,
					FullName = model.FullName,
					Ext = model.Ext,
					Mobile = model.Mobile,
					Locked = false,
					CreateBy = 1,
					CreateDate = DateTime.Now,
					IsApproved = false,
					ApproveDate = DateTime.Now,
					Approver = 1,
					IsDisplay = false,
					IsLogin = false,
					OrderNum = 0,
					CanApproveTask = false
				};
				_db.Users.Add(user);
				_db.SaveChanges();
				return RedirectToAction("Index", "Home");
			}

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            var groups = from g in _db.Groups
                         where g.IsDisplay
                         select g;
            ViewBag.Groups = groups.ToList();

			return View(model);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_db.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
