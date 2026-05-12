using Raylib_cs;
using System.Numerics;

namespace MeuJogo.Game;

public class GameManager
{
    // Configurações da janela e do grid
    private const int screenWidth = 800;
    private const int screenHeight = 600;
    private const int cellSize = 20;

    // Objetos principais do jogo
    private Snake snake;
    private Food food;
    private Random random = new();

    // Estado atual do jogo
    private GameState state = GameState.Start;

    // Controle da velocidade da cobra
    private float moveTimer = 0;
    private float moveDelay = 0.15f;

    // Pontuação
    private int score = 0;

    public GameManager()
    {
        Raylib.InitWindow(screenWidth, screenHeight, "Snake");

        Raylib.SetTargetFPS(60);

        snake = new Snake(cellSize);

        food = new Food(cellSize);

        food.Generate(random, screenWidth, screenHeight);
    }

    // Loop principal do jogo
    public void Run()
    {
        while (!Raylib.WindowShouldClose())
        {
            Update();

            Draw();
        }

        Raylib.CloseWindow();
    }

    // Atualiza lógica do jogo
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

    // Tela inicial
    private void UpdateStart()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Enter))
        {
            state = GameState.Playing;
        }
    }

    // Atualiza gameplay
    private void UpdateGame()
    {
        HandleInput();

        // Move a cobra baseado no tempo
        if (moveTimer >= moveDelay)
        {
            moveTimer = 0;

            Vector2 newHead = snake.Body[0] + snake.Direction;

            // Verifica se comeu a comida
            bool grow = newHead == food.Position;

            snake.Move(grow);

            if (grow)
            {
                score++;

                // Aumenta velocidade
                moveDelay -= 0.005f;

                // Limite mínimo de velocidade
                if (moveDelay < 0.05f)
                {
                    moveDelay = 0.05f;
                }

                food.Generate(random, screenWidth, screenHeight);
            }

            CheckCollision();
        }
    }

    // Tela de game over
    private void UpdateGameOver()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Enter))
        {
            snake.Reset();

            food.Generate(random, screenWidth, screenHeight);

            // Reseta valores
            score = 0;
            moveDelay = 0.15f;

            state = GameState.Playing;
        }
    }

    // Controle do teclado
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

    // Verifica colisões
    private void CheckCollision()
    {
        Vector2 head = snake.Body[0];

        // Colisão com parede
        if (
            head.X < 0 ||
            head.Y < 0 ||
            head.X >= screenWidth / cellSize ||
            head.Y >= screenHeight / cellSize
        )
        {
            state = GameState.GameOver;
        }

        // Colisão com o próprio corpo
        for (int i = 1; i < snake.Body.Count; i++)
        {
            if (snake.Body[i] == head)
            {
                state = GameState.GameOver;
            }
        }
    }

    // Renderização
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

    // Desenha tela inicial
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

    // Desenha gameplay
    private void DrawGame()
    {
        DrawGrid();

        // Score do jogador
        Raylib.DrawText(
            $"Score: {score}",
            10,
            10,
            30,
            Color.White
        );

        snake.Draw();

        food.Draw();
    }

    // Tela de derrota
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

    // Desenha a grade do mapa
    private void DrawGrid()
    {
        // Linhas verticais
        for (int x = 0; x < screenWidth; x += cellSize)
        {
            Raylib.DrawLine(
                x,
                0,
                x,
                screenHeight,
                Color.DarkGray
            );
        }

        // Linhas horizontais
        for (int y = 0; y < screenHeight; y += cellSize)
        {
            Raylib.DrawLine(
                0,
                y,
                screenWidth,
                y,
                Color.DarkGray
            );
        }
    }
}