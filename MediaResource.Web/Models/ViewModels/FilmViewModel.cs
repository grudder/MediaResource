using System;
using System.Collections.Generic;
using System.IO;
using MediaResource.Web.Helper;

namespace MediaResource.Web.Models.ViewModels
{
	public class FilmViewModel
	{
		public FilmViewModel(Film film)
		{
			Film = film;
		}

		public Film Film
		{
			get;
			private set;
		}

		public object FlvUrl
		{
			get
			{
                return String.IsNullOrEmpty(Film.FlvPath) ? "#" : WebHelper.Instance.RootUrl + Film.FlvPath;
			}
		}

		public string ImageUrl
		{
			get
			{
                return String.IsNullOrEmpty(Film.ImagePath) ? "#" : WebHelper.Instance.RootUrl + Film.ImagePath;
			}
		}

		public IList<string> ThubminalImages
		{
			get
			{
				IList<string> thubminalImages = new List<string>();
				if (Film.ImagesCount == null || string.IsNullOrEmpty(Film.ImagePath))
				{
					return thubminalImages;
				}

				if (Film.ImagePath.LastIndexOf('.') == -1)
				{
					return thubminalImages;
				}

				for (int i = 0; i < Film.ImagesCount; ++i)
				{
					string imageUrl = ImageHelper.GetSnapUrl(Film.ImagePath, i);
					thubminalImages.Add(imageUrl);
				}

				return thubminalImages;
			}
		}
	}
}