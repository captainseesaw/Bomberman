using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

enum MOVE {UP, DOWN, LEFT, RIGHT, UP_STAT, DOWN_STAT, LEFT_STAT, RIGHT_STAT };

namespace Bomberman
{
    public partial class BomberGame : Form
    {
        static string graphics = "C:/Users/mli/Documents/MECH423/Bomberman/Bomberman/graphics/";
        Bitmap BACKGROUND = new Bitmap(graphics + "Menu/title_background.jpg");

        // accelerometer data
        Mutex mut;
        List<int> x_acc, y_acc, z_acc;
        int xData, yData, zData;

        // threshold for orientation
        int ORIENTATION_LOW = 105;
        int ORIENTATION_UP = 155;
        

        // start game
        bool gameStarted = false;
        int startValue = 200;

        // blocks
        PictureBox[,] solidArray;
        PictureBox[,] explodeArray;

        // bomb
        PictureBox bomb;
        Bitmap bomb_gif = new Bitmap(graphics + "Bomb/bomb.gif");
        PictureBox[,] flame;
        Stopwatch bombWatch = new Stopwatch();

        // character
        PictureBox character;
        MOVE character_move = MOVE.DOWN_STAT;
        Bitmap solidBlock = new Bitmap(graphics + "Blocks/SolidBlock.png");
        Bitmap down = new Bitmap(graphics + "Bomberman/down.gif");
        Bitmap down_static = new Bitmap(graphics + "Bomberman/Front/Bman_F_f00.png");
        Bitmap up = new Bitmap(graphics + "Bomberman/up.gif");
        Bitmap up_static = new Bitmap(graphics + "Bomberman/Back/Bman_B_f00.png");
        Bitmap left = new Bitmap(graphics + "Bomberman/left.gif");
        Bitmap left_static = new Bitmap(graphics + "Bomberman/Left/Bman_F_f00.png");
        Bitmap right = new Bitmap(graphics + "Bomberman/right.gif");
        Bitmap right_static = new Bitmap(graphics + "Bomberman/Right/Bman_F_f00.png");


        private void timer_Tick(object sender, EventArgs e)
        {
            mut.WaitOne(100);
            //Console.WriteLine(x_acc[x_acc.Count-1]);
            xData = x_acc[x_acc.Count - 1];
            yData = y_acc[y_acc.Count - 1];
            zData = z_acc[z_acc.Count - 1];
            mut.ReleaseMutex();

            if (zData > startValue && !gameStarted)
                StartGame();

            if (gameStarted)
            {
                MoveCharacter(ref character,xData, yData, zData, ref character_move);
                if (zData > startValue)
                    PutBomb(character.Location);
            }
        }

        private void StartGame()
        {
            Console.WriteLine("Game started");
            gameStarted = true;
            this.BackgroundImage = new Bitmap(graphics + "Blocks/BackgroundTile.png");
            SpawnSolidBlock();
            SpawnCharacter();
        }

        public BomberGame(ref Mutex m, ref List<int> x, ref List<int> y,
                          ref List<int> z)
        {
            InitializeComponent();
            this.BackgroundImage = BACKGROUND;
            this.Size = BACKGROUND.Size;
            Console.WriteLine(this.Size);

            mut = m;
            x_acc = x;
            y_acc = y;
            z_acc = z;

            timer.Start();
        }

        private void SpawnSolidBlock()
        {
            solidArray = new PictureBox[this.Width / solidBlock.Width / 2,
                                                     this.Height / solidBlock.Height / 2];
            for (int i = 0; i < solidArray.GetLength(0); i++)
            {
                for (int j = 0; j < solidArray.GetLength(1); j++)
                {
                    solidArray[i, j] = new PictureBox
                    {
                        Image = solidBlock,
                        Size = solidBlock.Size,
                        Location = new Point((2 * i + 1) * solidBlock.Width,
                                            (2 * j + 1) * solidBlock.Height)
                    };
                    this.Controls.Add(solidArray[i, j]);
                }
            }
            Console.WriteLine(solidArray.Length);
        }

        private void SpawnCharacter()
        {
            character = new PictureBox
            {
                Image = down_static,
                Size = solidBlock.Size,
                Location = new Point(0,0),
                BackColor = Color.Transparent,
                SizeMode = PictureBoxSizeMode.Zoom
            };

            this.Controls.Add(character);
            character.BringToFront();
        }

        private void MoveCharacter(ref PictureBox target, int xData, int yData, int zData, ref MOVE state)
        {
            MOVE pre_state = state;
            // update orientation
            if (xData > ORIENTATION_UP)
            {
                target.Top -= 2;
                state = MOVE.UP;
            }
            else if (xData < ORIENTATION_LOW)
            {
                target.Top += 2;
                state = MOVE.DOWN;
            }        
            else if (yData > ORIENTATION_UP)
            {
                target.Left -= 2;
                state = MOVE.LEFT;
            }
            else if (yData < ORIENTATION_LOW)
            {
                target.Left += 2;
                state = MOVE.RIGHT;
            }
            else
            {
                if (state == MOVE.DOWN)
                    state = MOVE.DOWN_STAT;
                else if (state == MOVE.LEFT)
                    state = MOVE.LEFT_STAT;
                else if (state == MOVE.RIGHT)
                    state = MOVE.RIGHT_STAT;
                else if (state == MOVE.UP)
                    state = MOVE.UP_STAT;
            }

            if (pre_state != state)
                UpdateCharacter(ref target, ref state);
        }

        private void UpdateCharacter(ref PictureBox target, ref MOVE state)
        {
            switch (state)
            {
                case MOVE.DOWN:
                    target.Image = down;
                    break;
                case MOVE.DOWN_STAT:
                    target.Image = down_static;
                    break;
                case MOVE.LEFT:
                    target.Image = left;
                    break;
                case MOVE.LEFT_STAT:
                    target.Image = left_static;
                    break;
                case MOVE.RIGHT:
                    target.Image = right;
                    break;
                case MOVE.RIGHT_STAT:
                    target.Image = right_static;
                    break;
                case MOVE.UP:
                    target.Image = up;
                    break;
                case MOVE.UP_STAT:
                    target.Image = up_static;
                    break;
            }
        }

        private void PutBomb(Point p)
        {
            bomb = new PictureBox
            {
                Image = bomb_gif,
                BackColor = Color.Transparent,
                Size = bomb_gif.Size,
                Location = p
            };
            this.Controls.Add(bomb);
            bombWatch.Start();
        }
    }
}
