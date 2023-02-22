using System;

namespace Asteroids
{
	public class Projectile
	{
		public double angle;
		public double speed;

		public (double sin, double cos) sinCos;

		public int length;

		public Player parent;

		public Point center;
		public Point start;
		public Point end;

		public Color color;

		public Projectile(Player parent)
		{
			this.angle = parent.angle;
			this.speed = 10 + parent.velocity.Magnitude();
			this.length = 10;
			this.parent = parent;
			this.center = parent.verticies[0];
			this.color = parent.color;
			this.sinCos = Math.SinCos(this.angle);

			this.start = new Point(this.center.X - (this.sinCos.sin * this.length) / 2, this.center.Y + (this.sinCos.cos * this.length) / 2);
			this.end = new Point(this.center.X + (this.sinCos.sin * this.length) / 2, this.center.Y - (this.sinCos.cos * this.length) / 2);
		}

		public Projectile Move()
		{
			Point changeVector = new Point(this.sinCos.sin * this.speed, -this.sinCos.cos * this.speed);

			this.center += changeVector;
			this.start += changeVector;
			this.end += changeVector;
			return this;
		}

		public Projectile Draw(PaintEventArgs events)
		{
			Pen pen = new Pen(color);
			events.Graphics.DrawLine(pen, (int)this.start.X, (int)this.start.Y, (int)this.end.X, (int)this.end.Y);
			return this;
		}

		public bool InBounds(Asteroids.Form1 form)
		{
			if (this.start.X < 0 || form.Width < this.start.X || form.Width < this.start.Y || this.start.Y < 0) return false;
			return true;
		}
	}
}