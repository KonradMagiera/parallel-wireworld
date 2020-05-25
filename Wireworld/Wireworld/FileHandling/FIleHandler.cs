using System;
using System.Drawing;
using Wireworld.Logic;

namespace Wireworld.FileHandling
{
    class FileHandler
    {
        public static NodeType[] ReadFile(string filename, out int width, out int height)
        {
            Bitmap generation = new Bitmap(filename);

            width = generation.Width;
            height = generation.Height;

            Color CONDUCTOR = Color.FromArgb(255, 255, 0);
            Color HEAD = Color.FromArgb(0, 0, 255);
            Color TAIL = Color.FromArgb(255, 0, 0);

            NodeType[] nodes = new NodeType[generation.Width * generation.Height];
            int i = 0;
            for (int y = 0; y < generation.Height; y++)
                for (int x = 0; x < generation.Width; x++)
                {
                    Color c = generation.GetPixel(x, y);

                    if (c.Equals(CONDUCTOR))
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
                    else
                    {
                        nodes[i] = NodeType.Empty;
                    }
                    i++;
                }

            return nodes;
        }

        public static void SaveImage(Automata automata, string filename)
        {
            Bitmap generation = new Bitmap(automata.Width, automata.Height);

             Color EMPTY = Color.FromArgb(0, 0, 0);
            Color CONDUCTOR = Color.FromArgb(255, 255, 0);
            Color HEAD = Color.FromArgb(0, 0, 255);
            Color TAIL = Color.FromArgb(255, 0, 0);

            for (int y = 0; y < automata.Height; y++)
                for (int x = 0; x < automata.Width; x++)
                {
                    switch (automata[x, y])
                    {
                        case NodeType.Conductor:
                            generation.SetPixel(x, y, CONDUCTOR);
                            break;
                        case NodeType.Head:
                            generation.SetPixel(x, y, HEAD);
                            break;
                        case NodeType.Tail:
                            generation.SetPixel(x, y, TAIL);
                            break;
                        default:
                            generation.SetPixel(x, y, EMPTY);
                            break;
                    }
                }

            generation.Save(filename, System.Drawing.Imaging.ImageFormat.Png);
        }

    }
}
