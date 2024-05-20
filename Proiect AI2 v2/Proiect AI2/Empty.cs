using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_AI2
{
    [Serializable]
    class Empty : Piece
    {
        public Empty(PieceColor color, int row, int column) : base(color, row, column)
        {
            
        }

        public override List<Piece> GetAvailableMoves(Piece[,] pieceMatrix)
        {
            return null;
        }
    }
}
