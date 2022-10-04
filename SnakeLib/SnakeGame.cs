using System;
using System.Threading;
using System.Threading.Tasks;

namespace SnakeLib
{
	public class SnakeGame
	{
		public static int Height = 400;
		public static int Width = 400;
        private const int SegSize = SnakeSegment.RectSize;
		//private const int StartingDelay = 120;
		//private const double SpeedMultiplier = 1.5;

		public readonly PlayerInput playerInput;
		//private int delay = StartingDelay;

		public SnakeGame()
		{
			Food = new Food(4 * SegSize, 4 * SegSize);
			playerInput = new PlayerInput();
			Snake = BuildNewSnake();
		}

		public GameState GameState { get; private set; } = GameState.NotStarted;
		public Snake Snake { get; private set; }
		public Food Food { get; private set; }
		public int Score => Snake.Size;

		// Blazor webassembly does not support multithreading!
		//public void Run()
		//{
		//	Task.Run(() =>
		//	{
		//		while (true)
		//		{
		//			Update();
		//			Thread.Sleep(delay);
		//		}
		//	});
		//}

		private Snake BuildNewSnake()
		{
            var res = new Snake();
			int i = 0;
            for (; i < 2; i++)
            {
                res.Add((2 + i) * SegSize, 2 * SegSize);
            }
			i--;

            //for (int j = 0; j < 10; j++)
            //{
            //    res.Add((2 + i) * SegSize, (2 + j) * SegSize);
            //}
            return res;
        }

		public void Resize(int width, int height)
		{
			Width = width;
			Height = height;
		}

		public void ReadPlayerInput()
        {
            if (playerInput.GetEscapePressed())
            {
                PauseIfNeeded();
            }

            if (playerInput.GetEnterPressed())
            {
                GoInProgressIfNeeded();
            }
        }

        public void Update()
		{
            if (GameState != GameState.InProgress) return;
			//in-progress

            // check user input for next direction
            Snake.SetDirection(playerInput.GetPlayerDirection());

			// update snake, checking whether food was eaten
			if (!Snake.Update(Food))
			{
				GameState = GameState.GameOver;
			}

			//delay = StartingDelay - Math.Min(StartingDelay - 1, (int)(Snake.Size * SpeedMultiplier));
		}

		private void PauseIfNeeded()
		{
			if (GameState == GameState.InProgress)
			{
				GameState = GameState.Paused;
			}
			else if (GameState == GameState.Paused)
			{
				GameState = GameState.InProgress;
			}
		}

		private void GoInProgressIfNeeded()
		{
			if (GameState == GameState.InProgress)
			{
				return;
			}

			if (GameState == GameState.GameOver)
			{
				// reset the game

				Snake = BuildNewSnake();
                //delay = StartingDelay;
            }

			GameState = GameState.InProgress;
		}
	}
}
