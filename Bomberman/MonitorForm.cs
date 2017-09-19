using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;

enum READ_ACC {X,Y,Z,OVER}
enum GESTURE {IDLE, START, ACQUIRE, Processing, OVER}

namespace Bomberman
{
    public partial class monitorForm : Form
    {
        // threshold for orientation
        int ORIENTATION_LOW = 105;
        int ORIENTATION_UP = 155;

        // serial port
        int baudRate = 128000;
        int dataBits = 8; //bits
        int bytesToRead = 0;

        // dataQueue
        ConcurrentQueue<int> dataQueue = new ConcurrentQueue<int>();
        int itemLimit = 8; // item limit

        // gravity
        int positive_x = 166,
            negative_x = 93,
            positive_y = 165,
            negative_y = 92,
            positive_z = 162,
            negative_z = 87;

        // list for x,y,z values
        List<int> x_acc = new List<int>();
        List<int> y_acc = new List<int>();
        List<int> z_acc = new List<int>();
        List<double> grav = new List<double>();

        // gesture reading
        Stopwatch gestureWatch = new Stopwatch();
        GESTURE gestureState = GESTURE.IDLE;
        string readGesture = "";
        int gestureTimeout = 2000;
        int displayTimeout = 1000;

        // plot 
        int count = 0;

        // bombergame
        BomberGame bomberGame;


        public monitorForm()
        {
            InitializeComponent();

            // initialize serial port
            serialPort.BaudRate = baudRate;
            serialPort.DataBits = dataBits;
            serialPort.Parity = Parity.None;
            serialPort.StopBits = StopBits.One;

            // add wpf
            bomberGame = new BomberGame();
            elementHost.Child = bomberGame;
        }

        private void cmbPorts_Click(object sender, EventArgs e)
        {
            cmbPorts.Items.Clear();
            cmbPorts.Items.AddRange(SerialPort.GetPortNames().ToArray());
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (cmbPorts.SelectedIndex == -1)
            {
                return;
            }
  
            bool connected = serialPort.IsOpen;
            if (connected) { 
                serialPort.Close();
                btnConnect.Text = "Connect";
                Console.WriteLine("disconnected");
                cmbPorts.Enabled = true;
                // stop timer
                timer.Stop();
                dataQueue = new ConcurrentQueue<int>();
            }
            else
            {
                try
                {
                    serialPort.Open();
                }
                catch (System.IO.IOException)
                {
                    MessageBox.Show("Error, com port not found");
                    return;
                }
                btnConnect.Text = "Disconnect";
                Console.WriteLine("connected");
                cmbPorts.Enabled = false;
                // start timer
                timer.Start();
            }
        }

        private void cmbPorts_SelectionChangeCommitted(object sender, EventArgs e)
        {
            serialPort.PortName = cmbPorts.SelectedItem.ToString();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            serialPort.Close();
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // read all available bytes
            if (serialPort.IsOpen)
            {
                int numToRead = serialPort.BytesToRead;
                bytesToRead = numToRead;
                byte[] arr = new byte[numToRead];
                serialPort.Read(arr, 0, numToRead);

                foreach (int data in arr)
                {
                    //while (dataQueue.Count > itemLimit)
                    //    dataQueue.TryDequeue(out int trash);
                    dataQueue.Enqueue(data);
                    //Console.WriteLine(data);
                }
            }
        }

        private void Update_orientation(int xData, int yData, int zData)
        {
            // update orientation
            if (xData > ORIENTATION_UP)
                txtOrientation.Text = "face downward";
            else if (xData < ORIENTATION_LOW)
                txtOrientation.Text = "face forward";
            else if (yData > ORIENTATION_UP)
                txtOrientation.Text = "face left";
            else if (yData < ORIENTATION_LOW)
                txtOrientation.Text = "face right";
            else if (zData > ORIENTATION_UP)
                txtOrientation.Text = "face up";
            else if (zData < ORIENTATION_LOW)
                txtOrientation.Text = "face down";
            else
                txtOrientation.Text = String.Empty;
        } 

        private void Update_btnConnect()
        {
            bool connected = serialPort.IsOpen;
            if (connected)
            {
                btnConnect.Text = "Disconnect";
                cmbPorts.Enabled = false;
            }
            else
            {
                btnConnect.Text = "Connect";
                cmbPorts.Enabled = true;
            }
        }

        private void Calculate_average(int avgCount)
        {
            // calculate average
            if (avgCount != 0)
            {
                double avg_x = x_acc.Average();
                double avg_y = y_acc.Average();
                double avg_z = z_acc.Average();

                double grav_x = (avg_x - negative_x) * 2 * 9.81 / (positive_x - negative_x) - 9.81;
                double grav_y = (avg_y - negative_y) * 2 * 9.81 / (positive_y - negative_y) - 9.81;
                double grav_z = (avg_z - negative_z) * 2 * 9.81 / (positive_z - negative_z) - 9.81;

                txtXAvg.Text = grav_x.ToString();
                txtYAvg.Text = grav_y.ToString();
                txtZAvg.Text = grav_z.ToString();
                //txtGAvg.Text = Math.Sqrt(Math.Pow(grav_x,2)+ Math.Pow(grav_y, 2)+ Math.Pow(grav_z, 2)).ToString();

    //[A, B] --> [a, b]

    //use this formula
    //(val - A) * (b - a) / (B - A) + a

            }
            else
            {
                txtXAvg.Text = String.Empty;
                txtYAvg.Text = String.Empty;
                txtZAvg.Text = String.Empty;
                //txtGAvg.Clear();
            }
        }

