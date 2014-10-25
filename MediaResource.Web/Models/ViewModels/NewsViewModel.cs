using System;
using System.Collections.Generic;

using MediaResource.Web.Helper;

namespace MediaResource.Web.Models.ViewModels
{
	public class NewsViewModel
	{
		public NewsViewModel(News news)
		{
			News = news;
		}

		public News News
		{
			get;
			private set;
		}

		public object FlvUrl
		{
			get
            {
                return String.IsNullOrEmpty(News.FlvPath) ? "#" : WebHelper.Instance.RootUrl + News.FlvPath;
			}
		}

		public string ImageUrl
		{
			get
            {
                return String.IsNullOrEmpty(News.ImagePath) ? "#" : WebHelper.Instance.RootUrl + News.ImagePath;
			}
		}

		public IList<string> ThubminalImages
		{
			get
			{
				IList<string> thubminalImages = new List<string>();
                if (News.ImagesCount == null || string.IsNullOrEmpty(News.ImagePath))
				{
					return thubminalImages;
				}

				if (News.ImagePath.LastIndexOf('.') == -1)
				{
					return thubminalImages;
				}

				for (int i = 0; i < News.ImagesCount; ++i)
				{
					string imageUrl = ImageHelper.GetSnapUrl(News.ImagePath, i);
					thubminalImages.Add(imageUrl);
				}

				return thubminalImages;
			}
		}
	}
}