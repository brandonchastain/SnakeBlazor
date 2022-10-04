﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeLib
{
    public class PlayerInput
    {
        private Direction currentDirection;

        public PlayerInput()
        {
            currentDirection = Direction.Down;

            //Task.Run(() => hook.Run()); // multithreading not supported by blazor webassembly
        }

        //public delegate void Notify();
        public bool EscapePressed;
        public bool EnterPressed;

        public void SetEnterPressed()
        {
            this.EnterPressed = true;
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