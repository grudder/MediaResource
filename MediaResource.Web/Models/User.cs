using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaResource.Web.Models
{
	[Table("OA_User")]
	[DisplayName("用户")]
	[DisplayColumn("Name")]
	public class User
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id
		{
			get;
			set;
		}

		[Display(Name = "用户名", Order = 10)]
		[Required]
		[StringLength(50)]
		public string Name
		{
			get;
			set;
		}

		[Display(Name = "密码", Order = 20)]
		[Required]
		[StringLength(40)]
		[DataType(DataType.Password)]
		public string Password
		{
			get;
			set;
		}

		public int GroupId
		{
			get;
			set;
		}

		[Display(Name = "处室", Order = 30)]
		[ForeignKey("GroupId")]
		public virtual Group GroupEntity
		{
			get;
			set;
		}

		[Display(Name = "姓名", Order = 40)]
		[Required]
		[StringLength(50)]
		public string FullName
		{
			get;
			set;
		}

		[Display(Name = "固定电话", Order = 50)]
		[StringLength(50)]
		public string Ext
		{
			get;
			set;
		}

		[Display(Name = "手机", Order = 60)]
		[StringLength(50)]
		public string Mobile
		{
			get;
			set;
		}

		[Display(Name = "邮箱", Order = 70)]
		[StringLength(70)]
		[DataType(DataType.EmailAddress)]
		[EmailAddress]
		public string Email
		{
			get;
			set;
		}

		[Display(Name = "备注", Order = 80)]
		[StringLength(1000)]
		[DataType(DataType.MultilineText)]
		public string Remark
		{
			get;
			set;
		}

		[Display(Name = "是否锁定", Order = 90)]
		public bool Locked
		{
			get;
			set;
		}

		[Display(Name = "创建人", Order = 100)]
		public int CreateBy
		{
			get;
			set;
		}

		[Display(Name = "创建日期", Order = 110)]
		[DataType(DataType.DateTime)]
		public DateTime CreateDate
		{
			get;
			set;
		}

		[Display(Name = "IP地址", Order = 120)]
		[StringLength(50)]
		public string Ip
		{
			get;
			set;
		}

		[Display(Name = "是否审核", Order = 130)]
		public bool IsApproved
		{
			get;
			set;
		}

		[Display(Name = "审核日期", Order = 140)]
		public DateTime ApproveDate
		{
			get;
			set;
		}

		[Display(Name = "审核人", Order = 150)]
		public int Approver
		{
			get;
			set;
		}

		[Display(Name = "联系人", Order = 160)]
		[StringLength(50)]
		public string Contact
		{
			get;
			set;
		}

		[Display(Name = "是否显示", Order = 170)]
		public bool IsDisplay
		{
			get;
			set;
		}

		[Display(Name = "是否登录", Order = 180)]
		public bool IsLogin
		{
			get;
			set;
		}

		[Display(Name = "排列顺序", Order = 190)]
		public int OrderNum
		{
			get;
			set;
		}

		[Display(Name = "允许审核任务", Order = 200)]
		public bool? CanApproveTask
		{
			get;
			set;
		}
	}
}