using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TpRaoGradienProjecte.GradienPj
{
    public static class Methodes 
    {
        public static Polynome Derivee(this Polynome poly, string var)
        {
            Polynome Derivee = new Polynome();

            foreach (Variable varX in poly.Variables.Values)
            {
                Derivee.Variables.Add(varX.Nom, varX.SupVariable());  
            }

            Derivee.Coefficients = poly.Coefficients.SupCoefficients();

            foreach (string item in poly.Vars)
            {
                Derivee.Vars.Add(item);
            }

            Variable varD = Derivee.Variables.Values[Derivee.Variables.IndexOfKey(var)];

            foreach (Variable vars in Derivee.Variables.Values)
            {
                if (vars != varD)
                {
                    for (int i = 0; i < vars.Count; i++)
                    {
                        vars[i] = (vars[i] != 0 && varD[i] != 0) ? vars[i] : 0;
                    }
                }
            }

            for (int i = 0; i < Derivee.Coefficients.Count; i++)
            {
                Derivee.Coefficients[i] *= varD[i];
                varD[i]--;
            }

            int taille = varD.Count;

            for (int i = 0; i < varD.Count; i++)
            {
                if (varD[i] < 0)
                {
                    for (int j = 0; j < Derivee.Variables.Count; j++)
                    {
                        Derivee.Variables.Values[j].RemoveAt(i); 
                    }

                    Derivee.Coefficients.RemoveAt(i);
                    i--;
                }
            }

            return Derivee;
        }

        public static Polynome[] Gradient(this Polynome poly)
        {
            Polynome[] Grad = new Polynome[poly.Vars.Count];

            for (int i = 0; i < poly.Variables.Count; i++)
            {
                Grad[i] = poly.Derivee(poly.Variables.Keys[i]);
            }

            return Grad;
        }

        public static Variable SupVariable(this Variable vars)
        {
            Variable varX = new Variable(vars.Nom);

            foreach (int item in vars)
            {
                varX.Add(item);
            }

            return varX;
        }

        public static List<double> SupCoefficients(this List<double> Coeffs)
        {
            List<double> Coefficients = new List<double>();

            foreach (double item in Coeffs)
            {
                Coefficients.Add(item);
            }

            return Coefficients;
        }

        public static Matrice De(this Polynome[] gradient, SortedList<string, double> point)
        {
            Matrice R = new Matrice(1, gradient.Length);

            double[,] Resultat = R.Mat;

            for (int i = 0; i < gradient.Length; i++)
            {
                Resultat[0, i] = gradient[i].De(point);
            }
            return R;
        }

        public static double DeterminantMatrice(Matrice M)
        {
            double det = 1;
            int n = M.Mat.GetLength(1); //Ordre de la matrice

            int j = 0; //Indexeur de colonne

            while (j < n - 1)
            {
                if (M.Mat[j, j] == 0) //permutation ligne i en ligne k
                {
                    for (int i = j + 1; i < n; i++)
                    {
                        if (M.Mat[i, j] != 0)
                        {
                            for (int k = 0; k < n; k++)
                            {
                                double tampon = M.Mat[j, k];
                                M.Mat[j, k] = -M.Mat[i, k];
                                M.Mat[i, k] = tampon;
                            }

                            break;
                        }
                    }
                }

                for (int i = j + 1; i < n; i++)
                {
                    if (M.Mat[i, j] != 0)
                    {
                        double pivot = -M.Mat[j, j] / M.Mat[i, j];

                        for (int k = 0; k < n; k++)
                        {
                            M.Mat[i, k] = (M.Mat[j, k] + pivot * M.Mat[i, k]) / Math.Abs(pivot);
                        }
                    }
                }

                j++;
            }

            for (int i = 0; i < n; i++)
            {
                det *= M.Mat[i, i];
            }

            return det;
        }


        public static Matrice MatriceUnite(int n)
        {
            Matrice II = new Matrice(n, n);

            double[,] I = II.Mat;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    I[i, j] = i == j ? 1 : 0;
                }
            }

            return II;
        }

        public static bool EstNulle(this Matrice Matr)
        {
            bool ok = true;
            try
            {
                for (int i = 0; i < Matr.Mat.GetLength(0); i++)
                {
                    for (int j = 0; j < Matr.Mat.GetLength(1); j++)
                    {
                        if (Matr.Mat[i, j] != 0)
                        {
                            ok = false;
                            break;
                        }
                    }

                    if (!ok)
                        break;
                }
            }
            catch (Exception)
            {
                return ok;
            }
           

            return ok;
        }

        public static bool EstDefiniePositive(this Matrice Matr)
        {
            bool ok = true;
            try
            {
                for (int i = 0; i < Matr.Mat.GetLength(0); i++)
                {
                    for (int j = 0; j < Matr.Mat.GetLength(1); j++)
                    {
                        if (Matr.Mat[i, j] < 0)
                        {
                            ok = false;
                            break;
                        }
                    }

                    if (!ok)
                        break;
                }

            }
            catch (Exception)
            {
                return ok;
            }
           
            return ok;
        }
        
        public static bool EstScalaire(double[,] Matr)
        {
            return Matr.GetLength(0) == 1 && Matr.GetLength(1) == 1;
        }

        
    }
}
