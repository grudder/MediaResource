using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaResource.Web.Models
{
	[Table("OA_TaskList")]
	[DisplayName("任务单")]
	public class TaskList
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id
		{
			get;
			set;
		}

		[Required]
		[Display(Name = "名称", Order = 10)]
		[StringLength(256)]
		public string Name
		{
			get;
			set;
		}

		[Required]
		[Display(Name = "时间", Order = 20)]
		[DisplayFormat(DataFormatString = "{0:yyyy/M/d H:mm:ss}", ApplyFormatInEditMode = true)]
		public DateTime TaskDate
		{
			get;
			set;
		}

		[Required]
		[Display(Name = "地点", Order = 30)]
		[StringLength(1024)]
		public string Location
		{
			get;
			set;
		}

		[Required]
		[Display(Name = "类型", Order = 40)]
		[StringLength(50)]
		public string TaskListType
		{
			get;
			set;
		}

		[Required]
		[Display(Name = "分类", Order = 50)]
		[StringLength(50)]
		public string Category
		{
			get;
			set;
		}

		[Required]
		[Display(Name = "参与领导", Order = 60)]
		[StringLength(256)]
		public string Leader
		{
			get;
			set;
		}

		[Required]
		[Display(Name = "参与处室", Order = 70)]
		[StringLength(256)]
		public string Inoffices
		{
			get;
			set;
		}

		[Required]
		[Display(Name = "要求", Order = 80)]
		[DataType(DataType.MultilineText)]
		[StringLength(1024)]
		public string Demand
		{
			get;
			set;
		}

		[Required]
		[Display(Name = "联系人", Order = 90)]
		[StringLength(256)]
		public string Contact
		{
			get;
			set;
		}

		public int? CreateBy
		{
			get;
			set;
		}

		[Display(Name = "提交人", Order = 100)]
		[ForeignKey("CreateBy")]
		public virtual User CreateByEntity
		{
			get;
			set;
		}

		[Display(Name = "提交时间", Order = 110)]
		[DataType(DataType.DateTime)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
		public DateTime? CreateDate
		{
			get;
			set;
		}

		[Display(Name = "编号")]
		[Column("TaskNO")]
		public int? TaskNo
		{
			get;
			set;
		}

		[Display(Name = "派出人员")]
		[StringLength(1024)]
		public string Sent
		{
			get;
			set;
		}

		[Display(Name = "反馈意见")]
		[StringLength(1024)]
		public string Feedback
		{
			get;
			set;
		}

		[Display(Name = "审批状态")]
		public bool IsApprove
		{
			get;
			set;
		}

		[Display(Name = "确认状态")]
		public bool IsConfirm
		{
			get;
			set;
		}
	}
}