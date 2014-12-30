using System.Web.Mvc;

using MediaResource.Web.Models;
using MediaResource.Web.Services;

namespace MediaResource.Web.Controllers
{
    [Authorize]
    public class UserPlateController : Controller
    {
        private readonly UserPlateService _userPlateService = new UserPlateService();

        // GET: UserPlate/Topic?topicId=1&userPlateId=2
        public ActionResult Topic(int topicId, int userPlateId)
        {
            ViewBag.TopicId = topicId;
            ViewBag.UserPlateId = userPlateId;

            UserPlate userPlate = _userPlateService.Get(userPlateId);

            return View(userPlate);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _userPlateService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}