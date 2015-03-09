using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Messaging;

using MediaResource.Web.DataAccess;
using MediaResource.Web.Helper;
using MediaResource.Web.Models;
using MediaResource.Web.Models.ViewModels;

using PagedList;

namespace MediaResource.Web.Services
{
    public class UserVideoService : IDisposable
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public UserVideo Find(int? id)
        {
            return _db.UserVideos.Find(id);
        }

        public List<UserVideo> GetList(int userId)
        {
            var userVideos = from userVideo in _db.UserVideos
                             where userVideo.CreateBy == userId
                             orderby userVideo.CreateDate descending
                             select userVideo;

            return userVideos.ToList();
        }

        public UserVideo View(int? id)
        {
            UserVideo userVideo = _db.UserVideos.Find(id);
            ++userVideo.ClickCount;

            _db.Entry(userVideo).State = EntityState.Modified;
            _db.SaveChanges();

            return userVideo;
        }

        public List<ImageViewModel> GetTopImages(int count, int? groupId)
        {
            IQueryable<ImageViewModel> userVideos;
            IQueryable<ImageViewModel> videos;
            if (groupId == null)
            {
                userVideos = from userVideo in _db.UserVideos
                             where userVideo.IsPublic == true
                             orderby userVideo.CreateDate descending
                             select new ImageViewModel
                             {
                                 Id = userVideo.Id,
                                 Name = userVideo.Title,
                                 FileUrl = userVideo.ImagePath,
                                 CreateDate = userVideo.CreateDate,
                                 ObjectType = ObjectType.UserVideo
                             };

                videos = from video in _db.Videos
                         where video.Status == 1
                         && video.PreviewPath != null
                         && video.PreviewPath != ""
                         orderby video.CreateDate descending
                         select new ImageViewModel
                         {
                             Id = video.Id,
                             Name = video.Name,
                             FileUrl = video.PreviewPath,
                             CreateDate = video.CreateDate,
                             ObjectType = ObjectType.Video
                         };
            }
            else
            {
                string groupName = _db.Groups.Find(groupId).Name;

                userVideos = from userVideo in _db.UserVideos
                             where userVideo.CreateByEntity.GroupId == groupId
                             && (userVideo.CreateByEntity.GroupId == WebHelper.Instance.CurrentUser.GroupId
                             || userVideo.IsPublic == true)
                             orderby userVideo.CreateDate descending
                             select new ImageViewModel
                             {
                                 Id = userVideo.Id,
                                 Name = userVideo.Title,
                                 FileUrl = userVideo.ImagePath,
                                 CreateDate = userVideo.CreateDate,
                                 ObjectType = ObjectType.UserVideo
                             };

                videos = from video in _db.Videos
                         where video.Status == 1
                         && video.PreviewPath != null
                         && video.PreviewPath != ""
                         && (video.CreateByEntity.GroupId == groupId
                         || video.Offices.Contains(groupName)
                         || video.Association.Contains(groupName))
                         orderby video.CreateDate descending
                         select new ImageViewModel
                         {
                             Id = video.Id,
                             Name = video.Name,
                             FileUrl = video.PreviewPath,
                             CreateDate = video.CreateDate,
                             ObjectType = ObjectType.Video
                         };
            }

            // 获取截图地址
            List<ImageViewModel> groupVideos = userVideos.Union(videos).OrderByDescending(i => i.CreateDate).Take(count).ToList();
            foreach (ImageViewModel item in groupVideos)
            {
                if (item.ObjectType == ObjectType.UserVideo)
                {
                    item.FileUrl = ImageHelper.GetSnapUrl(item.FileUrl);
                }
                else if (item.ObjectType == ObjectType.Video)
                {
                    item.FileUrl = String.IsNullOrEmpty(item.FileUrl)
                        ? "#"
                        : WebHelper.Instance.RootUrl + item.FileUrl;
                }
            }

            return groupVideos;
        }

