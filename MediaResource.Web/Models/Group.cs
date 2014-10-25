using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaResource.Web.Models
{
	[Table("OA_Group")]
	[DisplayName("ดฆสา")]
	public class Group
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id
		{
			get;
			set;
		}

		[Required]
		[StringLength(50)]
		public string Name
		{
			get;
			set;
		}

		[StringLength(1000)]
		public string Remark
		{
			get;
			set;
		}

		public int CreateBy
		{
			get;
			set;
		}

		public DateTime CreateDate
		{
			get;
			set;
		}

		public int? Category
		{
			get;
			set;
		}

		public int? OrderNum
		{
			get;
			set;
		}

		public bool IsDisplay
		{
			get;
			set;
		}

		public bool IsLogin
		{
			get;
			set;
		}
	}
}