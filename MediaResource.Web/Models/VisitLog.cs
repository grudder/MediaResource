using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaResource.Web.Models
{
	/// <summary>
	/// ������־
	/// </summary>
	[Table("OA_VisitLog")]
	public class VisitLog
	{
		/// <summary>
		/// ��������
		/// </summary>
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id
		{
			get;
			set;
		}

		/// <summary>
		/// ����ר������
		/// </summary>
		public int? TopicId
		{
			get;
			set;
		}

		/// <summary>
		/// �ز�����
		/// </summary>
		[Required]
		public ObjectType MaterialType
		{
			get;
			set;
		}

		/// <summary>
		/// �ز�����
		/// </summary>
		public int MaterialId
		{
			get;
			set;
		}

		/// <summary>
		/// ��������
		/// </summary>
		[Required]
		public VisitType VisitType
		{
			get;
			set;
		}

		/// <summary>
		/// ������
		/// </summary>
		public int VisitedBy
		{
			get;
			set;
		}

		/// <summary>
		/// ������������������
		/// </summary>
		public int VisitorGroupId
		{
			get;
			set;
		}

		/// <summary>
		/// ������IP��ַ
		/// </summary>
		[Required]
		[StringLength(20)]
		public string VisitorIp
		{
			get;
			set;
		}

		/// <summary>
		/// ����ʱ��
		/// </summary>
		public DateTime VisitTime
		{
			get;
			set;
		}
	}
}