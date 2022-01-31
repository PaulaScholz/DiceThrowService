using System;
using System.Windows.Forms;
using DiceThrowLibrary;
//using Microsoft.Extensions.Logging;

namespace DataVisualizationDotNetCore
{
    public partial class Form1 : Form
    {
        // labels for the DieRoll chart
        readonly int[] DieRollX = { 1, 2, 3, 4, 5, 6 };

        // value buckets for the DieRoll chart
        readonly int[] DieRollY = { 0, 0, 0, 0, 0, 0 };

        // labels for the DiceTotal chart
        readonly int[] DiceTotalX = { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

        // value buckets for the DiceTotal chart
        readonly int[] DiceTotalY = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        // counter for total number of rolls
        int NumberOfRolls = 0;

        // the Microsoft.Extensions.Logging (MEL) logger for this particular form
        //private static ILogger iMELLogger;

        // Default status of the ETW Logger's DefaultEventListener.  It may be 
        // toggled on or off to suppress ETW messages on any attached debuggers.
        // Since the EventListenerStub class in Program.cs echos ETW messages to 
        // its attached log providers, you may wish to suppress the DefaultEventListener's
        // messages so they don't appear twice in the Visual Studio 2019 Output window.
        private bool DefaultEventListenerEnabled = true;

        public Form1()
        {
            InitializeComponent();

            // create the logger for this particular form
            //iMELLogger = Program.iMELLoggerFactory.CreateLogger<Form1>();

            Load += Form1_Load;
            FormClosing += Form1_FormClosing;
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //iMELLogger.LogInformation("Form1 is closing.");
            DiceThrowLibrary.Logging.Logger.Info("DiceThrow Application End");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // setup the chart.  This code used to be part of InitializeComponent()
            SetupChart();

            // set up the axes for the charts
            DieRollChart.ChartAreas["ChartArea1"].AxisX.Minimum = 1;
            DieRollChart.ChartAreas["ChartArea1"].AxisX.Maximum = 6;
            DieRollChart.ChartAreas["ChartArea1"].AxisX.Interval = 1;

            DiceThrowChart.ChartAreas["ChartArea1"].AxisX.Minimum = 1;
            DiceThrowChart.ChartAreas["ChartArea1"].AxisX.Maximum = 11;
            DiceThrowChart.ChartAreas["ChartArea1"].AxisX.Interval = 1;

            DieRollChart.Series["Die Rolls"].Points.DataBindXY(DieRollX, DieRollY);
            DiceThrowChart.Series["Dice Totals"].Points.DataBindXY(DiceTotalX, DiceTotalY);

            // send a log message
            // iMELLogger.LogInformation("Form1_Load completed.");
            DiceThrowLibrary.Logging.Logger.Info("DiceThrow Application Start");
        }

        /// <summary>
        /// This is used to set up the charts.  It was inside Form1.Designer.cs, inside the InitializeComponent() method,
        /// but was moved here because the System.Windows.Forms.DataVisualization namespace is not recognized by the
        /// Windows Forms designer in .Net 5 at this time, and any changes to the Form design in the Designer subsequent to publishing
        /// this project would not be preserved by the designer, and those changes to the Form would destroy this code.  Thus, 
        /// this code was created in a .Net Framework project and moved here to a separate method.  Note that if you change any
        /// part of the current design in the Designer, you will have to comment out the this.ResumeLayout(false) and this.PerformLayout()
        /// methods in the Windows Form Designer-generated code, as they are repeated herein.
        /// 
        /// Hopefully, the designer issue will be fixed in a future update of Visual Studio.  5/7/2021
        /// </summary>
        private void SetupChart()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea7 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend7 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea8 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend8 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series8 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.DiceThrowChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.DieRollChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.DiceThrowChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DieRollChart)).BeginInit();
            // 
            // DiceThrowChart
            // 
            chartArea7.Name = "ChartArea1";
            this.DiceThrowChart.ChartAreas.Add(chartArea7);
            legend7.Enabled = false;
            legend7.Name = "Legend1";
            this.DiceThrowChart.Legends.Add(legend7);
            this.DiceThrowChart.Location = new System.Drawing.Point(559, 244);
            this.DiceThrowChart.Name = "DiceThrowChart";
            series7.ChartArea = "ChartArea1";
            series7.IsXValueIndexed = true;
            series7.Legend = "Legend1";
            series7.LegendText = "Dice Totals";
            series7.Name = "Dice Totals";
            series7.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series7.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            this.DiceThrowChart.Series.Add(series7);
            this.DiceThrowChart.Size = new System.Drawing.Size(587, 365);
            this.DiceThrowChart.TabIndex = 0;
            this.DiceThrowChart.Text = "chart1";
            // 
            // DieRollChart
            // 
            chartArea8.Name = "ChartArea1";
            this.DieRollChart.ChartAreas.Add(chartArea8);
            legend8.Enabled = false;
            legend8.Name = "Legend1";
            this.DieRollChart.Legends.Add(legend8);
            this.DieRollChart.Location = new System.Drawing.Point(51, 244);
            this.DieRollChart.Name = "DieRollChart";
            series8.ChartArea = "ChartArea1";
            series8.IsXValueIndexed = true;
            series8.Legend = "Legend1";
            series8.Name = "Die Rolls";
            series8.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series8.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            this.DieRollChart.Series.Add(series8);
            this.DieRollChart.Size = new System.Drawing.Size(467, 365);
            this.DieRollChart.TabIndex = 1;
            this.DieRollChart.Text = "chart1";
            this.Controls.Add(this.DieRollChart);
            this.Controls.Add(this.DiceThrowChart);

