using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Demo.ViewModel
{
	public class ColorItemViewModel : TreeItemViewModel
	{
		#region Constructor

		public ColorItemViewModel(TreeItemViewModel parent, bool lazyLoadChildren)
			: base(parent, lazyLoadChildren)
		{
			Color = Colors.Silver;
		}

		#endregion Constructor

		#region Public properties

		private Color color;
		public Color Color
		{
			get { return color; }
			set
			{
				if (value != color)
				{
					color = value;
					OnPropertyChanged("Color");
					OnPropertyChanged("BackgroundBrush");
					OnPropertyChanged("ForegroundBrush");
					DisplayName = color.ToString();
				}
			}
		}

		public Brush BackgroundBrush
		{
			get { return new SolidColorBrush(Color); }
		}

		public Brush ForegroundBrush
		{
			get { return IsDarkColor(Color) ? Brushes.White : Brushes.Black; }
		}

		#endregion Public properties

		#region Private methods

		/// <summary>
		/// Computes the grey value value of a color.
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		private static byte ToGray(Color c)
		{
			return (byte) (c.R * 0.3 + c.G * 0.59 + c.B * 0.11);
		}

		/// <summary>
		/// Determines whether the color is dark or light.
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		private static bool IsDarkColor(Color c)
		{
			return ToGray(c) < 0x90;
		}

		#endregion Private methods
	}
}
