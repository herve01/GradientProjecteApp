using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TpRaoGradienProjecte
{
    public partial class Form_Trace_Algo : Form
    {
        public Form_Trace_Algo(String str)
        {
            InitializeComponent();
            webBrowser1.DocumentText = str;
        }

        private void Form_Trace_Algo_Load(object sender, EventArgs e)
        {

        }
    }
}
