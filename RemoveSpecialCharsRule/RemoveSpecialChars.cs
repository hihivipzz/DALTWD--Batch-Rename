﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contract;
using System.Windows;
using System.Windows.Markup;
using System.IO;

namespace RemoveSpecialCharsRule
{
    public class RemoveSpecialChars : IRule, ICloneable
    {
        public String BlackList { get; set; }

        public string Name => "Remove Special Chars";

        public bool IsChecked { get; set; }

        public RemoveSpecialChars()
        {
            BlackList = "";
            IsChecked = true;
        }

        public Dictionary<string, object> CreateRecord()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            result.Add("Name", Name);
            result.Add("BlackList", BlackList);
            result.Add("IsChecked", IsChecked);

            return result;
        }

        public IRule Parse(Dictionary<string, object> data)
        {
            string blackList = (string)data["BlackList"];
            bool isChecked = (bool)data["IsChecked"];

            RemoveSpecialChars result = new RemoveSpecialChars
            {
                BlackList = blackList,
                IsChecked = isChecked
            };
            return result;
        }

        public string Rename(string origin)
        {
            StringBuilder builder = new StringBuilder();
            const string replacement = "";

            foreach (var c in origin)
            {
                if (!BlackList.Contains(c))
                {
                    builder.Append(c);
                }
                else
                {
                    builder.Append(replacement);
                }
            }

            string result = builder.ToString();
            return result;
        }

        public object Clone()
        {
            return MemberwiseClone();
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
                                        <TextBlock FontSize=""12"" Text=""BlackList: ""></TextBlock>
                                        <TextBox Width=""120"" Text=""{Binding BlackList, UpdateSourceTrigger=PropertyChanged}""></TextBox>
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
