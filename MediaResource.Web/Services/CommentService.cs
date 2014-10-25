using System;
using System.Linq;

using MediaResource.Web.DataAccess;
using MediaResource.Web.Models;

using PagedList;

namespace MediaResource.Web.Services
{
	public class CommentService : IDisposable
	{
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public Comment Find(int? id)
        {
            return _db.Comments.Find(id);
        }

		public void Create(Comment comment)
		{
			_db.Comments.Add(comment);
			_db.SaveChanges();
		}

        public void Delete(Comment comment)
        {
            _db.Comments.Remove(comment);
            _db.SaveChanges();
        }

		public int GetCount(ObjectType objectType, int objectId)
		{
			var comments = from comment in _db.Comments
						   where comment.ObjectType == objectType
						   && comment.ObjectId == objectId
						   select comment;
			return comments.Count();
		}

		public IPagedList<Comment> GetPagedList(ObjectType objectType, int objectId, int pageSize, int? pageIndex)
		{
			var comments = from comment in _db.Comments
						   where comment.ObjectType == objectType
						   && comment.ObjectId == objectId
						   orderby comment.CreateDate descending
						   select comment;

			int pageNumber = (pageIndex ?? 1);
			return comments.ToPagedList(pageNumber, pageSize);
		}

		#region IDisposable 成员

		public void Dispose()
		{
			_db.Dispose();
		}

		#endregion
	}
}