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
        private int NodeNumber { get; set; } = 20;
        private double NodeSize { get; set; }
        private NodeType[,] tmp;

        public MainWindow()
        {
            InitializeComponent();
            NodeSize = canvasBoard.Height / NodeNumber;
            tmp = new NodeType[NodeNumber, NodeNumber];

            // Test
            tmp[5, 4] = NodeType.Head;
            tmp[3, 9] = NodeType.Conductor;
            tmp[9, 0] = NodeType.Tail;
            Draw();

        }

        private void Draw()
        {
            for (int i = 0; i < NodeNumber; i++)
                for (int j = 0; j < NodeNumber; j++)
                {
                    Rectangle r = new Rectangle
                    {
                        Height = NodeSize,
                        Width = NodeSize,
                        Stroke = Brushes.Gray,
                        StrokeThickness = 0.3
                    };

                    if (tmp[i, j] == NodeType.Empty) r.Fill = Brushes.Black;
                    else if (tmp[i, j] == NodeType.Conductor) r.Fill = Brushes.Yellow;
                    else if (tmp[i, j] == NodeType.Head) r.Fill = Brushes.Blue;
                    else if (tmp[i, j] == NodeType.Tail) r.Fill = Brushes.Red;

                    Canvas.SetLeft(r, NodeSize * i);
                    Canvas.SetTop(r, NodeSize * j);
                    canvasBoard.Children.Add(r);
                }
        }

        private void CanvasUserClick(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("Test");
            Point Node = Mouse.GetPosition(canvasBoard);
            int x = (int)(Node.X / NodeSize);
            int y = (int)(Node.Y / NodeSize);

            if(x >= 0 && y >= 0 && x < NodeNumber && y < NodeNumber)
            {
                if (tmp[x, y] == NodeType.Empty) tmp[x,y] = NodeType.Conductor;
                else if (tmp[x, y] == NodeType.Conductor) tmp[x, y] = NodeType.Head;
                else if (tmp[x, y] == NodeType.Head) tmp[x, y] = NodeType.Tail;
                else if (tmp[x, y] == NodeType.Tail) tmp[x, y] = NodeType.Empty;
                Draw();
            }

        }
    }
}
