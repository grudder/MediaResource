using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using MediaResource.Web.DataAccess;
using MediaResource.Web.Models;
using PagedList;

namespace MediaResource.Web.Services
{
	public class TaskListService : IDisposable
	{
		private readonly ApplicationDbContext _db = new ApplicationDbContext();

		public TaskList Find(int? id)
		{
			return _db.TaskLists.Find(id);
		}

		public void Update(TaskList taskList)
		{
			_db.Entry(taskList).State = EntityState.Modified;
			_db.SaveChanges();
		}

		public void Delete(int id)
		{
			TaskList taskList = _db.TaskLists.Find(id);
			_db.TaskLists.Remove(taskList);
			_db.SaveChanges();
		}

		public List<TaskList> GetTopTaskLists(int count)
		{
			var taskLists = from taskList in _db.TaskLists
							where taskList.IsApprove == true
							orderby taskList.CreateDate descending
							select taskList;
			return taskLists.Take(count).ToList();
		}

		public IPagedList<TaskList> GetByUser(User user, int? pageSize, int? pageIndex)
		{
			var taskLists = from taskList in _db.TaskLists
							where taskList.CreateBy == user.Id
							orderby taskList.CreateDate descending
							select taskList;

			pageSize = (pageSize ?? 10);
			pageIndex = (pageIndex ?? 1);
			return taskLists.ToPagedList(pageIndex.Value, pageSize.Value);
		}

		public void Create(TaskList taskList)
		{
			taskList.TaskNo = GenerateTaskListNo();

			_db.TaskLists.Add(taskList);
			_db.SaveChanges();
		}

		/// <summary>
		/// 按年度生成任务单流水号。
		/// </summary>
		/// <returns>任务单流水号。</returns>
		private int GenerateTaskListNo()
		{
			int year = DateTime.Today.Year;
			var taskLists = from taskList in _db.TaskLists
							where taskList.TaskNo >= year * 10000
							&& taskList.TaskNo < (year + 1) * 10000
							orderby taskList.TaskNo descending
							select taskList;
			if (!taskLists.Any())
			{
				return year * 10000 + 1;
			}

			int? maxTaskNo = taskLists.Take(1).Single().TaskNo;
			if (maxTaskNo == null)
			{
				return year * 10000 + 1;
			}

			return maxTaskNo.Value + 1;
		}

		public IPagedList<TaskList> GetListToApprove(int? pageSize, int? page)
		{
			var taskLists = from taskList in _db.TaskLists
							orderby taskList.CreateDate descending
							select taskList;

			pageSize = (pageSize ?? 10);
			page = (page ?? 1);
			return taskLists.ToPagedList(page.Value, pageSize.Value);
		}

		#region IDisposable 成员

		public void Dispose()
		{
			_db.Dispose();
		}

		#endregion
	}
}