        private double Calculate_grav(int xData, int yData, int zData)
        {
            double grav_x = (xData - negative_x) * 2 * 9.81 / (positive_x - negative_x) - 9.81;
            double grav_y = (yData - negative_y) * 2 * 9.81 / (positive_y - negative_y) - 9.81;
            double grav_z = (zData - negative_z) * 2 * 9.81 / (positive_z - negative_z) - 9.81;

            return Math.Sqrt(Math.Pow(grav_x, 2) + Math.Pow(grav_y, 2) + Math.Pow(grav_z, 2));
        }

        private void Calculate_std(int avgCount)
        {
            if (avgCount != 0)
            {
                //Compute the Average      
                double avg_x = x_acc.Average();
                double avg_y = y_acc.Average();
                double avg_z = z_acc.Average();
                double sum_x = 0, sum_y = 0, sum_z = 0;

                //Perform the Sum of (value-avg)_2_2
                for (int i =0; i < x_acc.Count; i++)
                {
                    sum_x += Math.Pow(x_acc[i] - avg_x, 2);
                    sum_y += Math.Pow(y_acc[i] - avg_y, 2);
                    sum_z += Math.Pow(z_acc[i] - avg_z, 2);
                }

                //Put it all together      
                double std_x = Math.Sqrt(sum_x) / avgCount;
                double std_y = Math.Sqrt(sum_y) / avgCount ;
                double std_z = Math.Sqrt(sum_z) / avgCount ;

                txtXStddev.Text = std_x.ToString();
                txtYStddev.Text = std_y.ToString();
                txtZStddev.Text = std_z.ToString();
            }
            else
            {
                txtXStddev.Clear();
                txtYStddev.Clear();
                txtZStddev.Clear();
            }
        }

        private void Update_gesture(int xData, int yData, int zData,
            int xThreshold, int yThreshold, int zThreshold, int yThresholdNeg, int zThresholdNeg)
        {
            switch (gestureState)
            {
                case (GESTURE.START):
                    gestureWatch.Start();
                    txtLED.BackColor = Color.Green;
                    gestureState = GESTURE.ACQUIRE;
                    break;
                case (GESTURE.ACQUIRE):
                    if (gestureWatch.ElapsedMilliseconds > gestureTimeout)
                    {
                        gestureState = GESTURE.OVER;
                        Console.WriteLine("over 2s, restart");
                    }
                    else if (gestureWatch.ElapsedMilliseconds > displayTimeout)
                    {
                        txtGesture.Clear();
                        //Console.WriteLine("over 1s, display clear");
                    }
                    if ( readGesture.Length <= 3)
                    {
                        if (xData > xThreshold && !readGesture.Contains("X") && !readGesture.Contains("Z") && !readGesture.Contains("z"))
                        {
                            readGesture += "X";
                            txtGesture.Text = readGesture;
                        }

                        if (zData < zThresholdNeg && !readGesture.Contains("z") && !readGesture.Contains("Z") && !readGesture.Contains("X") && !readGesture.Contains("Y"))
                        {
                            readGesture += "z";
                            txtGesture.Text = readGesture;
                        }

                        if (zData > zThreshold && !readGesture.EndsWith("Z") && !readGesture.EndsWith("z") && !readGesture.EndsWith("X") && !readGesture.EndsWith("Y") && !readGesture.EndsWith("y"))
                        {
                            readGesture += "Z";
                            txtGesture.Text = readGesture;
                        }

                        if (yData > yThreshold && !readGesture.Contains("Y") && (readGesture.Contains('X') || readGesture.Contains('Z')))
                        {
                            readGesture += "Y";
                            txtGesture.Text = readGesture;
                        }

                        if (yData < yThresholdNeg && !readGesture.EndsWith("y") && readGesture.Contains("ZY"))
                        {
                            readGesture += "y";
                            txtGesture.Text = readGesture;
                        }
                        
                    }
                    switch (readGesture)
                    {
                        case "z":
                            txtGesture.Text = "Free fall";
                            break;
                        case "XY":
                            txtGesture.Text = "Frisbee throw";
                            break;
                        case "ZYy":
                            txtGesture.Text = "Wave";
                            break;
                    }
                    break;
                case (GESTURE.OVER):
                    txtGesture.Clear();
                    txtLED.BackColor = Color.Red;
                    gestureWatch.Reset();
                    gestureState = GESTURE.IDLE;
                    readGesture = "";
                    break;
            }
        }

