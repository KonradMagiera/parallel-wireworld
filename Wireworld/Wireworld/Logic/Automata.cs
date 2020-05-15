using System;
using System.Threading;
using System.Threading.Tasks;

namespace Wireworld.Logic
{

    class Automata
    {
        public Automata(int size)
        {
            this.Size = size;
            Nodes = new NodeType[size * size];
            SupportNodes = new NodeType[size * size];
            Console.WriteLine("Def supp:" + SupportNodes[0]);
            NumberOfThreads = 1;
        }

        public NodeType[] Nodes { get; set; }
        public NodeType[] SupportNodes { get; set; }
        public int NumberOfThreads { get; set; }
        public int Size { get; set; }
        public bool Borders { get; set; }

        public NodeType this[int key1, int key2]
        {
            get => Nodes[key1 * Size + key2];
            set => Nodes[key1 * Size + key2] = value;
        }

        /// <summary>
        /// Single generation with parallel
        /// </summary>
        // a blank square always stays blank
        // an electron head always becomes an electron tail
        // an electron tail always becomes copper
        // copper stays as copper unless it has just one or two neighbours that are electron heads,
        //      in which case it becomes an electron head
        public void NextGeneration()
        {
            ParallelOptions options = new ParallelOptions { MaxDegreeOfParallelism = 8 };
            Parallel.ForEach(Nodes, options,
            (node, state, index) =>
            {
                switch (node)
                {
                    case NodeType.Head:
                        SupportNodes[index] = NodeType.Tail;
                        break;
                    case NodeType.Tail:
                        SupportNodes[index] = NodeType.Conductor;
                        break;
                    case NodeType.Conductor:
                        int heads = CountHeadsAroundNode((int)index);
                        if (heads == 1 || heads == 2)
                        {
                            SupportNodes[index] = NodeType.Head;
                        }
                        else
                        {
                            SupportNodes[index] = NodeType.Conductor;
                        }
                        break;
                    default:
                        SupportNodes[index] = node;
                        break;
                }
            }
            );

            // Swap boards
            NodeType[] tmp = Nodes;
            Nodes = SupportNodes;
            SupportNodes = tmp;
        }

        private int CountHeadsAroundNode(int idx)
        {
            int heads = 0;

            // row * size + col = idx
            int col = idx % Size;
            int row = (idx - col) / Size;

            for (int col_i = -1; col_i < 2; col_i++)
            {
                for (int row_i = -1; row_i < 2; row_i++)
                {
                    int current_idx = CalculateIndex(row + row_i, col + col_i);
                    if (current_idx != -1 && Nodes[current_idx] == NodeType.Head)
                    {
                        heads++;
                    }
                }
            }

            return heads;
        }

        private int CalculateIndex(int x, int y)
        {
            if (Borders && (x >= Size || y >= Size || x < 0 || y < 0))
            {
                return -1;
            }

            // Wrap x
            if (x >= Size)
                x = 0;
            else if (x < 0)
                x = Size - 1;
            // Wrap y
            if (y >= Size)
                y = 0;
            else if (y < 0)
                y = Size - 1;

            return x * Size + y;
        }
    }
}
