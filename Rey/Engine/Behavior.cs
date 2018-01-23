using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine
{
    /// <summary>
    /// Can be attached to a game object to do actions
    /// </summary>
    public class Behavior
    {
        public virtual void Load(GameObject self)
        {

        }

        public virtual void Update(GameObject self)
        {

        }

        public virtual void Draw(GameObject self, SpriteBatch sb)
        {

        }
    }
}
