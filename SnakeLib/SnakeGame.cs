using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SnakeLib.Contracts;
using SnakeLib.Data;

namespace SnakeLib
{
	public class SnakeGame
	{
		public static int Height = 400;
		public static int Width = 400;
        private const int SegSize = SnakeSegment.RectSize;

		public readonly IPlayerInput playerInput;

		private GameState prevState;


		private IHighScoreRepository repo;
		private IEnumerable<HighScore> highScores;
		private Task loadTask;
		private ILogger<SnakeGame> logger;

		public SnakeGame(ILogger<SnakeGame> logger, IHighScoreRepository highScoreRepository, IPlayerInput playerInput = null)
		{
			Food = new Food(4 * SegSize, 4 * SegSize);
			this.playerInput = playerInput ?? new PlayerInput();
			this.repo = highScoreRepository;
			Snake = BuildNewSnake();
			highScores = new List<HighScore>();
			this.logger = logger;
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
				else if (playerInput.GetHPressed())
				{
					GoHighScoresIfNeeded();
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

			if (playerInput.GetHPressed())
			{
				GoHighScoresIfNeeded();
			}
        }

        public void Update()
		{
			if (GameState == GameState.Initializing)
			{
				Console.WriteLine("Init");
				this.logger.LogInformation("Initializing");
				this.loadTask = this.LoadHighScores();
				this.logger.LogInformation("load task started.");
				GameState = GameState.InProgress;
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

			//delay = StartingDelay - Math.Min(StartingDelay - 1, (int)(Snake.Size * SpeedMultiplier));
		}

		public IEnumerable<HighScore> GetHighScores()
		{
			return highScores;
		}

		private async Task LoadHighScores()
		{
			this.highScores = await repo.GetHighScores();
		}

		private void GoHighScoresIfNeeded()
		{
            if (GameState == GameState.NotStarted || GameState == GameState.InProgress)
            {
				prevState = GameState;
                GameState = GameState.HighScores;
            }
			else if (GameState == GameState.HighScores)
			{
				GameState = prevState;
			}
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
			if (GameState == GameState.NotStarted)
			{
				GameState = GameState.Initializing;
				return;
			}

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
