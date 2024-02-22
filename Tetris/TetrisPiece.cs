using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    /// <summary>
    /// Tetris Piece comprised of blocks at specific postions.
    /// Along with offset to denote position in a TetrisGrid.
    /// </summary>
    public abstract class TetrisPiece
    {
        // Position of the tiles a tetris piece occupies as it rotates
        protected abstract Position[][] Tiles { get; }
        // The starting offset of a piece inside a grid
        protected abstract Position StartOffset { get; }
        // The type of a block denoted by a colour
        public abstract BlockColor BlockType { get; }
        // The state of rotation for a piece
        private int rotationState;
        // The offset of a Tetris Piece in a Tetris Grid
        private Position offset;
        public Position Offset { get { return offset; } set { offset = value; } }

        /// <summary>
        /// Instantiates a new Tetris piece and sets its offset to the starting offsets.
        /// </summary>
        public TetrisPiece()
        {
            offset = new Position(StartOffset.Row, StartOffset.Column);
        }

        /// <summary>
        /// Returns the positions of the tiles of a tetris piece
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Position> TilePositions()
        {
            foreach (Position p in Tiles[rotationState])
            {
                yield return new Position(p.Row + offset.Row, p.Column + offset.Column);
            }
        }

        /// <summary>
        /// Rotate the piece ClockWise
        /// </summary>
        public void RotateCW()
        {
            rotationState = (rotationState + 1) % Tiles.Length;
        }

        /// <summary>
        /// Rotate the piece Counter clockwise
        /// </summary>
        public void RotateCCW()
        {
            if (rotationState == 0)
            {
                rotationState = Tiles.Length - 1;
            }
            else
            {
                rotationState--;
            }
        }

        /// <summary>
        /// Move the piece by a given row and column increment.
        /// Changes the piece's offset.
        /// </summary>
        /// <param name="r"></param>
        /// <param name="c"></param>
        public void Move(int r, int c)
        {
            offset.Row += r;
            offset.Column += c;
        }

        /// <summary>
        /// Resets the piece to its default state.
        /// </summary>
        public void Reset()
        {
            rotationState = 0;
            offset.Row = StartOffset.Row;
            offset.Column = StartOffset.Column;
        }
    }
}
