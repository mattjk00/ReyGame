using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine
{
    public static class ItemData
    {
        static Random random = new Random();
        public static Item New(Item item)
        {
            var nitem = new Item(item.Name, item.ItemType, item.EquipmentType, item.Texture, item.AltTexture, item.Stats);
            var newID = Guid.NewGuid().ToString().Split('-')[0];
            nitem.ID = newID;
            
            return nitem;
        }
        
        // helmets
        public static readonly Item ironHelmet = new Item("Iron Helmet", ItemType.Gear, 
            EquipmentType.Helmet, AssetLoader.LoadTexture("Assets/Textures/armor/iron_helmet.png"),
            new EntityStats(0, 2, 1, 2, 1, 0));
        public static readonly Item mushroomHelmet = new Item("Shroom Helm", ItemType.Gear,
            EquipmentType.Helmet, AssetLoader.LoadTexture("Assets/Textures/armor/mushroom_helmet.png"),
            new EntityStats(0, 1, 1, 1, 1, 4));

        // chests
        public static readonly Item mushroomChest = new Item("Shroom Torso", ItemType.Gear,
            EquipmentType.Chest, AssetLoader.LoadTexture("Assets/Textures/armor/mushroom_chest.png"),
            AssetLoader.LoadTexture("Assets/Textures/armor/mushroom_magic_body.png"),
            new EntityStats(1, 2, 1, 2, 1, 4));
        public static readonly Item trainingChest = new Item("Clora Robes", ItemType.Gear,
            EquipmentType.Chest, AssetLoader.LoadTexture("Assets/Textures/armor/training_chest.png"),
            AssetLoader.LoadTexture("Assets/Textures/armor/training_magic_body.png"),
            new EntityStats(0, 3, 2, 2, 2, 2));



    }
}
