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
        public static Vector2 MousePosition;
        public static Camera2D Camera;
        public static GraphicsDevice GD;

        public static Vector2 ConvertToWindowPoint(Vector2 p)
        {
            return Vector2.Transform(p, Matrix.Invert(Camera.GetTransformation(GD)));
        }
    }
}
