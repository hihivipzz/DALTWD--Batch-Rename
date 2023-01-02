using Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace AddSuffixRule
{
    public class AddSuffix : IRule, ICloneable
    {
        public string Suffix { get; set; }
        public bool IsChecked { get; set; }

        public string Name => "Add Suffix";

        public AddSuffix()
        {
            Suffix = "";
            IsChecked = true;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public IRule Parse(string data)
        {
            var tokens = data.Split(new string[] { " " },
              StringSplitOptions.None);
            var parsedData = tokens[1];

            var pairs = parsedData.Split(new string[] { "=" },
                StringSplitOptions.None);

            var rule = new AddSuffix
            {
                Suffix = pairs[1]
            };

            return rule;
        }

        public string Rename(string origin)
        {
            string filename = Path.GetFileNameWithoutExtension(origin);
            string extension = Path.GetExtension(origin);

            var builder = new StringBuilder();

            builder.Append(filename);
            builder.Append(" ");
            builder.Append(Suffix);
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

                            <Border BorderThickness=""2"" BorderBrush=""DarkGray"" Margin=""5,5,5,0"">
                                <StackPanel Orientation=""Vertical"" Background=""White"">
                                    <StackPanel Orientation=""Horizontal"" Height=""19"" Margin=""2"">
                                        <TextBlock FontSize=""12"" Text=""Suffix: ""></TextBlock>
                                        <TextBox Width=""120"" Text=""{Binding Suffix, UpdateSourceTrigger=PropertyChanged}""></TextBox>
                                    </StackPanel>
                                </StackPanel>
                            </Border>

                        </Expander>

                    </DataTemplate>"));

            var template = (DataTemplate)XamlReader.Load(ms);

            return template;
        }
    }
}
