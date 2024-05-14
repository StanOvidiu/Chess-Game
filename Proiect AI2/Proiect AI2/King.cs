using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_AI2
{
    class King : Piece
    {
        public string whiteKing = "images/king-w.png";
        public string blackKing = "images/king-b.png";
        public const int whiteValue = 10000;
        public const int blackValue = -10000;
        public bool HasMoved = false;
        public King(PieceColor color, int rNumber, int cNumber) : base(color, rNumber, cNumber)
        {
            if (color == PieceColor.White)
                Image = whiteKing;
            else
                Image = blackKing;
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
                new int[] { -1, 0 }, new int[] { 1, 0 }, new int[] { 0, -1 }, new int[] { 0, 1 },
                new int[] { -1, -1 }, new int[] { -1, 1 }, new int[] { 1, -1 }, new int[] { 1, 1 }};

            foreach (var direction in directions)
            {
                int newX = row + direction[0];
                int newY = column + direction[1];
                Piece potentialMove = new Empty(PieceColor.None, newX, newY);

                if (potentialMove.IsValidCell(potentialMove))
                {
                    potentialMove = pieceMatrix[newX, newY];
                    if (potentialMove.IsCellEmpty(potentialMove) || potentialMove.IsCellOcupiedByOpponent(value, potentialMove))
                        availableMoves.Add(potentialMove);
                }
            }

            CheckCastlingMoves(pieceMatrix, availableMoves);

            return availableMoves;
        }

        private void CheckCastlingMoves(Piece[,] pieceMatrix, List<Piece> availableMoves)
        {
            if (HasMoved || IsInCheck(pieceMatrix))
                return;

            // Check for kingside castling
            if (CanCastle(pieceMatrix, row, column + 4, 1))
            {
                Piece kingsideCastleMove = new Empty(PieceColor.None, row, column + 2);
                availableMoves.Add(kingsideCastleMove);
            }

            // Check for queenside castling
            if (CanCastle(pieceMatrix, row, column - 5, -1))
            {
                Piece queensideCastleMove = new Empty(PieceColor.None, row, column - 2);
                availableMoves.Add(queensideCastleMove);
            }
        }

        private bool CanCastle(Piece[,] pieceMatrix, int rookRow, int rookColumn, int direction)
        {
            // Check if the Rook is in the correct place and hasn't moved
            Piece rook = pieceMatrix[rookRow, rookColumn];
            if (!(rook is Rook) || ((Rook)rook).HasMoved)
                return false;

            // Check that all cells between the King and the Rook are empty
            for (int i = 1; i < Math.Abs(rookColumn - column); i++)
            {
                if (!(pieceMatrix[row, column + i * direction] is Empty))
                    return false;
            }

            // Check that the King is not passing through or ending up in a check
            for (int i = 0; i <= 2; i++)
            {
                int checkColumn = column + i * direction;
                if (IsCellUnderAttack(pieceMatrix, row, checkColumn))
                    return false;
            }

            return true;
        }

        private bool IsInCheck(Piece[,] pieceMatrix)
        {
            return IsCellUnderAttack(pieceMatrix, row, column);
        }

        private bool IsCellUnderAttack(Piece[,] pieceMatrix, int x, int y)
        {
            foreach (var piece in pieceMatrix)
            {
                if (piece != null && piece.Color != this.Color)
                {
                    var opponentMoves = piece.GetAvailableMoves(pieceMatrix);
                    foreach (var move in opponentMoves)
                    {
                        if (move.row == x && move.column == y)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
