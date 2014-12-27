using System;
using System.Collections.Generic;
using System.Linq;

using MediaResource.Web.DataAccess;
using MediaResource.Web.Models;

namespace MediaResource.Web.Services
{
    public class UserPlateService : IDisposable
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public UserPlate Get(int? id)
        {
            return _db.UserPlates.Find(id);
        }

        /// <summary>
        /// 获取专题下的自建版块。
        /// </summary>
        /// <param name="topicId">专题编号。</param>
        /// <returns>专题下的自建版块列表。</returns>
        public List<UserPlate> GetUserPlatesByTopicId(int topicId)
        {
            var query = from node in _db.UserPlates
                        where node.IsDisplay == true
                        && node.TopicId == topicId
                        orderby node.OrderNum descending
                        select node;
            return query.ToList();
        }

        #region IDisposable 成员

        public void Dispose()
        {
            _db.Dispose();
        }

        #endregion
    }
}