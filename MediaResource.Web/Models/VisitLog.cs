using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaResource.Web.Models
{
	/// <summary>
	/// 访问日志
	/// </summary>
	[Table("OA_VisitLog")]
	public class VisitLog
	{
		/// <summary>
		/// 自增主键
		/// </summary>
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id
		{
			get;
			set;
		}

		/// <summary>
		/// 所属专题主键
		/// </summary>
		public int? TopicId
		{
			get;
			set;
		}

		/// <summary>
		/// 素材类型
		/// </summary>
		[Required]
		public ObjectType MaterialType
		{
			get;
			set;
		}

		/// <summary>
		/// 素材主键
		/// </summary>
		public int MaterialId
		{
			get;
			set;
		}

		/// <summary>
		/// 访问类型
		/// </summary>
		[Required]
		public VisitType VisitType
		{
			get;
			set;
		}

		/// <summary>
		/// 访问人
		/// </summary>
		public int VisitedBy
		{
			get;
			set;
		}

		/// <summary>
		/// 访问人所属处室主键
		/// </summary>
		public int VisitorGroupId
		{
			get;
			set;
		}

		/// <summary>
		/// 访问人IP地址
		/// </summary>
		[Required]
		[StringLength(20)]
		public string VisitorIp
		{
			get;
			set;
		}

		/// <summary>
		/// 访问时间
		/// </summary>
		public DateTime VisitTime
		{
			get;
			set;
		}
	}
}