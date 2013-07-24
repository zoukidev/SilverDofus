﻿using System;
using MySql.Data.MySqlClient;
using SilverGame.Services;

namespace SilverGame.Database
{
    class GameDbManager
    {
        public static MySqlConnection Connection;
        public static Object Lock = new Object();

        public GameDbManager()
        {
            Connection = new MySqlConnection(string.Format("server={0};uid={1};pwd={2};database={3}",
                                        Config.Get("Game_Database_Host"),
                                        Config.Get("Game_Database_Username"),
                                        Config.Get("Game_Database_Password"),
                                        Config.Get("Game_Database_Name")));
            try
            {
                SilverConsole.WriteLine("Connection to Game...");

                Connection.Open();

                SilverConsole.WriteLine("SQL : Connection to Game database successfully", ConsoleColor.Green);
            }
            catch (Exception e)
            {
                SilverConsole.WriteLine("SQL Error : " + e.Message, ConsoleColor.Red);
                Logs.LogWritter(Constant.ErrorsFolder, "Game connection database SQL error : " + e.Message);
            }
        }
    }
}
