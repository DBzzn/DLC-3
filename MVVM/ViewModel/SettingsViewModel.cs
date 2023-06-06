using DLC_3.Core;
using DLC_3.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLC_3.MVVM.ViewModel
{
    public class SettingsViewModel : Core.ViewModel
    {
        private INavigationService _navigation;

        public INavigationService Navigation
        {
            get { return _navigation; }
            set
            {
                _navigation = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand NavigateToCViewCommand { get; set; }
        public SettingsViewModel(INavigationService navigation)
        {
            Navigation = navigation;
            NavigateToCViewCommand = new RelayCommand(execute: o => { Navigation.NavigationTo<CViewModel>(); }, o => true);

        }
    }
}
