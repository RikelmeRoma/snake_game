using Raylib_cs;
using System.Numerics;

namespace MeuJogo.Game;

public class Snake
{
    // Lista que guarda todas as partes da cobra
    public List<Vector2> Body = new();

    // Direção atual da cobra
    public Vector2 Direction = new(1, 0);

    // Tamanho de cada célula
    private int cellSize;

    public Snake(int cellSize)
    {
        this.cellSize = cellSize;

        Reset();
    }

    // Reinicia a cobra
    public void Reset()
    {
        Body.Clear();

        // Posição inicial da cobra
        Body.Add(new Vector2(10, 10));
        Body.Add(new Vector2(9, 10));
        Body.Add(new Vector2(8, 10));

        Direction = new Vector2(1, 0);
    }

    // Move a cobra
    public void Move(bool grow)
    {
        // Nova posição da cabeça
        Vector2 newHead = Body[0] + Direction;

        // Adiciona a nova cabeça
        Body.Insert(0, newHead);

        // Remove a cauda caso não cresça
        if (!grow)
        {
            Body.RemoveAt(Body.Count - 1);
        }
    }

    // Desenha a cobra
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