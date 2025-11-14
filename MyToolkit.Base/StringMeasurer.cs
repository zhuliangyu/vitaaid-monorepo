using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MyToolkit.Base
{
	// https://stackoverflow.com/questions/3811916/measurestring-and-drawstring-difference
	public class StringMeasurer
	{
		public static int MeasureLineCountOfDisplayString(Graphics graphics, string text, Font font, float maxWidth)
		{
			var singleLineHeight = MeasureDisplayString(graphics, text, font, 0);
			if (singleLineHeight.Width <= maxWidth)
				return 1;
			var totalHeight = MeasureDisplayString(graphics, text, font, maxWidth);
			return (int)(totalHeight.Height / singleLineHeight.Height);
		}

		public static RectangleF MeasureDisplayString(Graphics graphics, string text, Font font, float maxWidth = 0)
		{
			System.Drawing.StringFormat format = new System.Drawing.StringFormat();
			System.Drawing.RectangleF rect = new System.Drawing.RectangleF(0, 0, (maxWidth == 0) ? short.MaxValue : maxWidth, short.MaxValue);
			System.Drawing.CharacterRange[] ranges = { new System.Drawing.CharacterRange(0, text.Length) };
			System.Drawing.Region[] regions = new System.Drawing.Region[1];

			format.SetMeasurableCharacterRanges(ranges);
			if (maxWidth == 0)
			{
				format.FormatFlags = StringFormatFlags.NoClip | StringFormatFlags.NoWrap;
				format.Trimming = StringTrimming.None;
			}
			regions = graphics.MeasureCharacterRanges(text, font, rect, format);
			rect = (regions.Length == 0) ? new RectangleF { X = 0, Y = 0, Height = 0, Width = 0 } : regions[0].GetBounds(graphics);
			return rect;
			//return (int)(rect.Right + 1.0f);
		}

		/* other reference
		private static SizeF GetScaleTransform(Matrix m)
		{
			
			 //3x3 matrix, affine transformation (skew - used by rotation)
			 //[ X scale,     Y skew,      0 ]
			 //[ X skew,      Y scale,     0 ]
			 //[ X translate, Y translate, 1 ]

			 //indices (0, ...): X scale, Y skew, Y skew, X scale, X translate, Y translate
			 
			return new SizeF(m.Elements[0], m.Elements[3]);
		}

		public static RectangleF MeasureString(Graphics graphics, string s, Font f)
		{
			//copy only scale, not rotate or transform
			var scale = GetScaleTransform(graphics.Transform);

			// Get initial estimate with MeasureText
			//TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.NoClipping;
			//Size proposedSize = new Size(int.MaxValue, int.MaxValue);
			//Size size = TextRenderer.MeasureText(graphics, s, f, proposedSize, flags);
			SizeF sizef = graphics.MeasureString(s, f);
			sizef.Width *= scale.Width;
			sizef.Height *= scale.Height;
			Size size = sizef.ToSize();

			int xLeft = 0;
			int xRight = size.Width - 1;
			int yTop = 0;
			int yBottom = size.Height - 1;

			// Create a bitmap
			using (Bitmap image = new Bitmap(size.Width, size.Height))
			{
				image.SetResolution(graphics.DpiX, graphics.DpiY);

				StringFormat strFormat = new StringFormat();
				strFormat.Alignment = StringAlignment.Near;
				strFormat.LineAlignment = StringAlignment.Near;

				// Draw the actual text
				using (Graphics g = Graphics.FromImage(image))
				{					
					g.SmoothingMode = graphics.SmoothingMode;
					g.TextRenderingHint = graphics.TextRenderingHint;
					g.Clear(Color.White);
					g.ScaleTransform(scale.Width, scale.Height);
					g.DrawString(s, f, Brushes.Black, new PointF(0, 0), strFormat);
				}
				// Find the true boundaries of the glyph

				// Find left margin
				for (; xLeft < xRight; xLeft++)
					for (int y = yTop; y <= yBottom; y++)
						if (image.GetPixel(xLeft, y).ToArgb() != Color.White.ToArgb())
							goto OUTER_BREAK_LEFT;
						OUTER_BREAK_LEFT:;

				// Find right margin
				for (; xRight > xLeft; xRight--)
					for (int y = yTop; y <= yBottom; y++)
						if (image.GetPixel(xRight, y).ToArgb() != Color.White.ToArgb())
							goto OUTER_BREAK_RIGHT;
						OUTER_BREAK_RIGHT:;

				// Find top margin
				for (; yTop < yBottom; yTop++)
					for (int x = xLeft; x <= xRight; x++)
						if (image.GetPixel(x, yTop).ToArgb() != Color.White.ToArgb())
							goto OUTER_BREAK_TOP;
						OUTER_BREAK_TOP:;

				// Find bottom margin
				for (; yBottom > yTop; yBottom--)
					for (int x = xLeft; x <= xRight; x++)
						if (image.GetPixel(x, yBottom).ToArgb() != Color.White.ToArgb())
							goto OUTER_BREAK_BOTTOM;
						OUTER_BREAK_BOTTOM:;
			}

			var pt = new PointF(xLeft, yTop);
			var sz = new SizeF(xRight - xLeft + 1, yBottom - yTop + 1);
			return new RectangleF(pt.X / scale.Width, pt.Y / scale.Height,
				sz.Width / scale.Width, sz.Height / scale.Height);
		}


		public static RectangleF MeasureDisplayStringWidth(Graphics graphics, string text, Font font)
		{
			System.Drawing.StringFormat format = new System.Drawing.StringFormat();
			System.Drawing.RectangleF rect = new System.Drawing.RectangleF(0, 0, 1000, 1000);
			System.Drawing.CharacterRange[] ranges = { new System.Drawing.CharacterRange(0, text.Length) };
			System.Drawing.Region[] regions = new System.Drawing.Region[1];

			format.SetMeasurableCharacterRanges(ranges);

			regions = graphics.MeasureCharacterRanges(text, font, rect, format);
			rect = regions[0].GetBounds(graphics);
			return rect;
			//return (int)(rect.Right + 1.0f);
		}
		public static SizeF MeasureStringSize(Graphics graphics, string text, Font font)
		{

			// Get initial estimate with MeasureText
			TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.NoClipping;
			Size proposedSize = new Size(int.MaxValue, int.MaxValue);
			Size size = TextRenderer.MeasureText(graphics, text, font, proposedSize, flags);

			// Create a bitmap
			Bitmap image = new Bitmap(size.Width, size.Height);
			image.SetResolution(graphics.DpiX, graphics.DpiY);

			StringFormat strFormat = new StringFormat();
			strFormat.Alignment = StringAlignment.Near;
			strFormat.LineAlignment = StringAlignment.Near;

			// Draw the actual text
			Graphics g = System.Drawing.Graphics.FromImage(image);
			g.SmoothingMode = SmoothingMode.HighQuality;
			g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
			g.Clear(System.Drawing.Color.White);
			g.DrawString(text, font, System.Drawing.Brushes.Black, new PointF(0, 0), strFormat);

			// Find the true boundaries of the glyph
			int xs = 0;
			int xf = size.Width - 1;
			int ys = 0;
			int yf = size.Height - 1;

			// Find left margin
			while (xs < xf)
			{
				for (int y = ys; y <= yf; y++)
				{
					if (image.GetPixel(xs, y).ToArgb() != System.Drawing.Color.White.ToArgb())
						break;
				}
				xs += 1;
			}
			// Find right margin
			while (xf > xs)
			{
				for (int y = ys; y <= yf; y++)
				{
					if (image.GetPixel(xf, y).ToArgb() != System.Drawing.Color.White.ToArgb())
						break;
				}
				xf -= 1;
			}
			// Find top margin
			while (ys < yf)
			{
				for (int x = xs; x <= xf; x++)
				{
					if (image.GetPixel(x, ys).ToArgb() != System.Drawing.Color.White.ToArgb())
						break;
				}
				ys += 1;
			}
			// Find bottom margin
			while (yf > ys)
			{
				for (int x = xs; x <= xf; x++)
				{
					if (image.GetPixel(x, yf).ToArgb() != System.Drawing.Color.White.ToArgb())
						break;
				}
				yf -= 1;
			}

			return new SizeF(xf - xs + 1, yf - ys + 1);
		}
		*/
	}
}
