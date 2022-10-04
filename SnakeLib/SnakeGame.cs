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
            res.Add(10 * SegSize, 10 * SegSize);
            return res;
        }

		public void Resize(int width, int height)
		{
			Width = width;
			Height = height;
		}

		public void ReadPlayerInput()
        {
			if (GameState == GameState.NotStarted)
			{
				if (playerInput.GetEscapePressed() || playerInput.GetEnterPressed())
				{
					GoInProgressIfNeeded();
				}
				return;
			}

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
			else if (GameState == GameState.GameOver)
			{
				GameState = GameState.NotStarted;
				Snake = BuildNewSnake();
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
				GameState = GameState.NotStarted;
				Snake = BuildNewSnake();
				//delay = StartingDelay;
				return;
            }

			GameState = GameState.InProgress;
		}
	}
}