            ((System.ComponentModel.ISupportInitialize)(this.DiceThrowChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DieRollChart)).EndInit();

            // these methods should be commented out inside InitializeComponent() if any 
            // design changes are made with the Forms Designer because they will be regenerated there
            // by Visual Studio 2019.
            this.ResumeLayout(false);
            this.PerformLayout();

            //iMELLogger.LogInformation("Just set up the chart controls inside SetupChart().");
        }

        private void AddRolls_Click(object sender, EventArgs e)
        {
            // make 100 dice throws
            for (int i=0; i< 100; i++)
            {
                int total = DiceThrow.GetDiceThrow(out int firstDie, out int secondDie);

                // increment the zero-based buckets, so subtract one from the roll,
                // die values are 1-6
                DieRollY[firstDie-1]++;
                DieRollY[secondDie-1]++;

                // increment the zero-based buckets, so subtract two from the roll,
                // dice total values are 2-12
                DiceTotalY[total-2]++;
            }

            NumberOfRolls += 100;

            // chart the results
            DieRollChart.Series["Die Rolls"].Points.DataBindXY(DieRollX, DieRollY);
            DiceThrowChart.Series["Dice Totals"].Points.DataBindXY(DiceTotalX, DiceTotalY);

            RollTotalLabel.Text = string.Format("{0} Total Rolls", NumberOfRolls);

            // log it
            //iMELLogger.LogInformation("AddRolls_Click completed.");
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            // clear the buckets
            for (int i = 0; i < DieRollY.Length; i++)
            {
                DieRollY[i] = 0;
            }

            for (int i = 0; i < DiceTotalY.Length; i++)
            {
                DiceTotalY[i] = 0;
            }

            // reset rolls counter
            NumberOfRolls = 0;

            // reset the chart with the zeroed buckets
            DieRollChart.Series["Die Rolls"].Points.DataBindXY(DieRollX, DieRollY);
            DiceThrowChart.Series["Dice Totals"].Points.DataBindXY(DiceTotalX, DiceTotalY);

            RollTotalLabel.Text = string.Format("{0} Total Rolls", NumberOfRolls);

            // reenable the DefaultEventListener in the library
            DiceThrow.EnableDefaultEventListener();

            // log it
            //iMELLogger.LogInformation("ClearButton_Click completed.");
            DiceThrowLibrary.Logging.Logger.Info("Log counters cleared.");
        }

        private void ExceptionButton_Click(object sender, EventArgs e)
        {
            //iMELLogger.LogInformation("ExceptionButton_Click about to generate DivideByZero exception.");

            // generate a divide by zero exception in the library, it is unhandled and will be caught
            // by the Program.Application_ThreadException handler where it will be logged.
            DiceThrowLibrary.DiceThrow.DivideByZero();
        }

        /// <summary>
        /// Toggle the status of the DefaultEventListener in the ETW logger to supress or enable
        /// ETW messages on attached debuggers like the Visual Studio debugger on the Output window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DefaultEventListenerButton_Click(object sender, EventArgs e)
        {
            // toggle the status 
            DefaultEventListenerEnabled = !DefaultEventListenerEnabled;

            if (DefaultEventListenerEnabled)
            {
                DiceThrow.EnableDefaultEventListener();

                DefaultEventListenerButton.Text = "Disable DefaultEventListener";
            }
            else
            {
                // Suppress ETW DefaultEventListener messages from appearing
                // on attached debuggers. They will still be logged by the EventListenerStub's
                // iMELLogger in Program.cs, but won't appear Visual Studio Output window twice.
                DiceThrow.DisableDefaultEventListener();

                DefaultEventListenerButton.Text = "Enable DefaultEventListener";
            }
        }
    }
}
