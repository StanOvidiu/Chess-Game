using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proiect_AI2
{
    public partial class Form1 : Form
    {
        Game game = new Game();
        public Form1()
        {
            InitializeComponent();
            SetFormProperties();
            Controls.Add(game.board);
        }
        public void SetFormProperties()
        {
            this.Width = 1250;
            this.Height = 1000;
            this.BackColor = Color.PeachPuff;
        }
    }
}
