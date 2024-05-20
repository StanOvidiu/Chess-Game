using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_AI2
{
    [Serializable]
    class Rook : Piece
    {
        public string whiteQueen = "images/rook-w.png";
        public string blackQueen = "images/rook-b.png";
        public const int whiteValue = 5;
        public const int blackValue = -5;
        public bool HasMoved = false;
        public Rook(PieceColor color, int rNumber, int cNumber) : base(color, rNumber, cNumber)
        {
            if (color == PieceColor.White)
                Image = whiteQueen;
            else
                Image = blackQueen;
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

            int[][] directions = {
                new int[] { -1, 0 }, new int[] { 1, 0 },
                new int[] { 0, -1 }, new int[] { 0, 1 } };

            foreach (var direction in directions)
            {
                for (int i = 1; i <= 10; i++)
                {
                    int newX = row + i * direction[0];
                    int newY = column + i * direction[1];
                    Piece potentialMove = new Empty(PieceColor.None, newX, newY);

                    if (!potentialMove.IsValidCell(potentialMove))
                        break;

                    potentialMove = pieceMatrix[newX, newY];

                    if (potentialMove.IsCellEmpty(potentialMove) || potentialMove.IsCellOcupiedByOpponent(value, potentialMove))
                        availableMoves.Add(potentialMove);

                    if (!(potentialMove.IsCellEmpty(potentialMove)) && !(potentialMove.IsCellOcupiedByOpponent(value, potentialMove)))
                        pieceMatrix[potentialMove.row, potentialMove.column].takeableByKing = false;

                    if (!potentialMove.IsCellEmpty(potentialMove))
                        break;
                }
            }

            return availableMoves;
        }
    }
}
