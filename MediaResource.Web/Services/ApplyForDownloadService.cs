using System;

using MediaResource.Web.DataAccess;
using MediaResource.Web.Models;

namespace MediaResource.Web.Services
{
	public class ApplyForDownloadService : IDisposable
	{
		private readonly ApplicationDbContext _db = new ApplicationDbContext();

		public void Create(ApplyForDownload applyForDownload)
		{
			_db.ApplyForDownloads.Add(applyForDownload);
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
