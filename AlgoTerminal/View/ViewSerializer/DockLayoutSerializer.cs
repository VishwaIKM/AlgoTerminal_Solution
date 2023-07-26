using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Windows.Input;
using AvalonDock;
using AvalonDock.Layout.Serialization;

namespace AlgoTerminal.View.ViewSerializer
{
    public static class DockLayoutSerializer
    {
        #region fields
        /// <summary>
        /// Backing store for LoadLayoutCommand dependency property
        /// </summary>
        private static readonly DependencyProperty LoadLayoutCommandProperty =
            DependencyProperty.RegisterAttached("LoadLayoutCommand",
            typeof(ICommand),
            typeof(DockLayoutSerializer),
            new PropertyMetadata(null, DockLayoutSerializer.OnLoadLayoutCommandChanged));

        /// <summary>
        /// Backing store for SaveLayoutCommand dependency property
        /// </summary>
        private static readonly DependencyProperty SaveLayoutCommandProperty =
            DependencyProperty.RegisterAttached("SaveLayoutCommand",
            typeof(ICommand),
            typeof(DockLayoutSerializer),
            new PropertyMetadata(null, DockLayoutSerializer.OnSaveLayoutCommandChanged));
        #endregion fields

        #region methods
        #region Load Layout
       
        public static ICommand GetLoadLayoutCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(LoadLayoutCommandProperty);
        }

        
        public static void SetLoadLayoutCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(LoadLayoutCommandProperty, value);
        }

      
        private static void OnLoadLayoutCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement framworkElement = d as FrameworkElement;     // Remove the handler if it exist to avoid memory leaks
            framworkElement.Loaded -= OnFrameworkElement_Loaded;

            var command = e.NewValue as ICommand;
            if (command != null)
            {
                // the property is attached so we attach the Drop event handler
                framworkElement.Loaded += OnFrameworkElement_Loaded;
            }
        }

       
        private static void OnFrameworkElement_Loaded(object sender, RoutedEventArgs e)
        {
            FrameworkElement frameworkElement = sender as FrameworkElement;

           
            if (frameworkElement == null)
                return;

            ICommand loadLayoutCommand = DockLayoutSerializer.GetLoadLayoutCommand(frameworkElement);

           
            if (loadLayoutCommand == null)
                return;

          
            if (loadLayoutCommand is RoutedCommand)
            {
               
                (loadLayoutCommand as RoutedCommand).Execute(frameworkElement, frameworkElement);
            }
            else
            {
               
                loadLayoutCommand.Execute(frameworkElement);
            }
        }
        #endregion Load Layout

        #region Save Layout
      
        public static ICommand GetSaveLayoutCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(SaveLayoutCommandProperty);
        }

     
        public static void SetSaveLayoutCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(SaveLayoutCommandProperty, value);
        }

      
        private static void OnSaveLayoutCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement framworkElement = d as FrameworkElement;     // Remove the handler if it exist to avoid memory leaks
            framworkElement.Unloaded -= OnFrameworkElement_Saveed;

            var command = e.NewValue as ICommand;
            if (command != null)
            {
                // the property is attached so we attach the Drop event handler
                framworkElement.Unloaded += OnFrameworkElement_Saveed;
            }
        }

     
        private static void OnFrameworkElement_Saveed(object sender, RoutedEventArgs e)
        {
            DockingManager frameworkElement = sender as DockingManager;

           
            if (frameworkElement == null)
                return;

            ICommand SaveLayoutCommand = DockLayoutSerializer.GetSaveLayoutCommand(frameworkElement);

          
            if (SaveLayoutCommand == null)
                return;

            string xmlLayoutString = string.Empty;

            using (StringWriter fs = new StringWriter())
            {
                XmlLayoutSerializer xmlLayout = new XmlLayoutSerializer(frameworkElement);

                xmlLayout.Serialize(fs);

                xmlLayoutString = fs.ToString();
            }

          
            if (SaveLayoutCommand is RoutedCommand)
            {
            
                (SaveLayoutCommand as RoutedCommand).Execute(xmlLayoutString, frameworkElement);
            }
            else
            {
               
                SaveLayoutCommand.Execute(xmlLayoutString);
            }
        }
        #endregion Save Layout
        #endregion methods
    }
}
