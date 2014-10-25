using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaResource.Web.Models
{
	[Table("OA_Category")]
	[DisplayName("·ÖÀà")]
	[DisplayColumn("Name")]
	public class Category
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id
		{
			get;
			set;
		}

		public int? ParentId
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

		[StringLength(256)]
		public string Name
		{
			get;
			set;
		}

		[StringLength(1024)]
		public string CategoryNum
		{
			get;
			set;
		}

		public ObjectType CategoryType
		{
			get;
			set;
		}

		public int? TypeLevel
		{
			get;
			set;
		}

		public int? Status
		{
			get;
			set;
		}

		public int? OldId
		{
			get;
			set;
		}

		public int? OrderNum
		{
			get;
			set;
		}
	}
}