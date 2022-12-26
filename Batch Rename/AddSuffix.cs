using System;
using System.IO;
using System.Text;

namespace Batch_Rename
{
    public class AddSuffix: IRule
    {
        public string Suffix { get; set; }

        public string Name => "Add Suffix";

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

            var rule = new AddSuffix
            {
                Suffix = pairs[1]
            };

            return rule;
        }

        public string Rename(string origin)
        {
            string filename = Path.GetFileNameWithoutExtension(origin);
            string extension = Path.GetExtension(origin);

            var builder = new StringBuilder();

            builder.Append(filename);
            builder.Append(" ");
            builder.Append(Suffix);
            builder.Append(extension);

            string result = builder.ToString();
            return result;
        }
    }
}
