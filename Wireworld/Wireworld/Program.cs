using System;
using Wireworld.Logic;
using Wireworld.FileHandling;
using System.Diagnostics;

namespace Wireworld
{
    class Program
    {

        static void Main(string[] args)
        {
            if (args.Length != 5)
            {
                Console.WriteLine("Try ./Wireworld <file.png> <output.gif> <number of generations> <number of threads> <true/false - turn on borders>");
                return;
            }

            ulong numberOfGenerations;
            byte numberOfThreads;
            bool borders;

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
            NodeType[] nodes;
            int width, height;

            try
            {
                nodes = FileHandler.ReadFile(args[0], out width, out height);
            }
            catch
            {
                Console.WriteLine("Given generation is invalid!");
                return;
            }

            // Create and run Automata

            Automata automata = new Automata(width, height)
            {
                NumberOfThreads = numberOfThreads,
                Borders = borders,
                Nodes = nodes
            };

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            for(ulong i = 0; i < numberOfGenerations; i++)
                automata.NextGeneration();
                

            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;
            
            FileHandler.SaveImage(automata, args[1]);

            Console.WriteLine("Time: {0:00}:{1:00}:{2:00}.{3}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
        }
    }
}
