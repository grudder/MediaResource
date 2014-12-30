﻿using System;
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
        /// <returns>专题下的媒体报道分页列表。</returns>
        public StaticPagedList<TopicNews> AdvancedSearch(int? topicId, int? nodeId, int? userPlateId, string keyword, int? pageSize, int? pageIndex)
        {
            // 执行查询
            IQueryable<TopicNews> query =
                from topicNews in _db.TopicNewss
                where topicNews.Locations != null
                orderby topicNews.OrderNum descending
                select topicNews;

            // 构造查询条件
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