        public List<ImageViewModel> GetTopRankList(int count, int? groupId)
        {
            IQueryable<ImageViewModel> userVideos;
            IQueryable<ImageViewModel> videos;
            if (groupId == null)
            {
                userVideos = from userVideo in _db.UserVideos
                             where userVideo.IsPublic == true
                             orderby userVideo.Score descending
                             select new ImageViewModel
                             {
                                 Id = userVideo.Id,
                                 Name = userVideo.Title,
                                 FileUrl = userVideo.ImagePath,
                                 CreateDate = userVideo.CreateDate,
                                 Score = userVideo.Score,
                                 ObjectType = ObjectType.UserVideo
                             };

                videos = from video in _db.Videos
                         where video.CategoryEntity.CategoryType == ObjectType.Video
                         && video.Status == 1
                         && video.PreviewPath != null
                         && video.PreviewPath != ""
                         orderby video.Score descending
                         select new ImageViewModel
                         {
                             Id = video.Id,
                             Name = video.Name,
                             FileUrl = video.PreviewPath,
                             CreateDate = video.CreateDate,
                             Score = video.Score,
                             ObjectType = ObjectType.Video
                         };
            }
            else
            {
                string groupName = _db.Groups.Find(groupId).Name;

                userVideos = from userVideo in _db.UserVideos
                             where userVideo.CreateByEntity.GroupId == groupId
                             && (userVideo.CreateByEntity.GroupId == WebHelper.Instance.CurrentUser.GroupId
                             || userVideo.IsPublic == true)
                             orderby userVideo.Score descending
                             select new ImageViewModel
                             {
                                 Id = userVideo.Id,
                                 Name = userVideo.Title,
                                 FileUrl = userVideo.ImagePath,
                                 CreateDate = userVideo.CreateDate,
                                 Score = userVideo.Score,
                                 ObjectType = ObjectType.UserVideo
                             };

                videos = from video in _db.Videos
                         where video.CategoryEntity.CategoryType == ObjectType.Video
                         && video.Status == 1
                         && video.PreviewPath != null
                         && video.PreviewPath != ""
                         && (video.CreateByEntity.GroupId == groupId
                         || video.Offices.Contains(groupName)
                         || video.Association.Contains(groupName))
                         orderby video.Score descending
                         select new ImageViewModel
                         {
                             Id = video.Id,
                             Name = video.Name,
                             FileUrl = video.PreviewPath,
                             CreateDate = video.CreateDate,
                             Score = video.Score,
                             ObjectType = ObjectType.Video
                         };
            }

            List<ImageViewModel> groupVideos = userVideos.Union(videos).OrderByDescending(i => i.Score).Take(count).ToList();

            // 获取截图地址
            foreach (ImageViewModel item in groupVideos)
            {
                if (item.ObjectType == ObjectType.UserVideo)
                {
                    item.FileUrl = ImageHelper.GetSnapUrl(item.FileUrl);
                }
                else if (item.ObjectType == ObjectType.Video)
                {
                    item.FileUrl = String.IsNullOrEmpty(item.FileUrl)
                        ? "#"
                        : WebHelper.Instance.RootUrl + item.FileUrl;
                }
            }

            return groupVideos;
        }

        public IPagedList<ImageViewModel> GetImagesByGroup(int? groupId, int? pageSize, int? pageIndex)
        {
            // 执行查询
            IQueryable<ImageViewModel> userVideos;
            IQueryable<ImageViewModel> videos;
            if (groupId == null)
            {
                userVideos = from userVideo in _db.UserVideos
                             where userVideo.CreateByEntity.GroupId == WebHelper.Instance.CurrentUser.GroupId
                             || userVideo.IsPublic == true
                             orderby userVideo.CreateDate descending
                             select new ImageViewModel
                             {
                                 Id = userVideo.Id,
                                 Name = userVideo.Title,
                                 FileUrl = userVideo.ImagePath,
                                 CreateDate = userVideo.CreateDate,
                                 ObjectType = ObjectType.UserVideo
                             };

                videos = from video in _db.Videos
                         where video.Status == 1
                         && video.CategoryEntity.CategoryType == ObjectType.Video
                         && video.Status == 1
                         && video.PreviewPath != null
                         && video.PreviewPath != ""
                         orderby video.CreateDate descending
                         select new ImageViewModel
                         {
                             Id = video.Id,
                             Name = video.Name,
                             FileUrl = video.PreviewPath,
                             CreateDate = video.CreateDate,
                             ObjectType = ObjectType.Video
                         };
            }
            else
            {
                string groupName = _db.Groups.Find(groupId).Name;

                userVideos = from userVideo in _db.UserVideos
                             where userVideo.CreateByEntity.GroupId == groupId
                             && (userVideo.CreateByEntity.GroupId == WebHelper.Instance.CurrentUser.GroupId
                             || userVideo.IsPublic == true)
                             orderby userVideo.CreateDate descending
                             select new ImageViewModel
                             {
                                 Id = userVideo.Id,
                                 Name = userVideo.Title,
                                 FileUrl = userVideo.ImagePath,
                                 CreateDate = userVideo.CreateDate,
                                 ObjectType = ObjectType.UserVideo
                             };

                videos = from video in _db.Videos
                         where video.Status == 1
                         && video.CategoryEntity.CategoryType == ObjectType.Video
                         && video.Status == 1
                         && video.PreviewPath != null
                         && video.PreviewPath != ""
                         && (video.Offices.Contains(groupName)
                         || video.Association.Contains(groupName))
                         orderby video.CreateDate descending
                         select new ImageViewModel
                         {
                             Id = video.Id,
                             Name = video.Name,
                             FileUrl = video.PreviewPath,
                             CreateDate = video.CreateDate,
                             ObjectType = ObjectType.Video
                         };
            }

            // 分页处理
            pageSize = (pageSize ?? 20);
            pageIndex = (pageIndex ?? 1);
            IQueryable<ImageViewModel> groupVideos = userVideos.Union(videos).OrderByDescending(i => i.CreateDate);
            var pagedList = groupVideos.ToPagedList(pageIndex.Value, pageSize.Value);

            // 获取截图地址
            foreach (ImageViewModel item in pagedList)
            {
                if (item.ObjectType == ObjectType.UserVideo)
                {
                    item.FileUrl = ImageHelper.GetSnapUrl(item.FileUrl);
                }
                else if (item.ObjectType == ObjectType.Video)
                {
                    item.FileUrl = String.IsNullOrEmpty(item.FileUrl)
                        ? "#"
                        : WebHelper.Instance.RootUrl + item.FileUrl;
                }
            }

            return pagedList;
        }

