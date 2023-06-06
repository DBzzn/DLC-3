using DLC_3.Core;
using DLC_3.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLC_3.MVVM.ViewModel
{
    public class MainViewModel : Core.ViewModel
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

        public RelayCommand NavigateCCommand { get; set; }
        public RelayCommand NavigateSettingsCommand { get; set; }
        public MainViewModel(INavigationService navService)
        {
            Navigation = navService;
            NavigateCCommand = new RelayCommand(execute: o => { Navigation.NavigationTo<CViewModel>(); }, canExecute: o => true);
            NavigateSettingsCommand = new RelayCommand(execute: o => { Navigation.NavigationTo<SettingsViewModel>(); }, canExecute: o => true);
            navService.NavigationTo<CViewModel>();
        }
    }
}
