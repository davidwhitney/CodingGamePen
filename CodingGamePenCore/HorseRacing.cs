using System;
using System.Collections.Generic;
using System.Linq;
using CodeingGamePen.Fakes;
using NUnit.Framework;

namespace CodeingGamePen
{
    [TestFixture(Explicit = true)]
    public class HorseRacing
    {
        public static FakeConsole Console { get; set; }

        [SetUp]
        public void SetUp() => Console = new FakeConsole();

        [Test]
        public void Execute()
        {
            Console.AddInput("3", "5", "8", "9");

            Simulate(1);

            Assert.That(Console.Outputs[0], Is.EqualTo("1"));
        }

        public static void Simulate(int? maxIterations = null)
        {
            int numberOfHorses = int.Parse(Console.ReadLine());
            var strengths = new List<int>();
            for (int i = 0; i < numberOfHorses; i++)
            {
                strengths.Add(int.Parse(Console.ReadLine()));
            }

            var ordered = strengths.OrderBy(x => x).ToList();
            var smallestDiff = int.MaxValue;
            for (var index = 1; index < ordered.Count; index++)
            {
                var last = ordered[index-1];
                var item = ordered[index];
                var diff = item - last;
                smallestDiff = diff < smallestDiff ? diff : smallestDiff;
            }

            Console.WriteLine(smallestDiff);
        }
        
    }
}