        public IPagedList<ImageViewModel> Search(string keyword, int? pageSize, int? pageIndex)
        {
            // 执行查询
            var userVideos = from userVideo in _db.UserVideos
                             where userVideo.Title.Contains(keyword)
                             && (userVideo.CreateByEntity.GroupId == WebHelper.Instance.CurrentUser.GroupId
                             || userVideo.IsPublic == true)
                             orderby userVideo.CreateDate descending
                             select new ImageViewModel
                             {
                                 Id = userVideo.Id,
                                 Name = userVideo.Title,
                                 FileUrl = userVideo.ImagePath,
                                 CreateDate = userVideo.CreateDate
                             };

            // 分页处理
            pageSize = (pageSize ?? 20);
            pageIndex = (pageIndex ?? 1);
            var pagedList = userVideos.ToPagedList(pageIndex.Value, pageSize.Value);

            // 获取截图地址
            foreach (ImageViewModel item in pagedList)
            {
                item.FileUrl = ImageHelper.GetSnapUrl(item.FileUrl);
            }

            return pagedList;
        }

        public void Create(UserVideo userVideo)
        {
            _db.UserVideos.Add(userVideo);
            _db.SaveChanges();

            SendMessageQueue(userVideo.Id);
        }

        public void Update(UserVideo userVideo, bool fileChanged)
        {
            _db.Entry(userVideo).State = EntityState.Modified;
            _db.SaveChanges();

            if (fileChanged)
            {
                SendMessageQueue(userVideo.Id);
            }
        }

        private void SendMessageQueue(int id)
        {
            string path = WebHelper.Instance.MessageQueuePath;
            if (!MessageQueue.Exists(path))
            {
                MessageQueue.Create(path);
            }

            var mq = new MessageQueue(path);
            string message = "{\"Command\":\"Video\",\"StartId\":\"" + id + "\",\"EndId\":\"" + id + "\",\"CategoryType\":\"7\"}";
            mq.Send(message);
        }

        public void Delete(UserVideo userVideo)
        {
            // 删除文件
            string filePath = Path.Combine(WebHelper.Instance.RootPath, userVideo.FileUrl);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            // 删除数据
            _db.UserVideos.Remove(userVideo);
            _db.SaveChanges();
        }

        public void PostScore(int userVideoId, double scoreValue, int scoreCount)
        {
            UserVideo userVideo = _db.UserVideos.Find(userVideoId);
            userVideo.Score = scoreValue;
            userVideo.ScoreCount = scoreCount;
            _db.Entry(userVideo).State = EntityState.Modified;
            _db.SaveChanges();
        }

        #region IDisposable 成员

        public void Dispose()
        {
            _db.Dispose();
        }

        #endregion
    }
}