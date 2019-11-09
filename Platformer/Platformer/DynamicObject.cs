using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer
{
    class DynamicObject : GameObject
    {
        public Vector2 velocity;

        public DynamicObject(Texture2D texture, Vector2 position, bool corporeal) : base(texture, position, corporeal) { }

        public Vector2 Velocity { get => velocity; set => velocity = value; }
    }
}
