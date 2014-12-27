using System;
using System.Collections.Generic;
using System.Linq;

using MediaResource.Web.DataAccess;
using MediaResource.Web.Models;
using MediaResource.Web.Models.ViewModels;

namespace MediaResource.Web.Services
{
    public class NodeService : IDisposable
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public List<Node> GetChildNodes(int nodeId)
        {
            var query = from node in _db.Nodes
                        where node.IsDisplay == true
                        && node.Id == nodeId
                        select node;

            var allNodes = GetChildNodes(query.ToList());
            return allNodes.ToList();
        }

        private List<Node> GetChildNodes(List<Node> nodes)
        {
            var allNodes = new List<Node>();
            foreach (Node node in nodes)
            {
                allNodes.Add(node);

                Node theNode = node;
                var query = from n in _db.Nodes
                            where n.IsDisplay == true
                            && n.ParentId == theNode.Id
                            orderby n.OrderNum descending
                            select n;
                var childCategoryIds = GetChildNodes(query.ToList());

                allNodes.AddRange(childCategoryIds);
            }
            return allNodes.ToList();
        }

        public List<ZTreeNode> GetChildTreeNodesByTopic(int topicId, int? parentNodeId)
        {
            if (parentNodeId == null)
            {
                var rootNode = new ZTreeNode
                {
                    Id = 0,
                    Name = "【节点】",
                    Href = String.Format("/Node?topicId={0}", topicId),
                    IsParent = true,
                    Open = true
                };

                return new List<ZTreeNode> { rootNode };
            }

            var query = from node in _db.Nodes
                        where node.IsDisplay == true
                        && node.TopicId == topicId
                        && node.ParentId == parentNodeId
                        orderby node.OrderNum descending
                        select new ZTreeNode
                        {
                            Id = node.Id,
                            Name = node.NodeName,
                            IsParent = true
                        };
            var treeNodes = query.ToList();
            foreach (ZTreeNode treeNode in treeNodes)
            {
                treeNode.Href = String.Format("/Node?nodeId={0}", treeNode.Id);
            }

            return treeNodes;
        }

        #region IDisposable 成员

        public void Dispose()
        {
            _db.Dispose();
        }

        #endregion
    }
}