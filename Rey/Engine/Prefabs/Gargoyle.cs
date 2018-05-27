using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine.Prefabs
{
    public class Gargoyle : Bat
    {
        public override void Load()
        {
            this.Name = "gargoyle";
            this.EntityStats = new EntityStats();
            this.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/Enemies/bats/gargoyle_head.png"); // load the player texture

            this.speed = 0.15f;
            this.State = EnemyState.Idle;
            this.AddDefaultBoundingBox();
            this.IsEnemy = true;
            this.Transform.Origin = new Vector2(this.Sprite.Texture.Width / 2, this.Sprite.Texture.Height / 2);
            this.EntityStats.MaxHP = 35;
            this.EntityStats.HP = 35;
            this.EntityStats.AttackSpeed = 50;
            this.EntityStats.Aggressive = true;
            this.EntityStats.AttackLevel = 2;
            this.ChooseNewTargetAndInterval();
            this.BoundingBoxes.Add(new Rectangle(0, 0, 0, 0));

            this.body.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/Enemies/bats/gargoyle_flying.png");
            this.body.LocalPosition = new Vector2(-5, 30);

            this.legs.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/Enemies/bats/gargoyle_legs.png"); // load the player texture
            //this.legs.Load();
            this.legs.LocalPosition = new Vector2(20, 75);

            this.shadow.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/player/shadow.png");
            this.shadow.Sprite.Color = new Color(255, 255, 255) * 0.3f;
            this.shadow.LocalPosition = new Vector2(25, 120);

            // load the healthbar and set the stats
            this.healthbar.Load();
            this.healthbar.AssignStats(this.EntityStats);
            this.healthbar.LocalPosition = new Vector2(5, -5);

            // drop table
            this.DropTable.Add(ItemData.New(ItemData.ironChest), 0.25f);
            this.DropTable.Add(ItemData.New(ItemData.ironHelmet), 0.25f);
            this.DropTable.Add(ItemData.New(ItemData.ironLegs), 0.25f);
        }
    }
}
