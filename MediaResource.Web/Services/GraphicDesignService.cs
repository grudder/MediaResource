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
    public class GraphicDesignService : IDisposable
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public GraphicDesign Get(int? id)
        {
            return _db.GraphicDesigns.Find(id);
        }

        public GraphicDesign View(int? id)
        {
            GraphicDesign graphicDesign = _db.GraphicDesigns.Find(id);
            graphicDesign.ClickCount = (graphicDesign.ClickCount == null) ? 1 : graphicDesign.ClickCount + 1;

            _db.Entry(graphicDesign).State = EntityState.Modified;
            _db.SaveChanges();

            return graphicDesign;
        }

        public List<ImageViewModel> GetTopImages(int count)
        {
            var graphicDesigns = from graphicDesign in _db.GraphicDesigns
                           where graphicDesign.Status == 1
                           && graphicDesign.PreviewPath != null
                           && graphicDesign.PreviewPath != ""
                           orderby graphicDesign.CreateDate descending
                           select new ImageViewModel
                           {
                               Id = graphicDesign.Id,
                               Name = graphicDesign.Name,
                               RawUrl = WebHelper.Instance.RootUrl + graphicDesign.PreviewPath,
                               FileUrl = graphicDesign.PreviewPath,
                               CreateDate = graphicDesign.CreateDate
                           };

            List<ImageViewModel> list = graphicDesigns.Take(count).ToList();
            foreach (ImageViewModel item in list)
            {
                item.FileUrl = WebHelper.Instance.RootUrl + item.FileUrl;
            }

            return list;
        }

        public IPagedList<ImageViewModel> GetImagesByCategory(int? categoryId, int? pageSize, int? pageIndex)
        {
            // 构造分类查询条件
            List<int?> childCategoryIds = new CategoryService().GetChildCategoryIds(ObjectType.GraphicDesign, categoryId);
            Expression<Func<GraphicDesign, bool>> condition = (i => false);
            condition = childCategoryIds.Aggregate(
                condition, (current, childCategoryId) =>
                    current.Or(i => i.Category == childCategoryId));

            // 执行查询
            var query = _db.GraphicDesigns
                .Where(graphicDesign => graphicDesign.Status == 1 && graphicDesign.PreviewPath != null && graphicDesign.PreviewPath != "")
                .Where(condition.Compile())
                .OrderByDescending(graphicDesign => graphicDesign.CreateDate)
                .Select(graphicDesign => new ImageViewModel
                {
                    Id = graphicDesign.Id,
                    Name = graphicDesign.Name,
                    RawUrl = WebHelper.Instance.RootUrl + graphicDesign.PreviewPath,
                    FileUrl = graphicDesign.PreviewPath,
                    CreateDate = graphicDesign.CreateDate
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
            var query = from graphicDesign in _db.GraphicDesigns
                        where graphicDesign.Status == 1
                        && graphicDesign.PreviewPath != null
                        && graphicDesign.PreviewPath != ""
                        && graphicDesign.Name.Contains(keyword)
                        orderby graphicDesign.CreateDate descending
                        select new ImageViewModel
                        {
                            Id = graphicDesign.Id,
                            Name = graphicDesign.Name,
                            RawUrl = WebHelper.Instance.RootUrl + graphicDesign.PreviewPath,
                            FileUrl = graphicDesign.PreviewPath,
                            CreateDate = graphicDesign.CreateDate
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

        public void PostScore(int graphicDesignId, double scoreValue, int scoreCount)
        {
            GraphicDesign graphicDesign = _db.GraphicDesigns.Find(graphicDesignId);
            graphicDesign.Score = scoreValue;
            graphicDesign.ScoreCount = scoreCount;
            _db.Entry(graphicDesign).State = EntityState.Modified;
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