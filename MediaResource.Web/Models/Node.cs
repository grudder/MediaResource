using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaResource.Web.Models
{
    [Table("OA_Node")]
	[DisplayName("节点")]
    public class Node
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id
		{
			get;
			set;
		}

        [Display(Name = "节点名称", Order = 10)]
		[StringLength(256)]
        public string NodeName
		{
			get;
			set;
        }

        public int? TopicId
        {
            get;
            set;
        }

        [Display(Name = "所属专题", Order = 20)]
        [ForeignKey("TopicId")]
        public virtual Topic Topic
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

        [Display(Name = "创建人", Order = 40)]
        [ForeignKey("CreateBy")]
        public virtual User CreateByEntity
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

        [Display(Name = "排列顺序", Order = 60)]
        public int OrderNum
        {
            get;
            set;
        }

        public int ParentId
        {
            get;
            set;
        }

        [ForeignKey("ParentId")]
        public virtual Node ParentNode
        {
            get;
            set;
        }

        public int NodeLevel
        {
            get;
            set;
        }

        public string Remark
        {
            get;
            set;
        }
	}
}