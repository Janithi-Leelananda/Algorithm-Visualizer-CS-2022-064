using AlgorithmVisualizer.Algorithms;
using AlgorithmVisualizer.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AlgorithmVisualizer.Forms
{
    public partial class PathfindingVisualizerForm : Form
    {
        private enum SelectionMode { StartNode, EndNode, WallNode }
        private SelectionMode currentSelection = SelectionMode.StartNode;

        private Node[,] grid;
        private int rows = 20, cols = 30;
        private int cellSize = 25;

        private Node startNode;
        private Node endNode;
        private BFSPathfinder _pathfinder = new BFSPathfinder();
        private int animationSpeed = 20;

        public PathfindingVisualizerForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            grid = new Node[rows, cols];
            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < cols; y++)
                {
                    grid[x, y] = new Node(x, y);
                }
            }

            startNode = grid[0, 0];
            endNode = grid[rows - 1, cols - 1];
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (startNode == null || endNode == null) return;

            ResetSearchStates();

            _pathfinder.InitializeSearch(grid, startNode, endNode);

            SetControlsEnabled(false);

            animationTimer.Interval = animationSpeed;
            animationTimer.Start();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            InitializeGrid();
            currentSelection = SelectionMode.StartNode;
            rbStart.Checked = true;
            pnlGrid.Invalidate();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SetControlsEnabled(bool enabled)
        {
            btnStart.Enabled = enabled;
            btnSettings.Enabled = enabled;
            btnResetGrid.Enabled = enabled;
            btnBack.Enabled = enabled;
            rbStart.Enabled = enabled;
            rbEnd.Enabled = enabled;
            rbDrawWall.Enabled = enabled;
        }
        private void btnSettings_Click(object sender, EventArgs e)
        {
            using (var settings = new SettingsForm(animationSpeed, rows))
            {
                if (settings.ShowDialog() == DialogResult.OK)
                {
                    this.animationSpeed = settings.Speed;

                    this.rows = settings.SizeValue;

                    this.cols = (int)(this.rows * 1.5);

                    InitializeGrid();
                    pnlGrid.Invalidate();
                }
            }
        }

        private void pnlGrid_Paint(object sender, PaintEventArgs e)
        {
            if (grid == null) return;

            Graphics g = e.Graphics;

            float cellWidth = (float)pnlGrid.Width / cols;
            float cellHeight = (float)pnlGrid.Height / rows;

            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < cols; y++)
                {
                    Color color;
                    if (grid[x, y] == startNode) color = Color.Green;
                    else if (grid[x, y] == endNode) color = Color.Red;
                    else if (grid[x, y].IsPath) color = Color.Gold;
                    else if (grid[x, y].IsWall) color = Color.Black;
                    else if (grid[x, y].IsVisited) color = Color.Cyan;
                    else color = Color.White;

                    g.FillRectangle(new SolidBrush(color),y * cellWidth,x * cellHeight,cellWidth - 1,cellHeight - 1);
                }
            }
        }

        private void rbStart_CheckedChanged(object sender, EventArgs e)
        {
            if (rbStart.Checked) currentSelection = SelectionMode.StartNode;
        }

        private void rbEnd_CheckedChanged(object sender, EventArgs e)
        {
            if (rbEnd.Checked) currentSelection = SelectionMode.EndNode;
        }

        private void rbDrawWall_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDrawWall.Checked) currentSelection = SelectionMode.WallNode;
        }

        private void animationTimer_Tick(object sender, EventArgs e)
        {
            bool found = _pathfinder.Step();

            if (found)
            {
                animationTimer.Stop();
                HandlePathFound();
            }
            else if (!_pathfinder.IsSearching)
            {
                animationTimer.Stop();
                SetControlsEnabled(true);
                MessageBox.Show("No path exists.");
            }

            pnlGrid.Invalidate();
        }

        private void ResetSearchStates()
        {
            foreach (var node in grid)
            {
                node.IsVisited = false;
                node.IsPath = false;
                node.Parent = null;
            }
        }

        private void HandlePathFound()
        {
            Node current = endNode.Parent;
            while (current != null && current != startNode)
            {
                current.IsPath = true;
                current = current.Parent;
            }

            pnlGrid.Invalidate();
            SetControlsEnabled(true);
            MessageBox.Show("Target Reached!");
        }

        private void pnlGrid_MouseDown(object sender, MouseEventArgs e)
        {
            float cellWidth = (float)pnlGrid.Width / cols;
            float cellHeight = (float)pnlGrid.Height / rows;

            int y = (int)(e.X / cellWidth);
            int x = (int)(e.Y / cellHeight);

            if (x >= 0 && x < rows && y >= 0 && y < cols)
            {
                Node target = grid[x, y];

                switch (currentSelection)
                {
                    case SelectionMode.StartNode:
                        if (!target.IsWall && target != endNode)
                        {
                            startNode = target;
                        }
                        break;

                    case SelectionMode.EndNode:
                        if (!target.IsWall && target != startNode)
                        {
                            endNode = target;
                        }
                        break;

                    case SelectionMode.WallNode:
                        if (target != startNode && target != endNode)
                        {
                            target.IsWall = !target.IsWall;
                        }
                        break;
                }
                pnlGrid.Invalidate();
            }
        }
    }
}