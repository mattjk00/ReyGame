using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Rey.Engine.Memory;
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
        public MapMarker marker;
        public bool Selected { get; set; } // if the tile is selected

        bool clickedThisLoop = false;

        int clickTimer = 0;

        public EditorTile(string name, Vector2 position, Texture2D texture, TileType typ) : base(name, position, texture, typ)
        {
            this.AddDefaultBoundingBox();
            this.StartingPosition = position;

        }

        public override void Update()
        {
            this.clickTimer++;

            this.UpdateDefaultBox(0);
            // get the mouse state
            mouse = Mouse.GetState();
            // set a mouse box
            Rectangle mouseBox = new Rectangle(mouse.Position, new Point(2, 2));

            if (this.clickTimer >= 50) // slows down clicking
            {
                this.clickTimer = 50;
                // same thing as above but an alternative for holding down the mouse
                if (mouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Pressed && mouseBox.Intersects(this.BoundingBoxes[0])
                    && this.Transform.Position.X > 300 && this.clickedThisLoop == false) // this prevents from clicking under UI
                {
                    this.GoOn();
                    this.clickTimer = 0;
                }

                // if mouse is clicked and intersecting this box
                if (mouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Released && mouseBox.Intersects(this.BoundingBoxes[0])
                    && this.Transform.Position.X > 300 && this.clickedThisLoop == false) // this prevents from clicking under UI
                {
                    this.Toggle();
                    this.clickTimer = 0;
                }


            }
            // remove selection if not selected
            if (EditorManager.SelectedTile != this)
                this.Selected = false;

            // if mouse is clicked and intersecting this box
            /*if (mouse.RightButton == ButtonState.Pressed && previousMouse.RightButton == ButtonState.Released && mouseBox.Intersects(this.BoundingBoxes[0])
                && this.Transform.Position.X > 300 && this.clickedThisLoop == false) // this prevents from clicking under UI
            {
                this.TileType = TileType.PlayerStart;
            }*/

            //if (this.marker != null)
            //this.marker.Position = this.Transform.Position;

            if (clickedThisLoop == true)
                previousMouse = mouse;
            clickedThisLoop = false;
        }

        void Toggle()
        {
            this.clickedThisLoop = true;

            if (this.On == false)
            {
                if (EditorManager.TileMode)
                {
                    this.GoOn();
                }
            }
            else
            {
                if (EditorManager.TileMode)
                {
                    this.On = false;
                    this.Sprite.Texture = EditorManager.defaultTile;
                    this.TileType = TileType.Empty;
                    this.marker = null;
                }
                else
                {
                    ToggleMarker();
                }
                //ToggleSelection();
            }

            
            
        }

        void GoOn()
        {
            ToggleSelection();
            if (EditorManager.TileMode)
            {
                this.clickedThisLoop = true;
                this.On = true;
                this.Sprite.Texture = EditorManager.currentTile.Sprite.Texture;
                this.TileType = EditorManager.currentTile.TileType;
                this.Name = EditorManager.currentTileName;
            }
            else
                ToggleMarker();
        }

        void ToggleMarker()
        {
            if (this.marker != null)
                this.marker = null;
            else
            {
                this.marker = new MapMarker(EditorManager.currentMarker.Name, this.StartingPosition, EditorManager.currentMarker.Texture, EditorManager.currentMarker.MarkerType);
                //this.marker.Texture = EditorManager.currentMarker.Texture;
            }
        }

        void ToggleSelection()
        {
            if (this.Selected == false)
            {
                this.Selected = true;
                EditorManager.SelectedTile = this;
            }
            else
            {
                this.Selected = false;
                EditorManager.SelectedTile = null;
            }
        }
    }
}
