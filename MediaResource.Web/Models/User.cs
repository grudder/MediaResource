using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaResource.Web.Models
{
	[Table("OA_User")]
	[DisplayName("�û�")]
	[DisplayColumn("Name")]
	public class User
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id
		{
			get;
			set;
		}

		[Display(Name = "�û���", Order = 10)]
		[Required]
		[StringLength(50)]
		public string Name
		{
			get;
			set;
		}

		[Display(Name = "����", Order = 20)]
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

		[Display(Name = "����", Order = 30)]
		[ForeignKey("GroupId")]
		public virtual Group GroupEntity
		{
			get;
			set;
		}

		[Display(Name = "����", Order = 40)]
		[Required]
		[StringLength(50)]
		public string FullName
		{
			get;
			set;
		}

		[Display(Name = "�̶��绰", Order = 50)]
		[StringLength(50)]
		public string Ext
		{
			get;
			set;
		}

		[Display(Name = "�ֻ�", Order = 60)]
		[StringLength(50)]
		public string Mobile
		{
			get;
			set;
		}

		[Display(Name = "����", Order = 70)]
		[StringLength(70)]
		[DataType(DataType.EmailAddress)]
		[EmailAddress]
		public string Email
		{
			get;
			set;
		}

		[Display(Name = "��ע", Order = 80)]
		[StringLength(1000)]
		[DataType(DataType.MultilineText)]
		public string Remark
		{
			get;
			set;
		}

		[Display(Name = "�Ƿ�����", Order = 90)]
		public bool Locked
		{
			get;
			set;
		}

		[Display(Name = "������", Order = 100)]
		public int CreateBy
		{
			get;
			set;
		}

		[Display(Name = "��������", Order = 110)]
		[DataType(DataType.DateTime)]
		public DateTime CreateDate
		{
			get;
			set;
		}

		[Display(Name = "IP��ַ", Order = 120)]
		[StringLength(50)]
		public string Ip
		{
			get;
			set;
		}

		[Display(Name = "�Ƿ����", Order = 130)]
		public bool IsApproved
		{
			get;
			set;
		}

		[Display(Name = "�������", Order = 140)]
		public DateTime ApproveDate
		{
			get;
			set;
		}

		[Display(Name = "�����", Order = 150)]
		public int Approver
		{
			get;
			set;
		}

		[Display(Name = "��ϵ��", Order = 160)]
		[StringLength(50)]
		public string Contact
		{
			get;
			set;
		}

		[Display(Name = "�Ƿ���ʾ", Order = 170)]
		public bool IsDisplay
		{
			get;
			set;
		}

		[Display(Name = "�Ƿ��¼", Order = 180)]
		public bool IsLogin
		{
			get;
			set;
		}

		[Display(Name = "����˳��", Order = 190)]
		public int OrderNum
		{
			get;
			set;
		}

		[Display(Name = "�����������", Order = 200)]
		public bool? CanApproveTask
		{
			get;
			set;
		}
	}
}