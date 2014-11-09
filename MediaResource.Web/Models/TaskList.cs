using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaResource.Web.Models
{
	[Table("OA_TaskList")]
	[DisplayName("����")]
	public class TaskList
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id
		{
			get;
			set;
		}

		[Required]
		[Display(Name = "����", Order = 10)]
		[StringLength(256)]
		public string Name
		{
			get;
			set;
		}

		[Required]
		[Display(Name = "ʱ��", Order = 20)]
		[DisplayFormat(DataFormatString = "{0:yyyy/M/d H:mm:ss}", ApplyFormatInEditMode = true)]
		public DateTime TaskDate
		{
			get;
			set;
		}

		[Required]
		[Display(Name = "�ص�", Order = 30)]
		[StringLength(1024)]
		public string Location
		{
			get;
			set;
		}

		[Required]
		[Display(Name = "����", Order = 40)]
		[StringLength(50)]
		public string TaskListType
		{
			get;
			set;
		}

		[Required]
		[Display(Name = "����", Order = 50)]
		[StringLength(50)]
		public string Category
		{
			get;
			set;
		}

		[Required]
		[Display(Name = "�����쵼", Order = 60)]
		[StringLength(256)]
		public string Leader
		{
			get;
			set;
		}

		[Required]
		[Display(Name = "���봦��", Order = 70)]
		[StringLength(256)]
		public string Inoffices
		{
			get;
			set;
		}

		[Required]
		[Display(Name = "Ҫ��", Order = 80)]
		[DataType(DataType.MultilineText)]
		[StringLength(1024)]
		public string Demand
		{
			get;
			set;
		}

		[Required]
		[Display(Name = "��ϵ��", Order = 90)]
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

		[Display(Name = "�ύ��", Order = 100)]
		[ForeignKey("CreateBy")]
		public virtual User CreateByEntity
		{
			get;
			set;
		}

		[Display(Name = "�ύʱ��", Order = 110)]
		[DataType(DataType.DateTime)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
		public DateTime? CreateDate
		{
			get;
			set;
		}

		[Display(Name = "���")]
		[Column("TaskNO")]
		public int? TaskNo
		{
			get;
			set;
		}

		[Display(Name = "�ɳ���Ա")]
		[StringLength(1024)]
		public string Sent
		{
			get;
			set;
		}

		[Display(Name = "�������")]
		[StringLength(1024)]
		public string Feedback
		{
			get;
			set;
		}

		[Display(Name = "����״̬")]
		public bool IsApprove
		{
			get;
			set;
		}

		[Display(Name = "ȷ��״̬")]
		public bool IsConfirm
		{
			get;
			set;
		}
	}
}