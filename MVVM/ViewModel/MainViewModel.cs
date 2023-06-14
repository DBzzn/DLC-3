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
using System.Windows.Navigation;
using System.Windows.Input;
using System.Windows.Media;

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
        private string _msg;
        private string _Username;
        public string Username
        {
            get { return _Username; }
            set
            {
                _Username = value;
                OnPropertyChanged(nameof(Username));
            }
        }
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





        public event Action changeUsername;

        public MainViewModel(INavigationService navService)
        {
            Navigation = navService;
            Server _server = new Server();
            NavigateCCommand = new RelayCommand(execute: o => { Navigation.NavigationTo<CViewModel>(); }, canExecute: o => true);
            NavigateSettingsCommand = new RelayCommand(execute: o => { Navigation.NavigationTo<SettingsViewModel>(); }, canExecute: o => true);
            
            Users = _server.SUsers;


            navService.NavigationTo<CViewModel>();
            Messages = new ObservableCollection<MessageModel>();            
            listMessages = new ObservableCollection<string>();
            Users = new ObservableCollection<UserModel>(); //bo???
            listMessages = new ObservableCollection<string>();

            changeUsername += _changeUsername;
            _server.connectedEvent += UserConnected;
            _server.msgReceivedEvent += MessageReceived;
            _server.userDisconnectedEvent += RemoveUser;


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


            Task.Run(() =>
            {
                while (true)
                {
                    codeCommandList.Add(1);
                    switch (codeCommandList.First())
                    {
                        case 1:
                            changeUsername?.Invoke();
                            codeCommandList.RemoveAt(0);
                            break;
                        
                        default:
                            break;
                    }
                }
            });





        }

        private void _changeUsername()
        {
            _Username = _server.cfg[0];            
        }



        public void RemoveUser()
        {
            var uid = _server.PacketReader.ReadMessage();
            var user = _server.SUsers.Where(x => x.UID == uid).FirstOrDefault();
            System.Windows.Application.Current.Dispatcher.Invoke(() => _server.SUsers.Remove(user));
            Users = _server.SUsers;

        }

        public void MessageReceived()
        {
            var msg = _server.PacketReader.ReadMessage();
            System.Windows.Application.Current.Dispatcher.Invoke(() => listMessages.Add(msg));
        }

        public void UserConnected()
        {
            var user = new UserModel
            {
                Username = _server.PacketReader.ReadMessage(),
                UID = _server.PacketReader.ReadMessage(),
                ImageSource = "C:\\Users\\User\\source\\repos\\DLChat\\Icons\\hon.png",
                Messages = Messages
            };


            if (!_server.SUsers.Any(x => x.UID == user.UID))
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() => _server.SUsers.Add(user));
            }
            Users = _server.SUsers;
        }
    }
}
