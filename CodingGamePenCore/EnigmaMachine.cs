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
        public void Encode23()
        {
            Console.AddInput("ENCODE", "7", "BDFHJLCPRTXVZNYEIWGAKMUSQO", "AJDKSIRUXBLHWTMCQGZNPYFVOE", "EKMFLGDQVZNTOWYHXUSPAIBRCJ", "WEATHERREPORTWINDYTODAY");

            Simulate();

            Assert.That(Console.Outputs[0], Is.EqualTo("ALWAURKQEQQWLRAWZHUYKVN"));
        }

        [Test]
        public void Decode21()
        {
            Console.AddInput("DECODE", "9", "BDFHJLCPRTXVZNYEIWGAKMUSQO", "AJDKSIRUXBLHWTMCQGZNPYFVOE", "EKMFLGDQVZNTOWYHXUSPAIBRCJ", "PQSACVVTOISXFXCIAMQEM");

            Simulate();

            Assert.That(Console.Outputs[0], Is.EqualTo("EVERYONEISWELCOMEHERE"));
        }

        [Test]
        public void ForumExample()
        {
            Console.AddInput("DECODE", "4", "BDFHJLCPRTXVZNYEIWGAKMUSQO", "AJDKSIRUXBLHWTMCQGZNPYFVOE", "EKMFLGDQVZNTOWYHXUSPAIBRCJ", "KFDI");

            Simulate();

            Assert.That(Console.Outputs[0], Is.EqualTo("ABCD"));
        }

        public void Simulate()
        {
            var operation = Console.ReadLine();
            var seed = int.Parse(Console.ReadLine());
            var rotors = new List<string>
            {
                Console.ReadLine(),
                Console.ReadLine(),
                Console.ReadLine()
            };

            var message = Console.ReadLine();

            if (operation == "ENCODE")
            {
                var shifted = CaesarShift(message, seed);

                var sb = new StringBuilder();
                foreach (var letter in shifted)
                {
                    sb.Append(rotors.Aggregate(letter, (currentValue, rotor) => rotor[currentValue - 65]));
                }

                Console.WriteLine(sb.ToString());
            }
            else
            {
                rotors.Reverse();
                var sb = new StringBuilder();
                foreach (var letter in message)
                {
                    var currentValue = letter;
                    foreach (var rotor in rotors)
                    {
                        var index = rotor.IndexOf(currentValue);
                        currentValue = (char) ((char)index + 65);
                    }

                    sb.Append(currentValue);
                }

                var preShift = sb.ToString();
                var shifted = CaesarShift(preShift, seed, true);
                Console.WriteLine(shifted);

            }
        }

        private static string CaesarShift(string message, int seed, bool inverse = false)
        {
            var shifted = new StringBuilder();
            var incrementingNumber = 0;

            foreach (var letter in message)
            {
                var shiftFactor = seed + incrementingNumber;
                shiftFactor = inverse ? shiftFactor * -1 : shiftFactor;

                var next = letter + shiftFactor;

                if (inverse)
                {
                    while (next < 'A')
                    {
                        next += 26;
                    }
                }
                else
                {
                    while (next > 'Z')
                    {
                        next -= 26;
                    }
                }

                shifted.Append((char)next);
                incrementingNumber++;
            }

            return shifted.ToString();
        }
    }
}
