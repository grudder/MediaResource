using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaResource.Web.Models
{
	[Table("OA_Comment")]
	[DisplayName("ÆÀÂÛ")]
	public class Comment
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

		[StringLength(1024)]
		public string Content
		{
			get;
			set;
		}

		public int CreateBy
		{
			get;
			set;
		}

		[ForeignKey("CreateBy")]
		public virtual User CreateByEntity
		{
			get;
			set;
		}

		[DataType(DataType.DateTime)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
		public DateTime CreateDate
		{
			get;
			set;
		}
	}
}