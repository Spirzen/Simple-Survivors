using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simple_Survivors
{
	/// <summary>
	/// Выбор бонуса.
	/// </summary>
	public partial class BonusForm : Form
	{
		/// <summary>
		/// Выбранный бонус.
		/// </summary>
		public int SelectedBonus = -1;

		/// <summary>
		/// Конструктор.
		/// </summary>
		public BonusForm()
		{
			this.Text = "Выберите бонус";
			this.StartPosition = FormStartPosition.CenterParent;
			this.FormBorderStyle = FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Size = new Size(300, 150);

			Button b1 = new Button { Text = "+1 к скорости игрока", Location = new Point(20, 20), Width = 240 };
			Button b2 = new Button { Text = "+1 к снарядам", Location = new Point(20, 50), Width = 240 };
			Button b3 = new Button { Text = "+1 к здоровью", Location = new Point(20, 80), Width = 240 };

			b1.Click += (s, e) => { SelectedBonus = 0; DialogResult = DialogResult.OK; Close(); };
			b2.Click += (s, e) => { SelectedBonus = 1; DialogResult = DialogResult.OK; Close(); };
			b3.Click += (s, e) => { SelectedBonus = 2; DialogResult = DialogResult.OK; Close(); };

			Controls.Add(b1);
			Controls.Add(b2);
			Controls.Add(b3);
		}
	}
}
