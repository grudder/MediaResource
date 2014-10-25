using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaResource.Web.Models
{
	[Table("OA_TaskListNO")]
	[DisplayName("»ŒŒÒ–Ú∫≈")]
	public class TaskListNo
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id
		{
			get;
			set;
		}

		public int? Year
		{
			get;
			set;
		}
	}
}