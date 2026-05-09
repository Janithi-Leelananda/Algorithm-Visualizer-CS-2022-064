using AlgorithmVisualizer.Algorithms;
using System;
using System.Drawing;
using System.Windows.Forms;


namespace AlgorithmVisualizer.Forms
{
    public partial class SortingVisualizerForm : Form
    {
        private int[] data;
        private Random random = new Random();
        private int comparisons = 0;
        private int currentComparingIdx = -1;
        private int currentSwappingIdx = -1;
        private bool isSorted = false;

        private int animationSpeed = 20;
        private int arraySize = 50;


        private InsertionSort _insertionSort = new InsertionSort();
        private QuickSort _quickSort = new QuickSort();

        public SortingVisualizerForm()
        {
            InitializeComponent();
            this.Text = "Sorting Visualizer";
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.DoubleBuffered = true;
        }

        private void GenerateNewArray()
        {
            data = new int[arraySize];

            for (int i = 0; i < arraySize; i++)
            {
                data[i] = random.Next(10, pnlChart.Height - 20);
            }

            comparisons = 0;
            isSorted = false;
            UpdateComparisonLabel();
            pnlChart.Invalidate();
        }

        private void UpdateComparisonLabel()
        {
            if (lblComparisons.InvokeRequired)
            {
                lblComparisons.Invoke(new Action(UpdateComparisonLabel));
            }
            else
            {
                lblComparisons.Text = $"Comparisons: {comparisons}";
            }
        }

        private void pnlChart_Paint(object sender, PaintEventArgs e)
        {
            if (data == null) return;

            Graphics g = e.Graphics;
            int barWidth = Math.Max(2, pnlChart.Width / data.Length);

            for (int i = 0; i < data.Length; i++)
            {
                Brush brush = Brushes.White;

                if (isSorted) brush = Brushes.LimeGreen;
                else if (i == currentComparingIdx) brush = Brushes.Red;
                else if (i == currentSwappingIdx) brush = Brushes.Gold;

                g.FillRectangle(brush, i * barWidth, pnlChart.Height - data[i], barWidth - 1, data[i]);
            }
        }

        private void SetControlsEnabled(bool enabled)
        {
            btnStart.Enabled = enabled;
            btnReset.Enabled = enabled;
            btnSettings.Enabled = enabled;
            btnBack.Enabled = enabled;
            rbInsertionSort.Enabled = enabled;
            rbQuickSort.Enabled = enabled;
        }

       
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (data == null) GenerateNewArray();

            SetControlsEnabled(false);
            isSorted = false;
            comparisons = 0;

            if (rbInsertionSort.Checked)
            {
                _insertionSort.Initialize(data);
            }
            else if (rbQuickSort.Checked)
            {
                _quickSort.Initialize(data, 0, data.Length - 1);
            }

            sortingTimer.Interval = Math.Max(1, animationSpeed);
            sortingTimer.Start();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            GenerateNewArray();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            using (var settings = new SettingsForm(animationSpeed, arraySize))
            {
                if (settings.ShowDialog() == DialogResult.OK)
                {
                    this.animationSpeed = settings.Speed;
                    this.arraySize = settings.SizeValue;
                    GenerateNewArray();
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void sortingTimer_Tick(object sender, EventArgs e)
        {
            bool finished = false;

            if (rbInsertionSort.Checked)
            {
                finished = _insertionSort.Step((idx1, idx2, isSwap) =>
                {
                    currentComparingIdx = idx1;
                    currentSwappingIdx = isSwap ? idx2 : -1;
                    comparisons++;
                });
            }
            else if (rbQuickSort.Checked)
            {
                finished = _quickSort.Step((idx1, idx2, isSwap) =>
                {
                    currentComparingIdx = idx1;
                    currentSwappingIdx = isSwap ? idx2 : -1;
                    comparisons++;
                });
            }

            UpdateComparisonLabel();
            pnlChart.Invalidate();

            if (finished)
            {
                sortingTimer.Stop();
                isSorted = true;
                currentComparingIdx = -1;
                currentSwappingIdx = -1;
                pnlChart.Invalidate();
                SetControlsEnabled(true);
                MessageBox.Show("Sorting Complete!");
            }
        }
    }
}