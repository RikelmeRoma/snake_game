using Raylib_cs;
using System.Numerics;

namespace MeuJogo.Game;

public class Snake
{
    public List<Vector2> Body = new();

    public Vector2 Direction = new(1, 0);

    private int cellSize;

    public Snake(int cellSize)
    {
        this.cellSize = cellSize;

        Reset();
    }

    public void Reset()
    {
        Body.Clear();

        Body.Add(new Vector2(10, 10));
        Body.Add(new Vector2(9, 10));
        Body.Add(new Vector2(8, 10));

        Direction = new Vector2(1, 0);
    }

    public void Move(bool grow)
    {
        Vector2 newHead = Body[0] + Direction;

        Body.Insert(0, newHead);

        if (!grow)
        {
            Body.RemoveAt(Body.Count - 1);
        }
    }

    public void Draw()
    {
        foreach (var part in Body)
        {
            Raylib.DrawRectangle(
                (int)part.X * cellSize,
                (int)part.Y * cellSize,
                cellSize,
                cellSize,
                Color.Green
                
            );
        }
    }
}