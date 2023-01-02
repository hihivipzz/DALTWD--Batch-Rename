using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contract;
using System.Windows;
using System.Windows.Markup;

namespace AddCounterRule
{
    public class AddCounterRule : IRule
    {
        private int _current = 0;
        private int _start = 0;
        public bool IsChecked { get; set; }

        public int Start
        {
            get => _start; set
            {
                _start = value;

                _current = value;
            }
        }
        public int Step { get; set; }

        public string Name => "Add Counter";

        public AddCounterRule()
        {
            Start = 1;
            Step = 1;
            IsChecked = true;
        }
        public string Rename(string origin)
        {
            string currentString = _current.ToString();
            if (_current <= 9 && _current >= 0)
            {
                currentString = $"0{currentString}";
            }

            string filename = Path.GetFileNameWithoutExtension(origin);
            string extension = Path.GetExtension(origin);

            var builder = new StringBuilder();

            builder.Append(filename);
            builder.Append(" ");
            builder.Append(currentString);
            builder.Append(extension);

            _current += Step;

            string result = builder.ToString();
            return result;
        }

        public object Clone()
        {
            return MemberwiseClone();
            
        }

        public IRule Parse(string line)
        {
            var tokens = line.Split(new string[] { " " },
                StringSplitOptions.None);
            var data = tokens[1];
            var attributes = data.Split(new string[] { "," },
                StringSplitOptions.None);
            var pairs0 = attributes[0].Split(new string[] { "=" },
                StringSplitOptions.None);
            var pairs1 = attributes[1].Split(new string[] { "=" },
                StringSplitOptions.None);

            var rule = new AddCounterRule();
            rule.Start = int.Parse(pairs0[1]);
            rule.Step = int.Parse(pairs1[1]);

            return rule;
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
                                        <TextBlock FontSize=""12"" Text=""Start: ""></TextBlock>
                                        <TextBox Width=""120"" Text=""{Binding Start, UpdateSourceTrigger=PropertyChanged}""></TextBox>
                                    </StackPanel>
                                    <StackPanel Orientation=""Horizontal"" Height=""19"" Margin=""2"">
                                        <TextBlock FontSize=""12"" Text=""Step: ""></TextBlock>
                                        <TextBox Width=""120"" Text=""{Binding Step, UpdateSourceTrigger=PropertyChanged}""></TextBox>
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
