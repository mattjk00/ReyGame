using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Rey.Engine.Prefabs
{
    enum MBossAttackMode
    {
        SingleSlowFire,
        SingleFastFire
    }

    public class MushroomBoss : MushroomMinion
    {
        // the hood of the mushroom
        protected ChildObject hood = new ChildObject();
        protected ChildObject legs = new ChildObject();

        bool hoodDestroyed = false;

        Texture2D boundingBoxTexture;

        MBossAttackMode AttackMode { get; set; } = MBossAttackMode.SingleFastFire;

        private int randomMagicTimer = 0;

        public override void Load()
        {
            this.Name = "mushroomboss";
            this.EntityStats = new EntityStats();
            this.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/Enemies/bosses/mushroomboss.png"); // load the player texture

            this.speed = 0;
            this.shotSpeed = 8;
            this.shotDamage = 3;
            this.State = EnemyState.Idle;
            this.range = 0;
            //this.Transform.Origin = new Vector2(this.Sprite.Texture.Width / 2, this.Sprite.Texture.Height / 2);
            //this.Transform.Position += new Vector2(100, 0);
            this.AddDefaultBoundingBox();
            this.IsEnemy = true;

            this.EntityStats.MaxHP = 1000;
            this.EntityStats.HP = 1000;
            this.EntityStats.AttackSpeed = 50;
            this.EntityStats.Aggressive = true;
            this.EntityStats.AttackLevel = 5;
            this.ChooseNewTargetAndInterval();
            //this.BoundingBoxes.Add(new Rectangle(0, 0, 0, 0));


            this.shadow.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/player/shadow.png");
            this.shadow.Sprite.Color = new Color(255, 255, 255) * 0.3f;
            this.shadow.LocalPosition = new Vector2(27, 65);

            // load the healthbar and set the stats
            this.healthbar.Load();
            this.healthbar.AssignStats(this.EntityStats);
            this.healthbar.LocalPosition = new Vector2(0, -220);

            this.hood.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/Enemies/bosses/mushroomboss_hood.png");
            this.hood.Load();
            this.hood.LocalPosition = new Vector2(-145, -195);

            this.legs.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/Enemies/bosses/mushroomboss_legs.png");
            this.legs.Load();
            this.legs.LocalPosition = new Vector2(-50, 120);

            this.BoundingBoxes.Add(new Rectangle((int)this.Transform.Position.X - 95,
                (int)this.Transform.Position.Y - this.hood.Sprite.Texture.Height + 85,
                this.hood.Sprite.Texture.Width - 100,
                this.hood.Sprite.Texture.Height - 75));

            this.BoundingBoxes.Add(new Rectangle((int)this.Transform.Position.X - 45,
                (int)this.Transform.Position.Y + 125,
                this.legs.Sprite.Texture.Width - 10,
                this.legs.Sprite.Texture.Height - 5));

            boundingBoxTexture = AssetLoader.LoadTexture("Assets/Textures/ui/bb.png");

            // drop table
            this.DropTable.Add(ItemData.New(ItemData.mushroomHelmet), 1);
            this.DropTable.Add(ItemData.New(ItemData.mushroomChest), 1);
            this.DropTable.Add(ItemData.New(ItemData.mushroomLegs), 1);


        }

        public override void Update()
        {

            base.Update();
            if (this.State != EnemyState.Dead)
                this.State = EnemyState.Chasing;
            this.hood.Update(this);
            this.legs.Update(this);

            // check to destroy hood
            if (this.EntityStats.HP < this.EntityStats.FullStats.MaxHP / 2 && !hoodDestroyed)
            {
                this.hood.Sprite.Color *= 0.85f;
                if (this.hood.Sprite.Color.A <= 0)
                {
                    hoodDestroyed = true;
                    this.hood.Sprite.Color = new Color(0, 0, 0, 1); // set basically invisible
                }
            }
        
            // if the hood is destroyed, remove the bounding box for it. the color variable is used so it doesn't try and delete the bounding box twice
            if (hoodDestroyed)
            {
                if (this.hood.Sprite.Color.A == 1)
                {
                    this.BoundingBoxes.RemoveAt(1); // i think  
                    this.hood.Sprite.Color = new Color(0, 0, 0, 0);
                }
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);

            hood.Draw(sb);
            legs.Draw(sb);


            /*foreach (Rectangle rect in this.BoundingBoxes)
            {
                sb.Draw(boundingBoxTexture, rect.Location.ToVector2(), null, Color.Green, 0, Vector2.Zero,
                    InputHelper.ScaleTexture(boundingBoxTexture, rect.Width, rect.Height),
                    SpriteEffects.None, 0);

                sb.Draw(boundingBoxTexture, this.BoundingBoxes[2].Location.ToVector2(), null, Color.Blue, 0, Vector2.Zero,
                    InputHelper.ScaleTexture(boundingBoxTexture, this.BoundingBoxes[2].Width, this.BoundingBoxes[2].Height),
                    SpriteEffects.None, 0);
            }*/
        }

        Random rand = new Random();
        protected override void MoveAround()
        {
            if (this.AttackMode == MBossAttackMode.SingleSlowFire)
            {
                this.magicTimer += magicSpeed;
                this.randomMagicTimer += magicSpeed;
            }
            else if (this.AttackMode == MBossAttackMode.SingleFastFire)
            {
                this.magicTimer += magicSpeed * 3;
                this.randomMagicTimer += magicSpeed * 3;
            }

            

            // shoot the playa
            if (this.magicTimer > this.EntityStats.AttackSpeed)
            {
                this.HandleMagicAttack();
            }

            if (this.randomMagicTimer > this.EntityStats.AttackSpeed/2)
            {
                this.randomMagicTimer = 0;
                this.projectileManager.ShootNew(this.Transform.Position, new Vector2(rand.Next(0, 2000), rand.Next(0, 2000)), shotSpeed, shotDamage, this.EntityStats, ProjectileType.Mushroom);
            }

            /*if (this.AttackMode == MBossAttackMode.SingleFastFire && this.magicTimer > this.EntityStats.AttackSpeed / 2)
                this.HandleMagicAttack();*/
        }

        protected override void HandleMagicAttack()
        {
            this.magicTimer = 0;

            // shoot a thing
            this.projectileManager.ShootNew(this.Transform.Position, this.playerTarget.Transform.Position, shotSpeed, shotDamage, this.EntityStats, ProjectileType.Mushroom);

        }
    }
}
