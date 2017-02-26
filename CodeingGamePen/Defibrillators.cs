using System;
using System.Linq;
using NUnit.Framework;

namespace CodeingGamePen
{
    [TestFixture(Explicit = true)]
    public class Defibrillators
    {
        public static FakeConsole Console { get; set; }

        [SetUp]
        public void SetUp() => Console = new FakeConsole();

        [Test]
        public void Execute()
        {
            Console.AddInput(
                "3,879483",
                "43,608177",
                "3",
                "1;Maison de la Prevention Sante;6 rue Maguelone 340000 Montpellier;;3,87952263361082;43,6071285339217",
                "2;Hotel de Ville;1 place Georges Freche 34267 Montpellier;;3,89652239197876;43,5987299452849",
                "3;Zoo de Lunaret;50 avenue Agropolis 34090 Mtp;;3,87388031141133;43,6395872778854");

            Simulate(1);

            Assert.That(Console.Outputs[0], Is.EqualTo("Maison de la Prevention Sante"));
        }

        public static void Simulate(int? maxIterations = null)
        {
            var current = new Location
            {
                Long = float.Parse(Console.ReadLine().Replace(',','.')),
                Lat = float.Parse(Console.ReadLine().Replace(',', '.'))
            };
            
            var defibCount = int.Parse(Console.ReadLine());

            Defib closest = null;
            double? closestDistance = null;

            for (var i = 0; i < defibCount; i++)
            {
                var data = Console.ReadLine();
                var parts = data.Split(';').Select(x => x.Trim()).ToList();

                var defib = new Defib
                {
                    Name = parts[1],
                    Location = new Location
                    {
                        Long = float.Parse(parts[4].Replace(',', '.')),
                        Lat = float.Parse(parts[5].Replace(',', '.'))
                    }
                };

                var x1 = (defib.Location.Long - current.Long) * Math.Cos((current.Lat - defib.Location.Lat)/2);
                var y = defib.Location.Lat - current.Lat;
                var distance = Math.Sqrt(Math.Pow(x1, 2) + Math.Pow(y, 2))*6371;

                if (distance < closestDistance.GetValueOrDefault(double.MaxValue))
                {
                    closest = defib;
                    closestDistance = distance;
                }
            }

            Console.WriteLine(closest.Name);
        }

        public class Location { public float Long { get; set; } public float Lat { get; set; } }
        public class Defib { public string Name { get; set; } public Location Location { get; set; } }
    }
}
