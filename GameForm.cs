using Simple_Survivors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace Simple_Survivors
{
	/// <summary>
	/// Основная форма.
	/// </summary>
	public partial class GameForm : Form
	{
		/// <summary>
		/// Игрок.
		/// </summary>
		private Player player = new Player();
		
		/// <summary>
		/// Враги.
		/// </summary>
		private List<Enemy> enemies = new List<Enemy>();
		
		/// <summary>
		/// Снаряды.
		/// </summary>
		private List<Projectile> projectiles = new List<Projectile>();
		
		/// <summary>
		/// Эффекты.
		/// </summary>
		private List<Effect> effects = new List<Effect>();
		
		/// <summary>
		/// Индикатор урона.
		/// </summary>
		private List<DamageText> damageTexts = new List<DamageText>();
		
		/// <summary>
		/// Игровой таймер (~60 FPS).
		/// </summary>
		private Timer gameTimer = new Timer { Interval = 16 };
		
		/// <summary>
		/// Таймер возрождения врагов.
		/// </summary>
		private Timer enemySpawnTimer = new Timer { Interval = 1000 };

		/// <summary>
		/// Таймер атаки.
		/// </summary>
		private Timer attackTimer = new Timer { Interval = 2000 };

		/// <summary>
		/// Количество очков.
		/// </summary>
		private int score = 0;

		/// <summary>
		/// Уровень игрока.
		/// </summary>
		private int level = 1;

		/// <summary>
		/// Количество опыта до следующего уровня.
		/// </summary>
		private int nextLevelXP = 10;

		/// <summary>
		/// Количество снарядов за одну атаку.
		/// </summary>
		private int projectilesPerAttack = 1;

		/// <summary>
		/// Количество убитых врагов.
		/// </summary>
		private int enemiesKilled = 0;

		/// <summary>
		/// Признак уведомления GameOver.
		/// </summary>
		private bool gameOverShown = false;

		/// <summary>
		/// Конструктор класса.
		/// </summary>
		public GameForm()
		{
			InitializeComponent();
			this.DoubleBuffered = true;
			this.ClientSize = new Size(800, 600);
			this.KeyDown += OnKeyDown;
			this.KeyUp += OnKeyUp;

			gameTimer.Tick += GameLoop;
			enemySpawnTimer.Tick += SpawnEnemy;
			attackTimer.Tick += Attack;

			gameTimer.Start();
			enemySpawnTimer.Start();
			attackTimer.Start();
		}

		/// <summary>
		/// Обработчик нажатия на клавишу.
		/// </summary>
		/// <param name="sender">object</param>
		/// <param name="e">KeyEventArgs</param>
		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			player.PressKey(e.KeyCode);
		}

		/// <summary>
		/// Обработчик отпускания клавиши.
		/// </summary>
		/// <param name="sender">object</param>
		/// <param name="e">KeyEventArgs</param>
		private void OnKeyUp(object sender, KeyEventArgs e)
		{
			player.ReleaseKey(e.KeyCode);
		}

		/// <summary>
		/// Игровой цикл.
		/// </summary>
		/// <param name="sender">object</param>
		/// <param name="e">EventArgs</param>
		private void GameLoop(object sender, EventArgs e)
		{
			player.Update(ClientSize);

			foreach (var enemy in enemies)
				enemy.Update(player.Position);

			foreach (var proj in projectiles)
				proj.Update();

			CheckCollisions();

			for (int i = effects.Count - 1; i >= 0; i--)
			{
				effects[i].Update();
				if (!effects[i].IsActive)
					effects.RemoveAt(i);
			}

			foreach (var dt in damageTexts.ToList())
			{
				dt.Update();
				if (!dt.IsActive)
					damageTexts.Remove(dt);
			}

			CheckGameOver();

			Invalidate();
		}

		/// <summary>
		/// Проверка столкновений.
		/// </summary>
		private void CheckCollisions()
		{
			for (int i = projectiles.Count - 1; i >= 0; i--)
			{
				var proj = projectiles[i];
				Enemy closest = null;
				double minDist = double.MaxValue;

				foreach (var enemy in enemies)
				{
					double dist = (enemy.Position.X - proj.Position.X) * (enemy.Position.X - proj.Position.X) +
								  (enemy.Position.Y - proj.Position.Y) * (enemy.Position.Y - proj.Position.Y);
					if (dist < 2000 && dist < minDist)
					{
						minDist = dist;
						closest = enemy;
					}
				}

				if (closest != null)
				{
					enemies.Remove(closest);
					projectiles.RemoveAt(i);
					AddScore(1);
					effects.Add(new Effect(closest.Position));
				}
			}

			foreach (var enemy in enemies)
			{
				if (player.Bounds.IntersectsWith(enemy.Bounds))
				{
					player.TakeDamage();
					enemies.Remove(enemy);
					effects.Add(new Effect(player.Position));
					damageTexts.Add(new DamageText(player.Position, "-1 HP"));
					break;
				}
			}
		}

		/// <summary>
		/// Возрождение врагов.
		/// </summary>
		/// <param name="sender">object</param>
		/// <param name="e">EventArgs</param>
		private void SpawnEnemy(object sender, EventArgs e)
		{
			Random r = new Random();
			enemies.Add(new Enemy(new Point(r.Next(50, ClientSize.Width - 50), r.Next(50, ClientSize.Height - 50))));
		}

		/// <summary>
		/// Атака.
		/// </summary>
		/// <param name="sender">object</param>
		/// <param name="e">EventArgs</param>
		private void Attack(object sender, EventArgs e)
		{
			if (enemies.Count == 0) return;

			List<Enemy> targets = GetClosestEnemies(projectilesPerAttack);

			foreach (var target in targets)
			{
				projectiles.Add(new Projectile(player.Position, target.Position));
			}

			PlayShootSound();
		}

		/// <summary>
		/// Получение врагов, находящихся близко к игроку.
		/// </summary>
		/// <param name="count"></param>
		/// <returns></returns>
		private List<Enemy> GetClosestEnemies(int count)
		{
			var ordered = enemies.OrderBy(e =>
			{
				double dx = e.Position.X - player.Position.X;
				double dy = e.Position.Y - player.Position.Y;
				return dx * dx + dy * dy;
			}).Take(count).ToList();

			return ordered;
		}

		/// <summary>
		/// Добавление очков.
		/// </summary>
		/// <param name="amount"></param>
		private void AddScore(int amount)
		{
			score += amount;
			enemiesKilled += amount;

			if (score >= nextLevelXP)
			{
				LevelUp();
			}
		}

		/// <summary>
		/// Повышение уровня.
		/// </summary>
		private void LevelUp()
		{
			score = 0;
			level++;
			nextLevelXP += 5;

			PauseGame();

			BonusForm form = new BonusForm();
			if (form.ShowDialog() == DialogResult.OK)
			{
				switch (form.SelectedBonus)
				{
					case 0: player.Speed++; break;
					case 1: projectilesPerAttack++; break;
					case 2: player.Health++; break;
				}
			}

			ResumeGame();
		}

		/// <summary>
		/// Проигрывание звуков.
		/// </summary>
		private void PlayShootSound()
		{
			System.Media.SystemSounds.Hand.Play();
		}

		/// <summary>
		/// Пауза игры.
		/// </summary>
		private void PauseGame()
		{
			gameTimer.Stop();
			enemySpawnTimer.Stop();
			attackTimer.Stop();
		}

		/// <summary>
		/// Возобновление игры.
		/// </summary>
		private void ResumeGame()
		{
			gameTimer.Start();
			enemySpawnTimer.Start();
			attackTimer.Start();
		}

		/// <summary>
		/// Проверка окна проигрыша.
		/// </summary>
		private void CheckGameOver()
		{
			if (player.Health <= 0 && !gameOverShown)
			{
				gameOverShown = true;
				ShowGameOver();
			}
		}

		/// <summary>
		/// Отображение окна проигрыша.
		/// </summary>
		private void ShowGameOver()
		{
			PauseGame();

			using (var form = new Form { Size = new Size(400, 300), StartPosition = FormStartPosition.CenterScreen, Text = "Game Over" })
			{
				var label = new Label
				{
					AutoSize = false,
					Width = 360,
					TextAlign = ContentAlignment.MiddleCenter,
					Font = new Font("Arial", 16),
					Text = $"Вы проиграли!\n\nДостигнутый уровень: {level}\nУбито врагов: {enemiesKilled}",
					Location = new Point(20, 50)
				};

				var button = new Button
				{
					Text = "Начать заново",
					Location = new Point(120, 200),
					Width = 160
				};

				button.Click += (s, e) =>
				{
					form.DialogResult = DialogResult.OK;
					form.Close();
				};

				form.Controls.Add(label);
				form.Controls.Add(button);

				if (form.ShowDialog() == DialogResult.OK)
				{
					Application.Restart();
				}
				else
				{
					Application.Exit();
				}
			}
		}

		/// <summary>
		/// Отрисовка графики.
		/// </summary>
		/// <param name="e">PaintEventArgs</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			byte[] backgroundData = Properties.Resources.sand;
			Image background = ImageHelper.ByteArrayToImage(backgroundData);
			e.Graphics.DrawImage(background, new Rectangle(0, 0, this.ClientSize.Width, this.ClientSize.Height));


			player.Draw(e.Graphics);

			foreach (var enemy in enemies)
				enemy.Draw(e.Graphics);

			foreach (var proj in projectiles)
				proj.Draw(e.Graphics);

			foreach (var effect in effects)
				effect.Draw(e.Graphics);

			foreach (var dt in damageTexts)
				dt.Draw(e.Graphics);

			e.Graphics.DrawString($"HP: {player.Health} | XP: {score}/{nextLevelXP} | Уровень: {level}",
				new Font("Arial", 14), Brushes.Black, new PointF(10, 10));
		}
	}
}