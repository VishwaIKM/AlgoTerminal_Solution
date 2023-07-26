using AlgoTerminal.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AlgoTerminal.ViewModel
{
    public class DockManagerViewModel
    {
        /// <summary>Gets a collection of all visible documents</summary>
        public ObservableCollection<DockWindowViewModel> Documents { get; private set; }

        public ObservableCollection<object> Anchorables { get; private set; }

        public DockManagerViewModel(IEnumerable<DockWindowViewModel> dockWindowViewModels)
        {
            this.Documents = new ObservableCollection<DockWindowViewModel>();
            this.Anchorables = new ObservableCollection<object>();

            foreach (var document in dockWindowViewModels)
            {
                document.PropertyChanged += DockWindowViewModel_PropertyChanged;
                if (!document.IsClosed)
                    this.Documents.Add(document);
            }
        }

        private void DockWindowViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DockWindowViewModel document = sender as DockWindowViewModel;

            if (e.PropertyName == nameof(DockWindowViewModel.IsClosed))
            {
                if (!document.IsClosed)
                    this.Documents.Add(document);
                else
                    this.Documents.Remove(document);
            }
        }
        private DockLayoutViewModel mAVLayout = null;
        public DockLayoutViewModel ADLayout
        {
            get
            {
                if (this.mAVLayout == null)
                    this.mAVLayout = new DockLayoutViewModel();

                return this.mAVLayout;
            }
        }
        public static string LayoutFileName
        {
            get
            {
                return "Layout.config";
            }
        }
        public static string DirAppData
        {
            get
            {
                string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                                 System.IO.Path.DirectorySeparatorChar + Company;

                try
                {
                    if (System.IO.Directory.Exists(dirPath) == false)
                        System.IO.Directory.CreateDirectory(dirPath);
                }
                catch
                {
                }

                return dirPath;
            }
        }

        public static string Company
        {
            get
            {
                return "Vishwa";
            }
        }
    }
}
