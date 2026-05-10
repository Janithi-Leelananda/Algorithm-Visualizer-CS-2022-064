using System;
using System.Drawing;
using System.Windows.Forms;

namespace AlgorithmVisualizer.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            this.BackColor = Color.FromArgb(45, 45, 48);
            this.Text = "Algorithm Visualizer";
        }


        private void btnSorting_Click(object sender, EventArgs e)
        {
            SortingVisualizerForm sortingForm = new SortingVisualizerForm();
            sortingForm.Show();
        }

        private void btnPathfinding_Click(object sender, EventArgs e)
        {
            PathfindingVisualizerForm pathForm = new PathfindingVisualizerForm();
            pathForm.Show(); 
        }

        private void btnExit_Click(object sender, EventArgs e) => Application.Exit();

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
