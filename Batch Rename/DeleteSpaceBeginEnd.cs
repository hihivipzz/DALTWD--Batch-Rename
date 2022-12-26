using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Batch_Rename
{
    public class DeleteSpaceBeginEnd : IRule
    {

        public string Name => "Delete Space Beginning Ending";

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

            var rule = new DeleteSpaceBeginEnd();

            return rule;
        }

        public string Rename(string origin)
        {
            string filename = Path.GetFileNameWithoutExtension(origin);
            string extension = Path.GetExtension(origin);

            var builder = new StringBuilder();

            string pattern = @"\S[\s\S]+\S";
            Regex rg = new Regex(pattern);
            MatchCollection match = rg.Matches(filename);

            string newFilename = "";
            for (int i = 0; i < match.Count; i++)
            {
                newFilename += match[i].Value;
            }

            builder.Append(newFilename);
            builder.Append(extension);

            string result = builder.ToString(); 
            return result;
        }
    }
}
