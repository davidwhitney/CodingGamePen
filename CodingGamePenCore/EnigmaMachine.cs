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

        public void Simulate()
        {
            var operation = Console.ReadLine();
            var seed = int.Parse(Console.ReadLine());

            var enigma = new Enigma(
                new Enigma.Rotor(Console.ReadLine()),
                new Enigma.Rotor(Console.ReadLine()),
                new Enigma.Rotor(Console.ReadLine())
            );

            var message = Console.ReadLine();

            var res = operation == "ENCODE"
                ? enigma.Encode(message, seed)
                : enigma.Decode(message, seed);

            Console.WriteLine(res);
        }

        public class Enigma
        {
            private readonly IEnumerable<Rotor> _rotors;
            public Enigma(params Rotor[] rotors) => _rotors = rotors.ToList();

            public string Encode(string message, int seed)
            {
                var shifted = CaesarShift(message, seed);
                return _rotors.Aggregate(shifted, (current, rotations) => rotations.EncodeString(current));
            }

            public string Decode(string message, int seed)
            {
                var decryptOrdered = new List<Rotor>(_rotors);
                decryptOrdered.Reverse();

                var rotated = decryptOrdered.Aggregate(message, (current, rot) => rot.DecodeString(current));
                return CaesarShift(rotated, seed, true);
            }

            private static string CaesarShift(string message, int seed, bool backwards = false)
            {
                var shiftDirection = backwards ? -1 : 1;
                var wrapDirection = backwards ? 1 : -1;
                var withinAlphabetBounds = backwards
                    ? new Func<int, bool>(next => next < 'A')
                    : next => next > 'Z';

                var incrementingNumber = 0;
                var shifted = new StringBuilder();

                foreach (var letter in message)
                {
                    var shiftedLetter = letter + (seed + incrementingNumber++) * shiftDirection;
                    while (withinAlphabetBounds(shiftedLetter))
                    {
                        shiftedLetter += 26 * wrapDirection;
                    }

                    shifted.Append((char)shiftedLetter);
                }

                return shifted.ToString();
            }

            public class Rotor
            {
                private readonly string _key;
                public Rotor(string key) => _key = key;
                public string EncodeString(string sequence) => new string(Encode(sequence).ToArray());
                public IEnumerable<char> Encode(string sequence) => sequence.Select(letter => _key[letter - 65]);
                public string DecodeString(string sequence) => new string(Decode(sequence).ToArray());
                public IEnumerable<char> Decode(string sequence) => sequence.Select(letter => (char)(_key.IndexOf(letter) + 65));
            }
        }


    }
}
