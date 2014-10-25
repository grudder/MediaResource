using System;
using System.ComponentModel.DataAnnotations;

namespace MediaResource.Web.Models.ViewModels
{
	public class ImageViewModel
	{
		public int Id
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

        public string RawUrl
        {
            get;
            set;
        }

		public string FileUrl
		{
			get;
			set;
		}

		[DataType(DataType.DateTime)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
		public DateTime? CreateDate
		{
			get;
			set;
		}

        public double Score
        {
            get;
            set;
        }

        public ObjectType ObjectType
        {
            get;
            set;
        }
	}
}