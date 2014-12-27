using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaResource.Web.Models
{
	[Table("OA_Topic")]
	[DisplayName("ר��")]
    public class Topic
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id
		{
			get;
			set;
		}

		[Display(Name = "����˳��", Order = 10)]
        public int OrderNum
		{
			get;
			set;
		}

		[Display(Name = "����", Order = 20)]
		[StringLength(256)]
        public string Name
		{
			get;
			set;
		}

		[Display(Name = "���촦��", Order = 30)]
		[StringLength(50)]
		public string Offices
		{
			get;
			set;
		}

		[Display(Name = "���봦��", Order = 40)]
		[StringLength(256)]
		public string Association
		{
			get;
			set;
		}

        [Display(Name = "�б�ͼƬ��ַ")]
		[StringLength(1024)]
        public string ListPhotoUrl
		{
			get;
			set;
		}

        [Display(Name = "����ͼƬ��ַ")]
        [StringLength(1024)]
        public string DetailPhotoUrl
        {
            get;
            set;
        }

        [Display(Name = "�Ƿ���ʾ")]
        public bool? IsDisplay
		{
			get;
			set;
        }

        public int? CreateBy
        {
            get;
            set;
        }

        [Display(Name = "������", Order = 50)]
        [ForeignKey("CreateBy")]
        public virtual User CreateByEntity
        {
            get;
            set;
        }

        [Display(Name = "����ʱ��", Order = 60)]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? CreateDate
        {
            get;
            set;
        }

        [Display(Name = "�ɼ�����", Order = 70)]
        public string VisibleOffices
        {
            get;
            set;
        }
	}
}