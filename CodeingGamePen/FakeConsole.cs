using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeingGamePen
{
    public class FakeConsole : ConsoleBufferBase
    {
        public ConsoleBufferBase Error { get; set; } = new ConsoleBufferBase();
    }

    public class ConsoleBufferBase : Queue<string>
    {
        public List<string> Outputs { get; set; } = new List<string>();
        public void AddInput(params string[] inputs) => inputs.ToList().ForEach(Enqueue);
        public string ReadLine() => Dequeue();
        public void WriteLine(object value)
        {
            var asString = value.ToString();
            Outputs.Add(asString);
            Console.WriteLine(asString);
        }
    }
}