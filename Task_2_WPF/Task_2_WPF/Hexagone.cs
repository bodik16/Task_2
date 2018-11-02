namespace Task_2_WPF
{
    using System;
    using System.Windows.Media;
    using System.Windows.Shapes;

    [Serializable]
    public class Hexagone
    {
        private readonly int verticesNumber = 6;
        private PointCollection points;

        public Hexagone()
        {
        }

        public Hexagone(Polygon figure)
        {
            this.Points = figure.Points;
            this.HexagoneColor = (figure.Fill as SolidColorBrush).Color;
        }

        public PointCollection Points
        {
            get
            {
                return this.points;
            }

            set
            {
                if (value.Count != this.verticesNumber)
                {
                    throw new ArgumentException("Number of points must be 6");
                }

                this.points = value;
            }
        }

        public Color HexagoneColor { get; set; }
    }
}