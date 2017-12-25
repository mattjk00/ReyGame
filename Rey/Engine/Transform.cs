using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace Rey.Engine
{

    /// <summary>
    /// Holds data for position for the gameobject
    /// </summary>
    public class Transform
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Rectangle Bounds { get; set; }
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }

        public float VelX
        {
            get { return Velocity.X; }
            set { Velocity = new Vector2(value, Velocity.Y); }
        }

        public float VelY
        {
            get { return Velocity.Y; }
            set { Velocity = new Vector2(Velocity.X, value); }
        }

        public Transform()
        {
            this.Position = Vector2.Zero;
            this.Velocity = Vector2.Zero;
            this.Bounds = Rectangle.Empty;
        }

        public Transform(Vector2 pos)
        {
            this.Position = pos;
            this.Velocity = Vector2.Zero;
            this.Bounds = Rectangle.Empty;
        }
    }
}
