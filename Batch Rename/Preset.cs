using Contract;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Batch_Rename
{
    internal class Preset: INotifyCollectionChanged
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public List<IRule> Rules { get; set; }

        public Preset()
        {
            Rules = new List<IRule>();
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}
