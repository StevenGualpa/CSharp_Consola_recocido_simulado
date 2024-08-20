using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp_Consola_recocido_simulado
{
    class Program
    {
        static void Main(string[] args)
        {
            double result = Anneal();
            Console.WriteLine($"Optimized result: x = {result}, f(x) = {ObjectiveFunction(result)}");
            Console.ReadKey();
        }
        static Random random = new Random();

        public static double ObjectiveFunction(double x)
        {
            return x * x;
        }

        public static double RandomStart()
        {
            return random.NextDouble() * 20 - 10;  // Rango entre -10 y 10
        }

        public static double RandomNeighbour(double x, double temperature)
        {
            double min = Math.Max(-10, x - temperature);
            double max = Math.Min(10, x + temperature);
            return min + (max - min) * random.NextDouble();
        }

        public static double AcceptanceProbability(double cost, double newCost, double temperature)
        {
            if (newCost < cost)
                return 1.0;
            return Math.Exp((cost - newCost) / temperature);
        }

        public static double Anneal()
        {
            double temperature = 10000.0;  // Temperatura inicial
            double coolingRate = 0.99;     // Tasa de enfriamiento
            double absoluteTemp = 0.0001;  // Temperatura mínima

            double x = RandomStart();
            double best = x;
            double bestCost = ObjectiveFunction(x);

            while (temperature > absoluteTemp)
            {
                double candidate = RandomNeighbour(x, temperature);
                double candidateCost = ObjectiveFunction(candidate);

                if (AcceptanceProbability(bestCost, candidateCost, temperature) > random.NextDouble())
                {
                    x = candidate;
                    best = x;
                    bestCost = candidateCost;
                }

                temperature *= coolingRate;
            }
            return best;
        }
    }
}
