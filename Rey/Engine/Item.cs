using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine
{
    public enum ItemType {
        Gear = 0,
        Food = 1,
    }
    public enum EquipmentType
    {
        NotValid = 0,
        Helmet = 1,
        Chest = 2,
        Legs = 3,
        Shield = 4,
        Weapon = 5
    }
    /// <summary>
    /// An item that you can collect in the game, for example food, weapons, armor
    /// </summary>
    public class Item
    {
        public string Name { get; set; }
        public Texture2D Texture { get; set; }
        public Texture2D AltTexture { get; set; } // alternate teture
        public ItemType ItemType { get; set; } = ItemType.Gear;
        public EquipmentType EquipmentType { get; set; } = EquipmentType.NotValid;
        public string ID { get; set; }
        public EntityStats Stats { get; set; }

        public Item() { } 

        public Item(string name, ItemType itemType, EquipmentType equipmentType, Texture2D texture, EntityStats entityStats)
        {
            this.Name = name;
            this.ItemType = itemType;
            this.EquipmentType = equipmentType;
            this.Texture = texture;
            this.Stats = entityStats;
        }

        public Item(string name, ItemType itemType, EquipmentType equipmentType, Texture2D texture, Texture2D altTexture, EntityStats entityStats)
        {
            this.Name = name;
            this.ItemType = itemType;
            this.EquipmentType = equipmentType;
            this.Texture = texture;
            this.AltTexture = altTexture;
            this.Stats = entityStats;
        }
    }
}
