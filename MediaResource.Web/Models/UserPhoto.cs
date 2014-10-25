using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaResource.Web.Models
{
	[Table("OA_UserPhoto")]
	[DisplayName("用户照片")]
	public class UserPhoto
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id
		{
			get;
			set;
		}

		[DisplayName("所属目录")]
		[Required]
		public int FolderId
		{
			get;
			set;
		}

		[DisplayName("所属目录")]
		[ForeignKey("FolderId")]
		public virtual UserFolder Folder
		{
			get;
			set;
		}

		[DisplayName("名称")]
		[StringLength(50)]
		[Required]
		public string Name
		{
			get;
			set;
		}

		[DisplayName("上传日期")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = false)]
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

		[Display(Name = "上传人")]
		[ForeignKey("CreateBy")]
		public virtual User CreateByEntity
		{
			get;
			set;
		}

		[DisplayName("保密性")]
		[Required]
		public bool? IsPublic
		{
			get;
			set;
		}

		[DisplayName("上传文件")]
		[StringLength(1024)]
		public string FileUrl
		{
			get;
			set;
		}

		[StringLength(1024)]
		public string TinyUrl
		{
			get;
			set;
		}

		public double Score
		{
			get;
			set;
		}

		public int ScoreCount
		{
			get;
			set;
		}

		public int ClickCount
		{
			get;
			set;
		}
	}
}