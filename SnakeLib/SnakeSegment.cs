using Blazor.Extensions.Canvas.Canvas2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeLib
{
	public class SnakeSegment : TileObject, ICanvasDrawable
	{
		public const int RectSize = 20;

		public SnakeSegment(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public int x { get; private set; }
		public int y { get; private set; }

		public async ValueTask Draw(Canvas2DContext context)
		{
			await context.SetFillStyleAsync("purple");
			await context.FillRectAsync(x, y, RectSize, RectSize);
		}
	}
}
