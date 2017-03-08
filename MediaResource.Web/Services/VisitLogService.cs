using System;
using System.Web;

using MediaResource.Web.DataAccess;
using MediaResource.Web.Helper;
using MediaResource.Web.Models;

namespace MediaResource.Web.Services
{
	public class VisitLogService : IDisposable
	{
		private readonly ApplicationDbContext _db = new ApplicationDbContext();

		#region IDisposable 成员

		public void Dispose()
		{
			_db.Dispose();
		}

		#endregion

		/// <summary>
		/// 记录点击日志
		/// </summary>
		/// <param name="materialType">素材类型</param>
		/// <param name="materialId">素材主键</param>
		/// <param name="topicId">所属专题主键</param>
		public void LogClick(ObjectType materialType, int materialId, int? topicId = null)
		{
			Create(materialType, materialId, VisitType.Click, topicId);
		}

		/// <summary>
		/// 记录下载日志
		/// </summary>
		/// <param name="materialType">素材类型</param>
		/// <param name="materialId">素材主键</param>
		/// <param name="topicId">所属专题主键</param>
		public void LogDownload(ObjectType materialType, int materialId, int? topicId = null)
		{
			Create(materialType, materialId, VisitType.Download, topicId);
		}

		/// <summary>
		/// 记录评分日志
		/// </summary>
		/// <param name="materialType">素材类型</param>
		/// <param name="materialId">素材主键</param>
		/// <param name="topicId">所属专题主键</param>
		public void LogScore(ObjectType materialType, int materialId, int? topicId = null)
		{
			Create(materialType, materialId, VisitType.Score, topicId);
		}

		/// <summary>
		/// 记录评论日志
		/// </summary>
		/// <param name="materialType">素材类型</param>
		/// <param name="materialId">素材主键</param>
		/// <param name="topicId">所属专题主键</param>
		public void LogComment(ObjectType materialType, int materialId, int? topicId = null)
		{
			Create(materialType, materialId, VisitType.Comment, topicId);
		}

		private void Create(ObjectType materialType, int materialId, VisitType visitType, int? topicId = null)
		{
			User currentUser = WebHelper.Instance.CurrentUser;
			var visitLog = new VisitLog
			{
				TopicId = topicId,
				MaterialType = materialType,
				MaterialId = materialId,
				VisitType = visitType,
				VisitedBy = currentUser.Id,
				VisitorGroupId = currentUser.GroupId,
				VisitorIp = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"],
				VisitTime = DateTime.Now
			};

			_db.VisitLogs.Add(visitLog);
			_db.SaveChanges();
		}
	}
}