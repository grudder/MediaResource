using System;
using System.Net;
using System.Web.Mvc;

using MediaResource.Web.Helper;
using MediaResource.Web.Models;
using MediaResource.Web.Services;
using PagedList;

namespace MediaResource.Web.Controllers
{
	public class TaskListController : Controller
	{
		private readonly TaskListService _taskListService = new TaskListService();

		[ChildActionOnly]
		public ActionResult IndexPartial()
		{
			return PartialView("_IndexPartial", _taskListService.GetTopTaskLists(3));
		}

		// GET: TaskList
		public ActionResult Index(int? pageSize, int? page)
		{
			User user = WebHelper.Instance.CurrentUser;
			IPagedList<TaskList> taskLists = _taskListService.GetByUser(user, pageSize, page);
			return View(taskLists);
		}

		// GET: TaskList/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			TaskList taskList = _taskListService.Find(id);
			if (taskList == null)
			{
				return HttpNotFound();
			}
			return View(taskList);
		}

		// GET: TaskList/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: TaskList/Create
		// 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
		// 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "Name,TaskDate,Location,TaskListType,Category,Leader,Inoffices,Demand,Contact")] TaskList taskList)
		{
			taskList.CreateBy = WebHelper.Instance.CurrentUser.Id;
			taskList.CreateDate = DateTime.Now;

			if (ModelState.IsValid)
			{
				_taskListService.Create(taskList);
				return RedirectToAction("Index");
			}

			return View(taskList);
		}

		// GET: TaskList/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			TaskList taskList = _taskListService.Find(id);
			if (taskList == null)
			{
				return HttpNotFound();
			}
			return View(taskList);
		}

		// POST: TaskList/Edit/5
		// 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
		// 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "Id,Name,TaskDate,Location,TaskListType,Category,Leader,Inoffices,Demand,Contact,CreateBy,CreateDate,TaskNo,Sent,Feedback,IsApprove,IsConfirm")] TaskList taskList)
		{
			if (ModelState.IsValid)
			{
				_taskListService.Update(taskList);
				return RedirectToAction("Index");
			}
			return View(taskList);
		}

		// GET: TaskList/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			TaskList taskList = _taskListService.Find(id);
			if (taskList == null)
			{
				return HttpNotFound();
			}
			return View(taskList);
		}

		// POST: TaskList/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			_taskListService.Delete(id);
			return RedirectToAction("Index");
		}

		[ChildActionOnly]
		// GET: TaskList/Approve
		public ActionResult ApprovePartial(int? pageSize, int? page)
		{
			// 分页
			IPagedList<TaskList> taskLists = _taskListService.GetListToApprove(pageSize, page);
			return PartialView("_ApprovePartial", taskLists);
		}

		public JsonResult DoApprove(int id, bool ifApprove)
		{
			TaskList taskList = _taskListService.Find(id);
			taskList.IsApprove = ifApprove;
			_taskListService.Update(taskList);

			return Json(new
			{
				success = true,
				message = "审核成功！"
			});
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_taskListService.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
