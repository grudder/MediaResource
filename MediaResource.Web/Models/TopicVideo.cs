using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaResource.Web.Models
{
    [Table("OA_TopicVideo")]
	[DisplayName("ר����Ƶ")]
    public class TopicVideo
    {
        [Display(Name = "ID������", Order = 0)]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id
		{
			get;
			set;
        }

        public int? TopicId
        {
            get;
            set;
        }

        [Display(Name = "����ר��")]
        [ForeignKey("TopicId")]
        public virtual Topic Topic
        {
            get;
            set;
        }

        [Display(Name = "�����")]
        public int OrderNum
        {
            get;
            set;
        }

        [Display(Name = "��������", Order = 10)]
        [StringLength(512)]
        public string Name
        {
            get;
            set;
        }

        [Display(Name = "�� �� ��", Order = 20)]
        [StringLength(1024)]
        public string KeyWords
        {
            get;
            set;
        }

        [Display(Name = "�����ڵ�", Order = 30)]
        public string Nodes
        {
            get;
            set;
        }

        [Display(Name = "ʱ������", Order = 40)]
        public DateTime? TextDate
		{
			get;
			set;
		}

        [Display(Name = "�ˡ���Ա", Order = 50)]
        [StringLength(1024)]
        public string Staff
        {
            get;
            set;
        }

        [Display(Name = "�š���Ҫ", Order = 60)]
        [StringLength(1024)]
        public string Summary
        {
            get;
            set;
        }

        [Display(Name = "��������", Order = 70)]
        [StringLength(1024)]
        public string Source
        {
            get;
            set;
        }

        [Display(Name = "�ļ�λ��", Order = 80)]
        [StringLength(1024)]
        public string Locations
        {
            get;
            set;
        }

        public int? CreateBy
        {
            get;
            set;
        }

        [Display(Name = "�� �� ��", Order = 80)]
        [ForeignKey("CreateBy")]
        public virtual User CreateByEntity
        {
            get;
            set;
        }

        [Display(Name = "�ύʱ��", Order = 90)]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? CreateDate
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

        [Display(Name = "�������", Order = 100)]
        public int ClickCount
        {
            get;
            set;
        }

        [Display(Name = "���ش���", Order = 110)]
        public int DownloadCount
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