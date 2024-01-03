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
        private const int SegSize = SnakeSegment.RectSize;
    	private const int InitialGameSpeed = 160;

		public static int Height = 400;
		public static int Width = 400;
    	public static int GameSpeed = InitialGameSpeed;
		public readonly IPlayerInput playerInput;

		private GameState prevState;
		private IHighScoreRepository repo;
		private IEnumerable<HighScore> highScores;
		private Task loadTask;
		private ILogger<SnakeGame> logger;
    	private DateTimeOffset lastGameTick;

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

        public void Tick()
		{
			ReadPlayerInput();

			if (DateTimeOffset.Now - lastGameTick > TimeSpan.FromMilliseconds(GameSpeed))
			{
				Update();
				lastGameTick = DateTimeOffset.Now;
			}
		}

		public void Update()
		{
            if (GameState != GameState.InProgress)
			{
				return;
			}

            // check user input for next direction
            Snake.SetDirection(playerInput.GetPlayerDirection());

			// update snake, checking whether food was eaten
			if (!Snake.Update(Food))
			{
				GameState = GameState.GameOver;
			}
		}

		public IEnumerable<HighScore> GetHighScores()
		{
			if (this.loadTask == null)
			{
				this.loadTask = this.LoadHighScores();
			}
			return highScores;
		}

		public async Task SaveHighScore(HighScore score)
		{
			await this.repo.SaveHighScore(score);
		}

		private async Task LoadHighScores()
		{
			logger.LogInformation("Loading high scores...");
			this.highScores = await repo.GetHighScores();
			logger.LogInformation("Loaded high scores.");
		}

		public void GoHighScoresIfNeeded()
		{
            if (GameState == GameState.NotStarted || GameState == GameState.InProgress)
            {
				logger.LogInformation("Going to high scores");
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
