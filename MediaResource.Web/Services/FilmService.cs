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
                item.FileUrl = ImageHelper.GetSnapUrl(item.FileUrl);
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

        public StaticPagedList<ImageViewModel> AdvancedSearch(string nameOrKeyword, string person, string startTime, string endTime, string groupIds, int? categoryId, int? pageSize, int? pageIndex)
        {
            // 执行查询
            IQueryable<Film> query;
            if (categoryId == null || categoryId == 0)
            {
                query = from film in _db.Films
                        where film.Status == 1
                        && film.ImagePath != null
                        && film.ImagePath != ""
                        && film.CategoryEntity.CategoryType == ObjectType.Film
                        orderby film.CreateDate descending
                        select film;
            }
            else
            {
                query = from film in _db.Films
                        where film.Status == 1
                        && film.ImagePath != null
                        && film.ImagePath != ""
                        && film.CategoryEntity.CategoryType == ObjectType.Film
                        && (film.Category == categoryId
                        || film.CategoryEntity.CategoryNum.Contains(categoryId + "_"))
                        orderby film.CreateDate descending
                        select film;
            }

            // 构造查询条件
            if (!String.IsNullOrWhiteSpace(nameOrKeyword))
            {
                query = query.Where(i => i.Title.Contains(nameOrKeyword));
            }
            //if (!String.IsNullOrWhiteSpace(person))
            //{
            //    query = query.Where(i => i.Leadership.Contains(person) || i.Participants.Contains(person));
            //}
            //if (!String.IsNullOrWhiteSpace(startTime))
            //{
            //    DateTime dateStart = DateTime.Parse(startTime);
            //    query = query.Where(i => i.RecordingTime != null && i.RecordingTime.Value >= dateStart);
            //}
            //if (!String.IsNullOrWhiteSpace(endTime))
            //{
            //    DateTime dateEnd = DateTime.Parse(endTime).AddDays(1);
            //    query = query.Where(i => i.RecordingTime != null && i.RecordingTime.Value < dateEnd);
            //}
            if (!String.IsNullOrWhiteSpace(groupIds))
            {
                Expression<Func<Film, bool>> groupCondition = i => false;
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

        private IEnumerable<ImageViewModel> GetImagesInPage(IEnumerable<Film> query, int pageIndex, int pageSize,
            out int totalCount)
        {
            IEnumerable<Film> enumerable = query as Film[] ?? query.ToArray();
            totalCount = enumerable.Count();
            IEnumerable<Film> films = enumerable.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return films.ToList().Select(film => new ImageViewModel
            {
                Id = film.Id,
                Name = film.Title,
                FileUrl = ImageHelper.GetSnapUrl(film.ImagePath),
                CreateDate = film.CreateDate
            });
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
                            FileUrl = film.ImagePath,
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