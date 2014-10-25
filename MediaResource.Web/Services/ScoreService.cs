using System;
using System.Linq;

using MediaResource.Web.DataAccess;
using MediaResource.Web.Models;

namespace MediaResource.Web.Services
{
	public class ScoreService : IDisposable
	{
		private readonly ApplicationDbContext _db = new ApplicationDbContext();

		public void Create(Score score)
		{
			_db.Scores.Add(score);
			_db.SaveChanges();
		}

		public bool ScoreExists(ObjectType objectType, int objectId, User createBy)
		{
			var scores = from score in _db.Scores
						 where score.ObjectType == objectType
						 && score.ObjectId == objectId
						 && score.CreateBy == createBy.Id
						 select score;
			return scores.Any();
		}

		public double GetAverageValue(ObjectType objectType, int objectId)
		{
			var scores = from score in _db.Scores
						 where score.ObjectType == objectType
						 && score.ObjectId == objectId
						 select score;
			return scores.Average(score => score.Value);
		}

		public int GetCount(ObjectType objectType, int objectId)
		{
			var scores = from score in _db.Scores
						 where score.ObjectType == objectType
						 && score.ObjectId == objectId
						 select score;
			return scores.Count();
		}

		#region IDisposable 成员

		public void Dispose()
		{
			_db.Dispose();
		}

		#endregion
	}
}