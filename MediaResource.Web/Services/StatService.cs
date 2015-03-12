using System;
using System.Collections.Generic;
using System.Linq;
using MediaResource.Web.DataAccess;
using MediaResource.Web.Helper;
using MediaResource.Web.Models;
using MediaResource.Web.Models.ViewModels;

namespace MediaResource.Web.Services
{
    public class StatService : IDisposable
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public List<StatViewModel> GetTotalStats()
        {
            var photos = from p in _db.Photos
                         where p.Status == 1
                         && p.FileUrl != null
                         && p.FileUrl != ""
                         select p;
            var videos = from v in _db.Videos
                         where v.Status == 1
                         && v.PreviewPath != null
                         && v.PreviewPath != ""
                         select v;
            var films = from f in _db.Films
                        where f.Status == 1
                        && f.IsConverted == true
                        && f.ImagePath != null
                        && f.ImagePath != ""
                        select f;
            var graphicDesigns = from gd in _db.GraphicDesigns
                                 where gd.Status == 1
                                 && gd.PreviewPath != null
                                 && gd.PreviewPath != ""
                                 select gd;
            var newss = from n in _db.Newss
                        where n.Status == 1
                        && n.IsConverted == true
                        && n.ImagePath != null
                        && n.ImagePath != ""
                        select n;
            var graphics = from g in _db.Graphics
                           where g.Status == 1
                           && g.PreviewPath != null
                           && g.PreviewPath != ""
                           select g;
            var groups = from g in _db.Groups
                         where g.IsDisplay
                         select g;

            var svms = new List<StatViewModel>
			{
				new StatViewModel
				{
					Name = "照片素材",
					Count = photos.LongCount()
				},
				new StatViewModel
				{
					Name = "视频素材",
					Count = videos.LongCount()
				},
				new StatViewModel
				{
					Name = "影视成片",
					Count = films.LongCount()
				},
				new StatViewModel
				{
					Name = "设计成品",
					Count = graphicDesigns.LongCount()
				},
				new StatViewModel
				{
					Name = "电视新闻",
					Count = newss.LongCount()
				},
				new StatViewModel
				{
					Name = "纸媒选登",
					Count = graphics.LongCount()
				},
				new StatViewModel
				{
					Name = "处室专栏",
					Count = groups.LongCount()
				}
			};

            return svms;
        }

        public List<StatViewModel> GetMonthStats()
        {
            DateTime today = DateTime.Today;
            var monthStartDate = new DateTime(today.Year, today.Month, 1);

            var photos = from p in _db.Photos
                         where p.Status == 1
                         && p.FileUrl != null
                         && p.FileUrl != ""
                         && p.CreateDate != null
                         && p.CreateDate.Value >= monthStartDate
                         select p;
            var videos = from v in _db.Videos
                         where v.Status == 1
                         && v.PreviewPath != null
                         && v.PreviewPath != ""
                         && v.CreateDate != null
                         && v.CreateDate.Value >= monthStartDate
                         select v;
            var films = from f in _db.Films
                        where f.Status == 1
                        && f.IsConverted == true
                        && f.ImagePath != null
                        && f.ImagePath != ""
                        && f.CreateDate != null
                        && f.CreateDate.Value >= monthStartDate
                        select f;
            var graphicDesigns = from gd in _db.GraphicDesigns
                           where gd.Status == 1
                           && gd.PreviewPath != null
                           && gd.PreviewPath != ""
                           && gd.CreateDate != null
                           && gd.CreateDate.Value >= monthStartDate
                           select gd;
            var newss = from n in _db.Newss
                        where n.Status == 1
                        && n.IsConverted == true
                        && n.ImagePath != null
                        && n.ImagePath != ""
                        && n.CreateDate != null
                        && n.CreateDate.Value >= monthStartDate
                        select n;
            var graphics = from g in _db.Graphics
                           where g.Status == 1
                           && g.PreviewPath != null
                           && g.PreviewPath != ""
                           && g.CreateDate != null
                           && g.CreateDate.Value >= monthStartDate
                           select g;
            var groups = from g in _db.Groups
                         where g.IsDisplay
                         && g.CreateDate >= monthStartDate
                         select g;

            var svms = new List<StatViewModel>
			{
				new StatViewModel
				{
					Name = "照片素材",
					Count = photos.LongCount()
				},
				new StatViewModel
				{
					Name = "视频素材",
					Count = videos.LongCount()
				},
				new StatViewModel
				{
					Name = "影视成片",
					Count = films.LongCount()
				},
				new StatViewModel
				{
					Name = "设计成品",
					Count = graphicDesigns.LongCount()
				},
				new StatViewModel
				{
					Name = "电视新闻",
					Count = newss.LongCount()
				},
				new StatViewModel
				{
					Name = "纸媒选登",
					Count = graphics.LongCount()
				},
				new StatViewModel
				{
					Name = "处室专栏",
					Count = groups.LongCount()
				}
			};

            return svms;
        }

