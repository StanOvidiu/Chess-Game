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
                if (IsKingChecked(auxPiece))
                {
                    if(auxPiece is King)
                        availableMoves = pieceMatrix[x.row, x.column].GetAvailableMoves(pieceMatrix);
                    else
                        availableMoves = GetMovesToEscapeCheck(pieceMatrix[x.row, x.column]);
                }
                else
                {
                    availableMoves = pieceMatrix[x.row, x.column].GetAvailableMoves(pieceMatrix);
                }
                OverlayAvailableMoves();
                firstClick = false;
            }
            else
            {
                if (availableMoves.Contains(pieceMatrix[x.row,x.column]))
                {
                    if (VerifyIfMoveIsKingSideCastle(auxPiece, pieceMatrix[x.row, x.column]))
                    {
                        MoveRookForKingSideCastle(x);
                    }
                    else if (VerifyIfMoveIsQueenSideCastle(auxPiece, pieceMatrix[x.row, x.column]))
                    {
                        MoveRookForQueenSideCastle(x);
                    }
                    VerifyIfMovedPieceIsKingOrRook(auxPiece);

                    if (!MoveLeadsToDiscoveredKing(x))
                    {
                        UnprotectProtectedPieces(auxPiece);

                        MovePieceAlgorithm(x);


                        if (VerifyIfKingIsCheckedAfterMove(x))
                        {
                            if (IsCheckMate(x))
                            {
                                MessageBox.Show("Checkmate", "Pop-up Title", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
                firstClick = true;
                RemoveOverlays();
            }
        }

        private bool MoveLeadsToDiscoveredKing(Cell x)
        {
            Piece[,] auxMatrix = Piece.DeepCopy(pieceMatrix);
            Piece verificationAuxPiece = Piece.DeepCopy(auxPiece);

            auxMatrix[verificationAuxPiece.row, verificationAuxPiece.column] = new Empty(PieceColor.None, verificationAuxPiece.row, verificationAuxPiece.column);

            auxMatrix[x.row, x.column] = verificationAuxPiece;

            auxMatrix[x.row, x.column].row = x.row;
            auxMatrix[x.row, x.column].column = x.column;

            foreach(var piece in auxMatrix)
            {
                if(!(piece is Empty) && piece.Color != auxMatrix[x.row, x.column].Color)
                {
                    foreach (var move in piece.GetAvailableMoves(auxMatrix))
                    {
                        if (move is King)
                            return true;
                    }
                }
            }
            return false;
        }

        private void UnprotectProtectedPieces(Piece piece)
        {
            foreach(var move in piece.protectedPieces)
            {
                pieceMatrix[move.row, move.column].takeableByKing = true;
            }
        }

        private bool IsCheckMate(Cell x)
        {
            foreach (var piece in pieceMatrix)
            {
                if (!(piece is Empty) && piece.Color != pieceMatrix[x.row, x.column].Color)
                {
                    List<Piece> pieceMoves = GetMovesToEscapeCheck(piece);
                    if(piece is King)
                    {
                        var kingMoves = ((King)piece).GetAvailableMoves(pieceMatrix);
                        pieceMoves.AddRange(kingMoves);
                    }
                    if (pieceMoves.Count > 0)
                        return false;
                }
            }
            return true;
        }
        private List<Piece> GetMovesToEscapeCheck(Piece piece)
        {
            List<Piece> validMoves = new List<Piece>();

            foreach(var move in piece.GetAvailableMoves(pieceMatrix))
            {
                Piece[,] temporaryPieceMatrix = GetTemporaryPieceMatrixAfterMove(piece, move);
                King king = GetPiecesKing(piece);
                if (!king.IsInCheck(temporaryPieceMatrix))
                {
                    validMoves.Add(move);
                }
            }
            return validMoves;
        }
        private Piece[,] GetTemporaryPieceMatrixAfterMove(Piece piece, Piece move)
        {
            Piece[,] temporaryPieceMatrix = (Piece[,])pieceMatrix.Clone();
            temporaryPieceMatrix[move.row, move.column] = piece;
            temporaryPieceMatrix[piece.row, piece.column] = new Empty(PieceColor.None, piece.row, piece.column);

            return temporaryPieceMatrix;
        }
        private bool IsKingChecked(Piece auxPiece)
        {
            King king = GetPiecesKing(auxPiece);
            if (king.IsChecked == true)
                return true;
            else
                return false;
        }
        private King GetPiecesKing(Piece auxPiece)
        {
            foreach(var piece in pieceMatrix)
            {
                if (piece is King && piece.Color == auxPiece.Color)
                    return (King)piece;
            }
            return null;
        }

        private bool VerifyIfKingIsCheckedAfterMove(Cell x)
        {
            availableMoves = pieceMatrix[x.row, x.column].GetAvailableMoves(pieceMatrix);
            foreach (var piece in availableMoves)
            {
                if (piece is King && piece.Color != pieceMatrix[x.row, x.column].Color)
                {
                    ((King)piece).IsChecked = true;
                    return true;
                }
            }
            return false;
        }

        public void OverlayAvailableMoves()
        {
            foreach(var move in availableMoves)
            {
                Panel overlay = new Panel();
                overlay.BackColor = Color.FromArgb(80, Color.Lime);
                overlay.Dock = DockStyle.Fill;
                overlay.Enabled = false;
                board.Grid[move.row, move.column].Controls.Add(overlay);
            }
        }
        public void RemoveOverlays()
        {
            foreach(var button in board.Grid)
            {
                button.Controls.Clear();
            }
        }

        public bool VerifyIfMoveIsKingSideCastle(Piece auxPiece, Piece destination)
        {
            if (auxPiece is King && destination.column == 8)
            {
                if (((King)auxPiece).HasMoved == false)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        public bool VerifyIfMoveIsQueenSideCastle(Piece auxPiece, Piece destination)
        {
            if (auxPiece is King && destination.column == 2)
                if (((King)auxPiece).HasMoved == false)
                    return true;
                else
                    return false;
            else
                return false;
        }
        private void VerifyIfMovedPieceIsKingOrRook(Piece piece)
        {
            if (piece is King)
                ((King)piece).HasMoved = true;
            if (piece is Rook)
                ((Rook)piece).HasMoved = true;
        }
        private void MoveRookForKingSideCastle(Cell x)
        {
            pieceMatrix[x.row, x.column - 1] = pieceMatrix[x.row, x.column + 1];
            pieceMatrix[x.row, x.column - 1].row = x.row;
            pieceMatrix[x.row, x.column - 1].column = x.column - 1;
            board.Grid[x.row, x.column - 1].Image = Image.FromFile(pieceMatrix[x.row, x.column - 1].Image);

            pieceMatrix[x.row, x.column + 1] = new Empty(PieceColor.None, x.row, x.column + 1);
            board.Grid[x.row, x.column + 1].Image = null;
        }
        private void MoveRookForQueenSideCastle(Cell x)
        {
            pieceMatrix[x.row, x.column + 1] = pieceMatrix[x.row, x.column - 2];
            pieceMatrix[x.row, x.column + 1].row = x.row;
            pieceMatrix[x.row, x.column + 1].column = x.column + 1;
            board.Grid[x.row, x.column + 1].Image = Image.FromFile(pieceMatrix[x.row, x.column - 2].Image);

            pieceMatrix[x.row, x.column - 2] = new Empty(PieceColor.None, x.row, x.column - 2);
            board.Grid[x.row, x.column - 2].Image = null;
        }

        private void MovePieceAlgorithm(Cell x)
        {
            pieceMatrix[auxPiece.row, auxPiece.column] = new Empty(PieceColor.None, auxPiece.row, auxPiece.column);
            board.Grid[auxPiece.row, auxPiece.column].Image = null;

            pieceMatrix[x.row, x.column] = auxPiece;

            pieceMatrix[x.row, x.column].row = x.row;
            pieceMatrix[x.row, x.column].column = x.column;
            board.Grid[x.row, x.column].Image = Image.FromFile(pieceMatrix[x.row, x.column].Image);

            auxPiece = null;
        }

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
