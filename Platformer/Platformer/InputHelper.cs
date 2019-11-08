using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Platformer
{
    public enum MouseButtons { LeftButton, RightButton }

    public class InputHelper
    {
        public KeyboardState currentKeyboardState = new KeyboardState();
        public MouseState currentMouseState = new MouseState();
        public KeyboardState lastKeyboardState = new KeyboardState();
        public MouseState lastMouseState = new MouseState();
        public Vector2 cursorPos = new Vector2(0, 0);
        public GamePadState currentGamePadState = new GamePadState();
        public GamePadState lastGamePadState = new GamePadState();
        public Vector2 leftJoystickPosition = new Vector2(0, 0);
        public Vector2 rightJoystickPosition = new Vector2(0, 0);
        public Vector2 deadzone = new Vector2(0.1f, 0.1f); //amount of joystick movement noise to ignore

        public InputHelper() { }

        public void Update()
        {
            lastKeyboardState = currentKeyboardState;
            lastMouseState = currentMouseState;
            lastGamePadState = currentGamePadState;

            currentKeyboardState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            //track cursor position as int
            cursorPos.X = (int)currentMouseState.X;
            cursorPos.Y = (int)currentMouseState.Y;

            //track gamepad's joysticks
            leftJoystickPosition = currentGamePadState.ThumbSticks.Left;
            rightJoystickPosition = currentGamePadState.ThumbSticks.Right;

            //check to make sure joysticks exceed deadzone, else ignore joystick's movement
            if (Math.Abs(leftJoystickPosition.X) < deadzone.X) { leftJoystickPosition.X = 0; }
            if (Math.Abs(leftJoystickPosition.Y) < deadzone.Y) { leftJoystickPosition.Y = 0; }
            if (Math.Abs(rightJoystickPosition.X) < deadzone.X) { rightJoystickPosition.X = 0; }
            if (Math.Abs(rightJoystickPosition.Y) < deadzone.Y) { rightJoystickPosition.Y = 0; }
        }

        //check for keyboard key press, hold, and release
        public bool IsNewKeyPress(Keys key)
        {
            return (currentKeyboardState.IsKeyDown(key) &&
                lastKeyboardState.IsKeyUp(key));
        }

        public bool IsKeyDown(Keys key)
        { return (currentKeyboardState.IsKeyDown(key)); }

        public bool IsNewKeyRelease(Keys key)
        {
            return (lastKeyboardState.IsKeyDown(key) &&
                currentKeyboardState.IsKeyUp(key));
        }

        public bool IsNewMouseButtonPress(MouseButtons button)
        {   //check to see the mouse button was pressed
            if (button == MouseButtons.LeftButton)
            {
                return (currentMouseState.LeftButton == ButtonState.Pressed &&
                    lastMouseState.LeftButton == ButtonState.Released);
            }
            else if (button == MouseButtons.RightButton)
            {
                return (currentMouseState.RightButton == ButtonState.Pressed &&
                    lastMouseState.RightButton == ButtonState.Released);
            }
            else { return false; }
        }

        public bool IsNewMouseButtonRelease(MouseButtons button)
        {   //check to see the mouse button was released
            if (button == MouseButtons.LeftButton)
            {
                return (lastMouseState.LeftButton == ButtonState.Pressed &&
                    currentMouseState.LeftButton == ButtonState.Released);
            }
            else if (button == MouseButtons.RightButton)
            {
                return (lastMouseState.RightButton == ButtonState.Pressed &&
                    currentMouseState.RightButton == ButtonState.Released);
            }
            else { return false; }
        }

        public Boolean IsMouseButtonDown(MouseButtons button)
        {   //check to see if the mouse button is being held down
            if (button == MouseButtons.LeftButton)
            { return (currentMouseState.LeftButton == ButtonState.Pressed); }
            else if (button == MouseButtons.RightButton)
            { return (currentMouseState.RightButton == ButtonState.Pressed); }
            else { return false; }
        }

        public Vector2 GetDirection()
        {
            Vector2 d = Vector2.Zero;
            if (currentKeyboardState.IsKeyDown(Keys.Right))
                d += new Vector2(1, 0);
            if (currentKeyboardState.IsKeyDown(Keys.Left))
                d += new Vector2(-1, 0);
            if (currentKeyboardState.IsKeyDown(Keys.Up))
                d += new Vector2(0, -1);
            if (currentKeyboardState.IsKeyDown(Keys.Down))
                d += new Vector2(0, 1);
            d.Normalize();
            return d;
        }

        //check for gamepad button presses and releases
        public bool IsNewButtonPress(Buttons button)
        { return (currentGamePadState.IsButtonDown(button) && lastGamePadState.IsButtonUp(button)); }

        public bool IsButtonDown(Buttons button)
        { return currentGamePadState.IsButtonDown(button); }

        public bool IsNewButtonRelease(Buttons button)
        { return (lastGamePadState.IsButtonDown(button) && currentGamePadState.IsButtonUp(button)); }

    }
}
