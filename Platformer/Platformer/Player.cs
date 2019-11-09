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
        private bool exit = false;
        public float speed = 3;
        public float jumpSpeed = 13;
        public float dashSpeed = 20;
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
        public bool Exit { get => exit; set => exit = value; }

        public void Update(ArrayList objects, InputHelper input, GameTime gameTime)
        {
            if(Position.Y > 720)
            {
                Exit = true;
            }
            KeyboardState k = input.currentKeyboardState;
            if (input.IsNewKeyPress(Keys.Space) && HasDash && input.GetDirection().Length() > 0)
            {
                dashing = true;
                hasDash = false;
                Dash(input);
            }
            if (k.IsKeyDown(Keys.Right))
                Velocity += new Vector2(speed, 0);
            if (k.IsKeyDown(Keys.Left))
                Velocity +=  new Vector2(-speed, 0);
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
                velocity += new Vector2(0, speed / 1.3f);
            float dashTime = .1f;
            Debug.Print("timer:" + dashTimer.ToString() + " position: " + position.ToString() + " Velocity: " + Velocity.ToString() + " dashVector: " + dashVector.ToString());

            if (dashing)
            {
                dashTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                
                Velocity = dashVector * dashSpeed;

                if (dashTimer >= dashTime)
                {
                    dashing = false;
                    dashTimer = 0;
                }
            }

            Vector2 gravity = new Vector2(0,1f);
            float slowing = .9f;

            if (!dashing)
            {
                Velocity += gravity;
                if (Math.Abs(Velocity.X) > 0)
                    Velocity = new Vector2(Velocity.X * slowing, Velocity.Y);
            }
            Velocity = new Vector2(MathHelper.Clamp(Velocity.X, -20, 20), MathHelper.Clamp(Velocity.Y, -20, 10));
            Move(Velocity, objects);
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
            Rectangle overlap = Rectangle.Intersect(BoundingBox, g.BoundingBox);
            if(overlap.Width <= overlap.Height)
            {
                if(Position.X <= g.Position.X) //if on the left of the colliding object
                {
                    position.X -= overlap.Width;
                }
                else //if on the right of the colliding object
                {
                    position.X += overlap.Width;
                }
                velocity.X = 0;
            }
            else
            {
                if (Position.Y <= g.Position.Y) //if above the colliding object
                {
                    position.Y -= overlap.Height;
                    CanJump = true;
                    TouchingGround = true;
                    hasDash = true;
                }
                else //if below the colliding object
                {
                    position.X += overlap.Height;
                }
                velocity.Y = 0;
            }
        }
    }
}
