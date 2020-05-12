using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;


namespace Wireworld
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly int BOARD_MARGIN = 8;

        public MainWindow()
        {
            InitializeComponent();
            NodeSize = (canvasBoard.Height - BOARD_MARGIN) / NodeNumber;
            tmp = new NodeType[NodeNumber, NodeNumber];

            Draw();

        }


        private int NodeNumber { get; set; } = 20;
        private double NodeSize { get; set; }

        // Variables below probably should be moved to backend
        private NodeType[,] tmp;
        private int NumberOfGenerations { get; set; }
        private int NumberOfThreads { get; set; }


        /// <summary>
        /// Draw whole board.
        /// </summary>
        private void Draw()
        {
            canvasBoard.Children.Clear();

            for (int i = 0; i < NodeNumber; i++)
                for (int j = 0; j < NodeNumber; j++)
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

            if (tmp[x, y] == NodeType.Empty) r.Fill = Brushes.Black;
            else if (tmp[x, y] == NodeType.Conductor) r.Fill = Brushes.Yellow;
            else if (tmp[x, y] == NodeType.Head) r.Fill = Brushes.Blue;
            else if (tmp[x, y] == NodeType.Tail) r.Fill = Brushes.Red;

            Canvas.SetLeft(r, NodeSize * x);
            Canvas.SetTop(r, NodeSize * y);
            canvasBoard.Children.Add(r);
        }

        private void CanvasUserClick(object sender, MouseButtonEventArgs e)
        {
            Point Node = Mouse.GetPosition(canvasBoard);
            int x = (int)(Node.X / NodeSize);
            int y = (int)(Node.Y / NodeSize);

            if (x >= 0 && y >= 0 && x < NodeNumber && y < NodeNumber)
            {
                if (tmp[x, y] == NodeType.Empty) tmp[x, y] = NodeType.Conductor;
                else if (tmp[x, y] == NodeType.Conductor) tmp[x, y] = NodeType.Head;
                else if (tmp[x, y] == NodeType.Head) tmp[x, y] = NodeType.Tail;
                else if (tmp[x, y] == NodeType.Tail) tmp[x, y] = NodeType.Empty;
                Draw(x, y);
            }
        }

        private void RunSimulation(object sender, RoutedEventArgs e)
        {
            try
            {
                NumberOfGenerations = int.Parse(numberOfGenerations.Text);
                NumberOfThreads = int.Parse(numberOfThreads.Text);
            }
            catch
            {
                Console.WriteLine("Given settings are incorrect! Only numbers are accepted.");
            }

            Console.WriteLine($"NumberOfGenerations: {NumberOfGenerations}, NumberOfThreads {NumberOfThreads}");
        }

        private void ChangeBoardSize(object sender, RoutedEventArgs e)
        {
            try
            {
                int tmpSize = int.Parse(sizeOfBoard.Text);

                if (tmpSize != NodeNumber)
                {
                    NodeNumber = tmpSize;
                    NodeSize = (canvasBoard.Height - BOARD_MARGIN) / NodeNumber;
                    tmp = new NodeType[NodeNumber, NodeNumber];
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
