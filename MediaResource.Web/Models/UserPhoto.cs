using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaResource.Web.Models
{
	[Table("OA_UserPhoto")]
	[DisplayName("�û���Ƭ")]
	public class UserPhoto
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id
		{
			get;
			set;
		}

		[DisplayName("����Ŀ¼")]
		[Required]
		public int FolderId
		{
			get;
			set;
		}

		[DisplayName("����Ŀ¼")]
		[ForeignKey("FolderId")]
		public virtual UserFolder Folder
		{
			get;
			set;
		}

		[DisplayName("����")]
		[StringLength(50)]
		[Required]
		public string Name
		{
			get;
			set;
		}

		[DisplayName("�ϴ�����")]
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

		[Display(Name = "�ϴ���")]
		[ForeignKey("CreateBy")]
		public virtual User CreateByEntity
		{
			get;
			set;
		}

		[DisplayName("������")]
		[Required]
		public bool? IsPublic
		{
			get;
			set;
		}

		[DisplayName("�ϴ��ļ�")]
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