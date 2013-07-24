﻿using System;
using System.Linq;
using System.Text;
using SilverGame.Services;
using SilverSock;

namespace SilverGame.Network.Abstract
{
    abstract class Client
    {
        public SilverSocket Socket;

        protected Client(SilverSocket socket)
        {
            Socket = socket;
            {
                socket.OnConnected += OnConnected;
                socket.OnDataArrivalEvent += DataArrival;
                socket.OnFailedToConnect += OnFailedToConnect;
                socket.OnSocketClosedEvent += OnSocketClosed;
            }
        }

        #region abstracts

        protected abstract void OnConnected();
        protected abstract void OnFailedToConnect(Exception e);
        public abstract void OnSocketClosed();
        protected abstract void DataReceived(string packet);

        #endregion

        public virtual void SendPackets(string packet)
        {
            SilverConsole.WriteLine(string.Format("send >>" + string.Format("{0}\x00", packet)), ConsoleColor.Cyan);
            Socket.Send(Encoding.UTF8.GetBytes(string.Format("{0}\x00", packet)));
        }

        private void DataArrival(byte[] data)
        {
            foreach (var packet in Encoding.UTF8.GetString(data).Replace("\x0a", "").Split('\x00').Where(x => x != ""))
            {
                SilverConsole.WriteLine(string.Format("Recv <<" + packet), ConsoleColor.Green);
                DataReceived(packet);
            }
        }
    }
}
