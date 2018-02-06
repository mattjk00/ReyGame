using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine
{
    public static class GameData
    {
        // the player's backpack
        public static List<Item> backpack = new List<Item>()
        {
            ItemData.New(ItemData.ironHelmet),
            ItemData.New(ItemData.mushroomChest),
            ItemData.New(ItemData.ironHelmet),
            ItemData.New(ItemData.mushroomHelmet),
            ItemData.New(ItemData.ironHelmet),
            ItemData.New(ItemData.ironHelmet),
            ItemData.New(ItemData.mushroomChest),
            ItemData.New(ItemData.mushroomHelmet),
            ItemData.New(ItemData.ironHelmet),
            ItemData.New(ItemData.ironHelmet),
            ItemData.New(ItemData.mushroomChest),
            ItemData.New(ItemData.mushroomHelmet),
            ItemData.New(ItemData.trainingChest)
        };

        public static Item EquippedHelmet = new Item();
        public static Item EquippedChest = new Item();
    }

    public enum EnemyIDs
    {
        Bat = 1,
        BabyFishDemon = 2
    }
}
