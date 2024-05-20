using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_AI2
{
    [Serializable]
    class Chancellor : Piece
    {
        public string whiteChancellor = "images/chancel-w.png";
        public string blackChancellor = "images/chancel-b.png";
        public const int whiteValue = 8;
        public const int blackValue = -8;
        public Chancellor(PieceColor color, int rNumber, int cNumber) : base(color, rNumber, cNumber)
        {
            if (color == PieceColor.White)
                Image = whiteChancellor;
            else
                Image = blackChancellor;
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

            //knight moves
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

                    if (!(potentialMove.IsCellEmpty(potentialMove)) && !(potentialMove.IsCellOcupiedByOpponent(value, potentialMove)))
                        pieceMatrix[potentialMove.row, potentialMove.column].takeableByKing = false;

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

                    if (!(potentialMove.IsCellEmpty(potentialMove)) && !(potentialMove.IsCellOcupiedByOpponent(value, potentialMove)))
                        pieceMatrix[potentialMove.row, potentialMove.column].takeableByKing = false;
                }
            }

            return availableMoves;
        }
    }
}
