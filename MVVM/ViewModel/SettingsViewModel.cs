using DLC_3.Core;
using DLC_3.NET;
using DLC_3.Services;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;

namespace DLC_3.MVVM.ViewModel
{
    public class SettingsViewModel : Core.ViewModel
    {
        private INavigationService _navigation;
        internal xmlS sFile = new xmlS();
        public string[] scfg { get; set; } = new string[3];
        
        
        public INavigationService Navigation
        {
            get => _navigation;
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
            Server _server = MainViewModel._server;
            scfg = sFile.readXML();

            NavigateToCViewCommand = new RelayCommand(o => {
                                    
                _server.setCFG(scfg);
                
                if (!string.IsNullOrEmpty(_server.cfg.ToString()))
                {
                    _server.connectToServer();
                    System.Windows.Forms.MessageBox.Show($"Login com Sucesso!\n SETUP => {_server.cfg[1]}:{_server.cfg[2]}//{_server.cfg[0]}");

                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Não conectado!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                codeCommandList.Add(1); // invoke da alteracao de set
                Navigation.NavigationTo<MainViewModel>();
            }, o => true);
        }
    }
}

