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
        static Dictionary<string, Course> Courses = new Dictionary<string, Course>();
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
                    continue; // Esto asegura que no intentemos procesar las líneas de encabezado como datos.
                }

                var parts = line.Split(',').Select(p => p.Trim()).ToArray();
                try
                {
                    switch (section)
                    {
                        case "# Cursos":
                            if (parts[0] == "ID") continue; // Ignora la línea de encabezado de cada sección
                            Courses[parts[0]] = new Course
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


        // Funciones para Simulated Annealing
        static double CalculateCost(Schedule schedule)
        {
            double cost = 0;
            // Implementar la lógica de costo
            return cost;
        }

        static Schedule GenerateNeighbor(Schedule current)
        {
            Schedule neighbor = new Schedule(current);
            // Implementar la lógica para generar vecino
            return neighbor;
        }

        static bool AcceptanceCriterion(double currentCost, double newCost, double temperature)
        {
            if (newCost < currentCost)
                return true;
            return Math.Exp((currentCost - newCost) / temperature) > random.NextDouble();
        }

        static Schedule SimulatedAnnealing()
        {
            double temperature = 1000.0;
            double coolingRate = 0.95;
            Schedule currentSchedule = GenerateInitialSchedule();
            Schedule bestSchedule = currentSchedule;
            double currentCost = CalculateCost(currentSchedule);

            while (temperature > 1)
            {
                Schedule newSchedule = GenerateNeighbor(currentSchedule);
                double newCost = CalculateCost(newSchedule);

                if (AcceptanceCriterion(currentCost, newCost, temperature))
                {
                    currentSchedule = newSchedule;
                    currentCost = newCost;

                    if (newCost < CalculateCost(bestSchedule))
                    {
                        bestSchedule = newSchedule;
                    }
                }

                temperature *= coolingRate;
            }

            return bestSchedule;
        }


        static Schedule GenerateInitialSchedule()
        {
            Schedule schedule = new Schedule();
            // Asigna horarios iniciales a los cursos de manera aleatoria dentro de las restricciones básicas
            foreach (var course in Courses.Values)
            {
                List<int> times = new List<int>();
                for (int i = 0; i < course.StudentCount; i++)
                {
                    // Asumiendo que 'times' representa bloques de tiempo, ej: 9 = 9AM, 10 = 10AM, etc.
                    times.Add(random.Next(8, 17)); // Horario entre 8AM y 5PM
                }
                schedule.Classes.Add(course.Id, times);
            }
            return schedule;
        }


        static void Main(string[] args)
        {

            LoadData("schedule_data.txt");
            Schedule optimalSchedule = SimulatedAnnealing();
            Console.WriteLine("Optimal schedule found with cost: " + CalculateCost(optimalSchedule));
            Console.ReadKey();
        }




    }
}
