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
    public class AddCounter : IRule
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

        public AddCounter()
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


        public Dictionary<string, object> CreateRecord()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            result.Add("Name", Name);
            result.Add("Start", Start);
            result.Add("Step", Step);
            result.Add("IsChecked", IsChecked);

            return result;
        }


        public IRule Parse(Dictionary<string, object> data)
        {
            int start = Convert.ToInt32(data["Start"]);
            int step = Convert.ToInt32(data["Step"]);
            bool isCheck = (bool)data["IsChecked"];

            var rule = new AddCounter
            {
                Start = start,
                Step = step,
                IsChecked = isCheck
            };

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
