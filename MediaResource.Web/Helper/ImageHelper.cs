using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;

namespace MediaResource.Web.Helper
{
	public class ImageHelper
	{
		private static readonly string ThumbPath = ConfigurationManager.AppSettings["ThumbPath"];
		private static readonly string SmallThumbWidth = ConfigurationManager.AppSettings["SmallThumbWidth"];
		private static readonly string LargeThumbWidth = ConfigurationManager.AppSettings["LargeThumbWidth"];

		public static string GetSmallThumbUrl(string imagePath)
		{
			return GetThumbUrl(imagePath, SmallThumbWidth);
		}

		public static string GetLargeThumbUrl(string imagePath)
		{
			return GetThumbUrl(imagePath, LargeThumbWidth);
		}

		private static string GetThumbUrl(string imagePath, string thumbWidth)
		{
			if (string.IsNullOrEmpty(imagePath))
			{
				return "#";
			}

			string[] paths = imagePath.Trim().Split(new[] { "\\", "/" }, StringSplitOptions.RemoveEmptyEntries);
			if (paths.Length < 1)
			{
				return "#";
			}

			string fileSuffix = string.Join("_", paths);
			return WebHelper.Instance.RootUrl + paths[0] + ThumbPath + "/" + thumbWidth + "/" + fileSuffix;
		}

		/// <summary>
		/// 获取截图访问地址。
		/// </summary>
		/// <param name="imagePath">图片路径。</param>
		/// <param name="index">第几张截图。</param>
		/// <returns>截图访问地址。</returns>
		public static string GetSnapUrl(string imagePath, int index = 1)
        {
			if (string.IsNullOrEmpty(imagePath) || imagePath.LastIndexOf('.') == -1)
			{
				return "#";
			}

			string prefix = imagePath.Substring(0, imagePath.LastIndexOf('_') + 1);
			string extension = imagePath.Substring(imagePath.LastIndexOf('.'));

			string imageUrl = WebHelper.Instance.RootUrl + prefix + index + extension;
			return imageUrl;
		}

		/// <summary>
		/// 生成验证码字符串。
		/// </summary>
		/// <returns>验证码字符串。</returns>
		public static string GenerateVerificationCode()
		{
			string verificationCode = string.Empty;
			var random = new Random();
			for (int i = 0; i < 5; i++)
			{
				int number = random.Next();
				char code;
				if (number % 2 == 0)
				{
					code = (char)(48 + (ushort)(number % 10));
				}
				else
				{
					code = (char)(65 + (ushort)(number % 26));
				}
				verificationCode += code.ToString(CultureInfo.InvariantCulture);
			}

			return verificationCode;
		}

		/// <summary>
		/// 根据字符串生成相应的图片。
		/// </summary>
		/// <param name="strInImage">图片中的字符串。</param>
		/// <returns>包含字符串的图片文件流。</returns>
		public static byte[] CreateImageForString(string strInImage)
		{
			if (strInImage == null || strInImage.Trim() == string.Empty)
			{
				return null;
			}

			var image = new Bitmap((int)Math.Ceiling(strInImage.Length * 12.5), 22);
			Graphics g = Graphics.FromImage(image);
			try
			{
				var random = new Random();
				g.Clear(Color.White);
				for (int i = 0; i < 25; i++)
				{
					int x = random.Next(image.Width);
					int x2 = random.Next(image.Width);
					int y = random.Next(image.Height);
					int y2 = random.Next(image.Height);
					g.DrawLine(new Pen(Color.Silver), x, y, x2, y2);
				}
				var font = new Font("Arial", 12f, FontStyle.Bold | FontStyle.Italic);
				var brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f,
													true);
				g.DrawString(strInImage, font, brush, 2f, 2f);
				for (int j = 0; j < 100; j++)
				{
					int x3 = random.Next(image.Width);
					int y3 = random.Next(image.Height);
					image.SetPixel(x3, y3, Color.FromArgb(random.Next()));
				}
				g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
				var ms = new MemoryStream();
				image.Save(ms, ImageFormat.Jpeg);

				return ms.ToArray();
			}
			finally
			{
				g.Dispose();
				image.Dispose();
			}
		}
	}
}
