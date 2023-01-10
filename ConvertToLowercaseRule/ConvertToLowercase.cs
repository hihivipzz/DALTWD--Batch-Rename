using Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace ConvertToLowercaseRule
{
    public class ConvertToLowercase : IRule, ICloneable
    {
        public string Name => "Convert To Lowercase";
        public bool IsChecked { get; set; }

        public ConvertToLowercase()
        {
            
            IsChecked = true;
        }

        public Dictionary<string, object> CreateRecord()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            result.Add("Name", Name);
            result.Add("IsChecked", IsChecked);

            return result;
        }

        public IRule Parse(Dictionary<string, object> data)
        {
            bool isCheck = (bool)data["IsChecked"];
            var rule = new ConvertToLowercase { IsChecked = isCheck };
            return rule;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public string Rename(string origin)
        {
            string filename = Path.GetFileNameWithoutExtension(origin);
            string extension = Path.GetExtension(origin);
            filename = filename.ToLower();

            var builder = new StringBuilder();

            builder.Append(filename);
            builder.Append(extension);

            string result = builder.ToString();
            return result;
        }

        public DataTemplate parameterTemplate()
        {
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(@"<DataTemplate xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                                                                 xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""  
                                                                 xmlns:local=""clr-namespace:Batch_Rename"">
                        <Expander Template=""{DynamicResource SimpleExpanderTemp}""  Margin=""0,5,0,5"">

                            <Expander.Header >

                                <StackPanel Orientation=""Horizontal"" Width=""284"" Height=""25"">

                                    <TextBlock Width=""232"" Padding=""5,4,3,0"" Text=""{Binding Name}""/> 


                                    <CheckBox Margin=""5"" IsChecked = ""{Binding IsChecked}""></CheckBox>
                                    <Button Background=""Transparent"" BorderThickness=""0"" Name=""DeleteThisRuleButton"" Command=""ApplicationCommands.New"">
                                        <Image Source=""/images/remove.png"" Height=""20"" Width=""20""/>
                                    </Button>
                                </StackPanel>
                            </Expander.Header>

                           

                        </Expander>

                    </DataTemplate>"));

            var template = (DataTemplate)XamlReader.Load(ms);

            return template;
        }
    }
}
