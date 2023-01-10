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
using Contract;
using System.Reflection;
using System.Data;
using System.Xml.Linq;

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

        ObservableCollection<IRule> _ruleList =
            new ObservableCollection<IRule>();

        ObservableCollection<IRule> _activeRule =
            new ObservableCollection<IRule>();

        ObservableCollection<object> _presetList =
            new ObservableCollection<object>();

        PreviewRenameConverter converter;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string exePath = Assembly.GetExecutingAssembly().Location;
            string folder = System.IO.Path.GetDirectoryName(exePath);
            FileInfo[] fis = new DirectoryInfo(folder).GetFiles("*.dll");

            foreach(FileInfo fileInfo in fis)
            {
                var domain = AppDomain.CurrentDomain;
                Assembly assembly = domain.Load(AssemblyName.GetAssemblyName(fileInfo.FullName));

                Type[] types = assembly.GetTypes();

                foreach(var type in types)
                {
                    if(type.IsClass && typeof(IRule).IsAssignableFrom(type))
                    {
                        IRule rule = (IRule)Activator.CreateInstance(type);
                        RuleFactory.Register(rule);
                        _ruleList.Add(rule);
                    }
                }
            }
           
            addRuleCombobox.ItemsSource = _ruleList;
           
            this.Width = (double)(Settings.Default["ScreenW"]);
            this.Height = (double)(Settings.Default["ScreenH"]);
            this.Top = (double)(Settings.Default["ScreenT"]);
            this.Left = (double)(Settings.Default["ScreenL"]);
            var presetfilename = (string)Settings.Default["LastPreset"];
            //Last preset
            if (File.Exists(presetfilename))
            {
                var info = new FileInfo(presetfilename);
                var shortName = System.IO.Path.GetFileNameWithoutExtension(info.Name);
                var extension = System.IO.Path.GetExtension(info.Name);
                if (extension != ".json")
                {
                    System.Windows.Forms.MessageBox.Show("file is not json file");
                    return;
                }
                List<Dictionary<string, object>> savedRule = (List<Dictionary<string, object>>)JsonUtils.LoadJson(presetfilename);
                _activeRule.Clear();
                foreach (var dictionary in savedRule)
                {
                    object rule = RuleFactory.Parse(dictionary);
                    if (rule != null)
                    {

                        _activeRule.Add((IRule)rule);
                    }
                }
                ActiveRulesListBox.ItemsSource = _activeRule;
                var preset = new Preset()
                {
                    Name = shortName,
                    Path = presetfilename,
                };
                foreach (IRule rule in _activeRule)
                {
                    preset.Rules.Add((IRule)rule.Clone());

                }
                _presetList.Add(preset);
                PresetCombobox.ItemsSource = _presetList;
                PresetCombobox.SelectedItem = preset;
                System.Windows.Forms.MessageBox.Show("Load last preset success");
                converter = (PreviewRenameConverter)FindResource("previewRenameConverter");
                converter.Rules = new List<IRule>(_activeRule);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Load Last Preset fail");
            }


        }

        private void BatchButton_Click(object sender, RoutedEventArgs e)
        {


            //check
            if(ActiveRulesListBox.Items.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Add Method Before Batching!", "Error Detected in Input", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return ;
            }
            else if (sourceListView.Items.Count == 0 && sourceFolderListView.Items.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Choose File Or Folder Before Batching!", "Error Detected in Input", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return;
            }
            string targetPath = "";
            List<IRule> rulesForFile = new List<IRule>();
            List<IRule> rulesForFolder = new List<IRule>();
            MessageBoxResult result = System.Windows.MessageBox.Show("Are you sure to rename file", "My App", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    foreach (IRule rule in _activeRule)
                    {
                        rulesForFile.Add((IRule)rule.Clone());
                        rulesForFolder.Add((IRule)rule.Clone());
                    }

                    //Newname file
                    foreach (FileName file in _sourceFiles)
                    {
                        file.NewName = file.ShortName;
                        foreach (var rule in rulesForFile)
                        {
                            if (rule.IsChecked)
                            {
                                file.NewName = rule.Rename(file.NewName);
                            }
                        }
                        
                    }
                    //newname folder
                    foreach (FolderName file in _sourceFolders)
                    {
                        file.NewName = file.ShortName;
                        foreach (var rule in rulesForFolder)
                        {
                            if (rule.IsChecked)
                            {
                                file.NewName = rule.Rename(file.NewName);
                            }

                        }
                        
                    }

                    //change
                    foreach (FileName file in _sourceFiles)
                    {
                        File.Move(file.FullPath + "/" + file.ShortName, file.FullPath + "/" + file.NewName);
                    }
                    foreach (FolderName file in _sourceFolders)
                    {
                        Directory.Move(file.FullPath + "/" + file.ShortName, file.FullPath + "/" + file.NewName);
                    }

                    foreach (FileName file in _sourceFiles)
                    {
                        file.ShortName = file.NewName;
                    };
                    foreach (FileName file in _sourceFolders)
                    {
                        file.ShortName = file.NewName;
                    }

                    System.Windows.Forms.MessageBox.Show("Batch success", "Process done");
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            sourceListView.Items.Refresh();
            sourceFolderListView.Items.Refresh();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            _activeRule.Clear();
            ActiveRulesListBox.ItemsSource = _activeRule;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
           _sourceFiles.Clear();
            _sourceFolders.Clear();
        }

        private void addMethodCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(addRuleCombobox.SelectedIndex != -1)
            {
                var item = (IRule)addRuleCombobox.SelectedItem;
                string name = item.Name;

                IRule rule = RuleFactory.Instance().createRule(name);
                _activeRule.Add(rule);


                ActiveRulesListBox.ItemsSource = _activeRule;
               
                addRuleCombobox.SelectedIndex = -1;

                
            }

        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            askForSave();
            var screen = new Microsoft.Win32.OpenFileDialog();
            if (screen.ShowDialog() == true)
            {
                string presetfilename = screen.FileName;
                var info = new FileInfo(presetfilename);
                var shortName = System.IO.Path.GetFileNameWithoutExtension(info.Name);
                var extension = System.IO.Path.GetExtension(info.Name); 
                if(extension != ".json")
                {
                    System.Windows.Forms.MessageBox.Show("file is not json file");
                    return;
                }
                List<Dictionary<string, object>> savedRule = (List<Dictionary<string, object>>)JsonUtils.LoadJson(presetfilename);
                _activeRule.Clear();
                foreach (var dictionary in savedRule)
                {
                    object rule = RuleFactory.Parse(dictionary);
                    if (rule != null)
                    {
                        
                        _activeRule.Add((IRule)rule);
                    }
                }
                ActiveRulesListBox.ItemsSource = _activeRule;
                var preset = new Preset()
                {
                    Name = shortName,
                    Path = presetfilename,
                };
                foreach (IRule rule in _activeRule)
                {
                    preset.Rules.Add((IRule)rule.Clone());
          
                }
                _presetList.Add(preset);
                PresetCombobox.ItemsSource = _presetList;
                PresetCombobox.SelectedItem= preset;
                System.Windows.Forms.MessageBox.Show("Load preset success");
            }            
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ActiveRulesListBox.Items.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Nothing To Save To Preset", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
            }
            else
            {
                Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                saveFileDialog.Filter = "JSON (*.json)|*.json";
                if (saveFileDialog.ShowDialog() == true)
                {
                    List<Dictionary<string, object>> saveRule = new List<Dictionary<string, object>>();

                    foreach (IRule rule in _activeRule)
                    {
                        saveRule.Add(rule.CreateRecord());
                    }

                    JsonUtils.WriteJson(saveRule, saveFileDialog.FileNames[0]);
                }
            }
            System.Windows.Forms.MessageBox.Show("Save preset success");
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new Microsoft.Win32.OpenFileDialog();
            screen.Multiselect= true;

            if(screen.ShowDialog()== true)
            {
                foreach (var fileName in screen.FileNames)
                {
                    var fullPath = fileName;
                    var info = new FileInfo(fullPath);
                    var shortName = info.Name;
                    var path = info.DirectoryName;

                    bool checkExist = false;
                    foreach(FileName file in _sourceFiles)
                    {
                        if(file.ShortName == shortName && file.FullPath == path)
                        {
                            checkExist = true;
                        }
                    }
                    if (!checkExist)
                    {
                        _sourceFiles.Add(new FileName
                        {
                            FullPath = path,
                            ShortName = shortName,
                        });
                    }
                    
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
                var path = info.DirectoryName;

                bool checkExist = false;
                foreach (FolderName file in _sourceFolders)
                {
                    if (file.ShortName == shortName && file.FullPath == path)
                    {
                        checkExist = true;
                    }
                }
                if (!checkExist)
                {
                    _sourceFolders.Add(new FolderName
                    {
                        FullPath = path,
                        ShortName = shortName,
                    });
                }

               
            }
            sourceFolderListView.ItemsSource = _sourceFolders;
        }

        private void AddFileFromFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new FolderBrowserDialog();

            if(screen.ShowDialog() == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(screen.SelectedPath))
            {
                string[] files = Directory.GetFiles(screen.SelectedPath,".",SearchOption.AllDirectories);
                
              
                foreach (var file in files)
                {
                    FileAttributes attr = File.GetAttributes(file);
                    bool isFolder = (attr & FileAttributes.Directory) == FileAttributes.Directory;
                    if (!isFolder)
                    {
                        var fullPath = file;
                        var info = new FileInfo(fullPath);
                        var shortName = info.Name;
                        var path = info.DirectoryName;

                        bool checkExist = false;
                        foreach (FileName _file in _sourceFiles)
                        {
                            if (_file.ShortName == shortName && _file.FullPath == path)
                            {
                                checkExist = true;
                            }
                        }
                        if (!checkExist)
                        {
                            _sourceFiles.Add(new FileName
                            {
                                FullPath = path,
                                ShortName = shortName,
                            });
                        }
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
            if (index != -1 )
            {
                var val = _activeRule[index];
                _activeRule.RemoveAt(index);
                _activeRule.Insert(0,val);
                ActiveRulesListBox.ItemsSource = _activeRule;
                ActiveRulesListBox.SelectedIndex = 0;
            }
        }

        private void ToUpRuleButton_Click(object sender, RoutedEventArgs e)
        {
            int index = ActiveRulesListBox.SelectedIndex;
            if (index != -1&& index!= 0)
            {
                (_activeRule[index], _activeRule[index - 1]) = (_activeRule[index - 1], _activeRule[index]);
                
                ActiveRulesListBox.ItemsSource = _activeRule;
                ActiveRulesListBox.SelectedIndex = index-1;
            }
        }

        private void ToDownRuleButton_Click(object sender, RoutedEventArgs e)
        {
            int index = ActiveRulesListBox.SelectedIndex;
            if (index != -1 && index != _activeRule.Count()-1)
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
                ActiveRulesListBox.SelectedIndex = _activeRule.Count() -1;
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
                foreach(var file in files)
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
                        });
                    }

                }
                sourceFolderListView.ItemsSource = _sourceFolders;
            }
        }

        public void DeleteThisRuleButton_Click(object sender, RoutedEventArgs e)
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
            askForSave();
            Settings.Default["ScreenW"] = this.Width;
            Settings.Default["ScreenH"] = this.Height;
            Settings.Default["ScreenL"] = this.Left;
            Settings.Default["ScreenT"] = this.Top;
            if(PresetCombobox.SelectedIndex!= -1)
            {
               var preset = (Preset)PresetCombobox.SelectedItem;
                Settings.Default["LastPreset"] = preset.Path;
            }
            Settings.Default.Save();
            this.Close();
        }

        private void PreviewFile_Click(object sender, RoutedEventArgs e)
        {
            List<IRule> rules= new List<IRule>();
            foreach (IRule rule in _activeRule)
            {
                rules.Add((IRule)rule.Clone());
            }

            foreach (FileName file in _sourceFiles)
            {
                file.NewName = file.ShortName;
                foreach (var rule in rules)
                {
                    if (rule.IsChecked)
                    {
                        file.NewName = rule.Rename(file.NewName);
                    }
                    
                }
            }
        }

        private void PreviewFolder_Click(object sender, RoutedEventArgs e)
        {
            List<IRule> rules = new List<IRule>();
            foreach (IRule rule in _activeRule)
            {
                rules.Add((IRule)rule.Clone());
            }

            foreach (FolderName file in _sourceFolders)
            {
                file.NewName = file.ShortName;
                foreach (var rule in rules)
                {
                    if (rule.IsChecked)
                    {
                        file.NewName = rule.Rename(file.NewName);
                    }
                    
                }
            }
        }

        private void PresetCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PresetCombobox.SelectedIndex != -1)
            {
                Preset preset = (Preset)PresetCombobox.SelectedItem;
                _activeRule.Clear();
                foreach (IRule rule in preset.Rules)
                {
                    _activeRule.Add((IRule)rule.Clone());

                }
            }
        }

        private void askForSave()
        {
            if (ActiveRulesListBox.Items.Count != 0)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Would you like save current preset?", "My App", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                        saveFileDialog.Filter = "JSON (*.json)|*.json";
                        if (saveFileDialog.ShowDialog() == true)
                        {
                            List<Dictionary<string, object>> saveRule = new List<Dictionary<string, object>>();

                            foreach (IRule rule in _activeRule)
                            {
                                saveRule.Add(rule.CreateRecord());
                            }

                            JsonUtils.WriteJson(saveRule, saveFileDialog.FileNames[0]);
                            System.Windows.Forms.MessageBox.Show("Save preset success");
                        }
                        break;
                    case MessageBoxResult.No:
                        break;
                }
                
            }
            
        }
    }
}
