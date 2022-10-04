using Blazor.Extensions.Canvas.Canvas2D;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SnakeLib
{

	public class SnakeGameBlazorDrawer : ICanvasDrawable
	{
		private static int	Width = SnakeGame.Width;
		private static int Height = SnakeGame.Height;

		private SnakeGame game;

		public SnakeGameBlazorDrawer(SnakeGame g)
		{
			game = g;
		}

		public async ValueTask Draw(Canvas2DContext context)
		{
			if (context == null)
			{
				return;
			}
			Debug.WriteLine("drawing");
			switch (game.GameState)
			{
				case GameState.NotStarted:
					await DrawTitle(context);
					break;
				case GameState.InProgress:
					await DrawGameInProgress(context);
					break;
				case GameState.Paused:
					await DrawPaused(context);
					break;
				case GameState.GameOver:
					await DrawGameOver(context);
					break;
				default:
					throw new Exception("Unrecognized game state");
			}
		}

		private async ValueTask DrawTitle(Canvas2DContext context)
		{
			await context.SetFillStyleAsync("black");
			await context.FillRectAsync(0, 0, Width, Height);
			await context.SetFillStyleAsync("white");
			await context.FillTextAsync("B-Snake.\nEnter to start.\nESC to pause.", Width / 2, Height / 2);
		}

		private async ValueTask DrawGameInProgress(Canvas2DContext context)
		{
			await context.SetFillStyleAsync("black");
			await context.FillRectAsync(0, 0, SnakeGame.Width, SnakeGame.Height);
			await context.SetStrokeStyleAsync("white");
			await context.StrokeRectAsync(1, 1, Width - 1, Height - 1);
			await game.Snake.Draw(context);
			await game.Food.Draw(context);
			await context.FillTextAsync($"Score: {game.Snake.Size}", 350, 350);
		}

		private async ValueTask DrawPaused(Canvas2DContext context)
		{
			await context.SetFillStyleAsync("black");
			await context.FillRectAsync(0, 0, Width, Height);
			await context.SetFillStyleAsync("white");
			await context.FillTextAsync("B-Snake.\nPAUSED", Width / 2, Height / 2);
		}

		private async ValueTask DrawGameOver(Canvas2DContext context)
		{
			await context.SetFillStyleAsync("black");
			await context.FillRectAsync(0, 0, Width, Height);
			await context.SetFillStyleAsync("white");
			await context.FillTextAsync("B-Snake.\nGAME OVER", Width / 2, Height / 2);
		}
	}
}
