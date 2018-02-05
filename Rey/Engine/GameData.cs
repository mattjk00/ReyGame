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
            ItemData.ironHelmet,
            ItemData.mushroomHelmet,
            ItemData.mushroomChest
        };

        public static Item EquippedHelmet = null;
        public static Item EquippedChest = null;
    }

    public enum EnemyIDs
    {
        Bat = 1,
        BabyFishDemon = 2
    }
}
