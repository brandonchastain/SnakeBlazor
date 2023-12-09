using System;
namespace SnakeLib
{
    public class PlayerInput
    {
        private Direction currentDirection;

        public PlayerInput()
        {
            currentDirection = Direction.Down;
        }

        public bool EscapePressed;
        public bool EnterPressed;
        public bool HPressed;

        public void SetEnterPressed()
        {
            this.EnterPressed = true;
        }
        public void SetEscapePressed()
        {
            this.EscapePressed = true;
        }

        public bool GetEscapePressed()
        {
            var old = EscapePressed;
            EscapePressed = false;
            return old;
        }

        public bool GetEnterPressed()
        {
            var oldVal = EnterPressed;
            EnterPressed = false;
            return oldVal;
        }

        public bool GetHPressed()
        {
            var oldVal = HPressed;
            HPressed = false;
            return oldVal;
        }

        public void OnKeyDown(string keyCode)
        {
            switch (keyCode)
            {
                case "Enter":
                    EnterPressed = true;
                    break;
                case "Escape":
                    EscapePressed = true;
                    break;
                case "ArrowLeft":
                    currentDirection = Direction.Left;
                    break;
                case "ArrowRight":
                    currentDirection = Direction.Right;
                    break;
                case "ArrowUp":
                    currentDirection = Direction.Up;
                    break;
                case "ArrowDown":
                    currentDirection = Direction.Down;
                    break;
                case "KeyH":
                    HPressed = true;
                    break;

                default:
                    throw new Exception(keyCode);
            }
        }

        public Direction GetPlayerDirection()
        {
            return currentDirection;
        }
    }
}
