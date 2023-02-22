using System;
using System.Drawing;
using System.IO.Enumeration;
using System.Runtime.CompilerServices;
using System.Windows.Forms.VisualStyles;
using System.Xml.Serialization;

namespace Asteroids
{
	public partial class Form1 : Form
	{
		Player player;
		public Form1()
		{
			InitializeComponent();

			foreach (Control control in this.Controls)
			{
				if (!(control is PictureBox)) continue;
				if (control.Tag.ToString() == "SpawnPoint.Player") this.player = new Player(new Point(control.Left, control.Top), Color.White);
			}
			if(this.player is null) this.player = new Player(new Point(0,0), Color.White);
		}

		private void timer1_Tick(object sender, EventArgs e)
		{

		}

		private void KeyIsDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Up) this.player.accelerating = 1;
			if (e.KeyCode == Keys.Down) this.player.accelerating = -1;
			if (e.KeyCode == Keys.Left) this.player.rotationDirection = -1;
			if (e.KeyCode == Keys.Right) this.player.rotationDirection = 1;
			if (e.KeyCode == Keys.Space) this.player.Shoot();
		}

		private void KeyIsUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Up) this.player.accelerating = 0;
			if (e.KeyCode == Keys.Down) this.player.accelerating = 0;
			if (e.KeyCode == Keys.Left) this.player.rotationDirection = 0;
			if (e.KeyCode == Keys.Right) this.player.rotationDirection = 0;
		}

		private void GameTick(object sender, EventArgs e)
		{
			foreach (Projectile p in this.player.projectiles.ToList()) 
			{
				if (!p.InBounds(this)) this.player.projectiles.Remove(p);
				p.Move();
			}
			this.player.Move().Rotate().Friction();
			Refresh();
		}

		private void DrawFrame(object sender, PaintEventArgs e)
		{
			foreach (Projectile p in this.player.projectiles) p.Draw(e);
			this.player.Draw(e);
		}

		private void Form1_Load(object sender, EventArgs e)
		{

		}

		private void pictureBox1_Click(object sender, EventArgs e)
		{

		}
	}
	
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

			this.start = new Point(this.center.X - (this.sinCos.sin * this.length)/2, this.center.Y + (this.sinCos.cos * this.length)/2);
			this.end = new Point(this.center.X + (this.sinCos.sin * this.length)/2, this.center.Y - (this.sinCos.cos * this.length)/2);
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
			if(this.start.X < 0 || form.Width < this.start.X || form.Width < this.start.Y || this.start.Y < 0) return false;
			return true;
		}
	}

	public class Point
	{
		public double X;
		public double Y;

		public Point(double x, double y) {
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
			Point res = new Point(0,0);

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