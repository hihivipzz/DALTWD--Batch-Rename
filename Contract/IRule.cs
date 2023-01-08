using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Contract
{
    public interface IRule : ICloneable
    {
        string Rename(string origin);

        Dictionary<string, object> CreateRecord();
        IRule Parse(Dictionary<string,object> data);

        string Name { get; }

        DataTemplate parameterTemplate();

        bool IsChecked { get; }

    }
}
