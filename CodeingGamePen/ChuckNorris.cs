using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace CodeingGamePen
{
    [TestFixture]
    public class ChuckNorris
    {
        public FakeConsole Console { get; set; }

        [SetUp]
        public void SetUp() => Console = new FakeConsole();

        [TestCase("C", "0 0 00 0000 0 00")]
        [TestCase("CC", "0 0 00 0000 0 000 00 0000 0 00")]
        [TestCase("%", "00 0 0 0 00 00 0 0 00 0 0 0")]
        public void Execute(string input, string expectation)
        {
            Console.AddInput(input);

            Simulate();

            Assert.That(Console.Outputs[0], Is.EqualTo(expectation));
        }

        public void Simulate(int? maxIterations = null)
        {
            var message = Console.ReadLine();
            Console.Error.WriteLine(message);

            var unary = new Unary();
            var encoded = unary.Translate(message);
            Console.WriteLine(encoded);
        }

        public class Unary
        {
            public string Translate(string message)
            {
                var bytes = Encoding.ASCII.GetBytes(message);
                var asString = string.Join("", bytes.Select(byt => Convert.ToString(byt, 2).PadLeft(7, '0')));
                var parts = CreateParts(asString);

                var output = "";
                foreach (var part in parts)
                {
                    output += output != "" ? " " : "";
                    var encoded = (part.Contains("1") ? "0" : "00") + " " + new string('0', part.Length);
                    output += encoded;
                }

                return output;
            }

            private static IEnumerable<string> CreateParts(string binaryString)
            {
                var blocks = new List<string>();
                var lastValue = '\0';
                var currentBlock = "";
                foreach (var ch in binaryString)
                {

                    if (ch != lastValue && ch != '\0')
                    {
                        blocks.Add(currentBlock);
                        currentBlock = ch.ToString();
                        lastValue = ch;
                        continue;
                    }

                    currentBlock += ch;
                }

                if (!string.IsNullOrWhiteSpace(currentBlock))
                {
                    blocks.Add(currentBlock);
                }
                blocks.RemoveAt(0);

                return blocks;
            }
        }
    }
}
