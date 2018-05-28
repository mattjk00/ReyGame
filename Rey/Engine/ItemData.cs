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
        public static readonly Item darkMageHelmet = new Item("Dark Mage Hood", ItemType.Gear,
            EquipmentType.Helmet, AssetLoader.LoadTexture("Assets/Textures/armor/dark_helmet.png"),
            new EntityStats(0, 7, 7, 7, 7, 7));

        // chests
        public static readonly Item mushroomChest = new Item("Shroom Torso", ItemType.Gear,
            EquipmentType.Chest, AssetLoader.LoadTexture("Assets/Textures/armor/mushroom_chest.png"),
            AssetLoader.LoadTexture("Assets/Textures/armor/mushroom_magic_body.png"),
            new EntityStats(1, 2, 1, 2, 1, 4));
        public static readonly Item trainingChest = new Item("Clora Robes", ItemType.Gear,
            EquipmentType.Chest, AssetLoader.LoadTexture("Assets/Textures/armor/training_chest.png"),
            AssetLoader.LoadTexture("Assets/Textures/armor/training_magic_body.png"),
            new EntityStats(0, 3, 2, 2, 2, 2));
        public static readonly Item ironChest = new Item("Iron Chest", ItemType.Gear,
           EquipmentType.Chest, AssetLoader.LoadTexture("Assets/Textures/armor/iron_chest.png"),
           AssetLoader.LoadTexture("Assets/Textures/armor/iron_magic_body.png"),
           new EntityStats(0, 3, 7, 15, 0, -5));
        public static readonly Item darkMageChest = new Item("Dark Mage Chest", ItemType.Gear,
           EquipmentType.Chest, AssetLoader.LoadTexture("Assets/Textures/armor/dark_chest.png"),
           AssetLoader.LoadTexture("Assets/Textures/armor/dark_magic_body.png"),
           new EntityStats(0, 10, 10, 10, 10, 10));

        // legs
        public static readonly Item ironLegs = new Item("Iron Legs", ItemType.Gear,
            EquipmentType.Legs, AssetLoader.LoadTexture("Assets/Textures/armor/iron_legs.png"),
            new EntityStats(0, 2, 2, 4, 1, 0));
        public static readonly Item mushroomLegs = new Item("Mushroom Legs", ItemType.Gear,
            EquipmentType.Legs, AssetLoader.LoadTexture("Assets/Textures/armor/mushroom_legs.png"),
            new EntityStats(0, 0, 1, -3, 0, 4));
        public static readonly Item darkMageLegs = new Item("Dark Mage Robes", ItemType.Gear,
            EquipmentType.Legs, AssetLoader.LoadTexture("Assets/Textures/armor/dark_legs.png"),
            new EntityStats(0, 5, 5, 0, 10, 10));

    }
}
