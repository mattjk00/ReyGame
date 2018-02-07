using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine
{
    public class Camera2D
    {
        protected float zoom; 
        public Matrix Transform { get; set; } 
        public Vector2 Position { get; set; } 
        public float Rotation { get; set; }
        public Vector2 Resolution { get; set; }

        public Camera2D(float x, float y)
        {
            zoom = 1.0f;
            Rotation = 0.0f;
            Position = Vector2.Zero;
            this.Resolution = new Vector2(x, y);
        }

        // Sets and gets zoom
        public float Zoom
        {
            get { return zoom; }
            set { zoom = value; if (zoom < 0.1f) zoom = 0.1f; } // Negative zoom will flip image
        }

        public float ViewportWidth { get; set; }
        public float ViewportHeight { get; set; }


        // Auxiliary function to move the camera
        public void Move(Vector2 amount)
        {
            Position += amount;
        }

        public Matrix GetTransformation(GraphicsDevice graphicsDevice, GraphicsDeviceManager gdm)
        {
            var width = (float)gdm.PreferredBackBufferWidth / this.Resolution.X;
            var height = (float)gdm.PreferredBackBufferHeight / this.Resolution.Y;
            Transform =      
              Matrix.CreateTranslation(new Vector3(-this.Position.X, -this.Position.Y, 0)) *
                                         Matrix.CreateRotationZ(Rotation) *
                                         Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                         Matrix.CreateTranslation(new Vector3(ViewportWidth * 0.5f, ViewportHeight * 0.5f, 0)) *
                                         Matrix.CreateScale(new Vector3(width, height, 1));
            return Transform;
        }
    }
}
