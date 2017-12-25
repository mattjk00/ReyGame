using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine
{
    public class Animation : GameObject
    {
        public int FrameSize { get; set; }
        public int AnimationSpeed { get; set; }
        public int FrameCount { get; set; }
        private int timer = 0;
        private int currentFrame = 0;

        public Animation(int frameSize, int frameCount, int animeSpeed)
        {
            this.FrameSize = frameSize;
            this.FrameCount = frameCount;
            this.AnimationSpeed = animeSpeed;
        }

        /// <summary>
        /// Animates by going through each frame of the spritesheet
        /// </summary>
        /// <param name="speed"></param>
        public void Animate(int speed = -1)
        {
            // change optional animation speed if too slow
            if (speed <= 0)
                speed = this.AnimationSpeed;

            this.timer++;
            if (this.timer > this.AnimationSpeed)
            {
                this.currentFrame++;
                if (this.currentFrame > this.FrameCount)
                {
                    this.currentFrame = 0;
                }
                this.timer = 0;
            }

            this.Transform.Bounds = new Rectangle(this.currentFrame * this.FrameSize, 0, this.FrameSize, this.FrameSize); // set the sprite bounds to match the animation
        }

        // sets to a certain frame
        public void SetFrame(int frame)
        {
            this.currentFrame = frame;
            this.Transform.Bounds = new Rectangle(this.currentFrame * this.FrameSize, 0, this.FrameSize, this.FrameSize); // set the sprite bounds to match the animation
        }

        public override void Update()
        {
            this.Animate();
            base.Update();
        }
    }
}
