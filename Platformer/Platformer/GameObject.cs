using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer
{
    class GameObject
    {
        public Texture2D texture;
        public Vector2 position;
        public bool solid;
        public float scale;
        public Texture2D Texture { get => texture; set => texture = value; }
        public Vector2 Position { get => position; set => position = value; }

        public GameObject(Texture2D texture, Vector2 position, bool solid)
        {
            this.Texture = texture;
            this.Position = position;
            this.solid = solid;
            this.scale = 1f;
        }

        public Rectangle BoundingBox
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height); }
        }

        public void Draw(SpriteBatch s)
        {
            s.Draw(Texture, position, Color.White);
        }

        public bool Colliding(GameObject o)
        {
            return BoundingBox.Intersects(o.BoundingBox);
        }
        public virtual void Move(Vector2 v, ArrayList objects)
        {
            this.Position += v;
        }
    }
}
