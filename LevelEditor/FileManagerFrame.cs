using Microsoft.Xna.Framework;
using Rey.Engine;
using Rey.Engine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelEditor
{
    public class FileManagerFrame : Frame
    {
        Button loadButton = new Button("load");
        Button saveButton = new Button("save");

        public override void Load()
        {
            this.Name = "fileui";
            this.Scrollable = false;
            this.Background = AssetLoader.LoadTexture("Textures/fileui.png");

            this.Width = 1280;
            this.Height = 20;

            loadButton.LoadTextures("Textures/load_button.png", "Textures/load_button_hover.png");
            loadButton.LocalPosition = new Vector2(0, 0);
            loadButton.OnClick += () =>
            {

            };

            saveButton.LoadTextures("Textures/save_button.png", "Textures/save_button_hover.png");
            saveButton.LocalPosition = new Vector2(100, 0);
            saveButton.OnClick += () =>
            {
                // save the file
                System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();
                sfd.Filter = "Rey Map File|*.guat";
                sfd.Title = "Save the map";
                System.Windows.Forms.DialogResult dialogResult = sfd.ShowDialog();
                
                if (sfd.FileName != "" && dialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    System.IO.File.WriteAllText(sfd.FileName, "rey was here");
                }
            };

            this.AddObject(loadButton);
            this.AddObject(saveButton);

            base.Load();
        }
    }
}
