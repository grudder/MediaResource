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
    public class VideoService : IDisposable
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public VideoViewModel View(int? id)
        {
            Video video = _db.Videos.Find(id);
            video.ClickCount = (video.ClickCount == null) ? 1 : video.ClickCount + 1;

            _db.Entry(video).State = EntityState.Modified;
            _db.SaveChanges();

            var videoViewModel = new VideoViewModel(video);
            return videoViewModel;
        }

        public List<ImageViewModel> GetTopImages(int count)
        {
            var videos = from video in _db.Videos
                         orderby video.CreateDate descending
                         where video.Status == 1
                         && video.PreviewPath != null
                         && video.PreviewPath != ""
                         select new ImageViewModel
                         {
                             Id = video.Id,
                             Name = video.Name,
                             FileUrl = String.IsNullOrEmpty(video.PreviewPath) ? "#" : WebHelper.Instance.RootUrl + video.PreviewPath,
                             CreateDate = video.CreateDate
                         };

            List<ImageViewModel> list = videos.Take(count).ToList();

            return list;
        }

        public List<ImageViewModel> GetTopImages(int count, int groupId)
        {
            var videos = from video in _db.Videos
                         orderby video.CreateDate descending
                         where video.Status == 1
                         && video.PreviewPath != null
                         && video.PreviewPath != ""
                         && video.CreateByEntity.GroupId == groupId
                         select new ImageViewModel
                         {
                             Id = video.Id,
                             Name = video.Name,
                             FileUrl = String.IsNullOrEmpty(video.PreviewPath) ? "#" : WebHelper.Instance.RootUrl + video.PreviewPath,
                             CreateDate = video.CreateDate
                         };

            List<ImageViewModel> list = videos.Take(count).ToList();

            return list;
        }

        public List<Video> GetTopRankList(int count)
        {
            var videos = from video in _db.Videos
                         where video.Status == 1
                         && video.PreviewPath != null
                         && video.PreviewPath != ""
                         orderby video.Score descending
                         select video;
            return videos.Take(count).ToList();
        }

        public List<Video> GetTopRankList(int count, int groupId)
        {
            var videos = from video in _db.Videos
                         where video.Status == 1
                         && video.CreateByEntity.GroupId == groupId
                         && video.PreviewPath != null
                         && video.PreviewPath != ""
                         orderby video.Score descending
                         select video;
            return videos.Take(count).ToList();
        }

        public IPagedList<ImageViewModel> GetImagesByCategory(int? categoryId, int? pageSize, int? pageIndex)
        {
            // 构造分类查询条件
            List<int?> childCategoryIds = new CategoryService().GetChildCategoryIds(ObjectType.Video, categoryId);
            Expression<Func<Video, bool>> condition = (i => false);
            condition = childCategoryIds.Aggregate(
                condition, (current, childCategoryId) =>
                    current.Or(i => i.Category == childCategoryId));

            // 执行查询
            var query = _db.Videos
                .Where(i => i.Status == 1
                    && i.PreviewPath != null
                    && i.PreviewPath != "")
                .Where(condition.Compile())
                .OrderByDescending(video => video.CreateDate)
                .Select(video => new ImageViewModel
                {
                    Id = video.Id,
                    Name = video.Name,
                    FileUrl = String.IsNullOrEmpty(video.PreviewPath) ? "#" : WebHelper.Instance.RootUrl + video.PreviewPath,
                    CreateDate = video.CreateDate
                });

            // 分页处理
            pageSize = (pageSize ?? 20);
            pageIndex = (pageIndex ?? 1);
            var pagedList = query.ToPagedList(pageIndex.Value, pageSize.Value);

            return pagedList;
        }

        public StaticPagedList<ImageViewModel> AdvancedSearch(string nameOrKeyword, string person, string startTime, string endTime, string groupIds, int? categoryId, int? pageSize, int? pageIndex)
        {
            // 执行查询
            IQueryable<Video> query;
            if (categoryId == null || categoryId == 0)
            {
                query = from video in _db.Videos
                        where video.Status == 1
                        && video.PreviewPath != null
                        && video.PreviewPath != ""
                        && video.CategoryEntity.CategoryType == ObjectType.Video
                        orderby video.CreateDate descending
                        select video;
            }
            else
            {
                query = from video in _db.Videos
                        where video.Status == 1
                        && video.PreviewPath != null
                        && video.PreviewPath != ""
                        && video.CategoryEntity.CategoryType == ObjectType.Video
                        && (video.Category == categoryId
                        || video.CategoryEntity.CategoryNum.Contains(categoryId + "_"))
                        orderby video.CreateDate descending
                        select video;
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
                Expression<Func<Video, bool>> groupCondition = i => false;
                foreach (string groupId in groupIds.Split(','))
                {
                    string groupName = _db.Groups.Find(int.Parse(groupId)).Name;
                    groupCondition = groupCondition.Or(i => i.Association == groupName);
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

        private IEnumerable<ImageViewModel> GetImagesInPage(IEnumerable<Video> query, int pageIndex, int pageSize,
            out int totalCount)
        {
            IEnumerable<Video> enumerable = query as Video[] ?? query.ToArray();
            totalCount = enumerable.Count();
            IEnumerable<Video> videos = enumerable.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return videos.ToList().Select(video => new ImageViewModel
            {
                Id = video.Id,
                Name = video.Name,
                FileUrl = String.IsNullOrEmpty(video.PreviewPath) ? "#" : WebHelper.Instance.RootUrl + video.PreviewPath,
                CreateDate = video.CreateDate
            });
        }

        public IPagedList<ImageViewModel> Search(string keyword, int? pageSize, int? pageIndex)
        {
            // 执行查询
            var query = from video in _db.Videos
                        where video.Status == 1
                        && video.PreviewPath != null
                        && video.PreviewPath != ""
                        && video.Name.Contains(keyword)
                        orderby video.CreateDate descending
                        select new ImageViewModel
                        {
                            Id = video.Id,
                            Name = video.Name,
                            FileUrl = String.IsNullOrEmpty(video.PreviewPath) ? "#" : WebHelper.Instance.RootUrl + video.PreviewPath,
                            CreateDate = video.CreateDate
                        };

            // 分页处理
            pageSize = (pageSize ?? 20);
            pageIndex = (pageIndex ?? 1);
            var pagedList = query.ToPagedList(pageIndex.Value, pageSize.Value);

            return pagedList;
        }

        public void PostScore(int videoId, double scoreValue, int scoreCount)
        {
            Video video = _db.Videos.Find(videoId);
            video.Score = scoreValue;
            video.ScoreCount = scoreCount;
            _db.Entry(video).State = EntityState.Modified;
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