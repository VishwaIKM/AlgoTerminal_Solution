using AlgoTerminal.Command;
using System.Windows.Input;

namespace AlgoTerminal.ViewModel
{
    public class DockWindowViewModel : BaseViewModel
    {
        #region Properties

        #region CloseCommand
        private ICommand _CloseCommand;
        public ICommand CloseCommand
        {
            get
            {
                if (_CloseCommand == null)
                    _CloseCommand = new RelayCommand(call => Close());
                return _CloseCommand;
            }
        }
        #endregion

        #region IsClosed
        private bool _IsClosed;
        public bool IsClosed
        {
            get { return _IsClosed; }
            set
            {
                if (_IsClosed != value)
                {
                    _IsClosed = value;
                    OnPropertyChanged(nameof(IsClosed));
                }
            }
        }
        #endregion

        #region CanClose
        private bool _CanClose;
        public bool CanClose
        {
            get { return _CanClose; }
            set
            {
                if (_CanClose != value)
                {
                    _CanClose = value;
                    OnPropertyChanged(nameof(CanClose));
                }
            }
        }
        #endregion

        #region Title
        private string _Title;
        public string Title
        {
            get { return _Title; }
            set
            {
                if (_Title != value)
                {
                    _Title = value;
                    OnPropertyChanged(nameof(Title));
                }
            }
        }
        #endregion

        #endregion

        public DockWindowViewModel()
        {
            this.CanClose = true;
            this.IsClosed = false;
        }

        public void Close()
        {
            this.IsClosed = true;
        }
        #region ContentId

        private string _contentId = null;
        public string ContentId
        {
            get { return _contentId; }
            set
            {
                if (_contentId != value)
                {
                    _contentId = value;
                    RaisePropertyChanged("ContentId");
                }
            }
        }

        #endregion

        #region IsSelected

        private bool _isSelected = false;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    RaisePropertyChanged("IsSelected");
                }
            }
        }

        #endregion

        #region IsActive

        private bool _isActive = false;
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    RaisePropertyChanged("IsActive");
                }
            }
        }

        #endregion
    }
}
