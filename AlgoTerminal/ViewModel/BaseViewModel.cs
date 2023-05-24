using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTerminal.ViewModel
{
    public class BaseViewModel :INotifyPropertyChanged
    {
        #region Methods
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler? PropertyChanged;
        public bool CanThisMethodExecute() { return true; }
        #endregion
    }
}
