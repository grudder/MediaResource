using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaResource.Web.Models
{
	[Table("OA_UserFolder")]
	[DisplayName("用户上传目录")]
	public class UserFolder
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id
		{
			get;
			set;
		}

		[Required]
		public ObjectType ObjectType
		{
			get;
			set;
		}

		[DisplayName("名称")]
		[StringLength(20)]
		[Required]
		public string Name
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

		[DisplayName("创建时间")]
		[DataType(DataType.DateTime)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = false)]
		public DateTime CreateDate
		{
			get;
			set;
		}
	}
}