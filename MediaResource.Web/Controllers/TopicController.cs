using System.Collections.Generic;
using System.Web.Mvc;

using MediaResource.Web.Models;
using MediaResource.Web.Services;

namespace MediaResource.Web.Controllers
{
    public class TopicController : Controller
    {
        private readonly TopicService _topicService = new TopicService();
        private readonly UserPlateService _userPlateService = new UserPlateService();

        // GET: Topic
        public ActionResult Index()
        {
            var map = _topicService.GetOfficesTopicsMap();
            return View(map);
        }

        // GET: Topic/Detail/5
        public ActionResult Detail(int id)
        {
            ViewBag.TopicId = id;

            List<UserPlate> userPlates = _userPlateService.GetUserPlatesByTopicId(id);
            ViewBag.UserPlates = userPlates;

            Topic topic = _topicService.Get(id);
            return View(topic);
        }

        [ChildActionOnly]
        // GET: Topic/SearchPartial/5
        public ActionResult TopicSearchPartial(int id)
        {
            Topic topic = _topicService.Get(id);
            return PartialView("_TopicSearchPartial", topic);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _topicService.Dispose();
                _userPlateService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}