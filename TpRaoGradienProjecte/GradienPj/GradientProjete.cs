using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TpRaoGradienProjecte.GradienPj
{
    public class GradientProjete
    {
        public ProblemeMathematique PM { get; set; }

        private int VarCount
        {
            get
            {
                return PM.FonctEconomique.Vars.Count;
            }
        }

        public SortedList<string, double> PointInitial { get; set; }

        public SortedList<string, double> SolutionOptimale
        {
            get
            {
                return Optimiser();
            }
        }

        private List<Matrice> VecteursHyperplan { get; set; }
        private Matrice Matr_A { get; set; }
        private Matrice Matr_P { get; set; }
        private List<int> IndiceHyperPlan { get; set; }
        private List<int> Matr_I_Bar { get; set; }
        private Matrice Direction_Z { get; set; }

        private Matrice PrCritere { get; set; }
        private Matrice DxCritere { get; set; }


        public string str { get; set; }

        public GradientProjete(ProblemeMathematique pm, SortedList<string, double> pointInitial)
        {
            str = "<html><head><link type=\"text/css\" rel=\"stylesheet\" href=\"" + Directory.GetCurrentDirectory() + "\\Styles.css\"/></head><body><div id=\"idEntete\">PROGRAMMATION NON LINEAIRE A CONTRAINTE LINEAIRES </div>";

            str += "<p id=\"textZ\">Min Z=" + pm.FonctEconomique.Format + "</p>";

            str += "<table>";
            foreach (Polynome poly in pm.Contraintes)
            {
                str += "<tr><td id=\"contrainte\">" + poly.Format + " ≤ 0 </td></tr>";
            }
            str += "</table>";

            str += "<p id=\"text\">Résolution : Méthode du gradient projecté</p>";
            PM = pm;
            PointInitial = pointInitial;
            VecteursHyperplan = new List<Matrice>();
            IndiceHyperPlan = new List<int>();
            Matr_I_Bar = new List<int>();

            foreach (Polynome poly in PM.Contraintes)
            {
                Matrice hyperplan = new Matrice(1, PM.FonctEconomique.Variables.Count);

                int k = 0;

                foreach (Variable varX in poly.Variables.Values)
                {
                    bool found = false;

                    for (int i = 0; i < varX.Count; i++)
                    {
                        if (varX[i] == 1)
                        {
                            hyperplan.Mat[0, k] = -poly.Coefficients[i];
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                        hyperplan.Mat[0, k] = 0;

                    k++;

                }

                VecteursHyperplan.Add(hyperplan);
            }

        }


        string str_premierephase = ""; //cette recuper les données dans la methode tracePremierePhase;

        private SortedList<string, double> Optimiser()
        {
            SortedList<string, double> SolOpt = new SortedList<string, double>();

            string pointOptimal = "";

            bool calculable = PremierePhase();

            while ((!PrCritere.EstNulle() || !DxCritere.EstDefiniePositive()) && calculable)
            {
                str += str_premierephase;

                Matrice GradientDeX = PM.FonctEconomique.Gradient().De(PointInitial);

                string ptin = "";

                foreach (double ptt in PointInitial.Values)
                {
                    ptin += ptin == "" ? "" + ptt : "  " + ptt;
                }

                Matrice GradientDeX_Tr = GradientDeX - 2;
                str += "<div class=\"phase\"> Phase II : Test d'optimalité </div>";

                str += dessinerTableHtml(PrCritere, "(*) Pj(g(X°))^t = 0");

                str += dessinerTableHtml(DxCritere, "(**) (Aj*Ajt)^-1) Aj*(g(Xo))^t ≥ 0");

                str += "<p class =\"err\"> (*) et (**) n'etant pas tous deux respectés le point X* = ( " + ptin + ") n'est donc pas une solution optimale </p>";

                str += "<p class=\"phase\">Phase III : Choix de la nouvelle direction </p>";

                str += dessinerTableHtml((((-1.0) * Matr_A) * GradientDeX_Tr), " ° -Aj(g(xo))^t = ");

                if ((((-1.0) * Matr_A) * GradientDeX_Tr).EstDefiniePositive())
                {
                    Direction_Z = (-1.0) * GradientDeX_Tr;
                    str += "<p id=\"text\"> > 0 </p>";
                }

                else if (!(Matr_P * GradientDeX_Tr).EstNulle())
                {
                    //Dir_Z = Methodes.MultiplicationMatrices(-1, GradientDeX_Tr);
                    Direction_Z = ((-1.0) * Matr_P) * GradientDeX_Tr;
                    str += "<p id=\"text\"> < 0 </p>";
                }

                else
                {
                    //str += dessinerTableHtml(Direction_Z, " °° Pj(g(X°))^t = ");

                    str += "<p id=\"text\"> °°° Z = -Pj-1(g(Xo))^t  </p>";

                    Matrice Matr_y = ((Matr_A - 2) - 1) * GradientDeX_Tr;

                    str += dessinerTableHtml(Matr_y, "y = (Aj^t)^-1) g(X°)^t");

                    int q = 0;
                    double yq = 0;

                    str += "<p id=\"text\"> Yq= min {Yq / Qq < 0 } </p>";

                    for (int i = 0; i < Matr_y.Mat.GetLength(0); i++)
                    {
                        for (int j = 0; j < Matr_y.Mat.GetLength(1); j++)
                        {
                            if (Matr_y.Mat[i, j] < 0 && Matr_y.Mat[i, j] < yq)
                            {
                                yq = Matr_y.Mat[i, j];
                                q = i;

                                str += "<p id=\"text\"> Y<sub>" + q + "</sub> " +"= " + yq + " </p>";
                            }
                        }
                    }

                    str += "<p id=\"text\"> Yq= min(y)= " + yq + " </p>";

                    str += "<p id=\"text\"> Position(" + yq + "= " + q + " </p>";

                    str += "<p id=\"text\"> On enlève la ligne de la Position(" + yq + ") à la matrice Aj </p>";

                    Matrice Matr_A_Moins_Un = new Matrice(Matr_A.Mat.GetLength(0) - 1, Matr_A.Mat.GetLength(1));

                    int k = 0; //Indexeur de la position d'insertion dans Matr_A_Moins_Un
                    for (int i = 0; i < Matr_A.Mat.GetLength(0); i++)
                    {
                        if (i != q)
                        {
                            for (int j = 0; j < Matr_A.Mat.GetLength(1); j++)
                            {
                                Matr_A_Moins_Un.Mat[k, j] = Matr_A.Mat[i, j];
                            }
                            k++;
                        }
                    }

                    str += dessinerTableHtml(Matr_A_Moins_Un, "A<sub>j-1</sub> = ");

                    Matrice Matr_A_Moins_Un_Tr = Matr_A_Moins_Un - 2;
                    Matrice Matr_P_Moins_Un = Methodes.MatriceUnite(PM.FonctEconomique.Vars.Count) - (Matr_A_Moins_Un_Tr * ((Matr_A_Moins_Un * Matr_A_Moins_Un_Tr) - 1) * Matr_A_Moins_Un);

                    str += dessinerTableHtml(Matr_P_Moins_Un, "Pj-1 = I<sub>" + PM.FonctEconomique.Vars.Count + "</sub> - <span style=\" text-decoration : overline\">(Pj-1)</span>");

                    Direction_Z = ((-1.0) * (Matr_P_Moins_Un)) * (GradientDeX_Tr);

                    str += dessinerTableHtml(Direction_Z, "°°° Z = -Pj-1(g(Xo))^t =");
                }
                int indice = 0;
                double lamda = double.MaxValue;

                str += "<p class=\"phase\">Phase IV : Determination de nouveau point admissible X<sup>k+1</sup> meilleur que X<sup>K</sup></p>";

                str += "<p id=\"text\"> X* = X° + γ*Z </p>";

                str += "<p id=\"text\"> γ* = min γq </p>";

                foreach (int i in Matr_I_Bar)
                {
                    double lamdaTampon = PM.Contraintes[i].De(PointInitial) / Methodes.DeterminantMatrice(VecteursHyperplan[i] * Direction_Z);

                    if (lamdaTampon >= 0)
                    {
                        lamda = Math.Min(lamda, lamdaTampon);
                        indice = i;
                    }
                }

                str += "<p id=\"text\"> γ<sub>" + indice + "</sub> = (h<sub>" + indice + "</sub>(X°)) / α<sub>" + indice + " </sub>* Z </p>";

                str += "<p id=\"text\"> γ<sub>" + indice + "</sub> = " + lamda + " </p>";

                Matrice newXPondDir = lamda * Direction_Z;

                str += "<p id=\"text\">X1*  = X° + γ*Z = </p>";

                SortedList<string, double> newPoint = new SortedList<string, double>();
                str += "<table>";
                for (int i = 0; i < PointInitial.Count; i++)
                {
                    str += "<tr><td id=\"MTitle\">" + (PointInitial.Values[i] + newXPondDir.Mat[i, 0]) + "</td></tr>";
                    newPoint.Add(PointInitial.Keys[i], PointInitial.Values[i] + newXPondDir.Mat[i, 0]);
                }

                str += "</table>";

                Matrice gradientT = PM.FonctEconomique.Gradient().De(newPoint) * Direction_Z;

                str += dessinerTableHtml(gradientT, "g(X1*)Z = ");

                if (!gradientT.EstDefiniePositive())
                {
                    PointInitial = newPoint;
                }

                else
                {
                    str += "<p id=\"text\"> X<sub>1</sub> = t*X° + (1 - t)X1* </p>";

                    double t = ((-1.0) * gradientT.Mat[0, 0] / ((GradientDeX * Direction_Z) - gradientT).Mat[0, 0]);

                    str += "<p id=\"text\"> t  = -g(X1*)Z/(g(X°)Z - g(X1*)Z) = " + t + "</p>";

                    SortedList<string, double> newPoint2 = new SortedList<string, double>();

                    str += "<p id=\"text\">X1 = </p><table>";

                    pointOptimal = "";

                    for (int i = 0; i < PointInitial.Count; i++)
                    {
                        newPoint2.Add(PointInitial.Keys[i], t * PointInitial.Values[i] + (1 - t) * newPoint.Values[i]);
                        str += "<tr><td id=\"MTitle\">" + (t * PointInitial.Values[i] + (1 - t) * newPoint.Values[i]) + "</td></tr>";
                        pointOptimal += pointOptimal == "" ? ""+(t * PointInitial.Values[i] + (1 - t) * newPoint.Values[i]) : " ;  " + (t * PointInitial.Values[i] + (1 - t) * newPoint.Values[i]);
                    }
                    str += "</table>";
                    PointInitial = newPoint2;
                }

                calculable = PremierePhase();
            }
            str += "<p class=\"valide\"> les deux conditions sur (*) et (**) étant vérifiées les point   X* =(" + pointOptimal + ")  est donc la solution optimal du PM </p>";
            str += "</body></html>";
            return PointInitial;
        }

        private bool PremierePhase()
        {
            bool correct = true;

            IndiceHyperPlan.Clear();
            Matr_I_Bar.Clear();
            str += "";

            for (int i = 0; i < PM.Contraintes.Count; i++)
            {
                if (PM.Contraintes[i].De(PointInitial) == 0)
                    IndiceHyperPlan.Add(i);
                else
                    Matr_I_Bar.Add(i);

            }
            if (IndiceHyperPlan.Count == 0)
            {
                correct = false;
            }
            else
            {
                Matr_A = new Matrice(IndiceHyperPlan.Count, PM.FonctEconomique.Vars.Count);

                int k = 0; //Position d'insertion dans la matrice

                foreach (int i in IndiceHyperPlan)
                {

                    for (int j = 0; j < PM.FonctEconomique.Vars.Count; j++)
                    {
                        Matr_A.Mat[k, j] = VecteursHyperplan[i].Mat[0, j];
                    }

                    k++;
                }


                Polynome[] gradient = PM.FonctEconomique.Gradient();

                Matrice GradientDeX = PM.FonctEconomique.Gradient().De(PointInitial);

                Matrice GradientDeX_Tr = GradientDeX - 2; //Transposer de la matrice

                Matrice Matr_A_Tr = Matr_A - 2; //Transposer de la matrice

                Matr_P = Methodes.MatriceUnite(VarCount) - (Matr_A_Tr * ((Matr_A * Matr_A_Tr) - 1) * Matr_A); // (Pjo) = At(A*At)-1 A

                PrCritere = Matr_P * GradientDeX_Tr;

                DxCritere = ((Matr_A * Matr_A_Tr) - 1) * Matr_A * GradientDeX_Tr;

                str_premierephase = TracePremierePhase(PointInitial, PM, Matr_A, Matr_P);
            }

            return correct;
        }


        string TracePremierePhase(SortedList<string, double> PointInitial, ProblemeMathematique pm, Matrice A, Matrice P)
        {
            string ch = "</br><div class=\"phase\">Phase I</div></br>";

            string valeur = "";

            foreach (double pt in PointInitial.Values)
            {
                valeur += valeur == "" ? "" + pt : ", " + pt;
            }
            ch += "<div id=\"text\">X° = (" + valeur + ") </div>";

            valeur = "";

            int i = 0;
 
            foreach (Polynome poly in pm.Contraintes)
            {
                ch += "<p id=\"text\">h<sub>" + (++i) + "</sub> (X)   = " + poly.Afficher() + " </p>";
               try
               {
                   valeur += "" + (poly.Coefficients[0] + "     " + poly.Coefficients[1]);   
               }
               catch (Exception)
               {
                   
                   valeur += "" + (poly.Coefficients[0] + "     " +  "0" );   
               }
             
                valeur += ";";

            }


            String[] coeffhyper = valeur.Split(';');
            i = 0;
            foreach (string item in coeffhyper)
            {
                if (item.Trim() != string.Empty)
                {
                    ch += "<p id=\"text\">α<sub>" + (++i) + "</sub>(X°) = ( " + item + ")</p>";
                }  
            }

            //dessiner la matrice Aj
            ch += "<p id=\"text\"> A<sub>j</sub>° = </p><table>";

            for (int l = 0; l < A.Mat.GetLength(0); l++)
            {
                ch += "<tr>";
                for (int c = 0; c < A.Mat.GetLength(1); c++)
                {
                    ch += "<td id=\"MTitle\">" + A.Mat[l, c] + "</td>";
                }
                ch += "</tr>";
            }

            ch += "</table>";

            //dessiner la matrice P
            ch += "<p id=\"text\">P<sub>j</sub>° = </p><table>";

            for (int l = 0; l < P.Mat.GetLength(0); l++)
            {
                ch += "<tr>";
                for (int c = 0; c < P.Mat.GetLength(1); c++)
                {
                    ch += "<td id=\"MTitle\">" + P.Mat[l, c] + "</td>";
                }
                ch += "</tr>";
            }

            ch += "</table>";

            //Gradient de G(x)
            ch += "<p id=\"text\">Le gradient de f G(x) = ∇f(X) </p>";
            ch += "<p id=\"text\">Le gradient de f G(X)  =  (" + pm.FonctEconomique.Gradient()[0].AfficherPol()+ "\t\t" + pm.FonctEconomique.Gradient()[1].AfficherPol() + ") </p>";

            valeur = "";
            foreach (double item in pm.FonctEconomique.Gradient().De(PointInitial).Mat)
            {
                valeur += valeur == "" ? "" + item : "                   " + item;
            }

            //Gradient de G(X0)
            ch += "<p id=\"text\">Le gradient de f G(X°) = (" + valeur + ")</p>";

            return ch;
        }


        string dessinerTableHtml(Matrice Matr, string libelle)
        {
            string ch = "";

            //dessiner la matrice Aj
            ch += "<p id=\"text\">" + libelle + " </p><table>";

            for (int i = 0; i < Matr.Mat.GetLength(0); i++)
            {
                ch += "<tr>";
                for (int j = 0; j < Matr.Mat.GetLength(1); j++)
                {
                    ch += "<td id=\"MTitle\">" + Matr.Mat[i, j] + "</td>";
                }
                ch += "</tr>";
            }

            ch += "</table>";

            return ch;
        }

    }
}
