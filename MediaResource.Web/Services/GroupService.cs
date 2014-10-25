using System;
using System.Collections.Generic;
using System.Linq;

using MediaResource.Web.DataAccess;
using MediaResource.Web.Models;

namespace MediaResource.Web.Services
{
	public class GroupService : IDisposable
	{
		private readonly ApplicationDbContext _db = new ApplicationDbContext();

		public Group Find(int? id)
		{
			return _db.Groups.Find(id);
		}

        public List<Group> GetTopVisibleList(int count)
		{
			var groups = from g in _db.Groups
						 where g.IsDisplay
						 select g;

            if (count == -1)
            {
                return groups.ToList();
            }
            return groups.Take(count).ToList();
		}

		#region IDisposable 成员

		public void Dispose()
		{
			_db.Dispose();
		}

		#endregion
	}
}