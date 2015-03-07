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
    public class UserPhotoService : IDisposable
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public UserPhoto Find(int? id)
        {
            return _db.UserPhotos.Find(id);
        }

        public List<UserPhoto> GetList(int userId)
        {
            var userPhotos = from userPhoto in _db.UserPhotos
                             where userPhoto.CreateBy == userId
                             orderby userPhoto.CreateDate descending
                             select userPhoto;

            return userPhotos.ToList();
        }

        public UserPhoto View(int? id)
        {
            UserPhoto userPhoto = _db.UserPhotos.Find(id);
            ++userPhoto.ClickCount;

            _db.Entry(userPhoto).State = EntityState.Modified;
            _db.SaveChanges();

            return userPhoto;
        }

        public List<ImageViewModel> GetTopImages(int count, int? groupId)
        {
            IQueryable<ImageViewModel> userPhotos;
            IQueryable<ImageViewModel> photos;
            if (groupId == null)
            {
                userPhotos = from userPhoto in _db.UserPhotos
                             where userPhoto.IsPublic == true
                             orderby userPhoto.CreateDate descending
                             select new ImageViewModel
                             {
                                 Id = userPhoto.Id,
                                 Name = userPhoto.Name,
                                 RawUrl = WebHelper.Instance.RootUrl + userPhoto.FileUrl,
                                 FileUrl = userPhoto.FileUrl,
                                 CreateDate = userPhoto.CreateDate,
                                 ObjectType = ObjectType.UserPhoto
                             };

                photos = from photo in _db.Photos
                         where photo.CategoryEntity.CategoryType == ObjectType.Photo
                         && photo.Status == 1
                         && photo.FileUrl != null
                         && photo.FileUrl != ""
                         orderby photo.CreateDate descending
                         select new ImageViewModel
                         {
                             Id = photo.Id,
                             Name = photo.Name,
                             RawUrl = WebHelper.Instance.RootUrl + photo.FileUrl,
                             FileUrl = photo.FileUrl,
                             CreateDate = photo.CreateDate,
                             ObjectType = ObjectType.Photo
                         };
            }
            else
            {
                string groupName = _db.Groups.Find(groupId).Name;

                userPhotos = from userPhoto in _db.UserPhotos
                             where userPhoto.CreateByEntity.GroupId == groupId
                             && (userPhoto.CreateByEntity.GroupId == WebHelper.Instance.CurrentUser.GroupId
                             || userPhoto.IsPublic == true)
                             orderby userPhoto.CreateDate descending
                             select new ImageViewModel
                             {
                                 Id = userPhoto.Id,
                                 Name = userPhoto.Name,
                                 RawUrl = WebHelper.Instance.RootUrl + userPhoto.FileUrl,
                                 FileUrl = userPhoto.FileUrl,
                                 CreateDate = userPhoto.CreateDate,
                                 ObjectType = ObjectType.UserPhoto
                             };

                photos = from photo in _db.Photos
                         where photo.CategoryEntity.CategoryType == ObjectType.Photo
                         && photo.Status == 1
                         && photo.FileUrl != null
                         && photo.FileUrl != ""
                         && (photo.CreateByEntity.GroupId == groupId
                         || photo.Offices.Contains(groupName)
                         || photo.Association.Contains(groupName))
                         orderby photo.CreateDate descending
                         select new ImageViewModel
                         {
                             Id = photo.Id,
                             Name = photo.Name,
                             RawUrl = WebHelper.Instance.RootUrl + photo.FileUrl,
                             FileUrl = photo.FileUrl,
                             CreateDate = photo.CreateDate,
                             ObjectType = ObjectType.Photo
                         };
            }

            // 获取缩略图地址
            List<ImageViewModel> groupPhotos = userPhotos.Union(photos).OrderByDescending(i => i.CreateDate).Take(count).ToList();
            foreach (ImageViewModel item in groupPhotos)
            {
                item.FileUrl = ImageHelper.GetSmallThumbUrl(item.FileUrl);
            }

            return groupPhotos;
        }

        public List<ImageViewModel> GetTopRankList(int count, int? groupId)
        {
            IQueryable<ImageViewModel> userPhotos;
            IQueryable<ImageViewModel> photos;
            if (groupId == null)
            {
                userPhotos = from userPhoto in _db.UserPhotos
                             where userPhoto.IsPublic == true
                             orderby userPhoto.Score descending
                             select new ImageViewModel
                             {
                                 Id = userPhoto.Id,
                                 Name = userPhoto.Name,
                                 RawUrl = WebHelper.Instance.RootUrl + userPhoto.FileUrl,
                                 FileUrl = userPhoto.FileUrl,
                                 CreateDate = userPhoto.CreateDate,
                                 Score = userPhoto.Score,
                                 ObjectType = ObjectType.UserPhoto
                             };

                photos = from photo in _db.Photos
                         where photo.CategoryEntity.CategoryType == ObjectType.Photo
                         && photo.Status == 1
                         && photo.FileUrl != null
                         && photo.FileUrl != ""
                         orderby photo.Score descending
                         select new ImageViewModel
                         {
                             Id = photo.Id,
                             Name = photo.Name,
                             RawUrl = WebHelper.Instance.RootUrl + photo.FileUrl,
                             FileUrl = photo.FileUrl,
                             CreateDate = photo.CreateDate,
                             Score = photo.Score,
                             ObjectType = ObjectType.Photo
                         };
            }
            else
            {
                string groupName = _db.Groups.Find(groupId).Name;

                userPhotos = from userPhoto in _db.UserPhotos
                             where userPhoto.CreateByEntity.GroupId == groupId
                             && (userPhoto.CreateByEntity.GroupId == WebHelper.Instance.CurrentUser.GroupId
                             || userPhoto.IsPublic == true)
                             orderby userPhoto.Score descending
                             select new ImageViewModel
                             {
                                 Id = userPhoto.Id,
                                 Name = userPhoto.Name,
                                 RawUrl = WebHelper.Instance.RootUrl + userPhoto.FileUrl,
                                 FileUrl = userPhoto.FileUrl,
                                 CreateDate = userPhoto.CreateDate,
                                 Score = userPhoto.Score,
                                 ObjectType = ObjectType.UserPhoto
                             };

                photos = from photo in _db.Photos
                         where photo.CategoryEntity.CategoryType == ObjectType.Photo
                         && photo.Status == 1
                         && photo.FileUrl != null
                         && photo.FileUrl != ""
                         && (photo.CreateByEntity.GroupId == groupId
                         || photo.Offices.Contains(groupName)
                         || photo.Association.Contains(groupName))
                         orderby photo.Score descending
                         select new ImageViewModel
                         {
                             Id = photo.Id,
                             Name = photo.Name,
                             RawUrl = WebHelper.Instance.RootUrl + photo.FileUrl,
                             FileUrl = photo.FileUrl,
                             CreateDate = photo.CreateDate,
                             Score = photo.Score,
                             ObjectType = ObjectType.Photo
                         };
            }

            List<ImageViewModel> groupPhotos = userPhotos.Union(photos).OrderByDescending(i => i.Score).Take(count).ToList();

            // 获取缩略图地址
            foreach (ImageViewModel item in groupPhotos)
            {
                item.FileUrl = ImageHelper.GetSmallThumbUrl(item.FileUrl);
            }

            return groupPhotos;
        }

        public IPagedList<ImageViewModel> GetImagesByGroup(int? groupId, int? pageSize, int? pageIndex)
        {
            // 执行查询
            IQueryable<ImageViewModel> userPhotos;
            IQueryable<ImageViewModel> photos;
            if (groupId == null)
            {
                userPhotos = from userPhoto in _db.UserPhotos
                             where userPhoto.CreateByEntity.GroupId == WebHelper.Instance.CurrentUser.GroupId
                             || userPhoto.IsPublic == true
                             orderby userPhoto.CreateDate descending
                             select new ImageViewModel
                             {
                                 Id = userPhoto.Id,
                                 Name = userPhoto.Name,
                                 RawUrl = WebHelper.Instance.RootUrl + userPhoto.FileUrl,
                                 FileUrl = userPhoto.FileUrl,
                                 CreateDate = userPhoto.CreateDate,
                                 ObjectType = ObjectType.UserPhoto
                             };

                photos = from photo in _db.Photos
                         where photo.CategoryEntity.CategoryType == ObjectType.Photo
                         && photo.Status == 1
                         && photo.FileUrl != null
                         && photo.FileUrl != ""
                         orderby photo.CreateDate descending
                         select new ImageViewModel
                         {
                             Id = photo.Id,
                             Name = photo.Name,
                             RawUrl = WebHelper.Instance.RootUrl + photo.FileUrl,
                             FileUrl = photo.FileUrl,
                             CreateDate = photo.CreateDate,
                             ObjectType = ObjectType.Photo
                         };
            }
            else
            {
                string groupName = _db.Groups.Find(groupId).Name;

                userPhotos = from userPhoto in _db.UserPhotos
                             where userPhoto.CreateByEntity.GroupId == groupId
                             && (userPhoto.CreateByEntity.GroupId == WebHelper.Instance.CurrentUser.GroupId
                             || userPhoto.IsPublic == true)
                             orderby userPhoto.CreateDate descending
                             select new ImageViewModel
                             {
                                 Id = userPhoto.Id,
                                 Name = userPhoto.Name,
                                 RawUrl = WebHelper.Instance.RootUrl + userPhoto.FileUrl,
                                 FileUrl = userPhoto.FileUrl,
                                 CreateDate = userPhoto.CreateDate,
                                 ObjectType = ObjectType.UserPhoto
                             };

                photos = from photo in _db.Photos
                         where photo.CategoryEntity.CategoryType == ObjectType.Photo
                         && photo.Status == 1
                         && photo.FileUrl != null
                         && photo.FileUrl != ""
                         && (photo.CreateByEntity.GroupId == groupId
                         || photo.Offices.Contains(groupName)
                         || photo.Association.Contains(groupName))
                         orderby photo.CreateDate descending
                         select new ImageViewModel
                         {
                             Id = photo.Id,
                             Name = photo.Name,
                             RawUrl = WebHelper.Instance.RootUrl + photo.FileUrl,
                             FileUrl = photo.FileUrl,
                             CreateDate = photo.CreateDate,
                             ObjectType = ObjectType.Photo
                         };
            }


            // 分页处理
            pageSize = (pageSize ?? 20);
            pageIndex = (pageIndex ?? 1);
            IQueryable<ImageViewModel> groupPhotos = userPhotos.Union(photos).OrderByDescending(i => i.CreateDate);
            var pagedList = groupPhotos.ToPagedList(pageIndex.Value, pageSize.Value);

            // 获取缩略图地址
            foreach (ImageViewModel item in pagedList)
            {
                item.FileUrl = ImageHelper.GetSmallThumbUrl(item.FileUrl);
            }

            return pagedList;
        }

        public IPagedList<ImageViewModel> Search(string keyword, int? pageSize, int? pageIndex)
        {
            // 执行查询
            var userPhotos = from userPhoto in _db.UserPhotos
                             where userPhoto.Name.Contains(keyword)
                             && (userPhoto.CreateByEntity.GroupId == WebHelper.Instance.CurrentUser.GroupId
                             || userPhoto.IsPublic == true)
                             orderby userPhoto.CreateDate descending
                             select new ImageViewModel
                             {
                                 Id = userPhoto.Id,
                                 Name = userPhoto.Name,
                                 RawUrl = WebHelper.Instance.RootUrl + userPhoto.FileUrl,
                                 FileUrl = userPhoto.FileUrl,
                                 CreateDate = userPhoto.CreateDate,
                                 ObjectType = ObjectType.UserPhoto
                             };

            // 分页处理
            pageSize = (pageSize ?? 20);
            pageIndex = (pageIndex ?? 1);
            var pagedList = userPhotos.ToPagedList(pageIndex.Value, pageSize.Value);

            // 获取缩略图地址
            foreach (ImageViewModel item in pagedList)
            {
                item.FileUrl = ImageHelper.GetSmallThumbUrl(item.FileUrl);
            }

            return pagedList;
        }

        public void Create(UserPhoto userPhoto)
        {
            userPhoto = _db.UserPhotos.Add(userPhoto);
            _db.SaveChanges();

            SendMessageQueue(userPhoto.Id);
        }

        public void Update(UserPhoto userPhoto, bool fileChanged)
        {
            _db.Entry(userPhoto).State = EntityState.Modified;
            _db.SaveChanges();

            if (fileChanged)
            {
                SendMessageQueue(userPhoto.Id);
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
            string message = "{\"Command\":\"ID\",\"StartId\":\"" + id + "\",\"EndId\":\"" + id + "\",\"CategoryType\":\"6\"}";
            mq.Send(message);
        }

        public void Delete(UserPhoto userPhoto)
        {
            // 删除文件
            string filePath = Path.Combine(WebHelper.Instance.RootPath, userPhoto.FileUrl);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            // 删除数据
            _db.UserPhotos.Remove(userPhoto);
            _db.SaveChanges();
        }

        public void PostScore(int userPhotoId, double scoreValue, int scoreCount)
        {
            UserPhoto userPhoto = _db.UserPhotos.Find(userPhotoId);
            userPhoto.Score = scoreValue;
            userPhoto.ScoreCount = scoreCount;
            _db.Entry(userPhoto).State = EntityState.Modified;
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