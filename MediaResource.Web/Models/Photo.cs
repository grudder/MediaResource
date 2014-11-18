using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaResource.Web.Models
{
	[Table("OA_Photo")]
	[DisplayName("照片")]
	public class Photo
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id
		{
			get;
			set;
		}

		[Display(Name = "名称", Order = 10)]
		[StringLength(256)]
		public string Name
		{
			get;
			set;
		}

		[Display(Name = "关键字", Order = 20)]
		[StringLength(256)]
		public string Keyword
		{
			get;
			set;
		}

		[Display(Name = "拍摄时间", Order = 30)]
		[DataType(DataType.DateTime)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime? RecordingTime
		{
			get;
			set;
		}

		[Display(Name = "拍摄地点", Order = 40)]
		[StringLength(1024)]
		public string Location
		{
			get;
			set;
		}

		[Display(Name = "拍摄人", Order = 50)]
		[StringLength(256)]
		public string Shooting
		{
			get;
			set;
		}

		[Display(Name = "参会领导", Order = 60)]
		[StringLength(256)]
		public string Leadership
		{
			get;
			set;
		}

		[Display(Name = "参会人员", Order = 70)]
		[StringLength(256)]
		public string Participants
		{
			get;
			set;
		}

		[Display(Name = "主办处室", Order = 80)]
		[StringLength(256)]
		public string Offices
		{
			get;
			set;
		}

		[Display(Name = "参与处室", Order = 90)]
		[StringLength(256)]
		public string Association
		{
			get;
			set;
		}

		[Display(Name = "上传时间", Order = 100)]
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

		[Display(Name = "上传人", Order = 110)]
		[ForeignKey("CreateBy")]
		public virtual User CreateByEntity
		{
			get;
			set;
		}

		[Display(Name = "点击次数", Order = 120)]
		public int? ClickCount
		{
			get;
			set;
		}

		[Display(Name = "下载次数", Order = 130)]
		public int? DownloadCount
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