using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TpRaoGradienProjecte.GradienPj
{
    public class Variable : List<int>
    {
        public string Nom { get; set; }

        public Variable(string nom)
        {
            Nom = nom;
        }
    }
}
