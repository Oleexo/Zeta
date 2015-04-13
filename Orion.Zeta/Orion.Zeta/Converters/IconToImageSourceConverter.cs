using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Orion.Zeta.Converters {
	public class IconToImageSourceConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			var icon = (Icon) value;
			BitmapFrame imageSource;
			using (var bmp = icon.ToBitmap()) {
				var stream = new MemoryStream();
				bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
				imageSource = BitmapFrame.Create(stream);
			}
			return imageSource;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotImplementedException();
		}
	}
}