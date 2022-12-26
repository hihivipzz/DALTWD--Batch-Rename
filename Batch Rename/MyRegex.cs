using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Batch_Rename
{
    class MyRegex
    {
        static private MyRegex _instance = null;
        readonly string filename = "MyRegex";

        private MyRegex()
        {
            filename = "";
        }
        static public MyRegex instance()
        {
            if (_instance == null)
            {
                _instance = new MyRegex();
            }
            return _instance;
        }
        public void test()
        {

        }
    }
}
