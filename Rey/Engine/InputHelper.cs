using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine
{
    public static class InputHelper
    {
        // these are being assigned in Game1.cs
        public static Vector2 MousePosition;
        public static Camera2D Camera;
        public static GraphicsDevice GD;
        public static GraphicsDeviceManager GDM;

        public static Vector2 ConvertToWindowPoint(Vector2 p)
        {
            return Vector2.Transform(p, Matrix.Invert(Camera.GetTransformation(GD, GDM)));
        }
    }
}
