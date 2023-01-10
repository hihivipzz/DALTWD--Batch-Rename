using Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Batch_Rename
{
    public class RuleFactory
    {
        static Dictionary<string, IRule> _prototypes = new Dictionary<string, IRule>();

        public static void Register(IRule prototype)
        {
            _prototypes.Add(prototype.Name, prototype);
        }

        private static RuleFactory _instance = null;
        public static RuleFactory Instance()
        {
            if (_instance == null)
            {
                _instance = new RuleFactory();
            }

            return _instance;
        }

        private RuleFactory()
        {

        }

        public IRule createRule(string name)
        {
            IRule result= null;
            if (_prototypes.ContainsKey(name))
            {
                result = (IRule)(_prototypes[name] as ICloneable).Clone();
            }

            return result;
        }

        public static object Parse(Dictionary<string, object> data)
        {

            if (data.ContainsKey("Name"))
            {
                string token = (string)data["Name"];
                if (_prototypes.ContainsKey(token))
                {
                    var rule = _prototypes[token].Parse(data);
                    return rule;
                }
            }
            return null;
        }

    }
}
