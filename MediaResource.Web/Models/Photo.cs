using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaResource.Web.Models
{
	[Table("OA_Photo")]
	[DisplayName("��Ƭ")]
	public class Photo
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

		[Display(Name = "�ؼ���", Order = 20)]
		[StringLength(256)]
		public string Keyword
		{
			get;
			set;
		}

		[Display(Name = "����ʱ��", Order = 30)]
		[DataType(DataType.DateTime)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime? RecordingTime
		{
			get;
			set;
		}

		[Display(Name = "����ص�", Order = 40)]
		[StringLength(1024)]
		public string Location
		{
			get;
			set;
		}

		[Display(Name = "������", Order = 50)]
		[StringLength(256)]
		public string Shooting
		{
			get;
			set;
		}

		[Display(Name = "�λ��쵼", Order = 60)]
		[StringLength(256)]
		public string Leadership
		{
			get;
			set;
		}

		[Display(Name = "�λ���Ա", Order = 70)]
		[StringLength(256)]
		public string Participants
		{
			get;
			set;
		}

		[Display(Name = "���촦��", Order = 80)]
		[StringLength(256)]
		public string Offices
		{
			get;
			set;
		}

		[Display(Name = "���봦��", Order = 90)]
		[StringLength(256)]
		public string Association
		{
			get;
			set;
		}

		[Display(Name = "�ϴ�ʱ��", Order = 100)]
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

		[Display(Name = "�ϴ���", Order = 110)]
		[ForeignKey("CreateBy")]
		public virtual User CreateByEntity
		{
			get;
			set;
		}

		[Display(Name = "�������", Order = 120)]
		public int? ClickCount
		{
			get;
			set;
		}

		[Display(Name = "���ش���", Order = 130)]
		public int? DownloadCount
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
	}
}