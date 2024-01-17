using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace IDZ_CLIENT.ViewModels
{
    class ServerConnector
    {
        private readonly IPAddress localAddress = IPAddress.Parse(ConfigurationManager.AppSettings.Get("LocalAddress"));
        private readonly int localPort = int.Parse(ConfigurationManager.AppSettings.Get("LocalPort"));
        private readonly int remotePortRead = int.Parse(ConfigurationManager.AppSettings.Get("RemotePortRead"));
        private readonly int remotePortEdit = int.Parse(ConfigurationManager.AppSettings.Get("RemotePortEdit"));
        private readonly int remotePortDelete = int.Parse(ConfigurationManager.AppSettings.Get("RemotePortDelete"));
        private readonly int remotePortUpdate = int.Parse(ConfigurationManager.AppSettings.Get("RemotePortUpdate"));
        private readonly int remotePortReport = int.Parse(ConfigurationManager.AppSettings.Get("RemotePortReport"));
        public string GetDataForReport()
        {
            SendMessage(remotePortReport);
            string answer = ReceiveDataFromServer();
            return answer;
        }

        public string GetDataFromServer()
        {
            SendMessage(remotePortRead);
            string answer = ReceiveDataFromServer();
            return answer;
        }
        public void SendDeleteData(string data)
        {
            SendMessage(remotePortDelete, data);
        }
        public void SendEditData(string data)
        {
            SendMessage(remotePortEdit, data);
        }
        public string UpdateDate()
        {
            SendMessage(remotePortUpdate);
            string answer = ReceiveDataFromServer();
            return answer;
        }
        private string ReceiveDataFromServer() // Вывод данных
        {
            using (UdpClient receiver = new UdpClient(localPort))
            {
                // получаем данные
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(localAddress, localPort);
                var result = receiver.Receive(ref RemoteIpEndPoint);
                return Encoding.UTF8.GetString(result);
            }
        }
        private void SendMessage(int remotePort, string message = "")
        {
            using (UdpClient sender = new UdpClient())
            {
                byte[] data = Encoding.UTF8.GetBytes(message);
                // и отправляем на 127.0.0.1:remotePort
                sender.Send(data, data.Length, new IPEndPoint(localAddress, remotePort));
            }
        }
    }
}
