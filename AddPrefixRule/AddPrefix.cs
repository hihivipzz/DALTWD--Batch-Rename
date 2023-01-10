using Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace AddPrefixRule
{
    public class AddPrefix : IRule
    {
        public string Prefix { get; set; }
        public bool IsChecked { get; set; }

        public string Name => "Add Prefix";

        public AddPrefix()
        {
            Prefix = "";
            IsChecked = true;
        }

        public Dictionary<string, object> CreateRecord()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            result.Add("Name", Name);
            result.Add("Prefix", Prefix);
            result.Add("IsChecked", IsChecked);

            return result;
        }
       
        public IRule Parse(Dictionary<string, object> data)
        {

            string prefix = (string)data["Prefix"];
            bool isCheck = (bool)data["IsChecked"];

            var rule = new AddPrefix
            {
                Prefix = prefix,
                IsChecked = isCheck
            };
            return rule;
        }

        public object Clone()
        {
            return MemberwiseClone();

        }

        public string Rename(string origin)
        {
            var builder = new StringBuilder();
            builder.Append(Prefix);
            builder.Append(" ");
            builder.Append(origin);

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
                                        <TextBlock FontSize=""12"" Text=""Prefix: ""></TextBlock>
                                        <TextBox Width=""120"" Text=""{Binding Prefix, UpdateSourceTrigger=PropertyChanged}""></TextBox>
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
