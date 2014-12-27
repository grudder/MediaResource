using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaResource.Web.Models
{
	[Table("OA_Topic")]
	[DisplayName("专题")]
    public class Topic
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id
		{
			get;
			set;
		}

		[Display(Name = "排列顺序", Order = 10)]
        public int OrderNum
		{
			get;
			set;
		}

		[Display(Name = "名称", Order = 20)]
		[StringLength(256)]
        public string Name
		{
			get;
			set;
		}

		[Display(Name = "主办处室", Order = 30)]
		[StringLength(50)]
		public string Offices
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

        [Display(Name = "列表图片地址")]
		[StringLength(1024)]
        public string ListPhotoUrl
		{
			get;
			set;
		}

        [Display(Name = "详情图片地址")]
        [StringLength(1024)]
        public string DetailPhotoUrl
        {
            get;
            set;
        }

        [Display(Name = "是否显示")]
        public bool? IsDisplay
		{
			get;
			set;
        }

        public int? CreateBy
        {
            get;
            set;
        }

        [Display(Name = "创建人", Order = 50)]
        [ForeignKey("CreateBy")]
        public virtual User CreateByEntity
        {
            get;
            set;
        }

        [Display(Name = "创建时间", Order = 60)]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? CreateDate
        {
            get;
            set;
        }

        [Display(Name = "可见部门", Order = 70)]
        public string VisibleOffices
        {
            get;
            set;
        }
	}
}