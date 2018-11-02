using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Task_2_WPF.Classes;
using System.Windows.Media;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    [TestClass()]
    public class HexagoneTests
    {
        [TestMethod()]
        public void Test_PointsProperty()
        {
            Hexagone hexagone = new Hexagone();
            PointCollection points = new PointCollection
            {
                new Point(67, 89),
                new Point(67, 45),
                new Point(56, 23),
                new Point(10, 9),
                new Point(100, 145),
                new Point(23, 76)
            };
            hexagone.Points = points;
            Assert.AreEqual(hexagone.Points, points);
        }
        [TestMethod]
        public void Test_HexagoneColorProperty()
        {
            Hexagone hexagone = new Hexagone();
            hexagone.HexagoneColor = Colors.Red;
            Assert.AreEqual(hexagone.HexagoneColor, Colors.Red);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_PointsPropertyException()
        {
            Hexagone hexagone = new Hexagone();
            PointCollection points = new PointCollection();

            //Too many points for hexagone (7)
            points.Add(new Point(67, 89));
            points.Add(new Point(67, 45));
            points.Add(new Point(56, 23));
            points.Add(new Point(10, 9));
            points.Add(new Point(100, 145));
            points.Add(new Point(23, 76));
            points.Add(new Point(23, 76));
            hexagone.Points = points;
        }
    }
}
