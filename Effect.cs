using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Survivors
{
	/// <summary>
	/// Средство для отображения эффектов.
	/// </summary>
	public class Effect
	{
		/// <summary>
		/// Позиция.
		/// </summary>
		public Point Position { get; private set; }

		/// <summary>
		/// Радиус.
		/// </summary>
		public int Radius { get; private set; }

		/// <summary>
		/// Признак активности.
		/// </summary>
		public bool IsActive => Radius > 0;

		/// <summary>
		/// Максимальный радиус.
		/// </summary>
		private int maxRadius = 15;

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="position">Point</param>
		public Effect(Point position)
		{
			Position = position;
			Radius = 1;
		}

		/// <summary>
		/// Обновление.
		/// </summary>
		public void Update()
		{
			if (Radius < maxRadius)
				Radius += 2;
			else
				Radius = 0;
		}

		/// <summary>
		/// Отрисовка.
		/// </summary>
		/// <param name="g">Graphics</param>
		public void Draw(Graphics g)
		{
			if (!IsActive) return;

			int size = Radius * 2;
			using (Brush brush = new SolidBrush(Color.FromArgb(128, Color.Orange)))
			{
				g.FillEllipse(brush, new Rectangle(Position.X - Radius, Position.Y - Radius, size, size));
			}
		}
	}
}
