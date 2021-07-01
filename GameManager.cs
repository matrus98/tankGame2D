using TankGame2D.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TankGame2D
{
    class GameManager
    {
        public Player player;

        public int BallCountTotal { get; private set; }
        private int countOfLeftBalls;
        public List<Ball> balls;

        public List<Obstacle> obstacles;
        public HorizontalWall horizontalWall;
        public VerticalWall verticalWall_left;
        public VerticalWall verticalWall_right;

        PictureBox pictureBox;
        Timer timer;

        private float SquareSize;
        private const int ballSize = 10;
        const int squarePerRow = 10;
        const int countOfObstaclesToCreate = 3;
        public int Score { get; private set; }

        static Random random = new Random();

        public GameManager(PictureBox pictureBox, Timer timer)
        {
            Score = 0;
            BallCountTotal = 5;

            this.timer = timer;
            timer.Tick += createBalls;

            player = new Player(
                    pictureBox.Width / 2,
                    pictureBox.Height - Resources.tank.Height / 4,
                    Resources.tank.Width * pictureBox.Width / 3000,
                    Resources.tank.Height * pictureBox.Height / 2500,
                    ballSize
                );

            this.pictureBox = pictureBox;

            obstacles = new List<Obstacle>();
            balls = new List<Ball>();

            createWalls();

            SquareSize = (pictureBox.Height - horizontalWall.Height) / 10;

            createObstacles();
        }

        private void createWalls()
        {
            horizontalWall =
                new HorizontalWall(
                    0,
                    0,
                    pictureBox.Width,
                    pictureBox.Height / 10,
                    balls
                );

            verticalWall_left =
                new VerticalWall(
                    0,
                    pictureBox.Height / 10,
                    pictureBox.Width / 10,
                    pictureBox.Height - pictureBox.Height / 10,
                    balls
                );

            verticalWall_right =
                new VerticalWall(
                    pictureBox.Width - pictureBox.Width / 10,
                    pictureBox.Height / 10,
                    pictureBox.Width / 10,
                    pictureBox.Height - pictureBox.Height / 10,
                    balls
                );

            obstacles.Add(horizontalWall);
            obstacles.Add(verticalWall_left);
            obstacles.Add(verticalWall_right);
        }

        public void Shot(int cursorX, int cursorY)
        {
            if (player.CanShot)
            {
                player.CanShot = false;
                countOfLeftBalls = BallCountTotal;
                player.CalculateAngle_And_DetermineVelocities(cursorX, cursorY);
            }
        }

        private void createBalls(object sender, EventArgs e)
        {
            if (!player.CanShot && countOfLeftBalls > 0)
            {
                if (balls.Count > 0)
                {
                    Ball ball = balls.Last();

                    if (Math.Sqrt(Math.Pow(player.X - ball.X, 2) + Math.Pow(player.Y - ball.Y, 2)) <= ball.Width * 3)
                        return;
                }

                balls.Add(player.Shot());
                countOfLeftBalls--;
            }
        }

        public void UpdateMovement()
        {
            if(!player.CanShot && balls.Count == 0 && countOfLeftBalls <= 0)
            {
                player.CanShot = true;

                float y = 0;

                foreach (Obstacle o in obstacles)
                    if (o is Square && o.Y > y)
                        y = o.Y;

                if(y + SquareSize * 2 > player.Y)
                {
                    MessageBox.Show("This is the end. Obstacles have reached You.\nYour score: " + Score, "End of game!", MessageBoxButtons.OK);

                    Application.Exit();
                }

                foreach (Obstacle o in obstacles.ToArray())
                {
                    if (o is Square)
                        ((Square)o).PerformFall(player.Ball_Radius_When_Spawned);
                    else if (o is PowerUp)
                        ((PowerUp)o).PerformFall(SquareSize + player.Ball_Radius_When_Spawned, player.Y);
                    else if (o is Splitter) {
                        if (!(o as Splitter).Am_I_Already_Destroyed)
                            (o as Splitter).PerformFall(SquareSize + player.Ball_Radius_When_Spawned, player.Y);
                        else
                            obstacles.Remove(o);
                    }
                }

                createObstacles();

                return;
            }

            for (int i = 0; i < balls.Count; i++)
            {
                if(balls[i].Y > pictureBox.Height)
                {
                    balls.Remove(balls[i]);
                    continue;
                }

                balls[i].Move();
            }
        }

        public void DetectCollision()
        {
            foreach (Obstacle o in obstacles.ToArray())
                o.DetectCollision();
        }

        private void createObstacles()
        {
            int obstaclesAvaibleToCreate = squarePerRow;

            float a = verticalWall_left.Width;
            float[] xpoints = new float[squarePerRow];

            for (int i = 0; i < squarePerRow; i++)
            {
                xpoints[i] = a + i * (SquareSize + player.Ball_Radius_When_Spawned) + player.Ball_Radius_When_Spawned * 2.5f;
            }

            float ypoint = horizontalWall.Height + player.Ball_Radius_When_Spawned * 2;

            int[] positions = new int[squarePerRow];

            for (int i = 0; i < squarePerRow; i++)
                positions[i] = i;

            int choosenOne, tmp;

            for(int i = 0; i < countOfObstaclesToCreate; i++)
            {
                choosenOne = random.Next(0, obstaclesAvaibleToCreate);

                tmp = positions[choosenOne];
                positions[choosenOne] = positions[obstaclesAvaibleToCreate - 1];
                positions[obstaclesAvaibleToCreate - 1] = tmp;

                obstaclesAvaibleToCreate--;
            }

            int r;
            for(int i = squarePerRow - 1; i > squarePerRow - 1 - countOfObstaclesToCreate; i--)
            {
                r = random.Next(0, 100);

                if (r < 70)
                {
                    Square square = new Square((int)xpoints[positions[i]], (int)ypoint, SquareSize, SquareSize, balls, random.Next(BallCountTotal / 2, BallCountTotal * 2));

                    square.OnObjectDestroyed += (squareObject, collision) =>
                    {
                        Score += square.MaxHealthPoints * 10;
                        obstacles.Remove(squareObject);
                    };

                    obstacles.Add(square);
                }
                else if(r >= 70 && r < 90)
                {
                    PowerUp powerUp = new PowerUp(
                        (int)(xpoints[positions[i]] + SquareSize / 2 - ballSize),
                        (int)(ypoint + SquareSize / 2 - ballSize),
                        ballSize * 2,
                        ballSize * 2,
                        balls
                    );

                    powerUp.OnObjectDestroyed += (powerUpObject, collision) =>
                    {
                        if(collision)
                            BallCountTotal++;

                        obstacles.Remove(powerUpObject);
                    };

                    obstacles.Add(powerUp);
                }

                else
                {
                    Splitter splitter = new Splitter(
                        (int)(xpoints[positions[i]] + SquareSize / 2.5f - ballSize),
                        (int)(ypoint + SquareSize / 2.5f - ballSize),
                        ballSize * 3,
                        ballSize * 3,
                        balls,
                        player.Core_Velocity_Core_Radius);

                    obstacles.Add(splitter);
                }
            }
        }

        //-----------------------------------------------
        //| Update game objects on Windows shape change |
        //-----------------------------------------------
        /*
        public void UpdateWalls()
        {
            horizontalWall.Change_Position_And_Shape
                (
                    0,
                    0,
                    pictureBox.Width,
                    pictureBox.Height / 10
                );

            verticalWall_left.Change_Position_And_Shape
                (
                    0,
                    pictureBox.Height / 10,
                    pictureBox.Width / 10,
                    pictureBox.Height - pictureBox.Height / 10
                );

            verticalWall_right.Change_Position_And_Shape
                (
                    pictureBox.Width - pictureBox.Width / 10,
                    pictureBox.Height / 10,
                    pictureBox.Width / 10,
                    pictureBox.Height - pictureBox.Height / 10
                );
        }

        public void UpdatePlayer()
        {
            float newX = player.X + player.Width < verticalWall_right.X - player.VX
                ? player.X : verticalWall_right.X - player.Width;

            newX = newX > verticalWall_left.X + verticalWall_left.Width + player.VX
                ? newX : verticalWall_left.X + verticalWall_left.Width + player.VX;

            float ground = Resources.tank.Height * pictureBox.Height / 2000;

            player.UpdateTank(
                    newX,
                    pictureBox.Height - ground,
                    Resources.tank.Width * pictureBox.Width / 3000,
                    ground
                );
        }

        public void UpdateBalls()
        {
            int newShape = pictureBox.Height / 50;

            player.UpdateBallShape(newShape);

            foreach (Ball b in balls)
                b.UpdateShape(newShape, newShape);
        }*/
    }
}
