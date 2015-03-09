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
    public class TopicImageService : IDisposable
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private readonly NodeService _nodeService = new NodeService();
        private readonly UserPlateService _userPlateService = new UserPlateService();

        public TopicImage Get(int? id)
        {
            return _db.TopicImages.Find(id);
        }

        public TopicImage View(int? id)
        {
            TopicImage topicImage = _db.TopicImages.Find(id);
            topicImage.ClickCount += 1;

            _db.Entry(topicImage).State = EntityState.Modified;
            _db.SaveChanges();

            return topicImage;
        }

        public TopicImage DownloadCount(int? id)
        {
            TopicImage topicImage = _db.TopicImages.Find(id);
            topicImage.DownloadCount += 1;

            _db.Entry(topicImage).State = EntityState.Modified;
            _db.SaveChanges();

            return topicImage;
        }

        /// <summary>
        /// 获取节点或自建版块下的专题图片分页列表。
        /// </summary>
        /// <param name="topicId">专题编号。</param>
        /// <param name="nodeId">节点标识。</param>
        /// <param name="userPlateId">板块标识。</param>
        /// <param name="keyword">关键字。</param>
        /// <param name="pageSize">分页大小。</param>
        /// <param name="pageIndex">页码。</param>
        /// <param name="searchCondition">高级搜索的查询条件。</param>
        /// <returns>节点或自建版块下的专题图片分页列表。</returns>
        public StaticPagedList<ImageViewModel> AdvancedSearch(int? topicId, int? nodeId, int? userPlateId, string keyword, int? pageSize, int? pageIndex, Dictionary<string, string> searchCondition)
        {
            // 执行查询
            IQueryable<TopicImage> query =
                from topicImage in _db.TopicImages
                where topicImage.Locations != null
                orderby topicImage.TextDate descending
                select topicImage;

            //
            // 构造查询条件
            //
            if (topicId != null)
            {
                query = query.Where(i => i.TopicId == topicId);
            }
            if (nodeId != null)
            {
                List<Node> nodes = _nodeService.GetChildNodes(nodeId.Value);
                Expression<Func<TopicImage, bool>> nodeCondition = i => false;
                foreach (Node node in nodes)
                {
                    var theNode = node;
                    nodeCondition = nodeCondition.Or(i => ("|" + i.Nodes + "|").Contains("|" + theNode.NodeName + "(" + theNode.Id + ")|"));
                }
                query = query.Where(nodeCondition);
            }
            if (userPlateId != null)
            {
                UserPlate userPlate = _userPlateService.Get(userPlateId);
                query = query.Where(i => ("|" + i.Nodes + "|").Contains("|" + userPlate.PlateName + "(" + userPlate.Id + ")|"));
            }
            if (!String.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(i => i.Name.Contains(keyword) || i.KeyWords.Contains(keyword) || i.Summary.Contains(keyword));
            }

            // 高级搜索的查询条件
            if (searchCondition.ContainsKey("Name"))
            {
                string name = searchCondition["Name"];
                query = query.Where(i => i.Name.Contains(name));
            }
            if (searchCondition.ContainsKey("KeyWords"))
            {
                string keyWords = searchCondition["KeyWords"];
                query = query.Where(i => i.KeyWords.Contains(keyWords));
            }
            if (searchCondition.ContainsKey("Staff"))
            {
                string staff = searchCondition["Staff"];
                query = query.Where(i => i.Staff.Contains(staff));
            }
            if (searchCondition.ContainsKey("Source"))
            {
                string source = searchCondition["Source"];
                query = query.Where(i => i.Source.Contains(source));
            }
            if (searchCondition.ContainsKey("Summary"))
            {
                string summary = searchCondition["Summary"];
                query = query.Where(i => i.Summary.Contains(summary));
            }
            if (searchCondition.ContainsKey("StartTextDate"))
            {
                DateTime startTextDate = DateTime.Parse(searchCondition["StartTextDate"]);
                query = query.Where(i => i.TextDate != null && i.TextDate >= startTextDate);
            }
            if (searchCondition.ContainsKey("EndTextDate"))
            {
                DateTime endTextDate = DateTime.Parse(searchCondition["EndTextDate"]).AddDays(1);
                query = query.Where(i => i.TextDate != null && i.TextDate < endTextDate);
            }

            // 进行静态分页处理
            pageSize = (pageSize ?? 20);
            pageIndex = (pageIndex ?? 1);
            int totalCount;
            IEnumerable<ImageViewModel> images = GetImagesInPage(query, pageIndex.Value, pageSize.Value, out totalCount);
            var pagedList = new StaticPagedList<ImageViewModel>(images, pageIndex.Value, pageSize.Value, totalCount);

            return pagedList;
        }

        private IEnumerable<ImageViewModel> GetImagesInPage(IEnumerable<TopicImage> query, int pageIndex, int pageSize,
            out int totalCount)
        {
            IEnumerable<TopicImage> enumerable = query as TopicImage[] ?? query.ToArray();
            totalCount = enumerable.Count();
            IEnumerable<TopicImage> topicImages = enumerable.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return topicImages.ToList().Select(topicImage => new ImageViewModel
            {
                Id = topicImage.Id,
                Name = topicImage.Name,
                RawUrl = WebHelper.Instance.RootUrl + topicImage.Locations,
                FileUrl = ImageHelper.GetSmallThumbUrl(topicImage.Locations),
                CreateDate = topicImage.CreateDate
            });
        }

        #region IDisposable 成员

        public void Dispose()
        {
            _db.Dispose();
        }

        #endregion
    }
}