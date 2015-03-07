using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using MediaResource.Web.DataAccess;
using MediaResource.Web.Helper;
using MediaResource.Web.Models;

using PagedList;

namespace MediaResource.Web.Services
{
    public class TopicNewsService : IDisposable
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private readonly NodeService _nodeService = new NodeService();
        private readonly UserPlateService _userPlateService = new UserPlateService();

        public TopicNews Get(int? id)
        {
            return _db.TopicNewss.Find(id);
        }

        public TopicNews View(int? id)
        {
            TopicNews topicNews = _db.TopicNewss.Find(id);
            topicNews.ClickCount += 1;

            _db.Entry(topicNews).State = EntityState.Modified;
            _db.SaveChanges();

            return topicNews;
        }

        public TopicNews DownloadCount(int? id)
        {
            TopicNews topicNews = _db.TopicNewss.Find(id);
            topicNews.DownloadCount += 1;

            _db.Entry(topicNews).State = EntityState.Modified;
            _db.SaveChanges();

            return topicNews;
        }

        /// <summary>
        /// 获取专题下的媒体报道分页列表。
        /// </summary>
        /// <param name="topicId">专题编号。</param>
        /// <param name="nodeId">节点标识。</param>
        /// <param name="userPlateId">板块标识。</param>
        /// <param name="keyword">关键字。</param>
        /// <param name="pageSize">分页大小。</param>
        /// <param name="pageIndex">页码。</param>
        /// <param name="searchCondition">高级搜索的查询条件。</param>
        /// <returns>专题下的媒体报道分页列表。</returns>
        public StaticPagedList<TopicNews> AdvancedSearch(int? topicId, int? nodeId, int? userPlateId, string keyword, int? pageSize, int? pageIndex, Dictionary<string, string> searchCondition)
        {
            // 执行查询
            IQueryable<TopicNews> query =
                from topicNews in _db.TopicNewss
                where topicNews.Locations != null
                orderby topicNews.CreateDate descending
                select topicNews;

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
                Expression<Func<TopicNews, bool>> nodeCondition = i => false;
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
            IEnumerable<TopicNews> topicNewss = GetTopicNewsInPage(query, pageIndex.Value, pageSize.Value, out totalCount);
            var pagedList = new StaticPagedList<TopicNews>(topicNewss, pageIndex.Value, pageSize.Value, totalCount);

            return pagedList;
        }

        private IEnumerable<TopicNews> GetTopicNewsInPage(IEnumerable<TopicNews> query, int pageIndex, int pageSize,
            out int totalCount)
        {
            IEnumerable<TopicNews> enumerable = query as TopicNews[] ?? query.ToArray();
            totalCount = enumerable.Count();
            IEnumerable<TopicNews> topicNewss = enumerable.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return topicNewss.ToList();
        }

        #region IDisposable 成员

        public void Dispose()
        {
            _db.Dispose();
        }

        #endregion
    }
}