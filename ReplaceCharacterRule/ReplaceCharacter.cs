using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contract;
using System.Windows;
using System.Windows.Controls;
using System.Net.Http.Headers;
using System.Windows.Markup;
using System.ComponentModel;

namespace ReplaceCharacterRule
{
    public class ReplaceCharacter : IRule
    {
        public string OldCharacter { get; set; }
        public string NewCharacter { get; set; }
        public string Name => "ReplaceCharacter";
        public bool IsChecked { get; set; }

        public ReplaceCharacter()
        {
            OldCharacter = "";
            NewCharacter = "";
            IsChecked= true;
        }

        public ReplaceCharacter(string OldCharacter,string NewCharacter,bool IsChecked)
        {
            this.OldCharacter = OldCharacter;
            this.NewCharacter = NewCharacter;
            this.IsChecked = IsChecked;
        }

        public Dictionary<string, object> CreateRecord()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            result.Add("Name", Name);
            result.Add("OldCharacter", OldCharacter);
            result.Add("NewCharacter", NewCharacter);
            result.Add("IsChecked", IsChecked);

            return result;
        }

        public IRule Parse(Dictionary<string,object> data)
        {
            string oldCharacter = (string)data["OldCharacter"];
            string newCharacter = (string)data["NewCharacter"];
            bool isChecked = (bool)data["IsChecked"];

            var rule = new ReplaceCharacter {
                OldCharacter = oldCharacter,
                NewCharacter= newCharacter,
                IsChecked = isChecked
               };
            return rule;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public string Rename(string origin)
        {
            string extension = Path.GetExtension(origin);
            string filename = Path.GetFileNameWithoutExtension(origin);
            if(OldCharacter!= "")
            {
                filename = filename.Replace(OldCharacter, NewCharacter);
            }
            

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
                        <Expander Template=""{DynamicResource SimpleExpanderTemp}""  >

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
                                        <TextBlock FontSize=""12"" Text=""Old character: ""></TextBlock>
                                        <TextBox Width=""120"" Text=""{Binding OldCharacter, UpdateSourceTrigger=PropertyChanged}""></TextBox>
                                    </StackPanel>
                                    <StackPanel Orientation=""Horizontal"" Height=""19"" Margin=""2"">
                                        <TextBlock FontSize=""12"" Text=""New character: ""></TextBlock>
                                        <TextBox Width=""120"" Text=""{Binding NewCharacter, UpdateSourceTrigger=PropertyChanged}""></TextBox>
                                    </StackPanel>
                                    <StackPanel  Orientation=""Horizontal"" Height=""19"" Margin=""2"">
                                        <TextBlock FontSize=""12"" Text=""Apply to: ""></TextBlock>
                                        <ComboBox SelectedIndex=""0"" Padding=""2,1,0,0"" Width=""115"">
                                            <ComboBoxItem >
                                                <TextBlock Text=""Name"" FontSize=""11""></TextBlock>
                                            </ComboBoxItem>
                                            <ComboBoxItem >
                                                <TextBlock Text=""Extension"" FontSize=""11""></TextBlock>
                                            </ComboBoxItem>
                                            <ComboBoxItem >
                                                <TextBlock Text=""All"" FontSize=""11""></TextBlock>
                                            </ComboBoxItem>
                                        </ComboBox>
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
