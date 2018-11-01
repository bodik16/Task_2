namespace Task_2_WPF
{
    using System;
    using System.Windows.Media;
    using System.Windows.Shapes;

    class Hexagone
    {
        private readonly int VERTICES_NUMBER = 6;
        private PointCollection points;
        public PointCollection Points
        {
            get
            {
                return points;
            }
            set
            {
                if (value.Count != VERTICES_NUMBER)
                {
                    throw new ArgumentException("Number of points must be 6");
                }
                points = value;
            }
        }
        public Color HexagoneColor { get; set; }

        public Hexagone() { }

        public Hexagone(Polygon figure)
        {
            Points = figure.Points;
            HexagoneColor = (figure.Fill as SolidColorBrush).Color;
        }
    }
}
