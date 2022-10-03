using Blazor.Extensions.Canvas.Canvas2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeLib
{
	public interface ICanvasDrawable
	{
		public ValueTask Draw(Canvas2DContext _context);
	}
}
