using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections;

namespace Platformer
{
    public class Platformer : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        InputHelper input;
        Texture2D pixel;

        
        public ArrayList staticObjects = new ArrayList();
        public ArrayList dynamicObjects = new ArrayList();
        public ArrayList hitboxes = new ArrayList();

        StaticObject background;
        StaticObject block;
        StaticObject floor;
        Player player;

        public Platformer()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            base.Initialize();
            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
        }

        protected override void LoadContent()
        {
            this.IsMouseVisible = true;
            LoadGameObjects();
            input = new InputHelper();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //origin = new Vector2(texture.Width / 2, texture.Height / 2);
        }

        void LoadGameObjects()
        {
            background =
                new StaticObject(
                    Content.Load<Texture2D>(@"background"),
                    new Vector2(0, 0),
                    false);
            //staticObjects.Add(background);

            player =
                new Player(
                    Content.Load<Texture2D>(@"player"),
                    new Vector2(0, graphics.PreferredBackBufferHeight / 2),
                    true);
            Texture2D blocktex = Content.Load<Texture2D>(@"block");
            block =
                new StaticObject(
                    blocktex,
                    new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight - blocktex.Height),
                    true);
            staticObjects.Add(block);
            floor =
                new StaticObject(
                    Content.Load<Texture2D>(@"floor"),
                    new Vector2(0, graphics.PreferredBackBufferHeight - blocktex.Height),
                    true);
            staticObjects.Add(floor);
        }

        void ShowHitboxes(int width)
        {
            foreach (GameObject o in staticObjects) {
                DrawBorder(o.BoundingBox, width, Color.Red);
            }
            foreach (GameObject o in dynamicObjects)
            { 
                DrawBorder(o.BoundingBox, width, Color.Red);
            }
            DrawBorder(player.BoundingBox, width, Color.Red);
        }
        private void DrawBorder(Rectangle rectangleToDraw, int thicknessOfBorder, Color borderColor)
        {
            // Draw top line
            spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, rectangleToDraw.Width, thicknessOfBorder), borderColor);

            // Draw left line
            spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), borderColor);

            // Draw right line
            spriteBatch.Draw(pixel, new Rectangle((rectangleToDraw.X + rectangleToDraw.Width - thicknessOfBorder),
                                            rectangleToDraw.Y,
                                            thicknessOfBorder,
                                            rectangleToDraw.Height), borderColor);
            // Draw bottom line
            spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X,
                                            rectangleToDraw.Y + rectangleToDraw.Height - thicknessOfBorder,
                                            rectangleToDraw.Width,
                                            thicknessOfBorder), borderColor);
        }
        protected override void UnloadContent() { }

        protected override void Update(GameTime gameTime)
        {
            input.Update();
            if (input.currentKeyboardState.IsKeyDown(Keys.Escape)) { Exit(); }
            base.Update(gameTime);

            player.Update(staticObjects, input, gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            foreach (GameObject o in staticObjects)
                o.Draw(spriteBatch);
            foreach (GameObject o in dynamicObjects)
                o.Draw(spriteBatch);
            player.Draw(spriteBatch);
            ShowHitboxes(2);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

