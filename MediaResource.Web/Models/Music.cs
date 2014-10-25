using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaResource.Web.Models
{
	[Table("OA_Music")]
	[DisplayName("音乐")]
	public class Music
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id
		{
			get;
			set;
		}

		[Display(Name = "音乐名称", Order = 10)]
		[StringLength(256)]
		public string Title
		{
			get;
			set;
		}

		[Display(Name = "音乐长度", Order = 10)]
		[StringLength(256)]
		public string Length
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

		[Display(Name = "原声名称", Order = 40)]
		[StringLength(256)]
		public string Original
		{
			get;
			set;
		}

		[Display(Name = "描述", Order = 50)]
		[StringLength(1024)]
		public string Description
		{
			get;
			set;
		}

		[Display(Name = "关联处室", Order = 60)]
		[StringLength(256)]
		public string Association
		{
			get;
			set;
		}

		[Display(Name = "上传时间", Order = 70)]
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

		[Display(Name = "上传人", Order = 80)]
		[ForeignKey("CreateBy")]
		public virtual User CreateByEntity
		{
			get;
			set;
		}

		[Display(Name = "点击次数", Order = 90)]
		public int? ClickCount
		{
			get;
			set;
		}

		[Display(Name = "下载次数", Order = 100)]
		public int? DownloadCount
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

		[StringLength(1024)]
		public string FileUrl
		{
			get;
			set;
		}

		public int? Status
		{
			get;
			set;
		}

		[Display(Name = "类别", Order = 30)]
		[StringLength(50)]
		public string MusicType
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