using Raylib_cs;
using System.Numerics;

namespace MeuJogo.Game;

public class Food
{
    // Posição da comida no mapa
    public Vector2 Position;

    // Tamanho de cada célula do jogo
    private int cellSize;

    public Food(int cellSize)
    {
        this.cellSize = cellSize;
    }

    // Gera uma posição aleatória para a comida
    public void Generate(Random random, int width, int height)
    {
        Position = new Vector2(
            random.Next(0, width / cellSize),
            random.Next(0, height / cellSize)
        );
    }

    // Desenha a comida na tela
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