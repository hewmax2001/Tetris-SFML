using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Tetris
{
    /// <summary>
    /// Game class representing a game of Tetris.
    /// </summary>
    public class Game
    {
        // Varaibles related to time
        private Clock Clock;
        private float delta;
        // Counts the time passed
        private float TimeCounter;
        // The speed of the pieces as they fall
        private float PieceSpeed;
        // Default starting speed is every 2 seconds
        private const float DEFAULT_SPEED = 2f;

        // Varaible to pause and denote a game over
        private bool Paused = false;
        private bool gameOver = false;
        public bool GameOver { 
            get { return gameOver; } 
            set 
            { 
                // Set pause to the same state as game over
                gameOver = value;
                Paused = value;
                // If gameOver, reset the game
                if (gameOver)
                    ResetGame();
            }
        }

        // Score varaibles
        private int Score;
        private int ScoreMultiplier;
        private const int DIFFICULTY_MULTIPLIER = 2;
        private const int SCORE_PER_ROW = 100;
        private const int ROW_MULTIPLIER = 2;

        // Grid and blocks varaibles
        private const int ROWS = 20;
        private const int COLUMNS = 10;
        private const uint BLOCK_SIZE_X = 30;
        private const uint BLOCK_SIZE_Y = 30;

        // Text varaibles
        private Font Font;
        private Text ScoreText;
        private Text ScoreValueText;
        private Text GameOverText;
        private Text GameOverSubText;

        private Text QueueNextText;
        private Text HoldText;
        // Hides the top 2 hidden rows of the Grid
        private RectangleShape HideBar;

        // The player's tetris piece
        private TetrisPiece playerPiece;
        public TetrisPiece PlayerPiece
        {
            get => playerPiece;
            private set
            {
                playerPiece = value;
            }
        }
        // Piece that the player can store and hold for later
        private TetrisPiece holdPiece;
        public TetrisPiece HoldPiece
        {
            get => holdPiece;
            private set
            {
                holdPiece = value;
            }
        }
        // Window where the game is drawn to
        public RenderWindow Window { get; }
        public TetrisGrid Grid { get; }
        private PieceQueue Queue { get; }

        private PieceGrid[] PieceGrids { get; }
        private PieceGrid HoldGrid { get; }

        /// <summary>
        /// Instantiates a new game with window.
        /// Creates:
        /// TetrisGrid
        /// Player Tetris piece
        /// Text
        /// Hidebar
        /// Time, Score and state varaibles
        /// </summary>
        public Game()
        { 
            // Set up the window
            VideoMode mode = new VideoMode(700, 700);
            Window = new RenderWindow(mode, "Tetris");
            setWindowEvents();

           

            // Default score multiplier
            ScoreMultiplier = 1;

            // Set up time varaibles
            Clock = new Clock();
            delta = 0f;
            TimeCounter = 0f;
            PieceSpeed = DEFAULT_SPEED;

            // Game starts not paused
            Paused = false;

            // Load textures
            TextureManager.LoadTextures();

            // Create grid with constants
            Vector2i pos = new Vector2i(0, 0);
            Grid = new TetrisGrid(ROWS, COLUMNS, BLOCK_SIZE_X, BLOCK_SIZE_Y, pos);

            // Set new grid positon based on its size
            // Center the grid to the middle of the window
            int yPosDecrement = 2 * (int)BLOCK_SIZE_Y;
            int xPos = (int)(Window.Size.X - Grid.size.X) / 2;
            int yPos = ((int)(Window.Size.Y - Grid.size.Y) / 2) - yPosDecrement;

            Grid.Position = new Vector2i(xPos, yPos);

            // Create HideBar and set it to cover the top two game over rows of the TetrisGrid
            HideBar = new RectangleShape();
            HideBar.Size = new Vector2f(Grid.size.X, 2 * BLOCK_SIZE_Y);
            HideBar.Position = new Vector2f(xPos, yPos);
            HideBar.FillColor = Color.Black;

            // Create new Queue with 4 piece objects
            Queue = new PieceQueue(4);

            // Create Player's piece and draw it into the grid
            PlayerPiece = new TPiece();
            SetPlayerPiece();

            PieceGrids = new PieceGrid[3];
            int startXPos = 50;
            int startYPos = 50;
            int spaceInBetween = 0;
            for (int i = 0; i < PieceGrids.Length; i++)
            {
                PieceGrids[i] = new PieceGrid(3, 4, 20, 20, new Vector2i(0, 0), Queue.Queue[i]);
                PieceGrids[i].Position = new Vector2i(startXPos, startYPos + (((int)PieceGrids[i].size.Y + spaceInBetween) * i));
            }

            HoldGrid = new PieceGrid(3, 4, 20, 20, new Vector2i(0, 0), HoldPiece);
            HoldGrid.Position = new Vector2i(startXPos, startYPos + 50 + (((int)PieceGrids[0].size.Y + spaceInBetween) * 3));

            // Set the text
            SetText();
        }

        /// <summary>
        /// Set the text used in the window
        /// </summary>
        private void SetText()
        {
            // Default font
            Font = new Font("C:/Windows/Fonts/arial.ttf");
            // Score: score
            Score = 0;
            ScoreText = new Text("Score:", Font);
            ScoreText.CharacterSize = 25;
            ScoreText.Position = new Vector2f(570, 100);

            ScoreValueText = new Text(Score.ToString(), Font);
            ScoreValueText.CharacterSize = 25;
            ScoreValueText.Position = new Vector2f(600, 150);

            // Game over text
            GameOverText = new Text("GAME OVER", Font);
            GameOverText.CharacterSize = 40;
            GameOverText.FillColor = Color.Red;
            // Center the Game Over text
            float textWidth = GameOverText.GetLocalBounds().Width;
            float textHeight = GameOverText.GetLocalBounds().Height;
            float xOffset = GameOverText.GetLocalBounds().Left;
            float yOffset = GameOverText.GetLocalBounds().Top;
            GameOverText.Origin = new Vector2f(textWidth / 2f + xOffset, textHeight / 2f + yOffset);
            GameOverText.Position = new Vector2f(Window.Size.X / 2f, Window.Size.Y / 2f);

            GameOverSubText = new Text();
            GameOverSubText.Font = Font;
            GameOverSubText.CharacterSize = 20;
            GameOverSubText.FillColor = Color.White;
            GameOverSubText.Position = GameOverText.Position + new Vector2f(0, 40);

            QueueNextText = new Text("Next", Font);
            QueueNextText.CharacterSize = 20;
            QueueNextText.Position = new Vector2f(PieceGrids[0].Position.X + 20, PieceGrids[0].Position.Y - 30);

            HoldText = new Text("Hold", Font);
            HoldText.CharacterSize = 20;
            HoldText.Position = new Vector2f(HoldGrid.Position.X + 20, HoldGrid.Position.Y - 30);

        }

        /// <summary>
        /// Return if player piece fits in grid
        /// </summary>
        /// <returns>PlayerPiece is in a legal Positon</returns>
        private bool PieceFits()
        {
            foreach (Position p in playerPiece.TilePositions())
            {
                if (!Grid.IsEmpty(p.Row, p.Column))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Rotate player piece ClockWise
        /// </summary>
        public void RotatePiece()
        {
            ClearPlayerPiece();
            PlayerPiece.RotateCW();

            if (!PieceFits())
            {
                //CorrectIllegalRotation();
                PlayerPiece.RotateCCW();
            }
            SetPlayerPiece();
        }

        /// <summary>
        /// Move Player piece left by one tile
        /// </summary>
        public void MovePieceLeft()
        {
            ClearPlayerPiece();
            PlayerPiece.Move(0, -1);

            if (!PieceFits())
            {
                playerPiece.Move(0, 1);
            }
            SetPlayerPiece();
        }

        /// <summary>
        /// Move player piece right by one tile
        /// </summary>
        public void MovePieceRight()
        {
            ClearPlayerPiece();
            PlayerPiece.Move(0, 1);

            if (!PieceFits())
            {
                playerPiece.Move(0, -1);
            }
            SetPlayerPiece();
        }

        public void HoldPlayerPiece()
        {
            ClearPlayerPiece();
            TetrisPiece tempPiece = playerPiece;
            if (HoldPiece == null)
                PlayerPiece = Queue.Deque();
            else
                PlayerPiece = HoldPiece;
            HoldPiece = tempPiece;
            PlayerPiece.Reset();
            HoldGrid.TetrisPiece = HoldPiece;
            Console.WriteLine(HoldPiece.BlockType);
        }

        /// <summary>
        /// Place the piece into the grid and add a new player piece
        /// </summary>
        private void PlacePiece()
        {
            // Map the blocks of Player's piece into the grid
            foreach (Position p in PlayerPiece.TilePositions())
            {
                Grid[p.Row, p.Column].Color = PlayerPiece.BlockType;
            }

            // Add score based on how many rows were cleared
            int addScore = CalculateScore(Grid.ClearFullRows());
            AddScore(addScore);

            // Check and set game over
            if (Grid.CheckGameOver())
            {
                GameOver = true;
            }

            // Set and reset player piece to the top of the grid
            PlayerPiece.Reset();
            PlayerPiece = Queue.Deque();
            UpdatePieceGrid();

        }

        private void UpdatePieceGrid()
        {
            for (int i = 0; i < PieceGrids.Length; i++)
            {
                PieceGrids[i].TetrisPiece = Queue.Queue[i];
            }
        }

        /// <summary>
        /// Move player piece down by one tile
        /// </summary>
        public void MovePieceDown()
        {
            ClearPlayerPiece();
            PlayerPiece.Move(1, 0);

            // If piece cannot go down any more, place the piece down
            if (!PieceFits())
            {
                PlayerPiece.Move(-1, 0);
                PlacePiece();
            }
            SetPlayerPiece();
        }

        /// <summary>
        /// Map the block tiles of the grid to the position of the blocks of the player's tetris piece
        /// </summary>
        private void SetPlayerPiece()
        {
            foreach (Position p in PlayerPiece.TilePositions())
            {
                Grid[p.Row, p.Column].Color = PlayerPiece.BlockType;
            }
        }

        /// <summary>
        /// Clear the mapped tiles of the player's piece
        /// </summary>
        private void ClearPlayerPiece()
        {
            foreach (Position p in PlayerPiece.TilePositions())
            {
                Grid[p.Row, p.Column].Color = BlockColor.EMPTY;
            }
        }

        /// <summary>
        /// Run the game
        /// </summary>
        public void Run()
        {
            while (Window.IsOpen)
            {
                Update();
                Draw();
            }
        }

        /// <summary>
        /// Update the game
        /// </summary>
        public void Update()
        {
            // Dispatch events and clear
            Window.DispatchEvents();
            Window.Clear();

            // If paused do not update
            if (Paused) { return; }

            // Add to counter till piece fall speed is met
            delta = Clock.Restart().AsSeconds();
            TimeCounter += delta;
            if (TimeCounter >= PieceSpeed)
            {
                // Reset counter + move piece down
                TimeCounter = 0f;
                MovePieceDown();
            }
           
        }

        /// <summary>
        /// Draw Tetris contents to the window
        /// </summary>
        public void Draw()
        {
            // Draw the game over screen if Game is over
            if (GameOver)
            {
                Window.Draw(GameOverText);
                Window.Draw(GameOverSubText);
                Window.Display();
            }

            // Do not draw is game is paused
            if (Paused) { return; }
            // Draw the tetris grid
            Grid.Draw(Window);
            // Draw the text of the game
            DrawText();
            // Draw the hide bar
            Window.Draw(HideBar);
            // Draw the Queue of tetris pieces
            foreach (PieceGrid p in PieceGrids)
            {
                p.Draw(Window);
            }
            HoldGrid.Draw(Window);
            // Dispaly the window
            Window.Display();
        }

        /// <summary>
        /// Draw the score text of the game
        /// </summary>
        private void DrawText()
        {
            Window.Draw(ScoreText);
            Window.Draw(ScoreValueText);
            Window.Draw(QueueNextText);
            Window.Draw(HoldText);
        }

        /// <summary>
        /// Close the window and exit the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Window_Closed(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            window.Close();
        }

        /// <summary>
        /// Execute events based on key pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Window_KeyPressed(object sender, KeyEventArgs e)
        {
            Window window = (Window)sender;
            // Close the window with Esc
            if (e.Code == Keyboard.Key.Escape)
            {
                window.Close();
            }

            // Pause / Unpause the game with P
            switch (e.Code)
            {
                case Keyboard.Key.P:
                    TogglePause();
                    break;
            }

            // Do not execute events if paused
            if (Paused) { return; }

            switch (e.Code)
            {
                case Keyboard.Key.Left:
                    MovePieceLeft();
                    break;
                case Keyboard.Key.Right:
                    MovePieceRight();
                    break;
                case Keyboard.Key.Down:
                    TimeCounter = 0f;
                    MovePieceDown();
                    break;
                case Keyboard.Key.Up:
                    RotatePiece();
                    break;
                case Keyboard.Key.C:
                    HoldPlayerPiece();
                    break;

            }
        }

        /// <summary>
        /// Executes events based on mouse button pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Mouse_ButtonPressed(object sender, MouseButtonEventArgs e)
        {
            // Do not execute events if paused
            if (Paused) { return; }

            switch (e.Button)
            {
                case Mouse.Button.Left:
                    TimeCounter = 0f;
                    MovePieceDown();
                    break;
            }
        }

        /// <summary>
        /// Add score to the score counter
        /// </summary>
        /// <param name="score"></param>
        private void AddScore(int score)
        {
            Score += (score * ScoreMultiplier);
            ScoreValueText.DisplayedString = Score.ToString();
        }

        /// <summary>
        /// Calculate score based on rows cleared
        /// </summary>
        /// <param name="rowsCleared"></param>
        /// <returns></returns>
        private int CalculateScore(int rowsCleared)
        {
            return (rowsCleared * ROW_MULTIPLIER) * SCORE_PER_ROW;
        }

        /// <summary>
        /// Increase the difficulty of the game by increasing Player's piece fall speed
        /// </summary>
        private void IncreaseDifficulty()
        {
            PieceSpeed /= 2;
            ScoreMultiplier *= DIFFICULTY_MULTIPLIER;
        }

        /// <summary>
        /// Reset the game
        /// </summary>
        private void ResetGame()
        {
            // Clear the grid
            Grid.Reset();
            // Fill the queue with new pieces
            Queue.Fill();
            // Reset playser piece to top of grid
            playerPiece.Reset();
            // Set Game Over text to show old score
            GameOverSubText.DisplayedString = "Score: " + Score.ToString();
            // Set new score text to show 0 score for a new game
            Score = 0;
            ScoreValueText.DisplayedString = Score.ToString();
        }

        /// <summary>
        /// Toggle the pause state of a game
        /// </summary>
        private void TogglePause()
        {
            // Unpausing the game also starts a new game
            if (Paused)
            {
                Paused = false;
                GameOver = false;
            }
            else
                Paused = true;
        }

        /// <summary>
        /// Executes events based on key released
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Window_KeyReleased(object sender, KeyEventArgs e)
        {
            Window window = (Window)sender;
        }

        /// <summary>
        /// Set the event listeners for the window
        /// </summary>
        private void setWindowEvents()
        {
            Window.Closed += Window_Closed;
            Window.KeyPressed += Window_KeyPressed;
            Window.KeyReleased += Window_KeyReleased;
            Window.MouseButtonPressed += Mouse_ButtonPressed;
        }
    }
}
