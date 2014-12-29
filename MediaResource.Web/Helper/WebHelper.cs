using System.Configuration;
using System.Web;

using MediaResource.Web.Models;
using MediaResource.Web.Services;

namespace MediaResource.Web.Helper
{
	public class WebHelper
	{
		private static WebHelper _instance;
		private readonly UserService _userService = new UserService();

		private WebHelper()
		{
		}

		public static WebHelper Instance
		{
			get
			{
				return _instance ?? (_instance = new WebHelper());
			}
		}

		public User CurrentUser
		{
			get
			{
				string name = HttpContext.Current.User.Identity.Name;
				User user = _userService.GetValidUserByName(name);

				HttpContext.Current.Session["User"] = user;
				return user;
			}
		}

		public string RootUrl
		{
			get
			{
				return ConfigurationManager.AppSettings["RootUrl"];
			}
		}

        public string RootPath
        {
            get
            {
                return ConfigurationManager.AppSettings["RootPath"];
            }
        }

        public string RootPath1
        {
            get
            {
                return ConfigurationManager.AppSettings["RootPath1"];
            }
        }

        public string TopicImageRootUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["TopicImageRootUrl"];
            }
        }

        public string MessageQueuePath
        {
            get
            {
                return ConfigurationManager.AppSettings["MessageQueuePath"];
            }
        }
	}
}