using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Rey.Engine.UI
{
    public class ItemButton : Button
    {
        public Item Item { get; set; } = new Item();

        public ItemButton (string name) : base (name)
        {
            this.OnClick += HandleClick;
        }

        // handles what to do when the inventory is clicked
        void HandleClick()
        {
            if (this.Item.ID != null)
            {
                // if a piece of gear
                if (this.Item.ItemType == ItemType.Gear)
                {
                    Item newItem = null; // the new value for this item
                                         // go through each choice
                                         //this.Item = newItem;

                    // find the current inventory slot
                    var oldSlot = new Item();
                    try
                    {
                        oldSlot = GameData.backpack.First(x => x.ID == this.Item.ID);
                    }
                    catch (NullReferenceException)
                    {
                        var a = 0;
                    }
                    var index = GameData.backpack.IndexOf(oldSlot);

                    // if the item is valid
                    if (index >= 0)
                    {
                        switch (this.Item.EquipmentType)
                        {
                            // exchange the currently equipped helmet for whatever the user clicked on
                            case EquipmentType.Helmet:
                                newItem = GameData.EquippedHelmet;
                                GameData.EquippedHelmet = this.Item;
                                break;
                            case EquipmentType.Chest:
                                newItem = GameData.EquippedChest;
                                GameData.EquippedChest = this.Item;
                                break;
                        }
                        GameData.backpack[index] = newItem; // remove the equipped item from the backpack because it is now on the player
                    }
                }

            }
        }

        public override void DrawUI(SpriteBatch sb, Frame frame)
        {
            base.DrawUI(sb, frame);

            if (this.Item != null)
                if (this.Item.ID != null)
                {
                    // scale the item correctly
                    Vector2 itemScale = new Vector2(
                            (float)this.Sprite.Texture.Width / (float)this.Item.Texture.Width,
                            (float)this.Sprite.Texture.Height / (float)this.Item.Texture.Height
                        ) * 0.8f;
                    sb.Draw(this.Item.Texture, this.Transform.Position + new Vector2(5, 5), null, Color.White, 0, Vector2.Zero, itemScale, SpriteEffects.None, 0);
                }
        }
    }
}
