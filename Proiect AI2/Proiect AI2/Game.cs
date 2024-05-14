using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proiect_AI2
{
    class Game
    {
        public Board board;
        public Piece[,] pieceMatrix = new Piece[8,10];
        public bool firstClick = true;
        public List<Piece> availableMoves;
        public Piece auxPiece;

        public Game()
        {
            InitializeBoard();
            LoadPieces();
            board.Invalidate();
        }

        private void InitializeBoard()
        {
            board = new Board(8, 10);

            board.Dock = DockStyle.Fill;
            board.Size = new Size(500, 600);

            for (int i = 0; i < board.rowSize; i++)
            {
                for (int j = 0; j < board.columnSize; j++)
                {
                    board.Grid[i, j].Click += Movement;
                    board.Grid[i, j].Location = new Point(j * board.Grid[i, j].Width, i * board.Grid[i, j].Height);
                    board.Controls.Add(board.Grid[i, j]);
                }
            }
        }

        private void Movement(object sender, EventArgs e)
        {
            var x = sender as Cell;
            if (firstClick)
            {
                if (pieceMatrix[x.row, x.column] is Empty)
                    return;
                auxPiece = pieceMatrix[x.row, x.column];
                availableMoves = pieceMatrix[x.row, x.column].GetAvailableMoves(pieceMatrix);
                firstClick = false;
            }
            else
            {
                if (availableMoves.Contains(pieceMatrix[x.row,x.column]))
                {
                    pieceMatrix[auxPiece.row, auxPiece.column] = new Empty(PieceColor.None, auxPiece.row, auxPiece.column);
                    board.Grid[auxPiece.row, auxPiece.column].Image = null;

                    pieceMatrix[x.row, x.column] = auxPiece;
                    auxPiece = null;

                    pieceMatrix[x.row, x.column].row = x.row;
                    pieceMatrix[x.row, x.column].column = x.column;
                    board.Grid[x.row, x.column].Image = Image.FromFile(pieceMatrix[x.row, x.column].Image);
                }
                firstClick = true;
            }
        }

        //private Piece IdentifyPiece(Piece piece, int row, int column)
        //{
        //    switch (piece)
        //    {
        //        case Pawn _:
        //            return new Pawn(piece.Color, row, column);
        //        case Knight _:
        //            return new Knight(piece.Color, row, column);
        //        case Bishop _:
        //            return new Bishop(piece.Color, row, column);
        //        case Rook _:
        //            return new Rook(piece.Color, row, column);
        //        case Queen _:
        //            return new Queen(piece.Color, row, column);
        //        case King _:
        //            return new King(piece.Color, row, column);
        //        case Chancellor _:
        //            return new Chancellor(piece.Color, row, column);
        //        case Archbishop _:
        //            return new Archbishop(piece.Color, row, column);
        //        default:
        //            return new Empty(piece.Color, row, column);
        //    }
        //}

        private void LoadPieces()
        {
            pieceMatrix[0, 0] = new Rook(PieceColor.Black, 0, 0);
            board.Grid[0, 0].Image = Image.FromFile(pieceMatrix[0, 0].Image);

            pieceMatrix[0, 1] = new Knight(PieceColor.Black, 0, 1);
            board.Grid[0, 1].Image = Image.FromFile(pieceMatrix[0, 1].Image);

            pieceMatrix[0, 2] = new Bishop(PieceColor.Black, 0, 2);
            board.Grid[0, 2].Image = Image.FromFile(pieceMatrix[0, 2].Image);

            pieceMatrix[0, 3] = new Queen(PieceColor.Black, 0, 3);
            board.Grid[0, 3].Image = Image.FromFile(pieceMatrix[0, 3].Image);

            pieceMatrix[0, 4] = new Chancellor(PieceColor.Black, 0, 4);
            board.Grid[0, 4].Image = Image.FromFile(pieceMatrix[0, 4].Image);

            pieceMatrix[0, 5] = new King(PieceColor.Black, 0, 5);
            board.Grid[0, 5].Image = Image.FromFile(pieceMatrix[0, 5].Image);

            pieceMatrix[0, 6] = new Archbishop(PieceColor.Black, 0, 6);
            board.Grid[0, 6].Image = Image.FromFile(pieceMatrix[0, 6].Image);

            pieceMatrix[0, 7] = new Bishop(PieceColor.Black, 0, 7);
            board.Grid[0, 7].Image = Image.FromFile(pieceMatrix[0, 7].Image);

            pieceMatrix[0, 8] = new Knight(PieceColor.Black, 0, 8);
            board.Grid[0, 8].Image = Image.FromFile(pieceMatrix[0, 8].Image);

            pieceMatrix[0, 9] = new Rook(PieceColor.Black, 0, 9);
            board.Grid[0, 9].Image = Image.FromFile(pieceMatrix[0, 9].Image);

            for(int i = 0; i < 10; i++)
            {
                pieceMatrix[1, i] = new Pawn(PieceColor.Black, 1, i);
                board.Grid[1, i].Image = Image.FromFile(pieceMatrix[1, i].Image);
            }

            for (int i = 2; i < 6; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    pieceMatrix[i, j] = new Empty(PieceColor.None, i, j);
                    board.Grid[i, j].Image = null;
                }
            }

            for (int i = 0; i < 10; i++)
            {
                pieceMatrix[6, i] = new Pawn(PieceColor.White, 6, i);
                board.Grid[6, i].Image = Image.FromFile(pieceMatrix[6, i].Image);
            }

            pieceMatrix[7, 0] = new Rook(PieceColor.White, 7, 0);
            board.Grid[7, 0].Image = Image.FromFile(pieceMatrix[7, 0].Image);

            pieceMatrix[7, 1] = new Knight(PieceColor.White, 7, 1);
            board.Grid[7, 1].Image = Image.FromFile(pieceMatrix[7, 1].Image);

            pieceMatrix[7, 2] = new Bishop(PieceColor.White, 7, 2);
            board.Grid[7, 2].Image = Image.FromFile(pieceMatrix[7, 2].Image);

            pieceMatrix[7, 3] = new Queen(PieceColor.White, 7, 3);
            board.Grid[7, 3].Image = Image.FromFile(pieceMatrix[7, 3].Image);

            pieceMatrix[7, 4] = new Chancellor(PieceColor.White, 7, 4);
            board.Grid[7, 4].Image = Image.FromFile(pieceMatrix[7, 4].Image);

            pieceMatrix[7, 5] = new King(PieceColor.White, 7, 5);
            board.Grid[7, 5].Image = Image.FromFile(pieceMatrix[7, 5].Image);

            pieceMatrix[7, 6] = new Archbishop(PieceColor.White, 7, 6);
            board.Grid[7, 6].Image = Image.FromFile(pieceMatrix[7, 6].Image);

            pieceMatrix[7, 7] = new Bishop(PieceColor.White, 7, 7);
            board.Grid[7, 7].Image = Image.FromFile(pieceMatrix[7, 7].Image);

            pieceMatrix[7, 8] = new Knight(PieceColor.White, 7, 8);
            board.Grid[7, 8].Image = Image.FromFile(pieceMatrix[7, 8].Image);

            pieceMatrix[7, 9] = new Rook(PieceColor.White, 7, 9);
            board.Grid[7, 9].Image = Image.FromFile(pieceMatrix[7, 9].Image);
        }
    }
}
