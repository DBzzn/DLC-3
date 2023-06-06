using DLC_3.Core;
using DLC_3.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public RelayCommand NavigateToSettingsViewCommand { get; set; }

        public CViewModel(INavigationService navigation)
        {
            Navigation = navigation;
            NavigateToSettingsViewCommand = new RelayCommand(o => { Navigation.NavigationTo<SettingsViewModel>(); }, o => true);
        }
    }
}
