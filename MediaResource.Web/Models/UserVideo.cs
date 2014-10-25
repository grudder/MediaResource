using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaResource.Web.Models
{
	[Table("OA_UserVideo")]
	[DisplayName("�û���Ƶ")]
	public class UserVideo
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
		[StringLength(256)]
		public string Title
		{
			get;
			set;
		}

		[DisplayName("��������")]
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
		public string FlvPath
		{
			get;
			set;
		}

		[StringLength(1024)]
		public string ImagePath
		{
			get;
			set;
		}

		public bool? IsConverted
		{
			get;
			set;
		}

		public int? ImagesCount
		{
			get;
			set;
		}

		public int? Status
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