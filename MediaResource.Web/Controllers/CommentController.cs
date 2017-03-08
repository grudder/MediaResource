using System;
using System.Web.Mvc;

using MediaResource.Web.Helper;
using MediaResource.Web.Models;
using MediaResource.Web.Services;
using PagedList;

namespace MediaResource.Web.Controllers
{
	public class CommentController : Controller
	{
		private readonly CommentService _commentService = new CommentService();
		private readonly VisitLogService _visitLogService = new VisitLogService();
		private readonly TopicImageService _topicImageService = new TopicImageService();
		private readonly TopicNewsService _topicNewsService = new TopicNewsService();
		private readonly TopicTextService _topicTextService = new TopicTextService();
		private readonly TopicVideoService _topicVideoService = new TopicVideoService();

		// GET: Comment
		public ActionResult Index(ObjectType objectType, int objectId, int pageSize, int? page)
		{
			var enumObjectType = objectType;
			IPagedList<Comment> pagedComments = _commentService.GetPagedList(enumObjectType, objectId, pageSize, page);
			return View(pagedComments);
		}

		// POST: Comment/Create
		[HttpPost]
		public JsonResult Create(ObjectType objectType, int objectId, string content)
		{
			var comment = new Comment
			{
				ObjectType = objectType,
				ObjectId = objectId,
				Content = content,
				CreateBy = WebHelper.Instance.CurrentUser.Id,
				CreateDate = DateTime.Now
			};
			_commentService.Create(comment);

			// 记录评论日志
			int? topicId;
			switch (objectType)
			{
				case ObjectType.Film:
				case ObjectType.Graphic:
				case ObjectType.Music:
				case ObjectType.News:
				case ObjectType.Photo:
				case ObjectType.Video:
				case ObjectType.GraphicDesign:
					topicId = null;
					break;

				case ObjectType.TopicText:
					topicId = _topicTextService.Get(objectId).TopicId;
					break;
				case ObjectType.TopicImage:
					topicId = _topicImageService.Get(objectId).TopicId;
					break;
				case ObjectType.TopicVideo:
					topicId = _topicVideoService.Get(objectId).TopicId;
					break;
				case ObjectType.TopicNews:
					topicId = _topicNewsService.Get(objectId).TopicId;
					break;

				default:
					topicId = null;
					break;
			}
			_visitLogService.LogComment(objectType, objectId, topicId);

			return Json(new
			{
				success = true,
				message = "评论成功！"
			});
		}

		// POST: Comment/Delete
		[HttpPost]
		public JsonResult Delete(int id)
		{
			Comment comment = _commentService.Find(id);
			_commentService.Delete(comment);

			return Json(new
			{
				success = true
			});
		}

		// GET: Comment/Count
		public JsonResult Count(ObjectType objectType, int objectId)
		{
			var jsonResult = new
			{
				count = _commentService.GetCount(objectType, objectId)
			};
			return Json(jsonResult, JsonRequestBehavior.AllowGet);
		}
	}
}