using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaResource.Web.Models
{
	[Table("OA_UserFolder")]
	[DisplayName("�û��ϴ�Ŀ¼")]
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

		[DisplayName("����")]
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

		[DisplayName("����ʱ��")]
		[DataType(DataType.DateTime)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = false)]
		public DateTime CreateDate
		{
			get;
			set;
		}
	}
}