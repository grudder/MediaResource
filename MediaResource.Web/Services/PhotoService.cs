using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using MediaResource.Web.DataAccess;
using MediaResource.Web.Helper;
using MediaResource.Web.Models;
using MediaResource.Web.Models.ViewModels;

using PagedList;

namespace MediaResource.Web.Services
{
    public class PhotoService : IDisposable
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public Photo Get(int? id)
        {
            return _db.Photos.Find(id);
        }

        public Photo View(int? id)
        {
            Photo photo = _db.Photos.Find(id);
            photo.ClickCount = (photo.ClickCount == null) ? 1 : photo.ClickCount + 1;

            _db.Entry(photo).State = EntityState.Modified;
            _db.SaveChanges();

            return photo;
        }

        public Photo DownloadCount(int? id)
        {
            Photo photo = _db.Photos.Find(id);
            photo.DownloadCount = (photo.DownloadCount == null) ? 1 : photo.DownloadCount + 1;

            _db.Entry(photo).State = EntityState.Modified;
            _db.SaveChanges();

            return photo;
        }

        private List<Category> GetTopPhotoCategories()
        {
            var categories = from category in _db.Categorys
                             where category.CategoryType == ObjectType.Photo
                             && category.Status == 1
                             && category.TypeLevel == 3
                             orderby category.CreateDate descending
                             select category;
            return categories.ToList();
        }

        /// <summary>
        /// 取最新分类下的第一张照片，如果该分类下无照片则继续取下一个分类。
        /// </summary>
        /// <param name="count">照片数量。</param>
        /// <returns>最新分类下的照片。</returns>
        public List<ImageViewModel> GetTopImages(int count)
        {
            var topPhotos = new List<ImageViewModel>();
            var topCategories = GetTopPhotoCategories();
            count = Math.Min(count, topCategories.Count);

            int j = 0;
            foreach (Category topCategory in topCategories)
            {
                if (j == count)
                {
                    break;
                }

                var category = topCategory;
                var photos = from p in _db.Photos
                             where p.Status == 1
                                   && p.FileUrl != null
                                   && p.FileUrl != ""
                                   && p.Category == category.Id
                             orderby p.CreateDate descending
                             select p;
                if (!photos.Any())
                {
                    continue;
                }

                var photo = photos.First();

                string fileUrl = (j == 0)
                    ? ImageHelper.GetLargeThumbUrl(photo.FileUrl)
                    : ImageHelper.GetSmallThumbUrl(photo.FileUrl);
                topPhotos.Add(new ImageViewModel
                {
                    Id = category.Id,
                    Name = category.Name,
                    RawUrl = WebHelper.Instance.RootUrl + photo.FileUrl,
                    FileUrl = fileUrl,
                    CreateDate = photo.CreateDate
                });
                ++j;
            }

            return topPhotos;
        }

        public List<Photo> GetTopRankList(int count)
        {
            var photos = from photo in _db.Photos
                         where photo.Status == 1
                         && photo.FileUrl != null
                         && photo.FileUrl != ""
                         orderby photo.Score descending
                         select photo;
            return photos.Take(count).ToList();
        }

        public StaticPagedList<ImageViewModel> GetImagesByCategory(int? categoryId, int? pageSize, int? pageIndex)
        {
            // 执行查询
            IQueryable<Photo> query;
            if (categoryId == null)
            {
                query = from photo in _db.Photos
                        where photo.Status == 1
                        && photo.FileUrl != null
                        && photo.FileUrl != ""
                        && photo.CategoryEntity.CategoryType == ObjectType.Photo
                        orderby photo.CreateDate descending
                        select photo;
            }
            else
            {
                query = from photo in _db.Photos
                        where photo.Status == 1
                        && photo.FileUrl != null
                        && photo.FileUrl != ""
                        && photo.CategoryEntity.CategoryType == ObjectType.Photo
                        && (photo.Category == categoryId
                        || photo.CategoryEntity.CategoryNum.Contains(categoryId + "_"))
                        orderby photo.CreateDate descending
                        select photo;
            }

            // 进行静态分页处理
            pageSize = (pageSize ?? 20);
            pageIndex = (pageIndex ?? 1);
            int totalCount;
            IEnumerable<ImageViewModel> images = GetImagesInPage(query, pageIndex.Value, pageSize.Value, out totalCount);
            var pagedList = new StaticPagedList<ImageViewModel>(images, pageIndex.Value, pageSize.Value, totalCount);

            return pagedList;
        }

