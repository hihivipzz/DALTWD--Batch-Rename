using System.Collections.Generic;
using System.Text;
using System.Windows.Documents;

namespace Batch_Rename
{
    public class RemoveSpecialChars : IRule
    {
        public List<char> BlackList { get; set; }

        public string Name => "Remove Special Chars";

        public RemoveSpecialChars()
        {
            BlackList = new List<char>();
        }

        public IRule Parse(string data)
        {
            RemoveSpecialChars result = new RemoveSpecialChars();

            return result;
        }

        public string Rename(string origin)
        {
            StringBuilder builder = new StringBuilder();
            const string replacement = " ";

            foreach(var c in origin)
            {
                if (!BlackList.Contains(c))
                {
                    builder.Append(c);
                }
                else
                {
                    builder.Append(replacement);
                }
            }

            string result = builder.ToString();
            return result;
        }

        public object Clone()
        {
            throw new System.NotImplementedException();
        }
    }
}
