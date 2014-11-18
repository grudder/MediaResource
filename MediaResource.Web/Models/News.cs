using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaResource.Web.Models
{
	[Table("OA_News")]
	[DisplayName("����")]
	public class News
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id
		{
			get;
			set;
		}

		[Display(Name = "����", Order = 10)]
		[StringLength(256)]
		public string Name
		{
			get;
			set;
		}

		[Display(Name = "����", Order = 20)]
		[StringLength(256)]
		public string BroadcastType
		{
			get;
			set;
		}

		[Display(Name = "����ʱ��", Order = 30)]
		[DataType(DataType.DateTime)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime? StoryTime
		{
			get;
			set;
		}

		[Display(Name = "������Ŀ", Order = 40)]
		[StringLength(256)]
		public string NewsSection
		{
			get;
			set;
		}

		[Display(Name = "����", Order = 50)]
		[StringLength(256)]
		public string Reporter
		{
			get;
			set;
		}

		[Display(Name = "����", Order = 60)]
		[StringLength(256)]
		public string Length
		{
			get;
			set;
		}

		[Display(Name = "���봦��", Order = 70)]
		[StringLength(256)]
		public string Association
		{
			get;
			set;
		}

		[Display(Name = "����", Order = 80)]
		[StringLength(2048)]
		public string Release
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

		[Display(Name = "�������", Order = 110)]
		public int? ClickCount
		{
			get;
			set;
		}

		[Display(Name = "���ش���", Order = 120)]
		public int? DownloadCount
		{
			get;
			set;
		}

		[Display(Name = "�ϴ�ʱ��", Order = 90)]
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

		[Display(Name = "�ϴ���", Order = 100)]
		[ForeignKey("CreateBy")]
		public virtual User CreateByEntity
		{
			get;
			set;
		}

		public int? Status
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