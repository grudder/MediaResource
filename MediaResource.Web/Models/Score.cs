using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaResource.Web.Models
{
	[Table("OA_Score")]
	[DisplayName("ÆÀ·Ö")]
	public class Score
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id
		{
			get;
			set;
		}

		public ObjectType ObjectType
		{
			get;
			set;
		}

		public int ObjectId
		{
			get;
			set;
		}

		public int Value
		{
			get;
			set;
		}

		[DataType(DataType.DateTime)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
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