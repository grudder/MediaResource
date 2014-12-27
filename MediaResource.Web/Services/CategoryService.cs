using System;
using System.Collections.Generic;
using System.Linq;

using MediaResource.Web.DataAccess;
using MediaResource.Web.Models;
using MediaResource.Web.Models.ViewModels;

namespace MediaResource.Web.Services
{
    public class CategoryService : IDisposable
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public Category Get(int? id)
        {
            return _db.Categorys.Find(id);
        }

        internal List<ZTreeNode> GetChildTreeNodesByObjectType(ObjectType objectType, int? categoryId, string actionName = "List")
        {
            if (categoryId == null)
            {
                int count = GetCountByCategory(objectType);
                var rootNode = new ZTreeNode
                {
                    Id = 0,
                    Name = "【分类】(" + count + ")",
                    Href = String.Format("/{0}/{1}/{2}", objectType, actionName, 0),
                    IsParent = true,
                    Open = true
                };

                return new List<ZTreeNode> { rootNode };
            }

            var query = from category in _db.Categorys
                        where category.ParentId == categoryId
                        && category.CategoryType == objectType
                        && category.Status == 1
                        orderby category.OrderNum
                        select new ZTreeNode
                        {
                            Id = category.Id,
                            Name = category.Name,
                            IsParent = true
                        };
            var treeNodes = query.ToList();
            foreach (ZTreeNode treeNode in treeNodes)
            {
                int count = GetCountByCategory(objectType, treeNode.Id);
                treeNode.Name += "(" + count + ")";
                treeNode.Href = String.Format("/{0}/{1}/{2}", objectType, actionName, treeNode.Id);
            }

            return treeNodes;
        }

        [Obsolete]
        public TreeNode GetTreeJsonByObjectType(ObjectType objectType, string actionName = "List")
        {
            int count = GetCountByCategory(objectType);

            string href = String.Format("/{0}/{1}/{2}", objectType, actionName, 0);
            var rootNode = new TreeNode
            {
                Id = 0,
                Text = "[分类](" + count + ")",
                Href = href,
                Nodes = new List<TreeNode>()
            };
            rootNode = BuildChildNode(objectType, rootNode, actionName);

            return rootNode;
        }

        [Obsolete]
        private TreeNode BuildChildNode(ObjectType objectType, TreeNode node, string actionName)
        {
            var categories = GetChildrenByObjectType(objectType, node.Id);
            foreach (Category category in categories)
            {
                int count = GetCountByCategory(objectType, category.Id);

                string href = String.Format("/{0}/{1}/{2}", objectType, actionName, category.Id);
                var childNode = new TreeNode
                {
                    Id = category.Id,
                    Text = category.Name + "(" + count + ")",
                    Href = href,
                    Nodes = new List<TreeNode>()
                };
                childNode = BuildChildNode(objectType, childNode, actionName);

                node.Nodes.Add(childNode);
            }

            return node;
        }

        [Obsolete]
        private IEnumerable<Category> GetChildrenByObjectType(ObjectType objectType, int parentId = 0)
        {
            var categorys = from category in _db.Categorys
                            where category.CategoryType == objectType
                            && category.ParentId == parentId
                            && category.Status == 1
                            orderby category.OrderNum
                            select category;
            return categorys.ToList();
        }

