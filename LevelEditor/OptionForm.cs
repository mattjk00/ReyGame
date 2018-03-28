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

            

            // if contains some data, give the second part
            if (EditorManager.SelectedTile.Data.Contains(';'))
            {
                this.textBox1.Text = EditorManager.SelectedTile.Data.Split(';')[0]; ;
                this.dataTextbox.Text = EditorManager.SelectedTile.Data.Split(';')[1];
            }
            else
            {
                this.textBox1.Text = EditorManager.SelectedTile.Data;
            }

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

            // set the data
            EditorManager.SelectedTile.Data = this.dataTextbox.Text;

            // if the tile type is a door, set the data for where it leads to
            if (EditorManager.SelectedTile.TileType == TileType.Door)
            {
                EditorManager.SelectedTile.Data = this.textBox1.Text;

                // if the data textbox contains data, append it. This box is used to name what the door is
                if (this.dataTextbox.Text.Trim(' ').Length > 0)
                    EditorManager.SelectedTile.Data += ";" + this.dataTextbox.Text;
            }

            this.Close();
        }
    }
}
 