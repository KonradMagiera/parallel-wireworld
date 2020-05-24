using System;
using System.Drawing;
using Wireworld.Logic;

namespace Wireworld.FileHandling
{
    class FileHandler
    {
        public static NodeType[] ReadFile(string filename)
        {
            Bitmap generation = new Bitmap(filename);

            // as we dont use GUI, maybe modify automata to different dimension values
            if (generation.Width != generation.Height) throw new NotSupportedException();

            Color EMPTY = Color.FromArgb(0, 0, 0);
            Color CONDUCTOR = Color.FromArgb(255, 255, 0);
            Color HEAD = Color.FromArgb(0, 0, 255);
            Color TAIL = Color.FromArgb(255, 0, 0);

            NodeType[] nodes = new NodeType[generation.Width * generation.Height];
            int i = 0;
            for (int y = 0; y < generation.Height; y++)
                for (int x = 0; x < generation.Width; x++)
                {
                    Color c = generation.GetPixel(x, y);

                    if (c.Equals(EMPTY))
                    {
                        nodes[i] = NodeType.Empty;
                    }
                    else if (c.Equals(CONDUCTOR))
                    {
                        nodes[i] = NodeType.Conductor;
                    }
                    else if (c.Equals(TAIL))
                    {
                        nodes[i] = NodeType.Tail;
                    }
                    else if (c.Equals(HEAD))
                    {
                        nodes[i] = NodeType.Head;
                    }
                    i++;
                }

            return nodes;
        }

        public static void SaveGif()
        {

        }

    }
}
