using System;
using Wireworld.Logic;

namespace Wireworld
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length != 5)
            {
                Console.WriteLine("Try ./Wireworld <file.png> <output.gif> <number of generations> <number of threads> <true/false - turn on borders>");
                return;
            }

            ulong numberOfGenerations = 1;
            byte numberOfThreads = 1;
            bool borders = true;

            // Read numbers
            try
            {
                numberOfGenerations = ulong.Parse(args[2]);
                numberOfThreads = byte.Parse(args[3]);
                borders = bool.Parse(args[4]);
                if (numberOfGenerations < 0 || numberOfThreads < 1) throw new ArgumentException();

            } catch
            {
                Console.WriteLine($"Please make sure 3rd and 4th parameters are positive numbers. NumberOfGenerations: {args[2]}, NumberOfThreads: {args[3]}");
                Console.WriteLine($"Also check if 5th parameter is \"true\" or \"false\". Borders: {args[4]}");
                return;
            }

            // Read png
            int size = 5; // temporary

            // Create and run Automata
            Automata automata = new Automata(size)
            {
                NumberOfThreads = numberOfThreads
            };

        }
    }
}
