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
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id
		{
			get;
			set;
        }

        [Display(Name = "节点", Order = 10)]
        public string Nodes
        {
            get;
            set;
        }

        [Display(Name = "排列顺序", Order = 20)]
        public int OrderNum
        {
            get;
            set;
        }

        [Display(Name = "关键字", Order = 30)]
		[StringLength(1024)]
        public string KeyWords
		{
			get;
			set;
        }

        [Display(Name = "时间", Order = 40)]
        public DateTime? TextDate
		{
			get;
			set;
		}

        [Display(Name = "人员", Order = 50)]
        [StringLength(1024)]
        public string Staff
        {
            get;
            set;
        }

        [Display(Name = "概要", Order = 60)]
        [StringLength(1024)]
        public string Summary
        {
            get;
            set;
        }

        [Display(Name = "出处", Order = 70)]
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

        [Display(Name = "创建时间", Order = 50)]
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

        [Display(Name = "创建人", Order = 100)]
        [ForeignKey("CreateBy")]
        public virtual User CreateByEntity
        {
            get;
            set;
        }

        [Display(Name = "名称", Order = 110)]
        [StringLength(512)]
        public string Name
        {
            get;
            set;
        }

        public int? TopicId
        {
            get;
            set;
        }

        [Display(Name = "所属专题", Order = 120)]
        [ForeignKey("TopicId")]
        public virtual Topic Topic
        {
            get;
            set;
        }
	}
}