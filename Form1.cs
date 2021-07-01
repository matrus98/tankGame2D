using BBTan.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BBTan
{
    public partial class Form1 : Form
    {
        GameManager gameManager;

        int cursorInWindow_X;
        int cursorInWindow_Y;

        public Form1()
        {
            InitializeComponent();

            gameManager = new GameManager(pictureBox1, timer1);

            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
        }

        private void setCursorVariables()
        {
            cursorInWindow_X = Cursor.Position.X - Left - Cursor.Size.Width / 3;
            cursorInWindow_Y = Cursor.Position.Y - Top - Cursor.Size.Height;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            setCursorVariables();

            gameManager.UpdateMovement();

            gameManager.DetectCollision();

            pictureBox1.Refresh();
            pictureBox2.Refresh();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            SolidBrush brush = new SolidBrush(Color.Black);

            e.Graphics.FillRectangle(brush, 0, 0, pictureBox1.Width, pictureBox1.Height);
            DrawWalls(e);
            DrawSquares(e);

            e.Graphics.DrawImage(
                    Resources.tank,
                    gameManager.player.X,
                    gameManager.player.Y,
                    gameManager.player.Width,
                    gameManager.player.Height
                );

            /*Point[] points = { 
                new Point(
                        (int)(gameManager.player.X + gameManager.player.Width / 2 - gameManager.player.Width / 10),
                        (int)(gameManager.player.Y)
                    ),
                new Point(
                        (int)(gameManager.player.X + gameManager.player.Width / 2- gameManager.player.Width / 10),
                        (int)(gameManager.player.Y - gameManager.player.Height)
                    ),
                new Point(
                        (int)(gameManager.player.X + gameManager.player.Width / 2 + gameManager.player.Width / 10), 
                        (int)(gameManager.player.Y)
                    )
            };*/

            /*e.Graphics.DrawImage(
                    Resources.lufa_testowa,
                    //points
                    //gameManager.player.X + gameManager.player.Width / 2,
                    //gameManager.player.Y,
                    gameManager.player.CannonX,
                    gameManager.player.CannonY,
                    gameManager.player.Width / 2,
                    gameManager.player.Height / 6
                );*/

            brush.Color = Color.Aqua;

            e.Graphics.FillRectangle(brush, cursorInWindow_X, cursorInWindow_Y, 5, 5);

            foreach(Ball ball in gameManager.balls)
            {
                brush.Color = Color.Aqua;
                e.Graphics.FillEllipse(brush, ball.X, ball.Y, ball.Width, ball.Height);
                brush.Color = Color.Yellow;
                foreach (Coordinates c in ball.collisionMeshOffSet)
                {
                    e.Graphics.FillEllipse(brush, c.x + ball.CenterX, c.y + ball.CenterY, 2, 2);
                }
            }
        }

        private void DrawWalls(PaintEventArgs e)
        {
            e.Graphics.DrawImage(
                Resources.wall2,
                gameManager.horizontalWall.X,
                gameManager.horizontalWall.Y,
                gameManager.horizontalWall.Width,
                gameManager.horizontalWall.Height
            );

            e.Graphics.DrawImage(
                Resources.wall2,
                gameManager.verticalWall_left.X,
                gameManager.verticalWall_left.Y,
                gameManager.verticalWall_left.Width,
                gameManager.verticalWall_left.Height
            );

            e.Graphics.DrawImage(
                Resources.wall2,
                gameManager.verticalWall_right.X,
                gameManager.verticalWall_right.Y,
                gameManager.verticalWall_right.Width,
                gameManager.verticalWall_right.Height
            );
        }

        private void DrawSquares(PaintEventArgs e)
        {
            SolidBrush b = new SolidBrush(Color.Green);
            SolidBrush c = new SolidBrush(Color.Blue);
            Font drawFont = new Font("Arial", 16);

            int red, green;

            foreach (Square s in gameManager.obstacles.FindAll(s => s is Square))
            {
                red = 50 * (s.MaxHealthPoints - s.CurrentHealthPoints);
                red = red > 255 ? 255 : red;

                green = 50 * s.CurrentHealthPoints;
                green = green > 255 ? 255 : green;

                b.Color = Color.FromArgb(red, green, 0);
                e.Graphics.FillRectangle(b, s.X, s.Y, s.Width, s.Height);
                e.Graphics.DrawString(s.CurrentHealthPoints.ToString(), drawFont, c, new PointF(s.CenterX - s.Width / 4, s.CenterY - s.Height / 4));
            }

            foreach(PowerUp p in gameManager.obstacles.FindAll(pu => pu is PowerUp))
            {
                e.Graphics.DrawImage(Resources.powerUp, p.X, p.Y, p.Width, p.Height);
            }

            foreach (Splitter s in gameManager.obstacles.FindAll(sp => sp is Splitter))
            {
                e.Graphics.DrawImage(Resources.Splitter, s.X, s.Y, s.Width, s.Height);
            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            //gameManager.UpdateWalls();
            //gameManager.UpdatePlayer();
            //gameManager.UpdateBalls();
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)97: //a
                    if(timer1.Enabled)
                        gameManager.player.MoveLeft(gameManager.verticalWall_left.X + gameManager.verticalWall_left.Width);
                    break;
                case (char)100: //d
                    if (timer1.Enabled)
                        gameManager.player.MoveRight(gameManager.verticalWall_right.X);
                    break;
                case (char)112: //p (pause)
                    timer1.Enabled = !timer1.Enabled;
                    break;
                case (char)113: //q
                    break;
                case (char)101: //e
                    break;
                case (char)32: //spacja
                    if (timer1.Enabled)
                        gameManager.Shot(cursorInWindow_X, cursorInWindow_Y);
                    break;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            gameManager.Shot(cursorInWindow_X, cursorInWindow_Y);
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            SolidBrush backBrush = new SolidBrush(Color.Black);
            SolidBrush brushString = new SolidBrush(Color.Yellow);

            Font drawFont = new Font("Arial", 16);

            e.Graphics.FillRectangle(backBrush, 0, 0, pictureBox1.Width, pictureBox1.Height);

            string text = 
                String.Format("Score: {0} \n\n" + 
                              "Movement Info \n" +
                              "Space - Shot \n" +
                              "LMB - Shot \n" +
                              "A - move left \n" +
                              "D - move right \n" +
                              "P - pause game\n\n" +
                              "Balls: {1}"
                              , gameManager.Score,
                              gameManager.BallCountTotal);

            e.Graphics.DrawString(text, drawFont, brushString, 0, 0);
        }
    }
}
