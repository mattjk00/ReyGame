using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine
{
    public class ChildAnimation : Animation
    {
        public Vector2 LocalPosition { get; set; }

        public ChildAnimation(int frameSize, int frameCount, int animeSpeed) : base(frameSize, frameCount, animeSpeed)
        {
            this.FrameSize = frameSize;
            this.FrameCount = frameCount;
            this.AnimationSpeed = animeSpeed;
        }

        public override void Load()
        {
            this.LocalPosition = new Vector2(0, 0);
            base.Load();
        }

        public void Update(GameObject parent)
        {
            this.Transform.Position = parent.Transform.Position + this.LocalPosition - parent.Transform.Origin;
        }
    }
}
