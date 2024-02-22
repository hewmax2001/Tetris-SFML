using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
   /// <summary>
   /// Grid to house the Tetris Blocks
   /// </summary>
    public class TetrisGrid
    {
        // Two dimensional array of tetris blocks
        private readonly Block[,] grid;
        public int Rows { get; }
        public int Columns { get; }
        // Size of Blocks on the X axis
        public uint blockX { get; }
        // Size of Blocks on the Y axis
        private uint blockY { get; }
        // Position of the grid
        private Vector2i _position;
        public Vector2i Position { 
            get => _position; 
            set {
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
        // Extra rows afforded to game over condition
        private const int GAME_OVER_ROWS = 2;

        // Set and get the grid array
        public Block this[int r, int c]
        {
            get => grid[r, c];
            set => grid[r, c] = value;
        }

        /// <summary>
        /// Instantiate a new TetrisGrid with rows, columns, width and height of the blocks as well as the position of the grid.
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        /// <param name="blockX"></param>
        /// <param name="blockY"></param>
        /// <param name="Position"></param>
        public TetrisGrid(int rows, int columns, uint blockX, uint blockY, Vector2i Position)
        {
            // Set Rows, Columns and Height and Width of blocks
            this.Rows = rows + GAME_OVER_ROWS;
            this.Columns = columns;
            this.blockX = blockX;
            this.blockY = blockY;
            // Instantiate Grid array
            grid = new Block[Rows, Columns];
            // Fill Grid with blocks
            FillGrid();
            // Set position
            this.Position = Position;
            // Set size of entire grid in pixels
            uint xSize = (uint)(columns * blockX);
            uint ySize = (uint)(rows * blockY);
            size = new Vector2u(xSize, ySize);
        }

        /// <summary>
        /// Check if a given coordinate is within the Grid array
        /// </summary>
        /// <param name="r"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public bool IsInside(int r, int c)
        {
            return r >= 0 && c >= 0 && r < Rows && c < Columns;
        }

        /// <summary>
        /// Check if a given cell is empty
        /// </summary>
        /// <param name="r"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public bool IsEmpty(int r, int c)
        {
            return IsInside(r, c) && grid[r, c].Color == BlockColor.EMPTY;
        }

        /// <summary>
        /// Check if a given row is empty
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public bool IsRowEmpty(int row)
        {
            for (int c = 0; c < Columns; c++)
            {
                if (!IsEmpty(row, c))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Check if a given row if full
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public bool IsRowFull(int row)
        {
            for (int c = 0; c < Columns; c++)
            {
                if (IsEmpty(row, c))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Clears all the full rows of the grid and moves Rows above to be below
        /// </summary>
        /// <returns></returns>
        public int ClearFullRows()
        {
            // Count the number of cleared rows
            int cleared = 0;

            // Iterate through all rows from bottom to top
            for (int r = Rows - 1; r > 0; r--)
            {
                // Clear any full rows and iterate cleared
                if (IsRowFull(r))
                {
                    ClearRow(r);
                    cleared++;
                }
                // If Cleared is greater than zero, move all other rows
                // based on how many rows beneath them ahve been cleared
                else if (cleared > 0)
                {
                    MoveRowDown(r, cleared);
                }
            }

            return cleared;
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
        /// Moves the given row by a specified number
        /// </summary>
        /// <param name="r"></param>
        /// <param name="num"></param>
        private void MoveRowDown(int r, int num)
        {
            for (int c = 0; c < Columns; c++)
            {
                grid[r + num, c].Color = grid[r, c].Color;
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
        /// Checks if the game over rowws (The top two rows) are empty (game over) or not
        /// </summary>
        /// <returns></returns>
        public bool CheckGameOver()
        {
            return !IsRowEmpty(0) || !IsRowEmpty(1);
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
