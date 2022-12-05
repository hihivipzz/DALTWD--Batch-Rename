using Contract;
using System;

namespace AddPrefixRule
{
    public class AddPrefix: IRule
    {
        public string Prefix { get; set; }

        public string Name => "AddPrefix";

        public IRule?  Parse(string data)
        {
            AddPrefix result = new AddPrefix();

            return result;
        }

        public string Rename(string origin)
        {
            string newName = $"{Prefix} {origin}";

            return newName;
        }
    }
}