        public StaticPagedList<ImageViewModel> AdvancedSearch(string nameOrKeyword, string person, string startTime, string endTime, string groupIds, int? categoryId, int? pageSize, int? pageIndex)
        {
            // 执行查询
            IQueryable<Photo> query;
            if (categoryId == null || categoryId == 0)
            {
                query = from photo in _db.Photos
                        where photo.Status == 1
                        && photo.FileUrl != null
                        && photo.FileUrl != ""
                        && photo.CategoryEntity.CategoryType == ObjectType.Photo
                        orderby photo.CreateDate descending
                        select photo;
            }
            else
            {
                query = from photo in _db.Photos
                        where photo.Status == 1
                        && photo.FileUrl != null
                        && photo.FileUrl != ""
                        && photo.CategoryEntity.CategoryType == ObjectType.Photo
                        && (photo.Category == categoryId
                        || photo.CategoryEntity.CategoryNum.Contains(categoryId + "_"))
                        orderby photo.CreateDate descending
                        select photo;
            }

            // 构造查询条件
            if (!String.IsNullOrWhiteSpace(nameOrKeyword))
            {
                query = query.Where(i => i.Name.Contains(nameOrKeyword) || i.Keyword.Contains(nameOrKeyword));
            }
            if (!String.IsNullOrWhiteSpace(person))
            {
                query = query.Where(i => i.Leadership.Contains(person) || i.Participants.Contains(person));
            }
            if (!String.IsNullOrWhiteSpace(startTime))
            {
                DateTime dateStart = DateTime.Parse(startTime);
                query = query.Where(i => i.RecordingTime != null && i.RecordingTime.Value >= dateStart);
            }
            if (!String.IsNullOrWhiteSpace(endTime))
            {
                DateTime dateEnd = DateTime.Parse(endTime).AddDays(1);
                query = query.Where(i => i.RecordingTime != null && i.RecordingTime.Value < dateEnd);
            }
            if (!String.IsNullOrWhiteSpace(groupIds))
            {
                Expression<Func<Photo, bool>> groupCondition = i => false;
                foreach (string groupId in groupIds.Split(','))
                {
                    string groupName = _db.Groups.Find(int.Parse(groupId)).Name;
                    groupCondition = groupCondition.Or(i => i.CreateByEntity.GroupId == int.Parse(groupId));
                    groupCondition = groupCondition.Or(i => i.Offices.Contains(groupName));
                    groupCondition = groupCondition.Or(i => i.Association.Contains(groupName));
                }
                query = query.Where(groupCondition);
            }

            // 进行静态分页处理
            pageSize = (pageSize ?? 20);
            pageIndex = (pageIndex ?? 1);
            int totalCount;
            IEnumerable<ImageViewModel> images = GetImagesInPage(query, pageIndex.Value, pageSize.Value, out totalCount);
            var pagedList = new StaticPagedList<ImageViewModel>(images, pageIndex.Value, pageSize.Value, totalCount);

            return pagedList;
        }

        private IEnumerable<ImageViewModel> GetImagesInPage(IEnumerable<Photo> query, int pageIndex, int pageSize,
            out int totalCount)
        {
            IEnumerable<Photo> enumerable = query as Photo[] ?? query.ToArray();
            totalCount = enumerable.Count();
            IEnumerable<Photo> photos = enumerable.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return photos.ToList().Select(photo => new ImageViewModel
                {
                    Id = photo.Id,
                    Name = photo.Name,
                    RawUrl = WebHelper.Instance.RootUrl + photo.FileUrl,
                    FileUrl = ImageHelper.GetSmallThumbUrl(photo.FileUrl),
                    CreateDate = photo.CreateDate
                });
        }

        public IPagedList<ImageViewModel> Search(string keyword, int? pageSize, int? pageIndex)
        {
            // 执行查询
            var query = from photo in _db.Photos
                        where (photo.Name.Contains(keyword)
                        || photo.Leadership.Contains(keyword)
                        || photo.Participants.Contains(keyword))
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
                            CreateDate = photo.CreateDate
                        };

            // 分页处理
            pageSize = (pageSize ?? 20);
            pageIndex = (pageIndex ?? 1);
            var pagedList = query.ToPagedList(pageIndex.Value, pageSize.Value);

            // 获取缩略图地址
            foreach (ImageViewModel item in pagedList)
            {
                item.FileUrl = ImageHelper.GetSmallThumbUrl(item.FileUrl);
            }

            return pagedList;
        }

        public void PostScore(int photoId, double scoreValue, int scoreCount)
        {
            Photo photo = _db.Photos.Find(photoId);
            photo.Score = scoreValue;
            photo.ScoreCount = scoreCount;
            _db.Entry(photo).State = EntityState.Modified;
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