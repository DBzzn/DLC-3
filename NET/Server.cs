using DLC_3.MVVM.Model;
using DLC_3.NET.IO;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DLC_3.NET
{
    public class Server
    {
        public TcpClient _client; 
        public PacketReader PacketReader;
        public string[] cfg = new string[3];

        public ObservableCollection<UserModel> SUsers { get; set; }

        private xmlS xfil = new xmlS();
        public bool con = false; 
        //Eventos

        public event Action connectedEvent;
        public event Action msgReceivedEvent;
        public event Action userDisconnectedEvent;




        public Server()
        {            
            _client = new TcpClient();
            cfg = xfil.readXML();
            SUsers = new ObservableCollection<UserModel>();

        }

        public void setCFG(string[] c)
        {
            if(xfil.chkA(c.ToString()))
            {                
                xfil.save(c);
                cfg = xfil.readXML();
            }
        }

        public void connectToServer()
        {
            if(!_client.Connected) 
            {                
                try
                {
                    
                    _client.Connect(cfg[1], int.Parse(cfg[2]));
                    PacketReader = new PacketReader(_client.GetStream());
                    con = _client.Client.Connected;
                    if (!string.IsNullOrEmpty(cfg[0]))
                    {
                        var connectPacket = new PacketBuilder();
                        //envio das informacoes do user para o serv
                        connectPacket.WriteOpCode(0);
                        connectPacket.WriteMessage(cfg[0]);
                        _client.Client.Send(connectPacket.GetPacketBytes());

                    }

                    ReadPackets();
                }
                catch
                {
                    con = _client.Client.Connected;
                    MessageBox.Show($"Não foi possível conectar ao servidor!\n SETUP => " +
                        $"{cfg[1]}:{cfg[2]}//{cfg[0]}", "ERRO", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
        }

        private void ReadPackets()
        {
            Task.Run(() =>
                {
                    while(true)
                    {
                        var opcode = PacketReader.ReadByte();
                        switch (opcode)
                        {
                            case 1:
                                connectedEvent?.Invoke();
                                break;

                            case 5:
                                msgReceivedEvent?.Invoke();
                                break;

                            case 10:
                                userDisconnectedEvent?.Invoke();
                                break;

                            default:
                                Console.WriteLine("AHHH ééééé");
                                break;
                        }
                    }
                });

        }


        public void SendMessageToServer(string msg)
        
        {

            try
            {
                var messagePacket = new PacketBuilder();
                messagePacket.WriteOpCode(5);
                messagePacket.WriteMessage(msg);
                _client.Client.Send(messagePacket.GetPacketBytes());

            }
            catch 
            {
                MessageBox.Show("Falha na conexão com o servidor", "ERRO!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

    }
}
