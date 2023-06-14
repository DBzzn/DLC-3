using DLC_3.Core;
using DLC_3.MVVM.Model;
using DLC_3.NET;
using DLC_3.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Interop;

namespace DLC_3.MVVM.ViewModel
{
    public class CViewModel: Core.ViewModel
    {
        private INavigationService _navigation;

        public INavigationService Navigation
        {
            get => _navigation;
            set
            {
                _navigation = value;
                OnPropertyChanged();
            }
        }

        

        private string _msg;
        public string Message
        {
            get { return _msg; }
            set
            {
                _msg = value;
                OnPropertyChanged();
            }
        }



        public RelayCommand NavigateToSettingsViewCommand { get; set; }
        



        public CViewModel(INavigationService navigation)
        {
            Navigation = navigation;            
            NavigateToSettingsViewCommand = new RelayCommand(execute: o => { Navigation.NavigationTo<SettingsViewModel>(); }, canExecute: o => true);












        }


    }
}
