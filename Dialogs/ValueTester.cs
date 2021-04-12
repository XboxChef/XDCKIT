using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XDevkit;

namespace XDevkitTester.XDevkit.Dialogs
{

    public partial class ValueTester : DevExpress.XtraEditors.XtraForm
    {
        private string values;
        private int totalTime;
        private int remainingTime;
        private bool isPaused = false;

        private bool getValueFromXbox;
        private int currentOffset;
        private int currentBatch;
        private int offsetsPerBatch;

        private int currentValue;
        private int totalValues;
        private bool useBatchPoking = false;
        Xbox XConsole = new Xbox();
        ObservableCollection<Offset> _OffsetCollection = new ObservableCollection<Offset>();

        public ObservableCollection<Offset> OffsetCollection
        { get { return _OffsetCollection; } }
        public ValueTester(string Values)
        {
            values = Values;
            InitializeComponent();
        }

        private void ValueTester_Load(object sender, EventArgs e)
        {
            offsetsList.BackColor = BackColor;

            //Connect
            if (!Xbox.Connected)
            {
                try
                {
                    XConsole.Connect();
                    getValueFromXbox = true;
                }
                catch
                {
                    getValueFromXbox = false;
                }
            }
            else
            {
                getValueFromXbox = true;
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (valuesBox.Text.Contains(','))
            {
                totalTime = Convert.ToInt32(timeBox.Text);
                StartIntervalPoking();
            }
            else
            {
                MessageBox.Show("Error", "Please fill in the values text box. ");
            }
        }
        private void StartIntervalPoking()
        {
            totalValues = valuesBox.Text.Split(',').Count();
            currentOffset = 0;
            currentBatch = 0;
            currentValue = 0;
            //prevent checking/unchecking once process has started
            if (batchTestingCheck.Checked == true)
            {
                useBatchPoking = true;
                offsetsPerBatch = Convert.ToInt32(batchesBox.Text);
                for (int i = 0; i < OffsetCollection.Count; i += Convert.ToInt32(batchesBox.Text))
                {
                    List<Offset> singleBatch = new List<Offset>();
                    for (int j = 0; j < Convert.ToInt32(batchesBox.Text); j++)
                    {
                        try
                        {
                            singleBatch.Add(OffsetCollection[i + j]);
                        }
                        catch
                        {

                        }
                    }
                    offsetBatches.Add(singleBatch);
                }
            }

            timer.Enabled = true;
            timer.Interval = 1000;
            timer.Tick += new EventHandler(timer_Tick);
            remainingTime = totalTime;
            if (useBatchPoking)
            {
                foreach (Offset offsetSinglet in offsetBatches[0])
                {
                    Offset OffsetSinglet = offsetSinglet;
                    OffsetSinglet.Value = valuesBox.Text.Split(',')[0];
                    XConsole.PokeXbox(OffsetSinglet);
                }
            }
            else
            {
                XConsole.PokeXbox(new Offset(OffsetCollection[0].Address, "float", valuesBox.Text.Split(',')[0]));
            }
            currentValue++;
        }
        private List<List<Offset>> offsetBatches = new List<List<Offset>>();
        private void timer_Tick(object sender, EventArgs e)
        {
            if (!isPaused)
            {
                if (remainingTime-- == 0)
                {
                    if (useBatchPoking == true)
                    {
                        if (currentBatch <= offsetBatches.Count)
                        {
                            remainingTime = totalTime;
                           // offsetsList.SelectedIndex = currentBatch * offsetsPerBatch;
                            string offset = OffsetCollection[currentOffset].Address;
                            string type = OffsetCollection[currentOffset].Type;
                            string value = valuesBox.Text.Split(',')[currentValue];

                            if (value != "No Console Detected" && value != "Not Connected")
                            {
                                foreach (Offset offsetSinglet in offsetBatches[currentBatch])
                                {
                                    Offset OffsetSinglet = offsetSinglet;
                                    OffsetSinglet.Value = value.Replace("DEFAULT", OffsetSinglet.Value);
                                    XConsole.PokeXbox(OffsetSinglet);
                                }
                            }
                            if (currentValue == totalValues)
                            {
                                currentBatch++;
                                currentValue = 0;
                            }
                            else
                            {
                                currentValue++;
                            }
                        }
                    }
                    else
                    {
                        if (currentOffset <= offsetsList.Items.Count)
                        {
                           // offsetsList.SelectedIndex = currentOffset;
                            remainingTime = totalTime;

                            string offset = OffsetCollection[currentOffset].Address;
                            string type = OffsetCollection[currentOffset].Type;
                            string value = valuesBox.Text.Split(',')[currentValue];
                            value = value.Replace("DEFAULT", OffsetCollection[currentOffset].Value);

                            if (value != "No Console Detected" && value != "Not Connected")
                            {
                                XConsole.PokeXbox(new Offset(offset, type, value));
                            }
                            if (currentValue == totalValues - 1)
                            {
                                currentOffset++;
                                currentValue = 0;
                            }
                            else
                            {
                                currentValue++;
                            }
                        }
                    }
                }
                else
                {
                    timeBar.EditValue = (((double)remainingTime / (double)totalTime) * 100);
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (currentValue == totalValues - 1)
            {
                currentOffset++;
                currentBatch++;
                currentValue = 0;
            }
            else
            {
                currentValue++;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            currentBatch++;
            currentOffset++;
            currentValue = 0;
        }
    }
}