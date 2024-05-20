using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_AI2
{
    [Serializable]
    public abstract class Piece
    {
        public PieceColor Color { get; set; }

        public string Image;
        public int row { get; set; }
        public int column { get; set; }

        public int value = 0;

        public bool takeableByKing = true;

        public List<Piece> protectedPieces;

        public Piece(PieceColor color, int rowIndex, int columnIndex)
        {
            Color = color;
            row = rowIndex;
            column = columnIndex;
            protectedPieces = new List<Piece>();
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

        public static T DeepCopy<T>(T original)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(memoryStream, original);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(memoryStream);
            }
        }

    }
}
