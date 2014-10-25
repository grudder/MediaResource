using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaResource.Web.Models
{
	[Table("OA_UserAssignment")]
	[DisplayName("�û���ɫ��Ȩ")]
	public class UserAssignment
	{
		[Key]
		[Column(Order = 0)]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int UserId
		{
			get;
			set;
		}

		[Key]
		[Column(Order = 1)]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int RoleId
		{
			get;
			set;
		}
	}
}