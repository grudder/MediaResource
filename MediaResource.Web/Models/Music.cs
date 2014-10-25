using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaResource.Web.Models
{
	[Table("OA_Music")]
	[DisplayName("����")]
	public class Music
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id
		{
			get;
			set;
		}

		[Display(Name = "��������", Order = 10)]
		[StringLength(256)]
		public string Title
		{
			get;
			set;
		}

		[Display(Name = "���ֳ���", Order = 10)]
		[StringLength(256)]
		public string Length
		{
			get;
			set;
		}

		public int? Category
		{
			get;
			set;
		}

		[ForeignKey("Category")]
		public virtual Category CategoryEntity
		{
			get;
			set;
		}

		[Display(Name = "ԭ������", Order = 40)]
		[StringLength(256)]
		public string Original
		{
			get;
			set;
		}

		[Display(Name = "����", Order = 50)]
		[StringLength(1024)]
		public string Description
		{
			get;
			set;
		}

		[Display(Name = "��������", Order = 60)]
		[StringLength(256)]
		public string Association
		{
			get;
			set;
		}

		[Display(Name = "�ϴ�ʱ��", Order = 70)]
		[DataType(DataType.DateTime)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
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

		[Display(Name = "�ϴ���", Order = 80)]
		[ForeignKey("CreateBy")]
		public virtual User CreateByEntity
		{
			get;
			set;
		}

		[Display(Name = "�������", Order = 90)]
		public int? ClickCount
		{
			get;
			set;
		}

		[Display(Name = "���ش���", Order = 100)]
		public int? DownloadCount
		{
			get;
			set;
		}

		[StringLength(1024)]
		public string Num
		{
			get;
			set;
		}

		[StringLength(1024)]
		public string FileUrl
		{
			get;
			set;
		}

		public int? Status
		{
			get;
			set;
		}

		[Display(Name = "���", Order = 30)]
		[StringLength(50)]
		public string MusicType
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
	}
}