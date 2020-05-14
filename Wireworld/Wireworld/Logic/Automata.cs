using System;
using System.Threading;

namespace Wireworld.Logic
{
    class Automata
    {
        public Automata(int size)
        {
            Nodes = new NodeType[size, size];
            SupportNodes = new NodeType[size, size];
            NumberOfGenerations = 100;
            NumberOfThreads = 1;

        }

        public NodeType[,] Nodes { get; set; }
        public NodeType[,] SupportNodes { get; set; }
        public int NumberOfGenerations { get; set; }
        public int NumberOfThreads { get; set; }

        /// <summary>
        /// Single generation with parallel
        /// </summary>
        public void NextGeneration()
        {

        }

    }
}
