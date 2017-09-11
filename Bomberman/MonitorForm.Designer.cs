namespace Bomberman
{
    partial class monitorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.cmbPorts = new System.Windows.Forms.ComboBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtX = new System.Windows.Forms.TextBox();
            this.txtY = new System.Windows.Forms.TextBox();
            this.txtZ = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.serialPort = new System.IO.Ports.SerialPort(this.components);
            this.txtOrientation = new System.Windows.Forms.TextBox();
            this.lblOrientation = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtZAvg = new System.Windows.Forms.TextBox();
            this.txtYAvg = new System.Windows.Forms.TextBox();
            this.txtXAvg = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtN = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtGesture = new System.Windows.Forms.TextBox();
            this.txtLED = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtZThreshold = new System.Windows.Forms.TextBox();
            this.txtYThreshold = new System.Windows.Forms.TextBox();
            this.txtXThreshold = new System.Windows.Forms.TextBox();
            this.chrAcc = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.btnStart = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chrAcc)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbPorts
            // 
            this.cmbPorts.FormattingEnabled = true;
            this.cmbPorts.Location = new System.Drawing.Point(12, 21);
            this.cmbPorts.Name = "cmbPorts";
            this.cmbPorts.Size = new System.Drawing.Size(121, 28);
            this.cmbPorts.TabIndex = 0;
            this.cmbPorts.SelectionChangeCommitted += new System.EventHandler(this.cmbPorts_SelectionChangeCommitted);
            this.cmbPorts.Click += new System.EventHandler(this.cmbPorts_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(155, 21);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(111, 37);
            this.btnConnect.TabIndex = 1;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtX
            // 
            this.txtX.Location = new System.Drawing.Point(15, 70);
            this.txtX.Name = "txtX";
            this.txtX.Size = new System.Drawing.Size(108, 26);
            this.txtX.TabIndex = 2;
            // 
            // txtY
            // 
            this.txtY.Location = new System.Drawing.Point(15, 112);
            this.txtY.Name = "txtY";
            this.txtY.Size = new System.Drawing.Size(108, 26);
            this.txtY.TabIndex = 3;
            // 
            // txtZ
            // 
            this.txtZ.Location = new System.Drawing.Point(14, 154);
            this.txtZ.Name = "txtZ";
            this.txtZ.Size = new System.Drawing.Size(108, 26);
            this.txtZ.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(181, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "X axis";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(181, 116);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "Y axis";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(181, 157);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "Z axis";
            // 
            // timer
            // 
            this.timer.Interval = 10;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // serialPort
            // 
            this.serialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.SerialPort_DataReceived);
            // 
            // txtOrientation
            // 
            this.txtOrientation.Location = new System.Drawing.Point(15, 233);
            this.txtOrientation.Name = "txtOrientation";
            this.txtOrientation.Size = new System.Drawing.Size(251, 26);
            this.txtOrientation.TabIndex = 8;
            // 
            // lblOrientation
            // 
            this.lblOrientation.AutoSize = true;
            this.lblOrientation.Location = new System.Drawing.Point(20, 204);
            this.lblOrientation.Name = "lblOrientation";
            this.lblOrientation.Size = new System.Drawing.Size(91, 20);
            this.lblOrientation.TabIndex = 9;
            this.lblOrientation.Text = "Orientation:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(181, 377);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 20);
            this.label4.TabIndex = 15;
            this.label4.Text = "Z avg";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(181, 336);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 20);
            this.label5.TabIndex = 14;
            this.label5.Text = "Y avg";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(181, 293);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 20);
            this.label6.TabIndex = 13;
            this.label6.Text = "X avg";
            // 
            // txtZAvg
            // 
            this.txtZAvg.Location = new System.Drawing.Point(14, 374);
            this.txtZAvg.Name = "txtZAvg";
            this.txtZAvg.Size = new System.Drawing.Size(108, 26);
            this.txtZAvg.TabIndex = 12;
            // 
            // txtYAvg
            // 
            this.txtYAvg.Location = new System.Drawing.Point(15, 332);
            this.txtYAvg.Name = "txtYAvg";
            this.txtYAvg.Size = new System.Drawing.Size(108, 26);
            this.txtYAvg.TabIndex = 11;
            // 
            // txtXAvg
            // 
            this.txtXAvg.Location = new System.Drawing.Point(15, 290);
            this.txtXAvg.Name = "txtXAvg";
            this.txtXAvg.Size = new System.Drawing.Size(108, 26);
            this.txtXAvg.TabIndex = 10;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(181, 418);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(20, 20);
            this.label7.TabIndex = 17;
            this.label7.Text = "N";
            // 
            // txtN
            // 
            this.txtN.Location = new System.Drawing.Point(15, 415);
            this.txtN.Name = "txtN";
            this.txtN.Size = new System.Drawing.Size(108, 26);
            this.txtN.TabIndex = 16;
            this.txtN.Text = "100";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(377, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 20);
            this.label8.TabIndex = 19;
            this.label8.Text = "Gesture:";
            // 
            // txtGesture
            // 
            this.txtGesture.Location = new System.Drawing.Point(372, 51);
            this.txtGesture.Name = "txtGesture";
            this.txtGesture.Size = new System.Drawing.Size(251, 26);
            this.txtGesture.TabIndex = 18;
            // 
            // txtLED
            // 
            this.txtLED.BackColor = System.Drawing.Color.Red;
            this.txtLED.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLED.Location = new System.Drawing.Point(446, 95);
            this.txtLED.Multiline = true;
            this.txtLED.Name = "txtLED";
            this.txtLED.Size = new System.Drawing.Size(94, 85);
            this.txtLED.TabIndex = 20;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(547, 288);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(93, 20);
            this.label9.TabIndex = 26;
            this.label9.Text = "Z Threshold";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(547, 247);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(94, 20);
            this.label10.TabIndex = 25;
            this.label10.Text = "Y Threshold";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(547, 204);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(94, 20);
            this.label11.TabIndex = 24;
            this.label11.Text = "X Threshold";
            // 
            // txtZThreshold
            // 
            this.txtZThreshold.Location = new System.Drawing.Point(380, 285);
            this.txtZThreshold.Name = "txtZThreshold";
            this.txtZThreshold.Size = new System.Drawing.Size(108, 26);
            this.txtZThreshold.TabIndex = 23;
            this.txtZThreshold.Text = "230";
            // 
            // txtYThreshold
            // 
            this.txtYThreshold.Location = new System.Drawing.Point(381, 243);
            this.txtYThreshold.Name = "txtYThreshold";
            this.txtYThreshold.Size = new System.Drawing.Size(108, 26);
            this.txtYThreshold.TabIndex = 22;
            this.txtYThreshold.Text = "200";
            // 
            // txtXThreshold
            // 
            this.txtXThreshold.Location = new System.Drawing.Point(381, 201);
            this.txtXThreshold.Name = "txtXThreshold";
            this.txtXThreshold.Size = new System.Drawing.Size(108, 26);
            this.txtXThreshold.TabIndex = 21;
            this.txtXThreshold.Text = "220";
            // 
            // chrAcc
            // 
            chartArea1.Name = "ChartArea1";
            this.chrAcc.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend";
            this.chrAcc.Legends.Add(legend1);
            this.chrAcc.Location = new System.Drawing.Point(372, 336);
            this.chrAcc.Name = "chrAcc";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend";
            series1.Name = "X";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Legend = "Legend";
            series2.Name = "Y";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.Legend = "Legend";
            series3.Name = "Z";
            this.chrAcc.Series.Add(series1);
            this.chrAcc.Series.Add(series2);
            this.chrAcc.Series.Add(series3);
            this.chrAcc.Size = new System.Drawing.Size(575, 300);
            this.chrAcc.TabIndex = 27;
            title1.Name = "Acceleartion Plot";
            title1.Text = "Acceleration Plot";
            this.chrAcc.Titles.Add(title1);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(751, 114);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(185, 110);
            this.btnStart.TabIndex = 28;
            this.btnStart.Text = "Start Game";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // monitorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(977, 656);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.chrAcc);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtZThreshold);
            this.Controls.Add(this.txtYThreshold);
            this.Controls.Add(this.txtXThreshold);
            this.Controls.Add(this.txtLED);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtGesture);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtN);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtZAvg);
            this.Controls.Add(this.txtYAvg);
            this.Controls.Add(this.txtXAvg);
            this.Controls.Add(this.lblOrientation);
            this.Controls.Add(this.txtOrientation);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtZ);
            this.Controls.Add(this.txtY);
            this.Controls.Add(this.txtX);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.cmbPorts);
            this.Name = "monitorForm";
            this.Text = "monitorForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.chrAcc)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbPorts;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txtX;
        private System.Windows.Forms.TextBox txtY;
        private System.Windows.Forms.TextBox txtZ;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Timer timer;
        private System.IO.Ports.SerialPort serialPort;
        private System.Windows.Forms.TextBox txtOrientation;
        private System.Windows.Forms.Label lblOrientation;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtZAvg;
        private System.Windows.Forms.TextBox txtYAvg;
        private System.Windows.Forms.TextBox txtXAvg;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtN;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtGesture;
        private System.Windows.Forms.TextBox txtLED;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtZThreshold;
        private System.Windows.Forms.TextBox txtYThreshold;
        private System.Windows.Forms.TextBox txtXThreshold;
        private System.Windows.Forms.DataVisualization.Charting.Chart chrAcc;
        private System.Windows.Forms.Button btnStart;
    }
}

