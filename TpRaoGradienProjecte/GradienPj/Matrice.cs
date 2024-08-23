using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TpRaoGradienProjecte.GradienPj

{
    public class Matrice
    {
        public double[,] Mat{get;set;}
        int n = 0, m = 0;
        public Matrice(int nbreLigne, int nbreColonne)
        {
            this.n = nbreLigne;
            this.m = nbreColonne;
            Mat=new double[n,m];
        }
        public Matrice()
        {
            this.n = 0;
            this.m = 0;
            Mat = new double[n, m];
        }

        public static Matrice operator *(double scal, Matrice Matr)
        {

            Matrice MatrResultat = new Matrice(Matr.Mat.GetLength(0), Matr.Mat.GetLength(1));

            double[,] Resultat = MatrResultat.Mat;

            for (int i = 0; i < Matr.Mat.GetLength(0); i++)
            {
                for (int j = 0; j < Matr.Mat.GetLength(1); j++)
                {
                    Resultat[i, j] = scal * Matr.Mat[i, j];
                }
            }
            return MatrResultat;
        }

        public static Matrice operator *(Matrice A, Matrice B)
        {
            int n = A.Mat.GetLength(0);
            int m = A.Mat.GetLength(1);
            int p = B.Mat.GetLength(1);

            Matrice multMatrice = new Matrice(n, p);
            double[,] C = multMatrice.Mat;

            if (n == 1 && m == 1)
            {
                multMatrice = A.Mat[0,0] * B;
            }
            else if (B.Mat.GetLength(0) == 1 && p == 1)
            {
                multMatrice = B.Mat[0, 0] * A;
            }
            else
            {

                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < p; j++)
                    {
                        C[i, j] = 0;

                        for (int k = 0; k < m; k++)
                        {
                            C[i, j] += A.Mat[i, k] * B.Mat[k, j];
                        }
                    }

                }
            }

            return multMatrice;
        }
        public static Matrice operator +(Matrice A, Matrice B)
        {
            int n = A.Mat.GetLength(0);
            int m = A.Mat.GetLength(1);

            Matrice AddMatrice=new Matrice(n, m);

            double[,] C = AddMatrice.Mat;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    C[i, j] = A.Mat[i, j] + B.Mat[i, j];
                }
            }

            return AddMatrice;
        }

        public static Matrice operator -(Matrice A, Matrice B)
        {
            int n = A.Mat.GetLength(0);
            int m = A.Mat.GetLength(1);

            Matrice sousMatrice = new Matrice(n, m);

            double[,] C = sousMatrice.Mat;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    C[i, j] = A.Mat[i, j] - B.Mat[i, j];
                }
            }

            return sousMatrice;
        }

        public static Matrice operator -(Matrice M, double valeur)
        {
            int n = M.Mat.GetLength(0);
            int m = M.Mat.GetLength(1);

            Matrice Matr = new Matrice(n, n);

            if (valeur==1)
            {
                double[,] MatriceInverse = Matr.Mat;

                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        MatriceInverse[i, j] = i == j ? 1 : 0;
                    }
                }

                for (int k = 0; k < n; k++)
                {
                    if (M.Mat[k, k] == 0) //permutation ligne i en ligne k
                    {
                        for (int i = k + 1; i < n; i++)
                        {
                            if (M.Mat[i, k] != 0)
                            {
                                for (int j = 0; j < n; j++)
                                {
                                    double tampon = M.Mat[k, j];
                                    M.Mat[k, j] = M.Mat[i, j];
                                    M.Mat[i, j] = tampon;

                                    tampon = MatriceInverse[k, j];
                                    MatriceInverse[k, j] = MatriceInverse[i, j];
                                    MatriceInverse[i, j] = tampon;
                                }

                                break;
                            }
                        }

                    }
                    double pvt = M.Mat[k, k];

                    for (int j = 0; j < n; j++)
                    {
                        M.Mat[k, j] = M.Mat[k, j] / pvt;
                        MatriceInverse[k, j] = MatriceInverse[k, j] / pvt;
                    }

                    for (int i = 0; i < n; i++)
                    {
                        if (i != k)
                        {
                            double pivot = M.Mat[i, k];
                            for (int j = 0; j < n; j++)
                            {
                                M.Mat[i, j] = M.Mat[i, j] - pivot * M.Mat[k, j];
                                MatriceInverse[i, j] = MatriceInverse[i, j] - pivot * MatriceInverse[k, j];
                            }
                        }
                    }
                }
            }
            else
            {  
                Matr = new Matrice(m, n);

                double[,] T = Matr.Mat;

                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        T[i, j] = M.Mat[j, i];
                    }
                } 
            }
           

            return  Matr;
        }

    }
}
