using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proiect_AI2
{
    public class Cell : Button
    {
        private const int cellWidth = 120;
        private const int cellHeight = 120;
        public int value = 0;
        public int row { get; set; }
        public int column { get; set; }

        public Cell(int rowIndex, int columnIndex)
        {
            row = rowIndex;
            column = columnIndex;
            Width = cellWidth;
            Height = cellHeight;
            FlatStyle = FlatStyle.Flat;
        }
    }
}
