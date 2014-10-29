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
    public class NewsService : IDisposable
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public NewsViewModel View(int? id)
        {
            News news = _db.Newss.Find(id);
            news.ClickCount = (news.ClickCount == null) ? 1 : news.ClickCount + 1;

            _db.Entry(news).State = EntityState.Modified;
            _db.SaveChanges();

            var newsViewModel = new NewsViewModel(news);
            return newsViewModel;
        }

        public List<ImageViewModel> GetTopImages(int count)
        {
            var newss = from news in _db.Newss
                        where news.Status == 1
                        && news.IsConverted == true
                        && news.ImagePath != null
                        && news.ImagePath != ""
                        orderby news.CreateDate descending
                        select new ImageViewModel
                        {
                            Id = news.Id,
                            Name = news.Name,
                            FileUrl = news.ImagePath,
                            CreateDate = news.CreateDate
                        };

            List<ImageViewModel> list = newss.Take(count).ToList();
            foreach (ImageViewModel image in list)
            {
                image.FileUrl = ImageHelper.GetSnapUrl(image.FileUrl);
            }

            return list;
        }

        public List<ImageViewModel> GetTopImages(int count, int? groupId)
        {
            IQueryable<ImageViewModel> newss;
            if (groupId == null)
            {
                newss = from news in _db.Newss
                           where news.CategoryEntity.CategoryType == ObjectType.News
                           && news.Status == 1
                           && news.IsConverted == true
                           && news.FileUrl != null
                           && news.FileUrl != ""
                           orderby news.CreateDate descending
                           select new ImageViewModel
                           {
                               Id = news.Id,
                               Name = news.Name,
                               RawUrl = WebHelper.Instance.RootUrl + news.FileUrl,
                               FileUrl = news.FileUrl,
                               CreateDate = news.CreateDate,
                               ObjectType = ObjectType.News
                           };
            }
            else
            {
                string groupName = _db.Groups.Find(groupId).Name;

                newss = from news in _db.Newss
                           where news.CategoryEntity.CategoryType == ObjectType.News
                           && news.Status == 1
                           && news.IsConverted == true
                           && news.FileUrl != null
                           && news.FileUrl != ""
                           && (news.CreateByEntity.GroupId == groupId
                           || news.Association.Contains(groupName))
                           orderby news.CreateDate descending
                           select new ImageViewModel
                           {
                               Id = news.Id,
                               Name = news.Name,
                               RawUrl = WebHelper.Instance.RootUrl + news.FileUrl,
                               FileUrl = news.FileUrl,
                               CreateDate = news.CreateDate,
                               ObjectType = ObjectType.News
                           };
            }

            // 获取缩略图地址
            List<ImageViewModel> groupNewss = newss.Take(count).ToList();
            foreach (ImageViewModel item in groupNewss)
            {
                item.FileUrl = ImageHelper.GetSmallThumbUrl(item.FileUrl);
            }

            return groupNewss;
        }

        public IPagedList<ImageViewModel> GetImagesByCategory(int? categoryId, int? pageSize, int? pageIndex)
        {
            // 构造分类查询条件
            List<int?> childCategoryIds = new CategoryService().GetChildCategoryIds(ObjectType.News, categoryId);
            Expression<Func<News, bool>> condition = (i => false);
            condition = childCategoryIds.Aggregate(
                condition, (current, childCategoryId) =>
                    current.Or(i => i.Category == childCategoryId));

            // 执行查询
            var query = _db.Newss
                .Where(news => news.Status == 1
                    && news.IsConverted == true
                    && news.ImagePath != null
                    && news.ImagePath != "")
                .Where(condition.Compile())
                .OrderByDescending(news => news.CreateDate)
                .Select(news => new ImageViewModel
                {
                    Id = news.Id,
                    Name = news.Name,
                    FileUrl = news.ImagePath,
                    CreateDate = news.CreateDate
                });

            // 分页处理
            pageSize = (pageSize ?? 20);
            pageIndex = (pageIndex ?? 1);
            var pagedList = query.ToPagedList(pageIndex.Value, pageSize.Value);

            // 获取截图地址
            foreach (ImageViewModel item in pagedList)
            {
                item.FileUrl = ImageHelper.GetSnapUrl(item.FileUrl);
            }

            return pagedList;
        }

        public IPagedList<ImageViewModel> GetImagesByGroup(int? groupId, int? pageSize, int? pageIndex)
        {
            // 执行查询
            IQueryable<ImageViewModel> newss;
            if (groupId == null)
            {
                newss = from news in _db.Newss
                           where news.CategoryEntity.CategoryType == ObjectType.News
                           && news.Status == 1
                           && news.IsConverted == true
                           && news.ImagePath != null
                           && news.ImagePath != ""
                           orderby news.CreateDate descending
                           select new ImageViewModel
                           {
                               Id = news.Id,
                               Name = news.Name,
                               RawUrl = WebHelper.Instance.RootUrl + news.FileUrl,
                               FileUrl = news.FileUrl,
                               CreateDate = news.CreateDate,
                               ObjectType = ObjectType.News
                           };
            }
            else
            {
                string groupName = _db.Groups.Find(groupId).Name;

                newss = from news in _db.Newss
                           where news.CategoryEntity.CategoryType == ObjectType.News
                           && news.Status == 1
                           && news.IsConverted == true
                           && news.ImagePath != null
                           && news.ImagePath != ""
                           && (news.CreateByEntity.GroupId == groupId
                           || news.Association.Contains(groupName))
                           orderby news.CreateDate descending
                           select new ImageViewModel
                           {
                               Id = news.Id,
                               Name = news.Name,
                               RawUrl = WebHelper.Instance.RootUrl + news.FileUrl,
                               FileUrl = news.FileUrl,
                               CreateDate = news.CreateDate,
                               ObjectType = ObjectType.News
                           };
            }


            // 分页处理
            pageSize = (pageSize ?? 20);
            pageIndex = (pageIndex ?? 1);
            var pagedList = newss.ToPagedList(pageIndex.Value, pageSize.Value);

            // 获取缩略图地址
            foreach (ImageViewModel item in pagedList)
            {
                item.FileUrl = ImageHelper.GetSmallThumbUrl(item.FileUrl);
            }

            return pagedList;
        }

        public StaticPagedList<ImageViewModel> AdvancedSearch(string nameOrKeyword, string person, string startTime, string endTime, string groupIds, int? categoryId, int? pageSize, int? pageIndex)
        {
            // 执行查询
            IQueryable<News> query;
            if (categoryId == null || categoryId == 0)
            {
                query = from news in _db.Newss
                        where news.Status == 1
                        && news.IsConverted == true
                        && news.ImagePath != null
                        && news.ImagePath != ""
                        && news.CategoryEntity.CategoryType == ObjectType.News
                        orderby news.CreateDate descending
                        select news;
            }
            else
            {
                query = from news in _db.Newss
                        where news.Status == 1
                        && news.IsConverted == true
                        && news.ImagePath != null
                        && news.ImagePath != ""
                        && news.CategoryEntity.CategoryType == ObjectType.News
                        && (news.Category == categoryId
                        || news.CategoryEntity.CategoryNum.Contains(categoryId + "_"))
                        orderby news.CreateDate descending
                        select news;
            }

            // 构造查询条件
            if (!String.IsNullOrWhiteSpace(nameOrKeyword))
            {
                query = query.Where(i => i.Name.Contains(nameOrKeyword));
            }
            if (!String.IsNullOrWhiteSpace(person))
            {
                query = query.Where(i => i.Reporter.Contains(person) || i.Reporter.Contains(person));
            }
            if (!String.IsNullOrWhiteSpace(startTime))
            {
                DateTime dateStart = DateTime.Parse(startTime);
                query = query.Where(i => i.StoryTime != null && i.StoryTime.Value >= dateStart);
            }
            if (!String.IsNullOrWhiteSpace(endTime))
            {
                DateTime dateEnd = DateTime.Parse(endTime).AddDays(1);
                query = query.Where(i => i.StoryTime != null && i.StoryTime.Value < dateEnd);
            }
            if (!String.IsNullOrWhiteSpace(groupIds))
            {
                Expression<Func<News, bool>> groupCondition = i => false;
                foreach (string groupId in groupIds.Split(','))
                {
                    string groupName = _db.Groups.Find(int.Parse(groupId)).Name;
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

        private IEnumerable<ImageViewModel> GetImagesInPage(IEnumerable<News> query, int pageIndex, int pageSize,
            out int totalCount)
        {
            IEnumerable<News> enumerable = query as News[] ?? query.ToArray();
            totalCount = enumerable.Count();
            IEnumerable<News> newss = enumerable.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return newss.ToList().Select(news => new ImageViewModel
            {
                Id = news.Id,
                Name = news.Name,
                RawUrl = WebHelper.Instance.RootUrl + news.FileUrl,
                FileUrl = ImageHelper.GetSmallThumbUrl(news.FileUrl),
                CreateDate = news.CreateDate
            });
        }

        public IPagedList<ImageViewModel> Search(string keyword, int? pageSize, int? pageIndex)
        {
            // 执行查询
            var query = from news in _db.Newss
                        where news.Status == 1
                        && news.IsConverted == true
                        && news.ImagePath != null
                        && news.ImagePath != ""
                        && news.Name.Contains(keyword)
                        orderby news.CreateDate descending
                        select new ImageViewModel
                        {
                            Id = news.Id,
                            Name = news.Name,
                            FileUrl = news.FileUrl,
                            CreateDate = news.CreateDate
                        };

            // 分页处理
            pageSize = (pageSize ?? 20);
            pageIndex = (pageIndex ?? 1);
            var pagedList = query.ToPagedList(pageIndex.Value, pageSize.Value);

            // 获取缩略图地址
            foreach (ImageViewModel item in pagedList)
            {
                item.FileUrl = ImageHelper.GetSnapUrl(item.FileUrl);
            }

            return pagedList;
        }

        public void PostScore(int newsId, double scoreValue, int scoreCount)
        {
            News news = _db.Newss.Find(newsId);
            news.Score = scoreValue;
            news.ScoreCount = scoreCount;
            _db.Entry(news).State = EntityState.Modified;
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