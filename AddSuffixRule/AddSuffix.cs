using Contract;
using System;

namespace AddSuffixRule
{
    public class AddSuffix: IRule
    {
        public string Suffix { get; set; }

        public string Name => "AddSuffix";

        public IRule? Parse(string data)
        {
            AddSuffix result = new AddSuffix();

            return result;
        }

        public string Rename(string origin)
        {
            string newName = $"{Suffix} {origin}";

            return newName;
        }
    }
}
