using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Wireworld.Logic;

namespace Wireworld
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly int BOARD_MARGIN = 8;
        private int INITIAL_SIZE = 20;
        private readonly Automata automata;

        public MainWindow()
        {
            InitializeComponent();
            NodeSize = (canvasBoard.Height - BOARD_MARGIN) / INITIAL_SIZE;
            automata = new Automata(INITIAL_SIZE);

            Draw();
        }

        private double NodeSize { get; set; } // Size of individual Rectangle

        /// <summary>
        /// Draw whole board.
        /// </summary>
        private void Draw()
        {
            canvasBoard.Children.Clear();

            for (int i = 0; i < automata.Nodes.GetLength(0); i++)
                for (int j = 0; j < automata.Nodes.GetLength(0); j++)
                {
                    Draw(i, j);
                }
        }

        /// <summary>
        /// Draw single Rectangle.
        /// </summary>
        /// <param name="x">Position X of Rectangle</param>
        /// <param name="y">Position Y of Rectangle</param>
        private void Draw(int x, int y)
        {
            Rectangle r = new Rectangle
            {
                Height = NodeSize,
                Width = NodeSize,
                Stroke = Brushes.Gray,
                StrokeThickness = 0.3
            };

            if (automata.Nodes[x, y] == NodeType.Empty) r.Fill = Brushes.Black;
            else if (automata.Nodes[x, y] == NodeType.Conductor) r.Fill = Brushes.Yellow;
            else if (automata.Nodes[x, y] == NodeType.Head) r.Fill = Brushes.Blue;
            else if (automata.Nodes[x, y] == NodeType.Tail) r.Fill = Brushes.Red;

            Canvas.SetLeft(r, NodeSize * x);
            Canvas.SetTop(r, NodeSize * y);
            canvasBoard.Children.Add(r);
        }

        private void CanvasUserClick(object sender, MouseButtonEventArgs e)
        {
            Point Node = Mouse.GetPosition(canvasBoard);
            int x = (int)(Node.X / NodeSize);
            int y = (int)(Node.Y / NodeSize);

            if (x >= 0 && y >= 0 && x < automata.Nodes.GetLength(0) && y < automata.Nodes.GetLength(0))
            {
                if (automata.Nodes[x, y] == NodeType.Empty) automata.Nodes[x, y] = NodeType.Conductor;
                else if (automata.Nodes[x, y] == NodeType.Conductor) automata.Nodes[x, y] = NodeType.Head;
                else if (automata.Nodes[x, y] == NodeType.Head) automata.Nodes[x, y] = NodeType.Tail;
                else if (automata.Nodes[x, y] == NodeType.Tail) automata.Nodes[x, y] = NodeType.Empty;
                Draw(x, y);
            }
        }

        private void RunSimulation(object sender, RoutedEventArgs e)
        {
            try
            {
                automata.NumberOfGenerations = int.Parse(numberOfGenerations.Text);
                automata.NumberOfThreads = int.Parse(numberOfThreads.Text);
            }
            catch
            {
                Console.WriteLine("Given settings are incorrect! Only numbers are accepted.");
            }

            Console.WriteLine($"NumberOfGenerations: {automata.NumberOfGenerations}, NumberOfThreads {automata.NumberOfThreads}");
        }

        private void ChangeBoardSize(object sender, RoutedEventArgs e)
        {
            try
            {
                int tmpSize = int.Parse(sizeOfBoard.Text);

                if (tmpSize != automata.Nodes.GetLength(0))
                {
                    automata.Nodes = new NodeType[tmpSize, tmpSize];
                    NodeSize = (canvasBoard.Height - BOARD_MARGIN) / automata.Nodes.GetLength(0);

                    Draw();
                }
            }
            catch
            {
                Console.WriteLine("Given size is incorrect! Only numbers are accepted.");
            }
        }
    }
}
