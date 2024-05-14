using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proiect_AI2
{
    class Board : Panel
    {
        public int rowSize { get; set; }
        public int columnSize { get; set; }
        public Cell[,] Grid { get; set; }

        public Board(int rSize, int cSize)
        {
            rowSize = rSize;
            columnSize = cSize;
            InitializeGrid();
        }

        public void InitializeGrid()
        {
            Grid = new Cell[rowSize, columnSize];

            for (int i = 0; i < rowSize; i++)
            {
                for (int j = 0; j < columnSize; j++)
                {
                    Grid[i, j] = new Cell(i,j);

                    if ((i + j) % 2 == 0)
                        Grid[i, j].BackColor = Color.AntiqueWhite;
                    else
                        Grid[i, j].BackColor = Color.Peru;
                }
            }
        }
    }
}
