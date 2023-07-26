using AlgoTerminal.Command;
using AvalonDock;
using AvalonDock.Layout.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AlgoTerminal.ViewModel
{
    public class DockLayoutViewModel
    {
        #region fields
        private RelayCommand mLoadLayoutCommand = null;
        private RelayCommand mSaveLayoutCommand = null;
        #endregion fields

        #region command properties
        /// <summary>
        /// Implement a command to load the layout of an AvalonDock-DockingManager instance.
        /// This layout defines the position and shape of each document and tool window
        /// displayed in the application.
        /// 
        /// Parameter:
        /// The command expects a reference to a <seealso cref="DockingManager"/> instance to
        /// work correctly. Not supplying that reference results in not loading a layout (silent return).
        /// </summary>
        public ICommand LoadLayoutCommand
        {
            get
            {
                if (this.mLoadLayoutCommand == null)
                {
                    this.mLoadLayoutCommand = new RelayCommand((p) =>
                    {
                        DockingManager docManager = p as DockingManager;

                        if (docManager == null)
                            return;

                        this.LoadDockingManagerLayout(docManager);
                    });
                }

                return this.mLoadLayoutCommand;
            }
        }

        /// <summary>
        /// Implements a command to save the layout of an AvalonDock-DockingManager instance.
        /// This layout defines the position and shape of each document and tool window
        /// displayed in the application.
        /// 
        /// Parameter:
        /// The command expects a reference to a <seealso cref="string"/> instance to
        /// work correctly. The string is supposed to contain the XML layout persisted
        /// from the DockingManager instance. Not supplying that reference to the string
        /// results in not saving a layout (silent return).
        /// </summary>
        public ICommand SaveLayoutCommand
        {
            get
            {
                if (this.mSaveLayoutCommand == null)
                {
                    this.mSaveLayoutCommand = new RelayCommand((p) =>
                    {
                        string xmlLayout = p as string;

                        //string xmlLayoutString = string.Empty;

                        //using (StringWriter fs = new StringWriter())
                        //{
                        //    //XmlLayoutSerializer xmlLayout1 = new XmlLayoutSerializer();
                        //    //xmlLayout1.Serialize(fs);
                        //    //xmlLayoutString = fs.ToString();
                        //}
                        //xmlLayout = xmlLayoutString;
                        if (xmlLayout == null)
                            return;

                        this.SaveDockingManagerLayout(xmlLayout);
                    });
                }

                return this.mSaveLayoutCommand;
            }
        }
        #endregion command properties

        #region methods
        #region LoadLayout
        /// <summary>
        /// Loads the layout of a particular docking manager instance from persistence
        /// and checks whether a file should really be reloaded (some files may no longer
        /// be available).
        /// </summary>
        /// <param name="docManager"></param>
        private void LoadDockingManagerLayout(DockingManager docManager)
        {
            string layoutFileName = System.IO.Path.Combine(DockManagerViewModel.DirAppData, DockManagerViewModel.LayoutFileName);

            if (System.IO.File.Exists(layoutFileName) == false)
                return;

            var layoutSerializer = new XmlLayoutSerializer(docManager);

            layoutSerializer.LayoutSerializationCallback += (s, args) =>
            {
                // This can happen if the previous session was loading a file
                // but was unable to initialize the view ...
                if (args.Model.Title == null)
                {
                    args.Cancel = true;
                    return;
                }

              // DockLayoutViewModel.ReloadContentOnStartUp(args);
            };

            layoutSerializer.Deserialize(layoutFileName);
        }

       // private static void ReloadContentOnStartUp(LayoutSerializationCallbackEventArgs args)
        //{
        //    string sId = args.Model.Title;

           
        //    if (string.IsNullOrWhiteSpace(sId) == true)
        //    {
        //        args.Cancel = true;
        //        return;
        //    }

        //    if (args.Model.Title == FileStatsViewModel.Title)
        //        args.Content = Workspace.This.FileStats;
        //    else
        //    {
        //        args.Content = DockLayoutViewModel.ReloadDocument(args.Model.ContentId);

        //        if (args.Content == null)
        //            args.Cancel = true;
        //    }
        //}

        //private static object ReloadDocument(string path)
        //{
        //    object ret = null;

        //    if (!string.IsNullOrWhiteSpace(path))
        //    {
        //        switch (path)
        //        {
        //            /***
        //                      case StartPageViewModel.StartPageContentId: // Re-create start page content
        //                        if (Workspace.This.GetStartPage(false) == null)
        //                        {
        //                          ret = Workspace.This.GetStartPage(true);
        //                        }
        //                        break;
        //            ***/
        //            default:
                       
        //                ret = Workspace.This.Open(path);
        //                break;
        //        }
        //    }

        //    return ret;
        //}
        #endregion LoadLayout

        #region SaveLayout
        private void SaveDockingManagerLayout(string xmlLayout)
        {
            // Create XML Layout file on close application (for re-load on application re-start)
            if (xmlLayout == null)
                return;

            string fileName = System.IO.Path.Combine(DockManagerViewModel.DirAppData, DockManagerViewModel.LayoutFileName);

            File.WriteAllText(fileName, xmlLayout);
        }
        #endregion SaveLayout
        #endregion methods
    }
}
