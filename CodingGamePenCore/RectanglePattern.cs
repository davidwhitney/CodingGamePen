using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CodeingGamePen.Fakes;
using NUnit.Framework;

namespace CodingGamePenCore
{

    [TestFixture(Explicit = false)]
    public class RectanglePattern
    {
        public static FakeConsole Console { get; set; }

        [SetUp]
        public void SetUp() => Console = new FakeConsole();

        [Test]
        public void JustASquare_Counts1()
        {
            Console.AddInput("2 2 0 0", "", "");

            Simulate();

            Assert.That(Console.Outputs[0], Is.EqualTo("1"));
        }

        [Test]
        public void OneShape_ButItsNotASquare_CountsZero()
        {
            Console.AddInput("2 4 0 0", "", "");

            Simulate();

            Assert.That(Console.Outputs[0], Is.EqualTo("0"));
        }

        [Test]
        public void TwoVerticalSquares_Counts2()
        {
            Console.AddInput("2 4 0 1", "", "2");

            Simulate();

            Assert.That(Console.Outputs[0], Is.EqualTo("2"));
        }

        [Test]
        public void TwoHorizontalSquares_Counts2()
        {
            Console.AddInput("4 2 1 0", "2", "");

            Simulate();

            Assert.That(Console.Outputs[0], Is.EqualTo("2"));
        }

        [Test]
        public void Execute()
        {
            Console.AddInput("10 5 2 1", "2 5", "3");

            Simulate();

            Assert.That(Console.Outputs[0], Is.EqualTo("4"));
        }

        public static void Simulate()
        {
            var dimensions = Console.ReadLine().Split(' ').Select(int.Parse).ToList();
            var xDimensionStrings = Console.ReadLine().Split(' ').ToList();
            var yDimensionStrings = Console.ReadLine().Split(' ').ToList();
            xDimensionStrings.RemoveAll(string.IsNullOrWhiteSpace);
            yDimensionStrings.RemoveAll(string.IsNullOrWhiteSpace);

            var xAxisIntersections = xDimensionStrings.Any() ? xDimensionStrings.Select(int.Parse).ToList() : new List<int>();
            var yAxisIntersections = yDimensionStrings.Any() ? yDimensionStrings.Select(int.Parse).ToList() : new List<int>();

            var width = dimensions[0];
            var height = dimensions[1];

            var shapesMade = new List<Area>();
            var xPoints = xAxisIntersections.Concat(new List<int> {0, width}).OrderBy(x => x).ToList();
            var yPoints = yAxisIntersections.Concat(new List<int> {0, height}).OrderBy(x => x).ToList();

            for (var index = 1; index < xPoints.Count; index++)
            {
                var xLine = xPoints[index];

                if (shapesMade.Any())
                {
                    var previousSnapshot = new List<Area>(shapesMade);
                    shapesMade.AddRange(previousSnapshot.Select(prev => new Area(xLine - prev.Width, xLine, 0, height)));
                }

                shapesMade.Add(new Area(0, xLine, 0, height));
            }

            for (var index = 1; index < yPoints.Count; index++)
            {
                var yLine = yPoints[index];

                if (shapesMade.Any())
                {
                    var previousSnapshot = new List<Area>(shapesMade);
                    shapesMade.AddRange(previousSnapshot.Select(prev => new Area(prev.Left, prev.Width, yLine - prev.Top, yLine)));
                }

                shapesMade.Add(new Area(0, width, 0, yLine));
            }
            
            var distinctShapes = shapesMade.Distinct().ToList();
            var squares = distinctShapes.Count(x => x.IsSquare);
            Console.WriteLine(squares);
        }

        [DebuggerDisplay("L{Left},R{Right},T{Top},B{Bottom} - IsSquare:{IsSquare}")]
        public class Area
        {
            public int Left { get; }
            public int Right { get; }
            public int Top { get; }
            public int Bottom { get; }
            public int Width => Right - Left;
            public int Height => Bottom - Top;
            public bool IsSquare => Width == Height;
           
            public Area(int left, int right, int top, int bottom)
            {
                Left = left;
                Right = right;
                Top = top;
                Bottom = bottom;
            }

            public override bool Equals(object obj) => Equals((Area)obj);
            protected bool Equals(Area other) => Left == other.Left && Right == other.Right && Top == other.Top && Bottom == other.Bottom;

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = Left;
                    hashCode = (hashCode * 397) ^ Right;
                    hashCode = (hashCode * 397) ^ Top;
                    hashCode = (hashCode * 397) ^ Bottom;
                    return hashCode;
                }
            }
        }
    }
}
    
