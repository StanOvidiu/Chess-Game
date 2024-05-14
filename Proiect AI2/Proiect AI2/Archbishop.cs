using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_AI2
{
    class Archbishop : Piece
    {
        public string whiteArchbishop = "images/archbis-w.png";
        public string blackArchbishop = "images/archbis-b.png";
        public const int whiteValue = 7;
        public const int blackValue = -7;
        public Archbishop(PieceColor color, int rNumber, int cNumber) : base(color, rNumber, cNumber)
        {
            if (color == PieceColor.White)
                Image = whiteArchbishop;
            else
                Image = blackArchbishop;
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
                new int[] { -1, -1 }, new int[] { -1, 1 },
                new int[] { 1, -1 }, new int[] { 1, 1 } };

            int[][] possibleMoves = {
                new int[] {-2, -1 }, new int[] {-2, 1 }, new int[] {-1, -2 }, new int[] {-1, 2 },
                new int[] { 1, -2 }, new int[] { 1, 2 }, new int[] { 2, -1 }, new int[] { 2, 1 }
            };

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

                    if (!potentialMove.IsCellEmpty(potentialMove))
                        break;
                }
            }

            foreach (var move in possibleMoves)
            {
                int newX = row + move[0];
                int newY = column + move[1];
                Piece potentialMove = new Empty(PieceColor.None, newX, newY);

                if (potentialMove.IsValidCell(potentialMove))
                {
                    potentialMove = pieceMatrix[newX, newY];
                    if (potentialMove.IsCellEmpty(potentialMove) || potentialMove.IsCellOcupiedByOpponent(value, potentialMove))
                        availableMoves.Add(potentialMove);
                }
            }

            return availableMoves;
        }
    }
}
