using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaResource.Web.Models
{
	[Table("OA_ApplyForDownload")]
	[DisplayName("œ¬‘ÿ…Í«Î")]
	public class ApplyForDownload
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id
		{
			get;
			set;
		}

		[StringLength(1024)]
		public string Reason
		{
			get;
			set;
		}

		public DateTime? CreateDate
		{
			get;
			set;
		}

		public int? CreateBy
		{
			get;
			set;
		}

		[StringLength(1024)]
		public string ApplyDetail
		{
			get;
			set;
		}

		public bool? IsApprove
		{
			get;
			set;
		}

		public int? Approver
		{
			get;
			set;
		}

		public DateTime? ApproveDate
		{
			get;
			set;
		}
	}
}