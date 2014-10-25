using System;
using System.Web.Mvc;

namespace MediaResource.Web.Helper
{
	/// <summary>
	/// 截取字符串类
	/// </summary>
	public static class TrimTextExtersions
	{
		/// <summary>
		/// 截取字符串方法
		/// </summary>
		/// <param name="helper"></param>
		/// <param name="str">字符串</param>
		/// <param name="maxLength">最大长度</param>
		/// <param name="flag">是否显示...</param>
		/// <returns></returns>
		public static string TrimText(this HtmlHelper helper, string str, int maxLength, bool flag = true)
		{
			if (!string.IsNullOrEmpty(str))
			{
				str = str.Trim();

				int count = 0;
				string strTemp = "";
				for (int i = 0; i < str.Length; i++)
				{
					if (Math.Abs(((str.Substring(i, 1).ToCharArray())[0])) > 255)
					{
						count += 2;
					}
					else
					{
						count += 1;
					}
					if (count <= maxLength)
					{
						strTemp += str.Substring(i, 1);
					}
					else
					{
						strTemp = strTemp + (flag ? "..." : "");
						return str.Replace(str, strTemp);
					}
				}
				return str.Replace(str, strTemp).Replace("　", "").Trim();
			}

			return "";
		}
	}
}