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
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _ruleList.Add(new
            {
                Name = "suffix",
            });
            _ruleList.Add(new
            {
                Name = "prefix",
            });
            addRuleCombobox.ItemsSource = _ruleList;
        
        }

        private void BatchButton_Click(object sender, RoutedEventArgs e)
        {
            //
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            //
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            //
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            //
        }

        private void addMethodCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(addRuleCombobox.SelectedIndex != -1)
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
            screen.Multiselect= true;

            if(screen.ShowDialog()== true)
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

       
    }
}
