using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TpRaoGradienProjecte.GradienPj
{
    public class Polynome 
    {
        public string Chaine { get; set; }
        public SortedList<string, Variable> Variables;
        public List<string> Vars;
        public int NombreVariables { get; set; }
        public List<double> Coefficients { get; set; }
        public int Degre { get; set; }

        public Polynome()
        {
            Vars = new List<string>();
            Coefficients = new List<double>();
            Variables = new SortedList<string, Variable>();
        }

        public string Format { get; set; }

        public Polynome(string chaine)
        {
            Vars = new List<string>();
            Variables = new SortedList<string, Variable>();
            Coefficients = new List<double>();
            Chaine = FormaterChaine(chaine);
            ConstruirePolynome();
            string[] str=chaine.Split('|');
            Format =str[0];
        }

        public string FormaterChaine(string chaine)
        {
            string[] chaines = chaine.Split('|');
            string ch = "";

            string[] vars = chaines[1].Split(',');

            foreach (string str in vars)
            {
                foreach (char c in str)
                {
                    if (c >= 'A' && c <= 'Z' && !Vars.Contains(c.ToString().ToUpper()))
                        Vars.Add(c.ToString().ToUpper());
                }
            }

            int cpt = 0;
            foreach (char c in chaines[0])
            {
                ch += c.ToString() == " " ? "" : (c.ToString() == "-" ? (cpt == 0 ? c.ToString() : "+" + c.ToString()) : c.ToString());
                cpt++;
            }

            foreach (string varX in Vars)
            {
                Variables.Add(varX, new Variable(varX));
            }

            return ch.ToUpper();
        }

        private void ConstruirePolynome()
        {
            string[] Monomes = Chaine.Split('+');

            int k = 0; //Compteur monômes

            foreach (string monome in Monomes)
            {
                string coeff = "";
                List<double> Coeffs = new List<double>();
                string pow = "";
                string vars = "";
                bool powActive = false;

                for (int i = 0; i < monome.Length; i++)
                {
                    if (monome[i] == '^')
                        powActive = true;

                    if (monome[i] >= 'A' && monome[i] <= 'Z')
                    {
                        if (vars != "")
                        {
                            Variable varX = new Variable(vars.ToUpper());
                            varX.Add(pow == ""? 1 : Convert.ToInt32(pow));

                            if (!Variables.ContainsKey(varX.Nom))
                                Variables.Add(varX.Nom, varX);
                            else
                                Variables.Values[Variables.IndexOfKey(varX.Nom)].Add(pow == "" ? 1 : Convert.ToInt32(pow));
                        }

                        powActive = false;
                        vars = monome[i].ToString().ToUpper();
                        pow = "";
                    }

                    if (monome[i] >= '0' && monome[i] <= '9')
                    {
                        if (powActive)
                            pow += monome[i];

                        else
                            coeff += monome[i];     
                    }

                    if (monome[i] == '*')
                    {
                        powActive = false;
                        Coeffs.Add(coeff == ""? 1 : double.Parse(coeff));
                        coeff = "";
                    }

                    if (monome[i] == '-')
                        coeff += monome[i];

                }

                Coeffs.Add(coeff == "" ? 1 : coeff == "-" ? -1 : double.Parse(coeff));
                double coefficient = 1;

                foreach (double coe in Coeffs)
                {
                    coefficient *= coe;
                }

                Coefficients.Add(coefficient);
                
                if (vars != "")
                {
                    Variable varX = new Variable(vars.ToUpper());
                    varX.Add(pow == "" ? 1 : Convert.ToInt32(pow));

                    if (!Variables.ContainsKey(varX.Nom))
                        Variables.Add(varX.Nom, varX);
                    else
                        Variables.Values[Variables.IndexOfKey(varX.Nom)].Add(pow == "" ? 1 : Convert.ToInt32(pow));
                }

                if (k == 0) //Rajoute d'autres variables non rencontrées dans le premier monômes dans la liste de variables.
                {
                    foreach (string varY in Vars)
                    {
                        if (!Variables.ContainsKey(varY))
                        {
                            Variable varZ = new Variable(varY);
                            varZ.Add(0);

                            Variables.Add(varZ.Nom, varZ);
                        }
                    }
                }

                foreach (Variable varX in Variables.Values)
                {
                    if (varX.Count < Coefficients.Count)
                        varX.Add(0);    
                }

                k++;
            }
            
        }

        public string Afficher()
        {
            string ch = "";

            for (int i = 0; i < Coefficients.Count; i++)
            {
                //ch +=( ch == "" ? (Coefficients[i] == 1 ? "" : Coefficients[i] + "") : 

                //    (Coefficients[i] > 0 ? (Coefficients[i] == 1 ? "+" : "+" + Coefficients[i]) : 
                //    (Coefficients[i] == -1 ? "-" : Coefficients[i] + "")));

                ch += ch =="" ? ""+ Coefficients[i] : (Coefficients[i] > 0 ? "+" + Coefficients[i] : "" + Coefficients[i]);

                ch = ch.Replace('1',' ');

                for (int j = 0; j < Variables.Values.Count; j++) 
                {
                    ch += Variables.Values[j][i] == 0 ? "" : (Variables.Values[j][i] == 1 ? Variables.Values[j].Nom : Variables.Values[j].Nom + "^" + Variables.Values[j][i]);
                }
            }

            return ch;
        }

        public string AfficherPol()
        {
            string ch = "";

            for (int i = 0; i < Coefficients.Count; i++)
            {
                ch += ch == "" ? "" + Coefficients[i] : (Coefficients[i] > 0 ? "+" + Coefficients[i] : "" + Coefficients[i]);

                for (int j = 0; j < Variables.Values.Count; j++)
                {

                    if (Variables.Values[j][i] == 0)
                    {
                        
                    }
                    ch += Variables.Values[j][i] == 0 ? "" : (Variables.Values[j][i] == 1 ? Variables.Values[j].Nom : Variables.Values[j].Nom + "^" + Variables.Values[j][i]);
                }

            }

            return ch;
        }

        public double De(SortedList<string, double> point)
        {
            double resultat = 0;

            if (point.Count == Vars.Count)
            {
                for (int i = 0; i < Coefficients.Count; i++)
                {
                    double monone = Coefficients[i];

                    foreach (Variable varD in Variables.Values)
                    {
                        monone *= Math.Pow(point.Values[point.IndexOfKey(varD.Nom)], varD[i]);
                    }

                    resultat += monone;
                }
            }

            return resultat;
        }

    }
}
