using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Frecuencias
{
    public partial class Form1 : Form
    {
        TableLayoutPanel tp = new TableLayoutPanel();
        public GeneradorMultiplicativoMixto.Form1 f1 = new GeneradorMultiplicativoMixto.Form1();
        public Form1()
        {
            InitializeComponent();
            tp = this.tableLayoutPanel1;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            List<double> nNum = new List<double>();            
            nNum.Clear();
            nNum = gen();
            bool val = f1.validos;
            if (nNum == null)
            {
                return;
            }
            if (nNum.Count == 0)
            {
                MessageBox.Show("Favor de generar los numeros en el generador");
                return;
            }
            if (!val)
            {
                MessageBox.Show("prueba de generar numeros validos desde el generador");
                return;
            }
            listBox1.DataSource = nNum;
            var y = textBox1.Text;
            double fe = 0;
            int x;
            if (int.TryParse(y, out x))
            {
                fe = nNum.Count / Convert.ToDouble(textBox1.Text);
                fe = Math.Round(fe, 5);
            }
            else
            {
                listBox1.DataSource = null;
                MessageBox.Show("Favor de ingresar una frecuencia valida");
                return;
            }
            textBox2.Text = fe.ToString();
            label7.Text = (nNum.Count - 1).ToString();
            textBox4.Enabled = true;
            separacion(ref nNum, ref fe);
            button1.Enabled = false;
            textBox1.Enabled = false;
        }

        public void separacion(ref List<double> nNum, ref double fe)
        {
            if (Convert.ToInt32(textBox1.Text) < 12)
            {
                tableLayoutPanel1.AutoScroll = false;
            }
            else
            {
                tableLayoutPanel1.AutoScroll = true;
            }
            tableLayoutPanel1.Controls.Clear();
            int n = Convert.ToInt32(textBox1.Text);
            tableLayoutPanel1.ColumnCount = n;
            TableLayoutColumnStyleCollection styles = this.tableLayoutPanel1.ColumnStyles;
            foreach (ColumnStyle style in styles)
            {
                style.SizeType = SizeType.Percent;
                style.Width = 100 / n;
            }
            List<rango> nRang = new List<rango>();
            for (int i = 0; i < n; i++)
            {
                rango r = new rango();
                Label lb = new Label();
                lb.Text = fe.ToString();
                Label lab = new Label();
                Label cont = new Label();
                int valor = Convert.ToInt32(textBox5.Text);
                r.min = Math.Round((((i) * fe)/valor), 3);
                r.max = Math.Round((((i + 1) * fe) /valor) , 3);
                lab.Text = r.min.ToString() + " - " + r.max.ToString();
                cont.Text = "0";
                tableLayoutPanel1.Controls.Add(lb, i, 0);
                tableLayoutPanel1.Controls.Add(lab, i, 2);
                tableLayoutPanel1.Controls.Add(cont, i, 1);
                nRang.Add(r);
            }
            foreach (var y in nNum)
            {
                for (int i = 0; i < nRang.Count; i++)
                {
                    if (nRang[i].min <= y && y < nRang[i].max)
                    {
                        Label lab = (Label)tableLayoutPanel1.GetControlFromPosition(i, 1);
                        lab.Text = (Convert.ToInt32(lab.Text) + 1).ToString();
                    }
                }
            }
            label3.Text = nNum.Count.ToString();
            List<sumatoria> nSum = new List<sumatoria>();
            double storia = 0;
            for (int i = 0; i < tableLayoutPanel1.ColumnCount; i++)
            {
                Label lb = (Label)tableLayoutPanel1.GetControlFromPosition(i, 1);
                sumatoria s = new sumatoria();
                s.fo = Convert.ToDouble(lb.Text);
                nSum.Add(s);
                s.r = s.fo - fe;
                s.r = s.r * s.r;
                storia += s.r;
            }
            storia = (1.00 / fe) * storia;
            storia = Math.Round(storia, 5);
            textBox3.Text = storia.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        public class rango
        {
            public double min { get; set; }
            public double max { get; set; }
        }

        public class sumatoria
        {
            public double fo { get; set; }
            public double r { get; set; }
        }

        public void limpiarcontroles()
        {
            listBox1.DataSource = null;
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.ColumnCount = 1;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox4.Enabled = false;
            label3.Text = "";
            label7.Text = "";
            label8.Text = "";
            button1.Enabled = true;
            textBox1.Enabled = true;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            double d;
            bool isD = double.TryParse(textBox4.Text, out d);
            if(!isD)
            {
                if (textBox4.Text != "")
                {
                    MessageBox.Show("El texto ingresado tiene que ser tipo double.");
                }
                textBox4.Text = "";
                return;                
            }
            double xo = Convert.ToDouble(textBox3.Text);
            double z = Convert.ToDouble(textBox4.Text);
            if(xo<z)
            {
                label8.Text = "Xo es menor que z por lo tanto los numeros son aceptados";
            }
            else
            {
                label8.Text = "Xo no es menor que z por lo tanto los numeros no son aceptados";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            limpiarcontroles();
            f1 = new GeneradorMultiplicativoMixto.Form1();
        }

        public List<double> gen()
        {
            int n = Convert.ToInt32(textBox5.Text);
            List<double> gene = new List<double>();
            f1.ShowDialog();
            List<GeneradorMultiplicativoMixto.Form1.item> lItem = f1.gen;
            if (n < lItem.Count)
            {
                for (int i = 0; i < n; i++)
                {
                    gene.Add(lItem[i].NumeroRectangular);
                }
            }
            else
            {
                MessageBox.Show("No se generaron suficientes numeros para el requisito de n");
                gene = null;
            }
            return gene;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            string txt = ((TextBox)sender).Text;
            try
            {
                Convert.ToInt64(txt);
                button1.Enabled = true;
            }
            catch
            {
                button1.Enabled = false;
                return;
            }
        }

    }
}
