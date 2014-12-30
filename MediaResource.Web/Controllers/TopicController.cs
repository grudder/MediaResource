using System.Collections.Generic;
using System.Web.Mvc;

using MediaResource.Web.Helper;
using MediaResource.Web.Models;
using MediaResource.Web.Services;

namespace MediaResource.Web.Controllers
{
    [Authorize]
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

        // POST: Topic/IsAuthorized
        [HttpPost]
        public JsonResult IsAuthorized(int id)
        {
            bool isAuthorized = _topicService.IsAuthorized(id, WebHelper.Instance.CurrentUser);

            return Json(new
            {
                result = isAuthorized
            });
        }

        // GET: Topic/Detail/5
        public ActionResult Detail(int id)
        {
            bool isAuthorized = _topicService.IsAuthorized(id, WebHelper.Instance.CurrentUser);
            if (!isAuthorized)
            {
                return RedirectToAction("Index");
            }

            ViewBag.TopicId = id;

            List<UserPlate> userPlates = _userPlateService.GetUserPlatesByTopicId(id);
            ViewBag.UserPlates = userPlates;

            Topic topic = _topicService.Get(id);
            return View(topic);
        }

        //
        // GET: Topico/Search/5?keyword=xxx
        public ActionResult Search(int id, string keyword)
        {
            ViewBag.Keyword = keyword;

            Topic topic = _topicService.Get(id);
            return View(topic);
        }

        //
        // GET: Topico/SearchFrame/5?keyword=xxx
        public ActionResult SearchFrame(int id, string keyword)
        {
            ViewBag.TopicId = id;
            ViewBag.Keyword = keyword;

            return View();
        }

        [ChildActionOnly]
        // GET: Topic/SearchPartial/5?keyword=xxx
        public ActionResult TopicSearchPartial(int id, string keyword)
        {
            ViewBag.Keyword = keyword;

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