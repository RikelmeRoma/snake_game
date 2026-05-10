using Raylib_cs;
using System.Numerics;

namespace MeuJogo.Game;

public class Food
{
    public Vector2 Position;

    private int cellSize;

    public Food(int cellSize)
    {
        this.cellSize = cellSize;
    }

    public void Generate(Random random, int width, int height)
    {
        Position = new Vector2(
            random.Next(0, width / cellSize),
            random.Next(0, height / cellSize)
        );
    }

    public void Draw()
    {
        Raylib.DrawRectangle(
            (int)Position.X * cellSize,
            (int)Position.Y * cellSize,
            cellSize,
            cellSize,
            Color.Red
        );
    }
}