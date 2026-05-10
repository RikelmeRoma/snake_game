using Raylib_cs;
using System.Numerics;

namespace MeuJogo.Game;

public class GameManager
{
    private const int screenWidth = 800;
    private const int screenHeight = 600;
    private const int cellSize = 20;

    private Snake snake;
    private Food food;

    private Random random = new();

    private GameState state = GameState.Start;

    private float moveTimer = 0;
    private float moveDelay = 0.15f;

    public GameManager()
    {
        Raylib.InitWindow(screenWidth, screenHeight, "Snake");

        Raylib.SetTargetFPS(60);

        snake = new Snake(cellSize);

        food = new Food(cellSize);

        food.Generate(random, screenWidth, screenHeight);
    }

    public void Run()
    {
        while (!Raylib.WindowShouldClose())
        {
            Update();

            Draw();
        }

        Raylib.CloseWindow();
    }

    private void Update()
    {
        float delta = Raylib.GetFrameTime();

        moveTimer += delta;

        switch (state)
        {
            case GameState.Start:
                UpdateStart();
                break;

            case GameState.Playing:
                UpdateGame();
                break;

            case GameState.GameOver:
                UpdateGameOver();
                break;
        }
    }

    private void UpdateStart()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Enter))
        {
            state = GameState.Playing;
        }
    }

    private void UpdateGame()
    {
        HandleInput();

        if (moveTimer >= moveDelay)
        {
            moveTimer = 0;

            Vector2 newHead = snake.Body[0] + snake.Direction;

            bool grow = newHead == food.Position;

            snake.Move(grow);

            if (grow)
            {
                food.Generate(random, screenWidth, screenHeight);
            }

            CheckCollision();
        }
    }

    private void UpdateGameOver()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Enter))
        {
            snake.Reset();

            food.Generate(random, screenWidth, screenHeight);

            state = GameState.Playing;
        }
    }

    private void HandleInput()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Right) && snake.Direction.X != -1)
            snake.Direction = new Vector2(1, 0);

        if (Raylib.IsKeyPressed(KeyboardKey.Left) && snake.Direction.X != 1)
            snake.Direction = new Vector2(-1, 0);

        if (Raylib.IsKeyPressed(KeyboardKey.Up) && snake.Direction.Y != 1)
            snake.Direction = new Vector2(0, -1);

        if (Raylib.IsKeyPressed(KeyboardKey.Down) && snake.Direction.Y != -1)
            snake.Direction = new Vector2(0, 1);
    }

    private void CheckCollision()
    {
        Vector2 head = snake.Body[0];

        // PAREDE
        if (
            head.X < 0 ||
            head.Y < 0 ||
            head.X >= screenWidth / cellSize ||
            head.Y >= screenHeight / cellSize
        )
        {
            state = GameState.GameOver;
        }

        // CORPO
        for (int i = 1; i < snake.Body.Count; i++)
        {
            if (snake.Body[i] == head)
            {
                state = GameState.GameOver;
            }
        }
    }

    private void Draw()
    {
        Raylib.BeginDrawing();

        Raylib.ClearBackground(Color.Black);

        switch (state)
        {
            case GameState.Start:
                DrawStart();
                break;

            case GameState.Playing:
                DrawGame();
                break;

            case GameState.GameOver:
                DrawGameOver();
                break;
        }

        Raylib.EndDrawing();
    }

    private void DrawStart()
    {
        Raylib.DrawText("SNAKE GAME", 220, 200, 60, Color.Green);

        Raylib.DrawText(
            "APERTE ENTER",
            270,
            320,
            30,
            Color.White
        );
    }

    private void DrawGame()
    {
        snake.Draw();

        food.Draw();
    }

    private void DrawGameOver()
    {
        DrawGame();

        Raylib.DrawText("GAME OVER", 240, 250, 60, Color.White);

        Raylib.DrawText(
            "ENTER PARA REINICIAR",
            180,
            340,
            30,
            Color.White
        );
    }
}