using Microsoft.Xna.Framework;
using Rey.Engine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine.Prefabs.UI
{
    public class NPCTalkingFrame : Frame
    {
        // the text of the npc
        Label text = new Label("npctext");
        Button nextButton = new Button("nextButton");

        int talkIndex = 0; // how many different texts that have been had so far

        public NPC NPC { get; set; } // the npc that is talking

        public override void Load()
        {
            this.Active = false;
            this.LockedPosition = true;

            // set the size
            this.Width = AssetLoader.Graphics.PreferredBackBufferWidth/2;
            this.Height = AssetLoader.Graphics.PreferredBackBufferHeight / 4;
            this.Position = new Vector2(0, AssetLoader.Graphics.PreferredBackBufferHeight * 0.65f);

            this.Background = AssetLoader.LoadTexture("Assets/Textures/UI/npc_talking.png");

            text.Text = "Hello there!!";
            text.TextScale = 1.2f;
            text.TextColor = Color.NavajoWhite;
            text.LocalPosition = new Vector2(this.Width / 2, this.Height / 2);
            this.AddObject(text);

            nextButton.LoadTextures(AssetLoader.LoadTexture("Assets/Textures/UI/invetory_bar.png"), AssetLoader.LoadTexture("Assets/Textures/UI/invetory_bar_hover.png"));
            nextButton.Text = "Continue...";
            nextButton.TextColor = Color.Black;
            nextButton.LocalPosition = new Vector2(this.Width / 2, this.Height - this.Height/4);
            nextButton.OnClick += () =>
            {
                this.talkIndex++;
                if (this.talkIndex > this.NPC.Script.Count - 1)
                {
                    // stop the interaction and tell the scene to go back to normal
                    this.Active = false;
                    SceneManager.GetCurrentScene().State = SceneState.Normal;
                    this.talkIndex = 0;
                    this.NPC.State = NPCState.Idle;
                }
            };
            this.AddObject(nextButton);

            base.Load();
        }

        public override void Update()
        {
            base.Update();

            text.Text = this.NPC.Script[this.talkIndex];
        }
    }
}
