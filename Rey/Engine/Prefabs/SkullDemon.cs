using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine.Prefabs
{
    public class SkullDemon : MushroomMinion
    {
        public override void Load()
        {
            this.Name = "skulldemon";
            this.EntityStats = new EntityStats();
            this.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/Enemies/demons/skull.png"); // load the player texture

            this.speed = 0.15f;
           // this.magicSpeed = 2;
            this.range = 200;
            this.State = EnemyState.Idle;
            this.AddDefaultBoundingBox();
            this.IsEnemy = true;
            this.Transform.Origin = new Vector2(this.Sprite.Texture.Width / 2, this.Sprite.Texture.Height / 2);
            this.EntityStats.MaxHP = 20;
            this.EntityStats.HP = 20;
            this.EntityStats.AttackSpeed = 50;
            this.EntityStats.Aggressive = true;
            this.EntityStats.AttackLevel = 2;
            this.ChooseNewTargetAndInterval();
            this.BoundingBoxes.Add(new Rectangle(0, 0, 0, 0));

            this.shadow.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/player/shadow.png");
            this.shadow.Sprite.Color = new Color(255, 255, 255) * 0.3f;
            this.shadow.LocalPosition = new Vector2(27, 65);

            // load the healthbar and set the stats
            this.healthbar.Load();
            this.healthbar.AssignStats(this.EntityStats);
            this.healthbar.LocalPosition = new Vector2(10, 10);

            // drop table
            this.DropTable.Add(ItemData.New(ItemData.ironHelmet), 0.2f);
            this.DropTable.Add(ItemData.New(ItemData.ironChest), 0.2f);
            this.DropTable.Add(ItemData.New(ItemData.ironLegs), 0.2f);

        }
    }
}
