using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DovizBar
{
    public partial class Form4 : Form
    {
        public static Form4 form4;
        private Screen scr;
        private int position;
        private FontStyle updatedCurrencyFont = FontStyle.Underline;

        public Form4()
        {
            form4 = this;
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            scr = Screen.FromPoint(this.Location);
            this.Location = new Point(scr.WorkingArea.Right - this.Width, scr.WorkingArea.Bottom - this.Height);
        }

        public virtual void Change_Currency_Text(string dolar, string euro)
        {
            txtDolar.Text = dolar;
            txtEuro.Text = euro;
        }

        public virtual void Change_Currency_Color(Color dolar, Color euro)
        {
            txtDolar.ForeColor = dolar;
            txtEuro.ForeColor = euro;
        }


        private void txtClose_MouseHover(object sender, EventArgs e)
        {
            txtClose.ForeColor = Color.Red;
        }

        private void txtClose_MouseLeave(object sender, EventArgs e)
        {
            txtClose.ForeColor = Color.White;
        }

        private void txtClose_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1.form1.Show();
        }

        private void Form4_DoubleClick(object sender, EventArgs e)
        {
            position++;
            PositionChanger();
        }

        private void PositionChanger()
        {
            if (position % 2 == 0)
                Location = new Point(scr.WorkingArea.Right - Width, scr.WorkingArea.Bottom - Height);
            else if (position % 2 == 1)
                Location = new Point(scr.WorkingArea.Left, scr.WorkingArea.Bottom - Height);
        }

        private void txtEuro_TextChanged(object sender, EventArgs e)
        {
            //txtEuro.Font.Style = updatedCurrencyFont;
        }
    }
}
