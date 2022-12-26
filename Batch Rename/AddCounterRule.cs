using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Batch_Rename
{
    public class AddCounterRule : IRule
    {
        private int _current = 0;
        private int _start = 0;

        public int Start
        {
            get => _start; set
            {
                _start = value;

                _current = value;
            }
        }
        public int Step { get; set; }

        public string Name => "Add Counter";

        public AddCounterRule()
        {
            Start = 1;
            Step = 3;
        }
        public string Rename(string origin)
        {
            string currentString = _current.ToString();
            if (_current <= 9 && _current >=0)
            {
                currentString = $"0{currentString}";
            }

            string filename = Path.GetFileNameWithoutExtension(origin);
            string extension = Path.GetExtension(origin);

            var builder = new StringBuilder();

            builder.Append(filename);
            builder.Append(" ");
            builder.Append(currentString);
            builder.Append(extension);

            _current += Step;

            string result = builder.ToString();
            return result;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public IRule Parse(string line)
        {
            var tokens = line.Split(new string[] { " " },
                StringSplitOptions.None);
            var data = tokens[1];
            var attributes = data.Split(new string[] { "," },
                StringSplitOptions.None);
            var pairs0 = attributes[0].Split(new string[] { "=" },
                StringSplitOptions.None);
            var pairs1 = attributes[1].Split(new string[] { "=" },
                StringSplitOptions.None);

            var rule = new AddCounterRule();
            rule.Start = int.Parse(pairs0[1]);
            rule.Step = int.Parse(pairs1[1]);

            return rule;
        }
    }
}
