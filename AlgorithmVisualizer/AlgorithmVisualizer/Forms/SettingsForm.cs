using System;
using System.Windows.Forms;

namespace AlgorithmVisualizer.Forms
{
    public partial class SettingsForm : Form
    {
        public int Speed { get; set; }
        public int SizeValue { get; set; }

        public SettingsForm(int currentSpeed, int currentSize)
        {
            InitializeComponent();
            numSpeed.Value = currentSpeed;
            numSize.Value = currentSize;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Speed = (int)numSpeed.Value;
            this.SizeValue = (int)numSize.Value;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnOk_Click_1(object sender, EventArgs e)
        {
            this.Speed = (int)numSpeed.Value;
            this.SizeValue = (int)numSize.Value;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}