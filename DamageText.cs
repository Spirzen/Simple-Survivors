using System.Drawing;

namespace Simple_Survivors
{
	/// <summary>
	/// Средство для отображения урона.
	/// </summary>
	public class DamageText
	{
		/// <summary>
		/// Позиция.
		/// </summary>
		public Point Position { get; private set; }
		
		/// <summary>
		/// Текст.
		/// </summary>
		public string Text { get; private set; }

		/// <summary>
		/// 1 секунда при ~60 FPS
		/// </summary>
		public int Lifetime { get; private set; } = 60;

		/// <summary>
		/// Признак активности.
		/// </summary>
		public bool IsActive => Lifetime > 0;

		/// <summary>
		/// Шрифт.
		/// </summary>
		private Font font = new Font("Arial", 12, FontStyle.Bold);

		/// <summary>
		/// Кисть.
		/// </summary>
		private Brush brush = Brushes.Red;

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="position">Point</param>
		/// <param name="text">string</param>
		public DamageText(Point position, string text)
		{
			Position = new Point(position.X - 15, position.Y - 20);
			Text = text;
		}

		/// <summary>
		/// Обновление с движением вверх.
		/// </summary>
		public void Update()
		{
			if (Lifetime > 0)
			{
				Lifetime--;
				Position = new Point(Position.X, Position.Y - 1);
			}
		}

		/// <summary>
		/// Отрисовка.
		/// </summary>
		/// <param name="g">Graphics</param>
		public void Draw(Graphics g)
		{
			if (!IsActive) return;
			g.DrawString(Text, font, brush, Position);
		}
	}
}