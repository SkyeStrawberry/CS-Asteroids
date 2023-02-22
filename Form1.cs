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
}