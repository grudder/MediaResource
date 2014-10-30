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
    public class GraphicService : IDisposable
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public Graphic Get(int? id)
        {
            return _db.Graphics.Find(id);
        }

        public Graphic View(int? id)
        {
            Graphic graphic = _db.Graphics.Find(id);
            graphic.ClickCount = (graphic.ClickCount == null) ? 1 : graphic.ClickCount + 1;

            _db.Entry(graphic).State = EntityState.Modified;
            _db.SaveChanges();

            return graphic;
        }

        public List<ImageViewModel> GetTopImages(int count)
        {
            var graphics = from graphic in _db.Graphics
                           where graphic.Status == 1
                           && graphic.PreviewPath != null
                           && graphic.PreviewPath != ""
                           orderby graphic.CreateDate descending
                           select new ImageViewModel
                           {
                               Id = graphic.Id,
                               Name = graphic.Name,
                               RawUrl = WebHelper.Instance.RootUrl + graphic.PreviewPath,
                               FileUrl = WebHelper.Instance.RootUrl + graphic.PreviewPath,
                               CreateDate = graphic.CreateDate
                           };

            List<ImageViewModel> list = graphics.Take(count).ToList();

            return list;
        }

        public List<ImageViewModel> GetTopImages(int count, int? groupId)
        {
            IQueryable<ImageViewModel> graphics;
            if (groupId == null)
            {
                graphics = from graphic in _db.Graphics
                           where graphic.CategoryEntity.CategoryType == ObjectType.Graphic
                           && graphic.Status == 1
                           && graphic.PreviewPath != null
                           && graphic.PreviewPath != ""
                           orderby graphic.CreateDate descending
                           select new ImageViewModel
                           {
                               Id = graphic.Id,
                               Name = graphic.Name,
                               RawUrl = WebHelper.Instance.RootUrl + graphic.PreviewPath,
                               FileUrl = WebHelper.Instance.RootUrl + graphic.PreviewPath,
                               CreateDate = graphic.CreateDate,
                               ObjectType = ObjectType.Graphic
                           };
            }
            else
            {
                string groupName = _db.Groups.Find(groupId).Name;

                graphics = from graphic in _db.Graphics
                           where graphic.CategoryEntity.CategoryType == ObjectType.Graphic
                           && graphic.Status == 1
                           && graphic.PreviewPath != null
                           && graphic.PreviewPath != ""
                           && (graphic.CreateByEntity.GroupId == groupId
                           || graphic.Association.Contains(groupName))
                           orderby graphic.CreateDate descending
                           select new ImageViewModel
                           {
                               Id = graphic.Id,
                               Name = graphic.Name,
                               RawUrl = WebHelper.Instance.RootUrl + graphic.PreviewPath,
                               FileUrl = WebHelper.Instance.RootUrl + graphic.PreviewPath,
                               CreateDate = graphic.CreateDate,
                               ObjectType = ObjectType.Graphic
                           };
            }

            // 获取缩略图地址
            List<ImageViewModel> groupGraphics = graphics.Take(count).ToList();

            return groupGraphics;
        }

        public IPagedList<ImageViewModel> GetImagesByCategory(int? categoryId, int? pageSize, int? pageIndex)
        {
            // 构造分类查询条件
            List<int?> childCategoryIds = new CategoryService().GetChildCategoryIds(ObjectType.Graphic, categoryId);
            Expression<Func<Graphic, bool>> condition = (i => false);
            condition = childCategoryIds.Aggregate(
                condition, (current, childCategoryId) =>
                    current.Or(i => i.Category == childCategoryId));

            // 执行查询
            var query = _db.Graphics
                .Where(graphic => graphic.Status == 1
                    && graphic.PreviewPath != null
                    && graphic.PreviewPath != "")
                .Where(condition.Compile())
                .OrderByDescending(graphic => graphic.CreateDate)
                .Select(graphic => new ImageViewModel
                {
                    Id = graphic.Id,
                    Name = graphic.Name,
                    RawUrl = WebHelper.Instance.RootUrl + graphic.PreviewPath,
                    FileUrl = WebHelper.Instance.RootUrl + graphic.PreviewPath,
                    CreateDate = graphic.CreateDate
                });

            // 分页处理
            pageSize = (pageSize ?? 20);
            pageIndex = (pageIndex ?? 1);
            var pagedList = query.ToPagedList(pageIndex.Value, pageSize.Value);
            
            return pagedList;
        }

        public IPagedList<ImageViewModel> GetImagesByGroup(int? groupId, int? pageSize, int? pageIndex)
        {
            // 执行查询
            IQueryable<ImageViewModel> graphics;
            if (groupId == null)
            {
                graphics = from graphic in _db.Graphics
                           where graphic.CategoryEntity.CategoryType == ObjectType.Graphic
                           && graphic.Status == 1
                           && graphic.PreviewPath != null
                           && graphic.PreviewPath != ""
                           orderby graphic.CreateDate descending
                           select new ImageViewModel
                           {
                               Id = graphic.Id,
                               Name = graphic.Name,
                               RawUrl = WebHelper.Instance.RootUrl + graphic.PreviewPath,
                               FileUrl = WebHelper.Instance.RootUrl + graphic.PreviewPath,
                               CreateDate = graphic.CreateDate,
                               ObjectType = ObjectType.Graphic
                           };
            }
            else
            {
                string groupName = _db.Groups.Find(groupId).Name;

                graphics = from graphic in _db.Graphics
                           where graphic.CategoryEntity.CategoryType == ObjectType.Graphic
                           && graphic.Status == 1
                           && graphic.PreviewPath != null
                           && graphic.PreviewPath != ""
                           && (graphic.CreateByEntity.GroupId == groupId
                           || graphic.Association.Contains(groupName))
                           orderby graphic.CreateDate descending
                           select new ImageViewModel
                           {
                               Id = graphic.Id,
                               Name = graphic.Name,
                               RawUrl = WebHelper.Instance.RootUrl + graphic.PreviewPath,
                               FileUrl = WebHelper.Instance.RootUrl + graphic.PreviewPath,
                               CreateDate = graphic.CreateDate,
                               ObjectType = ObjectType.Graphic
                           };
            }


            // 分页处理
            pageSize = (pageSize ?? 20);
            pageIndex = (pageIndex ?? 1);
            var pagedList = graphics.ToPagedList(pageIndex.Value, pageSize.Value);

            return pagedList;
        }

        public StaticPagedList<ImageViewModel> AdvancedSearch(string nameOrKeyword, string person, string startTime, string endTime, string groupIds, int? categoryId, int? pageSize, int? pageIndex)
        {
            // 执行查询
            IQueryable<Graphic> query;
            if (categoryId == null || categoryId == 0)
            {
                query = from graphic in _db.Graphics
                        where graphic.Status == 1
                        && graphic.PreviewPath != null
                        && graphic.PreviewPath != ""
                        && graphic.CategoryEntity.CategoryType == ObjectType.Graphic
                        orderby graphic.CreateDate descending
                        select graphic;
            }
            else
            {
                query = from graphic in _db.Graphics
                        where graphic.Status == 1
                        && graphic.FileUrl != null
                        && graphic.FileUrl != ""
                        && graphic.CategoryEntity.CategoryType == ObjectType.Graphic
                        && (graphic.Category == categoryId
                        || graphic.CategoryEntity.CategoryNum.Contains(categoryId + "_"))
                        orderby graphic.CreateDate descending
                        select graphic;
            }

            // 构造查询条件
            if (!String.IsNullOrWhiteSpace(nameOrKeyword))
            {
                query = query.Where(i => i.Name.Contains(nameOrKeyword));
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
                Expression<Func<Graphic, bool>> groupCondition = i => false;
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

        private IEnumerable<ImageViewModel> GetImagesInPage(IEnumerable<Graphic> query, int pageIndex, int pageSize,
            out int totalCount)
        {
            IEnumerable<Graphic> enumerable = query as Graphic[] ?? query.ToArray();
            totalCount = enumerable.Count();
            IEnumerable<Graphic> graphics = enumerable.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return graphics.ToList().Select(graphic => new ImageViewModel
            {
                Id = graphic.Id,
                Name = graphic.Name,
                RawUrl = WebHelper.Instance.RootUrl + graphic.PreviewPath,
                FileUrl = WebHelper.Instance.RootUrl + graphic.PreviewPath,
                CreateDate = graphic.CreateDate
            });
        }

        public IPagedList<ImageViewModel> Search(string keyword, int? pageSize, int? pageIndex)
        {
            // 执行查询
            var query = from graphic in _db.Graphics
                        where graphic.Status == 1
                        && graphic.PreviewPath != null
                        && graphic.PreviewPath != ""
                        && graphic.Name.Contains(keyword)
                        orderby graphic.CreateDate descending
                        select new ImageViewModel
                        {
                            Id = graphic.Id,
                            Name = graphic.Name,
                            RawUrl = WebHelper.Instance.RootUrl + graphic.PreviewPath,
                            FileUrl = WebHelper.Instance.RootUrl + graphic.PreviewPath,
                            CreateDate = graphic.CreateDate
                        };

            // 分页处理
            pageSize = (pageSize ?? 20);
            pageIndex = (pageIndex ?? 1);
            var pagedList = query.ToPagedList(pageIndex.Value, pageSize.Value);

            return pagedList;
        }

        public void PostScore(int graphicId, double scoreValue, int scoreCount)
        {
            Graphic graphic = _db.Graphics.Find(graphicId);
            graphic.Score = scoreValue;
            graphic.ScoreCount = scoreCount;
            _db.Entry(graphic).State = EntityState.Modified;
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