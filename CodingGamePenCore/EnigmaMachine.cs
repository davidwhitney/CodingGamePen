using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeingGamePen.Fakes;
using NUnit.Framework;

namespace CodingGamePenCore
{
    [TestFixture]
    public class EnigmaMachine
    {
        public static FakeConsole Console { get; set; }

        [SetUp]
        public void SetUp() => Console = new FakeConsole();

        [Test]
        public void Example()
        {
            Console.AddInput("ENCODE", "4", "BDFHJLCPRTXVZNYEIWGAKMUSQO", "AJDKSIRUXBLHWTMCQGZNPYFVOE", "EKMFLGDQVZNTOWYHXUSPAIBRCJ", "AAA");

            Simulate();

            Assert.That(Console.Outputs[0], Is.EqualTo("KQF"));
        }

        [Test]
        public void TestCase23()
        {
            Console.AddInput("ENCODE", "7", "BDFHJLCPRTXVZNYEIWGAKMUSQO", "AJDKSIRUXBLHWTMCQGZNPYFVOE", "EKMFLGDQVZNTOWYHXUSPAIBRCJ", "WEATHERREPORTWINDYTODAY");

            Simulate();

            Assert.That(Console.Outputs[0], Is.EqualTo("ALWAURKQEQQWLRAWZHUYKVN"));
        }

        public void Simulate()
        {
            var operation = Console.ReadLine();
            var startOffset = int.Parse(Console.ReadLine());
            var rotors = new List<string>();
            for (var i = 0; i < 3; i++)
            {
                rotors.Add(Console.ReadLine());
            }

            var message = Console.ReadLine();

            if (operation == "ENCODE")
            {
                var shifted = "";
                foreach (var letter in message)
                {
                    var next = (char)(letter + startOffset);
                    while (next > 'Z')
                    {
                        next = (char) (next - 26);
                    }
                    shifted += next;


                    startOffset++;
                }

                var sb = new StringBuilder();
                foreach (var letter in shifted)
                {
                    var current = letter;
                    foreach (var rotor in rotors)
                    {
                        var alphabetOffset = current - 65;
                        current = rotor[alphabetOffset];
                    }

                    sb.Append(current);
                }

                Console.WriteLine(sb.ToString());
            }
        }
    }
}
