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

namespace DeleteSpaceBeginEndRule
{
    public class DeleteSpaceBeginEnd : IRule, ICloneable
    {

        public string Name => "Delete Space Beginning Ending";
        public bool IsChecked { get; set; }

        public DeleteSpaceBeginEnd()
        {

            IsChecked = true;
        }

        public DeleteSpaceBeginEnd(bool IsChecked)
        {

            this.IsChecked = IsChecked;
        }

        public Dictionary<string, object> CreateRecord()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            result.Add("Name", Name);
            result.Add("IsChecked", IsChecked);

            return result;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public IRule Parse(Dictionary<string,object> data)
        {
            bool isCheck = (bool)data["IsChecked"];
            var rule = new DeleteSpaceBeginEnd(IsChecked = isCheck);

            return rule;
        }

        public string Rename(string origin)
        {
            string filename = Path.GetFileNameWithoutExtension(origin);
            string extension = Path.GetExtension(origin);

            var builder = new StringBuilder();

            string pattern = @"\S[\s\S]+\S";
            Regex rg = new Regex(pattern);
            MatchCollection match = rg.Matches(filename);

            string newFilename = "";
            for (int i = 0; i < match.Count; i++)
            {
                newFilename += match[i].Value;
            }

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

                           

                        </Expander>

                    </DataTemplate>"));

            var template = (DataTemplate)XamlReader.Load(ms);

            return template;
        }
    }
}
