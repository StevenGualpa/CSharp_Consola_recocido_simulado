using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp_Consola_recocido_simulado
{
    class Schedule
    {
        public Dictionary<string, List<int>> Classes;

        public Schedule()
        {
            Classes = new Dictionary<string, List<int>>();
        }

        public Schedule(Schedule other)
        {
            Classes = new Dictionary<string, List<int>>(other.Classes);
        }
    }
}
