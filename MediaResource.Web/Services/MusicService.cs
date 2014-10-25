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
    public class MusicService : IDisposable
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public Music View(int? id)
        {
            Music music = _db.Musics.Find(id);
            music.ClickCount = (music.ClickCount == null) ? 1 : music.ClickCount + 1;

            _db.Entry(music).State = EntityState.Modified;
            _db.SaveChanges();

            return music;
        }

        public List<Music> GetTopList(int count)
        {
            var groups = from music in _db.Musics
                         where music.Status == 1
                         orderby music.CreateDate descending
                         select music;
            return groups.Take(count).ToList();
        }

        public IPagedList<Music> GetByCategory(int? categoryId, int? pageSize, int? pageIndex)
        {
            // 构造分类查询条件
            List<int?> childCategoryIds = new CategoryService().GetChildCategoryIds(ObjectType.Music, categoryId);
            Expression<Func<Music, bool>> condition = (i => false);
            condition = childCategoryIds.Aggregate(
                condition, (current, childCategoryId) =>
                    current.Or(i => i.Category == childCategoryId));

            // 执行查询
            var query = _db.Musics
                .Where(music => music.Status == 1)
                .Where(condition.Compile())
                .OrderByDescending(music => music.CreateDate);

            // 分页处理
            pageSize = (pageSize ?? 20);
            pageIndex = (pageIndex ?? 1);
            var pagedList = query.ToPagedList(pageIndex.Value, pageSize.Value);

            return pagedList;
        }

        public IPagedList<Music> Search(string keyword, int? pageSize, int? pageIndex)
        {
            // 执行查询
            var query = from music in _db.Musics
                        where music.Status == 1
                        && music.Title.Contains(keyword)
                        orderby music.CreateDate descending
                        select music;

            // 分页处理
            pageSize = (pageSize ?? 20);
            pageIndex = (pageIndex ?? 1);
            var pagedList = query.ToPagedList(pageIndex.Value, pageSize.Value);

            return pagedList;
        }

        public void PostScore(int musicId, double scoreValue, int scoreCount)
        {
            Music music = _db.Musics.Find(musicId);
            music.Score = scoreValue;
            music.ScoreCount = scoreCount;
            _db.Entry(music).State = EntityState.Modified;
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