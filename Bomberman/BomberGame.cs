using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

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

        // start game
        bool gameStarted = false;
        int startValue = 200;

        // character
        PictureBox character;
        Bitmap solidBlock;
        int offset;

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
            solidBlock = new Bitmap(graphics + "Blocks/SolidBlock.png");
            PictureBox[,] blockArray = new PictureBox[this.Width / solidBlock.Width / 2,
                                                     this.Height / solidBlock.Height / 2];
            for (int i = 0; i < blockArray.GetLength(0); i++)
            {
                for (int j = 0; j < blockArray.GetLength(1); j++)
                {
                    blockArray[i, j] = new PictureBox
                    {
                        Image = solidBlock,
                        Size = solidBlock.Size,
                        Location = new Point((2 * i + 1) * solidBlock.Width,
                                            (2 * j + 1) * solidBlock.Height)
                    };
                    this.Controls.Add(blockArray[i, j]);
                }
            }
            Console.WriteLine(blockArray.Length);
        }

        private void SpawnCharacter()
        {
            Bitmap front_static = new Bitmap(graphics + "Bomberman/Front/Bman_F_f00.png");
            offset = front_static.Height - solidBlock.Height;
            character = new PictureBox
            {
                Image = front_static,
                Size = front_static.Size,
                Location = new Point(0, -offset),
                BackColor = Color.Transparent
            };
            this.Controls.Add(character);
        }
    }
}
