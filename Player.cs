using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Simple_Survivors.Properties;

namespace Simple_Survivors
{
	/// <summary>
	/// Игрок.
	/// </summary>
	public class Player
	{
		/// <summary>
		/// Позиция.
		/// </summary>
		public Point Position { get; private set; } = new Point(400, 300);

		/// <summary>
		/// Прямоугольник.
		/// </summary>
		public Rectangle Bounds => new Rectangle(Position.X - 10, Position.Y - 10, 20, 20);

		/// <summary>
		/// Скорость.
		/// </summary>
		public int Speed { get; set; } = 5;

		/// <summary>
		/// Здоровье.
		/// </summary>
		public int Health { get; set; } = 5;

		/// <summary>
		/// WASD клавиши.
		/// </summary>
		private bool[] keys = new bool[4];

		/// <summary>
		/// Нажатие клавиши.
		/// </summary>
		/// <param name="key">Keys</param>
		public void PressKey(Keys key)
		{
			if (key == Keys.W) keys[0] = true;
			if (key == Keys.A) keys[1] = true;
			if (key == Keys.S) keys[2] = true;
			if (key == Keys.D) keys[3] = true;
		}

		/// <summary>
		/// Отпускание клавиши.
		/// </summary>
		/// <param name="key">Keys</param>
		public void ReleaseKey(Keys key)
		{
			if (key == Keys.W) keys[0] = false;
			if (key == Keys.A) keys[1] = false;
			if (key == Keys.S) keys[2] = false;
			if (key == Keys.D) keys[3] = false;
		}

		/// <summary>
		/// Обновление.
		/// </summary>
		/// <param name="clientSize">Size</param>
		public void Update(Size clientSize)
		{
			int dx = 0, dy = 0;
			if (keys[0]) dy -= Speed;
			if (keys[1]) dx -= Speed;
			if (keys[2]) dy += Speed;
			if (keys[3]) dx += Speed;

			Position = new Point(
				Clamp(Position.X + dx, 20, clientSize.Width - 20),
				Clamp(Position.Y + dy, 20, clientSize.Height - 20)
			);
		}

		/// <summary>
		/// Зажим.
		/// </summary>
		/// <param name="val">Значение.</param>
		/// <param name="min">Минимум.</param>
		/// <param name="max">Максимум.</param>
		/// <returns></returns>
		private int Clamp(int val, int min, int max)
		{
			return Math.Max(min, Math.Min(max, val));
		}

		/// <summary>
		/// Получение урона.
		/// </summary>
		public void TakeDamage()
		{
			Health--;
		}

		/// <summary>
		/// Отрисовка.
		/// </summary>
		/// <param name="g">Graphics</param>
		public void Draw(Graphics g)
		{
			byte[] imageData = Properties.Resources.player;
			Image img = ImageHelper.ByteArrayToImage(imageData);
			g.DrawImage(img, new Rectangle(Position.X - 16, Position.Y - 16, 32, 32));
		}
	}
}
