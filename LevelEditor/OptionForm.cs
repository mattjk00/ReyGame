using Rey.Engine.Prefabs;
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
    public partial class OptionForm : Form
    {
        public OptionForm()
        {
            InitializeComponent();
        }

        private void OptionForm_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.numericUpDown1.Maximum = 10;
            this.numericUpDown1.Minimum = -10;

            this.numericUpDown1.Value = EditorManager.SelectedTile.Depth;

            // hide door stuff if the tile isn't a door
            if (EditorManager.SelectedTile.TileType != TileType.Door)
            {
                this.label2.Hide();
                this.textBox1.Hide();
            }

            this.AcceptButton = this.button1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // set the depth
            EditorManager.SelectedTile.Depth = (int)this.numericUpDown1.Value;

            // if the tile type is a door, set the data for where it leads to
            if (EditorManager.SelectedTile.TileType == TileType.Door)
            {
                EditorManager.SelectedTile.Data = this.textBox1.Text;
            }

            this.Close();
        }
    }
}
 