using Raylib_cs;

namespace MeuJogo.Draw;

public static class GameDraw
{
    public static void DrawScore(int score)
    {
        Raylib.DrawText(
            $"Score: {score}",
            10,
            10,
            30,
            Color.White
        );
    }
}