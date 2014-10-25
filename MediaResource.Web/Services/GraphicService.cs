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
                               FileUrl = graphic.PreviewPath,
                               CreateDate = graphic.CreateDate
                           };

            List<ImageViewModel> list = graphics.Take(count).ToList();
            foreach (ImageViewModel item in list)
            {
                item.FileUrl = WebHelper.Instance.RootUrl + item.FileUrl;
            }

            return list;
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
                .Where(graphic => graphic.Status == 1 && graphic.PreviewPath != null && graphic.PreviewPath != "")
                .Where(condition.Compile())
                .OrderByDescending(graphic => graphic.CreateDate)
                .Select(graphic => new ImageViewModel
                {
                    Id = graphic.Id,
                    Name = graphic.Name,
                    RawUrl = WebHelper.Instance.RootUrl + graphic.PreviewPath,
                    FileUrl = graphic.PreviewPath,
                    CreateDate = graphic.CreateDate
                });

            // 分页处理
            pageSize = (pageSize ?? 20);
            pageIndex = (pageIndex ?? 1);
            var pagedList = query.ToPagedList(pageIndex.Value, pageSize.Value);

            // 获取截图地址
            foreach (ImageViewModel item in pagedList)
            {
                item.FileUrl = WebHelper.Instance.RootUrl + item.FileUrl;
            }

            return pagedList;
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
                            FileUrl = graphic.PreviewPath,
                            CreateDate = graphic.CreateDate
                        };

            // 分页处理
            pageSize = (pageSize ?? 20);
            pageIndex = (pageIndex ?? 1);
            var pagedList = query.ToPagedList(pageIndex.Value, pageSize.Value);

            // 获取缩略图地址
            foreach (ImageViewModel item in pagedList)
            {
                item.FileUrl = WebHelper.Instance.RootUrl + item.FileUrl;
            }

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