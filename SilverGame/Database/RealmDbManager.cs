﻿using System;
using MySql.Data.MySqlClient;
using SilverGame.Services;

namespace SilverGame.Database
{
    class RealmDbManager
    {
        public static MySqlConnection Connection;
        public static Object Lock = new Object();

        public RealmDbManager()
        {
            Connection = new MySqlConnection(string.Format("server={0};uid={1};pwd={2};database={3}",
                                        Config.Get("Realm_Database_Host"),
                                        Config.Get("Realm_Database_Username"),
                                        Config.Get("Realm_Database_Password"),
                                        Config.Get("Realm_Database_Name")));
            try
            {
                SilverConsole.WriteLine("Connection to Realm...");

                Connection.Open();

                SilverConsole.WriteLine("SQL : Connection to Realm database successfully", ConsoleColor.Green);
            }
            catch (Exception e)
            {
                SilverConsole.WriteLine("SQL Error : " + e.Message, ConsoleColor.Red);
                Logs.LogWritter(Constant.ErrorsFolder, "Realm connection database SQL error : " + e.Message);
            }
        }
    }
}
