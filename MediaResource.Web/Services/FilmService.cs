using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

using MediaResource.Web.DataAccess;
using MediaResource.Web.Helper;
using MediaResource.Web.Models;
using MediaResource.Web.Models.ViewModels;

using PagedList;

namespace MediaResource.Web.Services
{
    public class FilmService : IDisposable
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public FilmViewModel View(int? id)
        {
            Film film = _db.Films.Find(id);
            film.ClickCount = (film.ClickCount == null) ? 1 : film.ClickCount + 1;

            _db.Entry(film).State = EntityState.Modified;
            _db.SaveChanges();

            var filmViewModel = new FilmViewModel(film);
            return filmViewModel;
        }

        public List<ImageViewModel> GetTopImages(int count)
        {
            var films = from film in _db.Films
                        where film.Status == 1
                        && film.IsConverted == true
                        && film.ImagePath != null
                        && film.ImagePath != ""
                        orderby film.CreateDate descending
                        select new ImageViewModel
                        {
                            Id = film.Id,
                            Name = film.Title,
                            FileUrl = film.ImagePath,
                            CreateDate = film.CreateDate
                        };

            List<ImageViewModel> list = films.Take(count).ToList();
            foreach (ImageViewModel item in list)
            {
                string fileUrl = item.FileUrl;
                fileUrl = ImageHelper.GetSnapUrl(fileUrl);
                item.FileUrl = fileUrl;
            }

            return list;
        }

        public IPagedList<ImageViewModel> GetImagesByCategory(int? categoryId, int? pageSize, int? pageIndex)
        {
            // 构造分类查询条件
            List<int?> childCategoryIds = new CategoryService().GetChildCategoryIds(ObjectType.Film, categoryId);
            Expression<Func<Film, bool>> condition = (i => false);
            condition = childCategoryIds.Aggregate(
                condition, (current, childCategoryId) =>
                    current.Or(i => i.Category == childCategoryId));

            // 执行查询
            var query = _db.Films
                .Where(film => film.Status == 1 && film.IsConverted == true && film.ImagePath != null && film.ImagePath != "")
                .Where(condition.Compile())
                .OrderByDescending(film => film.CreateDate)
                .Select(film => new ImageViewModel
                {
                    Id = film.Id,
                    Name = film.Title,
                    FileUrl = film.ImagePath,
                    CreateDate = film.CreateDate
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
            var query = from film in _db.Films
                        where film.Status == 1
                        && film.IsConverted == true
                        && film.ImagePath != null
                        && film.ImagePath != ""
                        && film.Title.Contains(keyword)
                        orderby film.CreateDate descending
                        select new ImageViewModel
                        {
                            Id = film.Id,
                            Name = film.Title,
                            FileUrl = film.FileUrl,
                            CreateDate = film.CreateDate
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

        public void PostScore(int filmId, double scoreValue, int scoreCount)
        {
            Film film = _db.Films.Find(filmId);
            film.Score = scoreValue;
            film.ScoreCount = scoreCount;
            _db.Entry(film).State = EntityState.Modified;
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