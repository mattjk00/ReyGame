using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Rey.Engine
{
    public class ChildObject : GameObject
    {
        public Vector2 LocalPosition { get; set; }

        public override void Load()
        {
            this.LocalPosition = new Vector2(0, 0);
            base.Load();
        }
        //            this.arm.Transform.Position = this.Transform.Position + this.arm.LocalPosition - this.Transform.Origin;

        public void Update(GameObject parent)
        {
            this.Transform.Position = parent.Transform.Position + this.LocalPosition - parent.Transform.Origin;
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }
    }
}
