using System.Web.Mvc;

using MediaResource.Web.Models;
using MediaResource.Web.Services;

namespace MediaResource.Web.Controllers
{
    public class UserPlateController : Controller
    {
        private readonly UserPlateService _userPlateService = new UserPlateService();

        // GET: UserPlate/Topic/5
        public ActionResult Topic(int id)
        {
            ViewBag.TopicId = Request["TopicId"];

            UserPlate userPlate = _userPlateService.Get(id);

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