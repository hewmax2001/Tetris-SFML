using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    /// <summary>
    /// Queue of TetrisPieces
    /// </summary>
    public class PieceQueue
    {
        // Array used to represent the Queue data structure
        public TetrisPiece[] Queue { get; }
        // Array to house all the possible Tetris Pieces to fill into the Queue
        private TetrisPiece[] PieceSelection = new TetrisPiece[]
        {
            new IPiece(),
            new JPiece(),
            new LPiece(),
            new OPiece(),
            new SPiece(),
            new TPiece(),
            new ZPiece()
        };

        /// <summary>
        /// Instantiate a new PieceQueue with given length
        /// </summary>
        /// <param name="length"></param>
        public PieceQueue(int length)
        {
            Queue = new TetrisPiece[length];
            // Fill the queue with random tetris pieces
            Fill();
        }

        /// <summary>
        /// Add a TetrisPiece to the beggining of the queue
        /// </summary>
        /// <param name="piece"></param>
        public void Enqueue(TetrisPiece piece)
        {
            Queue[Queue.Length - 1] = piece;
        }

        /// <summary>
        /// Remove and return a tetris piece from the end of the Queue
        /// </summary>
        /// <returns></returns>
        public TetrisPiece Deque()
        {
            // Get tetris piece at the end of the queue
            TetrisPiece tetrisPiece = Queue[0];
            // Create a new piece to enqueue
            TetrisPiece newPiece;
            // Iterate through queue shifting all pieces ahead one place in the queue
            for (int i = 0; i < Queue.Length - 1; i++)
            {
                Queue[i] = Queue[i + 1];
            }
            // Get a new that cnnot be the same as the piece ahead of it
            do
            {
                newPiece = GetRandomPiece();
            } while (newPiece == Queue[Queue.Length - 2]);
            // Enqueue the new piece
            Enqueue(newPiece);
            // Return the piece at the end of the queue
            return tetrisPiece;
        }

        /// <summary>
        /// Return the tetris piece at the end of the queue
        /// </summary>
        /// <returns></returns>
        public TetrisPiece Peek()
        {
            return Queue[0];
        }

        /// <summary>
        /// Fill the queue with random pieces
        /// </summary>
        public void Fill()
        {
            for (int i = 0; i < Queue.Length; i++)
            {
                Queue[i] = GetRandomPiece();
            }
        }

        /// <summary>
        /// Retrieve a random Tetris piece based on the PieceSelection array
        /// </summary>
        /// <returns></returns>
        private TetrisPiece GetRandomPiece()
        {
            Random rnd = new Random();
            int num = rnd.Next(0, PieceSelection.Length);
            return PieceSelection[num];
        }
    }
}
