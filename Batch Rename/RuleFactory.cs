﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Batch_Rename
{
    public class RuleFactory
   {
        static Dictionary<string, IRule> _protypes = new Dictionary<string, IRule>();
        public static void Register(IRule prototype)
       {
            _protypes.Add(prototype.Name, prototype);
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


    }
}
