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
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id
		{
			get;
			set;
        }

        [Display(Name = "�ڵ�", Order = 10)]
        public string Nodes
        {
            get;
            set;
        }

        [Display(Name = "����˳��", Order = 20)]
        public int OrderNum
        {
            get;
            set;
        }

        [Display(Name = "�ؼ���", Order = 30)]
		[StringLength(1024)]
        public string KeyWords
		{
			get;
			set;
        }

        [Display(Name = "ʱ��", Order = 40)]
        public DateTime? TextDate
		{
			get;
			set;
		}

        [Display(Name = "��Ա", Order = 50)]
        [StringLength(1024)]
        public string Staff
        {
            get;
            set;
        }

        [Display(Name = "��Ҫ", Order = 60)]
        [StringLength(1024)]
        public string Summary
        {
            get;
            set;
        }

        [Display(Name = "����", Order = 70)]
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

        [Display(Name = "����ʱ��", Order = 50)]
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

        [Display(Name = "������", Order = 100)]
        [ForeignKey("CreateBy")]
        public virtual User CreateByEntity
        {
            get;
            set;
        }

        [Display(Name = "����", Order = 110)]
        [StringLength(512)]
        public string Name
        {
            get;
            set;
        }

        public int? TopicId
        {
            get;
            set;
        }

        [Display(Name = "����ר��", Order = 120)]
        [ForeignKey("TopicId")]
        public virtual Topic Topic
        {
            get;
            set;
        }
	}
}