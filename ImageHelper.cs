using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Survivors
{
	/// <summary>
	/// Хелпер для работы с изображениями в byte[]
	/// </summary>
	public static class ImageHelper
	{
		/// <summary>
		/// Обработчик изображений.
		/// </summary>
		/// <param name="byteArray"></param>
		/// <returns></returns>
		public static Image ByteArrayToImage(byte[] byteArray)
		{
			using (var ms = new MemoryStream(byteArray))
			{
				return Image.FromStream(ms);
			}
		}
	}
}
