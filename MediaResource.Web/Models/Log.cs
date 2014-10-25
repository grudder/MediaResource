using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaResource.Web.Models
{
	[Table("OA_Log")]
	[DisplayName("»’÷æ")]
	public class Log
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id
		{
			get;
			set;
		}

		[Required]
		[StringLength(20)]
		public string Ip
		{
			get;
			set;
		}

		[Required]
		[StringLength(1000)]
		public string RawUrl
		{
			get;
			set;
		}

		[Required]
		[StringLength(100)]
		public string Module
		{
			get;
			set;
		}

		[Required]
		[StringLength(200)]
		public string Operation
		{
			get;
			set;
		}

		public int ObjectId
		{
			get;
			set;
		}

		[Required]
		[StringLength(100)]
		public string ObjectType
		{
			get;
			set;
		}

		[StringLength(1000)]
		public string Description
		{
			get;
			set;
		}

		public int OperateBy
		{
			get;
			set;
		}

		public DateTime OperateDate
		{
			get;
			set;
		}
	}
}