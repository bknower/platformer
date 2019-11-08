using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Platformer
{
    class Player : DynamicObject
    {
        public float speed = 8;
        public float jumpSpeed = 15;
        public float dashSpeed = 5;
        private bool hasDash = false;
        private bool canJump;
        private bool touchingGround;
        public float dashTimer = 0;
        public bool dashing = false;
        public Vector2 dashVector;
        public Player(Texture2D texture, Vector2 position, bool solid) : base(texture, position, solid)
        {
            canJump = false;
            TouchingGround = false;
        }

        public bool CanJump { get => canJump; set => canJump = value; }
        public bool TouchingGround { get => touchingGround; set => touchingGround = value; }
        public bool HasDash { get => hasDash; set => hasDash = value; }

        public void Update(ArrayList objects, InputHelper input, GameTime gameTime)
        {
            Debug.Print(position.ToString());
            Debug.Print(Velocity.ToString());
            KeyboardState k = input.currentKeyboardState;
            if (k.IsKeyDown(Keys.Space) && HasDash)
            {
                dashing = true;
                hasDash = false;
                Dash(input);
            }
            if (k.IsKeyDown(Keys.Right))
                Move(new Vector2(speed, 0), objects);
            if (k.IsKeyDown(Keys.Left))
                Move(new Vector2(-speed, 0), objects);
            if (k.IsKeyDown(Keys.Up))
            {
                if (CanJump)
                {
                    Velocity = new Vector2(Velocity.X, Velocity.Y - jumpSpeed);
                    TouchingGround = false;
                    CanJump = false;
                }
            }
            if (k.IsKeyDown(Keys.Down))
                Move(new Vector2(0, speed/1.3f), objects);
            float dashTime = .2f;

            if (dashing)
            {
                dashTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                Debug.Print(dashTimer.ToString());
                Velocity = dashVector * dashSpeed;
                Debug.Print("dashVector: " + dashVector.ToString());
                if (dashTimer >= dashTime)
                {
                    dashing = false;
                    dashTimer = 0;
                }
            }

            Vector2 gravity = new Vector2(0,1f);
            Vector2 slowing = new Vector2(.1f);
            Velocity += gravity;
            if (Math.Abs(Velocity.X) > 0)
                Velocity *= .98f;
        }
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
        public void Dash(InputHelper i)
        {
            dashing = true;
            dashVector = i.GetDirection();
        }
        public void MoveOutOf(Vector2 v, GameObject g)
        {
            if (v.X > 0)
            {
                position.X = g.position.X - BoundingBox.Width;
            }
            else if (v.X < 0)
            {
                position.X = g.position.X + g.BoundingBox.Width;
            }
            if (v.Y > 0)
            {
                position.Y = g.position.Y - BoundingBox.Height;
                Velocity = new Vector2(Velocity.X, 0);
                CanJump = true;
                TouchingGround = true;
                hasDash = true;
            }
            else if (v.Y < 0)
            {
                position.Y = g.position.Y + g.BoundingBox.Height;
                
            }
        }
    }
}
