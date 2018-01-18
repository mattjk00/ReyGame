using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Rey.Engine.Prefabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelEditor
{
    public class EditorTile : Tile
    {
        // whether it's on or off
        bool On { get; set; }
        MouseState mouse;
        MouseState previousMouse;
        public Vector2 StartingPosition;

        bool clickedThisLoop = false;

        public EditorTile(Vector2 position, Texture2D texture, TileType typ) : base(position, texture, typ)
        {
            this.AddDefaultBoundingBox();
            this.StartingPosition = position;
        }

        public override void Update()
        {
            this.UpdateDefaultBox(0);
            // get the mouse state
            mouse = Mouse.GetState();
            // set a mouse box
            Rectangle mouseBox = new Rectangle(mouse.Position, new Point(2, 2));
        
            // if mouse is clicked and intersecting this box
            if (mouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Released && mouseBox.Intersects(this.BoundingBoxes[0]) 
                && this.Transform.Position.X > 300 && this.clickedThisLoop == false) // this prevents from clicking under UI
            {
                this.Toggle();
            }

            // same thing as above but an alternative for holding down the mouse
            if (mouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Pressed && mouseBox.Intersects(this.BoundingBoxes[0])
                && this.Transform.Position.X > 300 && this.clickedThisLoop == false) // this prevents from clicking under UI
            {
                this.GoOn();
            }

            // if mouse is clicked and intersecting this box
            if (mouse.RightButton == ButtonState.Pressed && previousMouse.RightButton == ButtonState.Released && mouseBox.Intersects(this.BoundingBoxes[0])
                && this.Transform.Position.X > 300 && this.clickedThisLoop == false) // this prevents from clicking under UI
            {
                this.TileType = TileType.PlayerStart;
            }

            if (clickedThisLoop == true)
                previousMouse = mouse;
            clickedThisLoop = false;
        }

        void Toggle()
        {
            this.clickedThisLoop = true;
            if (this.On == false)
            {
                this.GoOn();
            }
            else
            {
                this.On = false;
                this.Sprite.Texture = EditorManager.defaultTile;
            }
        }

        void GoOn()
        {
            this.clickedThisLoop = true;
            this.On = true;
            this.Sprite.Texture = EditorManager.currentTile.Sprite.Texture;
            this.TileType = EditorManager.currentTile.TileType;
        }
    }
}