        public List<StatViewModel> GetYearStats()
        {
            DateTime today = DateTime.Today;
            var yearStartDate = new DateTime(today.Year, 1, 1);

            var photos = from p in _db.Photos
                         where p.Status == 1
                         && p.FileUrl != null
                         && p.FileUrl != ""
                         && p.CreateDate != null
                         && p.CreateDate.Value >= yearStartDate
                         select p;
            var videos = from v in _db.Videos
                         where v.Status == 1
                         && v.PreviewPath != null
                         && v.PreviewPath != ""
                         && v.CreateDate != null
                         && v.CreateDate.Value >= yearStartDate
                         select v;
            var films = from f in _db.Films
                        where f.Status == 1
                        && f.IsConverted == true
                        && f.ImagePath != null
                        && f.ImagePath != ""
                        && f.CreateDate != null
                        && f.CreateDate.Value >= yearStartDate
                        select f;
            var graphicDesigns = from gd in _db.GraphicDesigns
                           where gd.Status == 1
                           && gd.PreviewPath != null
                           && gd.PreviewPath != ""
                           && gd.CreateDate != null
                           && gd.CreateDate.Value >= yearStartDate
                           select gd;
            var newss = from n in _db.Newss
                        where n.Status == 1
                        && n.IsConverted == true
                        && n.ImagePath != null
                        && n.ImagePath != ""
                        && n.CreateDate != null
                        && n.CreateDate.Value >= yearStartDate
                        select n;
            var graphics = from g in _db.Graphics
                           where g.Status == 1
                           && g.PreviewPath != null
                           && g.PreviewPath != ""
                           && g.CreateDate != null
                           && g.CreateDate.Value >= yearStartDate
                           select g;
            var groups = from g in _db.Groups
                         where g.IsDisplay
                         && g.CreateDate >= yearStartDate
                         select g;

            var svms = new List<StatViewModel>
			{
				new StatViewModel
				{
					Name = "照片素材",
					Count = photos.LongCount()
				},
				new StatViewModel
				{
					Name = "视频素材",
					Count = videos.LongCount()
				},
				new StatViewModel
				{
					Name = "影视成片",
					Count = films.LongCount()
				},
				new StatViewModel
				{
					Name = "设计成品",
					Count = graphicDesigns.LongCount()
				},
				new StatViewModel
				{
					Name = "电视新闻",
					Count = newss.LongCount()
				},
				new StatViewModel
				{
					Name = "纸媒选登",
					Count = graphics.LongCount()
				},
				new StatViewModel
				{
					Name = "处室专栏",
					Count = groups.LongCount()
				}
			};

            return svms;
        }

        public List<StatViewModel> GetStats(int? groupId)
        {
            IQueryable<UserPhoto> userPhotos;
            IQueryable<Photo> photos;
            if (groupId == null)
            {
                userPhotos = from userPhoto in _db.UserPhotos
                             where userPhoto.IsPublic == true
                             select userPhoto;

                photos = from photo in _db.Photos
                         where photo.Status == 1
                         && photo.FileUrl != null
                         && photo.FileUrl != ""
                         select photo;
            }
            else
            {
                string groupName = _db.Groups.Find(groupId).Name;

                userPhotos = from userPhoto in _db.UserPhotos
                             where userPhoto.CreateByEntity.GroupId == groupId
                             && (userPhoto.CreateByEntity.GroupId == WebHelper.Instance.CurrentUser.GroupId
                             || userPhoto.IsPublic == true)
                             select userPhoto;

                photos = from photo in _db.Photos
                         where photo.Status == 1
                         && photo.FileUrl != null
                         && photo.FileUrl != ""
                         && (photo.CreateByEntity.GroupId == groupId
                         || photo.Keyword.Contains(groupName)
                         || photo.Offices.Contains(groupName)
                         || photo.Association.Contains(groupName))
                         select photo;
            }

            IQueryable<UserVideo> userVideos;
            IQueryable<Video> videos;
            if (groupId == null)
            {
                userVideos = from userVideo in _db.UserVideos
                             where userVideo.IsPublic == true
                             select userVideo;

                videos = from video in _db.Videos
                         where video.Status == 1
                         && video.PreviewPath != null
                         && video.PreviewPath != ""
                         select video;
            }
            else
            {
                string groupName = _db.Groups.Find(groupId).Name;

                userVideos = from userVideo in _db.UserVideos
                             where userVideo.CreateByEntity.GroupId == groupId
                                   && (userVideo.CreateByEntity.GroupId == WebHelper.Instance.CurrentUser.GroupId
                                       || userVideo.IsPublic == true)
                             select userVideo;

                videos = from video in _db.Videos
                         where video.Status == 1
                         && video.PreviewPath != null
                         && video.PreviewPath != ""
                         && (video.CreateByEntity.GroupId == groupId
                         || video.Keyword.Contains(groupName)
                         || video.Offices.Contains(groupName)
                         || video.Association.Contains(groupName))
                         select video;
            }

            var svms = new List<StatViewModel>
			{
				new StatViewModel
				{
					Name = "照片",
					Count = userPhotos.LongCount() + photos.LongCount()
				},
				new StatViewModel
				{
					Name = "视频",
					Count = userVideos.LongCount() + videos.LongCount()
				},
			};

            return svms;
        }

        #region IDisposable 成员

        public void Dispose()
        {
            _db.Dispose();
        }

        #endregion
    }
}