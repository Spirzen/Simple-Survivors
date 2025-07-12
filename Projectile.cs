using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Survivors
{
	/// <summary>
	/// Снаряды.
	/// </summary>
	public class Projectile
	{
		/// <summary>
		/// Позиция.
		/// </summary>
		public Point Position { get; private set; }

		/// <summary>
		/// Цель.
		/// </summary>
		private Point Target;

		/// <summary>
		/// Скорость снаряда.
		/// </summary>
		private int Speed = 10;

		/// <summary>
		/// Прямоугольник.
		/// </summary>
		public Rectangle Bounds => new Rectangle(Position.X - 3, Position.Y - 3, 6, 6);

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="start">Начало.</param>
		/// <param name="target">Цель.</param>
		public Projectile(Point start, Point target)
		{
			Position = start;
			Target = target;
		}

		/// <summary>
		/// Обновление.
		/// </summary>
		public void Update()
		{
			float dx = Target.X - Position.X;
			float dy = Target.Y - Position.Y;
			float dist = (float)Math.Sqrt(dx * dx + dy * dy);
			if (dist < Speed)
			{
				Position = Target;
			}
			else
			{
				Position = new Point(
					Position.X + (int)(dx / dist * Speed),
					Position.Y + (int)(dy / dist * Speed)
				);
			}
		}

		/// <summary>
		/// Отрисовка.
		/// </summary>
		/// <param name="g">Graphics</param>
		public void Draw(Graphics g)
		{
			g.FillEllipse(Brushes.Black, new Rectangle(Position.X - 4, Position.Y - 4, 8, 8));
			g.DrawLine(Pens.Gold, Position, new Point(Position.X - 10, Position.Y - 10));
			g.DrawLine(Pens.Gold, Position, new Point(Position.X + 5, Position.Y - 15));
		}
	}
}
