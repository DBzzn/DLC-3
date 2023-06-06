using DLC_3.Core;
using DLC_3.MVVM.Model;
using DLC_3.NET;
using DLC_3.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Remoting.Proxies;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Navigation;

namespace DLC_3.MVVM.ViewModel
{



    public class MainViewModel : Core.ViewModel
    {
        //nav
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

        //msgs
        public ObservableCollection<UserModel> Users { get; set; }
        public ObservableCollection<MessageModel> Messages { get; set; }
        public ObservableCollection<string> listMessages { get; set; }

        //var dec
        public string[] configs { get; set; } = { "Usuário", "192.168.15.133", "9773" };
        private Server _server;
        private xml sFile;
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

        private UserModel _selectedContact;
        public UserModel SelectedContact
        {
            get { return _selectedContact; }
            set
            {
                _selectedContact = value;
                OnPropertyChanged();
            }
        }




        //commands
        public RelayCommand NavigateCCommand { get; set; }
        public RelayCommand NavigateSettingsCommand { get; set; }


        public RelayCommand connectToServerCommand { get; set; }
        public RelayCommand SendMessageCommand { get; set; }
        public RelayCommand sConfig { get; set; }


        public MainViewModel(INavigationService navService)
        {
            Navigation = navService;
            NavigateCCommand = new RelayCommand(execute: o => { Navigation.NavigationTo<CViewModel>(); }, canExecute: o => true);
            NavigateSettingsCommand = new RelayCommand(execute: o => { Navigation.NavigationTo<SettingsViewModel>(); }, canExecute: o => true);
            navService.NavigationTo<CViewModel>();


            Messages = new ObservableCollection<MessageModel>();
            Users = new ObservableCollection<UserModel>();
            listMessages = new ObservableCollection<string>();
            _server = new Server();

            _server.connectedEvent += UserConnected;
            _server.msgReceivedEvent += MessageReceived;
            _server.userDisconnectedEvent += RemoveUser;


            sConfig = new RelayCommand(o =>
            {
                sFile = new xml();
                sFile.save(configs);
            }, o => true);


            connectToServerCommand = new RelayCommand(o =>
            {

                if (!string.IsNullOrEmpty(configs[0]) && !string.IsNullOrEmpty(_server.cfg[0]))
                {
                    _server.cfg = configs;
                    _server.connectToServer();
                    System.Windows.Forms.MessageBox.Show($"Login com Sucesso!\n SETUP => {configs[1]}:{configs[2]}//{configs[0]}");
                    
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Não conectado!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }, o=> true);

            SendMessageCommand = new RelayCommand(o =>
            {
                _server.SendMessageToServer(Message);
                Messages.Add(new MessageModel
                {
                    Message = Message,
                    FirstMessage = false
                });
                Message = "";
            }, o => !string.IsNullOrEmpty(Message));


        }


        private void RemoveUser()
            {
                var uid = _server.PacketReader.ReadMessage();
                var user = Users.Where(x => x.UID == uid).FirstOrDefault();
                System.Windows.Application.Current.Dispatcher.Invoke(() => Users.Remove(user));
                
            }

            private void MessageReceived()
            {
                var msg = _server.PacketReader.ReadMessage();
                System.Windows.Application.Current.Dispatcher.Invoke(() => listMessages.Add(msg));
            }

            private void UserConnected()
            {
            var user = new UserModel
            {
                Username = _server.PacketReader.ReadMessage(),
                UID = _server.PacketReader.ReadMessage(),
                ImageSource = "C:\\Users\\User\\source\\repos\\DLChat\\Icons\\hon.png",
                Messages = Messages
                };


                if (!Users.Any(x => x.UID == user.UID))
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(() => Users.Add(user));
                }
            
        }
    }
}
