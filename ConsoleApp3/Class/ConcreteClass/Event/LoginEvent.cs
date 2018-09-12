using ConsoleApp3.Class.AbstractClass;
using System.Data;
using System.Data.SqlClient;
using System;

namespace ConsoleApp3.Class.ConcreteClass.Event
{
    class LoginEvent : EventData
    {
        public LoginEvent()
        {
            comand.CommandText = @"INSERT INTO log (Type,Time,UserName,PcName) VALUES (@Type,@Time,@UserName,@PcName)";

            comand.Parameters.Add("@Type", SqlDbType.TinyInt);
            comand.Parameters.Add("@Time", SqlDbType.DateTime);
            comand.Parameters.Add("@UserName", SqlDbType.NVarChar, 255);
            comand.Parameters.Add("@PcName", SqlDbType.NVarChar, 255);
        }
        public override void InIt()
        {
            Console.WriteLine("EventLogin Вошли");
        }

        public override void Save()
        {
            comand.Parameters["@Type"].Value = events.Login;
            comand.Parameters["@Time"].Value = DateTime.Now;
            comand.Parameters["@UserName"].Value = Environment.UserName;
            comand.Parameters["@PcName"].Value = Environment.MachineName;

            comand.ExecuteNonQuery();

            Console.WriteLine("EventLogin Сохранено!!!");

        }
    }
}
