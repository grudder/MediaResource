using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaResource.Web.Models
{
	[Table("OA_UserDownloadFile")]
	[DisplayName("用户下载文件")]
	public class UserDownloadFile
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		public int FileId
		{
			get;
			set;
		}

		public int DownloadDate
		{
			get;
			set;
		}

		public int DownloadCount
		{
			get;
			set;
		}

		public int FileType
		{
			get;
			set;
		}
	}
}