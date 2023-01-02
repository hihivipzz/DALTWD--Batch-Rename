using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Contract;
using System.Windows;
using System.Windows.Markup;


namespace ConvertPascalCaseRule
{
    public class ConvertPascalCase : IRule, ICloneable
    {
        public string Name => "ConvertTo Pascal Case";
        public bool IsChecked { get; set; }

        public ConvertPascalCase()
        {
        }

        public IRule Parse(string data)
        {
            var tokens = data.Split(new string[] { " " },
                StringSplitOptions.None);
            var parsedData = tokens[1];

            var pairs = parsedData.Split(new string[] { "=" },
                StringSplitOptions.None);

            var rule = new ConvertPascalCase();
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
            string pattern = @"\w+";
            Regex rg = new Regex(pattern);
            MatchCollection match = rg.Matches(filename);

            string newFilename = "";
            for (int i = 0; i < match.Count; i++)
            {
                string value = match[i].Value.ToLower();
                string pascalCaseValue = $"{value[0].ToString().ToUpper()}{value.Substring(1)}";
                newFilename += pascalCaseValue;
            }

            var builder = new StringBuilder();

            builder.Append(newFilename);
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
                                
                            </Border>

                        </Expander>

                    </DataTemplate>"));

            var template = (DataTemplate)XamlReader.Load(ms);

            return template;
        }
    }
}
