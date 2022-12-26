using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using Batch_Rename.Properties;

namespace Batch_Rename
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        ObservableCollection<object> _sourceFiles =
            new ObservableCollection<object>();

        ObservableCollection<object> _sourceFolders =
            new ObservableCollection<object>();

        ObservableCollection<object> _ruleList =
             new ObservableCollection<object>();

        ObservableCollection<object> _activeRule =
            new ObservableCollection<object>();

        ////////////////////////////////////////////////////////////////////////////////
        IRule prefix = new AddPrefix() { Prefix = "CV" };
        IRule suffix = new AddSuffix() { Suffix = "CV" };
        IRule counter = new AddCounterRule() {Start = 1,Step = 1};
        IRule replaceChar = new ReplaceCharacter() { OldCharacter = "-", NewCharacter = "_" };
        IRule deleteSpace = new DeleteSpaceBeginEnd();
        IRule changeExtension = new ChangeExtension() { Extension = ""};
        IRule lowercase = new ConvertToLowercase();
        IRule convertPascalCase = new ConvertPascalCase();

        readonly Dictionary<string, object> mapRule = new Dictionary<string, object>();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            mapRule.Add(prefix.Name, prefix);
            mapRule.Add(suffix.Name, suffix);
            mapRule.Add(counter.Name, counter);
            mapRule.Add(deleteSpace.Name, deleteSpace);
            mapRule.Add(replaceChar.Name, replaceChar);
            mapRule.Add(changeExtension.Name, changeExtension);
            mapRule.Add(lowercase.Name,lowercase);
            mapRule.Add(convertPascalCase.Name, convertPascalCase);
            
            /////////////////////////////////////////

            foreach (KeyValuePair<string, object> value in mapRule)
            {
                _ruleList.Add(new { Name = value.Key });
            }
         
            addRuleCombobox.ItemsSource = _ruleList;
            
            ////////////////////////////////////////////
            
            Width = (double)Settings.Default["ScreenW"];
            Height = (double)Settings.Default["ScreenH"];
            MainWindow mainWindow = this;
            mainWindow.Top = (double)Settings.Default["ScreenT"];
            Left = (double)Settings.Default["ScreenL"];

            ////////////////////////////////////////////

        }

        private void BatchButton_Click(object sender, RoutedEventArgs e)
        {
            List<IRule> selectedRules = new List<IRule>();

            var v = new { Amount = 108, Message = "Hello" };

            for (int i = 0; i < ActiveRulesListBox.SelectedItems.Count; i++)
            {
                IRule rule = (IRule)mapRule[(string)GetValueFromAnonyType(ActiveRulesListBox.SelectedItems[i], "Name")];
                selectedRules.Add(rule);
            }

            for (int i = 0 ;i < _sourceFiles.Count;i++)
            {
                object fileName = _sourceFiles[i];
                
                string fullpath = (string)GetValueFromAnonyType(fileName, "FullPath");
                string oldName = (string)GetValueFromAnonyType(fileName, "ShortName");
                string newName = oldName;

                foreach (var rule in selectedRules)
                {
                    newName = rule.Rename(newName);
                }

                string newpath = fullpath.Replace(oldName, newName);
                File.Move(fullpath, newpath);
                
                _sourceFiles[i] = new { FullPath = newpath, ShortName = newName };
            }
        }


        private object GetValueFromAnonyType(object source, string key)
        {
            string result = source.GetType().GetProperty(key).GetValue(source, null).ToString();

            return result;
        }

        

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            sourceListView.Items.Refresh();
            sourceFolderListView.Items.Refresh();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            int index = ActiveRulesListBox.SelectedIndex;
            if (index != -1)
            {
                _activeRule.RemoveAt(index);
                ActiveRulesListBox.ItemsSource = _activeRule;
            }

        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            _activeRule.Clear();
            ActiveRulesListBox.ItemsSource = _activeRule;
        }

        private void addMethodCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (addRuleCombobox.SelectedIndex != -1)
            {
                _activeRule.Add(addRuleCombobox.SelectedItem);
                ActiveRulesListBox.ItemsSource = _activeRule;

                addRuleCombobox.SelectedIndex = -1;
            }

        }

        private void load_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new Microsoft.Win32.OpenFileDialog();
            screen.Multiselect = true;

            if (screen.ShowDialog() == true)
            {
                foreach (var fileName in screen.FileNames)
                {
                    var fullPath = fileName;
                    var info = new FileInfo(fullPath);
                    var shortName = info.Name;

                    _sourceFiles.Add(new
                    {
                        FullPath = fullPath,
                        ShortName = shortName,
                    });
                }
                sourceListView.ItemsSource = _sourceFiles;
            }
        }

        private void AddFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new FolderBrowserDialog();


            if (screen.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var fullPath = screen.SelectedPath;
                var info = new FileInfo(fullPath);
                var shortName = info.Name;

                _sourceFolders.Add(new
                {
                    FullPath = fullPath,
                    ShortName = shortName,
                });
            }
            sourceFolderListView.ItemsSource = _sourceFolders;
        }

        private void AddFileFromFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new FolderBrowserDialog();

            if (screen.ShowDialog() == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(screen.SelectedPath))
            {
                string[] files = Directory.GetFiles(screen.SelectedPath, ".", SearchOption.AllDirectories);


                foreach (var file in files)
                {
                    FileAttributes attr = File.GetAttributes(file);
                    bool isFolder = (attr & FileAttributes.Directory) == FileAttributes.Directory;
                    if (!isFolder)
                    {
                        var fullPath = file;
                        var info = new FileInfo(fullPath);
                        var shortName = info.Name;

                        _sourceFiles.Add(new
                        {
                            FullPath = fullPath,
                            ShortName = shortName,
                        });
                    }
                }

            }

            sourceListView.ItemsSource = _sourceFiles;
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new HelpWindown();
            screen.ShowDialog();
        }

        private void ToFirstRuleButton_Click(object sender, RoutedEventArgs e)
        {
            int index = ActiveRulesListBox.SelectedIndex;
            if (index != -1)
            {
                var val = _activeRule[index];
                _activeRule.RemoveAt(index);
                _activeRule.Insert(0, val);
                ActiveRulesListBox.ItemsSource = _activeRule;
                ActiveRulesListBox.SelectedIndex = 0;
            }
        }

        private void ToUpRuleButton_Click(object sender, RoutedEventArgs e)
        {
            int index = ActiveRulesListBox.SelectedIndex;
            if (index != -1 && index != 0)
            {
                (_activeRule[index], _activeRule[index - 1]) = (_activeRule[index - 1], _activeRule[index]);

                ActiveRulesListBox.ItemsSource = _activeRule;
                ActiveRulesListBox.SelectedIndex = index - 1;
            }
        }

        private void ToDownRuleButton_Click(object sender, RoutedEventArgs e)
        {
            int index = ActiveRulesListBox.SelectedIndex;
            if (index != -1 && index != _activeRule.Count() - 1)
            {
                (_activeRule[index], _activeRule[index + 1]) = (_activeRule[index + 1], _activeRule[index]);

                ActiveRulesListBox.ItemsSource = _activeRule;
                ActiveRulesListBox.SelectedIndex = index + 1;
            }
        }

        private void toBottomRuleButton_Click(object sender, RoutedEventArgs e)
        {
            int index = ActiveRulesListBox.SelectedIndex;
            if (index != -1)
            {
                var val = _activeRule[index];
                _activeRule.RemoveAt(index);
                _activeRule.Insert(_activeRule.Count(), val);
                ActiveRulesListBox.ItemsSource = _activeRule;
                ActiveRulesListBox.SelectedIndex = _activeRule.Count() - 1;
            }
        }

        private void ToFirstFileButton_Click(object sender, RoutedEventArgs e)
        {
            var items = sourceListView.SelectedItems;
            if (items.Count > 0)
            {
                var count = items.Count;
                for (int i = 0; i < count; i++)
                {
                    var item = items[0];
                    //Đang ở tab file

                    int index = sourceListView.Items.IndexOf(item);
                    if (TabControl.SelectedIndex == 0)
                    {
                        _sourceFiles.RemoveAt(index);
                        _sourceFiles.Insert(i, item);
                    }

                }
                sourceListView.ItemsSource = _sourceFiles;
            }

        }

        private void ToUpFileButton_Click(object sender, RoutedEventArgs e)
        {
            var items = sourceListView.SelectedItems;

            if (items.Count > 0)
            {
                var count = items.Count;
                for (int i = 0; i < count; i++)
                {
                    var item = items[0];
                    //Đang ở tab file

                    int index = sourceListView.Items.IndexOf(item);
                    if (TabControl.SelectedIndex == 0)
                    {
                        _sourceFiles.RemoveAt(index);
                        _sourceFiles.Insert(i, item);
                    }

                }
                sourceListView.ItemsSource = _sourceFiles;
            }

        }

        private void ToLowFileButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ToBottomFileButton_Click(object sender, RoutedEventArgs e)
        {
            var items = sourceListView.SelectedItems;
            if (items.Count > 0)
            {
                var count = items.Count;
                for (int i = 0; i < count; i++)
                {
                    var item = items[0];
                    //Đang ở tab file

                    int index = sourceListView.Items.IndexOf(item);
                    if (TabControl.SelectedIndex == 0)
                    {
                        _sourceFiles.RemoveAt(index);
                        _sourceFiles.Insert(_sourceFiles.Count(), item);
                    }

                }
                sourceListView.ItemsSource = _sourceFiles;
            }
        }

        private void sourceListView_Drop(object sender, System.Windows.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(System.Windows.DataFormats.FileDrop);
                foreach (var file in files)
                {
                    FileAttributes attr = File.GetAttributes(file);
                    bool isFolder = (attr & FileAttributes.Directory) == FileAttributes.Directory;
                    if (!isFolder)
                    {
                        var fullPath = file;
                        var info = new FileInfo(fullPath);
                        var shortName = info.Name;

                        _sourceFiles.Add(new
                        {
                            FullPath = fullPath,
                            ShortName = shortName,
                        });
                    }

                }
                sourceListView.ItemsSource = _sourceFiles;
            }
        }

        private void sourceFolderListView_Drop(object sender, System.Windows.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(System.Windows.DataFormats.FileDrop);
                foreach (var file in files)
                {
                    FileAttributes attr = File.GetAttributes(file);
                    bool isFolder = (attr & FileAttributes.Directory) == FileAttributes.Directory;
                    if (isFolder)
                    {
                        var fullPath = file;
                        var info = new FileInfo(fullPath);
                        var shortName = info.Name;
                        _sourceFolders.Add(new
                        {
                            FullPath = fullPath,
                            ShortName = shortName,
                            DirectoryName = info.DirectoryName,
                        });
                    }

                }
                sourceFolderListView.ItemsSource = _sourceFolders;
            }
        }

        private void DeleteThisRuleButton_Click(object sender, RoutedEventArgs e)
        {
            int index = ActiveRulesListBox.SelectedIndex;
            if (index != -1)
            {
                _activeRule.RemoveAt(index);
                ActiveRulesListBox.ItemsSource = _activeRule;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Settings.Default["ScreenW"] = this.Width;
            Settings.Default["ScreenH"] = this.Height;
            Settings.Default["ScreenL"] = this.Left;
            Settings.Default["ScreenT"] = this.Top;
            Settings.Default.Save();
            this.Close();
        }
    }
}


/*
namespace Batch_Rename
{
    public static class TenGiCungDuocKhongQuanTrong
    {
        public static List<IRule> Clone(this List<IRule> source)
        {
            List<IRule> result = new List<IRule>();
            foreach (var rule in source)
            {
                result.Add((IRule)rule.Clone());
            }

            return result;
        }
    }
}*/
