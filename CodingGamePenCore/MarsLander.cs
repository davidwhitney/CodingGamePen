using System;
using System.Collections.Generic;
using System.Linq;
using CodeingGamePen.Fakes;
using NUnit.Framework;

namespace CodeingGamePen
{
    [TestFixture(Explicit = true)]
    [TestFixture]
    public class MarsLander
    {
        public FakeConsole Console { get; set; }

        [SetUp]
        public void SetUp() => Console = new FakeConsole();

        [Test]
        public void Execute()
        {
            Simulate(128);
        }

        public void Simulate(int? maxIterations = null)
        {
            // START

            string[] inputs;
            int surfaceN = int.Parse(Console.ReadLine()); // the number of points used to draw the surface of Mars.
            Console.Error.WriteLine(surfaceN);

            for (int i = 0; i < surfaceN; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                int landX = int.Parse(inputs[0]); // X coordinate of a surface point. (0 to 6999)
                int landY = int.Parse(inputs[1]); // Y coordinate of a surface point. By linking all the points together in a sequential fashion, you form the surface of Mars.
            }

            
            var loops = maxIterations.GetValueOrDefault(500);
            while (loops > 0)
            {
                var instructions = Console.ReadLine();
                Console.Error.WriteLine(instructions);
                inputs = instructions.Split(' ');

                int X = int.Parse(inputs[0]);
                int Y = int.Parse(inputs[1]);
                int hSpeed = int.Parse(inputs[2]); // the horizontal speed (in m/s), can be negative.
                int vSpeed = int.Parse(inputs[3]); // the vertical speed (in m/s), can be negative.
                int fuel = int.Parse(inputs[4]); // the quantity of remaining fuel in liters.
                int rotate = int.Parse(inputs[5]); // the rotation angle in degrees (-90 to 90).
                int power = int.Parse(inputs[6]); // the thrust power (0 to 4).

                var absVspeed = Math.Abs(vSpeed);

                var speedMap = new Dictionary<int, int>
                {
                    {20, 4},
                    {15, 3},
                    {10, 2},
                    {0, 1}
                };

                var p = speedMap.First(x => absVspeed >= x.Key).Value;
                Console.WriteLine(0 + " " + p);
              
                loops--;
            }


            // END
        }
    }

}
