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
    public class TopicTextService : IDisposable
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private readonly NodeService _nodeService = new NodeService();
        private readonly UserPlateService _userPlateService = new UserPlateService();

        public TopicText Get(int? id)
        {
            return _db.TopicTexts.Find(id);
        }

        public TopicText View(int? id)
        {
            TopicText topicText = _db.TopicTexts.Find(id);
            topicText.ClickCount = (topicText.ClickCount == null) ? 1 : topicText.ClickCount + 1;

            _db.Entry(topicText).State = EntityState.Modified;
            _db.SaveChanges();

            return topicText;
        }

        /// <summary>
        /// 获取专题下的文字资料分页列表。
        /// </summary>
        /// <param name="topicId">专题编号。</param>
        /// <param name="nodeId">节点标识。</param>
        /// <param name="userPlateId">板块标识。</param>
        /// <param name="keyword">关键字。</param>
        /// <param name="pageSize">分页大小。</param>
        /// <param name="pageIndex">页码。</param>
        /// <returns>专题下的文字资料分页列表。</returns>
        public StaticPagedList<TopicText> AdvancedSearch(int? topicId, int? nodeId, int? userPlateId, string keyword, int? pageSize, int? pageIndex)
        {
            // 执行查询
            IQueryable<TopicText> query =
                from topicText in _db.TopicTexts
                where topicText.Locations != null
                orderby topicText.OrderNum descending
                select topicText;

            // 构造查询条件
            if (topicId != null)
            {
                query = query.Where(i => i.TopicId == topicId);
            }
            if (nodeId != null)
            {
                List<Node> nodes = _nodeService.GetChildNodes(nodeId.Value);
                Expression<Func<TopicText, bool>> nodeCondition = i => false;
                foreach (Node node in nodes)
                {
                    var theNode = node;
                    nodeCondition = nodeCondition.Or(i => ("|" + i.Nodes + "|").Contains("|" + theNode.NodeName + "|"));
                }
                query = query.Where(nodeCondition);
            }
            if (userPlateId != null)
            {
                UserPlate userPlate = _userPlateService.Get(userPlateId);
                query = query.Where(i => ("|" + i.Nodes + "|").Contains("|" + userPlate.PlateName + "|"));
            }
            if (!String.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(i => i.Name.Contains(keyword) || i.KeyWords.Contains(keyword) || i.Summary.Contains(keyword));
            }

            // 进行静态分页处理
            pageSize = (pageSize ?? 8);
            pageIndex = (pageIndex ?? 1);
            int totalCount;
            IEnumerable<TopicText> topicTexts = GetTopicTextInPage(query, pageIndex.Value, pageSize.Value, out totalCount);
            var pagedList = new StaticPagedList<TopicText>(topicTexts, pageIndex.Value, pageSize.Value, totalCount);

            return pagedList;
        }

        private IEnumerable<TopicText> GetTopicTextInPage(IEnumerable<TopicText> query, int pageIndex, int pageSize,
            out int totalCount)
        {
            IEnumerable<TopicText> enumerable = query as TopicText[] ?? query.ToArray();
            totalCount = enumerable.Count();
            IEnumerable<TopicText> topicTexts = enumerable.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return topicTexts.ToList();
        }

        #region IDisposable 成员

        public void Dispose()
        {
            _db.Dispose();
        }

        #endregion
    }
}