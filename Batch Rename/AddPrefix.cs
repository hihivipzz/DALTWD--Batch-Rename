using System;
using System.Text;

namespace Batch_Rename
{
    public class AddPrefix : IRule
    {
        public string Prefix { get; set; }

        public string Name => "Add Prefix";

        public AddPrefix()
        {
            Prefix = "";
        }

        public IRule Parse(string data)
        {
            var tokens = data.Split(new string[] { " " },
                StringSplitOptions.None);
            var parsedData = tokens[1];

            var pairs = parsedData.Split(new string[] { "=" },
                StringSplitOptions.None);

            var rule = new AddPrefix
            {
                Prefix = pairs[1]
            };
            return rule;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public string Rename(string origin)
        {
            var builder = new StringBuilder();
            builder.Append(Prefix);
            builder.Append(" ");
            builder.Append(origin);

            string result = builder.ToString();
            return result;
        }
    }
}
