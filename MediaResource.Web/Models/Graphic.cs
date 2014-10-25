using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaResource.Web.Models
{
	[Table("OA_Graphic")]
	[DisplayName("图片")]
	public class Graphic
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

		[Display(Name = "文件大小", Order = 20)]
		[StringLength(256)]
		public string FileSize
		{
			get;
			set;
		}

		[Display(Name = "文件格式", Order = 30)]
		[StringLength(256)]
		public string Format
		{
			get;
			set;
		}

		[Display(Name = "描述", Order = 40)]
		[StringLength(2048)]
		public string Description
		{
			get;
			set;
		}

		[Display(Name = "关联处室", Order = 50)]
		[StringLength(1024)]
		public string Association
		{
			get;
			set;
		}

		[Display(Name = "上传时间", Order = 60)]
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

		[Display(Name = "上传人", Order = 70)]
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

        [StringLength(1024)]
        public string PreviewPath
        {
            get;
            set;
        }
	}
}