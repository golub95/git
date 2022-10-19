using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HRKladeCalc
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (double.TryParse(tbKoef1.Text, out double koef1) && double.TryParse(tbKoef2.Text, out double koef2) && double.TryParse(tbUlog.Text, out double ulog))
            {
                double izracun1 = 1 / (0.95 * koef1 - 0.095);
                double izracun2 = 1 / (0.95 * koef2 - 0.095);

                lbPostoProfit.Text = ((1 - (izracun1 + izracun2)) * 100).ToString() + " %";

                double uplata1 = ulog * izracun1;
                double uplata2 = ulog * izracun2;

                lbUplata1.Text = uplata1.ToString();
                lbUplata2.Text = uplata2.ToString();

                /*
                double z1 = (0.95 * x * y - 0.095 * y);
                lbuplata1Text.Text = (z1).ToString();
                double calc = z1 / Math.Pow((0.95 * x - 0.095), 2);
                label14.Text = (calc).ToString();
                label15.Text = ((0.95 * x * calc - 0.095 * calc)).ToString();*/
            }
        }
    }
}
