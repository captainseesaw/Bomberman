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
//using System.Threading;
using System.Windows.Threading;
using WpfAnimatedGif;
using System.Timers;

public enum MOVE { UP, DOWN, LEFT, RIGHT, UP_STAT, DOWN_STAT, LEFT_STAT, RIGHT_STAT };

namespace Bomberman
{
    /// <summary>
    /// Interaction logic for BomberGame.xaml
    /// </summary>
    /// 
    public class Character
    {
        public MOVE direction;
        public Image img;
        public int offset; // vertical offset
        static string graphics = "C:/Users/mli/Documents/MECH423/Bomberman/Bomberman/graphics/";
        public BitmapImage down, down_static, up, up_static, left, left_static, right, right_static;

        public Character(string name)
        {
            if (name == "bomberman")
            {
                down = new BitmapImage(new Uri(graphics + "Bomberman/down.gif"));
                down_static = new BitmapImage(new Uri(graphics + "Bomberman/Front/Bman_F_f00.png"));
                up = new BitmapImage(new Uri(graphics + "Bomberman/up.gif"));
                up_static = new BitmapImage(new Uri(graphics + "Bomberman/Back/Bman_B_f00.png"));
                left = new BitmapImage(new Uri(graphics + "Bomberman/left.gif"));
                left_static = new BitmapImage(new Uri(graphics + "Bomberman/Left/Bman_F_f00.png"));
                right = new BitmapImage(new Uri(graphics + "Bomberman/right.gif"));
                right_static = new BitmapImage(new Uri(graphics + "Bomberman/Right/Bman_F_f00.png"));
                offset = 64;
            }

            direction = MOVE.DOWN_STAT;
            img = new Image
            {
                Width = down.Width,
                Height = down.Height,
            };
            Canvas.SetTop(img, -offset);
            Canvas.SetLeft(img, 0);
            ImageBehavior.SetAnimatedSource(img, down_static);
        }
    }

    public class Bomb
    {
        public Image img;
        static string graphics = "C:/Users/mli/Documents/MECH423/Bomberman/Bomberman/graphics/";
        public BitmapImage bomb = new BitmapImage(new Uri(graphics + "Bomb/bomb.gif")), flame;
        bool bomb_init = false;
        Timer timer;


        // TODO - bomb doesnt initialize first time?
        // - bomb timer
        // - bomb explode
        public Bomb()
        {
            timer = new Timer();
            img = new Image
            {
                Width = bomb.Width,
                Height = bomb.Height,
                Visibility = Visibility.Hidden
            }; 
            ImageBehavior.SetAnimatedSource(img, bomb);
        }
        public void Initialize(double xData, double yData)
        {
            Canvas.SetTop(img, yData);
            Canvas.SetLeft(img, xData);
            img.Visibility = Visibility.Visible;
            timer.Start();
            bomb_init = true;
        }
        public bool GetBombInitialization()
        {
            return bomb_init;
        }
    }

    public partial class BomberGame : UserControl
    {
        static string graphics = "C:/Users/mli/Documents/MECH423/Bomberman/Bomberman/graphics/";
        BitmapImage BACKGROUND = new BitmapImage(new Uri(graphics + "Menu/title_background.jpg"));
        static int ORIENTATION_LOW = 105;
        static int ORIENTATION_UP = 155;

        // timer
        DispatcherTimer dispatcherTimer;

        // blocks
        Image[,] solidArray;
        BitmapImage solidBlock = new BitmapImage(new Uri(graphics + "Blocks/SolidBlock.png"));

        // characters
        Character bomber;

        // bomb
        Bomb bomb;
        int bomb_thres = 200;

        // accelerometer data
        System.Threading.Mutex mut;
        List<int> x_acc, y_acc, z_acc;
        int xData, yData, zData;

        public BomberGame(ref System.Threading.Mutex m, ref List<int> x, ref List<int> y,
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
            SpawnBomb();
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
            bomber = new Character("bomberman"); 
            canvas.Children.Add(bomber.img);
        }

        private void SpawnBomb()
        {
            bomb = new Bomb();
            canvas.Children.Add(bomb.img);
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            mut.WaitOne(100);
            xData = x_acc[x_acc.Count - 1];
            yData = y_acc[y_acc.Count - 1];
            zData = z_acc[z_acc.Count - 1];
            mut.ReleaseMutex();

            UpdatePosition(ref bomber, xData, yData, zData);

            if (zData > bomb_thres && !bomb.GetBombInitialization())
                bomb.Initialize(Canvas.GetLeft(bomber.img),Canvas.GetTop(bomber.img)+bomber.offset);

        }

