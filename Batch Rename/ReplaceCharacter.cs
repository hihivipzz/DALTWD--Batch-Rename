using System;
using System.IO;
using System.Text;
namespace Batch_Rename
{
    public class ReplaceCharacter : IRule
    {
        public string OldCharacter { get; set; }
        public string NewCharacter { get; set; }
        public string Name => "Replace Character";

        public ReplaceCharacter()
        {
            OldCharacter = "";
            NewCharacter = "";
        }

        public IRule Parse(string data)
        {
            var tokens = data.Split(new string[] { " " },
                StringSplitOptions.None);
            var parsedData = tokens[1];

            var pairs = parsedData.Split(new string[] { "=" },
                StringSplitOptions.None);

            var rule = new ReplaceCharacter
            {
                OldCharacter= pairs[1],
                NewCharacter = pairs[2],
            };
            return rule;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public string Rename(string origin)
        {
            string extension = Path.GetExtension(origin);
            string filename = Path.GetFileNameWithoutExtension(origin);
            filename = filename.Replace(OldCharacter, NewCharacter);
            
            var builder = new StringBuilder();
            
            builder.Append(filename);
            builder.Append(extension);

            string result = builder.ToString();
            return result;
        }
    }
}
