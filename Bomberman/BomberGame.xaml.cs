using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Threading;

enum MOVE { UP, DOWN, LEFT, RIGHT, UP_STAT, DOWN_STAT, LEFT_STAT, RIGHT_STAT };

namespace Bomberman
{
    /// <summary>
    /// Interaction logic for BomberGame.xaml
    /// </summary>
    public partial class BomberGame : UserControl
    {
        static string graphics = "C:/Users/mli/Documents/MECH423/Bomberman/Bomberman/graphics/";
        BitmapImage BACKGROUND = new BitmapImage(new Uri(graphics + "Menu/title_background.jpg"));

        // timer
        DispatcherTimer dispatcherTimer;

        // blocks
        Image[,] solidArray;
        BitmapImage solidBlock = new BitmapImage(new Uri(graphics + "Blocks/SolidBlock.png"));

        // character
        class Bomber
        {
            public MOVE state;
            public Image img;
            static BitmapImage down = new BitmapImage(new Uri(graphics + "Bomberman/down.gif"));
            static BitmapImage down_static = new BitmapImage(new Uri(graphics + "Bomberman/Front/Bman_F_f00.png"));
            static BitmapImage up = new BitmapImage(new Uri(graphics + "Bomberman/up.gif"));
            static BitmapImage up_static = new BitmapImage(new Uri(graphics + "Bomberman/Back/Bman_B_f00.png"));
            static BitmapImage left = new BitmapImage(new Uri(graphics + "Bomberman/left.gif"));
            static BitmapImage left_static = new BitmapImage(new Uri(graphics + "Bomberman/Left/Bman_F_f00.png"));
            static BitmapImage right = new BitmapImage(new Uri(graphics + "Bomberman/right.gif"));
            static BitmapImage right_static = new BitmapImage(new Uri(graphics + "Bomberman/Right/Bman_F_f00.png"));
            int offset = 64;
            // threshold for orientation
            static int ORIENTATION_LOW = 105;
            static int ORIENTATION_UP = 155;

            public Bomber()
            {
                state = MOVE.DOWN_STAT;
                img = new Image
                {
                    Source = down_static,
                    Width = down.Width,
                    Height = down.Height
                    
                };
                Canvas.SetTop(img, -offset);
            }
            public void UpdateGraphics()
            {
                switch (state)
                {
                    case MOVE.DOWN:
                        img.Source = down;
                        break;
                    case MOVE.DOWN_STAT:
                        img.Source = down_static;
                        break;
                    case MOVE.LEFT:
                        img.Source = left;
                        break;
                    case MOVE.LEFT_STAT:
                        img.Source = left_static;
                        break;
                    case MOVE.RIGHT:
                        img.Source = right;
                        break;
                    case MOVE.RIGHT_STAT:
                        img.Source = right_static;
                        break;
                    case MOVE.UP:
                        img.Source = up;
                        break;
                    case MOVE.UP_STAT:
                        img.Source = up_static;
                        break;
                }
            }

            public void UpdatePosition(int xData, int yData, int zData)
            {
                MOVE pre_state = state;
                // update orientation
                if (xData > ORIENTATION_UP)
                {
                    Canvas.SetTop(img, Canvas.GetTop(img)-2);
                    state = MOVE.UP;
                }
                else if (xData < ORIENTATION_LOW)
                {
                    Canvas.SetTop(img, Canvas.GetTop(img) + 2);
                    state = MOVE.DOWN;
                }
                else if (yData > ORIENTATION_UP)
                {
                    Canvas.SetLeft(img, Canvas.GetLeft(img) - 2);
                    state = MOVE.LEFT;
                }
                else if (yData < ORIENTATION_LOW)
                {
                    Canvas.SetLeft(img, Canvas.GetLeft(img) + 2);
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
                    UpdateGraphics();
            }
        }
        Bomber bomber = new Bomber();

        // accelerometer data
        Mutex mut;
        List<int> x_acc, y_acc, z_acc;
        int xData, yData, zData;

        public BomberGame(ref Mutex m, ref List<int> x, ref List<int> y,
                          ref List<int> z)
        {
            InitializeComponent();
            this.Background = new ImageBrush(BACKGROUND);

            mut = m;
            x_acc = x;
            y_acc = y;
            z_acc = z;

            // initialize timer
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            
        }

        public void StartGame()
        {
            dispatcherTimer.Start();
            this.Background = new ImageBrush {
                ImageSource = new BitmapImage(new Uri(graphics + "Blocks/BackgroundTile.png")),
                TileMode = TileMode.Tile,
                ViewportUnits = BrushMappingMode.Absolute,
                Viewport = new Rect(0,0,64,64)
            };
            SpawnSolidBlock();
            SpawnCharacter();
        }

        public void StopGame()
        {
            this.Background = new ImageBrush(BACKGROUND);
            canvas.Children.Clear();
        }

        private void SpawnSolidBlock()
        {
            solidArray = new Image[2, 2];

            for (int i = 0; i < solidArray.GetLength(0); i++)
            {
                for (int j = 0; j < solidArray.GetLength(1); j++)
                {
                    solidArray[i, j] = new Image
                    {
                        Source = solidBlock,
                        Width = solidBlock.Width,
                        Height = solidBlock.Height,
                    };
                    Canvas.SetTop(solidArray[i, j], solidBlock.Width * (i + 1) * (i+1));
                    Canvas.SetLeft(solidArray[i, j], solidBlock.Height * (j + 1) * (j+1));
                    canvas.Children.Add(solidArray[i,j]);
                }
            }
        }

        private void SpawnCharacter()
        {
            canvas.Children.Add(bomber.img);
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            mut.WaitOne(100);
            //Console.WriteLine(x_acc[x_acc.Count-1]);
            xData = x_acc[x_acc.Count - 1];
            yData = y_acc[y_acc.Count - 1];
            zData = z_acc[z_acc.Count - 1];


            bomber.UpdatePosition(xData, yData, zData);
            //if (zData > startValue)
                //PutBomb(character.Location);
            mut.ReleaseMutex(); 
        }
    }
}
