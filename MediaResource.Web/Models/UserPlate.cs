using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaResource.Web.Models
{
    [Table("OA_UserPlate")]
	[DisplayName("�Խ����")]
    public class UserPlate
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id
		{
			get;
			set;
		}

        [Display(Name = "�������", Order = 10)]
		[StringLength(256)]
        public string PlateName
		{
			get;
			set;
        }

        public int? TopicId
        {
            get;
            set;
        }

        [Display(Name = "����ר��", Order = 20)]
        [ForeignKey("TopicId")]
        public virtual Topic Topic
        {
            get;
            set;
        }

		[Display(Name = "���", Order = 30)]
		[StringLength(30)]
        public string Category
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

        [Display(Name = "������", Order = 40)]
        [ForeignKey("CreateBy")]
        public virtual User CreateByEntity
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

        [Display(Name = "����˳��", Order = 60)]
        public int OrderNum
        {
            get;
            set;
        }
	}
}