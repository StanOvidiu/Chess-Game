using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_AI2
{
    class Pawn : Piece
    {
        public string whitePawn = "images/pawn-w.png";
        public string blackPawn = "images/pawn-b.png";
        public const int whiteValue = 1;
        public const int blackValue = -1;

        public Pawn(PieceColor color, int row, int column) : base(color, row, column)
        {
            if (color == PieceColor.White)
                Image = whitePawn;
            else
                Image = blackPawn;
            SetValue(color);
        }
        public void SetValue(PieceColor color)
        {
            if (color == PieceColor.White)
                value = whiteValue;
            else
                value = blackValue;
        }

        public override List<Piece> GetAvailableMoves(Piece[,] pieceMatrix)
        {
            List<Piece> availableMoves = new List<Piece>();
            int direction = (Color == PieceColor.White) ? -1 : 1;

            Piece forwardOneCell = pieceMatrix[row + direction, column];
            if (forwardOneCell.IsCellEmpty(forwardOneCell))
            {
                availableMoves.Add(forwardOneCell);

                if ((Color == PieceColor.White && row == 6) || (Color == PieceColor.Black && row == 1))
                {
                    Piece forwardTwoSquares = pieceMatrix[row + (2 * direction), column];
                    if (forwardOneCell.IsCellEmpty(forwardTwoSquares))
                        availableMoves.Add(forwardTwoSquares);
                }
            }

            return availableMoves;
        }
    }
}