        /// <summary>
        /// 获取分类下的对象数量。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="categoryId">分类标识。</param>
        /// <returns>分类下的对象数量。</returns>
        private int GetCountByCategory(ObjectType objectType, int? categoryId = null)
        {
            int count = 0;
            switch (objectType)
            {
                case ObjectType.Film:
                    var films = from f in _db.Films
                                where f.Status == 1
                                && f.IsConverted == true
                                && f.ImagePath != null
                                && f.ImagePath != ""
                                select f;
                    if (categoryId != null)
                    {
                        films = films.Where(f => f.Category == categoryId
                                                 || f.CategoryEntity.CategoryNum.Contains(categoryId + "_"));
                    }
                    count = films.Count();
                    break;

                case ObjectType.Graphic:
                    var graphics = from g in _db.Graphics
                                   where g.Status == 1
                                   && g.PreviewPath != null
                                   && g.PreviewPath != ""
                                   select g;
                    if (categoryId != null)
                    {
                        graphics = graphics.Where(g => g.Category == categoryId
                                                 || g.CategoryEntity.CategoryNum.Contains(categoryId + "_"));
                    }
                    count = graphics.Count();
                    break;

                case ObjectType.Music:
                    var musics = from m in _db.Musics
                                 where m.Status == 1
                                 && m.FileUrl != null
                                 select m;
                    if (categoryId != null)
                    {
                        musics = musics.Where(m => m.Category == categoryId
                                                 || m.CategoryEntity.CategoryNum.Contains(categoryId + "_"));
                    }
                    count = musics.Count();
                    break;

                case ObjectType.News:
                    var newss = from n in _db.Newss
                                where n.Status == 1
                                && n.IsConverted == true
                                && n.ImagePath != null
                                && n.ImagePath != ""
                                select n;
                    if (categoryId != null)
                    {
                        newss = newss.Where(n => n.Category == categoryId
                                                 || n.CategoryEntity.CategoryNum.Contains(categoryId + "_"));
                    }
                    count = newss.Count();
                    break;

                case ObjectType.Photo:
                    var photos = from p in _db.Photos
                                 where p.Status == 1
                                 && p.FileUrl != null
                                 && p.FileUrl != ""
                                 select p;
                    if (categoryId != null)
                    {
                        photos = photos.Where(p => p.Category == categoryId
                                                 || p.CategoryEntity.CategoryNum.Contains(categoryId + "_"));
                    }
                    count = photos.Count();
                    break;

                case ObjectType.Video:
                    var videos = from v in _db.Videos
                                 where v.Status == 1
                                 && v.PreviewPath != null
                                 && v.PreviewPath != ""
                                 select v;
                    if (categoryId != null)
                    {
                        videos = videos.Where(v => v.Category == categoryId
                                                 || v.CategoryEntity.CategoryNum.Contains(categoryId + "_"));
                    }
                    count = videos.Count();
                    break;

                case ObjectType.GraphicDesign:
                    var graphicDesigns = from gd in _db.GraphicDesigns
                                         where gd.Status == 1
                                         && gd.PreviewPath != null
                                         && gd.PreviewPath != ""
                                         select gd;
                    if (categoryId != null)
                    {
                        graphicDesigns = graphicDesigns.Where(gd => gd.Category == categoryId
                                                 || gd.CategoryEntity.CategoryNum.Contains(categoryId + "_"));
                    }
                    count = graphicDesigns.Count();
                    break;
            }

            return count;
        }

        [Obsolete]
        public List<int?> GetChildCategoryIds(ObjectType objectType, int? categoryId = null)
        {
            var categoryIds = new List<int?> { categoryId };
            List<int?> childCategoryIds = GetChildCategoryIds(objectType, categoryIds);
            return childCategoryIds;
        }

        [Obsolete]
        private List<int?> GetChildCategoryIds(ObjectType objectType, IEnumerable<int?> categoryIds)
        {
            var allCategoryIds = new List<int?>();
            foreach (int? categoryId in categoryIds)
            {
                allCategoryIds.Add(categoryId);

                int? theCategoryId = categoryId;
                var query = from category in _db.Categorys
                            where category.ParentId == theCategoryId
                            && category.CategoryType == objectType
                            && category.Status == 1
                            orderby category.OrderNum
                            select (int?)category.Id;
                var childCategoryIds = GetChildCategoryIds(objectType, query.ToList());

                allCategoryIds.AddRange(childCategoryIds);
            }
            return allCategoryIds.ToList();
        }

        #region IDisposable 成员

        public void Dispose()
        {
            _db.Dispose();
        }

        #endregion
    }
}