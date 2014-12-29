using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaResource.Web.Models
{
    [Table("OA_TopicVideo")]
	[DisplayName("专题视频")]
    public class TopicVideo
    {
        [Display(Name = "ID　　号", Order = 0)]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id
		{
			get;
			set;
        }

        public int? TopicId
        {
            get;
            set;
        }

        [Display(Name = "所属专题")]
        [ForeignKey("TopicId")]
        public virtual Topic Topic
        {
            get;
            set;
        }

        [Display(Name = "排序号")]
        public int OrderNum
        {
            get;
            set;
        }

        [Display(Name = "名　　称", Order = 10)]
        [StringLength(512)]
        public string Name
        {
            get;
            set;
        }

        [Display(Name = "关 键 字", Order = 20)]
        [StringLength(1024)]
        public string KeyWords
        {
            get;
            set;
        }

        [Display(Name = "隶属节点", Order = 30)]
        public string Nodes
        {
            get;
            set;
        }

        [Display(Name = "时　　间", Order = 40)]
        public DateTime? TextDate
		{
			get;
			set;
		}

        [Display(Name = "人　　员", Order = 50)]
        [StringLength(1024)]
        public string Staff
        {
            get;
            set;
        }

        [Display(Name = "概　　要", Order = 60)]
        [StringLength(1024)]
        public string Summary
        {
            get;
            set;
        }

        [Display(Name = "出　　处", Order = 70)]
        [StringLength(1024)]
        public string Source
        {
            get;
            set;
        }

        [Display(Name = "文件位置", Order = 80)]
        [StringLength(1024)]
        public string Locations
        {
            get;
            set;
        }

        public int? CreateBy
        {
            get;
            set;
        }

        [Display(Name = "提 交 人", Order = 80)]
        [ForeignKey("CreateBy")]
        public virtual User CreateByEntity
        {
            get;
            set;
        }

        [Display(Name = "提交时间", Order = 90)]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? CreateDate
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

        [Display(Name = "点击次数", Order = 100)]
        public int ClickCount
        {
            get;
            set;
        }

        [Display(Name = "下载次数", Order = 110)]
        public int DownloadCount
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