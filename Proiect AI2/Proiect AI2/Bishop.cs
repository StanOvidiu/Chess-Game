using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_AI2
{
    class Bishop : Piece
    {
        public string whiteBishop = "images/bishop-w.png";
        public string blackBishop = "images/bishop-b.png";
        public const int whiteValue = 3;
        public const int blackValue = -3;
        public Bishop(PieceColor color, int rNumber, int cNumber) : base(color, rNumber, cNumber)
        {
            if (color == PieceColor.White)
                Image = whiteBishop;
            else
                Image = blackBishop;
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

            return availableMoves;
        }
    }
}
