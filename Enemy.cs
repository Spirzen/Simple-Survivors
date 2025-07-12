using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Survivors
{
	/// <summary>
	/// Враги.
	/// </summary>
	public class Enemy
	{
		/// <summary>
		/// Позиция.
		/// </summary>
		public Point Position { get; set; }

		/// <summary>
		/// Прямоугольник.
		/// </summary>
		public Rectangle Bounds => new Rectangle(Position.X - 10, Position.Y - 10, 20, 20);

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="position">Point</param>
		public Enemy(Point position)
		{
			Position = position;
		}

		/// <summary>
		/// Обновление.
		/// </summary>
		/// <param name="playerPos">Point</param>
		public void Update(Point playerPos)
		{
			int dx = playerPos.X > Position.X ? 1 : -1;
			int dy = playerPos.Y > Position.Y ? 1 : -1;

			Position = new Point(Position.X + dx, Position.Y + dy);
		}

		/// <summary>
		/// Отрисовка.
		/// </summary>
		/// <param name="g">Graphics</param>
		public void Draw(Graphics g)
		{
			byte[] imageData = Properties.Resources.enemy;
			Image img = ImageHelper.ByteArrayToImage(imageData);
			g.DrawImage(img, new Rectangle(Position.X - 16, Position.Y - 16, 32, 32));
		}
	}
}
