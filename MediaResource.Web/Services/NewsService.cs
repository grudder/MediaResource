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
                .Where(news => news.Status == 1 && news.IsConverted == true && news.ImagePath != null && news.ImagePath != "")
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