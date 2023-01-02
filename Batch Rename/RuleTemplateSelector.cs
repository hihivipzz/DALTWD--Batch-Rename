using Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Batch_Rename
{
    public class RuleTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            IRule rule = item as IRule;
            var result = rule.parameterTemplate();
            
            return result;
        }
    }
}
