using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaResource.Web.Models
{
	[Table("OA_Role")]
	[DisplayName("½ÇÉ«")]
	public class Role
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
	}
}