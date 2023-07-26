using AlgoTerminal.NewViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace AlgoTerminal.NewView
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : Window
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private string _keyPath = System.Environment.Is64BitOperatingSystem ? @"SOFTWARE\Wow6432Node\OpenControls\WpfDockManagerDemo" : @"SOFTWARE\OpenControls\WpfDockManagerDemo";

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(_keyPath);
            if (key == null)
            {
                key = Registry.CurrentUser.CreateSubKey(_keyPath);
            }
            else
            {
                Object obj = key.GetValue("Height");
                if (obj != null)
                {
                    Height = Convert.ToDouble(obj);
                }
                obj = key.GetValue("Width");
                if (obj != null)
                {
                    Width = Convert.ToDouble(obj);
                }
                obj = key.GetValue("Top");
                if (obj != null)
                {
                    Top = Convert.ToDouble(obj);
                }
                obj = key.GetValue("Left");
                if (obj != null)
                {
                    Left = Convert.ToDouble(obj);
                }
            }

            _layoutManager.Initialise();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(_keyPath, true);
            if (key == null)
            {
                key = Registry.CurrentUser.CreateSubKey(_keyPath, true);
            }

            key.SetValue("Height", ActualHeight);
            key.SetValue("Width", ActualWidth);
            key.SetValue("Top", Top);
            key.SetValue("Left", Left);

            if (_layoutManager != null)
            {
                _layoutManager.Shutdown();
            }
        }
        private void LoadLayout(string path)
        {
            try
            {
                _layoutManager.LoadLayoutFromFile(path);
                HomeViewModel mainViewModel = DataContext as HomeViewModel;
                mainViewModel.LayoutLoaded = true;
            }
            catch (Exception exception)
            {
                MessageBox.Show("Unable to load layout: " + exception.Message);
            }
        }

        private void LoadLayout()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog == null)
            {
                return;
            }

            dialog.Filter = "Layout Files (*.xml)|*.xml";
            dialog.CheckFileExists = true;
            if (dialog.ShowDialog() != DialogResult)
            {
                return;
            }

            try
            {
                LoadLayout(dialog.FileName);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Unable to load layout: " + exception.Message);
            }
        }

        private void SaveLayout()
        {
           OpenFileDialog dialog = new OpenFileDialog();
            if (dialog == null)
            {
                return;
            }

            dialog.Filter = "Layout Files (*.xml)|*.xml";
            dialog.CheckFileExists = false;
            if (dialog.ShowDialog() != DialogResult)
            {
                return;
            }

            try
            {
                _layoutManager.SaveLayoutToFile(dialog.FileName);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Unable to save layout: " + exception.Message);
            }
        }

        private void LoadDefaultLayout()
        {
            try
            {
                LoadLayout(null);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Unable to load layout: " + exception.Message);
            }
        }

        private void _buttonWindow_Click(object sender, RoutedEventArgs e)
        {
            ContextMenu contextMenu = new ContextMenu();
            MenuItem menuItem = null;

            menuItem = new MenuItem();
            menuItem.Header = "Load Default Layout";
            menuItem.IsChecked = false;
            menuItem.Command = new OpenControls.Wpf.Utilities.Command(delegate { LoadDefaultLayout(); }, delegate { return true; });
            contextMenu.Items.Add(menuItem);

            menuItem = new MenuItem();
            menuItem.Header = "Load Layout";
            menuItem.IsChecked = false;
            menuItem.Command = new OpenControls.Wpf.Utilities.Command(delegate { LoadLayout(); }, delegate { return true; });
            contextMenu.Items.Add(menuItem);

            menuItem = new MenuItem();
            menuItem.Header = "Save Layout";
            menuItem.IsChecked = false;
            menuItem.Command = new OpenControls.Wpf.Utilities.Command(delegate { SaveLayout(); }, delegate { return true; });
            contextMenu.Items.Add(menuItem);

            contextMenu.IsOpen = true;
        }

        private void _buttonTools_Click(object sender, RoutedEventArgs e)
        {
            HomeViewModel mainViewModel = DataContext as HomeViewModel;
            System.Diagnostics.Trace.Assert(mainViewModel != null);

            ContextMenu contextMenu = new ContextMenu();
            MenuItem menuItem = null;

            menuItem = new MenuItem();
            menuItem.Header = "Logger";
            menuItem.IsChecked = mainViewModel.LoggerUcViewModelVisible;
            menuItem.Command = new OpenControls.Wpf.Utilities.Command(delegate { mainViewModel.LoggerUcViewModelVisible = !mainViewModel.LoggerUcViewModelVisible; }, delegate { return true; });
            contextMenu.Items.Add(menuItem);

          

            contextMenu.IsOpen = true;
        }

        private void _buttonDocuments_Click(object sender, RoutedEventArgs e)
        {
            //HomeViewModel mainViewModel = DataContext as HomeViewModel;
            //System.Diagnostics.Trace.Assert(mainViewModel != null);

            //ContextMenu contextMenu = new ContextMenu();
            //MenuItem menuItem = null;

            //menuItem = new MenuItem();
            //menuItem.Header = mainViewModel.DocumentOne.URL;
            //menuItem.IsChecked = mainViewModel.DocumentOneVisible;
            //menuItem.Command = new OpenControls.Wpf.Utilities.Command(delegate { mainViewModel.DocumentOneVisible = !mainViewModel.DocumentOneVisible; }, delegate { return true; });
            //contextMenu.Items.Add(menuItem);

           

            //contextMenu.IsOpen = true;
        }
    }
}
