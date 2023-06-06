using DLC_3.NET.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DLC_3.NET
{
    class Server
    {
        public TcpClient _client;
        public PacketReader PacketReader;
        public string[] cfg = { "Username", "192.168.15.133", "9773" };

        //Eventos

        public event Action connectedEvent;
        public event Action msgReceivedEvent;
        public event Action userDisconnectedEvent;

        public Server()
        {
            _client = new TcpClient();
        }

        public void connectToServer()
        {
            if(!_client.Connected) 
            {
                try
                {
                    _client.Connect(cfg[1], int.Parse(cfg[2]));
                    PacketReader = new PacketReader(_client.GetStream());

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
                            default: break;
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
