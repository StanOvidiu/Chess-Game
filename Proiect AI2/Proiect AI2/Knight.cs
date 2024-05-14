using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_AI2
{
    class Knight : Piece
    {
        public string whiteKnight = "images/knight-w.png";
        public string blackKnight = "images/knight-b.png";
        public const int whiteValue = 3;
        public const int blackValue = -3;

        public Knight(PieceColor color, int row, int column) : base(color, row, column)
        {
            if (color == PieceColor.White)
                Image = whiteKnight;
            else
                Image = blackKnight;
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

            int[][] possibleMoves = {
                new int[] {-2, -1 }, new int[] {-2, 1 }, new int[] {-1, -2 }, new int[] {-1, 2 },
                new int[] { 1, -2 }, new int[] { 1, 2 }, new int[] { 2, -1 }, new int[] { 2, 1 }
            };

            foreach(var move in possibleMoves)
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
