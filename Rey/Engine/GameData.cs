﻿using System;
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

        public static void AddItemToBackpack(Item item)
        {
            var indexOfFirstEmpty = -1;
            // iterate through and find the first empty slot
            for (int i = 0; i < backpack.Count; i++)
            {
                // if it's empty, log it and get out of the loop
                if (backpack[i].ID == null)
                {
                    indexOfFirstEmpty = i;
                    i = 999;
                }
            }
            // if a valid index, add the item
            if (indexOfFirstEmpty >= 0)
                backpack[indexOfFirstEmpty] = item;
        }

        public static Item EquippedHelmet = new Item();
        public static Item EquippedChest = new Item();
    }

    public enum EnemyIDs
    {
        Bat = 1,
        BabyFishDemon = 2
    }
}
