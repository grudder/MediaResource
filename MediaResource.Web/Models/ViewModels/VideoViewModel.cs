using System;
using System.Collections.Generic;

using MediaResource.Web.Helper;

namespace MediaResource.Web.Models.ViewModels
{
    public class VideoViewModel
    {
        public VideoViewModel(Video video)
        {
            Video = video;
        }

        public Video Video
        {
            get;
            private set;
        }

        public object FlvUrl
        {
            get
            {
                return String.IsNullOrEmpty(Video.FlvPath) ? "#" : WebHelper.Instance.RootUrl + Video.FlvPath;
            }
        }

        public string ImageUrl
        {
            get
            {
                return String.IsNullOrEmpty(Video.PreviewPath) ? "#" : WebHelper.Instance.RootUrl + Video.PreviewPath;
            }
        }

        public IList<string> PreviewImages
        {
            get
            {
                IList<string> previewImages = new List<string>();

                if (!String.IsNullOrEmpty(Video.PreviewPath))
                {
                    previewImages.Add(WebHelper.Instance.RootUrl + Video.PreviewPath);
                }
                if (!String.IsNullOrEmpty(Video.PreviewPath1))
                {
                    previewImages.Add(WebHelper.Instance.RootUrl + Video.PreviewPath1);
                }
                if (!String.IsNullOrEmpty(Video.PreviewPath2))
                {
                    previewImages.Add(WebHelper.Instance.RootUrl + Video.PreviewPath2);
                }
                if (!String.IsNullOrEmpty(Video.PreviewPath3))
                {
                    previewImages.Add(WebHelper.Instance.RootUrl + Video.PreviewPath3);
                }
                if (!String.IsNullOrEmpty(Video.PreviewPath4))
                {
                    previewImages.Add(WebHelper.Instance.RootUrl + Video.PreviewPath4);
                }
                if (!String.IsNullOrEmpty(Video.PreviewPath5))
                {
                    previewImages.Add(WebHelper.Instance.RootUrl + Video.PreviewPath5);
                }

                return previewImages;
            }
        }
    }
}