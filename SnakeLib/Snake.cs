using Blazor.Extensions.Canvas.Canvas2D;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SnakeLib
{
	public class Snake : ICanvasDrawable
	{
		private const int SegSize = SnakeSegment.RectSize;
		private LinkedList<SnakeSegment> parts = new LinkedList<SnakeSegment>();
		private Direction Direction = Direction.Left;
		private Random random = new Random();
		private ISet<(int, int)> snakeBody = new HashSet<(int, int)>();

		public int Size { get; private set; }

		public void SetDirection(Direction dir)
		{
			if (Size == 1 || dir != OppositeDirection(Direction))
			{
				Direction = dir;
			}
		}

		private Direction OppositeDirection(Direction d)
		{
			switch (d)
			{
				case Direction.Up:
					return Direction.Down;
				case Direction.Down:
					return Direction.Up;
				case Direction.Left:
					return Direction.Right;
				case Direction.Right:
					return Direction.Left;
				default:
					throw new Exception("unspoorted dir");
			}
		}

		public void Add(int x, int y)
		{
			snakeBody.Add((x, y));
			parts.AddFirst(new SnakeSegment(x, y));
			Size++;
		}

		public void RemoveTail()
		{
			var seg = parts.Last.ValueRef;
			snakeBody.Remove((seg.x, seg.y));
			parts.RemoveLast();
			Size--;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="food"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public bool Update(Food food)
		{
			// check if snake will consume food
			// add snake head, remove old tail (unless sized increased due to consuming food)
			bool match = false;
			int nextX = parts.First.ValueRef.x;
			int nextY = parts.First.ValueRef.y;

			switch (Direction)
			{
				case Direction.Right:
                    nextX = parts.First.ValueRef.x + SegSize;
                    if (nextX >= SnakeGame.Width)
					{
						nextX = 0;
					}

					break;
				case Direction.Left:
                    nextX = parts.First.ValueRef.x - SegSize;
                    if (nextX < 0)
					{
						nextX = (SnakeGame.Width / SegSize) * SegSize;
					}

					break;
				case Direction.Up:

                    nextY = parts.First.ValueRef.y - SegSize;
                    if (nextY < 0)
					{
						nextY = (SnakeGame.Height / SegSize) * SegSize;
                    }
					break;
				case Direction.Down:

                    nextY = parts.First.ValueRef.y + SegSize;
                    if (nextY >= SnakeGame.Height)
					{
						nextY = 0;
					}

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
