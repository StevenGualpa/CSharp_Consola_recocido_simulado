using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp_Consola_recocido_simulado
{
    class Program
    {

        static Random random = new Random();
        static Dictionary<string, Curso> Courses = new Dictionary<string, Curso>();
        static Dictionary<string, Room> Rooms = new Dictionary<string, Room>();
        static Dictionary<string, Professor> Professors = new Dictionary<string, Professor>();

        // Cargar los datos de un archivo de texto
        public static void LoadData(string filePath)
        {
            string section = string.Empty;
            foreach (var line in File.ReadLines(filePath))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                if (line.Trim().StartsWith("#"))
                {
                    section = line.Trim();
                    continue;
                }

                var parts = line.Split(',').Select(p => p.Trim()).ToArray();
                try
                {
                    switch (section)
                    {
                        case "# Cursos":
                            if (parts[0] == "ID") continue;
                            Courses[parts[0]] = new Curso
                            {
                                Id = parts[0],
                                Name = parts[1],
                                StudentCount = int.Parse(parts[2]),
                                Requirements = parts.Length > 3 ? parts[3] : string.Empty
                            };
                            break;
                        case "# Aulas":
                            if (parts[0] == "ID") continue;
                            Rooms[parts[0]] = new Room
                            {
                                Id = parts[0],
                                Capacity = int.Parse(parts[1]),
                                Resources = parts.Length > 2 ? parts[2] : string.Empty
                            };
                            break;
                        case "# Profesores":
                            if (parts[0] == "ID") continue;
                            Professors[parts[0]] = new Professor
                            {
                                Id = parts[0],
                                Name = parts[1],
                                Availability = parts[2]
                            };
                            break;
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine($"Error parsing data from line: {line}. Error: {ex.Message}");
                }
            }
        }



        static Cronograma GenerateInitialSchedule()
        {
            Cronograma cronograma = new Cronograma();
            // Asigna horarios iniciales a los cursos de manera aleatoria
            foreach (var course in Courses.Values)
            {
                List<int> times = new List<int>();
                for (int i = 0; i < course.StudentCount; i++)
                {

                    times.Add(random.Next(8, 17)); // Horario inicial y donde termine
                }
                cronograma.Classes.Add(course.Id, times);
            }
            return cronograma;
        }


        static Cronograma GenerateNeighbor(Cronograma current)
        {
            Cronograma neighbor = new Cronograma(current);
            // Implementar la lógica para generar vecino
            return neighbor;
        }


        // Funciones para Simulated Annealing
        static double CalculateCost(Cronograma cronograma)
        {
            double cost = 0;
            // Implementar la lógica de costo
            return cost;
        }

        static bool AcceptanceCriterion(double currentCost, double newCost, double temperature)
        {
            if (newCost < currentCost)
                return true;
            return Math.Exp((currentCost - newCost) / temperature) > random.NextDouble();
        }

        static Cronograma SimulatedAnnealing()
        {
            double temperature = 1000.0;
            double coolingRate = 0.95;
            Cronograma currentCronograma = GenerateInitialSchedule();
            Cronograma bestCronograma = currentCronograma;
            double currentCost = CalculateCost(currentCronograma);

            while (temperature > 1)
            {
                Cronograma newCronograma = GenerateNeighbor(currentCronograma);
                double newCost = CalculateCost(newCronograma);

                if (AcceptanceCriterion(currentCost, newCost, temperature))
                {
                    currentCronograma = newCronograma;
                    currentCost = newCost;

                    if (newCost < CalculateCost(bestCronograma))
                    {
                        bestCronograma = newCronograma;
                    }
                }

                temperature *= coolingRate;
            }

            return bestCronograma;
        }


        static void PrintSchedule(Cronograma cronograma, string title)
        {
            Console.WriteLine(title);
            foreach (var classEntry in cronograma.Classes)
            {
                string courseId = classEntry.Key;
                var times = classEntry.Value;
                Console.WriteLine($"Curso: {Courses[courseId].Name}, Horarios: {String.Join(", ", times)}");
            }
        }


        static void Main(string[] args)
        {

            LoadData("schedule_data.txt");  // Carga los datos necesarios

            Console.WriteLine("Generando horario inicial...");
            Cronograma initialCronograma = GenerateInitialSchedule();  // Genera el horario inicial
            PrintSchedule(initialCronograma, "Horario Inicial");  // Imprime el horario inicial

            Console.WriteLine("\nOptimizando el horario mediante recocido simulado...");
            Cronograma optimalCronograma = SimulatedAnnealing();  // Optimiza el horario
            PrintSchedule(optimalCronograma, "Horario Final");  // Imprime el horario final

            Console.WriteLine("Optimal schedule found with cost: " + CalculateCost(optimalCronograma));
            Console.ReadKey();
        }




    }
}
