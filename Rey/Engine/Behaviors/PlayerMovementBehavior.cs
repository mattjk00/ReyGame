using Microsoft.Xna.Framework.Input;
using Rey.Engine.Prefabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine.Behaviors
{
    public class PlayerMovementBehavior : Behavior
    {
        KeyboardState keyboard;
        KeyboardState lastKeyboard;

        public float Speed;
        public Direction Direction;

        public override void Update(GameObject self)
        {
            self = self as Player;
            this.keyboard = Keyboard.GetState();
            //this.mouse = Mouse.GetState();

            /* movement input */
            if (this.keyboard.IsKeyDown(Keys.D))
            {
                self.Transform.VelX += Speed;
                //self.arm.LocalPosition = new Vector2(13, 40);
            }
            if (this.keyboard.IsKeyDown(Keys.A))
            {
                self.Transform.VelX += -Speed;
                Direction = Direction.MovingLeft;
                //this.arm.LocalPosition = new Vector2(26, 40);
            }
            if (this.keyboard.IsKeyDown(Keys.W))
                self.Transform.VelY += -Speed;
            if (this.keyboard.IsKeyDown(Keys.S))
                self.Transform.VelY += Speed;

            if (InputHelper.MousePosition.X > self.Transform.Position.X)
                Direction = Direction.MovingRight;
            else
                Direction = Direction.MovingLeft;

            lastKeyboard = this.keyboard;
            //lastMouse = this.mouse;
        }
    }
}
