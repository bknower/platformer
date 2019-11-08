﻿using System;
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
        public DynamicObject(Texture2D texture, Vector2 position, bool corporeal) : base(texture, position, corporeal) { }
    }
}
