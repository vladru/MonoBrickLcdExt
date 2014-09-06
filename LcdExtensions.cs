using System;
using MonoBrickFirmware.Display;

namespace LcdDraw
{
	public static class LcdExtensions
	{
		private static bool pixelInLcd(Point pixel)
		{
			return (pixel.X >= 0) && (pixel.Y >= 0) && (pixel.X <= Lcd.Width) && (pixel.Y <= Lcd.Height);
		}

		private static bool pixelInLcd(int x, int y)
		{
			return	(x >= 0) && (y >= 0) && (x <= Lcd.Width) && (y <= Lcd.Height);
		}

		public static void SetPixelInLcd(this Lcd lcd, int x, int y, bool color)
		{
			if (pixelInLcd (x, y))
				lcd.SetPixel (x, y, color);
		}

		public static void DrawLine (this Lcd lcd, Point start, Point end, bool color)
		{
			int height = Math.Abs (end.Y - start.Y);
			int width = Math.Abs (end.X - start.X);

			int ix = start.X;
			int iy = start.Y;
			int sx = start.X < end.X ? 1 : -1;
			int sy = start.Y < end.Y ? 1 : -1;

			int err = width + (-height);
			int e2;

			do {
				lcd.SetPixelInLcd (ix, iy, color);

				if (ix == end.X && iy == end.Y)
					break;

				e2 = 2 * err;
				if (e2 > (-height)) {
					err += (-height);
					ix += sx;
				}
				if (e2 < width) {
					err += width;
					iy += sy;
				}

			} while (true);

		}

		public static void DrawCircle (this Lcd lcd, Point center, ushort radius, bool color)
		{	
			int f = 1 - radius;
			int ddF_x = 0;
			int ddF_y = -2 * radius;
			int x = 0;
			int y = radius;

			var right = new Point(center.X + radius, center.Y);
			var top = new Point (center.X, center.Y - radius);
			var left = new Point (center.X - radius, center.Y);
			var bottom = new Point(center.X, center.Y + radius);

			lcd.SetPixelInLcd (right.X, right.Y, color);
			lcd.SetPixelInLcd (top.X, top.Y, color);
			lcd.SetPixelInLcd (left.X, left.Y, color);
			lcd.SetPixelInLcd (bottom.X, bottom.Y, color);

			while (x < y) {
				if (f >= 0) {
					y--;
					ddF_y += 2;
					f += ddF_y;
				}
				x++;
				ddF_x += 2;
				f += ddF_x + 1;

				lcd.SetPixelInLcd (center.X + x, center.Y + y, color);
				lcd.SetPixelInLcd (center.X - x, center.Y + y, color);
				lcd.SetPixelInLcd (center.X + x, center.Y - y, color);
				lcd.SetPixelInLcd (center.X - x, center.Y - y, color);
				lcd.SetPixelInLcd (center.X + y, center.Y + x, color);
				lcd.SetPixelInLcd (center.X - y, center.Y + x, color);
				lcd.SetPixelInLcd (center.X + y, center.Y - x, color);
				lcd.SetPixelInLcd (center.X - y, center.Y - x, color);
			}
		}

		public static void DrawCircleFilled (this Lcd lcd, Point center, ushort radius, bool color)
		{
			for (int y = -radius; y <= radius; y++) {
				for (int x = -radius; x <= radius; x++) {
					if (x * x + y * y <= radius * radius) {
						lcd.SetPixelInLcd (center.X + x, center.Y + y, color);
					}
				}
			}
		}

		public static void DrawEllipse(this Lcd lcd, Point center, ushort radiusA, ushort radiusB, bool color)
		{

			int dx = 0;
			int dy = radiusB;
			int a2 = radiusA * radiusA;
			int b2 = radiusB * radiusB;
			int err = b2 - (2 * radiusB - 1) * a2;
			int e2;

			do {
				lcd.SetPixelInLcd(center.X + dx, center.Y + dy, color); /* I. Quadrant */
				lcd.SetPixelInLcd(center.X - dx, center.Y + dy, color); /* II. Quadrant */
				lcd.SetPixelInLcd(center.X - dx, center.Y - dy, color); /* III. Quadrant */
				lcd.SetPixelInLcd(center.X + dx, center.Y - dy, color); /* IV. Quadrant */

				e2 = 2 * err;

				if (e2 < (2 * dx + 1) * b2)
				{
					dx++;
					err += (2 * dx + 1) * b2;
				}

				if (e2 > -(2 * dy - 1) * a2)
				{
					dy--;
					err -= (2 * dy - 1) * a2;
				}
			} while (dy >= 0);

			while (dx++ < radiusA)
			{
				lcd.SetPixelInLcd(center.X + dx, center.Y, color); 
				lcd.SetPixelInLcd(center.X - dx, center.Y, color);
			}
		}

		public static void DrawEllipseFilled
			(this Lcd lcd, Point center, ushort radiusA, ushort radiusB, bool color)
		{
			int hh = radiusB * radiusB;
			int ww = radiusA * radiusA;
			int hhww = hh * ww;
			int x0 = radiusA;
			int dx = 0;

			// do the horizontal diameter
			for (int x = -radiusA; x <= radiusA; x++)
				lcd.SetPixelInLcd(center.X + x, center.Y, color);

			// now do both halves at the same time, away from the diameter
			for (int y = 1; y <= radiusB; y++)
			{
				int x1 = x0 - (dx - 1);  // try slopes of dx - 1 or more

				for (; x1 > 0; x1--)
					if (x1 * x1 * hh + y * y * ww <= hhww)
						break;

				dx = x0 - x1;  // current approximation of the slope
				x0 = x1;

				for (int x = -x0; x <= x0; x++) {
					lcd.SetPixelInLcd(center.X + x, center.Y - y, color);
					lcd.SetPixelInLcd(center.X + x, center.Y + y, color);
				}
			}
		}

		public static void DrawRectangle(this Lcd lcd, Rectangle r, bool color)
		{
			int length = r.P2.X - r.P1.X;
			int height = r.P2.Y - r.P1.Y;

			lcd.DrawHLine(new Point(r.P1.X, r.P1.Y), length, color);
			lcd.DrawHLine(new Point(r.P1.X, r.P2.Y), length, color);

			lcd.DrawVLine(new Point(r.P1.X, r.P1.Y+1), height-2, color);
			lcd.DrawVLine(new Point(r.P2.X, r.P1.Y+1), height-2, color);
		}

		public static void DrawVLine(this Lcd lcd, Point startPoint, int height, bool color)
		{
			for (var y = 0; y <= height; y++) {
				lcd.SetPixelInLcd (startPoint.X, startPoint.Y + y, color);			
			}
		}

	}
}
