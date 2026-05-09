using Raylib_cs;
using System.Numerics;


// CONFIGURAÇÕES DA TELA

const int tamanhoCelula = 20;
const int larguraTela = 800;
const int alturaTela = 600;

// CRIA A JANELA
Raylib.InitWindow(larguraTela, alturaTela, "Snake");

// FPS FIXO
Raylib.SetTargetFPS(60);


// Gera números aleatórios
Random random = new Random();

// Lista que representa o corpo da cobra
List<Vector2> cobra = new List<Vector2>();

// Direção atual da cobra
Vector2 direcao;

// Posição da comida
Vector2 comida;

// Controla se o jogador perdeu
bool gameOver = false;

// Controla a tela inicial
bool telaStart = true;


// FUNÇÃO PARA REINICIAR O JOGO

void ReiniciarJogo()
{
    cobra.Clear();

    // CRIA O CORPO INICIAL
    cobra.Add(new Vector2(10, 10));
    cobra.Add(new Vector2(9, 10));
    cobra.Add(new Vector2(8, 10));

    // DIREÇÃO INICIAL
    direcao = new Vector2(1, 0);

    // POSIÇÃO ALEATÓRIA DA COMIDA
    comida = new Vector2(
        random.Next(0, larguraTela / tamanhoCelula),
        random.Next(0, alturaTela / tamanhoCelula)
    );

    gameOver = false;
}

// INICIA O JOGO
ReiniciarJogo();


// CONTROLE DE TEMPO


// Faz a cobra andar em intervalos
float tempoMovimento = 0;

// Velocidade da cobra
float intervalo = .15f;


// LOOP PRINCIPAL

while (!Raylib.WindowShouldClose())
{
    // TEMPO ENTRE FRAMES
    float delta = Raylib.GetFrameTime();

    tempoMovimento += delta;

   
    // TELA INICIAL
    
    if (telaStart)
    {
        // Aperte enter para começar
        if (Raylib.IsKeyPressed(KeyboardKey.Enter))
        {
            telaStart = false;
        }
    }
    else if (!gameOver)
    {
    
        // INPUT DO TECLADO
        // Impede a cobra de voltar para trás

        if (Raylib.IsKeyPressed(KeyboardKey.Right) && direcao.X != -1)
            direcao = new Vector2(1, 0);

        if (Raylib.IsKeyPressed(KeyboardKey.Left) && direcao.X != 1)
            direcao = new Vector2(-1, 0);

        if (Raylib.IsKeyPressed(KeyboardKey.Up) && direcao.Y != 1)
            direcao = new Vector2(0, -1);

        if (Raylib.IsKeyPressed(KeyboardKey.Down) && direcao.Y != -1)
            direcao = new Vector2(0, 1);

        
        // MOVIMENTO DA COBRA
        if (tempoMovimento >= intervalo)
        {
            tempoMovimento = 0;

            // NOVA POSIÇÃO DA CABEÇA
            Vector2 novaCabeca = cobra[0] + direcao;

          
            // COLISÃO COM PAREDE
    
            if (
                novaCabeca.X < 0 ||
                novaCabeca.Y < 0 ||
                novaCabeca.X >= larguraTela / tamanhoCelula ||
                novaCabeca.Y >= alturaTela / tamanhoCelula
            )
            {
                gameOver = true;
            }

         
            // COLISÃO COM O PRÓPRIO CORPO
  
            foreach (var parte in cobra)
            {
                if (parte == novaCabeca)
                {
                    gameOver = true;
                }
            }

            // ADICIONA NOVA CABEÇA
            cobra.Insert(0, novaCabeca);

      
            // VERIFICA SE COME A COMIDA
    
            if (novaCabeca == comida)
            {
                // GERA NOVA COMIDA
                comida = new Vector2(
                    random.Next(0, larguraTela / tamanhoCelula),
                    random.Next(0, alturaTela / tamanhoCelula)
                );
            }
            else
            {
                // REMOVE A ÚLTIMA PARTE DO CORPO
                cobra.RemoveAt(cobra.Count - 1);
            }
        }
    }


    // BOTÃO DE REINICIAR


    Rectangle botao = new Rectangle(300, 320, 200, 60);

    Vector2 mouse = Raylib.GetMousePosition();

    // VERIFICA SE O MOUSE ESTÁ NO BOTÃO
    bool mouseNoBotao = Raylib.CheckCollisionPointRec(mouse, botao);

    // REINICIA AO CLICAR
    if (gameOver && mouseNoBotao && Raylib.IsMouseButtonPressed(MouseButton.Left))
    {
        ReiniciarJogo();
    }


    // DESENHO NA TELA

    Raylib.BeginDrawing();

    Raylib.ClearBackground(Color.Black);

    // TELA START

    if (telaStart)
    {
        Raylib.DrawText("SNAKE GAME", 230, 180, 60, Color.Green);

        Raylib.DrawText(
            "APERTE ENTER PARA JOGAR",
            180,
            300,
            30,
            Color.White
        );
    }
    else
    {

        // DESENHA A COBRA
     
        foreach (var parte in cobra)
        {
            Raylib.DrawRectangle(
                (int)parte.X * tamanhoCelula,
                (int)parte.Y * tamanhoCelula,
                tamanhoCelula,
                tamanhoCelula,
                Color.Green
            );
        }

     
        // DESENHA A COMIDA
      
        Raylib.DrawRectangle(
            (int)comida.X * tamanhoCelula,
            (int)comida.Y * tamanhoCelula,
            tamanhoCelula,
            tamanhoCelula,
            Color.Red
        );

  
        // TELA DE GAME OVER
      
        if (gameOver)
        {
            Raylib.DrawText("GAME OVER", 260, 200, 50, Color.White);

            // BOTÃO
            Raylib.DrawRectangleRec(
                botao,
                mouseNoBotao ? Color.DarkGreen : Color.Green
            );

            Raylib.DrawText("REINICIAR", 330, 340, 25, Color.White);
        }
    }

    Raylib.EndDrawing();
}

// FECHA A JANELA
Raylib.CloseWindow();