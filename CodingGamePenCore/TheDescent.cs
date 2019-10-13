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
    public class TheDescent
    {
        public FakeConsole Console { get; set; }

        [SetUp]
        public void SetUp() => Console = new FakeConsole();

        [Test]
        public void Execute()
        {
            Console.AddInput("9", "8", "7", "6", "5", "4", "3", "2");

            Simulate(1);

            Assert.That(Console.Outputs[0], Is.EqualTo("0"));
        }

        public void Simulate(int? maxIterations = null)
        {
            var loops = maxIterations.GetValueOrDefault(100);
            while (loops > 0)
            {
                var mountains = new Dictionary<int, int>();

                for (int i = 0; i < 8; i++)
                {
                    int mountainH = int.Parse(Console.ReadLine());
                    mountains.Add(i, mountainH);
                }

                var ordered = mountains.OrderByDescending(x => x.Value);

                Console.WriteLine(ordered.First().Key);

                loops--;
            }
        }
    }
}
