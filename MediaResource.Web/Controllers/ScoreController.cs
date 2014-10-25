using System;
using System.Web.Mvc;

using MediaResource.Web.Helper;
using MediaResource.Web.Models;
using MediaResource.Web.Services;

namespace MediaResource.Web.Controllers
{
	public class ScoreController : Controller
	{
		private readonly ScoreService _scoreService = new ScoreService();
		private readonly VideoService _videoService = new VideoService();
		private readonly PhotoService _photoService = new PhotoService();
		private readonly NewsService _newsService = new NewsService();
		private readonly MusicService _musicService = new MusicService();
		private readonly GraphicService _graphicService = new GraphicService();
		private readonly FilmService _filmService = new FilmService();
        private readonly UserPhotoService _userPhotoService = new UserPhotoService();
        private readonly GraphicDesignService _graphicDesignService = new GraphicDesignService();

		// POST: Score/Create
		[HttpPost]
		public JsonResult Create(ObjectType objectType, int objectId, int value)
		{
			if (_scoreService.ScoreExists(objectType, objectId, WebHelper.Instance.CurrentUser))
			{
				return Json(new
				{
					success = false,
					message = "每个用户只能评分一次！"
				});
			}

			var score = new Score
			{
				ObjectType = objectType,
				ObjectId = objectId,
				Value = value,
				CreateBy = WebHelper.Instance.CurrentUser.Id,
				CreateDate = DateTime.Now
			};
			_scoreService.Create(score);

			double scoreValue = _scoreService.GetAverageValue(objectType, objectId);
			int scoreCount = _scoreService.GetCount(objectType, objectId);
			switch (objectType)
			{
				case ObjectType.Video:
					_videoService.PostScore(objectId, scoreValue, scoreCount);
					break;

				case ObjectType.Photo:
					_photoService.PostScore(objectId, scoreValue, scoreCount);
					break;

				case ObjectType.News:
					_newsService.PostScore(objectId, scoreValue, scoreCount);
					break;

				case ObjectType.Music:
					_musicService.PostScore(objectId, scoreValue, scoreCount);
					break;

				case ObjectType.Graphic:
					_graphicService.PostScore(objectId, scoreValue, scoreCount);
					break;

				case ObjectType.Film:
					_filmService.PostScore(objectId, scoreValue, scoreCount);
					break;

				case ObjectType.UserPhoto:
					_userPhotoService.PostScore(objectId, scoreValue, scoreCount);
                    break;

                case ObjectType.GraphicDesign:
                    _graphicDesignService.PostScore(objectId, scoreValue, scoreCount);
                    break;
			}

			return Json(new
			{
				success = true,
				message = "评分成功！",
				Score = scoreValue,
				ScoreCount = scoreCount
			});
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_scoreService.Dispose();
				_videoService.Dispose();
				_photoService.Dispose();
				_newsService.Dispose();
				_musicService.Dispose();
                _graphicService.Dispose();
                _filmService.Dispose();
                _graphicDesignService.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}