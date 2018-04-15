using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelEditor
{
    public partial class StartForm : Form
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public StartForm()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void startButton_Click(object sender, EventArgs e)
        {
            this.Width = (int)widthInput.Value;
            this.Height = (int)heightInput.Value;

            // form good
            if (this.Width > 1 && this.Height > 1)
            {
                this.DialogResult = DialogResult.OK;
                
            }
            else
            {
                // show error
                this.DialogResult = DialogResult.Cancel;
                MessageBox.Show("Values too small!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
