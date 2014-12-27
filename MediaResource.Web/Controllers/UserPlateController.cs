using System.Web.Mvc;

using MediaResource.Web.Models;
using MediaResource.Web.Services;

namespace MediaResource.Web.Controllers
{
    public class UserPlateController : Controller
    {
        private readonly UserPlateService _userPlateService = new UserPlateService();

        [ChildActionOnly]
        // GET: UserPlate/TopicPartial/5
        public ActionResult TopicPartial(int id)
        {
            ViewBag.TopicId = Request["TopicId"];

            UserPlate userPlate = _userPlateService.Get(id);

            return PartialView("_TopicPartial", userPlate);
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