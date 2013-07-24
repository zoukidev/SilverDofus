﻿using System;
using System.Collections.Generic;
using SilverRealm.Services;
using SilverRealm.Network.Abstract;
using SilverSock;

namespace SilverRealm.Network.Realm
{
    sealed class RealmServer : Server
    {
        public static List<RealmClient> Clients;
        public static Object Lock = new Object();

        public RealmServer()
            : base(Services.Config.Get("Realm_ip"), Int32.Parse(Services.Config.Get("Realm_port")))
        {
            Clients = new List<RealmClient>();
        }

        protected override void OnListening()
        {
            Console.WriteLine("Waiting for new Connexions");

            Logs.LogWritter(Constant.RealmFolder, "RealmServer Waiting for connexion ");
        }

        protected override void OnListeningFailed(Exception e)
        {
            SilverConsole.WriteLine("Realm : Listening Failed : " + e.Message, ConsoleColor.Red);

            Logs.LogWritter(Constant.RealmFolder, string.Format("RealmServer Listening Failed {0}", e.Message));
        }

        protected override void OnSocketAccepted(SilverSocket socket)
        {
            SilverConsole.WriteLine("Realm : New connection established", ConsoleColor.Green);

            Logs.LogWritter(Constant.RealmFolder, string.Format("RealmServer Connection successfuly with Realm Client {0}", socket.IP));

            lock (Lock)
                Clients.Add(new RealmClient(socket));
        }
    }
}
