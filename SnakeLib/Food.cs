using Blazor.Extensions.Canvas.Canvas2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeLib
{
    public class Food : TileObject, ICanvasDrawable
    {
        private const int RectSize = SnakeSegment.RectSize;

        public Food(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int x { get; set; }
        public int y { get; set; }

        public async ValueTask Draw(Canvas2DContext context)
        {
            await context.SetFillStyleAsync("green");
            await context.FillRectAsync(x, y, RectSize, RectSize);
        }
    }
}
