using Microsoft.Xna.Framework;
using Rey.Engine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine.Prefabs.UI
{
    /// <summary>
    /// Frame for displaying player stats
    /// </summary>
    public class StatsFrame : Frame
    {
        Label attackLevelLbl = new Label("attackLevel");
        Label defenceLevelLbl = new Label("defenceLevel");
        Label magicSpeedLbl = new Label("magicSpeed");

        public Player player; // the player in the game

        public override void Load()
        {
            this.Name = "inventory";

            this.Width = 200;
            this.Height = 125;
            this.LockedPosition = true;

            this.Position = new Vector2(InputHelper.GDM.PreferredBackBufferWidth - 200, 125);//new Microsoft.Xna.Framework.Vector2(1030, 320);
            this.Background = AssetLoader.LoadTexture("Assets/Textures/UI/ui_back.png");

            attackLevelLbl.TextColor = Color.Goldenrod;
            attackLevelLbl.LocalPosition = new Vector2(5, 5);

            defenceLevelLbl.TextColor = Color.CornflowerBlue;
            defenceLevelLbl.LocalPosition = new Vector2(5, 20);

            magicSpeedLbl.TextColor = Color.PaleVioletRed;
            magicSpeedLbl.LocalPosition = new Vector2(5, 35);

            this.AddObject(attackLevelLbl);
            this.AddObject(defenceLevelLbl);
            this.AddObject(magicSpeedLbl);

            base.Load();
        }

        public override void Update()
        {
            base.Update();

            // get the plyer
            player = SceneManager.GetCurrentScene().gameObjects.Find(x => x.GetType() == typeof(Player)) as Player;

            attackLevelLbl.Text = "Attack: " + player.EntityStats.FullStats.AttackLevel;
            defenceLevelLbl.Text = "Defence: " + player.EntityStats.FullStats.DefenceLevel;
            magicSpeedLbl.Text = "Magic Speed: " + player.EntityStats.FullStats.MagicSpeed;
        }
    }
}
