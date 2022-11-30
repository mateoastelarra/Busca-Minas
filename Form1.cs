using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks; 
using System.Windows.Forms;

namespace BuscaMinas
{
    public partial class Form1 : Form
    {
        private Button[,] mat;
        private bool explota;
        private int bombasEncontradas;
        private int M = 10;
        private int bombas;

        public Form1()
        {
            InitializeComponent(M);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int x = 10;
            int y = 50;
            int s = -2 * M + 70;
            mat = new Button[M, M];
            for (int fil = 0; fil < mat.GetLength(0); fil++)
            {
                for (int col = 0; col < mat.GetLength(1); col++)
                {
                    mat[fil, col] = new Button();
                    mat[fil, col].Location = new Point(x, y);
                    mat[fil, col].Size = new Size(s, s);
                    Controls.Add(mat[fil, col]);
                    mat[fil, col].MouseClick += MyButton_MouseClick;
                    mat[fil, col].MouseDown += MyButton_MouseClick;
                    x = x + s + 2;
                }
                y = y + s + 2;
                x = 10;
            }
            Crear(M);
        }

        private void plantarMinas()
        {
            Random ale = new Random();
            bombas = M * M / 5 - M;
            int minas = 0;
            while (minas < bombas)
            {
                int fil = ale.Next(0, M);
                int col = ale.Next(0, M);
                if (mat[fil, col].Tag.ToString() != "-1")
                {
                    mat[fil, col].Tag = "-1";
                    minas++;
                }
            }
        }

        private void bombasCercanas()
        {
            for (int fil = 0; fil < M; fil++)
            {
                for (int col = 0; col < M; col++)
                {
                    if (mat[fil, col].Tag.ToString() != "-1")
                    {
                        mat[fil, col].Text = "";
                        int bombasCerc = 0;
                        for (int i = -1; i <= 1; i++)
                        {
                            for (int j = -1; j <= 1; j++)
                            {
                                if (fil + i >= 0 && col + j >= 0 && fil + i < M && col + j < M)
                                {
                                    if (mat[fil + i, col + j].Tag.ToString() == "-1")
                                    {
                                        bombasCerc++;
                                    }
                                }
                            }
                        }
                        mat[fil, col].Tag = bombasCerc.ToString();
                    }

                }
            }
        }

        private void Crear(int M)
        {
            InitializeComponent(M);
            Text = "Busca Minas";
            explota = false;
            button1.Enabled = true;
            bombasEncontradas = 0;
            for (int fil = 0; fil < M; fil++)
            {
                for (int col = 0; col < M; col++)
                {
                     mat[fil, col].Tag = "0";
                     mat[fil, col].Text = "";
                     mat[fil, col].BackColor = Color.Azure;
                }
            }
            plantarMinas();
            bombasCercanas();
        }

        private void Perder()
        {
            for (int fil = 0; fil < M; fil++)
            {
                for (int col = 0; col < M; col++)
                {
                    if (mat[fil, col].Tag.ToString() == "-1")
                    {
                        if (mat[fil, col].Text == "B")
                        {
                            mat[fil, col].BackColor = Color.Green;
                        }
                        else
                        {
                            mat[fil, col].BackColor = Color.Red;
                        }
                    }
                }
            }
        }

        private void Ganar()
        {
            for (int fil = 0; fil < M; fil++)
            {
                for (int col = 0; col < M; col++)
                {
                    if (mat[fil, col].Tag.ToString() == "-1")
                    {
                        if (mat[fil, col].Text == "B")
                        {
                            mat[fil, col].BackColor = Color.Green;
                        }
                    }
                }
            }
        }

        private void Colorear(int fil, int col)
        {
            string cercanas = mat[fil, col].Text;
            switch (cercanas)
            {
                case "1":
                    mat[fil,col].ForeColor = Color.Blue;
                    break;
                case "2":
                    mat[fil, col].ForeColor = Color.Green;
                    break;
                case "3":
                    mat[fil, col].ForeColor = Color.Red;
                    break;
                case "4":
                    mat[fil, col].ForeColor = Color.Purple;
                    break;
            }
        }

        private void Clickear(int fil, int col)
        {
            if (fil >= 0 && fil < M && col >= 0 && col < M && explota == false)
            {
                if (mat[fil, col].Tag.ToString() == "-1")
                {
                    explota = true;
                    mat[fil, col].BackColor = Color.Red;
                    Text = " Perdiste!!!";
                    Perder();
                }
                else
                {
                    if (mat[fil, col].Tag.ToString() == "0")
                    {
                        mat[fil, col].BackColor = Color.Yellow;
                        mat[fil, col].Tag = "10";
                        Clickear(fil, col + 1);
                        Clickear(fil + 1, col);
                        Clickear(fil - 1, col);
                        Clickear(fil, col - 1);
                        Clickear(fil - 1, col - 1);
                        Clickear(fil - 1, col + 1);
                        Clickear(fil + 1, col - 1);
                        Clickear(fil + 1, col + 1);
                    }
                    else if (mat[fil, col].Tag.ToString() == "10")
                    {
                        return;
                    }
                    else
                    {
                        mat[fil, col].Text = mat[fil, col].Tag.ToString();
                        Colorear(fil, col);
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            M = 10;
            Crear(10);
        }

        private void MyButton_MouseClick(object sender, MouseEventArgs e)
        {
            
            Button btn = (Button)sender;
            int col = (btn.Location.X - 10) / (-2 * M + 72);
            int fil = (btn.Location.Y - 50) / (-2 * M + 72);
            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (mat[fil, col].Text != "B")
                    {
                        Clickear(fil, col);
                    }
                    break;

                case MouseButtons.Right:
                    if (mat[fil,col].Text == "B" && explota == false)
                    {
                        mat[fil, col].Text = "";
                        mat[fil, col].BackColor = Color.Azure;
                        if (mat[fil, col].Tag.ToString() == "-1")
                        {
                            bombasEncontradas--;
                        }
                    }
                    else if (explota == false)
                    {
                        mat[fil, col].Text = "B";
                        mat[fil, col].BackColor = Color.Black;
                        mat[fil, col].ForeColor = Color.White;
                        if (mat[fil, col].Tag.ToString() == "-1")
                        {
                            bombasEncontradas++;
                        }
                    }
                    if (bombasEncontradas == bombas && explota == false)
                    {
                        explota = true;
                        Text = " Ganaste!!!";
                        Ganar();
                    }
                    break;   
            }
        }
    }
}
