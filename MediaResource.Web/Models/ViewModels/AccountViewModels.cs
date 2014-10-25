using System.ComponentModel.DataAnnotations;

using MediaResource.Web.Models.CustomValidations;

namespace MediaResource.Web.Models.ViewModels
{
	public class LoginViewModel
	{
		[Display(Name = "用户名", Order = 10)]
		[Required]
		[StringLength(50)]
		public string Name
		{
			get;
			set;
		}

		[Display(Name = "密码", Order = 20)]
		[Required]
		[StringLength(40)]
		[DataType(DataType.Password)]
		public string Password
		{
			get;
			set;
		}
	}

	public class RegisterViewModel
	{
        [CustomValidation(typeof(UserValidation), "ValidateNameNotDuplicate")]
		[Display(Name = "用户名", Order = 10)]
		[Required]
		[StringLength(50)]
		public string Name
		{
			get;
			set;
		}

		[Display(Name = "密码", Order = 20)]
		[Required]
		[StringLength(40)]
		[DataType(DataType.Password)]
		public string Password
		{
			get;
			set;
		}

		[DataType(DataType.Password)]
		[Display(Name = "确认密码")]
		[Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
		public string ConfirmPassword
		{
			get;
			set;
		}

		[Display(Name = "所属单位", Order = 30)]
		public int GroupId
		{
			get;
			set;
		}

		[Display(Name = "真实姓名", Order = 40)]
		[Required]
		[StringLength(50)]
		public string FullName
		{
			get;
			set;
		}

        [Display(Name = "固定电话", Order = 50)]
        [Required]
		[StringLength(50)]
		public string Ext
		{
			get;
			set;
		}

        [Display(Name = "手机", Order = 60)]
        [Required]
		[StringLength(50)]
		public string Mobile
		{
			get;
			set;
		}
	}
}