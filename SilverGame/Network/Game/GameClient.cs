﻿using System;
using System.Linq;
using System.Text;
using SilverGame.Models.Accounts;
using SilverGame.Models.Characters;
using SilverGame.Services;
using SilverSock;

namespace SilverGame.Network.Game
{
    sealed class GameClient
    {
        public readonly SilverSocket Socket;

        private readonly GameParser.GameParser _parser;
        public Account Account;
        public Character Character;

        public GameClient(SilverSocket socket)
        {
            Socket = socket;
            {
                Socket.OnDataArrivalEvent += DataArrival;
                Socket.OnSocketClosedEvent += OnSocketClosed;
            }

            _parser = new GameParser.GameParser(this);
        }

        public void OnSocketClosed()
        {
            SilverConsole.WriteLine("Connection closed", ConsoleColor.Yellow);

            Logs.LogWritter(Constant.GameFolder, Account != null
                ? string.Format("{0}:ip {1} Connection Closed", Account.Username, Socket.IP)
                : string.Format("ip {0} Connection Closed", Socket.IP));

            if (Account != null)
                Account.Disconnect();

            if(Character != null)
                Character.Disconnect();

            RemoveMeOnList();
        }

        private void RemoveMeOnList()
        {
            lock (GameServer.Lock)
                GameServer.Clients.Remove(this);
        }

        public void SendPackets(string packet)
        {
            SilverConsole.WriteLine(string.Format("send >>" + string.Format("{0}\x00", packet)), ConsoleColor.Cyan);
            Socket.Send(Encoding.UTF8.GetBytes(string.Format("{0}\x00", packet)));

            Logs.LogWritter(Constant.GameFolder, Account != null
                ? string.Format("{0}:ip {1} Send >> {2}", Account.Username, Socket.IP, packet)
                : string.Format("ip {0} Send >> {1}", Socket.IP, packet));
        }

        private void DataArrival(byte[] data)
        {
            foreach (var packet in Encoding.UTF8.GetString(data).Replace("\x0a", "").Split('\x00').Where(x => x != ""))
            {
                SilverConsole.WriteLine(string.Format("Recv <<" + packet), ConsoleColor.Green);

                Logs.LogWritter(Constant.GameFolder, Account != null
                ? string.Format("{0}:ip {1} Recv << {2}", Account.Username, Socket.IP, packet)
                : string.Format("ip {0} Recv << {1}", Socket.IP, packet));

                _parser.Parse(packet);
            }
        }
    }
}