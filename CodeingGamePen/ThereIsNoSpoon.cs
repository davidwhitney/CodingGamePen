using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeingGamePen.Fakes;
using NUnit.Framework;

namespace CodeingGamePen
{
    [TestFixture(Explicit = true)]
    public class ThereIsNoSpoon
    {
        public static FakeConsole Console { get; set; }

        [SetUp]
        public void SetUp() => Console = new FakeConsole();

        [Test]
        public void Execute()
        {
            Console.AddInput(
                "2", "2", "00", "0.");

            Simulate(1);

            Assert.That(Console.Outputs[0], Is.EqualTo("0 0 1 0 0 1"));

            Console = new FakeConsole();
            Console.AddInput(
                "5", "1", "0.0.0");

            Simulate(1);

            Assert.That(Console.Outputs[0], Is.EqualTo("0 0 2 0 -1 -1"));
        }

        [Test]
        public void Execute2()
        {
            Console.AddInput("5", "1", "0.0.0");

            Simulate(1);

            Assert.That(Console.Outputs[0], Is.EqualTo("0 0 2 0 -1 -1"));
        }

        [Test]
        public void Execute3()
        {
            Console.AddInput("3", "3", "0.0", "...", "0.0");

            Simulate(1);

            Assert.That(Console.Outputs[0], Is.EqualTo("0 0 2 0 0 2"));
            Assert.That(Console.Outputs[1], Is.EqualTo("2 0 -1 -1 2 2"));
        }

        private void Simulate(int i)
        {
            var width = int.Parse(Console.ReadLine()); // the number of cells on the X axis
            var height = int.Parse(Console.ReadLine()); // the number of cells on the Y axis
            
            var rowStrings = new List<string>();
            for (var heightY = 0; heightY < height; heightY++)
            {
                rowStrings.Add(Console.ReadLine());
                Console.Error.WriteLine(rowStrings.Last());
            }

            for (var y = 0; y < rowStrings.Count; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var thisNode = SafeGet(rowStrings, y, x);
                    if (thisNode == '.') continue;

                    var columnStrings = string.Join("", rowStrings.Select(row => row[x]).ToList());

                    var nextX = rowStrings[y].IndexOf("0", x + 1);
                    var right = SafeGet(rowStrings, y, nextX);
                    int nextXy;
                    if (right == '0')
                    {
                        nextXy = y;
                    }
                    else
                    {
                        nextX = nextXy = -1;
                    }

                    var nextY = columnStrings.IndexOf('0', y + 1);
                    var down = SafeGet(columnStrings.Select(s => s.ToString()).ToList(), nextY, 0);
                    int nextYx;
                    if (down == '0')
                    {
                        nextYx = x;
                    }
                    else
                    {
                        nextY = nextYx = -1;
                    }

                    var formattableString = $"{x} {y} {nextX} {nextXy} {nextYx} {nextY}";
                    Console.WriteLine(formattableString);
                }
            }
        }

        private static char SafeGet(List<string> collection, int y, int x)
        {
            try { return collection[y][x]; }
            catch { return '.'; }
        }
    }
}
