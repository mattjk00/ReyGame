﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine.Prefabs
{
    /// <summary>
    /// A projectile is a flying object that likely does damage, for example, an arrow or magic blast
    /// </summary>
    public class Projectile : GameObject
    {
        public bool MagicType { get; set; } = true;
        

        // make sure to call this after loading a texture
        public override void Load()
        {
            this.Transform.Origin = new Vector2(this.Sprite.Texture.Width/2, this.Sprite.Texture.Height / 2);
            this.AddDefaultBoundingBox();
        }

        // shoots a projectile at a target
        public void LookAt(Vector2 target)
        {
            Vector2 dir = target - this.Transform.Position;
            this.Transform.Rotation = (float)Math.Atan2(dir.Y, dir.X); // points towards an object
        }

        public override void Update()
        {
            Vector2 dir = new Vector2((float)Math.Cos(this.Transform.Rotation), (float)Math.Sin(this.Transform.Rotation));
            dir.Normalize();
            this.Transform.Position += dir * 10;

            base.Update();
        }
    }
}
