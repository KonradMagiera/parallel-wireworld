using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Threading.Tasks;
using Wireworld.Logic;
using System.Threading;

namespace Wireworld
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly int BOARD_MARGIN = 8;
        private readonly int INITIAL_SIZE = 20;
        private readonly int LIMIT_DRAWING = 40;
        private readonly Automata automata;
        private bool initalRun = true;

        public MainWindow()
        {
            InitializeComponent();
            NodeSize = (canvasBoard.Height - BOARD_MARGIN) / INITIAL_SIZE;
            automata = new Automata(INITIAL_SIZE);

            Draw();
            initalRun = false;
        }

        private double NodeSize { get; set; }
        private int NumberOfGenerations { get; set; }

        /// <summary>
        /// Draw whole board.
        /// </summary>
        private void Draw()
        {
            if (initalRun)
                canvasBoard.Children.Clear();

            if (automata.Size > LIMIT_DRAWING) return;

            for (int i = 0; i < automata.Size; i++)
                for (int j = 0; j < automata.Size; j++)
                {
                    if (!initalRun && automata[j, i] == NodeType.Empty) continue;
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
            if (automata.Size > LIMIT_DRAWING) return;

            Rectangle r = new Rectangle
            {
                Height = NodeSize,
                Width = NodeSize,
                Stroke = Brushes.Gray,
                StrokeThickness = 0.3
            };

            if (automata[y, x] == NodeType.Empty) r.Fill = Brushes.Black;
            else if (automata[y, x] == NodeType.Conductor) r.Fill = Brushes.Yellow;
            else if (automata[y, x] == NodeType.Head) r.Fill = Brushes.Blue;
            else if (automata[y, x] == NodeType.Tail) r.Fill = Brushes.Red;

            Canvas.SetLeft(r, NodeSize * x);
            Canvas.SetTop(r, NodeSize * y);
            canvasBoard.Children.Add(r);
        }

        private void CanvasUserClick(object sender, MouseButtonEventArgs e)
        {
            Point Node = Mouse.GetPosition(canvasBoard);
            int x = (int)(Node.X / NodeSize);
            int y = (int)(Node.Y / NodeSize);

            if (x >= 0 && y >= 0 && x < automata.Size && y < automata.Size)
            {
                if (automata[y, x] == NodeType.Empty) automata[y, x] = NodeType.Conductor;
                else if (automata[y, x] == NodeType.Conductor) automata[y, x] = NodeType.Head;
                else if (automata[y, x] == NodeType.Head) automata[y, x] = NodeType.Tail;
                else if (automata[y, x] == NodeType.Tail) automata[y, x] = NodeType.Empty;

                Console.WriteLine("Editing idx: " + (y * automata.Size + x));

                Draw(x, y);
            }
        }

        private void RunSimulation(object sender, RoutedEventArgs e)
        {
            try
            {
                NumberOfGenerations = int.Parse(numberOfGenerations.Text);
                automata.NumberOfThreads = int.Parse(numberOfThreads.Text);
            }
            catch
            {
                Console.WriteLine("Given settings are incorrect! Only numbers are accepted.");
                return;
            }

            Console.WriteLine($"NumberOfGenerations: {NumberOfGenerations}, NumberOfThreads {automata.NumberOfThreads}");

            automata.Borders = (bool)bordersMode.IsChecked;
            Console.WriteLine($"Borders: {bordersMode.IsChecked}");

            Task.Run(() =>
            {
                for (int i = 0; i < NumberOfGenerations; i++)
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        automata.NextGeneration();
                        Draw();
                    }));
                    Thread.Sleep(75);
                }
            });

        }

        private void ChangeBoardSize(object sender, RoutedEventArgs e)
        {
            try
            {
                int tmpSize = int.Parse(sizeOfBoard.Text);

                if (tmpSize != automata.Size)
                {
                    automata.Nodes = new NodeType[tmpSize * tmpSize];
                    automata.SupportNodes = new NodeType[tmpSize * tmpSize];
                    automata.Size = tmpSize;
                    NodeSize = (canvasBoard.Height - BOARD_MARGIN) / automata.Size;
                    initalRun = true;
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
