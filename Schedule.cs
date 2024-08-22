using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp_Consola_recocido_simulado
{
    class Cronograma
    {
        public Dictionary<string, List<int>> Classes;

        public Cronograma()
        {
            Classes = new Dictionary<string, List<int>>();
        }

        public Cronograma(Cronograma other)
        {
            Classes = new Dictionary<string, List<int>>(other.Classes);
        }
    }
}
