using System;
using System.Collections.Generic;
using System.Linq;

using MediaResource.Web.DataAccess;
using MediaResource.Web.Helper;
using MediaResource.Web.Models;

namespace MediaResource.Web.Services
{
    public class TopicService : IDisposable
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public Topic Get(int? id)
        {
            return _db.Topics.Find(id);
        }

        /// <summary>
        /// 获取专题与主办处室的映射。
        /// </summary>
        /// <returns>专题与主办处室的映射。</returns>
        public Dictionary<string, List<Topic>> GetOfficesTopicsMap()
        {
            // 获取所有主办处室
            var query = from topic in _db.Topics
                        where topic.IsDisplay == true
                        select topic.Offices;

            // 获取主办处室下的专题
            var map = new Dictionary<string, List<Topic>>();
            foreach (string offices in query.Distinct().ToArray())
            {
                List<Topic> topics = GetTopicsByOffices(offices);
                map.Add(offices, topics);
            }

            return map;
        }

        /// <summary>
        /// 获取主办处室的专题列表。
        /// </summary>
        /// <param name="offices">主办处室。</param>
        /// <returns>主办处室的专题列表。</returns>
        private List<Topic> GetTopicsByOffices(string offices)
        {
            var query = from topic in _db.Topics
                        where topic.IsDisplay == true
                        && topic.Offices == offices
                        orderby topic.OrderNum descending
                        select topic;
            return query.ToList();
        }

        public bool IsAuthorized(int topicId, User user)
		{
#if DEBUG
	        return true;
#endif
			string groupName = user.GroupEntity.Name;
            if (groupName == "测试用组" || groupName == "发改委记者站" || groupName == "中心领导")
            {
                return true;
            }

            Topic topic = Get(topicId);
			bool isAuthorized = topic.Offices.Contains(user.GroupEntity.Name);
			return isAuthorized;
        }

        #region IDisposable 成员

        public void Dispose()
        {
            _db.Dispose();
        }

        #endregion
    }
}