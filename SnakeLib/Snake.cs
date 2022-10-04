using Blazor.Extensions.Canvas.Canvas2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace SnakeLib
{
	public class Snake : ICanvasDrawable
	{
		private const int SegSize = SnakeSegment.RectSize;
		private object lockobject = new object();
		private IList<SnakeSegment> parts = new List<SnakeSegment>();
		private Direction Direction = Direction.Left;
		private Random random = new Random();
		private ISet<(int, int)> snakeBody = new HashSet<(int, int)>();

		public int Size { get; private set; }

		public void SetDirection(Direction dir)
		{
			Direction = dir;
		}

		public void Add(int x, int y)
		{
			snakeBody.Add((x, y));
			parts.Insert(0, new SnakeSegment(x, y));
			Size++;
		}

		public void RemoveTail()
		{
			var seg = parts[Size - 1];
			snakeBody.Remove((seg.x, seg.y));
			parts.RemoveAt(Size - 1);
			Size--;
		}

		public bool Update(Food food)
		{
			// check if snake will consume food
			// add snake head, remove old tail (unless sized increased due to consuming food)
			bool match = false;
			int nextX = parts[0].x;
			int nextY = parts[0].y;

			switch (Direction)
			{
				case Direction.Right:
					if (parts[0].x + SegSize >= SnakeGame.Width)
					{
						return false;
					}

					nextX = parts[0].x + SegSize;
					break;
				case Direction.Left:
					if (parts[0].x - SegSize < 0)
					{
						return false;
					}

					nextX = parts[0].x - SegSize;
					break;
				case Direction.Up:
					if (parts[0].y - SegSize < 0)
					{
						return false;
					}

					nextY = parts[0].y - SegSize;
					break;
				case Direction.Down:
					if (parts[0].y + SegSize >= SnakeGame.Height)
					{
						return false;
					}

					nextY = parts[0].y + SegSize;
					break;
				default:
					throw new Exception("unrecognized direction");
			}


			if (nextY == food.y && nextX == food.x)
			{
				match = true;
				food.x = SegSize + SegSize * random.Next((SnakeGame.Width - SegSize) / SegSize);
				food.y = SegSize + SegSize * random.Next((SnakeGame.Height - SegSize) / SegSize);
			}

			if (snakeBody.Contains((nextX, nextY)))
			{
				return false;
			}

			Add(nextX, nextY);

			if (!match)
			{
				RemoveTail();
			}

			return true;
		}

		public async ValueTask Draw(Canvas2DContext context)
		{
			foreach (ICanvasDrawable snakeSeg in parts)
			{
				await snakeSeg.Draw(context);
			}
		}
	}
}
