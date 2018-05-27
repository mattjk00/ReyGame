using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Rey.Engine.Prefabs
{
    // something you can pick up off the ground
    public class Pickup : GameObject
    {
        // direction of bob
        int bounceDir = 1;
        int bounceCount = 1;

        public Item PickupItem { get; set; }

        public Rectangle Box { get; set; }

        public override void Load()
        {
            this.Sprite.Texture = this.PickupItem.Texture;
            base.Load();

        }

        public override void Update()
        {
            base.Update();

            Bounce();
            

            if (this.PickupItem.EquipmentType == EquipmentType.Legs)
            {
                this.Transform.Bounds = new Rectangle(0, 0, this.Sprite.Texture.Width/7, this.Sprite.Texture.Height);
                this.Box = new Rectangle((int)this.Transform.Position.X, (int)this.Transform.Position.Y, (int)(this.Sprite.Texture.Width / 7), this.Sprite.Texture.Height);
                this.Transform.Origin = new Vector2((int)(this.Sprite.Texture.Width / 7)/2, this.Sprite.Texture.Height / 2);
            }
            else
            {
                this.Transform.Origin = new Vector2(this.Sprite.Texture.Width / 2, this.Sprite.Texture.Height / 2);
                this.Box = new Rectangle((int)this.Transform.Position.X, (int)this.Transform.Position.Y, this.Sprite.Texture.Width, this.Sprite.Texture.Height);
            }
        }

        void Bounce()
        {
            bounceCount += 1;

            if (bounceCount > 20)
            {
                bounceDir *= -1;
                bounceCount =1;
            }

            // bounce up and down
            //this.Transform.Position += new Vector2(0, bounceDir);
            //this.Transform.Rotation += MathHelper.ToRadians(bounceCount);
            this.Transform.Scale += new Vector2((float)bounceDir / (float)100);
            this.Sprite.Color = Color.LightYellow * 0.95f;
        }

        /// <summary>
        /// Get picked up
        /// </summary>
        public void GetPickedUp()
        {
            // add item and destroy it if successfully added to backpack
            this.ToBeDestroyed = GameData.AddItemToBackpack(ItemData.New(this.PickupItem));
        }
    }
}
