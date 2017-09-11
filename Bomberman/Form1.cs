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

        // dataQueue
        ConcurrentQueue<int> dataQueue = new ConcurrentQueue<int>();
        int itemLimit = 8; // item limit

        // list for x,y,z values
        List<int> x_acc = new List<int>();
        List<int> y_acc = new List<int>();
        List<int> z_acc = new List<int>();

        // gesture reading
        Stopwatch gestureWatch = new Stopwatch();
        GESTURE gestureState = GESTURE.IDLE;
        string readGesture = "";
        int gestureTimeout = 2000;
        int displayTimeout = 1000;

        // plot 
        int count = 0;


        public monitorForm()
        {
            InitializeComponent();

            // initialize serial port
            serialPort.BaudRate = baudRate;
            serialPort.DataBits = dataBits;
            serialPort.Parity = Parity.None;
            serialPort.StopBits = StopBits.One;
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
                serialPort.Open();
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
            int numToRead = serialPort.BytesToRead;
            byte[] arr = new byte[numToRead];
            serialPort.Read(arr,0, numToRead);

            foreach (int data in arr)
            {
                //while (dataQueue.Count > itemLimit)
                //    dataQueue.TryDequeue(out int trash);
                dataQueue.Enqueue(data);
                //Console.WriteLine(data);
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

        private void Calculate_average(int avgCount)
        {
            // calculate average
            if (avgCount != 0)
            {
                txtXAvg.Text = (x_acc.Sum() / x_acc.Count).ToString();
                txtYAvg.Text = (y_acc.Sum() / y_acc.Count).ToString();
                txtZAvg.Text = (z_acc.Sum() / z_acc.Count).ToString();
            }
            else
            {
                txtXAvg.Text = String.Empty;
                txtYAvg.Text = String.Empty;
                txtZAvg.Text = String.Empty;
            }
        }

        private void Update_gesture(int xData, int yData, int zData,
            int xThreshold, int yThreshold, int zThreshold)
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
                        Console.WriteLine("over 1s, display clear");
                    }
                    if ( readGesture.Length <= 3)
                    {
                        if (xData > xThreshold && !readGesture.EndsWith("X"))
                        {
                            readGesture += "X";
                            txtGesture.Text = readGesture;
                        }

                        if (yData > yThreshold && !readGesture.EndsWith("Y") && readGesture.Contains('X'))
                        {
                            readGesture += "Y";
                            txtGesture.Text = readGesture;
                        }

                        if (zData > zThreshold && !readGesture.EndsWith("Z") && !readGesture.Contains("ZX"))
                        {
                            readGesture += "Z";
                            txtGesture.Text = readGesture;
                        }
                    }
                    switch (readGesture)
                    {
                        case "X":
                            txtGesture.Text = "Simple punch";
                            break;
                        case "ZX":
                            txtGesture.Text = "High punch";
                            break;
                        case "XYZ":
                            txtGesture.Text = "Right-hook";
                            break;
                    }
                    //Console.WriteLine(readGesture);
                    break;
                case (GESTURE.OVER):
                    txtGesture.Clear();
                    txtLED.BackColor = Color.Red;
                    gestureWatch.Reset();
                    gestureState = GESTURE.IDLE;
                    readGesture = "";
                    Console.WriteLine("over 1s, clear");
                    
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

            // check if queue is empty
            if (dataQueue.Count == 0)
                return;
            else
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
                    avgCount = int.Parse(txtN.Text);
                }
                catch (System.FormatException)
                {
                    avgCount = 0;
                }
                
                for (READ_ACC curr =  READ_ACC.X; curr != READ_ACC.OVER; curr++)
                {
                    dataQueue.TryDequeue(out data);
                    switch (curr)
                    {
                        case READ_ACC.X:
                            txtX.Text = data.ToString();
                            while (x_acc.Count > avgCount)
                                x_acc.RemoveAt(0);
                            x_acc.Add(data);
                            break;
                        case READ_ACC.Y:
                            txtY.Text = data.ToString();
                            while (y_acc.Count > avgCount)
                                y_acc.RemoveAt(0);
                            y_acc.Add(data);
                            break;
                        case READ_ACC.Z:
                            txtZ.Text = data.ToString();
                            while (z_acc.Count > avgCount)
                                z_acc.RemoveAt(0);
                            z_acc.Add(data);
                            break;
                    }
                }

                // retreive last x,y,z data
                int xData = x_acc[x_acc.Count - 1];
                int yData = y_acc[y_acc.Count - 1];
                int zData = z_acc[z_acc.Count - 1];

                // read threshold values
                int xThreshold, yThreshold, zThreshold;
                try
                {
                    xThreshold = int.Parse(txtXThreshold.Text);
                    yThreshold = int.Parse(txtYThreshold.Text);
                    zThreshold = int.Parse(txtZThreshold.Text);
                }
                catch (System.FormatException)
                {
                    xThreshold = 220;
                    yThreshold = 220;
                    zThreshold = 220;
                }
                

                // start gesture reading
                if ((xData > xThreshold || zData > zThreshold) && gestureState == GESTURE.IDLE)
                {
                    gestureState = GESTURE.START;
                }

                Update_orientation(xData, yData, zData);
                Calculate_average(avgCount);
                Update_gesture(xData, yData, zData, xThreshold, yThreshold, zThreshold);
                Update_plot(xData, yData, zData, count);

                while (dataQueue.Count > itemLimit)
                    dataQueue.TryDequeue(out int trash);
            }
        }
    }
}
