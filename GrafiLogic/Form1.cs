using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace GrafiLogic
{
    struct Cella
    {
        public int Line;
        public int Row;
        public Cella(int line, int row)
        {
            Line = line;
            Row = row;
        }
    }
    public partial class Form1 : Form
    {
        public int[,] tabla_ertek;
        public PictureBox[,] tabla;
        public int szines;
        public PictureBox[] kepek = new PictureBox[5];
        public Random r = new Random();
        public bool x = true;
        public PictureBox[] elet = new PictureBox[3];
        public int hiba = 3;
        public Label[] fuggoleges;
        public Label[] vizszintes;
        public int nagysag;
        public int db = 0;
        public int[,] palya_kepek = new int[30, 30];
        public bool random = false;
        public int kepkocka_v = 0;
        public int kepkocka_f = 0;
        public int lepesek = 1;
        public int kep_szama = 0;
        public int[,] koordinata = new int[2,2];
        public Form1()
        {
            InitializeComponent();
            kepbeolvaso();
        }
        public void kepbeolvaso()
        {
            for (int i = 0; i < 5; i++)
            {
                kepek[i] = new PictureBox();
                Bitmap a = new Bitmap($"{i}.png");
                kepek[i] = new PictureBox();
                kepek[i].Image = (Image)a;

            }

        }
        public void alap()
        {
            hiba = 3;
            if (random == true)
            {
                tabla_rajz();
            }
            else
            {
                Bitmap kep = new Bitmap($"{kep_szama}.png");
                for (int i = 0; i < 30; i++)
                {
                    for (int j = 0; j < 30; j++)
                    {
                        Color color = kep.GetPixel(j, i);
                        palya_kepek[i, j] = color.R;
                    }
                }

                pixels();
            }
            
            StreamWriter be = new StreamWriter("teszt.txt");
            for (int i = 0; i < nagysag; i++)
            {
                for (int j = 0; j < nagysag; j++)
                {
                    be.Write(tabla_ertek[i, j]);
                }
                be.WriteLine();
            }
            be.Close();

            tabla = new PictureBox[nagysag, nagysag];
            for (int i = 0; i < nagysag; i++)
            {
                for (int j = 0; j < nagysag; j++)
                {
                    tabla[i, j] = new PictureBox();
                    tabla[i, j].SizeMode = PictureBoxSizeMode.StretchImage;
                    tabla[i, j].Left = 100 + j * 50;
                    tabla[i, j].Top = 85 + i * 50;
                    tabla[i, j].Width = 50;
                    tabla[i, j].Height = 50;

                    tabla[i, j].Image = kepek[0].Image;
                    this.Controls.Add(tabla[i, j]);
                   // tabla[i, j].Click += kep_klikk;
                    tabla[i, j].MouseDown += tabla_MouseDown;
                   // tabla[i, j].MouseMove += tabla_MouseMove;
                    tabla[i, j].MouseUp += tabla_MouseUp;
                    Cella cella = new Cella(i, j);
                    tabla[i, j].Tag = cella;

                }
            }
            for (int i = 0; i < 3; i++)
            {
                elet[i] = new PictureBox();
                elet[i].SizeMode = PictureBoxSizeMode.StretchImage;
                elet[i].Left = 0 + i * 25;
                elet[i].Top = 25;
                elet[i].Width = 25;
                elet[i].Height = 25;
                elet[i].Image = kepek[4].Image;
                this.Controls.Add(elet[i]);
            }
            button1.Visible = true;
            button2.Visible = true;

            szamok();

        }
        
        
        private void tabla_MouseDown(object sender, MouseEventArgs e)
        {
            PictureBox clicked_pd = (PictureBox)sender;
            Cella c = (Cella)clicked_pd.Tag;
          

            koordinata[0, 0] = c.Line;
            koordinata[0, 1] = c.Row;
        }

        private void tabla_MouseUp(object sender, MouseEventArgs e)
        {
            PictureBox clicked_pd = (PictureBox)sender;
            Cella c = (Cella)clicked_pd.Tag;
            cella_kereses();
            label2.Text = koordinata[0, 0] + " " + koordinata[0, 1] + "\n" + koordinata[1, 0] + " " + koordinata[1, 1] + "\n";
            
            
            if (koordinata[0, 0]==koordinata[1, 0] && koordinata[0,1]== koordinata[1, 1] )
            {
                actions(koordinata[0, 0], koordinata[1, 1]);
            }
            
            else if (koordinata[0, 0] == koordinata[1, 0] || koordinata[0, 1] == koordinata[1, 1])
              {
                    hosszulepesek();
              }
            

        }
       
        public void tabla_rajz()
        {
            tabla_ertek = new int[nagysag, nagysag];
            for (int i = 0; i < nagysag; i++)
            {
                for (int j = 0; j < nagysag; j++)
                {
                    tabla_ertek[i, j] = r.Next(0, 2);
                    if (tabla_ertek[i, j] == 1)
                    {
                        szines++;
                    }
                }
            }

            
        }
        public void pixels()
        {
            nagysag = 15;
            tabla_ertek = new int[nagysag, nagysag];

            for (int i = 0; i < nagysag; i++)
            {
                for (int j = 0; j < nagysag; j++)
                {
                    if (palya_kepek[i + kepkocka_f, j + kepkocka_v] == 255)
                    {
                        tabla_ertek[i, j] = 0;
                    }
                    else if (palya_kepek[i + kepkocka_f, j + kepkocka_v] == 0)
                    {
                        tabla_ertek[i, j] = 1;
                        szines++;
                    }
                }
            }

        }
        /*
        public void kep_klikk(object sender, System.EventArgs e)
        {
            PictureBox clicked_pd = (PictureBox)sender;
            Cella c = (Cella)(clicked_pd.Tag);

            actions(c.Line, c.Row);

        }
        */
      
        public void actions(int i, int j)
        {

            switch (x)
            {
                case true:
                    if (tabla_ertek[i, j] == 1 && tabla[i, j].Image != kepek[1].Image)
                    {
                        tabla[i, j].Image = kepek[1].Image;
                        db++;
                        vege();



                    }
                    else if (tabla_ertek[i, j] == 0 && hiba>0 && tabla[i, j].Image!=kepek[2].Image)
                    {
                        
                        tabla[i, j].Image = kepek[2].Image;
                        elet[hiba - 1].Visible = false;
                        
                        hiba--;
                        if (hiba == 0)
                        {
                            vege();
                        }



                    }
                    break;
                case false:
                   if (tabla[i, j].Image == kepek[0].Image)
                    {
                        tabla[i, j].Image = kepek[3].Image;
                    }
                    else if (tabla[i, j].Image == kepek[3].Image)
                    {
                        tabla[i, j].Image = kepek[0].Image;
                    }
                    break;

            }
        }

        public void szamok()
        {

            vizszintes = new Label[nagysag];
            fuggoleges = new Label[nagysag];

            for (int i = 0; i < nagysag; i++)
            {
                vizszintes[i] = new Label();
                vizszintes[i].Left = 40;
                vizszintes[i].Top = 100 + i * 50;
                vizszintes[i].Text = "";
                vizszintes[i].Width = 50;
                vizszintes[i].Height = 50;
                fuggoleges[i] = new Label();
                fuggoleges[i].Left = 100 + i * 50;
                fuggoleges[i].Top = 0;
                fuggoleges[i].Width = 50;
                fuggoleges[i].Height = 100;
                fuggoleges[i].Text = "";
                this.Controls.Add(vizszintes[i]);
                this.Controls.Add(fuggoleges[i]);
            }

            for (int i = 0; i < nagysag; i++)
            {
                int v = 0;
                int f = 0;
                for (int j = 0; j < nagysag; j++)
                {
                    v = tabla_ertek[i, j] + v;
                    if (v > 0 & tabla_ertek[i, j] == 0 || j == nagysag - 1 & tabla_ertek[i, nagysag - 1] != 0)
                    {
                        vizszintes[i].Text = vizszintes[i].Text + " " + v;
                        v = 0;
                    }
                    f = tabla_ertek[j, i] + f;
                    if (f > 0 & tabla_ertek[j, i] == 0 || j == nagysag - 1 & tabla_ertek[nagysag - 1, i] != 0)
                    {
                        fuggoleges[i].Text = fuggoleges[i].Text + '\n' + f;
                        f = 0;
                    }
                }
            }

        }
        public void button1_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
            button1.Enabled = false;
            button2.FlatStyle = FlatStyle.Popup;
            button1.FlatStyle = FlatStyle.Standard;
            button1.Text = "";
            button2.Text = "⬛";


            x = true;
        }

        public void button2_Click(object sender, EventArgs e)
        {

            button2.Enabled = false;
            button1.Enabled = true;
            button1.FlatStyle = FlatStyle.Popup;
            button2.FlatStyle = FlatStyle.Standard;
            button2.Text = "";
            button1.Text = "X";
            x = false;



        }
        public void vege()
        {


            if (hiba == 0)
            {
                label1.Visible = true;
                label1.Text = "Vesztettél";
                for (int i = 0; i < nagysag; i++)
                {
                    for (int j = 0; j < nagysag; j++)
                    {
                        tabla[i, j].Enabled = false;
                    }

                }
            }
            if (db == szines)
            {
                label1.Visible = true;
                label1.Text = "Nyertél!";
                if (random == false)
                {
                    button6.Visible = true;
                }

                if (lepesek == 4)
                {
                    this.Controls.Remove(button1);
                    this.Controls.Remove(button2);
                    this.Controls.Remove(button6);
                    for (int i = 0; i < 15; i++)
                    {
                        this.Controls.Remove(vizszintes[i]);
                        this.Controls.Remove(fuggoleges[i]);
                        for (int j = 0; j < 15; j++)
                        {
                            this.Controls.Remove(tabla[i, j]);
                        }
                    }
                    PictureBox nagykep = new PictureBox();
                    nagykep.SizeMode = PictureBoxSizeMode.StretchImage;
                    nagykep.Left = 100;
                    nagykep.Top = 100;
                    nagykep.Width = 300;
                    nagykep.Height = 300;
                    Bitmap kep = new Bitmap("5.png");
                    nagykep.Image = (Image)kep;
                    this.Controls.Add(nagykep);
                }

                for (int i = 0; i < nagysag; i++)
                {
                    for (int j = 0; j < nagysag; j++)
                    {
                        tabla[i, j].Enabled = false;
                    }

                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Visible = false;
            button4.Visible = false;

            konnyu.Visible = true;
            kozepes.Visible = true;
            nehez.Visible = true;
            konnyu.Left = 271;
            kozepes.Left = 271;
            nehez.Left = 271;


        }

        private void konnyu_Click(object sender, EventArgs e)
        {
            nagysag = 5;
            konnyu.Visible = false;
            kozepes.Visible = false;
            nehez.Visible = false;
            button5.Visible = true;
            random = true;
            alap();


        }

        private void kozepes_Click(object sender, EventArgs e)
        {
            nagysag = 10;
            konnyu.Visible = false;
            kozepes.Visible = false;
            nehez.Visible = false;
            button5.Visible = true;
            random = true;
            alap();
        }

        private void nehez_Click(object sender, EventArgs e)
        {
            nagysag = 15;
            konnyu.Visible = false;
            kozepes.Visible = false;
            nehez.Visible = false;
            button5.Visible = true;
            random = true;
            alap();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button3.Visible = false;
            button4.Visible = false;
            button5.Visible = true;
            button7.Visible = true;
            numericUpDown1.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            szines = 0;
            x = true;
            hiba = 3;
            db = 0;
            random = false;
            kepkocka_f = 0;
            kepkocka_v = 0;
            lepesek = 0;
            InitializeComponent();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 15; i++)
            {
                this.Controls.Remove(vizszintes[i]);
                this.Controls.Remove(fuggoleges[i]);
                for (int j = 0; j < 15; j++)
                {
                    this.Controls.Remove(tabla[i, j]);
                }
            }
            for (int i = 0; i < 3; i++)
            {
                this.Controls.Remove(elet[i]);
            }
            szines = 0;
            x = true;
            hiba = 3;
            db = 0;
            random = false;
            button5.Visible = true;
            button6.Visible = false;
            label1.Visible = false;


            if (lepesek % 2 != 0)
            {
                kepkocka_v = 15;
            }
            else if (lepesek != 0 && lepesek % 2 == 0)
            {
                kepkocka_f = 15;
                kepkocka_v = 0;
            }
            lepesek++;
            alap();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            kep_szama = Convert.ToInt32(numericUpDown1.Value) + 4;
            button7.Visible = false;
            numericUpDown1.Visible = false;
            szines = 0;
            db = 0;
            alap();
        }

        private void cella_kereses()
        {
            if ((Cursor.Position.Y -80) / 50>=0 && (Cursor.Position.Y - 80) / 50<nagysag)
            {
                koordinata[1, 0] = (Cursor.Position.Y  -80)/ 50;
            }

            if ((Cursor.Position.X -100) / 50 >= 0  && (Cursor.Position.X -100) / 50 <nagysag)
            {
                koordinata[1, 1] = (Cursor.Position.X -100) / 50;
            }
            
        }

        
        private void hosszulepesek()
        {
            int hossz = 0;
            if (koordinata[1, 1] > koordinata[0, 1])
            {
                hossz = koordinata[1, 1] - koordinata[0, 1];
                for (int i = 0; i <= hossz; i++)
                {
                    actions(koordinata[0, 0], koordinata[0, 1] + i);
                }
                hossz = 0;


            }
            else if (koordinata[0, 1] > koordinata[1, 1])
            {
                hossz = koordinata[0, 1] - koordinata[1, 1];
                for (int i = 0; i <= hossz; i++)
                {
                    actions(koordinata[0, 0], koordinata[1, 1] + i);
                }
                hossz = 0;


            }


            if (koordinata[0, 0] > koordinata[1, 0])
            {
                hossz = koordinata[0, 0] - koordinata[1, 0];
                for (int i = 0; i <= hossz; i++)
                {
                    actions(koordinata[1, 0] + i, koordinata[1, 1]);

                }
                hossz = 0;

            }
            else if (koordinata[0, 0] < koordinata[1, 0])
            {
                hossz = koordinata[1, 0] - koordinata[0, 0];
                for (int i = 0; i < hossz; i++)
                {
                    actions(koordinata[0, 0] + i, koordinata[1, 1]);

                }
                hossz = 0;



            }

        }

        

        //this.TopMost = true;
        //this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        //this.WindowState = System.Windows.Forms.FormWindowState.Maximized;

    }
}
