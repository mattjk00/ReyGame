using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine
{
    public static class ItemData
    {
        // helmets
        public static readonly Item ironHelmet = new Item("ironHelmet", ItemType.Gear, EquipmentType.Helmet, AssetLoader.LoadTexture("Assets/Textures/armor/iron_helmet.png"));
        public static readonly Item mushroomHelmet = new Item("mushroomHelmet", ItemType.Gear, EquipmentType.Helmet, AssetLoader.LoadTexture("Assets/Textures/armor/mushroom_helmet.png"));

        // chests
        public static readonly Item mushroomChest = new Item("mushroomChest", ItemType.Gear, EquipmentType.Chest, AssetLoader.LoadTexture("Assets/Textures/armor/mushroom_chest.png"), AssetLoader.LoadTexture("Assets/Textures/armor/mushroom_magic_body.png"));

    }
}
