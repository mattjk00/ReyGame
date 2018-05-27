using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine
{
    /// <summary>
    /// Particle Effects Like Blood
    /// </summary>
    public class ParticleManager
    {
        // particles
        public List<GameObject> Particles { get; private set; } = new List<GameObject>();

        public void AddParticle(Vector2 pos, Vector2 velocity, Color color)
        {
            // create the particle
            var part = new GameObject("particle", pos);
            part.Transform.Velocity = velocity; // set velocity
            part.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/particles/particle.png");
            part.Load();
            part.Sprite.Color = color; // set color
            this.Particles.Add(part); // add the particle
        }

        Random rand = new Random();
        public void Burst(Vector2 pos, Vector2 velocity, Color color, int count)
        {
            for (int i = 0; i < count; i++)
            {
                this.AddParticle(pos + new Vector2(rand.Next(-8, 8), rand.Next(-4, 4)),
                    velocity + new Vector2(rand.Next(-3, 3), rand.Next(-2, 2)),
                    new Color(color.R + rand.Next(-5, 5), color.G + rand.Next(-5, 5), color.B + rand.Next(-5, 5)));
            }
        }

        public void Update()
        {
            // iterate through particles
            foreach (GameObject particle in Particles)
            {
                particle.Update();
                //particle.Sprite.Color = new Color(particle.Sprite.Color.R, particle.Sprite.Color.G, particle.Sprite.Color.B, particle.Sprite.Color.A *);
                particle.Sprite.Color *= 0.98f;

                if (particle.Sprite.Color.A <= 0)
                    particle.ToBeDestroyed = true;
            }
            this.Particles.RemoveAll(x => x.ToBeDestroyed == true);
        }

        public void Draw(SpriteBatch sb)
        {
            // iterate through particles
            foreach (GameObject particle in Particles)
            {
                particle.Draw(sb);
            }
        }
    }
}
