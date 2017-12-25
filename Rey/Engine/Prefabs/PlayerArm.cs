using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine.Prefabs
{
    public class PlayerArm : GameObject
    {
        public Vector2 LocalPosition { get; set; }

        public override void Load()
        {
            this.LocalPosition = new Vector2(13, 40);
            base.Load();
        }
    }
}
