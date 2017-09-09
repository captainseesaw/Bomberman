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

enum Pos {X,Y,Z,OVER}

namespace SerialReaderSuperSimple
{
    public partial class Form1 : Form
    {
        int baudRate = 128000;
        int dataBits = 8; //bits

        // dataQueue
        ConcurrentQueue<int> dataQueue = new ConcurrentQueue<int>();

        // case idx
        int idx = 0;
        // item limit
        int itemLimit = 8;

        public Form1()
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

        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // read all available bytes
            int numToRead = serialPort.BytesToRead;
            byte[] arr = new byte[numToRead];
            serialPort.Read(arr,0, numToRead);

            foreach (int data in arr)
            {
                if (dataQueue.Count > itemLimit)
                    dataQueue.TryDequeue(out int trash);
                dataQueue.Enqueue(data);
                //Console.WriteLine(data);
            }
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
                // display x, y, z
                for (Pos curr =  Pos.X; curr != Pos.OVER; curr++)
                {
                    dataQueue.TryDequeue(out data);
                    switch (curr)
                    {
                        case Pos.X:
                            txtX.Text = data.ToString();
                            break;
                        case Pos.Y:
                            txtY.Text = data.ToString();
                            break;
                        case Pos.Z:
                            txtZ.Text = data.ToString();
                            break;
                    }
                }

            }
        }
    }
}
