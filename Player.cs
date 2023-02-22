using System;

namespace Asteroids
{
	public class Player
	{
		public double angle = 0;
		public double acceleration = 3;
		public double frictionCoefficient = 0.07;
		public double angularVelocity = 0;
		public double angularAcceleration = 3 * Math.PI / 180;
		public double angularFriction = 15 * Math.PI / 180;
		public double shotTimer = 0.5;
		public double lastShotTime;

		public int shots = 0;
		public int hits = 0;
		public int score = 0;
		public int rotationDirection = 0;
		public int accelerating = 0;
		public int sideLength = 24;

		public bool invulnerable = false;

		public Point center;
		public Point velocity;
		public Color color = Color.White;

		public List<Point> verticies;
		public List<Projectile> projectiles;

		public Player(Point center, Color color)
		{
			this.center = center;
			this.color = color;
			this.velocity = new Point(0, 0);
			this.projectiles = new List<Projectile> { };
			this.verticies = new List<Point>
			{
				new Point(center.X, center.Y - (this.sideLength * 2 * Math.Sqrt(3))/6),
				new Point(center.X - (this.sideLength / 2), center.Y + this.sideLength / 9 + this.sideLength * Math.Sqrt(3)/6),
				new Point(center.X + (this.sideLength / 2), center.Y + this.sideLength / 9 + this.sideLength * Math.Sqrt(3)/6),
			};
		}

		private Player __Move(Point velocity)
		{
			foreach (Point point in this.verticies.ToList())
			{
				this.verticies[this.verticies.IndexOf(point)] += velocity;
			}
			this.center += velocity;
			return this;
		}

		public Player Move()
		{
			double deltaX = (this.accelerating * this.acceleration * Math.Sin(this.angle)) / 2;
			double deltaY = (this.accelerating * this.acceleration * Math.Cos(this.angle)) / 2;
			this.velocity = new Point(this.velocity.X + deltaX, this.velocity.Y - deltaY);
			this.__Move(this.velocity);
			return this;
		}
		public Player Draw(PaintEventArgs events)
		{
			Pen pen = new Pen(color);
			List<Point> verticiesCopy = new List<Point> { this.verticies[0], this.verticies[1], this.center, this.verticies[2] };
			foreach (Point point in verticiesCopy)
			{
				int nextIndex = verticiesCopy.IndexOf(point) + 1;
				if (verticiesCopy.IndexOf(verticiesCopy.Last()) < nextIndex) nextIndex = 0;
				events.Graphics.DrawLine(pen, (int)point.X, (int)point.Y, (int)verticiesCopy[nextIndex].X, (int)verticiesCopy[nextIndex].Y);
			}

			return this;
		}

		public Player Rotate()
		{
			/*
			 * Calculates the new coordinates of each vertex using 
			 * x' = ( x - xᵣ)cos(θ) - (y - yᵣ)sin(θ) + xᵣ
			 * y' = ( x - xᵣ)sin(θ) + (y - yᵣ)cos(θ) + yᵣ , where:
			 * (xᵣ, yᵣ) is the point of rotation, and
			 * (x', y') is the new set of cordinates for the vertex, and
			 * θ is the change in angle
			 */

			this.angularVelocity += this.rotationDirection * this.angularAcceleration;
			this.angle += this.angularVelocity;

			List<Point> verticiesCopy = new List<Point> { this.verticies[0], this.verticies[1], this.verticies[2] };
			(double sin, double cos) sc = Math.SinCos(this.angularVelocity);

			foreach (Point point in verticiesCopy)
			{
				/*x' = ( x - xᵣ)cos(θ) - (y - yᵣ)sin(θ) + xᵣ*/
				double xPrime = (point.X - this.center.X) * sc.cos - (point.Y - this.center.Y) * sc.sin + this.center.X;
				/*y' = ( x - xᵣ)sin(θ) + (y - yᵣ)cos(θ) + yᵣ*/
				double yPrime = (point.X - this.center.X) * sc.sin + (point.Y - this.center.Y) * sc.cos + this.center.Y;
				this.verticies[this.verticies.IndexOf(point)] = new Point(xPrime, yPrime);
			}
			return this;
		}

		public Player Friction()
		{
			double resolveX = this.velocity.X - (this.velocity.X * this.frictionCoefficient);
			double resolveY = this.velocity.Y - (this.velocity.Y * this.frictionCoefficient);

			this.velocity = new Point(resolveX, resolveY);
			this.angularVelocity -= this.angularVelocity * this.angularFriction;

			return this;
		}

		public Player Shoot()
		{
			this.projectiles.Add(new Projectile(this));
			this.shots += 1;
			return this;
		}
	}
}