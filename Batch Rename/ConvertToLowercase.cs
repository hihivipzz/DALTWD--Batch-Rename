using System;
using System.IO;
using System.Text;

namespace Batch_Rename
{
    public class ConvertToLowercase : IRule
    {
        public string Name => "Convert To Lowercase";

        public IRule Parse(string data)
        {
            var tokens = data.Split(new string[] { " " },
                StringSplitOptions.None);
            var parsedData = tokens[1];

            var pairs = parsedData.Split(new string[] { "=" },
                StringSplitOptions.None);

            var rule = new ConvertToLowercase();
            return rule;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public string Rename(string origin)
        {
            string filename = Path.GetFileNameWithoutExtension(origin);
            string extension = Path.GetExtension(origin);
            filename = filename.ToLower();
            
            var builder = new StringBuilder();

            builder.Append(filename);
            builder.Append(extension);

            string result = builder.ToString();
            return result;
        }
    }
}
