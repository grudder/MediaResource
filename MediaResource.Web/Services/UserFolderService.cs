using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using MediaResource.Web.DataAccess;
using MediaResource.Web.Models;

namespace MediaResource.Web.Services
{
	public class UserFolderService : IDisposable
	{
		private readonly ApplicationDbContext _db = new ApplicationDbContext();

		public UserFolder Find(int? id)
		{
			return _db.UserFolders.Find(id);
		}

		public List<UserFolder> GetList(ObjectType objectType, int userId)
		{
			var userFolders = from userFolder in _db.UserFolders
							  where userFolder.ObjectType == objectType
							  && userFolder.CreateBy == userId
							  orderby userFolder.CreateDate
							  select userFolder;

			// 如果用户没有“默认目录”，则创建之
			if (!userFolders.Any(i => i.Name == "默认目录"))
			{
				var userFolder = new UserFolder
				{
					Name = "默认目录",
					ObjectType = objectType,
					CreateBy = userId,
					CreateDate = DateTime.Now
				};
				Create(userFolder);
			}

			return userFolders.ToList();
		}

		public bool IsUsed(UserFolder userFolder)
		{
			if (userFolder.ObjectType == ObjectType.UserPhoto)
			{
				return _db.UserPhotos.Any(i => i.FolderId == userFolder.Id);
			}
            if (userFolder.ObjectType == ObjectType.UserVideo)
			{
				return _db.UserVideos.Any(i => i.FolderId == userFolder.Id);
			}

			return false;
		}

		public void Create(UserFolder userFolder)
		{
			_db.UserFolders.Add(userFolder);
			_db.SaveChanges();
		}

		public void Update(UserFolder userFolder)
		{
			_db.Entry(userFolder).State = EntityState.Modified;
			_db.SaveChanges();
		}

		public void Delete(UserFolder userFolder)
		{
			_db.UserFolders.Remove(userFolder);
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