        private void Update_plot (int xData, int yData, int zData, int count)
        {
            //if (xData > 30)
                chrAcc.Series["X"].Points.AddXY(count, xData);
            //else
                //chart.Series["xData"].Points.Clear();

            //if (checkY.Checked && yData > 30)
                chrAcc.Series["Y"].Points.AddXY(count, yData);
            //else
                //chrAcc.Series["yData"].Points.Clear();

            //if (checkZ.Checked && zData > 30)
                chrAcc.Series["Z"].Points.AddXY(count, zData);
            //else
                //chrAcc.Series["zData"].Points.Clear();

            chrAcc.ChartAreas["ChartArea1"].AxisX.Maximum = count;
            chrAcc.ChartAreas["ChartArea1"].AxisX.Minimum = count - 50;
            chrAcc.ChartAreas["ChartArea1"].AxisY.Maximum = 255;
            chrAcc.ChartAreas["ChartArea1"].AxisY.Minimum = 0;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            int data;

            txtBytesToRead.Text = bytesToRead.ToString();
            txtQueueCount.Text = dataQueue.Count.ToString();

            Update_btnConnect();

            // check if queue is empty
            if (dataQueue.Count > 0 && serialPort.IsOpen)
            {
                dataQueue.TryDequeue(out data);
                // read a packet of data
                // first find 255
                while (data != 255)
                {
                    dataQueue.TryDequeue(out data);
                }

                // retrieve x, y, z data, show in textbox and add to list
                int avgCount; // how many to average
                count++; // for x-axis of plot
                try
                {
                    avgCount = int.Parse(txtAvgN.Text);
                }
                catch (System.FormatException)
                {
                    avgCount = 0;
                }

                int stddevCount;
                try
                {
                    stddevCount = int.Parse(txtStddevN.Text);
                }
                catch (System.FormatException)
                {
                    stddevCount = 0;
                }


                for (READ_ACC curr = READ_ACC.X; curr != READ_ACC.OVER; curr++)
                {
                    dataQueue.TryDequeue(out data);
                    switch (curr)
                    {
                        case READ_ACC.X:
                            txtX.Text = data.ToString();
                            while (x_acc.Count > stddevCount)
                                x_acc.RemoveAt(0);
                            x_acc.Add(data);
                            break;
                        case READ_ACC.Y:
                            txtY.Text = data.ToString();
                            while (y_acc.Count > stddevCount)
                                y_acc.RemoveAt(0);
                            y_acc.Add(data);
                            break;
                        case READ_ACC.Z:
                            txtZ.Text = data.ToString();
                            while (z_acc.Count > stddevCount)
                                z_acc.RemoveAt(0);
                            z_acc.Add(data);
                            break;
                    }
                }


                // retreive last x,y,z data
                int xData = x_acc[x_acc.Count - 1];
                int yData = y_acc[y_acc.Count - 1];
                int zData = z_acc[z_acc.Count - 1];

                if (!bomberGame.GetGameStatus())
                {
                    if (grav.Count > avgCount)
                        grav.RemoveAt(0);
                    grav.Add(Calculate_grav(xData, yData, zData));
                    txtGAvg.Text = grav.Average().ToString();

                    // read threshold values
                    int xThreshold, yThreshold, zThreshold, yThresholdNeg, zThresholdNeg;
                    try
                    {
                        xThreshold = int.Parse(txtXThreshold.Text);
                        yThreshold = int.Parse(txtYThreshold.Text);
                        zThreshold = int.Parse(txtZThreshold.Text);
                        yThresholdNeg = int.Parse(txtYNeg.Text);
                        zThresholdNeg = int.Parse(txtZNeg.Text);
                    }
                    catch (System.FormatException)
                    {
                        xThreshold = 220;
                        yThreshold = 220;
                        zThreshold = 220;
                        yThresholdNeg = 100;
                        zThresholdNeg = 100;
                    }

                    // start gesture reading
                    if ((xData > xThreshold || zData > zThreshold || zData < zThresholdNeg) && gestureState == GESTURE.IDLE)
                    {
                        gestureState = GESTURE.START;
                    }

                    Update_orientation(xData, yData, zData);
                    Calculate_average(avgCount);
                    Calculate_std(stddevCount);
                    Update_gesture(xData, yData, zData, xThreshold, yThreshold, zThreshold, yThresholdNeg, zThresholdNeg);
                    Update_plot(xData, yData, zData, count);
                }

                while (dataQueue.Count > itemLimit)
                    dataQueue.TryDequeue(out int trash);

                if (bomberGame.GetGameStatus())
                    bomberGame.UpdatePosition(ref bomberGame.bomber, xData, yData, zData); // update bomberman position
                else if (btnStart.Text != "Start")
                    btnStart.Text = "Start";
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (bomberGame.GetGameStatus())
            {
                btnStart.Text = "Start";
                bomberGame.StopGame();
            }
            else if (serialPort.IsOpen)
            {
                btnStart.Text = "Stop";
                bomberGame.StartGame();
            }
        }
    }
}
