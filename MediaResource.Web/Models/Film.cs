using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaResource.Web.Models
{
	[Table("OA_Film")]
	[DisplayName("电影")]
	public class Film
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id
		{
			get;
			set;
		}

		[Display(Name = "片名", Order = 10)]
		[StringLength(256)]
		public string Title
		{
			get;
			set;
		}

		[Display(Name = "片长", Order = 20)]
		[StringLength(256)]
		public string Length
		{
			get;
			set;
		}

		[Display(Name = "描述", Order = 30)]
		[StringLength(1024)]
		public string Description
		{
			get;
			set;
		}

		[Display(Name = "参与处室", Order = 40)]
		[StringLength(256)]
		public string Association
		{
			get;
			set;
		}

		public int? Category
		{
			get;
			set;
		}

		[ForeignKey("Category")]
		public virtual Category CategoryEntity
		{
			get;
			set;
		}

		[StringLength(1024)]
		public string Num
		{
			get;
			set;
		}

		[Display(Name = "上传时间", Order = 50)]
		[DataType(DataType.DateTime)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
		public DateTime? CreateDate
		{
			get;
			set;
		}

		public int? CreateBy
		{
			get;
			set;
		}

		[Display(Name = "上传人", Order = 60)]
		[ForeignKey("CreateBy")]
		public virtual User CreateByEntity
		{
			get;
			set;
		}

		[StringLength(1024)]
		public string FileUrl
		{
			get;
			set;
		}

		[Display(Name = "点击次数", Order = 70)]
		public int? ClickCount
		{
			get;
			set;
		}

		[Display(Name = "下载次数", Order = 80)]
		public int? DownloadCount
		{
			get;
			set;
		}

		public int? Status
		{
			get;
			set;
		}

		[StringLength(1024)]
		public string FlvPath
		{
			get;
			set;
		}

		[StringLength(1024)]
		public string ImagePath
		{
			get;
			set;
		}

		public bool? IsConverted
		{
			get;
			set;
		}

		public int? ImagesCount
		{
			get;
			set;
		}

		public double Score
		{
			get;
			set;
		}

		public int ScoreCount
		{
			get;
			set;
		}
	}
}