using System;
using System.Collections.Generic;

using MediaResource.Web.Helper;

namespace MediaResource.Web.Models.ViewModels
{
	public class TopicVideoViewModel
	{
		public TopicVideoViewModel(TopicVideo topicVideo)
		{
			TopicVideo = topicVideo;
		}

		public TopicVideo TopicVideo
		{
			get;
			private set;
		}

		public object FlvUrl
		{
			get
            {
                return String.IsNullOrEmpty(TopicVideo.FlvPath) ? "#" : WebHelper.Instance.RootUrl + TopicVideo.FlvPath;
			}
		}

		public string ImageUrl
		{
			get
            {
                return String.IsNullOrEmpty(TopicVideo.ImagePath) ? "#" : WebHelper.Instance.RootUrl + TopicVideo.ImagePath;
			}
		}

		public IList<string> ThubminalImages
		{
			get
			{
				IList<string> thubminalImages = new List<string>();
                if (TopicVideo.ImagesCount == null || string.IsNullOrEmpty(TopicVideo.ImagePath))
				{
					return thubminalImages;
				}

				if (TopicVideo.ImagePath.LastIndexOf('.') == -1)
				{
					return thubminalImages;
				}

				for (int i = 0; i < TopicVideo.ImagesCount; ++i)
				{
					string imageUrl = ImageHelper.GetSnapUrl(TopicVideo.ImagePath, i);
					thubminalImages.Add(imageUrl);
				}

				return thubminalImages;
			}
		}
	}
}