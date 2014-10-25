using System;
using System.Data.Entity;
using System.Linq;

using MediaResource.Web.DataAccess;
using MediaResource.Web.Models;
using PagedList;

namespace MediaResource.Web.Services
{
	public class UserService : IDisposable
	{
		private readonly ApplicationDbContext _db = new ApplicationDbContext();

		public User Find(int? id)
		{
			return _db.Users.Find(id);
		}

		public void Update(User user)
		{
			_db.Entry(user).State = EntityState.Modified;
			_db.SaveChanges();
		}

		public User GetValidUserByName(string name)
		{
			var users = from u in _db.Users
						where u.Name == name
						&& u.IsApproved
						&& !u.Locked
						select u;
			User user = users.SingleOrDefault();

			return user;
		}

		internal IPagedList<User> GetPagedList(int? pageSize, int? page)
		{
			var comments = from user in _db.Users
						   where user.IsApproved
						   && user.IsDisplay
						   orderby user.OrderNum
						   select user;

			pageSize = pageSize ?? 10;
			page = (page ?? 1);
			return comments.ToPagedList(page.Value, pageSize.Value);
		}

		#region IDisposable 成员

		public void Dispose()
		{
			_db.Dispose();
		}

		#endregion
	}
}