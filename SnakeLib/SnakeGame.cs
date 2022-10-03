using System;
using System.Threading;
using System.Threading.Tasks;

namespace SnakeLib
{
	public class SnakeGame
	{
		public static int Size = 400;
		private const int SegSize = SnakeSegment.RectSize;
		private const int StartingDelay = 120;
		private const double SpeedMultiplier = 1.5;

		public readonly PlayerInput playerInput;
		private int delay = StartingDelay;

		public SnakeGame()
		{
			Food = new Food(4 * SegSize, 4 * SegSize);
			playerInput = new PlayerInput();
			Snake = new Snake();
			Snake.Add(15 * SegSize, 15 * SegSize);
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

		public void Resize(int size)
		{
			Size = size;
		}

		public void Update()
		{
			if (playerInput.GetEscapePressed())
            {
                PauseIfNeeded();
            }

			if (playerInput.GetEnterPressed())
			{
				GoInProgressIfNeeded();
            }

            if (GameState != GameState.InProgress) return;
			//in-progress

            // check user input for next direction
            Snake.SetDirection(playerInput.GetPlayerDirection());

			// update snake, checking whether food was eaten
			if (!Snake.Update(Food))
			{
				GameState = GameState.GameOver;
			}

			delay = StartingDelay - Math.Min(StartingDelay - 1, (int)(Snake.Size * SpeedMultiplier));
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

				Snake = new Snake();
				Snake.Add(10 * SnakeSegment.RectSize, 10 * SnakeSegment.RectSize);
				delay = StartingDelay;
			}

			GameState = GameState.InProgress;
		}
	}
}
