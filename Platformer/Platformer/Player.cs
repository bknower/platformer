using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer
{
    class Player : DynamicObject
    {
        public Player(Texture2D texture, Vector2 position, bool solid) : base(texture, position, solid) { }

        public override void Move(Vector2 v, ArrayList objects)
        {
            base.Move(v, objects);
            foreach (GameObject g in objects)
            {
                if (Colliding(g))
                {
                    MoveOutOf(v, g);
                }
            }
            
        }

    }
}
