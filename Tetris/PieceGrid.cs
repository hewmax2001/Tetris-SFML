using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    /// <summary>
    /// Contains and displays a tetris piece
    /// </summary>
    public class PieceGrid
    {
        // The tetris piece to display
        private TetrisPiece tetrisPiece;
        public TetrisPiece TetrisPiece
        {
            get { return tetrisPiece; }
            set
            {
                // Set off set to top left of grid and map Tetris Piece's tiles to the grid
                tetrisPiece = value;
                tetrisPiece.Offset = new Position(0, 0);
                SetPiece();
            }
        }

        private readonly Block[,] grid;
        public int Rows { get; }
        public int Columns { get; }
        // Size of Blocks on the X axis
        public uint blockX { get; }
        // Size of Blocks on the Y axis
        private uint blockY { get; }
        // Position of the grid
        private Vector2i _position;
        public Vector2i Position
        {
            get => _position;
            set
            {
                // Changing the position of the grid changes the position of the Blocks
                Vector2i dif = value - _position;
                _position = value;
                foreach (Block block in grid)
                {
                    block.Position += new Vector2f(dif.X, dif.Y);
                }
            }
        }
        // Size of the grid in pixels
        public Vector2u size;

        // Set and get the grid array
        public Block this[int r, int c]
        {
            get => grid[r, c];
            set => grid[r, c] = value;
        }

        /// <summary>
        /// Instantiate a new PieceGrid with passed rows, columns size of blocks, position and initial tetris piece
        /// </summary>
        /// <param name="Rows"></param>
        /// <param name="Columns"></param>
        /// <param name="blockX"></param>
        /// <param name="blockY"></param>
        /// <param name="Position"></param>
        /// <param name="TetrisPiece"></param>
        public PieceGrid(int Rows, int Columns, uint blockX, uint blockY, Vector2i Position, TetrisPiece TetrisPiece)
        {
            // Set rows and block sizes
            this.Rows = Rows;
            this.Columns = Columns;
            this.blockX = blockX;
            this.blockY = blockY;
            // Create grid
            grid = new Block[Rows, Columns];
            // Fill grid with empty blocks
            FillGrid();
            // Set position and tetris piece
            this.Position = Position;
            if (TetrisPiece != null)
                this.TetrisPiece = TetrisPiece;
            // Set size of entire grid in pixels
            uint xSize = (uint)(Columns * blockX);
            uint ySize = (uint)(Rows * blockY);
            size = new Vector2u(xSize, ySize);
        }

        /// <summary>
        /// Map the TetrisPiece's blocks into the grid
        /// </summary>
        public void SetPiece()
        {
            // Clear the grid and make all blocks empty
            Reset();
            foreach (Position p in TetrisPiece.TilePositions())
            {
                grid[p.Row, p.Column].Color = TetrisPiece.BlockType;
            }
        }

        /// <summary>
        /// Draws the entire grid of blocks
        /// </summary>
        /// <param name="window"></param>
        public void Draw(RenderWindow window)
        {
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    DrawBlock(window, grid[r, c]);
                }
            }
        }

        /// <summary>
        /// Draws a given block
        /// </summary>
        /// <param name="window"></param>
        /// <param name="block"></param>
        private void DrawBlock(RenderWindow window, Block block)
        {
            block.Draw(window);
        }

        /// <summary>
        /// Fills the grid with empty blocks
        /// </summary>
        private void FillGrid()
        {
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    // Create new block object as EMPTY with defined height and size
                    Block block = new Block(BlockColor.EMPTY, new Vector2u(blockX, blockY));
                    // Set the position relative to its cell index and the position of the grid
                    block.Position = new Vector2f((c * blockX), (r * blockY));
                    grid[r, c] = block;
                }
            }
        }

        /// <summary>
        /// Clears the given row
        /// </summary>
        /// <param name="r"></param>
        private void ClearRow(int r)
        {
            for (int c = 0; c < Columns; c++)
            {
                grid[r, c].Color = BlockColor.EMPTY;
            }
        }

        /// <summary>
        /// Clears the grid of all non empty blocks
        /// </summary>
        public void Reset()
        {
            for (int r = 0; r < Rows; r++)
            {
                ClearRow(r);
            }
        }
    }
}
