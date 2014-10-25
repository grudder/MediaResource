using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaResource.Web.Models
{
	[Table("OA_GraphicDesign")]
	[DisplayName("图文设计成品")]
    public class GraphicDesign
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id
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

		[Display(Name = "名称", Order = 10)]
		[StringLength(512)]
		public string Name
		{
			get;
			set;
		}

        [Display(Name = "关键字", Order = 20)]
        [StringLength(1024)]
        public string Keyword
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

        [StringLength(1024)]
        public string PreviewPath
        {
            get;
            set;
        }

        [Display(Name = "页数", Order = 30)]
        [StringLength(512)]
        public string PageNum
        {
            get;
            set;
        }

        public int? Status
        {
            get;
            set;
        }

        [Display(Name = "关联处室", Order = 40)]
        [StringLength(1024)]
        public string Associate
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

        [StringLength(1024)]
        public string Num
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

		[Display(Name = "描述", Order = 70)]
		[StringLength(2048)]
		public string Description
		{
			get;
			set;
		}

		[Display(Name = "点击次数", Order = 80)]
		public int? ClickCount
		{
			get;
			set;
		}

		[Display(Name = "下载次数", Order = 90)]
		public int? DownloadCount
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