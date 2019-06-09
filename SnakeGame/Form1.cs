using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
 
    public partial class Form1 : Form
    {
        private List<Circle> snake = new List<Circle>();
        private Circle food = new Circle();
        
        public Form1()
        {
            
            InitializeComponent();

            new Settings();

            gameTimer.Interval = 3000/Settings.Speed;
            gameTimer.Tick += UpdateScreen;
            gameTimer.Start();

            StartGame();
        }
        
        private void UpdateScreen(object sender, EventArgs e)
        {
            if (Settings.GameOver == true)
            {
                if (Input.KeyPressed(Keys.Enter))
                {
                    StartGame();
                }
            }
            else
            {
                if (Input.KeyPressed(Keys.Right) && Settings.Direction != Direction.Left)
                    Settings.Direction = Direction.Right;
                else if (Input.KeyPressed(Keys.Left) && Settings.Direction != Direction.Right)
                    Settings.Direction = Direction.Left;
                else if (Input.KeyPressed(Keys.Up) && Settings.Direction != Direction.Down)
                    Settings.Direction = Direction.Up;
                else if (Input.KeyPressed(Keys.Down) && Settings.Direction != Direction.Up)
                    Settings.Direction = Direction.Down;

                MovePlayer();
            }

            pbCanvas.Invalidate();
        }

        private void StartGame()
        {
            lblGameOver.Visible = false;

            new Settings();

            snake.Clear();
            Circle head = new Circle();
            head.X = 10;
            head.Y = 5;
            snake.Add(head);

            lvlScore.Text = Settings.Score.ToString();
            GenerateFood();

        }

        private void GenerateFood()
        {
            int maxXPos = pbCanvas.Size.Width / Settings.Width;
            int maxYPos = pbCanvas.Size.Height / Settings.Height;

            Random r = new Random();

            food = new Circle();
            food.X = r.Next(0, maxXPos);
            food.Y = r.Next(0, maxYPos);
        }

        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            if (!Settings.GameOver)
            {
                Brush snakeColor;

                for (int i = 0; i < snake.Count; i++)
                {
                    if (i == 0)
                        snakeColor = Brushes.IndianRed;
                    else
                        snakeColor = Brushes.DarkSeaGreen;

                    canvas.FillEllipse(snakeColor, new Rectangle(snake[i].X * Settings.Width, snake[i].Y * Settings.Height, Settings.Width, Settings.Height));

                    canvas.FillEllipse(Brushes.Aqua, new Rectangle(food.X* Settings.Width, food.Y* Settings.Height, Settings.Width, Settings.Height));
                }
            }
            else
            {
                string gameOver = "Game Over \n Your final score - "+Settings.Score+"\n Press Enter to try again";
                lblGameOver.Text = gameOver;
                lblGameOver.Visible = true;
            }
        }

        private void MovePlayer()
        {
            for (int i=snake.Count-1; i>=0; i-- )
            {
                if (i==0)
                {
                    switch(Settings.Direction)
                    {
                        case Direction.Right:
                            snake[i].X++;
                            break;
                        case Direction.Left:
                            snake[i].X--;
                            break;
                        case Direction.Up:
                            snake[i].Y--;
                            break;
                        case Direction.Down:
                            snake[i].Y++;
                            break;

                    }

                    int maxXPos = pbCanvas.Size.Width / Settings.Width;
                    int maxYPos = pbCanvas.Size.Height / Settings.Height;

                        if (snake[i].X < 0 || snake[i].Y < 0 || snake[i].X >= maxXPos || snake[i].Y >= maxYPos)
                        {
                            Die();
                        }

                    for (int j = 1; j <snake.Count; j++)
                    {
                        if (snake[i].X == snake[j].X && snake[i].Y == snake[j].Y)
                        {
                            Die();
                        }
                    }

                    if (snake[0].X == food.X && snake[0].Y == food.Y)
                    {
                        Eat();
                    }
                }

                else
                {
                    snake[i].X = snake[i - 1].X;
                    snake[i].Y = snake[i - 1].Y;
                }
            }
        }

        private void Eat()
        {
            Circle food = new Circle();
            food.X = snake[snake.Count - 1].X;
            food.Y = snake[snake.Count - 1].Y;

            snake.Add(food);

            Settings.Score += Settings.Points;
            lvlScore.Text = Settings.Score.ToString();

            GenerateFood();
        }

        private void Die()
        {
            Settings.GameOver = true;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, true);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, false);
        }
    }
}
