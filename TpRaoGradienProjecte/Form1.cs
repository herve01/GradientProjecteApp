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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            string[] typeOpt = {"Minimiser"};
            CmbTypeOpt.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            CmbTypeOpt.AutoCompleteSource = AutoCompleteSource.CustomSource;
            CmbTypeOpt.Items.AddRange(typeOpt);
            CmbTypeOpt.AutoCompleteCustomSource.AddRange(typeOpt);
            CmbTypeOpt.Text = typeOpt[0];
        }

        bool verifierChamp()
        {
            bool ver = false;
            foreach (TextBox cntrl in panel1.Controls.OfType<TextBox>())
            {
                if (cntrl.Text.Equals(string.Empty))
                {
                    ver = true;
                    break;
                }
            }
            return ver;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        int nbreVar = 0;

        private void btopt_Click(object sender, EventArgs e)
        {
            ProblemeMathematique pm = new ProblemeMathematique();
            try
            {
                if (verifierChamp())
                {
                    MessageBox.Show("Veillez remplir tous les champ correctement !!");
                }
                else
                {
                    string variables = "";
                    for (int i = 0; i < nbreVar; i++)         //cette boucle permet de recuperer les variables que nous avons dans notre PM
                    {
                        variables += variables == "" ? var[i] : ", " + var[i];
                    }

                    //
                    string contrainte = "";
                    int t = 0;
                    string OperateurComp = "";

                    foreach (Control cntrl in PnlDessin.Controls)
                    {
                        Polynome poly;

                        if (cntrl is TextBox)
                        {
                            if (cntrl.Name == "TxtFonctionEcon")
                            {
                                poly = new Polynome(cntrl.Text + " | " + variables);
                                pm.FonctEconomique = poly;
                            }
                            else if (cntrl.Name == "txtIndep")
                            {
                                //poly = new Polynome(cntrl.Text + " | " + variables);  
                                
                                contrainte += cntrl.Text == "0" ? "" : (int.Parse(cntrl.Text) >= 0 ? "-" + cntrl.Text.Trim() : cntrl.Text.Trim().Replace('-', '+'));
                                
                                if (OperateurComp.Trim() == "≥") //Ici on met la contrainte sous la forme normale
                                {
                                    contrainte=NormaliserContrainte(contrainte);
                                } 

                                poly = new Polynome(contrainte + " | " + variables);

                                pm.Contraintes.Add(poly);
                                contrainte = "";
                                t = 0;
                            }

                            else
                            {
                               
                                contrainte+=contrainte=="" ? (cntrl.Text == "0" ? "" : (cntrl.Text == "1" ? var[t] : (cntrl.Text == "-1" ? "-" + var[t] : cntrl.Text + "" + var[t]))) : (cntrl.Text.Trim() == "0" ? "" : (int.Parse(cntrl.Text) > 0 ? (cntrl.Text.Trim()=="1" ? " + "+ var[t] : " + "+cntrl.Text +""+ var[t]): (cntrl.Text.Trim()=="-1" ?  var[t] : " "+cntrl.Text +""+ var[t])));
                                t++;
                            }
                        }
                        else if (cntrl is ComboBox)
                            OperateurComp = cntrl.Text.Trim();
                      
                    }
                    new PointDepart(pm, nbreVar).Show();
                }
            }
            catch (Exception)
            {
                
                throw;
            }   
        }

        string NormaliserContrainte(string contrainte)
        {
            string str = "";
            foreach (char c in contrainte)
            {
                str += str == "" ? (c == '-' ? "" : "-" + c) : (c == '+' ? "-" : (c == '-' ? "+" : "" + c));
            }

            return str;
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

        bool formatFonctEco(string s)
        {
            Regex Rg = new Regex("[-+]|[0-9\\b]");
            return Rg.IsMatch(s.ToString());
        }

        string[] var = { "X", "Y", "Z", "T", "V", "W" };
        private void Btcreer_Click(object sender, EventArgs e)
        {

            if (txtNbreCont.Text==string.Empty | txtNbreVar.Text.Equals(string.Empty))
            {
                MessageBox.Show("veillez que tous soient remplit correctement !!");
            }
            else
            {
                this.PnlDessin.Controls.Clear();

                nbreVar = Convert.ToInt16(txtNbreVar.Text);
                int nbreCont = Convert.ToInt16(txtNbreCont.Text);
                string[] symbol = { "≤", "≥" };

                TextBox txtBox;
                Label label;
                ComboBox cmbFormCont;

                int newAbs = 70, newOrd = 50;

                Size dimChamp = new Size(70, 30);
                Size dimChampZ = new Size(80 + nbreVar * 2 * 80, 30);
                Size dimLabel = new Size(50, 30);
                Size dimCombo = new Size(50, 30);
                Size dimLabelz = new Size(300, 30);


                label = new Label();
                label.Text = "Fonction économique";
                label.Size = dimLabelz;
                label.FlatStyle = FlatStyle.Flat;
                label.BorderStyle = BorderStyle.None;
                label.Font = new System.Drawing.Font("Segoe UI Light", 16);
                label.Location = new System.Drawing.Point(5, 5);
                PnlDessin.Controls.Add(label);

                label = new Label();
                label.Text = CmbTypeOpt.Text.Trim() == "Minimiser" ? "Min Z=" : "Max Z=";
                label.Size = dimLabel;
                label.FlatStyle = FlatStyle.Flat;
                label.BorderStyle = BorderStyle.FixedSingle;
                label.Font = new System.Drawing.Font("Times New Roman", 8);
                label.Location = new System.Drawing.Point(5, newOrd);
                PnlDessin.Controls.Add(label);

                txtBox = new TextBox();
                txtBox.Size = dimChampZ;
                txtBox.Name = "TxtFonctionEcon";
                //txtBox.KeyPress += new KeyPressEventHandler(txtBox_KeyPress);
                txtBox.BorderStyle = BorderStyle.FixedSingle;
                txtBox.Font = new System.Drawing.Font("Times New Roman", 8);
                txtBox.Location = new System.Drawing.Point(newAbs, newOrd);
                PnlDessin.Controls.Add(txtBox);
                newOrd += dimLabel.Height + 10;

                label = new Label();
                label.Text = "Contraintes";
                label.Size = dimLabelz;
                label.FlatStyle = FlatStyle.Flat;
                label.BorderStyle = BorderStyle.None;
                label.Font = new System.Drawing.Font("Segoe UI Light", 16);
                label.Location = new System.Drawing.Point(5, newOrd);
                PnlDessin.Controls.Add(label);

                newOrd += dimLabel.Height + 10;

                newAbs = 70;
                int l = 0, c = 0;
                for (int i = 0; i < nbreCont + nbreVar; i++)
                {
                    l += i < nbreCont ? 0 : 1;

                    for (int j = 0, indVar = 0; j < nbreVar * 2; j++)
                    {
                        if ((j + 1) % 2 != 0)
                        {
                            c += i < nbreCont ? 0 : 1;
                            //Champ pour le terme aij coefficient de X
                            txtBox = new TextBox();
                            txtBox.Size = dimChamp;
                            txtBox.BorderStyle = BorderStyle.FixedSingle;
                            txtBox.Font = new System.Drawing.Font("Times New Roman", 8);
                            txtBox.Location = new System.Drawing.Point(newAbs, newOrd);
                            txtBox.KeyPress += new KeyPressEventHandler(textBox_KeyPress);
                            txtBox.Leave += new EventHandler(txtBox_Leave);
                            txtBox.GotFocus += new EventHandler(txtBox_GotFocus);
                            txtBox.Text = i < nbreCont ? "" : ((c == l ? "-1" : "0"));
                            txtBox.Enabled = i < nbreCont ? true : false;
                            newAbs += dimChamp.Width + 10;
                            PnlDessin.Controls.Add(txtBox);
                        }
                        else
                        {
                            label = new Label();
                            label.Font = new System.Drawing.Font("Times New Roman", 8);
                            label.FlatStyle = FlatStyle.Flat;
                            label.BorderStyle = BorderStyle.FixedSingle;
                            label.Text = "+ " + var[indVar++];
                            label.Size = dimLabel;
                            label.Location = new System.Drawing.Point(newAbs, newOrd);
                            newAbs += dimLabel.Width + 10;
                            PnlDessin.Controls.Add(label);
                        }

                    }
                    c = 0;

                    //Le combo pour forme des contreiantes
                    cmbFormCont = new ComboBox();
                    cmbFormCont.Size = dimCombo;
                    cmbFormCont.Font = new System.Drawing.Font("Times New Roman", 8);
                    cmbFormCont.FlatStyle = FlatStyle.Flat;
                    cmbFormCont.Location = new System.Drawing.Point(newAbs, newOrd);
                    cmbFormCont.Items.AddRange(symbol);
                    cmbFormCont.Text = i < nbreCont ? "" : "≤";
                    cmbFormCont.Enabled = i < nbreCont ? true : false;
                    newAbs += cmbFormCont.Width + 5;
                    PnlDessin.Controls.Add(cmbFormCont);
                    //

                    //Le champ pour le terme independant
                    txtBox = new TextBox();
                    txtBox.Size = dimChamp;
                    txtBox.Name = "txtIndep";
                    txtBox.BorderStyle = BorderStyle.FixedSingle;
                    txtBox.Font = new System.Drawing.Font("Times New Roman", 8);
                    txtBox.Location = new System.Drawing.Point(newAbs, newOrd);
                    txtBox.KeyPress += new KeyPressEventHandler(textBox_KeyPress);
                    txtBox.Leave += new EventHandler(txtBox_Leave);
                    txtBox.GotFocus += new EventHandler(txtBox_GotFocus);
                    txtBox.Text = i < nbreCont ? "" : "0";
                    txtBox.Enabled = i < nbreCont ? true : false;
                    PnlDessin.Controls.Add(txtBox);
                    //

                    newAbs = 70;
                    newOrd += dimLabel.Height + 5;
                }
            }
        }

        void txtBox_GotFocus(object sender, EventArgs e)
        {
            TextBox txtleave = sender as TextBox;
                txtleave.BackColor = Color.FromArgb(255, 255, 255);
        }

        void txtBox_Leave(object sender, EventArgs e)
        {
            TextBox txtleave = sender as TextBox;
            int valTest=0;
            if (!int.TryParse(txtleave.Text, out valTest) )
            {
                txtleave.BackColor = Color.FromArgb(241,243,248);
            }
            else
            {
                txtleave.BackColor = Color.FromArgb(255, 255, 255);
            }

        }

        void txtBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !formatFonctEco(e.KeyChar.ToString());
        }
        private void button1_Click(object sender, EventArgs e)
        {

            ProblemeMathematique pm = new ProblemeMathematique();
            string variables = "";    
            for (int i = 0; i < nbreVar; i++)         //cette boucle permet de recuperer les variables que nous avons dans notre PM
            {
                variables += variables == "" ? var[i] : ", " + var[i];
            }

            //
            string contrainte = ""; //Cette permet de recuperer chaque contrainte dans le formulaire

            int countNbreVariable = 0;

            string symboleComp = "";

            foreach (Control cntrl in PnlDessin.Controls)  //on parcour tout les controles se trouvant dans le formulaire
            {
                Polynome poly;
                if (cntrl is ComboBox)
                {
                    symboleComp = cntrl.Text;
                }
                else if ( cntrl is TextBox)
                {
                    if (cntrl.Name=="TxtFonctionEcon")
                    {
                        poly = new Polynome(cntrl.Text + " | " + variables);
                        pm.FonctEconomique = poly;
                    }
                    else if (cntrl.Name == "txtIndep")
                    {
                        //poly = new Polynome(cntrl.Text + " | " + variables);  
                        contrainte += cntrl.Text=="0" ? "" : (int.Parse(cntrl.Text) >= 0 ? "-" + cntrl.Text.Trim() : cntrl.Text.Trim().Replace('-', '+')); //Ici on veux contruire le polynome avec des coefficient recuperer dans le formulairre
                        poly = new Polynome(contrainte + " | " + variables);
                        pm.Contraintes.Add(poly);
                        contrainte = "";
                        countNbreVariable = 0;
                    }

                    else 
                    {
                        contrainte += contrainte == "" ? (cntrl.Text == "0" ? "" : (cntrl.Text == "1" ? var[countNbreVariable] : (cntrl.Text == "-1" ? "-" + var[countNbreVariable] : cntrl.Text + "" + var[countNbreVariable]))) : (cntrl.Text == "0" ? "" : (cntrl.Text == "1" ? var[countNbreVariable] : (cntrl.Text == "-1" ? "-" + var[countNbreVariable] : (int.Parse(cntrl.Text) >= 0 ? " + " + cntrl.Text.Trim() + "" + var[countNbreVariable] : cntrl.Text.Trim() + "" + var[countNbreVariable]))));     
                        countNbreVariable++;
                    }     
                }
            }

            new PointDepart(pm, nbreVar).Show();
        }

        private void txtNbreCont_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !isNumeric(e.KeyChar.ToString());
        }

        private void txtNbreVar_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !isNumeric(e.KeyChar.ToString());
        }

        bool isNumeric(string s)
        {
            Regex Rg = new Regex("[0-9\\b]");
            return Rg.IsMatch(s.ToString());
        }
    
    }
}
