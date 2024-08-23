using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TpRaoGradienProjecte.GradienPj;
using System.Text.RegularExpressions;

namespace TpRaoGradienProjecte
{
    public partial class PointDepart : Form
    {
        ProblemeMathematique pm;
        public PointDepart(ProblemeMathematique pm, int nbreVar)
        {
            InitializeComponent();
            this.pm = new ProblemeMathematique();
            this.pm = pm;
            Size sizeCntrl = new System.Drawing.Size(30, 30);
            Size sizeCntrtxt = new System.Drawing.Size(100, 30);
            int newAbs = 20, newOrd = 20;
            Label Labl;
            TextBox txtbox;
            
            panel1.Controls.Clear();

            for (int i = 0; i < nbreVar; i++)
            {
                Labl = new Label();
                Labl.Text = var[i] + " : ";
                Labl.Size = sizeCntrl;
                Labl.Font = new System.Drawing.Font("Times New Roman", 8);
                Labl.Location = new System.Drawing.Point(newAbs, newOrd);
                panel1.Controls.Add(Labl);

                txtbox = new TextBox();
                txtbox.Size = sizeCntrtxt;
                txtbox.BorderStyle = BorderStyle.FixedSingle;
                txtbox.Font = new System.Drawing.Font("Times New Roman", 8);
                txtbox.SelectAll();
                txtbox.Text = "0";
                txtbox.Location = new System.Drawing.Point(sizeCntrl.Width + newAbs, newOrd);
                txtbox.KeyPress += new KeyPressEventHandler(textBox_KeyPress); 
                panel1.Controls.Add(txtbox);

                newAbs = 20; newOrd += sizeCntrl.Height;
            }
        }
        private void PointDepart_Load(object sender, EventArgs e)
        {

        }

        void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !IsNumeric(e.KeyChar.ToString());
        }

         bool IsNumeric(string s)
        {
            Regex Rg = new Regex("[-+]|[0-9\\b]");
            return Rg.IsMatch(s.ToString());
        }

        string[] var = { "X", "Y", "Z", "T", "V", "W" };

        private void BtnOpt_Click(object sender, EventArgs e)
        {
            SortedList<string, double> Point = new SortedList<string, double>();
            int i = 0;
            foreach (TextBox item in panel1.Controls.OfType<TextBox>())
            {
                Point.Add(var[i++],  int.Parse(item.Text));
            }
 
            GradientProjete g = new GradientProjete(pm, Point);
            SortedList<string, double> Solution = g.SolutionOptimale;

            new Form_Trace_Algo(g.str.Trim()).Show();
        }
    }
}
