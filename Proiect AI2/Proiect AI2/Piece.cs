using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_AI2
{
    public abstract class Piece
    {
        public PieceColor Color { get; set; }

        public string Image;
        public int row { get; set; }
        public int column { get; set; }

        public int value = 0;

        public Piece(PieceColor color, int rowIndex, int columnIndex)
        {
            Color = color;
            row = rowIndex;
            column = columnIndex;
        }

        public bool IsCellEmpty(Piece piece)
        {
            if (piece is Empty)
                return true;
            else
                return false;
        }

        public bool IsValidCell(Piece piece)
        {
            return piece.row >= 0 && piece.row < 8 && piece.column >= 0 && piece.column < 10;
        }

        internal bool IsCellOcupiedByOpponent(int value, Piece potentialMove)
        {
            if (value > 0 && potentialMove.value < 0)
                return true;
            else if (value < 0 && potentialMove.value > 0)
                return true;
            else
                return false;
        }

        public abstract List<Piece> GetAvailableMoves(Piece[,] pieceMatrix);

    }
}
