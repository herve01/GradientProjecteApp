using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TpRaoGradienProjecte.GradienPj
{
    public class ProblemeMathematique
    {
        public Polynome FonctEconomique { get; set; }
        public List<Polynome> Contraintes { get; set; }

        public ProblemeMathematique()
        {
            FonctEconomique = new Polynome();
            Contraintes = new List<Polynome>();
        }
    }
}
