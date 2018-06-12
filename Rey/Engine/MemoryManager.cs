using Newtonsoft.Json;
using Rey.Engine.Prefabs;
using Rey.Engine.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine
{
    /// <summary>
    /// Class for loading and saving games
    /// </summary>
    public static class MemoryManager
    {
        public static readonly string FilePath = "Assets/Saves/save.guat";

        /// <summary>
        /// Saves
        /// </summary>
        public static void Save()
        {
            // create a new save fiel
            SaveFile saveFile;
            // get the current scene
            var scene = SceneManager.GetCurrentScene() as TestScene;

            // try to get the player's position
            var playerPos = SceneManager.TryToGetPlayerPosition();
            // set the backpack
            var inventory = new List<string>();

            // get all the item names
            foreach (Item item in GameData.backpack)
            {
                inventory.Add(item.Name);
            }

            // helmet = 0, chest = 1, legs = 2
            var equipped = new List<string>() { GameData.EquippedHelmet.Name, GameData.EquippedChest.Name, GameData.EquippedLegs.Name };
            var map = scene.GetMapPath();

            saveFile = new SaveFile(playerPos, inventory, equipped, map);
            var json = JsonConvert.SerializeObject(saveFile);
            // write the save file
            System.IO.File.WriteAllText(FilePath, json);
        }

        public static void Load()
        {
            // load the file
            string json = System.IO.File.ReadAllText(FilePath);
            // deserialize
            var saveFile = JsonConvert.DeserializeObject<SaveFile>(json);

            List<Item> newBackpack = new List<Item>();
            // iterate through
            foreach (string name in saveFile.Backpack)
            {
                // add the item to it
                if (name != null)
                    newBackpack.Add(ItemData.New(ItemData.AllItems().First(x => x.Name == name)));
                else
                    newBackpack.Add(new Item());
            }
            // refill backpack
            //GameData.backpack.Clear();
            for (int i = 0; i < GameData.backpack.Count; i++)
            {
                GameData.backpack[i] = new Item(); // clear
            }
            for (int i = 0; i < GameData.backpack.Count; i++)
            {
                if (newBackpack[i].ID != null)
                    GameData.backpack[i] = ItemData.New(newBackpack[i]);
            }
            var bp = GameData.backpack;
            if (saveFile.Equipped[0] != null)
                GameData.EquippedHelmet = ItemData.New(ItemData.AllItems().First(x => x.Name == saveFile.Equipped[0]));
            if (saveFile.Equipped[1] != null)
                GameData.EquippedChest = ItemData.New(ItemData.AllItems().First(x => x.Name == saveFile.Equipped[1]));
            if (saveFile.Equipped[2] != null)
                GameData.EquippedLegs = ItemData.New(ItemData.AllItems().First(x => x.Name == saveFile.Equipped[2]));

            // set scene and drop in
            SceneManager.SetScene("test");
            var ts = SceneManager.GetCurrentScene() as TestScene;
            ts.GoToMap(true, saveFile.Map);
            var player = ts.gameObjects.First(x => x.GetType() == typeof(Player)) as Player;
            player.Transform.Position = saveFile.PlayerPosition;
            player.EntityStats.HP = player.EntityStats.FullStats.MaxHP; // heal
        }
    }
}
