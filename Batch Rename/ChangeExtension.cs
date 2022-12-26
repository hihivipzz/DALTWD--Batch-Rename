using System;
using System.IO;
using System.Text;

namespace Batch_Rename
{
    public class ChangeExtension : IRule
    {
        public string Extension { get; set; }

        public string Name => "Change Extension";

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public IRule Parse(string data)
        {
            var tokens = data.Split(new string[] { " " },
              StringSplitOptions.None);
            var parsedData = tokens[1];

            var pairs = parsedData.Split(new string[] { "=" },
                StringSplitOptions.None);

            var rule = new ChangeExtension
            {
                Extension = pairs[1]
            };

            return rule;
        }

        public string Rename(string origin)
        {
            string filename = Path.GetFileNameWithoutExtension(origin);

            var builder = new StringBuilder();

            builder.Append(filename);
            builder.Append(".");
            builder.Append(Extension);

            string result = builder.ToString();
            return result;
        }
    }
}
