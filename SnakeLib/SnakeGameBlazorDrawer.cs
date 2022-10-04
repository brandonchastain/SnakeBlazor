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
            await context.SetFontAsync("bold 48px Helvetica");
            await context.FillTextAsync("B SNAKE", Width - 300, Height - 400);

            await context.SetFontAsync("bold 24px Helvetica");
            await context.FillTextAsync("[CLICK / TAP] START", Width - 325, Height - 300);
        }

		private async ValueTask DrawGameInProgress(Canvas2DContext context)
		{
			await context.SetFillStyleAsync("black");
			await context.FillRectAsync(0, 0, SnakeGame.Width, SnakeGame.Height);
			await context.SetStrokeStyleAsync("white");
			await context.StrokeRectAsync(1, 1, Width - 1, Height - 1);
			await game.Snake.Draw(context);
			await game.Food.Draw(context);
			await context.SetFontAsync("bold 48px Helvetica");
			await context.FillTextAsync($"SCORE {game.Snake.Size}", Width - 300, Height - 100);
		}

		private async ValueTask DrawPaused(Canvas2DContext context)
		{
			await context.SetFillStyleAsync("black");
			await context.FillRectAsync(0, 0, Width, Height);
			await context.SetFillStyleAsync("white");
            await context.SetFontAsync("bold 48px Helvetica");
            await context.FillTextAsync("PAUSED", Width - 300, Height - 100);
		}

		private async ValueTask DrawGameOver(Canvas2DContext context)
		{
			await context.SetFillStyleAsync("black");
			await context.FillRectAsync(0, 0, Width, Height);
			await context.SetFillStyleAsync("white");
            //await context.SetFontAsync("bold 48px Helvetica");
            await context.FillTextAsync("GAME OVER", Width - 350, Height - 100);
		}
	}
}
