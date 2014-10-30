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
                               FileUrl = WebHelper.Instance.RootUrl + graphicDesign.PreviewPath,
                               CreateDate = graphicDesign.CreateDate
                           };

            List<ImageViewModel> list = graphicDesigns.Take(count).ToList();

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
                .Where(graphicDesign => graphicDesign.Status == 1
                    && graphicDesign.PreviewPath != null
                    && graphicDesign.PreviewPath != "")
                .Where(condition.Compile())
                .OrderByDescending(graphicDesign => graphicDesign.CreateDate)
                .Select(graphicDesign => new ImageViewModel
                {
                    Id = graphicDesign.Id,
                    Name = graphicDesign.Name,
                    RawUrl = WebHelper.Instance.RootUrl + graphicDesign.PreviewPath,
                    FileUrl = WebHelper.Instance.RootUrl + graphicDesign.PreviewPath,
                    CreateDate = graphicDesign.CreateDate
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
            IQueryable<GraphicDesign> query;
            if (categoryId == null || categoryId == 0)
            {
                query = from graphicDesign in _db.GraphicDesigns
                        where graphicDesign.Status == 1
                        && graphicDesign.PreviewPath != null
                        && graphicDesign.PreviewPath != ""
                        && graphicDesign.CategoryEntity.CategoryType == ObjectType.GraphicDesign
                        orderby graphicDesign.CreateDate descending
                        select graphicDesign;
            }
            else
            {
                query = from graphicDesign in _db.GraphicDesigns
                        where graphicDesign.Status == 1
                        && graphicDesign.PreviewPath != null
                        && graphicDesign.PreviewPath != ""
                        && graphicDesign.CategoryEntity.CategoryType == ObjectType.GraphicDesign
                        && (graphicDesign.Category == categoryId
                        || graphicDesign.CategoryEntity.CategoryNum.Contains(categoryId + "_"))
                        orderby graphicDesign.CreateDate descending
                        select graphicDesign;
            }

            // 构造查询条件
            if (!String.IsNullOrWhiteSpace(nameOrKeyword))
            {
                query = query.Where(i => i.Name.Contains(nameOrKeyword) || i.Keyword.Contains(nameOrKeyword));
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
                Expression<Func<GraphicDesign, bool>> groupCondition = i => false;
                foreach (string groupId in groupIds.Split(','))
                {
                    string groupName = _db.Groups.Find(int.Parse(groupId)).Name;
                    groupCondition = groupCondition.Or(i => i.Associate.Contains(groupName));
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

        private IEnumerable<ImageViewModel> GetImagesInPage(IEnumerable<GraphicDesign> query, int pageIndex, int pageSize,
            out int totalCount)
        {
            IEnumerable<GraphicDesign> enumerable = query as GraphicDesign[] ?? query.ToArray();
            totalCount = enumerable.Count();
            IEnumerable<GraphicDesign> graphicDesigns = enumerable.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return graphicDesigns.ToList().Select(graphicDesign => new ImageViewModel
            {
                Id = graphicDesign.Id,
                Name = graphicDesign.Name,
                RawUrl = WebHelper.Instance.RootUrl + graphicDesign.PreviewPath,
                FileUrl = WebHelper.Instance.RootUrl + graphicDesign.PreviewPath,
                CreateDate = graphicDesign.CreateDate
            });
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
                            FileUrl = WebHelper.Instance.RootUrl + graphicDesign.PreviewPath,
                            CreateDate = graphicDesign.CreateDate
                        };

            // 分页处理
            pageSize = (pageSize ?? 20);
            pageIndex = (pageIndex ?? 1);
            var pagedList = query.ToPagedList(pageIndex.Value, pageSize.Value);

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