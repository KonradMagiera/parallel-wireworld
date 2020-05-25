using System;
using System.Threading;
using System.Threading.Tasks;

namespace Wireworld.Logic
{

    class Automata
    {
        public Automata(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            Nodes = new NodeType[width * height];
            SupportNodes = new NodeType[width * height];
            NumberOfThreads = 1;
        }

        public NodeType[] Nodes { get; set; }
        public NodeType[] SupportNodes { get; set; }
        public int NumberOfThreads { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public bool Borders { get; set; }

        public NodeType this[int x, int y]
        {
            get => Nodes[y * Width + x];
            set => Nodes[y * Width + x] = value;
        }

        /// <summary>
        /// Single generation with parallel
        /// </summary>
        public void NextGeneration()
        {
            ParallelOptions options = new ParallelOptions { MaxDegreeOfParallelism = NumberOfThreads };
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

            //get => Nodes[y * Width + x];
            //set => Nodes[y * Width + x] = idx;
            int x = idx % Width;
            int y = (idx - x) / Width;

            for (int ix = -1; ix < 2; ix++)
                for (int iy = -1; iy < 2; iy++)
                {
                    int current_idx = CalculateIndex(x + ix, y + iy);
                    if (current_idx != -1 && Nodes[current_idx] == NodeType.Head)
                    {
                        heads++;
                    }
                }

            return heads;
        }

        private int CalculateIndex(int x, int y)
        {
            if (Borders && (x >= Width || y >= Height || x < 0 || y < 0))
                return -1;

            // Wrap x
            if (x >= Width)
                x = 0;
            else if (x < 0)
                x = Width - 1;
            // Wrap y
            if (y >= Height)
                y = 0;
            else if (y < 0)
                y = Height - 1;

            return y * Width + x;
        }
    }
}
