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
    public class TopicVideoService : IDisposable
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private readonly NodeService _nodeService = new NodeService();
        private readonly UserPlateService _userPlateService = new UserPlateService();

        public TopicVideo Get(int? id)
        {
            return _db.TopicVideos.Find(id);
        }

        public TopicVideoViewModel View(int? id)
        {
            TopicVideo topicVideo = _db.TopicVideos.Find(id);
            topicVideo.ClickCount += 1;

            _db.Entry(topicVideo).State = EntityState.Modified;
            _db.SaveChanges();

            var topicVideoViewModel = new TopicVideoViewModel(topicVideo);
            return topicVideoViewModel;
        }

        public TopicVideo DownloadCount(int? id)
        {
            TopicVideo topicVideo = _db.TopicVideos.Find(id);
            topicVideo.ClickCount += 1;

            _db.Entry(topicVideo).State = EntityState.Modified;
            _db.SaveChanges();

            return topicVideo;
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
            IQueryable<TopicVideo> query =
                from topicVideo in _db.TopicVideos
                where topicVideo.IsConverted == true
                && topicVideo.ImagePath != null
                orderby topicVideo.OrderNum descending
                select topicVideo;

            // 构造查询条件
            if (topicId != null)
            {
                query = query.Where(i => i.TopicId == topicId);
            }
            if (nodeId != null)
            {
                List<Node> nodes = _nodeService.GetChildNodes(nodeId.Value);
                Expression<Func<TopicVideo, bool>> nodeCondition = i => false;
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

        private IEnumerable<ImageViewModel> GetImagesInPage(IEnumerable<TopicVideo> query, int pageIndex, int pageSize,
            out int totalCount)
        {
            IEnumerable<TopicVideo> enumerable = query as TopicVideo[] ?? query.ToArray();
            totalCount = enumerable.Count();
            IEnumerable<TopicVideo> topicVideos = enumerable.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return topicVideos.ToList().Select(topicVideo => new ImageViewModel
            {
                Id = topicVideo.Id,
                Name = topicVideo.Name,
                RawUrl = WebHelper.Instance.RootUrl + topicVideo.ImagePath,
                FileUrl = ImageHelper.GetSnapUrl(topicVideo.ImagePath),
                CreateDate = topicVideo.CreateDate
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