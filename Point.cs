using System;

namespace Asteroids
{
	public class Point
	{
		public double X;
		public double Y;

		public Point(double x, double y)
		{
			this.X = x;
			this.Y = y;
		}
		public static Point operator +(Point left, Point right)
		{
			return new Point(left.X + right.X, left.Y + right.Y);
		}

		public static Point operator -(Point left, Point right)
		{
			return new Point(left.X - right.X, left.Y - right.Y);
		}

		public override string ToString()
		{
			return $"{{X= {this.X}, Y= {this.Y}}}";
		}

		public static Point Sum(params Point[] points)
		{
			Point res = new Point(0, 0);

			foreach (Point p in points)
			{
				res += p;
			}

			return res;
		}

		public double Magnitude()
		{
			return Math.Sqrt(Math.Pow(this.X, 2) + Math.Pow(this.Y, 2));
		}
	}
}