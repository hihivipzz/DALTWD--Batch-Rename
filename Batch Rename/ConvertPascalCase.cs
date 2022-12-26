
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Batch_Rename
{
    public class ConvertPascalCase : IRule
    {
        public string Name => "ConvertTo Pascal Case";

        public ConvertPascalCase()
        {
        }

        public IRule Parse(string data)
        {
            var tokens = data.Split(new string[] { " " },
                StringSplitOptions.None);
            var parsedData = tokens[1];

            var pairs = parsedData.Split(new string[] { "=" },
                StringSplitOptions.None);

            var rule = new ConvertPascalCase();
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
            string pattern = @"\w+";
            Regex rg = new Regex(pattern);
            MatchCollection match = rg.Matches(filename);

            string newFilename = "";
            for (int i = 0; i < match.Count; i++)
            {
                string value = match[i].Value.ToLower();
                string pascalCaseValue = $"{value[0].ToString().ToUpper()}{value.Substring(1)}";
                newFilename += pascalCaseValue;
            }

            var builder = new StringBuilder();

            builder.Append(newFilename);
            builder.Append(extension);

            string result = builder.ToString();
            return result;
        }
    }
}
