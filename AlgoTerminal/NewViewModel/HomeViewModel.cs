using AlgoTerminal.ViewModel;
using OpenControls.Wpf.DockManager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTerminal.NewViewModel
{
    public class HomeViewModel :BaseViewModel
    {
        public HomeViewModel()
        {
            Tools = new ObservableCollection<IViewModel>();
            Tools.Add(new LoggerUcViewModel());

            LayoutLoaded = false;
        }
        private System.Collections.ObjectModel.ObservableCollection<IViewModel> _documents;
        public System.Collections.ObjectModel.ObservableCollection<IViewModel> Documents
        {
            get
            {
                return _documents;
            }
            set
            {
                if (value != Documents)
                {
                    _documents = value;
                    RaisePropertyChanged("Documents");
                }
            }
        }

        private System.Collections.ObjectModel.ObservableCollection<IViewModel> _tools;
        public System.Collections.ObjectModel.ObservableCollection<IViewModel> Tools
        {
            get
            {
                return _tools;
            }
            set
            {
                if (value != Tools)
                {
                    _tools = value;
                    RaisePropertyChanged("Tools");
                }
            }
        }

        private bool _layoutLoaded;
        public bool LayoutLoaded
        {
            get
            {
                return _layoutLoaded;
            }
            set
            {
                _layoutLoaded = value;
                RaisePropertyChanged("LayoutLoaded");
            }
        }

        public bool IsToolVisible(Type type)
        {
            return (Tools.Where(n => n.GetType() == type).Count() > 0);
        }

        public void ShowTool(bool show, Type type)
        {
            bool isVisible = IsToolVisible(type);
            if (isVisible == show)
            {
                return;
            }

            if (show == false)
            {
                var enumerator = Tools.Where(n => n.GetType() == type);
                Tools.Remove(enumerator.First());
            }
            else
            {
                IViewModel iViewModel = (IViewModel)Activator.CreateInstance(type);
                System.Diagnostics.Trace.Assert(iViewModel != null);
                Tools.Add(iViewModel);
            }
        }
        public bool LoggerUcViewModelVisible
        {
            get
            {
                return (Tools.Where(n => n.GetType() == typeof(LoggerUcViewModel)).Count() > 0);
            }
            set
            {
                ShowTool(value, typeof(LoggerUcViewModel));
                RaisePropertyChanged("LoggerUcViewModelVisible");
            }
        }
    }
}