        public void UpdateGraphics(ref Character c)
        {
            switch (c.direction)
            {
                case MOVE.DOWN:
                    ImageBehavior.SetAnimatedSource(c.img, c.down);
                    break;
                case MOVE.DOWN_STAT:
                    ImageBehavior.SetAnimatedSource(c.img, c.down_static);
                    break;
                case MOVE.LEFT:
                    ImageBehavior.SetAnimatedSource(c.img, c.left);
                    break;
                case MOVE.LEFT_STAT:
                    ImageBehavior.SetAnimatedSource(c.img, c.left_static);
                    break;
                case MOVE.RIGHT:
                    ImageBehavior.SetAnimatedSource(c.img, c.right);
                    break;
                case MOVE.RIGHT_STAT:
                    ImageBehavior.SetAnimatedSource(c.img, c.right_static);
                    break;
                case MOVE.UP:
                    ImageBehavior.SetAnimatedSource(c.img, c.up);
                    break;
                case MOVE.UP_STAT:
                    ImageBehavior.SetAnimatedSource(c.img, c.up_static);
                    break;
            }
        }

        public void UpdatePosition(ref Character c, int xData, int yData, int zData)
        {
            MOVE pre_state = c.direction;
            // update orientation
            if (xData > ORIENTATION_UP)
            {
                c.direction = MOVE.UP;
                Canvas.SetTop(c.img, Canvas.GetTop(c.img) - 2); // attempt move
                if (CheckBlockCollision(ref c) || CheckWallCollision(ref c)) // if collision, revert move
                    Canvas.SetTop(c.img, Canvas.GetTop(c.img) + 2);
            }
            else if (xData < ORIENTATION_LOW)
            {
                c.direction = MOVE.DOWN;
                Canvas.SetTop(c.img, Canvas.GetTop(c.img) + 2);
                if (CheckBlockCollision(ref c) || CheckWallCollision(ref c))
                    Canvas.SetTop(c.img, Canvas.GetTop(c.img) - 2);
            }
            else if (yData > ORIENTATION_UP)
            {
                c.direction = MOVE.LEFT;
                Canvas.SetLeft(c.img, Canvas.GetLeft(c.img) - 2);
                if (CheckBlockCollision(ref c) || CheckWallCollision(ref c))
                    Canvas.SetLeft(c.img, Canvas.GetLeft(c.img) + 2);
            }
            else if (yData < ORIENTATION_LOW)
            {
                c.direction = MOVE.RIGHT;
                Canvas.SetLeft(c.img, Canvas.GetLeft(c.img) + 2);
                if (CheckBlockCollision(ref c) || CheckWallCollision(ref c))
                    Canvas.SetLeft(c.img, Canvas.GetLeft(c.img) - 2);
            }
            else
            {
                if (c.direction == MOVE.DOWN)
                    c.direction = MOVE.DOWN_STAT;
                else if (c.direction == MOVE.LEFT)
                    c.direction = MOVE.LEFT_STAT;
                else if (c.direction == MOVE.RIGHT)
                    c.direction = MOVE.RIGHT_STAT;
                else if (c.direction == MOVE.UP)
                    c.direction = MOVE.UP_STAT;
            }

            if (pre_state != c.direction)
                UpdateGraphics(ref c);
        }

        public bool CheckCollision(ref Image a, int offset_a, Image b, int offset_b, int tolerance)
        {
            double top_a = Canvas.GetTop(a) + offset_a, left_a = Canvas.GetLeft(a),
                   bot_a = top_a + a.Height - offset_a, right_a = left_a + a.Width,
                   top_b = Canvas.GetTop(b) + offset_b+ tolerance, left_b = Canvas.GetLeft(b) + tolerance,
                   bot_b = top_b + b.Height - offset_b - tolerance, right_b = left_b + b.Width - tolerance;

            if (top_a < bot_b && bot_a > top_b && right_a > left_b && left_a < right_b)
                return true;
            return false;
        } // no ref for image b, might be slow

        public bool CheckBlockCollision(ref Character c)
        {
            foreach (Image s in solidArray)
                if (CheckCollision(ref c.img, c.offset, s, 0, 15))
                    return true;
            return false;
        }

        public bool CheckWallCollision(ref Character c)
        {
            double top = Canvas.GetTop(c.img) + c.offset, left = Canvas.GetLeft(c.img),
                   bot = top + c.img.Height - c.offset, right = left + c.img.Width;
            if (top < 0 || bot > this.Height || right > this.Width || left < 0)
                return true;
            return false;
        }
    }
}
